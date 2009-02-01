//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   FuzzyRuleBase 
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
    public class FuzzyRuleBase 
    {
        #region Variables

        internal static int NextId = 0;
        private string msName = "";

        // RuleBase options
        private double mdAlphaCut = Constants.FuzzyAlphaCutDefault;
        private EnumFuzzyCorrelationMethod meCorrelationMethod;
        private EnumFuzzyDefuzzifyMethod meDefuzzifyMethod;
        private EnumFuzzyInferenceMethod meInferenceMethod;

        // Lists of variables
        private int varId; // Unique Id of next variable
        private Dictionary<string, FuzzyRuleVariable> moVariableList; // List of all variables

        // Lists of rules
        private int ruleId; // Unique Id of next rule
        private List<FuzzyRule> moRuleList; // List of all rules
        private List<FuzzyRule> moCndRuleList; // List of conditional rules
        private List<FuzzyRule> moUncRuleList; // Sequence of unconditional rules

        // Fact Base
        private System.Collections.BitArray moFbInitial; // Initial Fact Base
        #endregion

        #region Properties
 
        /// <summary>
        /// Name of the Rule Base
        /// </summary>
        public string Name
        {
            get
            {
                return msName;
            }

            set
            {
                msName = value;
            }
        }

        /// <summary>
        /// Alpha Cut Value
        /// </summary>
        public double AlphaCut
        {
            get
            {
                return mdAlphaCut;
            }

            set
            {
                if ((value > 0.0) && (value < 1.0))
                {
                    mdAlphaCut = value;
                }
            }
        }

        /// <summary>
        /// Correlation Method
        /// </summary>
        public EnumFuzzyCorrelationMethod CorrelationMethod
        {
            get
            {
                return meCorrelationMethod;
            }

            set
            {
                meCorrelationMethod = value;
            }
        }

        /// <summary>
        /// Defuzzify Method
        /// </summary>
        public EnumFuzzyDefuzzifyMethod DefuzzifyMethod
        {
            get
            {
                return meDefuzzifyMethod;
            }

            set
            {
                meDefuzzifyMethod = value;
            }
        }

        /// <summary>
        /// Inference Method
        /// </summary>
        public EnumFuzzyInferenceMethod InferenceMethod
        {
            get
            {
                return meInferenceMethod;
            }

            set
            {
                meInferenceMethod = value;
            }
        }

        /// <summary>
        /// Variables
        /// </summary>
        public Dictionary<string, FuzzyRuleVariable> Variables
        {
            get
            {
                return moVariableList;
            }

        }
       
        /// <summary>
        /// Rule List
        /// </summary>
        virtual public List<FuzzyRule> Rules
        {
            get
            {
                return moRuleList;
            }

        }
        
        /// <summary>
        /// Initial Fact Base
        /// </summary>
        public System.Collections.BitArray InitialFactBase
        {
            get
            {
                return moFbInitial;
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Creaes a Fuzzy Rule Base
        /// </summary>
        /// <param name="vsName">Name of the Rule Base</param>
        public FuzzyRuleBase(string vsName)
        {
            mdAlphaCut = Constants.FuzzyAlphaCutDefault;
            meCorrelationMethod = EnumFuzzyCorrelationMethod.Product;
            meDefuzzifyMethod = EnumFuzzyDefuzzifyMethod.Centroid;
            meInferenceMethod = EnumFuzzyInferenceMethod.Add;
            varId = Constants.FuzzyVarIdInitial;
            moVariableList = new Dictionary<string, FuzzyRuleVariable>();
            ruleId = Constants.FuzzyRuleIdInitial;
            moRuleList = new List<FuzzyRule>(10);
            moCndRuleList = new List<FuzzyRule>(10);
            moUncRuleList = new List<FuzzyRule>(10);
            moFbInitial = new System.Collections.BitArray(Constants.FUZZY_MAXVALUES);
            msName = vsName;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Forward Chain through the Fuzzy Rulebase
        ///   Rules are Reset
        ///   Uncoditional Rules are Fired
        ///   Conditional Rules are Fired
        /// </summary>
        public void ForwardChain()
        {
            // reset all rules, variables keep values set by user

            if (moRuleList != null)
            {
                foreach (FuzzyRule fuzzyRule in moRuleList)
                {
                    fuzzyRule.Reset();
                }
            }


            // The working fact base starts with the fact base
            // from the initial setup.
            System.Collections.BitArray factBase = (System.Collections.BitArray)moFbInitial.Clone();

            factBase.Set(0, true);

            // Process the unconditional rules (assertions).
            ProcessAssertionRules(factBase);

            // Process conditional rules in the fuzzy manner.
            ProcessConditionalRules(factBase);
        }

        /// <summary>
        /// Reset the Variables
        /// </summary>
        public void Reset()
        {
            if (moVariableList != null)
            {
                foreach (FuzzyRuleVariable fuzzyRuleVariable in moVariableList.Values)
                {
                    fuzzyRuleVariable.Reset();
                }
            }

            if (moRuleList != null)
            {
                foreach (FuzzyRule fuzzyRule in moRuleList)
                {
                    fuzzyRule.Reset();
                }
            }
        }

        /// <summary>
        /// Add the Variable
        /// </summary>
        /// <param name="variable">The Variable</param>
        public void AddVariable(FuzzyRuleVariable variable)
        {
            moVariableList[variable.Name] = variable;
        }

        /// <summary>
        /// Get the Variable based on the Name
        /// </summary>
        /// <param name="name">Name of Variable</param>
        /// <returns>FuzzyRuleVariable</returns>
        public FuzzyRuleVariable GetVariable(string name)
        {
            if (moVariableList.ContainsKey(name))
            {
                return moVariableList[name];
            }
            return null;
        }

        /// <summary>
        /// Add a Conditional Rule
        /// </summary>
        /// <param name="rule">The Rule</param>
        public void AddConditionalRule(FuzzyRule rule)
        {
            moRuleList.Add(rule); // add a rule to the rule list
            moCndRuleList.Add(rule);
        }

        /// <summary>
        /// Add an Unconditional Rule
        /// </summary>
        /// <param name="rule">The Rule</param>
        public void AddUnconditionalRule(FuzzyRule rule)
        {
            moRuleList.Add(rule); // add a rule to the rule list
            moUncRuleList.Add(rule);
        }

        /// <summary>
        /// Crate the Clause
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="oper">Operator</param>
        /// <param name="hedges">Hedges</param>
        /// <param name="setName">Set Name</param>
        /// <returns>FuzzyClause</returns>
        public FuzzyClause CreateClause(ContinuousFuzzyRuleVariable lhs, EnumFuzzyOperator oper, string hedges, string setName)
        {

            // Retrieve the fuzzy set object based on hedges and setName
            FuzzySet rhs = GetFuzzySet(lhs, setName, hedges);

            // Create a clause to place into a rule.
            FuzzyClause clause = new FuzzyClause(lhs, oper, rhs);

            return clause;
        }

        /// <summary>
        /// Get the FuzzySet based on the hedges
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="setName">The Set Name that contains the Right Hand Side</param>
        /// <param name="hedges">The Hedge Name that contains the list of hedges</param>
        /// <returns></returns>
        private FuzzySet GetFuzzySet(ContinuousFuzzyRuleVariable lhs, string setName, string hedges)
        {
            FuzzySet rhs = null;
            string sValue = setName.Trim();
            string tmpHedges = hedges.Trim();

            // The value on the Rhs represents a fuzzy set name;
            // The named set must exist.
            if (lhs.SetExist(sValue))
            {
                if (tmpHedges.Length == 0)
                {
                    rhs = lhs.GetSet(sValue);
                }
                else
                {
                    rhs = lhs.GetOrAddHedgedSet(sValue, tmpHedges);
                }
            }
            else
            {
                Console.Out.WriteLine("Error: Invalid fuzzy set name " + sValue);
            }
            return rhs;
        }

        /// <summary>
        /// Clear the RuleBase
        /// </summary>
        public void Clear()
        {

            // Ruleset name
            msName = "";

            // Ruleset options
            mdAlphaCut = Constants.FuzzyAlphaCutDefault;
            meCorrelationMethod = EnumFuzzyCorrelationMethod.Product;
            meDefuzzifyMethod = EnumFuzzyDefuzzifyMethod.Centroid;
            meInferenceMethod = EnumFuzzyInferenceMethod.Add;

            // Lists of variables
            varId = Constants.FuzzyVarIdInitial;
            moVariableList.Clear();

            // Lists of rules          
            ruleId = Constants.FuzzyRuleIdInitial;
            moRuleList.Clear();
            moCndRuleList.Clear();
            moUncRuleList.Clear();
            List<FuzzyRule> temp_fuzzylist;

            // Fact Base
            moFbInitial = new System.Collections.BitArray(Constants.FUZZY_MAXVALUES);
        }
        
        /// <summary>
        /// Fires the rule (if-then)
        /// </summary>
        /// <param name="factBase">The Fact Set</param>
        private void ProcessConditionalRules(System.Collections.BitArray factBase)
        {
            bool moreRules = true;
            List<FuzzyRule> tmpRuleSet = new List<FuzzyRule>(10);

            Console.Out.WriteLine(Environment.NewLine + "Processing conditional fuzzy rules ");
            while (moreRules)
            {
                // Create a rule set:
                // Examine all conditional rules.
                // If a rule has fired, ignore it.
                // If a rule has not fired, and if it can be fired given
                // the current fact base, add it to the rule set.

                if (moCndRuleList != null)
                {
                    foreach (FuzzyRule rule in moCndRuleList)
                    {
                        if (!rule.Fired)
                        {

                            // The rule has not fired; check the rule's antecedents reference
                            System.Collections.BitArray tmpRuleFacts = rule.RdReferences;
                            System.Collections.BitArray tempFacts = rule.RdReferences;

                            tempFacts.And(factBase);

                            // If the antecedents reference variables whose values have been
                            // determined, the rule can fire, so add it to the rule set.
                            if (BitArrayEquality(tempFacts, tmpRuleFacts) == true)
                            {
                                tmpRuleSet.Add(rule);
                            }
                        }
                    }
                }

                // If the rule set is empty, no rules can be fired; exit the loop.
                if ((tmpRuleSet.Count == 0))
                {
                    moreRules = false;
                    break;
                }

                // Okay, we have some rules in the generated rule set;
                // Attempt to fire them all.  Rules that fire successfully
                // update the fact base directly.
                for (int i = 0; i < tmpRuleSet.Count; i++)
                {
                    FuzzyRule rule = (FuzzyRule)tmpRuleSet[i];

                    Console.Out.WriteLine(Environment.NewLine + "Firing fuzzy rule: " + rule.Name);
                    rule.Fire(mdAlphaCut, factBase);
                }

                // We may or may not have fired all the rules in the rule set;
                // In any event, do a purge of the rule set to get ready for the
                // next iteration of the while loop.
                tmpRuleSet.Clear();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Processes the Assertion Rules
        /// </summary>
        /// <param name="factBase"></param>
        private void ProcessAssertionRules(System.Collections.BitArray factBase)
        {
            Console.Out.WriteLine(Environment.NewLine + "Processing unconditional fuzzy rules ");

            if (moUncRuleList != null)
            {
                foreach (FuzzyRule fuzzyRule in moUncRuleList)
                {
                    fuzzyRule.Fire(mdAlphaCut, factBase);
                }
            }
        }

        /// <summary>
        /// Check if BitArrays are Equal
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns>True if they are Equal</returns>
        private bool BitArrayEquality(System.Collections.BitArray b1, System.Collections.BitArray b2)
        {
            if (b1.Length != b2.Length)
                return false;

            bool[] leftArray = new bool[b1.Length];
            bool[] rightArray = new bool[b2.Length];

            for (int i = 0; i < leftArray.Length; i++)
            {
                if (leftArray[i] != rightArray[i])
                    return false;
            }
            return true;
        }
        #endregion
    }
}
