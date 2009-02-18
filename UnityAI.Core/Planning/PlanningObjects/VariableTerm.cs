//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Variable Term in a Partial Order Plan
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Planning
{
    /// <summary>
    /// Represents a variable term bound to an object
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <remarks>Still considering if a generic is the way to go for this,
    /// since it may get us into issues when evaluating a mixed list of terms,
    /// but will go with it for now.</remarks>
    [Serializable]
    public class VariableTerm<T> : Term
    {
        #region Fields
        private T moBindingObject = default(T);
        #endregion

        #region Properties
        /// <summary>
        /// Object the term is bound to
        /// </summary>
        public T BindingObject
        {
            get
            {
                return moBindingObject;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Construct a Variable Term
        /// </summary>
        /// <param name="name">Term name</param>
        /// <param name="bindingObject">Object to bind to</param>
        protected VariableTerm(string name, T bindingObject)
        {
            msName = name;
            meTermType = EnumTermType.Variable;
            moBindingObject = bindingObject;
        }
        #endregion

        #region Static methods
        /// <summary>
        /// Finds a term by name and type
        /// </summary>
        /// <param name="name"></param>
        /// <param name="termType"></param>
        /// <returns></returns>
        public static VariableTerm<T> FindTerm(String name, T bindingObject)
        {
            VariableTerm<T> term = null;
            int code = CalculateHashCode(name, bindingObject);
            if (CreatedTerms.ContainsKey(code))
                term = CreatedTerms[code] as VariableTerm<T>;
            return term;
        }

        /// <summary>
        /// Calculates a hash code based on the name and object
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="o">Object to be bound</param>
        /// <returns>Hash code</returns>
        /// <remarks>Implemented separetly from GetHashCode so that it can be
        /// used to generate hashes without the need for an instance</remarks>
        protected static int CalculateHashCode(string name, object bindingObject)
        {
            return Term.CalculateHashCode(name, EnumTermType.Variable) ^ bindingObject.GetHashCode();
        }
        #endregion

        #region Factory methods
        /// <summary>
        /// Create the Variable Term
        /// </summary>
        /// <param name="name">Name of the Term</param>
        /// <param name="o">Object to be bound</param>
        /// <returns>Variable Term of the corresponding type</returns>
        public static VariableTerm<T> Create(string name, T bindingObject)
        {
            VariableTerm<T> term = FindTerm(name, bindingObject);
            if (term == null)
            {
                term = new VariableTerm<T>(name, bindingObject);
                Term.AddTerm(term);
            }
            return term;
        }
        #endregion

        #region IComparable methods
        /// <summary>
        /// Is this object equal to another object?
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is VariableTerm<T>)
            {
                VariableTerm<T> t = obj as VariableTerm<T>;
                result = this.TermType == t.TermType && this.Name == t.Name && this.BindingObject.Equals(t.BindingObject);
            }
            return result;
        }

        /// <summary>
        /// Calculates the object's HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return CalculateHashCode(msName, moBindingObject);
        }

        #endregion
    }
}
