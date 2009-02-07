//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   TriangleFuzzySet 
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
    public class TriangleFuzzySet : FuzzySet
    {
        #region Fields
        private double mdPointLeft;
        private double mdPointCenter;
        private double mdPointRight;
        #endregion

        #region Properties

        /// <summary> 
        /// Retrieves the beginning point of the fuzzy set.
        /// </summary>
        virtual public double LeftPoint
        {
            get
            {
                return mdPointLeft;
            }
        }

        /// <summary> 
        /// Retrieves the peak point of the fuzzy set.
        /// </summary>
        virtual public double CenterPoint
        {
            get
            {
                return mdPointCenter;
            }
        }

        /// <summary> 
        /// Retrieves the end point of the fuzzy set.
        /// </summary>
        virtual public double RightPoint
        {
            get
            {
                return mdPointRight;
            }

        }

        #endregion

        #region Constructor
        /// <summary> 
        /// Creates a new Triangle fuzzy set.
        /// </summary>
        /// <param name="parentVar">the ContinuousFuzzyRuleVariable object that is the parent</param>
        /// <param name="name">the String object that contains the name</param>
        /// <param name="alphaCut">the double value for the alpha cut</param>
        /// <param name="ptLeft">the double value of the beginning point of the fuzzy set</param>
        /// <param name="ptCenter">the double value of the peak of the fuzzy set</param>
        /// <param name="ptRight">the double value of the end point of the fuzzy set</param>
        internal TriangleFuzzySet(ContinuousFuzzyRuleVariable parentVar, string name, double alphaCut, double ptLeft, double ptCenter, double ptRight)
            : base(EnumFuzzySetType.Triangle, name, parentVar, alphaCut)
        {

            // Save the original parameters
            this.mdPointLeft = ptLeft;
            this.mdPointCenter = ptCenter;
            this.mdPointRight = ptRight;

            // Set the domain values in the base class!
            mdDomainLo = parentVar.DiscourseLo;
            mdDomainHi = parentVar.DiscourseHi;

            // Working variables
            int numberOfValues = 5;
            double[] lclScalarVector = new double[7];
            double[] lclTruthVector = new double[7];

            // Set up the vectors for a trapezoid:
            lclScalarVector[0] = mdDomainLo;
            lclTruthVector[0] = 0.0;
            lclScalarVector[1] = ptLeft;
            lclTruthVector[1] = 0.0;
            lclScalarVector[2] = ptCenter;
            lclTruthVector[2] = 1.0;
            lclScalarVector[3] = ptRight;
            lclTruthVector[3] = 0.0;
            lclScalarVector[4] = mdDomainHi;
            lclTruthVector[4] = 0.0;

            // Fill in the truth vector for this set:
            SegmentCurve(numberOfValues, lclScalarVector, lclTruthVector);
        }
        #endregion

        #region Methods
        /// <summary> 
        /// Creates a clone of this fuzzy set and adds it to the parent variable.
        /// </summary>
        /// <param name="newName">the String object that contains the name of the clone</param>
        internal override void AddClone(string newName)
        {
            // Using the original parameters, create another set like this one and
            // add it to the containing variable's set list.
            moParentVar.AddSetTriangle(newName, mdAlphaCut, mdPointLeft, mdPointCenter, mdPointRight);
        }
        #endregion
    }
}
