//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Constant Term in a Partial Order Plan
//
// Modification Notes:
// Date		Author        	Notes
// -------- ------          -----------------------------------------
// 01/26/09	SMcCarthy		Initial Implementation
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core
{
    public class ConstantTerm : Term
    {
        #region Constructors
        /// <summary>
        /// Constuct a Constant Term
        /// </summary>
        /// <param name="vsName"></param>
        public ConstantTerm(string vsName)
        {
            Name = vsName;
            meTermType = EnumTermType.Constant;
        }
        #endregion
    }
}
