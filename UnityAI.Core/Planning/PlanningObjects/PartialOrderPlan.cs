﻿//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Partial Order Plan in a Partial Order 
//                Plan
//
// Modification Notes:
// Date		Author        	Notes
// -------- ------          -----------------------------------------
// 01/26/09	SMcCarthy		Initial Implementation
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Planning
{
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
        public PartialOrderPlan(IEnumerable<Predicate> voInitState, IEnumerable<Predicate> voGoalState)
        {
            moInitialState = new List<Predicate>();
            moGoalState = new List<Predicate>();
            moActions = new List<Action>();
            moOrderingConstraints = new List<OrderingConstraint>();
            moOpenPreconditions = new List<ActionPredicatePair>();
            moCausalLinks = new List<CausalLink>();

            moStartAction = Action.CreateStart();
            moFinishAction = Action.CreateFinish();

            if (voInitState != null)
            {
                foreach (Predicate p in voInitState)
                {
                    moInitialState.Add(p);
                    moStartAction.AddEffect(p);
                }
            }
            foreach(Predicate p in voGoalState)
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
        /// <param name="voFrom">From Action</param>
        /// <param name="voAchieves">The Predicate that Achieves</param>
        /// <param name="voTo">To Action</param>
        public void AddCausalLink(Action voFrom, Predicate voAchieves, Action voTo)
        {
            moCausalLinks.Add(new CausalLink(voFrom, voAchieves, voTo));
        }

        /// <summary>
        /// Add an Ordering Constraint
        /// </summary>
        /// <param name="voBefore">Before Action</param>
        /// <param name="voAfter">After Action</param>
        public void AddOrderingConstraint(Action voBefore, Action voAfter)
        {
            //Will adding this constraint cause a loop?
            bool bLoopDetected = CheckLoop(voBefore, voAfter);

            if (bLoopDetected == false)
            {
                moOrderingConstraints.Add(new OrderingConstraint(voBefore, voAfter));
                moOrderingConstraints.Add(new OrderingConstraint(moStartAction, voBefore));
                if (voAfter.Identity != moFinishAction.Identity)
                    moOrderingConstraints.Add(new OrderingConstraint(voBefore, moFinishAction));
            }
            else
            {
                string sException = string.Format("Loop detected ordering {0} {{ {1}", voBefore.Identity.Name,
                                                  voAfter.Identity.Name);
                throw new ConsistencyCheckException(sException);
            }
        }

        private bool CheckLoop(Action voBefore, Action voAfter)
        {
            if (voBefore == voAfter)
                return true;

            List<OrderingConstraint> oList = moOrderingConstraints.FindAll(delegate(OrderingConstraint o)
                                                                                    {
                                                                                        return o.Before == voAfter;
                                                                                    }
                                                                                    );

            //We are seeing if there is a condition like A->B B->C C->A
            foreach(OrderingConstraint after in oList)
            {
                bool bCheck = CheckLoop(voBefore, after.After);
                if (bCheck == true)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Add an Action to the Plan
        /// </summary>
        /// <param name="voAction">Action to Add</param>
        public void AddAction(Action voAction)
        {
            moActions.Add(voAction);
            foreach(Predicate pred in voAction.Preconditions)
            {
                if (moStartAction.Effects.Exists(delegate(Predicate p)
                            {
                                return p == pred && p.IsNegative == pred.IsNegative;
                            }) == false
                    )
                {
                    moOpenPreconditions.Add(new ActionPredicatePair(voAction, pred));
                }
                else
                {
                    this.AddCausalLink(moStartAction, pred, voAction);
                }
            }

            CheckPlanConsistent(voAction);
        }

        /// <summary>
        /// Remove the Action
        /// </summary>
        /// <param name="voAction">Action to Remove</param>
        /// <param name="voActionPredicatePair">Action-Predicate Pair</param>
        public void RemoveAction(Action voAction, ActionPredicatePair voActionPredicatePair)
        {
            moActions.Remove(voAction);
            moCausalLinks.RemoveAll(delegate(CausalLink c)
                                        {
                                            return c.From == voAction && c.Achieves == voActionPredicatePair.Second &&
                                                   c.Achieves.IsNegative == voActionPredicatePair.Second.IsNegative &&
                                                   c.To == voActionPredicatePair.First;
                                        });
            moOrderingConstraints.RemoveAll(delegate(OrderingConstraint o)
                                                {
                                                    return o.Before == voAction && o.After == voActionPredicatePair.First;
                                                });
            //TODO: Clean up the Start and Finish ones as well
        }

        /// <summary>
        /// Check the Plan for Consistency
        /// </summary>
        /// <param name="voAction"></param>
        private void CheckPlanConsistent(Action voAction)
        {
            //check for conflicts in the Causal Links
            foreach (CausalLink c in moCausalLinks)
            {
                if (c.To.Identity != voAction.Identity)
                {
                    //adding a negative predicate that cancels out a Causal Link
                    //causes a conflict we can add an ordering constraint either B { C or C { A
                    bool bConflict = voAction.Effects.Exists(delegate(Predicate p)
                            {
                                return
                                    p.IsNegative != c.Achieves.IsNegative && c.Achieves == p;
                            });
                    if (bConflict == true)
                    {
                        //if no loop would occur by adding B { C then add it
                        if (c.To != moFinishAction && CheckLoop(c.To, voAction) == false)
                        {
                            AddOrderingConstraint(c.To, voAction); 
                        }
                        else if (c.From != moStartAction) //we have no choice then to try adding C { A
                        {
                            AddOrderingConstraint(voAction, c.From);
                        }
                        else
                        {
                            string sException = string.Format("{0} {{ {1} and {1} {{ {2} invalid.", c.To.Identity.Name,
                                          voAction.Identity.Name, c.From.Identity.Name);
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
