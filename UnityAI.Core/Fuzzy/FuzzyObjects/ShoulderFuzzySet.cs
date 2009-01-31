//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   ShoulderFuzzySet 
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
    public class ShoulderFuzzySet : FuzzySet
    {
        #region Fields
        private double mdPointBegin; // Left point
        private double mdPointEnd; // Right point
        private EnumFuzzySetDirection meSetDir; // Left or Right
        #endregion

        #region Properties

        /// <summary> 
        /// Retrieves the beginning point of the fuzzy set.
        /// </summary>
        virtual public double LeftPoint
        {
            get
            {
                return mdPointBegin;
            }

        }

        /// <summary> 
        /// Retrieves the end point of the fuzzy set.
        /// </summary>
        virtual public double RightPoint
        {
            get
            {
                return mdPointEnd;
            }

        }
        /// <summary> 
        /// Retrieves the direction of the fuzzy set.
        /// </summary>
        virtual public string SetDirection
        {
            get
            {
                return meSetDir.ToString();
            }

        }
        #endregion

        #region Constructors
        /// <summary> 
        /// Creates a new Shoulder fuzzy set.
        /// </summary>
        /// <param name="parentVar">the ContinuousFuzzyRuleVariable object that is the parent</param>
        /// <param name="name">the String object that contains the name</param>
        /// <param name="alphaCut">the double value for the alpha cut</param>
        /// <param name="ptBeg">the double value of the beginning point of the fuzzy set</param>
        /// <param name="ptEnd">the double value of the end point of the fuzzy set</param>
        /// <param name="setDirection">the integer that represents the direction of the shoulder</param>
        internal ShoulderFuzzySet(ContinuousFuzzyRuleVariable parentVar, string name, double alphaCut, double ptBeg, double ptEnd, EnumFuzzySetDirection setDirection)
            : base(EnumFuzzySetType.Shoulder, name, parentVar, alphaCut)
        {

            // Save the original parameters
            this.mdPointBegin = ptBeg;
            this.mdPointEnd = ptEnd;
            this.meSetDir = setDirection;

            // Set the domain values in the base class!
            mdDomainLo = parentVar.DiscourseLo;
            mdDomainHi = parentVar.DiscourseHi;

            // Working variables
            int numberOfValues = 4;
            double[] lclScalarVector = new double[5];
            double[] lclTruthVector = new double[5];

            // Set up the vectors for a shoulder
            if (setDirection == EnumFuzzySetDirection.Left)
            {
                lclScalarVector[0] = mdDomainLo;
                lclTruthVector[0] = 1.0;
                lclScalarVector[1] = ptBeg;
                lclTruthVector[1] = 1.0;
                lclScalarVector[2] = ptEnd;
                lclTruthVector[2] = 0.0;
                lclScalarVector[3] = mdDomainHi;
                lclTruthVector[3] = 0.0;
            }
            else
            {
                lclScalarVector[0] = mdDomainLo;
                lclTruthVector[0] = 0.0;
                lclScalarVector[1] = ptBeg;
                lclTruthVector[1] = 0.0;
                lclScalarVector[2] = ptEnd;
                lclTruthVector[2] = 1.0;
                lclScalarVector[3] = mdDomainHi;
                lclTruthVector[3] = 1.0;
            }

            // Fill in the truth vector for this set:
            SegmentCurve(numberOfValues, lclScalarVector, lclTruthVector);
        }
        #endregion

        #region Methods
        /// <summary> 
        /// Creates a clone of this fuzzy set and adds it to the parent variable.
        /// </summary>
        /// <param name="newName">the String object that contains the name of the clone/param>
        internal override void AddClone(string newName)
        {

            // Using the original parameters, create another set like this one and
            // add it to the containing variable's set list.
            moParentVar.AddSetShoulder(newName, mdAlphaCut, mdPointBegin, mdPointEnd, meSetDir);
        }
        #endregion
    }
}
