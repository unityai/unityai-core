using UnityAI.Core.Planning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnityAI.Test
{
    
    
    /// <summary>
    ///This is a test class for TaskTest and is intended
    ///to contain all TaskTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TaskTest
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
        ///A test for Decompose
        ///</summary>
        [TestMethod()]
        public void DecomposeTest()
        {
            Task GetPermit = new Task(Predicate.Create("GetPermit"));
            GetPermit.AddPrecondition(Predicate.Create("Land"));
            GetPermit.AddEffect(Predicate.Create("Permit"));
            Task HireBuilder = new Task(Predicate.Create("HireBuilder"));
            HireBuilder.AddEffect(Predicate.Create("Contract"));
            Task Construction = new Task(Predicate.Create("Construction"));
            Construction.AddPrecondition(Predicate.Create("Permit"));
            Construction.AddPrecondition(Predicate.Create("Contract"));
            Construction.AddEffect(Predicate.Create("HouseBuilt"));
            Construction.AddEffect(Predicate.Create("Contract", true));
            Task PayBuilder = new Task(Predicate.Create("PayBuilder"));
            PayBuilder.AddPrecondition(Predicate.Create("Money"));
            PayBuilder.AddPrecondition(Predicate.Create("HouseBuilt"));
            PayBuilder.AddEffect(Predicate.Create("Money", true));
            PayBuilder.AddEffect(Predicate.Create("House"));
            PayBuilder.AddEffect(Predicate.Create("Contract", true));

            Task target = new Task(Predicate.Create("BuildHouse"), GetPermit, HireBuilder, Construction, PayBuilder);
            target.AddPrecondition(Predicate.Create("Land"));
            target.AddEffect(Predicate.Create("House"));

            PartialOrderPlan expected = null; // TODO: Initialize to an appropriate value
            PartialOrderPlan actual;
            actual = target.Decompose();
            Assert.IsTrue(actual != null);
        }
    }
}
