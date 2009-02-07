//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   TrapezoidFuzzySet 
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
    public class TrapezoidFuzzySet : FuzzySet
    {
        #region Fields
        private double mdPointLeft;
        private double mdPointLeftCore;
        private double mdPointRightCore;
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
        /// Retrieves the beginning point of the plateau.
        /// </summary>
        virtual public double LeftCorePoint
        {
            get
            {
                return mdPointLeftCore;
            }
        }

        /// <summary> 
        /// Retrieves the end point of the plateau.
        /// </summary>
        virtual public double RightCorePoint
        {
            get
            {
                return mdPointRightCore;
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
        /// Creates a new Trapezoid fuzzy set.
        /// </summary>
        /// <param name="parentVar">the ContinuousFuzzyRuleVariable object that is the parent</param>
        /// <param name="name">the String object that contains the name</param>
        /// <param name="alphaCut">the double value for the alpha cut</param>
        /// <param name="ptLeft">the double value of the beginning point of the fuzzy set</param>
        /// <param name="ptLeftCore">the double value of the beginning point of the plateau</param>
        /// <param name="ptLeftCore">the double value of the end point of the plateau</param>
        /// <param name="ptRight">the double value of the end point of the fuzzy set</param>
        internal TrapezoidFuzzySet(ContinuousFuzzyRuleVariable parentVar, string name, double alphaCut, double ptLeft, double ptLeftCore, double ptRightCore, double ptRight)
            : base(EnumFuzzySetType.Trapezoid, name, parentVar, alphaCut)
        {

            // Save the original parameters
            this.mdPointLeft = ptLeft;
            this.mdPointLeftCore = ptLeftCore;
            this.mdPointRightCore = ptRightCore;
            this.mdPointRight = ptRight;

            // Set the domain values in the base class!
            mdDomainLo = parentVar.DiscourseLo;
            mdDomainHi = parentVar.DiscourseHi;

            // Working variables
            int numberOfValues = 6;
            double[] lclScalarVector = new double[7];
            double[] lclTruthVector = new double[7];

            // Set up the vectors for a trapezoid:
            lclScalarVector[0] = mdDomainLo;
            lclTruthVector[0] = 0.0;
            lclScalarVector[1] = ptLeft;
            lclTruthVector[1] = 0.0;
            lclScalarVector[2] = ptLeftCore;
            lclTruthVector[2] = 1.0;
            lclScalarVector[3] = ptRightCore;
            lclTruthVector[3] = 1.0;
            lclScalarVector[4] = ptRight;
            lclTruthVector[4] = 0.0;
            lclScalarVector[5] = mdDomainHi;
            lclTruthVector[5] = 0.0;

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
            moParentVar.AddSetTrapezoid(newName, mdAlphaCut, mdPointLeft, mdPointLeftCore, mdPointRightCore, mdPointRight);
        }
        #endregion
    }
}
