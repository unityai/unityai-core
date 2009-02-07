//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Partial Order Plan in a Partial Order 
//                Plan
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Planning
{
    [Serializable]
    public class PartialOrderPlan
    {
        #region Fields
        private List<Predicate> moInitialState;
        private List<Predicate> moGoalState;
        private Action moStartAction;
        private Action moFinishAction;
        private List<Action> moActions;
        private List<OrderingConstraint> moOrderingConstraints;
        private List<CausalLink> moCausalLinks;
        private List<ActionPredicatePair> moOpenPreconditions;
        #endregion

        #region Properties
        /// <summary>
        /// Do we have open precondtions?
        /// </summary>
        public bool HasOpenPreconditions
        {
            get { return moOpenPreconditions.Count > 0;  }
        }

        /// <summary>
        /// The Open Preconditions
        /// </summary>
        public List<ActionPredicatePair> OpenPreconditions
        {
            get { return moOpenPreconditions;  }
        }

        public List<Action> SortedActions
        {
            get { return SortAction(); }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Partial Order Plan
        /// </summary>
        /// <param name="initState">Init State</param>
        /// <param name="goalState">Goal State</param>
        public PartialOrderPlan(IEnumerable<Predicate> initState, IEnumerable<Predicate> goalState)
        {
            moInitialState = new List<Predicate>();
            moGoalState = new List<Predicate>();
            moActions = new List<Action>();
            moOrderingConstraints = new List<OrderingConstraint>();
            moOpenPreconditions = new List<ActionPredicatePair>();
            moCausalLinks = new List<CausalLink>();

            moStartAction = Action.CreateStart();
            moFinishAction = Action.CreateFinish();

            if (initState != null)
            {
                foreach (Predicate p in initState)
                {
                    moInitialState.Add(p);
                    moStartAction.AddEffect(p);
                }
            }
            foreach(Predicate p in goalState)
            {
                moGoalState.Add(p);
                moFinishAction.AddPrecondition(p);
            }

            foreach(Predicate pred in moFinishAction.Preconditions)
            {
                //if the start symbol has this effect we don't add it
                if (moStartAction.Effects.Exists(delegate(Predicate p)
                            {
                                //if they are the same including the negative
                                return p == pred && p.IsNegative == pred.IsNegative;
                            }) == false
                    )
                {
                    moOpenPreconditions.Add(new ActionPredicatePair(moFinishAction, pred));
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Randomly pick an open precondition
        /// </summary>
        /// <returns></returns>
        public ActionPredicatePair PickOpenPrecondition()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int i = rand.Next(0, moOpenPreconditions.Count);

            return moOpenPreconditions[i];
        }

        /// <summary>
        /// Add a Causal Link
        /// </summary>
        /// <param name="from">From Action</param>
        /// <param name="achieves">The Predicate that Achieves</param>
        /// <param name="to">To Action</param>
        public void AddCausalLink(Action from, Predicate achieves, Action to)
        {
            moCausalLinks.Add(new CausalLink(from, achieves, to));
        }

        /// <summary>
        /// Add an Ordering Constraint
        /// </summary>
        /// <param name="before">Before Action</param>
        /// <param name="after">After Action</param>
        public void AddOrderingConstraint(Action before, Action after)
        {
            //Will adding this constraint cause a loop?
            bool bLoopDetected = CheckLoop(before, after);

            if (bLoopDetected == false)
            {
                moOrderingConstraints.Add(new OrderingConstraint(before, after));
                moOrderingConstraints.Add(new OrderingConstraint(moStartAction, before));
                if (after.Identity != moFinishAction.Identity)
                    moOrderingConstraints.Add(new OrderingConstraint(before, moFinishAction));
            }
            else
            {
                string sException = string.Format("Loop detected ordering {0} {{ {1}", before.Identity.Name,
                                                  after.Identity.Name);
                throw new ConsistencyCheckException(sException);
            }
        }

        /// <summary>
        /// Check for a loop
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        private bool CheckLoop(Action before, Action after)
        {
            if (before == after)
                return true;

            List<OrderingConstraint> oList = moOrderingConstraints.FindAll(delegate(OrderingConstraint o)
                                                                                    {
                                                                                        return o.Before == after;
                                                                                    }
                                                                                    );

            //We are seeing if there is a condition like A->B B->C C->A
            foreach(OrderingConstraint oc in oList)
            {
                bool bCheck = CheckLoop(before, oc.After);
                if (bCheck == true)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Add an Action to the Plan
        /// </summary>
        /// <param name="action">Action to Add</param>
        public void AddAction(Action action)
        {
            moActions.Add(action);
            foreach(Predicate pred in action.Preconditions)
            {
                if (moStartAction.Effects.Contains(pred) == false)
                {
                    moOpenPreconditions.Add(new ActionPredicatePair(action, pred));
                }
                else
                {
                    this.AddCausalLink(moStartAction, pred, action);
                }
            }

            CheckPlanConsistent(action);
        }

        /// <summary>
        /// Remove the Action
        /// </summary>
        /// <param name="action">Action to Remove</param>
        /// <param name="actionPredicatePair">Action-Predicate Pair</param>
        public void RemoveAction(Action action, ActionPredicatePair actionPredicatePair)
        {
            moActions.Remove(action);
            moCausalLinks.RemoveAll(delegate(CausalLink c)
                                        {
                                            return c.From == action && c.Achieves == actionPredicatePair.Predicate &&
                                                   c.To == actionPredicatePair.Action;
                                        });
            moOrderingConstraints.RemoveAll(delegate(OrderingConstraint o)
                                                {
                                                    return o.Before == action && o.After == actionPredicatePair.Action;
                                                });
            //TODO: Clean up the Start and Finish ones as well
        }

        /// <summary>
        /// Check the Plan for Consistency
        /// </summary>
        /// <param name="action"></param>
        private void CheckPlanConsistent(Action action)
        {
            //check for conflicts in the Causal Links
            foreach (CausalLink c in moCausalLinks)
            {
                if (c.To.Identity != action.Identity)
                {
                    //adding a negative predicate that cancels out a Causal Link
                    //causes a conflict we can add an ordering constraint either B { C or C { A
                    bool bConflict = action.Effects.Exists(delegate(Predicate p)
                            {
                                return p.IsComplimentOf(c.Achieves);
                            });
                    if (bConflict == true)
                    {
                        //if no loop would occur by adding B { C then add it
                        if (c.To != moFinishAction && CheckLoop(c.To, action) == false)
                        {
                            AddOrderingConstraint(c.To, action); 
                        }
                        else if (c.From != moStartAction) //we have no choice then to try adding C { A
                        {
                            AddOrderingConstraint(action, c.From);
                        }
                        else
                        {
                            string sException = string.Format("{0} {{ {1} and {1} {{ {2} invalid.", c.To.Identity.Name,
                                          action.Identity.Name, c.From.Identity.Name);
                            throw new ConsistencyCheckException(sException);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// TODO: Just temporary sort action method 
        /// </summary>
        /// <returns></returns>
        private List<Action> SortAction()
        {
            List<Action> oSorted = new List<Action>();
            foreach(Action action in moActions)
            {
                //find an orderconstraint
                OrderingConstraint oc =
                    moOrderingConstraints.Find(
                        delegate(OrderingConstraint o)
                            {
                                return o.Before.Identity == action.Identity ||
                                       o.After.Identity == action.Identity;
                            });
                if (oc == null)
                {
                    oSorted.Add(action);
                }
                else
                {
                    //this action must occur before oc.After
                    if (oc.Before.Identity == action.Identity)
                    {
                        int iIndex = oSorted.FindIndex(delegate(Action a) { return a.Identity == oc.After.Identity; });
                        if (iIndex > -1)
                            oSorted.Insert(iIndex,action);
                        else
                            oSorted.Add(action);
                    }
                    else
                    {
                        int iIndex = oSorted.FindIndex(delegate(Action a) { return a.Identity == oc.Before.Identity; });
                        if (iIndex > -1)
                            oSorted.Insert(iIndex+1,action);
                        else
                            oSorted.Add(action);
                    }
                }
            }
            return oSorted;
        }
        #endregion
    }
}
