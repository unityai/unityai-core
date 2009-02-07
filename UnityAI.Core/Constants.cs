//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Constants
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core
{
    public static class Constants
    {
        /// <summary>
        /// The UnityAI Framework Identifier
        /// </summary>
        public const string UNITY_FRAMEWORK = "UnityAI";

        #region Fuzzy Constants
        public const double FuzzyAlphaCutDefault = .10;
        public const char FuzzyHedgeNull = '.';
        public const char FuzzyHedgeExtremely = 'E';
        public const char FuzzyHedgeSlightly = 'S';
        public const char FuzzyHedgeSomewhat = 'M';
        public const char FuzzyHedgeVery = 'V';
        public const int FUZZY_MAXVALUES = 256;

        // miscellaneous constants
        internal const int FuzzyContinuousVariable = 1;
        internal const int FuzzySet = 2;
        internal const int FuzzyRuleIdInitial = 1;
        internal const int FuzzyVarIdInitial = 1;
        internal const int FuzzyVarIdNull = 0;
        internal const string FuzzySymbolNull = "Fuzzy_NULL";
        #endregion
    }
}
