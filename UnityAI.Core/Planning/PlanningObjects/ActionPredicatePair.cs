//-------------------------------------------------------------------
// (c) Copyright 2009  UnityAI Core Team
// Developed For:  UnityAI
// License: Artistic License 2.0
//
// Description:   Represents a ActionPredicatePair that is a 2-Tuple
//
// Authors: SMcCarthy
//-------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAI.Core.Planning
{
    public class ActionPredicatePair : Pair<Action, Predicate>
    {
        #region Properties
        /// <summary>
        /// First Part of the ActionPredicatePair
        /// </summary>
        public Action Action
        {
            get { return First;  }
            set { First = value; }
        }
        /// <summary>
        /// First Part of the ActionPredicatePair
        /// </summary>
        public Predicate Predicate   
        {
            get { return Second; }
            set { Second = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Construct a ActionPredicatePair
        /// </summary>
        /// <param name="a">First</param>
        /// <param name="b">Second</param>
        public ActionPredicatePair(Action a, Predicate b)
            : base(a,b)
        {
        }
        #endregion
    }

    public class Pair<A, B>
    {
        #region Fields
        private A moFirst;
        private B moSecond;
        #endregion

        #region Properties
        /// <summary>
        /// First Part of the ActionPredicatePair
        /// </summary>
        public A First
        {
            get { return moFirst; }
            set { moFirst = value; }
        }
        /// <summary>
        /// First Part of the ActionPredicatePair
        /// </summary>
        public B Second
        {
            get { return moSecond; }
            set { moSecond = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Construct a ActionPredicatePair
        /// </summary>
        /// <param name="a">First</param>
        /// <param name="b">Second</param>
        public Pair(A a, B b)
        {
            moFirst = a;
            moSecond = b;
        }
        #endregion
    }
}
