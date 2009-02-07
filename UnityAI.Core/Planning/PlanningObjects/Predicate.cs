//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Predicate in a Partial Order Plan
//
// Authors: SMcCarthy, RJMendez
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Planning
{
    [Serializable]
    public class Predicate : IComparable
    {
        #region Fields
        private string msName = string.Empty;
        private List<Term> moParameters = null;
        private bool mbIsNegative = false;
        private static Dictionary<int, Predicate> moPredicateCache;
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
        #endregion

        #region Static properties
        /// <summary>
        /// Current cache of terms
        /// </summary>
        /// <remarks>Consiering removing this one, do not rely on it</remarks>
        protected static Dictionary<int, Predicate> PredicateCache
        {
            get { return moPredicateCache; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Static Constructor
        /// </summary>
        static Predicate()
        {
            InitializeCache();
        }
        /// <summary>
        /// Construct a Predicate ie At(Monster,Location)
        /// </summary>
        /// <param name="name">Name of the Predicate</param>
        /// <param name="parameters">Parameters</param>
        protected Predicate(string name, params Term[] parameters)
            : this(name, false, parameters)
        {
        }

        /// <summary>
        /// Construct a Predicate ie At(Monster,Location)
        /// </summary>
        /// <param name="name">Name of the Predicate</param>
        /// <param name="isNegative">Is Negated?</param>
        /// <param name="parameters">Parameters</param>
        protected Predicate(string name, bool isNegative, params Term[] parameters)
        {
            msName = name;
            mbIsNegative = isNegative;
            if (parameters != null)
            {
                moParameters = new List<Term>(parameters);
            }
            else
            {
                moParameters = new List<Term>();
            }
        }
        #endregion

        #region IComparable Members

        /// <summary>
        /// Is this predicate Equal to another Predicate?
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is Predicate)
            {
                Predicate predicate = obj as Predicate;

                if (Name == predicate.Name && IsNegative == predicate.IsNegative && Parameters.Count == predicate.Parameters.Count)
                {
                    result = true;
                    for (int i = 0; i < Parameters.Count; i++)
                    {
                        if (Parameters[i] != predicate.Parameters[i])
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj is Predicate == false)
                throw new ArgumentException("Predicates can only be compared to other Predicates");

            int comparison = 0;
            Predicate p = obj as Predicate;

            //lets put negative preconditions first
            if (IsNegative == true && p.IsNegative == false)
            {
                comparison = -1;
            }
            else if (IsNegative == false && p.IsNegative == true)
            {
                comparison = 1;
            }
            else if (Parameters.Count == p.Parameters.Count)
            {
                for(int i=0;i<Parameters.Count;i++)
                {
                    if (Parameters[i] != p.Parameters[i])
                    {
                        comparison = Parameters[i].CompareTo(p.Parameters[i]);
                    }
                }
                comparison = Name.CompareTo(p.Name);
            }
            else if (Parameters.Count < p.Parameters.Count)
            {
                comparison = -1;
            }
            else
            {
                comparison = 1;
            }
            return comparison;
        }

        /// <summary>
        /// Calculates the predicate's  hashcode
        /// </summary>
        /// <returns>Hascode</returns>
        public override int GetHashCode()
        {
            return CalculateHashCode(this);
        }

        /// <summary>
        /// Calculates a hashcode for a predicate
        /// </summary>
        /// <param name="predicate">Predicate to evaluate</param>
        /// <returns>Hashcode</returns>
        /// <remarks>Written separately so that it can be used for calculating hashes on predicat caching</remarks>
        protected static int CalculateHashCode(Predicate predicate)
        {
            return CalculateHashCode(predicate.Name, predicate.IsNegative, predicate.Parameters);
        }

        /// <summary>
        /// Calculates a hashcode for a predicate's parameters
        /// </summary>
        /// <param name="name">Predicate name</param>
        /// <param name="parameters">Predicate parameters</param>
        /// <param name="isNegative">Is the predicate negative?</param>
        /// <returns>Hashcode</returns>
        /// <remarks>Written separately so that it can be used for calculating hashes on predicat caching</remarks>
        public static int CalculateHashCode(string name, bool isNegative, IEnumerable<Term> parameters)
        {
            int hashCode = name.GetHashCode();
            if (parameters != null)
            {
                foreach (var term in parameters)
                {
                    hashCode ^= term.GetHashCode();
                }
            }
            hashCode ^= isNegative.GetHashCode();
            return hashCode;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Are the Predicates Negatives of Eachother
        /// </summary>
        /// <param name="predicate">Predicate to Compare</param>
        /// <returns>True is the Predicate is the Compliment</returns>
        public bool IsComplimentOf(Predicate predicate)
        {
            bool result = false;

            if (Name == predicate.Name && IsNegative != predicate.IsNegative && Parameters.Count == predicate.Parameters.Count)
            {
                result = true;
                for (int i = 0; i < Parameters.Count; i++)
                {
                    if (Parameters[i] != predicate.Parameters[i])
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// String Representation of the Predicate
        /// </summary>
        /// <returns>Predicate:{negation}{name}({parameters})</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Predicate:");
            if (mbIsNegative)
                sb.Append("~");
            sb.Append(Name);
            sb.Append("(");
            if (Parameters != null && Parameters.Count > 0)
            {
                foreach (Term t in Parameters)
                {
                    sb.Append(t.ToString());
                    sb.Append(",");
                }
                sb.Length--;
            }
            sb.Append(")");
            return sb.ToString();
        }
        #endregion

        #region Factory Methods
        /// <summary>
        /// Create a Predicate
        /// </summary>
        /// <param name="name">Name of Predicate</param>
        /// <returns>Predicate</returns>
        public static Predicate Create(string name)
        {
            return Create(name, false, null);
        }

        /// <summary>
        /// Create a Predicate
        /// </summary>
        /// <param name="name">Name of Predicate</param>
        /// <param name="isNegative">Is Negative?</param>
        /// <returns>Predicate</returns>
        public static Predicate Create(string name, bool isNegative)
        {
            return Create(name, isNegative, null);
        }

        /// <summary>
        /// Create a Predicate
        /// </summary>
        /// <param name="name">Name of Predicate</param>
        /// <param name="isNegative">Is Negative</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Predicate</returns>
        public static Predicate Create(string name, bool isNegative, params Term[] parameters)
        {
            Predicate predicate = Predicate.FindPredicate(name, isNegative, parameters);
            ConstantTerm ct = null;
            if (predicate == null)
            {
                predicate = new Predicate(name, isNegative, parameters);
                Predicate.AddPredicate(predicate);
            }
            return predicate;
        }
        #endregion

        #region Static Methods

        /// <summary>
        /// Find the Predicate in the Cache
        /// </summary>
        /// <param name="name">Name of the Predicate</param>
        /// <param name="isNegative">Is Negated</param>
        /// <param name="parameters">Terms</param>
        /// <returns>Predicate or null</returns>
        public static Predicate FindPredicate(string name, bool isNegative, params Term[] parameters)
        {
            Predicate predicate = null;
            int code = CalculateHashCode(name, isNegative, parameters);
            if (moPredicateCache.ContainsKey(code))
                predicate = moPredicateCache[code];
            return predicate;
        }

        /// <summary>
        /// Adds a Predicate to the current cache
        /// </summary>
        /// <param name="predicate">Predicate to add</param>
        protected static void AddPredicate(Predicate predicate)
        {
            moPredicateCache[predicate.GetHashCode()] = predicate;
        }

        /// <summary>
        /// Initialize the Predicate Cache
        /// </summary>
        private static void InitializeCache()
        {
            moPredicateCache = new Dictionary<int, Predicate>();
        }

        #endregion
    }
}
