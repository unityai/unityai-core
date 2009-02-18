//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Enumerations
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core
{
    #region Planning Enums
    /// <summary>
    /// Term Type
    /// </summary>
    public enum EnumTermType : int
    {
        Unknown = 0,
        Constant = 1,
        Variable = 2,
    }
    #endregion

    #region Fuzzy Enums
    /// <summary>
    /// Correlation Method
    /// </summary>
    public enum EnumFuzzyCorrelationMethod : int
    {
        Product = 1,
        Minimise = 2
    }

    /// <summary>
    /// Defuzzify Method
    /// </summary>
    public enum EnumFuzzyDefuzzifyMethod : int
    {
        Centroid = 1,
        MaxHeight = 2,
    }

    /// <summary>
    /// Inference Method
    /// </summary>
    public enum EnumFuzzyInferenceMethod : int
    {
        Add =1,
        MinMax = 2,
    }

    /// <summary>
    /// Set Direction
    /// </summary>
    public enum EnumFuzzySetDirection : int
    {
        Left = 1,
        Right = 2,
    }

    /// <summary>
    /// Set Type
    /// </summary>
    public enum EnumFuzzySetType : int
    {
               // fuzzy set types
        Shoulder = 1,
        Trapezoid = 2,
        Triangle = 3,
        Work = 4,
    }

    /// <summary>
    /// Data Type
    /// </summary>
    public enum EnumFuzzyDataType : int
    {
        				
        ContinuousVariable = 1,
        FuzzySet = 2,
    }

    /// <summary>
    /// Fuzzy Operator
    /// </summary>
    public enum EnumFuzzyOperator : int
    {
        Assign = 1,
        Compare = 2,
    }
    #endregion
}
