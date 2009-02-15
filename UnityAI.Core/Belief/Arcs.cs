//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents the Arcs in the BeliefNet
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
    public class Arcs
    {
        #region Fields
        private EnumArcType meArcType = EnumArcType.Unknown;
        private List<Arc> moArcList = null;
        private List<Arcs> moInnerArcs = null;
        #endregion

        #region Properties

        /// <summary>
        /// The Arc type
        /// </summary>
        public EnumArcType ArcType
        {
            get { return meArcType; }
            set { meArcType = value; }
        }

        /// <summary>
        /// Arcs represents by the type
        /// </summary>
        public List<Arc> ArcList
        {
            get { return moArcList; }
            set { moArcList = value; }
        }

        /// <summary>
        /// Next Arc to support nesting
        /// </summary>
        public List<Arcs> InnerArcs
        {
            get { return moInnerArcs; }
            set { moInnerArcs = value; }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Constructs the Arcs using Xml
        /// </summary>
        /// <param name="vsXml">Xml data</param>
        public Arcs(string vsXml)
        {
            XmlTextReader reader = new XmlTextReader(new StringReader(vsXml));
            reader.Read();

            ArcType = (EnumArcType)Enum.Parse(typeof(EnumArcType), reader.GetAttribute("arctype"));

            moArcList = new List<Arc>();

            while (reader.Read())
            {
                switch (reader.Name)
                {
                    case "Arcs" :
                        if (reader.NodeType != XmlNodeType.EndElement)
                        {
                            if (moInnerArcs == null)
                            {
                                moInnerArcs = new List<Arcs>();
                            }
                            moInnerArcs.Add(new Arcs(reader.ReadOuterXml()));
                        }
                        break;
                    case "Arc" :
                        moArcList.Add(new Arc(reader.ReadOuterXml()));
                        break;
                }
            }
        }

        #endregion
    }
}
