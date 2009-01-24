//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents an Ordering Constraint in a Partial Order 
//                Plan
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
    public class OrderingConstraint
    {
        #region Fields
        private Action moBeforeAction;
        private Action moAfterAction;
        #endregion

        #region Properties
        /// <summary>
        /// The Before Action
        /// </summary>
        public Action Before
        {
            get { return moBeforeAction;  }
            set { moBeforeAction = value;  }
        }

        /// <summary>
        /// Represents the After Action
        /// </summary>
        public Action After
        {
            get { return moAfterAction;  }
            set { moAfterAction = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Create an Ordering that is A is before B
        /// </summary>
        /// <param name="voBefore">The Action that Comes Before</param>
        /// <param name="voAfter">The Action that Comes After</param>
        public OrderingConstraint(Action voBefore, Action voAfter)
        {
            moBeforeAction = voBefore;
            moAfterAction = voAfter;
        }
        #endregion
    }
}
