//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents an Effect in a Partial Order Plan
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
    /// <summary>
    /// Represents the Effect of an Action
    /// </summary>
    public class EffectList : List<Predicate>
    {
        #region Methods
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
