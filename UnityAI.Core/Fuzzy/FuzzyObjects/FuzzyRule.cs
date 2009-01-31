//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   FuzzyRule
//                From Constructing Intelligent Agents using Java
//
// Modification Notes:
// Date		Author        	Notes
// -------- ------          -----------------------------------------
// 01/26/09	SMcCarthy		Initial Implementation
//-------------------------------------------------------------------
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace UnityAI.Core.Fuzzy
{
    [Serializable]
    public class FuzzyRule
    {
        #region Fields
        private FuzzyRuleBase moRuleBase; // Fuzzy rule base
        private string msName; // Name or label of this rule
        private System.Collections.BitArray moRdRefs; // Variables referenced in this rule
        private System.Collections.BitArray moWrRefs; // Variable set by this rule
        private bool mbFiredFlag; // Whether the rule has fired
        private List<FuzzyClause> moAntecedents; // Antecedent clauses
        private FuzzyClause moConsequent; // Consequent clause
        #endregion

        #region Properties
        
        /// <summary>
        /// Name of this Rule
        /// </summary>
        virtual public string Name
        {
            get
            {
                return msName;
            }

        }
        
        /// <summary>
        /// Variables references by this rule
        /// </summary>
        virtual public System.Collections.BitArray RdReferences
        {
            get
            {
                return (System.Collections.BitArray)moRdRefs.Clone();
            }

        }
        
        /// <summary>
        /// Variables Set by this Rule
        /// </summary>
        virtual public System.Collections.BitArray WrReferences
        {
            get
            {
                return (System.Collections.BitArray)moWrRefs.Clone();
            }

        }
        
        /// <summary>
        /// Is this rule Fired?
        /// </summary>
        virtual public bool Fired
        {
            get
            {
                return mbFiredFlag;
            }

        }

        /// <summary>
        /// The Antecedents of the this Rule
        /// </summary>
        virtual public List<FuzzyClause> Antecedents
        {
            get
            {
                return moAntecedents;
            }

        }
        
        /// <summary>
        /// Consequents of this Rule
        /// </summary>
        virtual public FuzzyClause Consequent
        {
            get
            {
                return moConsequent;
            }

        }
        #endregion

        #region Constructor
       
        /// <summary>
        /// Create a new Fuzzy Rule
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="name"></param>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        internal FuzzyRule(FuzzyRuleBase rb, string name, FuzzyClause lhs, FuzzyClause rhs)
        {
            moRuleBase = rb;
            msName = name;
            moRdRefs = new System.Collections.BitArray(128);
            moWrRefs = new System.Collections.BitArray(128);
            mbFiredFlag = false;
            moAntecedents = new List<FuzzyClause>(10);
            moAntecedents.Add(lhs);
            moConsequent = rhs;
            if (moConsequent != null)
            {
                AddWrReference(moConsequent.LhsReferent);
                AddRdReference(moConsequent.RhsReferent);
            }
            moRuleBase.AddConditionalRule(this); // add self to rule list
        }


        /// <summary>
        /// Create a new Fuzzy Rule
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="name"></param>
        /// <param name="lhsClauses"></param>
        /// <param name="rhs"></param>
        public FuzzyRule(FuzzyRuleBase rb, string name, FuzzyClause[] lhsClauses, FuzzyClause rhs)
        {
            moRuleBase = rb;
            msName = name;
            moRdRefs = new System.Collections.BitArray(128);
            moWrRefs = new System.Collections.BitArray(128);
            mbFiredFlag = false;
            moAntecedents = new List<FuzzyClause>(10);
            for (int i = 0; i < lhsClauses.Length; i++)
            {
                moAntecedents.Add(lhsClauses[i]);
            }
            moConsequent = rhs;
            if (moConsequent != null)
            {
                AddWrReference(moConsequent.LhsReferent);
                AddRdReference(moConsequent.RhsReferent);
            }
            moRuleBase.AddConditionalRule(this); // add self to rule list
        }


        /// <summary>
        /// Create a new Fuzzy Rule
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="name"></param>
        /// <param name="rhs"></param>
        internal FuzzyRule(FuzzyRuleBase rb, string name, FuzzyClause rhs)
        {
            moRuleBase = rb;
            msName = name;
            moRdRefs = new System.Collections.BitArray(128);
            moWrRefs = new System.Collections.BitArray(128);
            mbFiredFlag = false;
            moAntecedents = new List<FuzzyClause>(10);
            moConsequent = rhs;
            if (moConsequent != null)
            {
                AddWrReference(moConsequent.LhsReferent);
                AddRdReference(moConsequent.RhsReferent);
            }
            moRuleBase.AddUnconditionalRule(this); // add self to rule list
        }
        #endregion

        #region Methods
        
        /// <summary>
        /// Adds to variables references by this rule
        /// </summary>
        /// <param name="id"></param>
        internal virtual void AddRdReference(int id)
        {
            moRdRefs.Set(id, true);
        }


        /// <summary>
        /// Adds to variables set by this rule
        /// </summary>
        /// <param name="id"></param>
        internal virtual void AddWrReference(int id)
        {
            moWrRefs.Set(id, true);
        }


        /// <summary>
        /// Resets the Rule
        /// </summary>
        internal virtual void Reset()
        {
            mbFiredFlag = false;
        }


        /// <summary>
        /// <p>
        /// If this is an unconditional rule (there are no antecedents),
        /// simply evaluates the consequent for its side-effects.
        /// <p>
        /// If this is a conditional rule (there are antecedents),
        /// evaluates the antecedents:
        /// <ul>
        /// <li> If the value of an antecedent clause is below the alphacut
        /// threshold, the rule does not fire.
        /// <li> Keep track of the minimum truthvalue returned when evaluating
        /// each antecedent clause.
        /// <li> Pass the minimum to the correlation method ('eval' on the
        /// consequent clause).
        /// <li> Update working memory to show that the variable in the
        /// consequent clause has changed.
        /// </ul>
        /// </summary>
        /// <param name="alphaCut"></param>
        /// <param name="workingSet"></param>
        internal virtual void Fire(double alphaCut, System.Collections.BitArray workingSet)
        {

            // (If there are no antecedents or a consequent, serious error!)
            if ((moAntecedents.Count == 0))
            {
                // unconditional rule
                if (moConsequent != null)
                {
                    moConsequent.Evaluate();
                    workingSet.Or(moWrRefs);
                    return;
                }
                else
                {
                    // If there are no antecedents, there must at least be a consequent!
                    Console.WriteLine("Error: FuzzyRule cannot fire" + msName);
                }
            }

            // This is a conditional rule (there are antecedents),
            FuzzyClause clause;
            double truthValue;
            double truthValueMin = 1.0;
            bool skipConsequent = false;

            for (int i = 0; i < moAntecedents.Count; i++)
            {

                // Get an antecedent clause and evaluate it.
                clause = (FuzzyClause)(moAntecedents[i]);
                truthValue = clause.Evaluate();
                mbFiredFlag = true;

                // Check the truth value against the alphacut threshold.
                // If the truth value is less than the threshold
                // stop evaluating the antecedent clauses and
                // prevent the consequent from being evaluated.
                if (truthValue <= alphaCut)
                {
                    skipConsequent = true;
                    break;
                }

                // Keep track of the smallest truth value from each
                // evaluated antecedent clause.
                if (truthValue < truthValueMin)
                {
                    truthValueMin = truthValue;
                }
            }

            // If all the antecedent clauses evaluated successfully,
            // evaluate the consequent clause, and if that is evaluated
            // successfully, update the fact base.
            if (!skipConsequent)
            {
                if (moConsequent != null)
                {
                    moConsequent.Evaluate(truthValueMin);
                    workingSet.Or(moWrRefs);
                }
            }
        }

        /// <summary>
        /// String Value of this Rule
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string tmpStr = "";

            if (moAntecedents.Count == 0)
            {
                //  unconditional rule (fact)
                tmpStr = msName + ": " + moConsequent.ToString();
            }
            else
            {
                // conditional rule
                tmpStr = msName + ": IF ";
                foreach (FuzzyClause fuzzyClause in moAntecedents)
                {
                    tmpStr += fuzzyClause.ToString() + Environment.NewLine + "     AND ";
                }
                tmpStr = Regex.Replace(tmpStr, "AND $", "", RegexOptions.IgnoreCase);

                tmpStr += Environment.NewLine + "     THEN " + moConsequent.ToString();
            }
            return tmpStr;
        }
        #endregion
    }
}