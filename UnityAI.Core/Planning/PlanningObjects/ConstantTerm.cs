//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Constant Term in a Partial Order Plan
//
// Authors: SMcCarthy, RJMendez 
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Planning
{
    public class ConstantTerm : Term
    {
        #region Constructors
        /// <summary>
        /// Construct a Constant Term
        /// </summary>
        /// <param name="name"></param>
        protected ConstantTerm(string name)
        {
            msName = name;
            meTermType = EnumTermType.Constant;
        }
        #endregion

        #region Factory methods
        /// <summary>
        /// Create the Constant Term
        /// </summary>
        /// <param name="name">Name of the Constant</param>
        /// <returns>ConstantTerm</returns>
        public static ConstantTerm Create(string name)
        {
            Term term = Term.FindTerm(name, EnumTermType.Constant);
            ConstantTerm ct = null;
            if (term != null)
                ct = term as ConstantTerm;
            else
            {
                ct = new ConstantTerm(name);
                Term.AddTerm(ct);
            }
            return ct;
        }

        /// <summary>
        /// Implicit Operator to allow strings to used in place of params
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static implicit operator ConstantTerm(string name)  // implicit string to constant term conversion operator
        {
            Console.Out.WriteLine("implict conversion to ConstantTerm occurred");
            return Create(name);  // implicit conversion
        }
        #endregion
    }
}
