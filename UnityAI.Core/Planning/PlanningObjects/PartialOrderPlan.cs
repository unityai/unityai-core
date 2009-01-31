//-------------------------------------------------------------------
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
        private List<CasualLink> moCasualLinks;
        private List<Predicate> moOpenPreconditions;
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
        public List<Predicate> OpenPreconditions
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
            moOpenPreconditions = new List<Predicate>();
            moCasualLinks = new List<CasualLink>();

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
                                return p == pred;
                            }) == false
                    )
                {
                    moOpenPreconditions.Add(pred);
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Randomly pick an open precondition
        /// </summary>
        /// <returns></returns>
        public Predicate PickOpenPrecondition()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int i = rand.Next(0, moOpenPreconditions.Count);

            return moOpenPreconditions[i];
        }

        /// <summary>
        /// Add a Casual Link
        /// </summary>
        /// <param name="voFrom">From Action</param>
        /// <param name="voAchieves">The Predicate that Achieves</param>
        /// <param name="voTo">To Action</param>
        public void AddCasualLink(Action voFrom, Predicate voAchieves, Action voTo)
        {
            moCasualLinks.Add(new CasualLink(voFrom, voAchieves, voTo));
        }

        /// <summary>
        /// Add an Ordering Constraint
        /// </summary>
        /// <param name="voBefore">Before Action</param>
        /// <param name="voAfter">After Action</param>
        public void AddOrderingConstraint(Action voBefore, Action voAfter)
        {
            moOrderingConstraints.Add(new OrderingConstraint(voBefore, voAfter));
            moOrderingConstraints.Add(new OrderingConstraint(moStartAction, voBefore));
            if (voAfter.ActionIdentity != moFinishAction.ActionIdentity)
                moOrderingConstraints.Add(new OrderingConstraint(voBefore, moFinishAction));
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
                                return p == pred;
                            }) == false
                    )
                {
                    moOpenPreconditions.Add(pred);
                }
                else
                {
                    this.AddCasualLink(moStartAction, pred, voAction);
                }
            }

            CheckPlanConsistent(voAction);
        }

        private void CheckPlanConsistent(Action voAction)
        {
            //check for conflicts in the casual links
            foreach (CasualLink c in moCasualLinks)
            {
                if (c.To.ActionIdentity != voAction.ActionIdentity)
                {
                    bool bConflict = voAction.Effects.Exists(delegate(Predicate p)
                            {
                                return
                                    p.IsNegative == true && c.Achieves.CompareTo(p) == 0;
                            });
                    if (bConflict == true)
                    {
                        AddOrderingConstraint(c.To, voAction);
                    }
                }
            }

            //TODO: Check the ordering contraints
        }

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
                                return o.Before.ActionIdentity == action.ActionIdentity ||
                                       o.After.ActionIdentity == action.ActionIdentity;
                            });
                if (oc == null)
                {
                    oSorted.Add(action);
                }
                else
                {
                    //this action must occur before oc.After
                    if (oc.Before.ActionIdentity == action.ActionIdentity)
                    {
                        int iIndex = oSorted.FindIndex(delegate(Action a) { return a.ActionIdentity == oc.After.ActionIdentity; });
                        if (iIndex > -1)
                            oSorted.Insert(iIndex,action);
                        else
                            oSorted.Add(action);
                    }
                    else
                    {
                        int iIndex = oSorted.FindIndex(delegate(Action a) { return a.ActionIdentity == oc.Before.ActionIdentity; });
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
