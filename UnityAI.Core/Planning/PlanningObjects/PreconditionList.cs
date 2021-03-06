﻿//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Precondition in a Partial Order Plan
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
    public class PreconditionList : List<Predicate>
    {
        #region Constructors
        /// <summary>
        /// Create Empty Precondition List
        /// </summary>
        public PreconditionList() : base()
        {
        }

        /// <summary>
        /// Create Precondition List of Given Size
        /// </summary>
        /// <param name="capacity">Initial Size</param>
        public PreconditionList(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Create a Precondition List
        /// </summary>
        /// <param name="collection">Collection to Load</param>
        public PreconditionList(IEnumerable<Predicate> collection)
            : base(collection)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// String Representation of the PreconditionList
        /// </summary>
        /// <returns>[ {Predicates} ]</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (Predicate p in this)
            {
                sb.Append(p.ToString());
                sb.Append(",");
            }
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
