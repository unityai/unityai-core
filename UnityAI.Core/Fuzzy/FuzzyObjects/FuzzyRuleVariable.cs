//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   FuzzyRuleVariable 
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
    public abstract class FuzzyRuleVariable
    {
        #region Fields
        internal static int NextId = 0; // For generated Ids only
        protected EnumFuzzyDataType meType; // Variable type
        protected int miId; // Unique Id
        protected string msName; // Variable name
        protected FuzzyRuleBase moRuleBase; // Containing ruleset
        #endregion

        #region Properties
        
        /// <summary>
        /// Id of variable this refers too
        /// </summary>
        virtual public int Referent
        {
            get { return Id; }
        }
        
        /// <summary>
        /// Type of Object
        /// </summary>
        virtual public EnumFuzzyDataType Type
        {
            get
            {
                return meType;
            }

        }
        
        /// <summary>
        /// Value of the Variable
        /// </summary>
        internal abstract string ValueString { get; set; }
        
        virtual public string TypeAsString
        {
            get
            {
                return meType.ToString();
            }

        }

        /// <summary>
        /// Id of this variable
        /// </summary>
        virtual public int Id
        {
            get
            {
                return miId;
            }

        }
        
        /// <summary>
        /// Variable Name
        /// </summary>
        virtual public string Name
        {
            get
            {
                return msName;
            }

        }
        
        /// <summary>
        /// Rule Base this rule belongs too
        /// </summary>
        virtual internal FuzzyRuleBase RuleBase
        {
            get
            {
                return moRuleBase;
            }

        }
        
        /// <summary>
        /// Set the FuzzySet
        /// </summary>
        public abstract FuzzySet Value { set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Create a Fuzzy Rule Variable
        /// </summary>
        /// <param name="veType">Type</param>
        /// <param name="rb">RuleBase</param>
        /// <param name="name">Name</param>
        protected internal FuzzyRuleVariable(EnumFuzzyDataType veType, FuzzyRuleBase rb, string name)
        {
            meType = veType;
            moRuleBase = rb;
            miId = NextId++; // give unique identifier for use in BitSets
            msName = name;
            rb.AddVariable(this); // add self to rule base
        }
        #endregion

        #region Methods
        /// <summary>
        /// Reset the Rule
        /// </summary>
        public abstract void Reset();


        /// <summary>
        /// Set the Fuzzy Value of the Variable
        /// </summary>
        /// <param name="newValue">FuzzySet</param>
        public abstract void SetFuzzyValue(FuzzySet newValue);


        /// <summary>
        /// Get the Numeric Value of this Rule
        /// </summary>
        /// <returns></returns>
        public abstract double GetNumericValue();


        /// <summary>
        /// Returns the Name of the Variable
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return msName;
        }
        #endregion
    }
}
