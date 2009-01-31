//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Casual Link in a Partial Order Plan
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
    public class CasualLink
    {
        #region Fields
        private Action moFromAction;
        private Predicate moAchieves;
        private Action moToAction;
        #endregion

        #region Properties
        /// <summary>
        /// The From Action
        /// </summary>
        public Action From
        {
            get { return moFromAction;  }
            set { moFromAction = value;  }
        }

        /// <summary>
        /// The To Action
        /// </summary>
        public Action To
        {
            get { return moToAction;  }
            set { moToAction = value;  }
        }

        /// <summary>
        /// From Achives To
        /// </summary>
        public Predicate Achieves
        {
            get { return moAchieves;  }
            set { moAchieves = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a CasualLink From Achieves To
        /// </summary>
        /// <param name="voFrom">The From Action</param>
        /// <param name="voAchieves">The Predicates that Achieves</param>
        /// <param name="voTo">The To Action</param>
        public CasualLink(Action voFrom, Predicate voAchieves, Action voTo)
        {
            moFromAction = voFrom;
            moAchieves = voAchieves;
            moToAction = voTo;
        }
        #endregion
    }
}
