//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents an Action in a Partial Order Plan
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Planning
{
    [Serializable]
    public class Action
    {
        #region Field
        protected Predicate moActionIdentity;
        protected PreconditionList moPreconditionList = new PreconditionList();
        protected EffectList moEffectList = new EffectList();
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
        /// <summary>
        /// Create an Action
        /// </summary>
        /// <param name="actionIdentity"></param>
        public Action(Predicate actionIdentity)
        {
            moActionIdentity = actionIdentity;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add a Precondition
        /// </summary>
        /// <param name="predicate">A Predicate to Add</param>
        public void AddPrecondition(Predicate predicate)
        {
            moPreconditionList.Add(predicate);
        }

        /// <summary>
        /// Add an Effect to the Effects
        /// </summary>
        /// <param name="predicate">A Predicate to Add</param>
        public void AddEffect(Predicate predicate)
        {
            moEffectList.Add(predicate);
        }

        /// <summary>
        /// String Representation of the Action
        /// </summary>
        /// <returns>Action: [ C={Preconditions} E={Effects} ]</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Action: [");
            sb.Append(moActionIdentity.ToString());
            sb.Append("  C= ");
            sb.Append(Preconditions.ToString());
            sb.Append("  E= ");
            sb.Append(Effects.ToString());
            sb.Append("]");
            return sb.ToString();
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// The Start Symbol has only Effects
        /// </summary>
        /// <param name="effects">The Effects of the Start</param>
        /// <returns>New Start Symbol</returns>
        public static Action CreateStart(params Predicate[] effects)
        {
            Action start = new Action(Predicate.Create("Start"));
            foreach (Predicate p in effects)
            {
                start.AddEffect(p);
            }

            return start;
        }

        /// <summary>
        /// Create the finish symbol
        /// </summary>
        /// <param name="preconditions">The Preconditions of the Finish</param>
        /// <returns>New Finish Symbol</returns>
        public static Action CreateFinish(params Predicate[] preconditions)
        {
            Action finish = new Action(Predicate.Create("Finish"));
            foreach (Predicate p in preconditions)
            {
                finish.AddPrecondition(p);
            }

            return finish;
        }
        #endregion
    }
}
