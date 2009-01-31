//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   ContinuousFuzzyRuleVariable 
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
    public class ContinuousFuzzyRuleVariable : FuzzyRuleVariable
    {
        #region Fields
        internal double mdDiscourseLo; // Universe of discourse, low end
        internal double mdDiscourseHi; // Universe of discourse, high end
        internal Dictionary<string, FuzzySet> moSetList; // Fuzzy sets defined over this variable
        internal double mdValCrisp; // Crisp value of fuzzy work set (if known)
        internal WorkingFuzzySet moValFzy; // Fuzzy Solution Variable
        internal bool bValKnown; // Is crisp value known (T) or undefined?
        internal WorkingFuzzySet moValFzyTmp; // Fuzzy working space to hold temp copies
        #endregion

        #region Properties
        /// <summary> Retrieves the value of this object as a symbolic value.
        /// 
        /// </summary>
        /// <returns> a <code>String</code>, if the value of this object can be represented
        /// as a symbolic value
        /// </returns>
        /// <summary> Sets the value of this object to the given symbolic value.
        /// 
        /// </summary>
        /// <param name="newValue"> the String value to which this variable is set
        /// </param>
        virtual public string SymbolicValue
        {
            get
            {
                return GetRawValue().ToString();
            }

            set
            {
                SetNumericValue(System.Double.Parse(value));
            }

        }
        
        /// <summary>
        /// Set the Value of the Variable
        /// </summary>
        override public FuzzySet Value
        {
            set
            {
                SetFuzzyValue(value);
            }

        }
        
        /// <summary>
        /// Get/Set the Value as a String
        /// </summary>
        override internal string ValueString
        {
            get
            {
                return mdValCrisp.ToString();
            }

            set
            {
                try
                {
                    double dValue = System.Double.Parse(value);
                    SetRawValue(dValue);
                }
                catch (System.FormatException e)
                {
                    Console.Out.WriteLine("Error: " + msName + " " + value);
                }
            }

        }
        
        /// <summary>
        /// Low end of the Universe of Discourse
        /// </summary>
        virtual public double DiscourseLo
        {
            get
            {
                return mdDiscourseLo;
            }

        }
       
        /// <summary>
        /// High end of hte Universe of Discourse
        /// </summary>
        virtual public double DiscourseHi
        {
            get
            {
                return mdDiscourseHi;
            }

        }
        /// <summary> Retrieves the fuzzy sets.
        /// 
        /// </summary>
        /// <returns> the Dictionary object that contains list of fuzzy sets.
        /// 
        /// </returns>
        virtual public Dictionary<string, FuzzySet> FuzzySets
        {
            get
            {
                return moSetList;
            }
        }

        /// <summary> Retrieves the fuzzy work area of this continuous variable.
        /// 
        /// </summary>
        /// <returns>  the FuzzySet object that contains the work area.
        /// </returns>
        virtual public FuzzySet FuzzyWorkArea
        {
            get
            {
                return moValFzyTmp;
            }

        }
        #endregion

        #region Constructors
       /// <summary>
       /// Create a new Continuous Variable
       /// </summary>
       /// <param name="fuzzyBase">FuzzyBase</param>
       /// <param name="name">Name</param>
       /// <param name="discourseLo">Low</param>
       /// <param name="discourseHi">High</param>
        public ContinuousFuzzyRuleVariable(FuzzyRuleBase fuzzyBase, string name, double discourseLo, double discourseHi)
            : base(EnumFuzzyDataType.ContinuousVariable, fuzzyBase, name)
        {
            mdDiscourseLo = discourseLo;
            mdDiscourseHi = discourseHi;
            moSetList = new Dictionary<string, FuzzySet>();
            bValKnown = false;
            mdValCrisp = 0.0;
            moValFzy = new WorkingFuzzySet(this, name + " Fuzzy Solution Space", moRuleBase.AlphaCut, discourseLo, discourseHi);
            moValFzyTmp = new WorkingFuzzySet(this, name + " Fuzzy Work Space", moRuleBase.AlphaCut, discourseLo, discourseHi);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the Fuzzy Value
        /// </summary>
        /// <returns></returns>
        public virtual FuzzySet GetFuzzyValue()
        {
            return moValFzy;
        }


       /// <summary>
       /// Get the Numeric Value
       /// </summary>
       /// <returns></returns>
        public override double GetNumericValue()
        {
            return GetRawValue();
        }

        /// <summary>
        /// Set the Fuzzy Value
        /// </summary>
        /// <param name="newValue"></param>
        public override void SetFuzzyValue(FuzzySet newValue)
        {
            SetRawValue(newValue);
        }


        /// <summary>
        /// Set the Numeric Value
        /// </summary>
        /// <param name="newValue"></param>
        public virtual void SetNumericValue(double newValue)
        {
            SetRawValue(newValue);
        }


        /// <summary>
        /// Reset teh Variable
        /// </summary>
        public override void Reset()
        {
            bValKnown = false;
            mdValCrisp = 0.0;
            moValFzy.Reset();
            moValFzyTmp.Reset();
        }


        /// <summary>
        /// Checks if variable is within the discourse
        /// </summary>
        /// <param name="dValue"></param>
        /// <returns></returns>
        internal virtual bool WithinUniverseOfDiscourse(double dValue)
        {
            if ((mdDiscourseLo <= dValue) && (dValue <= mdDiscourseHi))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Set the Crispt value
        /// </summary>
        /// <param name="newValue"></param>
        internal virtual void SetRawValue(double newValue)
        {
            if (WithinUniverseOfDiscourse(newValue))
            {
                mdValCrisp = newValue;
            }
            else
            {
                Console.Out.WriteLine("Error: Value is out of range " + msName);
            }
        }

        /// <summary>
        /// Get the Crisp Value
        /// </summary>
        /// <returns></returns>
        internal virtual double GetRawValue()
        {
            return mdValCrisp;
        }

        /// <summary>
        /// Set the Raw Value
        /// </summary>
        /// <param name="newSet"></param>
        internal virtual void SetRawValue(FuzzySet newSet)
        {

            //This is an assertion following the "Minimum Law of Fuzzy Assertions".
            //Change the Fuzzy Work Space 'valFzy' with the new set.
            moValFzy.CopyOrAssertFuzzy(newSet);

            //When all done, defuzzify the work space and store in valCrisp
            mdValCrisp = moValFzy.Defuzzify(moRuleBase.DefuzzifyMethod);
        }


        /// <summary>
        /// Set the Truth Value
        /// </summary>
        /// <param name="newSet"></param>
        /// <param name="truthValue"></param>
        internal virtual void SetFuzzyValue(FuzzySet newSet, double truthValue)
        {

            //This is correlation with a truth value.
            //Change the Fuzzy Work Space 'valFzy' with the new set
            //correlated to truth value 'truthValue'
            moValFzyTmp.CorrelateWith(newSet, moRuleBase.CorrelationMethod, truthValue);
            moValFzy.ImplicateTo(moValFzyTmp, moRuleBase.InferenceMethod);

            //When all done, defuzzify the work space and store in valCrisp
            mdValCrisp = moValFzy.Defuzzify(moRuleBase.DefuzzifyMethod);
        }


        /// <summary>
        /// Check if the Set Exists
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        internal virtual bool SetExist(string setName)
        {
            return moSetList.ContainsKey(setName);
        }


        /// <summary>
        /// Get the Set By Name
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        internal virtual FuzzySet GetSet(string setName)
        {
            if (SetExist(setName))
            {
                return (FuzzySet)(moSetList[setName]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Get the Hedged Set or Add it
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="hedges"></param>
        /// <returns></returns>
        internal virtual FuzzySet GetOrAddHedgedSet(string setName, string hedges)
        {
            string hedgeName = setName + " " + hedges;

            // The base set must, of course, exist!
            if (!SetExist(setName))
            {
                Console.Out.WriteLine("Error: Unknown Set " + msName + " " + setName);
            }

            // If the hedged set already exists, simply return it.
            if (SetExist(hedgeName))
            {
                return GetSet(hedgeName);
            }

            // The hedged set does not exist; create it.
            // Attempt to create a clone of the specified set.
            ((FuzzySet)(moSetList[setName])).AddClone(hedgeName);

            // Clone was created and added to the set list;
            // Now apply the hedges to the clone.
            ((FuzzySet)(moSetList[hedgeName])).ApplyHedges(hedges);

            // Now return the newly created hedged set.
            return GetSet(hedgeName);
        }

        /// <summary>
        /// Add Set Shoulder
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="alphaCut"></param>
        /// <param name="ptBeg"></param>
        /// <param name="ptEnd"></param>
        /// <param name="setDir"></param>
        public virtual void AddSetShoulder(string setName, double alphaCut, double ptBeg, double ptEnd, EnumFuzzySetDirection setDir)
        {
            lock (this)
            {
                moSetList[setName] = new ShoulderFuzzySet(this, setName, alphaCut, ptBeg, ptEnd, setDir);
            }
        }


        /// <summary>
        /// Add the Trapezoid Set
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="alphaCut"></param>
        /// <param name="ptLeft"></param>
        /// <param name="ptLeftCore"></param>
        /// <param name="ptRightCore"></param>
        /// <param name="ptRight"></param>
        public virtual void AddSetTrapezoid(string setName, double alphaCut, double ptLeft, double ptLeftCore, double ptRightCore, double ptRight)
        {
            lock (this)
            {
                moSetList[setName] = new TrapezoidFuzzySet(this, setName, alphaCut, ptLeft, ptLeftCore, ptRightCore, ptRight);
            }
        }

        /// <summary>
        /// Add a Set Triangle
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="alphaCut"></param>
        /// <param name="ptLeft"></param>
        /// <param name="ptCenter"></param>
        /// <param name="ptRight"></param>
        public virtual void AddSetTriangle(string setName, double alphaCut, double ptLeft, double ptCenter, double ptRight)
        {
            lock (this)
            {
                moSetList[setName] = new TriangleFuzzySet(this, setName, alphaCut, ptLeft, ptCenter, ptRight);
            }
        }


        /// <summary> Returns the name of this variable.
        /// 
        /// </summary>
        /// <returns> the String object that contains the name
        /// 
        /// </returns>
        public override string ToString()
        {
            return msName;
        }
        #endregion
    }
}
