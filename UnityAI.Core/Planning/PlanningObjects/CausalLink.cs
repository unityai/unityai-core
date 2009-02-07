//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Causal Link in a Partial Order Plan
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Planning
{
    [Serializable]
    public class CausalLink
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
        /// Creates a CausalLink From Achieves To
        /// </summary>
        /// <param name="from">The From Action</param>
        /// <param name="achieves">The Predicates that Achieves</param>
        /// <param name="to">The To Action</param>
        public CausalLink(Action from, Predicate achieves, Action to)
        {
            moFromAction = from;
            moAchieves = achieves;
            moToAction = to;
        }
        #endregion
    }
}
