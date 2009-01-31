//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   WorkingFuzzySet 
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
    public class WorkingFuzzySet : FuzzySet
    {
        #region Fields
        internal bool mbSetEmpty;
        #endregion

        #region Properties
        /// <summary> 
        /// Checks if the working set is empty.
        /// </summary>
        virtual public bool Empty
        {
            get
            {
                return mbSetEmpty;
            }
        }
        #endregion

        #region Constructors
        /// <summary> 
        /// Creates a fuzzy work set with the given parameters.
        /// </summary>
        /// <param name="parentVar">the ContinuousFuzzyRuleVariable object that is the parent
        /// of this fuzzy set</param>
        /// <param name="setName">the String object that contains the set name</param>
        /// <param name="alphaCut">the double value for the alphacut threshold</param>
        /// <param name="discourseLo">the double value for the low end of the discourse</param>
        /// <param name="discourseHi">the double value for the high end of the discourse</param>
        internal WorkingFuzzySet(ContinuousFuzzyRuleVariable parentVar, string setName, double alphaCut, double discourseLo, double discourseHi)
            : base(EnumFuzzySetType.Work, setName, parentVar, alphaCut, discourseLo, discourseHi)
        {
            mbSetEmpty = true;
        }
        #endregion

        #region Methods
        /// <summary> 
        /// Does nothing.
        /// </summary>
        /// <param name="cloneName">the String object that contains the cloned set name</param>
        internal override void AddClone(string cloneName)
        {
            // Work sets are not cloneable; do nothing!
        }

        /// <summary> 
        /// Copies the input set if the current working set is empty, otherwise
        /// asserts the given set.
        /// </summary>
        /// <param name="inputSet">the FuzzySet object to be copied or asserted</param>
        internal virtual void CopyOrAssertFuzzy(FuzzySet inputSet)
        {
            if (mbSetEmpty)
            {
                Copy(inputSet);
            }
            else
            {
                Assert(inputSet);
            }
        }

        /// <summary> 
        /// Correlates the working set with the given input set using the given
        /// correlation method and truth value.
        /// </summary>
        /// <param name="inputSet">the FuzzySet object that contains the fuzzy set to be
        /// correlated with</param>
        /// <param name="corrMethod">the integer that represents the correlation method</param>
        /// <param name="truthValue">the double truth value</param>
        internal virtual void CorrelateWith(FuzzySet inputSet, EnumFuzzyCorrelationMethod corrMethod, double truthValue)
        {
            switch (corrMethod)
            {
                case EnumFuzzyCorrelationMethod.Minimise:
                    for (int i = 0; i < Constants.FUZZY_MAXVALUES; ++i)
                    {
                        if (truthValue <= inputSet.GetTruthValue(i))
                        {
                            mdTruthVector[i] = truthValue;
                        }
                        else
                        {
                            mdTruthVector[i] = inputSet.GetTruthValue(i);
                        }
                    }
                    break;

                case EnumFuzzyCorrelationMethod.Product:
                    for (int i = 0; i < Constants.FUZZY_MAXVALUES; ++i)
                    {
                        mdTruthVector[i] = (inputSet.GetTruthValue(i) * truthValue);
                    }
                    break;
            }
        }


        /// <summary> 
        /// Implicates the current working set to the given fuzzy set using the
        /// given infer method.
        /// </summary>
        /// <param name="inputSet">the WorkingFuzzySet object to implicate to</param>
        /// <param name="inferMethod">the integer that represents the infer method</param>
        internal virtual void ImplicateTo(WorkingFuzzySet inputSet, EnumFuzzyInferenceMethod inferMethod)
        {

            switch (inferMethod)
            {
                case EnumFuzzyInferenceMethod.Add:
                    for (int i = 0; i < Constants.FUZZY_MAXVALUES; ++i)
                    {
                        double sum = mdTruthVector[i] + inputSet.GetTruthValue(i);

                        if (sum < 1.0)
                        {
                            mdTruthVector[i] = sum;
                        }
                        else
                        {
                            mdTruthVector[i] = 1.0;
                        }
                    }
                    break;

                case EnumFuzzyInferenceMethod.MinMax:
                    for (int i = 0; i < Constants.FUZZY_MAXVALUES; ++i)
                    {
                        if (mdTruthVector[i] < inputSet.GetTruthValue(i))
                        {
                            mdTruthVector[i] = inputSet.GetTruthValue(i);
                        }
                    }
                    break;
            }
        }


        /// <summary> 
        /// Resets the working fuzzy set by setting the empty flag to true and setting
        /// the truth vector values to 0.0.
        /// </summary>
        internal virtual void Reset()
        {
            mbSetEmpty = true;
            for (int i = 0; i < Constants.FUZZY_MAXVALUES; ++i)
            {
                mdTruthVector[i] = 0.0;
            }
        }

        /// <summary> 
        /// Asserts the truth values from the given fuzzy set.
        /// </summary>
        /// <param name="inputSet">the FuzzySet object to be used for the assertion</param>
        internal virtual void Assert(FuzzySet inputSet)
        {
            for (int i = 0; i < Constants.FUZZY_MAXVALUES; ++i)
            {
                if (mdTruthVector[i] > inputSet.GetTruthValue(i))
                {
                    mdTruthVector[i] = inputSet.GetTruthValue(i);
                }
            }
        }

        /// <summary> 
        /// Copies the truth values from the given fuzzy set.
        /// </summary>
        /// <param name="inputSet">the FuzzySet object to be copied</param>
        internal virtual void Copy(FuzzySet inputSet)
        {
            for (int i = 0; i < Constants.FUZZY_MAXVALUES; ++i)
            {
                mdTruthVector[i] = inputSet.GetTruthValue(i);
            }
            mbSetEmpty = false;
        }
        #endregion
    }
}
