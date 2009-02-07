//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents an Ordering Constraint in a Partial Order 
//                Plan
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Planning
{
    [Serializable]
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
        /// <param name="before">The Action that Comes Before</param>
        /// <param name="after">The Action that Comes After</param>
        public OrderingConstraint(Action before, Action after)
        {
            moBeforeAction = before;
            moAfterAction = after;
        }
        #endregion
    }
}
