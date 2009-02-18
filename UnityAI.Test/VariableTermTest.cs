using System;
using System.Text;
using System.Collections.Generic;
using UnityAI.Core;
using UnityAI.Core.Planning;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnityAI.Test
{
    /// <summary>
    /// Test class for VariableTerm objects
    /// </summary>
    [TestClass]
    public class VariableTermTest
    {
        #region Constructors and initializers
        public VariableTermTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        #endregion

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestCache()
        {
            String name = "A variable Term";
            VariableTerm<String> term = VariableTerm<String>.FindTerm(name, "PeterRanAway");
            Assert.IsNull(term);
            term = VariableTerm<String>.Create(name, "PeterRanAway");
            Assert.IsNotNull(term);
            VariableTerm<String> term2  = VariableTerm<String>.FindTerm(name, "PeterRanAway");
            Assert.IsNotNull(term2);
            Assert.AreEqual<Term>(term, term2);
        }
    }
}
