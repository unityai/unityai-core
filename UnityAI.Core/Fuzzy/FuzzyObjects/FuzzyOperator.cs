//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   FuzzyOperator 
//                From Constructing Intelligent Agents using Java
//
// Modification Notes:
// Date		Author        	Notes
// -------- ------          -----------------------------------------
// 01/26/09	SMcCarthy		Initial Implementation
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Fuzzy
{
    [Serializable]
    public class FuzzyOperator
    {
        #region Constructors
        /// <summary>
        /// Create a Fuzzy Operator
        /// </summary>
        private FuzzyOperator()
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Assign the Rhs to the Lhs
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        internal static void Assign(FuzzyRuleVariable lhs, FuzzySet rhs)
        {
            lhs.SetFuzzyValue(rhs);
        }

        /// <summary>
        /// Assign the Rhs Fuzzy to the Lhs with a truth value
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <param name="truthValue"></param>
        internal static void Assign(FuzzyRuleVariable lhs, FuzzySet rhs, double truthValue)
        {

            //This is correlation with a truth value.
            ((ContinuousFuzzyRuleVariable)lhs).SetFuzzyValue(rhs, truthValue);
        }

        /// <summary>
        /// Determines the Membership
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        internal static double Compare(FuzzyRuleVariable lhs, FuzzySet rhs)
        {

            // Take crisp value and look up membership
            return (rhs.Membership(lhs.GetNumericValue()));
        }
        #endregion
    }
}
