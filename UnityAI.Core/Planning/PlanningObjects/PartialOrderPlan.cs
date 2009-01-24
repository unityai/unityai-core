//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a Partial Order Plan in a Partial Order 
//                Plan
//
// Modification Notes:
// Date		Author        	Notes
// -------- ------          -----------------------------------------
// 01/26/09	SMcCarthy		Initial Implementation
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core
{
    public class PartialOrderPlan
    {
        #region Fields
        private Dictionary<string, Predicate> moInitialState;
        private Dictionary<string, Predicate> moGoalState;
        private Action moStartAction;
        private Action moFinishAction;
        private Dictionary<string, Action> moActions;
        private Dictionary<string, OrderingConstraint> moOrderingConstraints;
        private Dictionary<string, CasualLink> moCasualLinks;
        private Dictionary<string, Predicate> moOpenPreconditions;
        #endregion
    }
}
