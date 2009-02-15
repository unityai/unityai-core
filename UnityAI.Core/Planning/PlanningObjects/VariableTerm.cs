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
    [Serializable]
    public class VariableTerm : Term
    {
        #region Fields
        private object moBindingObject = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Construct a Variable Term
        /// </summary>
        /// <param name="name"></param>
        protected VariableTerm(string name)
        {
            msName = name;
            meTermType = EnumTermType.Variable;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Bind the Variable
        /// </summary>
        /// <param name="o">Bind the Variable to this Object</param>
        public void BindVariable(object o)
        {
            moBindingObject = o;
        }
        #endregion

        #region Factory methods
        /// <summary>
        /// Create the Constant Term
        /// </summary>
        /// <param name="name">Name of the Constant</param>
        /// <returns>ConstantTerm</returns>
        public static VariableTerm Create(string name)
        {
            Term term = Term.FindTerm(name, EnumTermType.Variable);
            VariableTerm vt = null;
            if (term != null)
                vt = term as VariableTerm;
            else
            {
                vt = new VariableTerm(name);
                Term.AddTerm(vt);
            }
            return vt;
        }
        #endregion
    }
}
