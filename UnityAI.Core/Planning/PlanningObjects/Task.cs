//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Task that is an Action that is Primitive
//                or NonPrimitive
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Planning
{
    public class Task : Action
    {
        #region Fields
        private bool mbIsPrimitive;
        private TaskList moSubTasks;
        #endregion

        #region Properties
        /// <summary>
        /// Is this a Primitive Task
        /// </summary>
        public bool IsPrimitive
        {
            get { return mbIsPrimitive;  }
        }
        /// <summary>
        /// The SubTasks
        /// </summary>
        public TaskList SubTasks
        {
            get { return moSubTasks;  }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a Task
        /// </summary>
        /// <param name="actionIdentity"></param>
        public Task(Predicate actionIdentity)
            : base(actionIdentity)
        {
            mbIsPrimitive = true;
        }

        /// <summary>
        /// Create a Non-Primitive Task
        /// </summary>
        /// <param name="actionIdentity"></param>
        /// <param name="subTasks"></param>
        public Task(Predicate actionIdentity, params Task[] subTasks) 
            : base(actionIdentity)
        {
            mbIsPrimitive = false;
            moSubTasks = new TaskList(subTasks);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Decompose this Task into a Partial Order Plan
        /// </summary>
        /// <returns></returns>
        public PartialOrderPlan Decompose()
        {
            //TODO: Unfinished
            PartialOrderPlanner planner = new PartialOrderPlanner(SubTasks.ToArray());

            return planner.PlanOrder(new List<Predicate>(Preconditions), new List<Predicate>(Effects));
        }

        /// <summary>
        /// String Representation of the Action
        /// </summary>
        /// <returns>{Primitive | NonPrimitive} Action: [ C={Preconditions} E={Effects} T={Sub Tasks}]</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} Task: [", (IsPrimitive ? "Primitive" : "NonPrimitive"));
            sb.Append(moActionIdentity.ToString());
            sb.Append("  C= ");
            sb.Append(Preconditions.ToString());
            sb.Append("  E= ");
            sb.Append(Effects.ToString());
            if (IsPrimitive == false)
            {
                sb.Append("  T= ");
                sb.Append(SubTasks.ToString());
            }
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
