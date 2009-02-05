using System;
using System.Text;
using UnityAI.Core.Planning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnityAI.Test
{
    
    
    /// <summary>
    ///This is a test class for PartialOrderPlannerTest and is intended
    ///to contain all PartialOrderPlannerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PartialOrderPlannerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for PlanOrder
        ///</summary>
        [TestMethod()]
        public void PlanOrderTest()
        {
            Action RightShoe = new Action(new Predicate("RightShoe"));
            RightShoe.AddPrecondition(new Predicate("RightSockOn"));
            RightShoe.AddEffect(new Predicate("RightShoeOn"));
            Action RightSock = new Action(new Predicate("RightSock"));
            RightSock.AddEffect(new Predicate("RightSockOn"));
            Action LeftShoe = new Action(new Predicate("LeftShoe"));
            LeftShoe.AddPrecondition(new Predicate("LeftSockOn"));
            LeftShoe.AddEffect(new Predicate("LeftShoeOn"));
            Action LeftSock = new Action(new Predicate("LeftSock"));
            LeftSock.AddEffect(new Predicate("LeftSockOn"));


            PartialOrderPlanner target = new PartialOrderPlanner(RightShoe, RightSock, LeftShoe, LeftSock);

            IEnumerable<Predicate> voInitialState = null;
            List<Predicate> voGoalState = new List<Predicate>();
            voGoalState.Add(new Predicate("RightShoeOn"));
            voGoalState.Add(new Predicate("LeftShoeOn"));

            PartialOrderPlan plan = target.PlanOrder(voInitialState, voGoalState);
            Assert.IsTrue(plan != null);

            List<Action> theActions = plan.SortedActions;
            foreach(Action action in theActions)
            {
                string sOut = string.Format("{0}", action.Identity.Name);
                Console.Out.WriteLine(sOut);
            }
        }

        /// <summary>
        ///A test for PlanOrder
        ///</summary>
        [TestMethod()]
        public void PlanOrderTestTireProblem()
        {
            Action RemoveSpare = new Action(new Predicate("Remove", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Trunk")));
            RemoveSpare.AddPrecondition(new Predicate("At", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Trunk")));
            RemoveSpare.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Trunk")));
            RemoveSpare.AddEffect(new Predicate("At", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Ground")));

            Action RemoveFlat = new Action(new Predicate("Remove", false, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Axle")));
            RemoveFlat.AddPrecondition(new Predicate("At", false, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Axle")));
            RemoveFlat.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Axle")));
            RemoveFlat.AddEffect(new Predicate("At", false, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Ground")));

            Action PutOnSpare = new Action(new Predicate("PutOn", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Axle")));
            PutOnSpare.AddPrecondition(new Predicate("At", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Ground")));
            PutOnSpare.AddPrecondition(new Predicate("At", true, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Axle")));
            PutOnSpare.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Ground")));
            PutOnSpare.AddEffect(new Predicate("At", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Axle")));

            Action LeaveOvernight = new Action(new Predicate("LeaveOvernight"));
            LeaveOvernight.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Ground")));
            LeaveOvernight.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Axle")));
            LeaveOvernight.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Trunk")));
            LeaveOvernight.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Ground")));
            LeaveOvernight.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Axle")));

            PartialOrderPlanner target = new PartialOrderPlanner(RemoveSpare, RemoveFlat, PutOnSpare, LeaveOvernight);

            List<Predicate> voInitialState = new List<Predicate>();
            voInitialState.Add(new Predicate("At", false, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Axle")));
            voInitialState.Add(new Predicate("At", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Trunk")));
            List<Predicate> voGoalState = new List<Predicate>();
            voGoalState.Add(new Predicate("At", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Axle")));

            PartialOrderPlan plan = target.PlanOrder(voInitialState, voGoalState);
            Assert.IsTrue(plan != null);

            List<Action> theActions = plan.SortedActions;
            foreach (Action action in theActions)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}", action.Identity.Name);
                if (action.Identity.Parameters != null && action.Identity.Parameters.Count > 0)
                {
                    sb.Append("(");
                    foreach(Term t in action.Identity.Parameters)
                    {
                        sb.Append(t.Name + ",");
                    }
                    sb.Length--;
                    sb.Append(")");
                }
                Console.Out.WriteLine(sb.ToString());
            }
        }

        /// <summary>
        ///A test for PlanOrder
        ///</summary>
        [TestMethod()]
        public void PlanOrderTestTireProblemForceBacktrack()
        {
            Action RemoveSpare = new Action(new Predicate("Remove", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Trunk")));
            RemoveSpare.AddPrecondition(new Predicate("At", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Trunk")));
            RemoveSpare.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Trunk")));
            RemoveSpare.AddEffect(new Predicate("At", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Ground")));

            Action RemoveFlat = new Action(new Predicate("Remove", false, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Axle")));
            RemoveFlat.AddPrecondition(new Predicate("At", false, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Axle")));
            RemoveFlat.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Axle")));
            RemoveFlat.AddEffect(new Predicate("At", false, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Ground")));

            Action PutOnSpare = new Action(new Predicate("PutOn", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Axle")));
            PutOnSpare.AddPrecondition(new Predicate("At", true, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Axle")));
            PutOnSpare.AddPrecondition(new Predicate("At", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Ground")));
            PutOnSpare.AddEffect(new Predicate("At", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Axle")));
            PutOnSpare.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Ground")));

            Action LeaveOvernight = new Action(new Predicate("LeaveOvernight"));
            LeaveOvernight.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Ground")));
            LeaveOvernight.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Axle")));
            LeaveOvernight.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Trunk")));
            LeaveOvernight.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Ground")));
            LeaveOvernight.AddEffect(new Predicate("At", true, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Axle")));

            PartialOrderPlanner target = new PartialOrderPlanner(PutOnSpare, RemoveSpare, LeaveOvernight, RemoveFlat);

            List<Predicate> voInitialState = new List<Predicate>();
            voInitialState.Add(new Predicate("At", false, ConstantTerm.CreateTerm("Flat"), ConstantTerm.CreateTerm("Axle")));
            voInitialState.Add(new Predicate("At", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Trunk")));
            List<Predicate> voGoalState = new List<Predicate>();
            voGoalState.Add(new Predicate("At", false, ConstantTerm.CreateTerm("Spare"), ConstantTerm.CreateTerm("Axle")));

            PartialOrderPlan plan = target.PlanOrder(voInitialState, voGoalState);
            Assert.IsTrue(plan != null);

            List<Action> theActions = plan.SortedActions;
            foreach (Action action in theActions)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}", action.Identity.Name);
                if (action.Identity.Parameters != null && action.Identity.Parameters.Count > 0)
                {
                    sb.Append("(");
                    foreach (Term t in action.Identity.Parameters)
                    {
                        sb.Append(t.Name + ",");
                    }
                    sb.Length--;
                    sb.Append(")");
                }
                Console.Out.WriteLine(sb.ToString());
            }
        }
    }
}
