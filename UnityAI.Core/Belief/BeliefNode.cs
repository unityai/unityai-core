//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents the BeliefNode
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace UnityAI.Core.Belief
{
    public class BeliefNode
    {
        #region Fields
        private string msName = null;
        private float mfPriorProbability = 0.0f;
        private float mfCurrentProbability = 0.0f;
        private Arcs moArcs = null;
        private float mfEvaluatedArcExpression;
        #endregion

        #region Properties

        /// <summary>
        /// The evaluated Arc expression
        /// </summary>
        public float EvaluatedArcExpression
        {
            get { return mfEvaluatedArcExpression; }
            set { mfEvaluatedArcExpression = value; }
        }
        /// <summary>
        /// Name of the Node
        /// </summary>
        public string Name
        {
            get { return msName; }
            set { msName = value; }
        }

        /// <summary>
        /// The Prior Probability
        /// </summary>
        public float PriorProbability
        {
            get { return mfPriorProbability; }
            set { mfPriorProbability = value; }
        }

        /// <summary>
        /// The Prior Odds
        /// </summary>
        public float PriorOdds
        {
            get { return Utility.CalculateOdds(mfPriorProbability); }
        }

        /// <summary>
        /// The Current Probability
        /// </summary>
        public float CurrentProbability
        {
            get { return mfCurrentProbability; }
            set { mfCurrentProbability = value; }
        }

        /// <summary>
        /// The Current Odds
        /// </summary>
        public float CurrentOdds
        {
            get { return Utility.CalculateOdds(mfCurrentProbability); }          
        }

        /// <summary>
        /// Arcs represented in this Belief Node
        /// </summary>
        public Arcs Arcs
        {
            get { return moArcs; }
            set { moArcs = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Construct the Belief Node given the input Xml
        /// </summary>
        /// <param name="vsXml">Xml Data</param>
        public BeliefNode(string vsXml)
        {
            XmlTextReader reader = new XmlTextReader(new StringReader(vsXml));
            reader.Read();

            Name = reader.GetAttribute("name");
            PriorProbability = float.Parse(reader.GetAttribute("priorprob"));
            CurrentProbability = float.Parse(reader.GetAttribute("currentprob"));

            while(reader.Read())
            {
                switch (reader.Name)
                {
                    case "Arcs" :
                        moArcs = new Arcs(reader.ReadOuterXml());
                        break;
                }
            }
        }

        #endregion
    }
}
