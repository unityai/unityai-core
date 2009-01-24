//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents Term in a Partial Order Plan
//                That is a Constant, Variable or Function    
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
    [Serializable]
    public class Term
    {
        #region Fields
        protected EnumTermType meTermType = EnumTermType.Unknown;
        protected string msName = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// The name of the Term
        /// </summary>
        public string Name
        {
            get { return msName;  }
            set { msName = value; }
        }

        /// <summary>
        /// The TermType (Constant, Variable, Function)
        /// </summary>
        public EnumTermType TermType
        {
            get { return meTermType;  }        
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Term()
        {
        }
        #endregion
    }
}
