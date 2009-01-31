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

namespace UnityAI.Core.Planning
{
    public class Predicate : IComparable
    {
        #region Fields
        private string msName = string.Empty;
        private List<Term> moParameters = null;
        private Action moParentAction = null;
        private bool mbIsNegative = false;
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

        /// <summary>
        /// The Parameters
        /// </summary>
        public List<Term> Parameters
        {
            get { return moParameters; }
        }

        /// <summary>
        /// Is this a Negative Predicate 
        /// </summary>
        public bool IsNegative
        {
            get { return mbIsNegative;  }
            set { mbIsNegative = value; }
        }

        /// <summary>
        /// The Parent Action
        /// </summary>
        public Action ParentAction
        {
            get { return moParentAction;  }
            set { moParentAction = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Construct a Predicate ie At(Monster,Location)
        /// </summary>
        /// <param name="vsName">Name of the Predicate</param>
        /// <param name="voParameters">Parameters</param>
        public Predicate(string vsName, params Term[] voParameters)
            : this(vsName, false, voParameters)
        {
        }

        /// <summary>
        /// Construct a Predicate ie At(Monster,Location)
        /// </summary>
        /// <param name="vsName">Name of the Predicate</param>
        /// <param name="vbNegated">Is Negated?</param>
        /// <param name="voParameters">Parameters</param>
        public Predicate(string vsName, bool vbNegated, params Term[] voParameters)
        {
            msName = vsName;
            mbIsNegative = vbNegated;
            if (voParameters != null)
            {
                moParameters = new List<Term>(voParameters);
            }
        }
        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj is Predicate == false)
                throw new ArgumentException("Predicates can only be compared to other Predicates");

            Predicate p = obj as Predicate;

            if (Parameters.Count == p.Parameters.Count)
            {
                for(int i=0;i<Parameters.Count;i++)
                {
                    if (Parameters[i] != p.Parameters[i])
                    {
                        return Parameters[i].CompareTo(p.Parameters[i]);
                    }
                }
                return Name.CompareTo(p.Name);
            }
            else if (Parameters.Count < p.Parameters.Count)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public static bool operator == (Predicate p1, Predicate p2)
        {
            return p1.CompareTo(p2) == 0;
        }

        public static bool operator !=(Predicate p1, Predicate p2)
        {
            return !(p1 == p2);
        }
        #endregion
    }
}
