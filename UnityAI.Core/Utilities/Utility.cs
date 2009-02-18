//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Utility
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core
{
    public class Utility
    {
        /// <summary>
        /// Given the Probability Calculate the Odds
        /// </summary>
        /// <param name="voProbability">Probability to Calculate Odds from</param>
        /// <returns>Odds</returns>
        public static float CalculateOdds(float voProbability)
        {
            return voProbability / (1.0f - voProbability);
        }

        /// <summary>
        /// Given the Odds Calculate the Probability
        /// </summary>
        /// <param name="voOdds">Odds to Calculate Probablity from</param>
        /// <returns>Probability</returns>
        public static float CalculateProbability(float voOdds)
        {
            return voOdds / (1.0f + voOdds);
        }
    }
}
