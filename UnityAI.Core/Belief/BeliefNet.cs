//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents the BeliefNet
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;

namespace UnityAI.Core.Belief
{
    public class BeliefNet
    {
        #region Fields
        private BeliefNodeList moNodes = new BeliefNodeList();
        #endregion

        #region Properties
        /// <summary>
        /// Nodes of the Belief Net
        /// </summary>
        public BeliefNodeList Nodes
        {
            get { return moNodes; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads the nodes from Xml
        /// </summary>
        public void LoadXml(string vsFileName)
        {
            XmlTextReader reader = new XmlTextReader(vsFileName);
            while (reader.Read())
            {
                switch (reader.Name)
                {
                    case "Nodes":
                        break;  
                    case "Node":
                        BeliefNode node = new BeliefNode(reader.ReadOuterXml());
                        moNodes.Add(node);
                        break;
                }
            }
        }

        /// <summary>
        /// Updates the Nodes in the Belief Net
        /// </summary>
        /// <returns>True if successfull otherwise false</returns>
        public bool UpdateNodes()
        {
            foreach (BeliefNode bn in moNodes)
            {
                //we can only update nodes that have arcs
                if (bn.Arcs != null)
                {
                    UpdateNode(bn);
                }
            }

            return true;
        }

        /// <summary>
        /// Updates a single node in the Belief Net
        /// </summary>
        /// <param name="voBeliefNode">Belief Node</param>
        /// <returns>True if node is updated, otherwise false</returns>
        public bool UpdateNode(BeliefNode voBeliefNode)
        {
            voBeliefNode.EvaluatedArcExpression = EvaluateArcExpression(voBeliefNode, voBeliefNode.Arcs);
            float fCurrentOdds = voBeliefNode.PriorOdds * voBeliefNode.EvaluatedArcExpression;

            float fCurrentProbability = Utility.CalculateProbability(fCurrentOdds);

            Console.Out.WriteLine(string.Format("Updated value of node {0} = {1}\n", voBeliefNode.Name, fCurrentProbability));

            //voBeliefNode.PriorProbability = voBeliefNode.CurrentProbability;          
            voBeliefNode.CurrentProbability = fCurrentProbability;

            return true;
        }

        /// <summary>
        /// For a single Arc, this evalulate the expressions
        /// </summary>
        /// <param name="voArcs">Arcs</param>
        /// <returns>Arc Expression</returns>
        public float EvaluateArcExpression(BeliefNode voBeliefNode, Arcs voArcs)
        {
            switch (voArcs.ArcType)
            {
                case EnumArcType.Independent :
                    return CalculateCombinedIndependent(voBeliefNode, voArcs);
                case EnumArcType.Conjunctive:
                    return CalculateCombinedConjunctive(voBeliefNode, voArcs);
                case EnumArcType.Disjunctive :
                    return CalculateCombinedDisjunctive(voBeliefNode, voArcs);
                default:
                    throw new Exception(string.Format("Invalid arc type {0}",voArcs.ArcType.ToString()));
            }
        }

        /// <summary>
        /// Returnes Updates for the Probability 
        /// </summary>
        /// <param name="voBeliefNode">The BeliefNode</param>
        /// <param name="voArc">Arc to Evaluate</param>
        /// <returns>Updated Probability</returns>
        public float UpdateProbability(BeliefNode voBeliefNode, Arc voArc)
        {
            BeliefNode arcNode = moNodes.GetBelief(voArc.Name);
            float fUpdatedValue = 0.0f;
            if (arcNode.CurrentProbability > arcNode.PriorProbability)
            {
                DebugProgress(voBeliefNode,voArc,arcNode,"Supporting arc");
                fUpdatedValue = (float)(voBeliefNode.PriorProbability +
                    ((Utility.CalculateProbability(voArc.Sufficiency * voBeliefNode.PriorOdds) - voBeliefNode.PriorProbability)
                    /
                    (1.0 - arcNode.PriorProbability) * (arcNode.CurrentProbability - arcNode.PriorProbability)));
                            
            }
            else
            {
                DebugProgress(voBeliefNode, voArc, arcNode, "Inhibiting arc");
                fUpdatedValue = 
                    ((Utility.CalculateProbability(voArc.Neccessity * voBeliefNode.PriorOdds)) +
                    (voBeliefNode.PriorProbability - Utility.CalculateProbability(voArc.Neccessity * voBeliefNode.PriorOdds))
                    /
                    arcNode.PriorProbability
                    *
                    arcNode.CurrentProbability);                    
            }
            return fUpdatedValue;
        }

        /// <summary>
        /// Debug Process, make sure values line up with LISP code
        /// </summary>
        /// <param name="voNode">Node we are updating</param>
        /// <param name="voArc">current arc</param>
        /// <param name="voArcNode">the arc node</param>
        /// <param name="vsProgress">message</param>
        public void DebugProgress(BeliefNode voNode, Arc voArc, BeliefNode voArcNode,string vsProgress)
        {
            Console.Out.WriteLine(string.Format("{0} update of {1} along arc ({2} {3} {4}) with prior odds {5}", vsProgress, voNode.Name, voArc.Name, voArc.Sufficiency, voArc.Neccessity, voNode.PriorOdds));
            Console.Out.WriteLine(string.Format("The prior and current prob of E are {0}, {1}", voArcNode.PriorProbability, voArcNode.CurrentProbability));
        }

        /// <summary>
        /// Calcualtes the Effective Arc Lambda
        /// </summary>
        /// <param name="voArc">Single Arc</param>
        /// <returns>Calculate Lambda</returns>
        public float CalculateEffectiveArcLambda(BeliefNode voBeliefNode, Arc voArc)
        {
            return Utility.CalculateOdds(UpdateProbability(voBeliefNode, voArc)) / voBeliefNode.PriorOdds;            
        }

        /// <summary>
        /// For the List of Arc Calculate the Combined Independent value
        /// </summary>
        /// <param name="voArcs">List of Arc</param>
        /// <returns>Calculated Lambda</returns>
        public float CalculateCombinedIndependent(BeliefNode voBeliefNode, Arcs voArcs)
        {
            float fCombined = 1.0f;
            if (voArcs.InnerArcs != null)
            {
                foreach (Arcs arcs in voArcs.InnerArcs)
                {
                    fCombined *= EvaluateArcExpression(voBeliefNode, arcs);
                }
            }
            foreach (Arc arc in voArcs.ArcList)
            {
                fCombined *= CalculateEffectiveArcLambda(voBeliefNode, arc);                
            }
            return fCombined;
        }

        /// <summary>
        /// For the List of Arc Calculate the Conjunctive value (min)
        /// </summary>
        /// <param name="voArcs">List of Arc</param>
        /// <returns>Calculated Lambda</returns>
        public float CalculateCombinedConjunctive(BeliefNode voBeliefNode, Arcs voArcs)
        {
            float fValue = float.MaxValue;

            if (voArcs.InnerArcs != null)
            {
                foreach (Arcs arcs in voArcs.InnerArcs)
                {
                    float fTest = EvaluateArcExpression(voBeliefNode, arcs);
                    fValue = (fValue <= fTest ? fValue : fTest);                
                }
            }
            foreach (Arc arc in voArcs.ArcList)
            {
                float fTest = CalculateEffectiveArcLambda(voBeliefNode, arc);
                fValue = (fValue <= fTest ? fValue : fTest);                
            }
            return fValue;
        }

        /// <summary>
        /// For the List of Arc Calculate the Disjunctive value (max)
        /// </summary>
        /// <param name="voArcs">List of Arc</param>
        /// <returns>Calculated Lambda</returns>
        public float CalculateCombinedDisjunctive(BeliefNode voBeliefNode, Arcs voArcs)
        {
            float fValue = float.MinValue;

            if (voArcs.InnerArcs != null)
            {
                foreach (Arcs arcs in voArcs.InnerArcs)
                {
                    float fTest = EvaluateArcExpression(voBeliefNode, arcs);
                    fValue = (fValue >= fTest ? fValue : fTest);
                }
            }           

            foreach (Arc arc in voArcs.ArcList)
            {
                float fTest = CalculateEffectiveArcLambda(voBeliefNode, arc);
                fValue = (fValue >= fTest ? fValue : fTest);
            }
            return fValue;
        }
        #endregion
    }
}
