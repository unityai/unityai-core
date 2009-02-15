﻿//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents an Effect in a Partial Order Plan
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Planning
{
    /// <summary>
    /// Represents the Effect of an Action
    /// </summary>
    [Serializable]
    public class EffectList : List<Predicate>
    {
        #region Constructors
        /// <summary>
        /// Create Empty Effect List
        /// </summary>
        public EffectList() : base()
        {
        }

        /// <summary>
        /// Create Effect List of Given Size
        /// </summary>
        /// <param name="capacity">Initial Size</param>
        public EffectList(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Create a Effect List
        /// </summary>
        /// <param name="collection">Collection to Load</param>
        public EffectList(IEnumerable<Predicate> collection)
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
