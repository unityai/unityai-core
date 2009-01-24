//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Predicate in a Partial Order Plan
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
    public class Predicate
    {
        #region Fields
        private string msName = string.Empty;
        private List<Term> moParameters = null;
        #endregion

        #region Properties
        /// <summary>
        /// Name of the Predicate
        /// </summary>
        public string Name
        {
            get { return msName;  }
            set { msName = value;  }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Construct a Predicate ie At(Monster,Location)
        /// </summary>
        /// <param name="vsName">Name of the Predicate</param>
        /// <param name="voParameters">Parameters</param>
        public Predicate(string vsName, params Term[] voParameters)
        {
            msName = vsName;
            if (voParameters != null)
            {
                moParameters = new List<Term>(voParameters);
            }
        }
        #endregion
    }
}
