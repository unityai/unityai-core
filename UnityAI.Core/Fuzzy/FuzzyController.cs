//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   FuzzyController 
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace UnityAI.Core.Fuzzy
{
    [Serializable]
    public class FuzzyController
    {
        #region Fields
        private FuzzyRuleBase rb = null;
        #endregion

        #region Properties
        public FuzzyRuleBase FuzzyRules
        {
            get { return rb; }
        }
        #endregion

        #region Contructors
        public FuzzyController()
        {
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
                    case "RuleBase":
                        if (reader.NodeType != XmlNodeType.EndElement)
                        {
                            CreateRulesBase(reader);
                        }
                        break;
                    case "ContinuousRuleVariable":
                        CreateContinuousRuleVariable(reader.ReadOuterXml());                                                
                        break;
                    case "Rule":
                        //reader.ReadOuterXml();
                        CreateRules(reader.ReadOuterXml());
                        break;
                }
            }
            reader.Close();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Create the Rule Base
        /// </summary>
        /// <param name="reader"></param>
        private void CreateRulesBase(XmlTextReader reader)
        {
            string sRuleName = reader.GetAttribute("name");
            //1 product, 2 minimize
            EnumFuzzyCorrelationMethod eCorrelation = (EnumFuzzyCorrelationMethod)int.Parse(reader.GetAttribute("correlation"));
            //1 fuzzy add, 2 minmax
            EnumFuzzyInferenceMethod eInference = (EnumFuzzyInferenceMethod)int.Parse(reader.GetAttribute("inference"));
            //1 centroid, 2 maxheight
            EnumFuzzyDefuzzifyMethod eDefuzzify = (EnumFuzzyDefuzzifyMethod)int.Parse(reader.GetAttribute("defuzzify"));

            rb = new FuzzyRuleBase(sRuleName);
            rb.AlphaCut = 0.01;
            rb.CorrelationMethod = eCorrelation;
            rb.InferenceMethod = eInference;
            rb.DefuzzifyMethod = eDefuzzify;
        }

        /// <summary>
        /// Create a ContinuousRuleVariable
        /// </summary>
        /// <param name="vsXml"></param>
        private void CreateContinuousRuleVariable(string vsXml)
        {
            XmlTextReader reader = new XmlTextReader(new StringReader(vsXml));
            reader.Read();

            string sName = reader.GetAttribute("name");
            float fStart = float.Parse(reader.GetAttribute("start"));
            float fEnd = float.Parse(reader.GetAttribute("end"));
            ContinuousFuzzyRuleVariable fuzzyVariable = new ContinuousFuzzyRuleVariable(rb, sName, fStart, fEnd);

            while (reader.Read())
            {
                switch (reader.Name)
                {
                    case "Shoulder":
                        CreateShoulder(fuzzyVariable, reader.ReadOuterXml());
                        break;
                    case "Triangle":
                        CreateTriangle(fuzzyVariable, reader.ReadOuterXml());
                        break;
                    case "Trapezoid":
                        CreateTrapezoid(fuzzyVariable, reader.ReadOuterXml());
                        break;
                }
            }
            reader.Close();
        }

        /// <summary>
        /// Create a Shoulder
        /// </summary>
        /// <param name="vFuzzyVar"></param>
        /// <param name="vsXml"></param>
        private void CreateShoulder(ContinuousFuzzyRuleVariable vFuzzyVar, string vsXml)
        {
            XmlTextReader reader = new XmlTextReader(new StringReader(vsXml));
            reader.Read();

            string sName = reader.GetAttribute("name");
            float fAlpha = float.Parse(reader.GetAttribute("alpha"));
            float fStart = float.Parse(reader.GetAttribute("start"));
            float fEnd = float.Parse(reader.GetAttribute("end"));
            //left = 1, right = 2
            EnumFuzzySetDirection eType = (EnumFuzzySetDirection)int.Parse(reader.GetAttribute("type"));
            vFuzzyVar.AddSetShoulder(sName, fAlpha, fStart, fEnd, eType);
            reader.Close();
        }

        /// <summary>
        /// Create a Triangle
        /// </summary>
        /// <param name="vFuzzyVar"></param>
        /// <param name="vsXml"></param>
        private void CreateTriangle(ContinuousFuzzyRuleVariable vFuzzyVar, string vsXml)
        {
            XmlTextReader reader = new XmlTextReader(new StringReader(vsXml));
            reader.Read();

            string sName = reader.GetAttribute("name");
            float fAlpha = float.Parse(reader.GetAttribute("alpha"));
            float fLeft = float.Parse(reader.GetAttribute("left"));
            float fCenter = float.Parse(reader.GetAttribute("center"));
            float fRight = float.Parse(reader.GetAttribute("right"));

            vFuzzyVar.AddSetTriangle(sName, fAlpha, fLeft, fCenter, fRight);
            reader.Close();
        }

        /// <summary>
        /// Create a Trapezoid
        /// </summary>
        /// <param name="vFuzzyVar"></param>
        /// <param name="vsXml"></param>
        private void CreateTrapezoid(ContinuousFuzzyRuleVariable vFuzzyVar, string vsXml)
        {
            XmlTextReader reader = new XmlTextReader(new StringReader(vsXml));
            reader.Read();

            string sName = reader.GetAttribute("name");
            float fAlpha = float.Parse(reader.GetAttribute("alpha"));
            float fLeft = float.Parse(reader.GetAttribute("left"));
            float fLeftCore = float.Parse(reader.GetAttribute("leftcore"));           
            float fRight = float.Parse(reader.GetAttribute("right"));
            float fRightCore = float.Parse(reader.GetAttribute("rightcore"));

            vFuzzyVar.AddSetTrapezoid(sName, fAlpha, fLeft, fLeftCore, fRightCore, fRight);
            reader.Close();
        }

        /// <summary>
        /// Create Rules
        /// </summary>
        /// <param name="vsXml"></param>
        private void CreateRules(string vsXml)
        {
            XmlTextReader reader = new XmlTextReader(new StringReader(vsXml));
            reader.Read();

            string sName = reader.GetAttribute("name");

            List<FuzzyClause> oClause = new List<FuzzyClause>();

            FuzzyClause assignClause = null;
            while (reader.Read())
            {
                switch (reader.Name)
                {
                    case "CompareClause":
                        oClause.Add(GetClause(reader.ReadOuterXml()));
                        break;
                    case "AssignClause":
                        assignClause = GetClause(reader.ReadOuterXml());
                        break;
                }
            }

            FuzzyRule rule = new FuzzyRule(rb, sName, oClause.ToArray(), assignClause);
            reader.Close();
        }

        /// <summary>
        /// Create GetClause
        /// </summary>
        /// <param name="vsXml"></param>
        /// <returns></returns>
        private FuzzyClause GetClause(string vsXml)
        {
            XmlTextReader reader = new XmlTextReader(new StringReader(vsXml));
            reader.Read();

            string sVariableName = reader.GetAttribute("variablename");
            EnumFuzzyOperator eCompare = (EnumFuzzyOperator)int.Parse(reader.GetAttribute("condition"));
            string sHedge = reader.GetAttribute("hedge");
            string sSet = reader.GetAttribute("setname");

            ContinuousFuzzyRuleVariable fVar = rb.GetVariable(sVariableName) as ContinuousFuzzyRuleVariable;
            reader.Close();
            return rb.CreateClause(fVar, eCompare, sHedge, sSet);
        }
        #endregion
    }
}
