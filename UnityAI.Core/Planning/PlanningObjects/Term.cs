//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents Term in a Partial Order Plan
//                That is a Constant, Variable or Function   
// 
// Authors: SMcCarthy, RJMendez 
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Planning
{
    [Serializable]
    public class Term : IComparable
    {
        #region Static fields
        private static Dictionary<int, Term> moCreatedTerms;
        #endregion 

        #region Fields
        protected EnumTermType meTermType = EnumTermType.Unknown;
        protected string msName = string.Empty;
        #endregion

        #region Static properties
        /// <summary>
        /// Current cache of terms
        /// </summary>
        /// <remarks>Consiering removing this one, do not rely on it</remarks>
        protected static Dictionary<int, Term> CreatedTerms
        {
            get { return moCreatedTerms; }
        }
        #endregion

        #region Member Properties
        /// <summary>
        /// The name of the Term
        /// </summary>
        public string Name
        {
            get { return msName;  }
            set { msName = value; }
        }

        /// <summary>
        /// The TermType (Constant, Variable, Function)
        /// </summary>
        public EnumTermType TermType
        {
            get { return meTermType;  }        
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>Classes should not be built directly</remarks>
        protected Term()
        {
        }

        static Term()
        {
            InitializeCache();
        }
        #endregion

        #region Static methods
        /// <summary>
        /// Initialize the Cache
        /// </summary>
        public static void InitializeCache()
        {
            moCreatedTerms = new Dictionary<int, Term>();
        }

        /// <summary>
        /// Finds a term by name and type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="termType"></param>
        /// <returns></returns>
        public static Term FindTerm(String name, EnumTermType termType)
        {
            Term term = null;
            int code = CalculateHashCode(name, termType);
            if (moCreatedTerms.ContainsKey(code))
                term = moCreatedTerms[code];
            return term;
        }

        /// <summary>
        /// Adds a term to the current cache
        /// </summary>
        /// <param name="term">Term to add</param>
        protected static void AddTerm(Term term)
        {
            moCreatedTerms[term.GetHashCode()] = term;
        }
        #endregion

        #region IComparable and comparison Members
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj is Term == false)
                throw new ArgumentException("Terms can only be compared to other Terms");

            Term t = obj as Term;

            if (TermType == t.TermType)
            {
                return Name.CompareTo(t.Name);
            }
            else
            {
                return TermType.CompareTo(t.TermType);
            }
        }

        /// <summary>
        /// Is this object equal to another object?
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is Term)
            {
                Term t = obj as Term;
                result = this.TermType == t.TermType && this.Name == t.Name;
            }
            return result;
        }

        /// <summary>
        /// Calculates the object's HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return CalculateHashCode(msName, meTermType); 
        }

        /// <summary>
        /// Calculates a hash code based on the name and term type
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="termType">Term type</param>
        /// <returns>Hash code</returns>
        /// <remarks>Implemented separetly from GetHashCode so that it can be
        /// used to generate hashes without the need for an instance</remarks>
        protected static int CalculateHashCode(string name, EnumTermType termType)
        {
            return name.GetHashCode() ^ termType.GetHashCode();
        }
        #endregion

        #region Methods
        /// <summary>
        /// String Representaion of the Term
        /// </summary>
        /// <returns>{ {Name} }</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append(Name);
            sb.Append("}");
            return sb.ToString();
        }
        #endregion
    }
}
