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
    public class PartialOrderPlanner
    {
        #region Fields
        private List<Action> moActions;
        #endregion

        #region Constructor
        public PartialOrderPlanner(params Action[] voActions)
        {
            moActions = new List<Action>(voActions);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Plan the Order using POP Alogorithm
        /// </summary>
        /// <param name="voInitialState">Initial Predicates</param>
        /// <param name="voGoalState">Goal Predicates</param>
        /// <returns></returns>
        public PartialOrderPlan PlanOrder(IEnumerable<Predicate> voInitialState, IEnumerable<Predicate> voGoalState)
        {
            PartialOrderPlan plan = new PartialOrderPlan(voInitialState, voGoalState);

            //while we have open preconditions
            while(plan.HasOpenPreconditions)
            {
                Predicate pickedPrecondition = plan.PickOpenPrecondition();
                Action pickedAction = null;

                foreach(Action action in moActions)
                {
                    //if an action has the effect of the picked precondtion
                    if (action.Effects.Exists(
                        delegate(Predicate p)
                            {
                                return p == pickedPrecondition && p.IsNegative == pickedPrecondition.IsNegative;
                            }))
                    {
                        pickedAction = action;
                        break;
                    }
                }

                if (pickedAction == null)
                    throw new Exception("No action to pick");

                plan.AddCasualLink(pickedAction, pickedPrecondition, pickedPrecondition.ParentAction);
                
                plan.AddOrderingConstraint(pickedAction, pickedPrecondition.ParentAction);

                plan.AddAction(pickedAction);

                plan.OpenPreconditions.Remove(pickedPrecondition);
            }

            return plan;
        }
        #endregion
    }
}
