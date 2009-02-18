//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents an BeliefNodeList in the BeliefNet
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Belief
{
    public class BeliefNodeList : List<BeliefNode>
    {
        #region Constructors
        /// <summary>
        /// Create Empty Belief List
        /// </summary>
        public BeliefNodeList() : base()
        {
        }

        /// <summary>
        /// Create Belif List of Given Size
        /// </summary>
        /// <param name="capacity">Initial Size</param>
        public BeliefNodeList(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Create a BeliefNode List
        /// </summary>
        /// <param name="collection">Collection to Load</param>
        public BeliefNodeList(IEnumerable<BeliefNode> collection)
            : base(collection)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// String Representation of the BeliefList
        /// </summary>
        /// <returns>[ {BeliefNode} ]</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (BeliefNode b in this)
            {
                sb.Append(b.ToString());
                sb.Append(",");
            }
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// Get the Belief given by name
        /// </summary>
        /// <param name="vsName">Name of Node</param>
        /// <returns>Belief Node with that name</returns>
        public BeliefNode GetBelief(string vsName)
        {
            return Find(delegate(BeliefNode bn) { return bn.Name.ToUpper() == vsName.ToUpper(); });
        }
        #endregion
    }
}
