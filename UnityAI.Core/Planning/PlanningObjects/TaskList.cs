//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Task List 
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Planning
{
    /// <summary>
    /// Represents the Precondition of an Action Object
    /// </summary>
    [Serializable]
    public class TaskList : List<Task>
    {
        #region Constructors
        /// <summary>
        /// Create Empty Task List
        /// </summary>
        public TaskList() : base()
        {
        }

        /// <summary>
        /// Create Task List of Given Size
        /// </summary>
        /// <param name="capacity">Initial Size</param>
        public TaskList(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Create a Task List
        /// </summary>
        /// <param name="collection">Collection to Load</param>
        public TaskList(IEnumerable<Task> collection) : base(collection)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// String Representation of the TaskList
        /// </summary>
        /// <returns>[ {Tasks} ]</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (Task t in this)
            {
                sb.Append(t.ToString());
                sb.Append(",");
            }
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
