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

namespace UnityAI.Core.Planning
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
        public Predicate Identity
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
            voPredicate.ParentAction = this;
            moPreconditionList.Add(voPredicate);
        }

        /// <summary>
        /// Add an Effect to the Effects
        /// </summary>
        /// <param name="voPredicate">A Predicate to Add</param>
        public void AddEffect(Predicate voPredicate)
        {
            voPredicate.ParentAction = this;
            moEffectList.Add(voPredicate);
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// The Start Symbol has only Effects
        /// </summary>
        /// <param name="voEffects">The Effects of the Start</param>
        /// <returns>New Start Symbol</returns>
        public static Action CreateStart(params Predicate[] voEffects)
        {
            Action start = new Action(new Predicate("Start"));
            foreach (Predicate p in voEffects)
            {
                start.AddEffect(p);
            }

            return start;
        }

        /// <summary>
        /// Create the finish symbol
        /// </summary>
        /// <param name="voPreconditions">The Preconditions of the Finish</param>
        /// <returns>New Finish Symbol</returns>
        public static Action CreateFinish(params Predicate[] voPreconditions)
        {
            Action finish = new Action(new Predicate("Finish"));
            foreach (Predicate p in voPreconditions)
            {
                finish.AddPrecondition(p);
            }

            return finish;
        }
        #endregion
    }
}
