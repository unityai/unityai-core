//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   FuzzyClause 
//                From Constructing Intelligent Agents using Java
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Fuzzy
{
    [Serializable]
    public class FuzzyClause
    {
        #region Fields
        internal FuzzyRuleVariable moLhs; // Lefthand side value
        internal EnumFuzzyOperator meOp; // Operator
        internal FuzzySet moRhs; // Righthand side value
        internal bool mbConsequent; // true if clause is a consequent clause
        #endregion

        #region Properties
        
        /// <summary>
        /// Left Hand Side
        /// </summary>
        virtual public FuzzyRuleVariable Lhs
        {
            get
            {
                return moLhs;
            }

        }

        /// <summary>
        /// The Operator
        /// </summary>
        virtual public EnumFuzzyOperator Op
        {
            get
            {
                return meOp;
            }

        }
        
        /// <summary>
        /// Right Hand Side of the Clause
        /// </summary>
        virtual public FuzzySet Rhs
        {
            get
            {
                return moRhs;
            }

        }
        
        /// <summary>
        /// Referent of the Right Hand Side
        /// </summary>
        virtual protected internal int Referent
        {
            get
            {
                return moRhs.Referent;
            }

        }
        
        /// <summary>
        /// Referent of the Left Hand Side
        /// </summary>
        virtual protected internal int LhsReferent
        {
            get
            {
                return moLhs.Referent;
            }

        }
        
        /// <summary>
        /// Referent of the Right Hand Side
        /// </summary>
        virtual protected internal int RhsReferent
        {
            get
            {
                return moRhs.Referent;
            }

        }
        #endregion

        #region Constructors
        /// <summary> Creates a new fuzzy clause with the specified lvalue, operator, and
        /// rvalue.
        /// 
        /// </summary>
        /// <param name="lhs">the FuzzyRuleVariable which is the lvalue
        /// </param>
        /// <param name="op">the integer operator
        /// </param>
        /// <param name="rhs">the FuzzySet which is the rvalue
        /// </param>
        protected internal FuzzyClause(FuzzyRuleVariable lhs, EnumFuzzyOperator op, FuzzySet rhs)
        {
            this.moLhs = lhs;
            this.meOp = op;
            this.moRhs = rhs;
            mbConsequent = (op == EnumFuzzyOperator.Assign);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Evaluate the Clause
        /// </summary>
        /// <returns></returns>
        protected internal virtual double Evaluate()
        {
            if (mbConsequent == false)
            {
                return FuzzyOperator.Compare(moLhs, moRhs); // antecedent
            }
            else
            {
                FuzzyOperator.Assign(moLhs, moRhs); // consequent
                return 0.0;
            }
        }

        /// <summary>
        /// Evaluate the Clause as a Consequent
        /// </summary>
        /// <param name="truthValue"></param>
        /// <returns></returns>
        protected internal virtual double Evaluate(double truthValue)
        {
            FuzzyOperator.Assign(moLhs, moRhs, truthValue);
            return 0.0;
        }

        /// <summary>
        /// String Representing this Object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string tmpStr = "";

            tmpStr = moLhs.Name;
            tmpStr = tmpStr + " " + meOp.ToString() + " ";
            tmpStr = tmpStr + moRhs.SetName;

            return tmpStr;
        }
        #endregion
    }
}
