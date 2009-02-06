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
            List<Action> oSkipList = new List<Action>();

            //while we have open preconditions
            while(plan.HasOpenPreconditions)
            {
                ActionPredicatePair pickedPair = plan.PickOpenPrecondition();
                Action pickedAction = null;

                foreach(Action action in moActions)
                {
                    if (oSkipList.Contains(action))
                        continue; 

                    //if an action has the effect of the picked precondtion
                    action.Effects.ForEach(delegate(Predicate p)
                    {
                        Console.Out.WriteLine(p + " " + pickedPair.Second + "  " + (pickedPair.Second == p));
                    });
                    if (action.Effects.Exists(
                        delegate(Predicate p)
                            {
                                return p == pickedPair.Second && p.IsNegative == pickedPair.Second.IsNegative;
                            }))
                    {
                        pickedAction = action;
                        break;
                    }
                }

                //TODO: Backtrack
                if (pickedAction == null)
                    throw new Exception("No action to pick");

                plan.AddCausalLink(pickedAction, pickedPair.Second, pickedPair.First);
                
                plan.AddOrderingConstraint(pickedAction, pickedPair.First);

                try
                {
                    plan.AddAction(pickedAction);
                    plan.OpenPreconditions.Remove(pickedPair);
                    oSkipList.Clear();
                }
                catch (ConsistencyCheckException ex)
                {
                    Console.Out.WriteLine("Can't add action results in consistency failure: " + ex.Message);
                    //TODO: remove causual link and ordering constraint, and re-pick an action - may need to back track
                    plan.RemoveAction(pickedAction, pickedPair);
                    oSkipList.Add(pickedAction);
                }
            }

            return plan;
        }
        #endregion
    }
}
