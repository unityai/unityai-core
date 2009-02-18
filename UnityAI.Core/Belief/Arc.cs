//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents an Arc in the BeliefNet
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
    /// <summary>
    /// Represents the different type of arcs
    /// </summary>
    public enum EnumArcType : int
    {
        Unknown,
        Independent,
        Conjunctive,
        Disjunctive,
    }

    /// <summary>
    /// Represents a single Arc in Bayes Net
    /// </summary>
    public class Arc
    {
        #region Fields
        private string msName = null;
        private float mfSufficiency = 0.0f;
        private float mfNeccessity = 0.0f;
        #endregion

        #region Properties
        /// <summary>
        /// Represents the Name of the Arc
        /// </summary>
        public string Name
        {
            get { return msName; }
            set { msName = value; }
        }

        /// <summary>
        /// Represents the Sufficiency of the Arc
        /// </summary>
        public float Sufficiency
        {
            get { return mfSufficiency; }
            set { mfSufficiency = value; }
        }

        /// <summary>
        /// Represents the Neccessity of the Arc
        /// </summary>
        public float Neccessity
        {
            get { return mfNeccessity; }
            set { mfNeccessity = value; }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Create an Arc given input xml
        /// </summary>
        /// <param name="vsXml">Xml Data</param>
        public Arc(string vsXml)
        {
            XmlTextReader reader = new XmlTextReader(new StringReader(vsXml));
            reader.Read();

            Name = reader.GetAttribute("name");
            Sufficiency = float.Parse(reader.GetAttribute("sufficiency"));
            Neccessity = float.Parse(reader.GetAttribute("neccessity"));
        }

        #endregion
    }
}
