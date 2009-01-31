//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   FuzzySet 
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
    public abstract class FuzzySet
    {
        #region Fields
        protected internal EnumFuzzySetType meSetType; // Set type
        protected internal string msSetName; // Set name
        protected internal double mdAlphaCut; // AlphaCut
        protected internal double mdDomainLo; // Domain, low end
        protected internal double mdDomainHi; // Domain, high end
        protected internal double[] mdTruthVector; // Truth values
        protected internal ContinuousFuzzyRuleVariable moParentVar; // Containing variable
        #endregion

        #region Properties
        
        /// <summary>
        /// Numeric Value of this Set
        /// </summary>
        virtual public double NumericValue
        {
            get
            {
                return Defuzzify((moParentVar.RuleBase).DefuzzifyMethod);
            }

        }
        
        /// <summary>
        /// Id of Object this Set Refers too
        /// </summary>
        virtual public int Referent
        {
            get
            {
                return Constants.FuzzyVarIdNull;
            }
        }
       
        /// <summary>
        /// Type of this Fuzzy Set
        /// </summary>
        virtual public EnumFuzzySetType SetType
        {
            get
            {
                return meSetType;
            }

        }
        
        /// <summary>
        /// Name of the Fuzzy Set
        /// </summary>
        virtual public string SetName
        {
            get
            {
                return msSetName;
            }

        }
       
        /// <summary>
        /// Alpha Cut Value
        /// </summary>
        virtual public double AlphaCut
        {
            get
            {
                return mdAlphaCut;
            }

        }
        
        /// <summary>
        /// Low Domain value
        /// </summary>
        virtual public double DomainLo
        {
            get
            {
                return mdDomainLo;
            }

        }
        
        /// <summary>
        /// High Domain Value
        /// </summary>
        virtual public double DomainHi
        {
            get
            {
                return mdDomainHi;
            }

        }
        
        /// <summary>
        /// Truth Values
        /// </summary>
        virtual public double[] TruthValues
        {
            get
            {
                return mdTruthVector;
            }

        }
        
        /// <summary>
        /// Finds Truth Value between 0 and 1
        /// </summary>
        virtual internal double SetHeight
        {
            get
            {
                double truthValue = mdTruthVector[0];

                for (int i = 0; i < Constants.FUZZY_MAXVALUES; i++)
                {
                    if (mdTruthVector[i] > truthValue)
                    {
                        truthValue = mdTruthVector[i];
                    }
                }
                return truthValue;
            }

        }
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new Fuzzy Set
        /// </summary>
        /// <param name="setType"></param>
        /// <param name="setName"></param>
        /// <param name="parentVar"></param>
        /// <param name="alphaCut"></param>
        /// <param name="domainLo"></param>
        /// <param name="domainHi"></param>
        protected internal FuzzySet(EnumFuzzySetType setType, string setName, ContinuousFuzzyRuleVariable parentVar, double alphaCut, double domainLo, double domainHi)
        {
            meSetType = setType;
            msSetName = setName;
            moParentVar = parentVar;
            mdAlphaCut = alphaCut;
            mdDomainLo = domainLo;
            mdDomainHi = domainHi;
            mdTruthVector = new double[Constants.FUZZY_MAXVALUES];
            for (int i = 0; i < Constants.FUZZY_MAXVALUES; i++)
            {
                mdTruthVector[i] = 0.0;
            }
        }

        /// <summary>
        /// Create a new Fuzzy Set
        /// </summary>
        /// <param name="setType"></param>
        /// <param name="setName"></param>
        /// <param name="parentVar"></param>
        /// <param name="alphaCut"></param>
        protected internal FuzzySet(EnumFuzzySetType setType, string setName, ContinuousFuzzyRuleVariable parentVar, double alphaCut)
            : this(setType, setName, parentVar, alphaCut, 0.0, 0.0)
        {
        }
        #endregion

        #region Methods
        
        /// <summary>
        /// Adda Clone of the Set
        /// </summary>
        /// <param name="setName"></param>
        internal abstract void AddClone(string setName);


        /// <summary>
        /// Get the Truth Value
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal virtual double GetTruthValue(int index)
        {
            return mdTruthVector[index];
        }

        /// <summary> 
        /// Zeroes out truthValues if they are below alphaCut threshold.
        /// </summary>
        internal virtual void ApplyAlphaCut()
        {
            for (int i = 1; i < Constants.FUZZY_MAXVALUES; i++)
            {
                if (!AboveAlphaCut(mdTruthVector[i]))
                {
                    mdTruthVector[i] = 0.0;
                }
            }
        }

        /// <summary> 
        /// Uses linguistic hedges to modify fuzzy set.
        /// </summary>
        /// <param name="hedges">set Name</param>
        internal virtual void ApplyHedges(string hedges)
        {
            // Apply the hedges in reverse order, ignoring null hedges.
            for (int i = (hedges.Length) - 1; i >= 0; --i)
            {
                char hedge = hedges[i];

                switch (hedge)
                {

                    case Constants.FuzzyHedgeNull:
                        break;

                    case Constants.FuzzyHedgeExtremely:
                        ApplyHedgeConDil(3.0);
                        break;

                    case Constants.FuzzyHedgeSlightly:
                        ApplyHedgeConDil(0.3);
                        break;

                    case Constants.FuzzyHedgeSomewhat:
                        ApplyHedgeConDil(0.5);
                        break;

                    case Constants.FuzzyHedgeVery:
                        ApplyHedgeConDil(2.0);
                        break;
                }
            }
        }

        /// <summary>
        /// Concentrates or Dilutes 
        /// </summary>
        /// <param name="exp"></param>
        internal virtual void ApplyHedgeConDil(double exp)
        {
            for (int i = 0; i < Constants.FUZZY_MAXVALUES; i++)
            {
                mdTruthVector[i] = (double)System.Math.Pow(mdTruthVector[i], exp);
            }
        }


        /// <summary>
        /// Defuzzify the Set
        /// </summary>
        /// <param name="veDefuzzMethod"></param>
        /// <returns></returns>
        internal virtual double Defuzzify(EnumFuzzyDefuzzifyMethod veDefuzzMethod)
        {

            // Set up local working variables:
            double defuzzedNumber = 0.0; // The returned number.
            int j = 0;
            int k = 0;
            int m = 0;
            double truthValue1;
            double truthValue2;

            // Apply the alpha cut to the fuzzy set;
            // If the resulting set has a maximum height of zero, there is no
            // point in defuzzing it.  Set the crisp number to zero.
            ApplyAlphaCut();
            if (SetHeight == 0)
            {
                defuzzedNumber = 0.0;
                return defuzzedNumber;
            }

            // The alpha cut set is non-zero; apply the specified defuzz method:
            switch (veDefuzzMethod)
            {


                // Centroid Method
                case EnumFuzzyDefuzzifyMethod.Centroid:
                    truthValue1 = 0.0;
                    truthValue2 = 0.0;
                    for (int i = 0; i < Constants.FUZZY_MAXVALUES; ++i)
                    {
                        truthValue1 = truthValue1 + mdTruthVector[i] * (double)i;
                        truthValue2 = truthValue2 + mdTruthVector[i];
                    }
                    if (truthValue2 == 0.0)
                    {
                        defuzzedNumber = 0.0;
                    }
                    else
                    {
                        j = (int)(truthValue1 / truthValue2);
                        defuzzedNumber = GetScalar(j);
                    }
                    break;

                // Max Height Method

                case EnumFuzzyDefuzzifyMethod.MaxHeight:
                    truthValue1 = mdTruthVector[0];
                    j = 0;
                    for (int i = 0; i < Constants.FUZZY_MAXVALUES; ++i)
                    {
                        if (mdTruthVector[i] > truthValue1)
                        {
                            truthValue1 = mdTruthVector[i];
                            k = i;
                        }
                    }
                    for (m = k + 1; m < Constants.FUZZY_MAXVALUES; ++m)
                    {
                        if (mdTruthVector[m] != truthValue1)
                        {
                            break;
                        }
                    }
                    j = (int)(((double)m - (double)k) / 2);
                    if (j == 0)
                    {
                        j = k;
                    }
                    defuzzedNumber = GetScalar(j);
                    break;

                // Unknown defuzz method; return zero

                default:
                    defuzzedNumber = 0.0;
                    break;

            }
            return defuzzedNumber;
        }


        /// <summary> 
        /// Retrieves the membership value for the given scalar value.
        /// </summary>
        /// <param name="scalar">the double scalar value</param>
        /// <returns> the double truth value</returns>
        internal virtual double Membership(double scalar)
        {
            double truthValue = 0.0;
            int index;
            double range;

            // If the scalar is lower than this set's low domain value, set the
            // truth value to be the same as the left-edge truth value.
            if (scalar < mdDomainLo)
            {
                truthValue = mdTruthVector[0];
            }
            // If the scalar is higher than this set's high domain value, set the
            // truth value to be the same as the right-edge truth value.
            else if (scalar > mdDomainHi)
            {
                truthValue = mdTruthVector[Constants.FUZZY_MAXVALUES];
            }
            else
            {

                // Calcualte an index into the membership vector and then use the
                // index to pull out the membership value.
                range = mdDomainHi - mdDomainLo;
                index = (int)(((scalar - mdDomainLo) / range) * (Constants.FUZZY_MAXVALUES - 1));
                truthValue = mdTruthVector[index];
            }
            return truthValue;
        }


        /// <summary> 
        /// Checks if the truth values is above the alpha cut value.
        /// </summary>
        /// <param name="truthValue">the double truth value to be checked</param>
        /// <returns> the boolean true if truthValue is above alphaCut</returns>
        internal virtual bool AboveAlphaCut(double truthValue)
        {
            if (truthValue > mdAlphaCut)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary> 
        /// Adjusts the truthValues so that the max is 1.0.
        /// </summary>
        internal virtual void Normalize()
        {
            double maxTruthValue = SetHeight;

            // If the set is entirely false, just return
            // (and prevent divide by zero!)
            if (maxTruthValue == 0.0)
            {
                return;
            }

            // If the set is already normalised, just return.
            if (maxTruthValue == 1.0)
            {
                return;
            }
            for (int i = 0; i < Constants.FUZZY_MAXVALUES; i++)
            {
                mdTruthVector[i] = mdTruthVector[i] / maxTruthValue;
            }
        }

        /// <summary> 
        /// Retrives the scalar for the given index within the domain range.
        /// </summary>
        /// <param name="index">the integer value of the index</param>
        /// <returns> the double scalar value</returns>
        internal virtual double GetScalar(int index)
        {
            double range = mdDomainHi - mdDomainLo;
            double width = range / Constants.FUZZY_MAXVALUES;
            double scalar = (index * width) + mdDomainLo;

            return scalar;
        }

        /// <summary> 
        /// Retrieves the first and last points in a segment curve interpolated
        /// from the working truth vector.
        /// </summary>
        /// <param name="numberOfValues">the integer value for the number of truth values</param>
        /// <param name="scalarVector">the double[] scalar vector</param>
        /// <param name="aTruthVector">the double[] truth vector</param>
        /// <returns>the int[] that contains the first and last points of the segment</returns>
        internal virtual int[] SegmentCurve(int numberOfValues, double[] scalarVector, double[] aTruthVector)
        {

            bool normalSet = false; // Is set normal?
            int point = 256; // Working curve point
            double[] tmpTruthVector = new double[Constants.FUZZY_MAXVALUES + 1]; // Working truth vector
            double widthDomain = mdDomainHi - mdDomainLo;
            int[] tmpSegs = new int[] { -1, Constants.FUZZY_MAXVALUES }; // First & last user points

            // Fill in the truth vector for this set:
            // Initialise the working truth vector
            for (int i = 0; i < Constants.FUZZY_MAXVALUES; ++i)
            {
                tmpTruthVector[i] = -1;
            }

            // Determine where in the working truth vector to place the
            // user's designated truth value given the truth value's
            // associated scalar.
            for (int i = 0; i < numberOfValues; ++i)
            {
                point = (int)(((scalarVector[i] - mdDomainLo) / widthDomain) * Constants.FUZZY_MAXVALUES);
                tmpTruthVector[point] = aTruthVector[i];
                if (tmpSegs[0] == -1)
                {
                    tmpSegs[0] = point; // The first user point
                }
                if (aTruthVector[i] == 1.0)
                {
                    normalSet = true;
                }
            }
            tmpSegs[1] = point; // The last user point

            // Interpolate the curve in the working truth vector between
            // the user's specified points.
            if (tmpTruthVector[0] == -1)
            {
                tmpTruthVector[0] = 0.0;
            }
            VectorInterpret(tmpTruthVector);

            // Now update this set's actual truth vector by copying the
            // working truth vector into it.
            for (int i = 0; i < Constants.FUZZY_MAXVALUES; ++i)
            {
                mdTruthVector[i] = tmpTruthVector[i];
            }
            return tmpSegs; // Return the first & last user points
        }


        /// <summary>
        /// Fills in the bits in the truth vector.
        /// </summary>
        /// <param name="aTruthVector">the double[] truth vector</param>
        internal virtual void VectorInterpret(double[] aTruthVector)
        {

            int i, j, k;
            int point1, point2;
            double factor;
            double seg1, seg2;

            // Scan the input truth vector, looking for values that the user
            // has entered.  When a user value is found, fill in the bits of
            // the vector between that point and the user's previous point (or
            // the start of the vector).
            i = 0;
            j = 0;

            for (; ; )
            {
                j++;

                // If we have looked at all values, return to caller.
                if (j > Constants.FUZZY_MAXVALUES)
                {
                    return;
                }

                // Skip over all values pre-initialized to -1, looking for
                // a value that the user has entered (which must, of course,
                // be between 0.0 and 1.0)
                if (aTruthVector[j] == -1)
                {
                    continue;
                }

                // We found a value other than -1;
                // we now want to fill in the truth vector from where we
                // started the scan up to here.
                point1 = i + 1;
                point2 = j;
                for (k = point1; k < point2 + 1; ++k)
                {
                    seg1 = k - i;
                    seg2 = j - i;
                    factor = seg1 / seg2;
                    if (k > Constants.FUZZY_MAXVALUES)
                    {
                        return;
                    }
                    aTruthVector[k] = aTruthVector[i] + (factor * (aTruthVector[j] - aTruthVector[i]));
                }

                // Ok, we've filled in that bit.
                // See if we have scanned the entire vector.
                i = j;
                if (i >= Constants.FUZZY_MAXVALUES)
                {
                    return;
                }

                // Got some vector left.
                // Go scan past -1 values, looking for valid truth values
                // or the end of the vector.
            }
        }

        /// <summary> 
        /// Retrieves a string describing the contents of the object.
        /// </summary>
        /// <returns>a String containing the current contents of the object </returns>
        public override string ToString()
        {
            return moParentVar.Name + "." + msSetName + "(" + NumericValue + ") ";
        }
        #endregion
    }
}
