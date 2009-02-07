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
    public class PartialOrderPlanner
    {
        #region Fields
        private List<Action> moActions;
        #endregion

        #region Constructor
        /// <summary>
        /// Create the Planner
        /// </summary>
        /// <param name="actions">Actions</param>
        public PartialOrderPlanner(params Action[] actions)
        {
            moActions = new List<Action>(actions);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Plan the Order using POP Alogorithm
        /// </summary>
        /// <param name="initialState">Initial Predicates</param>
        /// <param name="goalState">Goal Predicates</param>
        /// <returns></returns>
        public PartialOrderPlan PlanOrder(IEnumerable<Predicate> initialState, IEnumerable<Predicate> goalState)
        {
            PartialOrderPlan plan = new PartialOrderPlan(initialState, goalState);
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
                        Console.Out.WriteLine(p + " " + pickedPair.Predicate + "  " + (pickedPair.Predicate == p));
                    });
                    if (action.Effects.Contains(pickedPair.Predicate))
                    {
                        pickedAction = action;
                        break;
                    }
                }

                //TODO: Backtrack
                if (pickedAction == null)
                    throw new Exception("No action to pick");

                plan.AddCausalLink(pickedAction, pickedPair.Predicate, pickedPair.Action);
                
                plan.AddOrderingConstraint(pickedAction, pickedPair.Action);

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
