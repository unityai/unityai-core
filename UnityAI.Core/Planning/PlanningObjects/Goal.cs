//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Goal in a plan
//
// Authors: RJMendez
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UnityAI.Core.Planning
{
    public class Goal: IComparable
    {
        #region Fields
        Predicate moPredicate;
        float mfDesirability;
        #endregion 

        #region Public properties
        /// <summary>
        /// Predicate that represents achieving this goal
        /// </summary>
        public Predicate Predicate
        {
            get { return moPredicate; }
        }

        /// <summary>
        /// How desirable is this goal for the agent?
        /// </summary>
        /// <remarks>Value will be clamped to (0, 1)</remarks>
        public float Desirability
        {
            get { return mfDesirability; }
            set { mfDesirability = Mathf.Clamp(value, 0, 1); }
        }
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a goal
        /// </summary>
        /// <param name="predicate">Predicate to accomplish</param>
        /// <param name="desirability">Goal desirabilitys</param>
        public Goal(Predicate predicate, float desirability)
        {
            moPredicate = predicate;
            Desirability = desirability;
        }
        #endregion


        #region IComparable Members
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;            
            if (!(obj  is Goal))
            {
                throw new ArgumentException("Goals can only be compared to other Goals");
            }
            Goal other = obj as Goal;
            int comparison = 0;
            if (Predicate == null && other.Predicate != null)
                comparison = -1;
            else if (Predicate != null)
                comparison = Predicate.CompareTo(other.Predicate);
            else
                comparison = Desirability.CompareTo(other.Desirability);
            return comparison;
        }
        #endregion
    }
}
