//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents an Action in a Partial Order Plan
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
    public class Action
    {
        #region Field
        private Predicate moActionIdentity;
        private PreconditionList moPreconditionList = new PreconditionList();
        private EffectList moEffectList = new EffectList();
        #endregion

        #region Properties
        /// <summary>
        /// The predicate that represents this action
        /// </summary>
        public Predicate ActionIdentity
        {
            get { return moActionIdentity;  }
        }

        /// <summary>
        /// Represents the Preconditions
        /// </summary>
        public PreconditionList Preconditions
        {
            get { return moPreconditionList;  }
        }

        /// <summary>
        /// Represents the Effects
        /// </summary>
        public EffectList Effects
        {
            get { return moEffectList;  }
        }
        #endregion

        #region Constructors
        public Action(Predicate voActionIdentity)
        {
            moActionIdentity = voActionIdentity;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add a Precondition
        /// </summary>
        /// <param name="voPredicate">A Predicate to Add</param>
        public void AddPrecondition(Predicate voPredicate)
        {
            moPreconditionList.Add(voPredicate);
        }

        /// <summary>
        /// Add an Effect to the Effects
        /// </summary>
        /// <param name="voPredicate">A Predicate to Add</param>
        public void AddEffect(Predicate voPredicate)
        {
            moEffectList.Add(voPredicate);
        }
        #endregion
    }
}
