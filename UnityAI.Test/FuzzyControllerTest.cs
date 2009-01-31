using System;
using UnityAI.Core.Fuzzy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
namespace UnityAI.Test
{
    
    
    /// <summary>
    ///This is a test class for FuzzyControllerTest and is intended
    ///to contain all FuzzyControllerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FuzzyControllerTest
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
        ///A test for LoadXml
        ///</summary>
        [TestMethod()]
        public void FuzzyTest()
        {
            FuzzyController target = new FuzzyController(); // TODO: Initialize to an appropriate value
            string vsFileName = "rules.xml"; // TODO: Initialize to an appropriate value
            target.LoadXml("..\\..\\..\\UnityAI.Test\\" + vsFileName);
            target.FuzzyRules.Reset();
            ContinuousFuzzyRuleVariable view = target.FuzzyRules.GetVariable("view") as ContinuousFuzzyRuleVariable;
            ContinuousFuzzyRuleVariable quadrant = target.FuzzyRules.GetVariable("quadrant") as ContinuousFuzzyRuleVariable;
            //view.SetNumericValue(3.0543);
            //quadrant.SetNumericValue(5.5850);
            view.SetNumericValue(0.52359);
            quadrant.SetNumericValue(0.52359); 
            target.FuzzyRules.ForwardChain();

            ContinuousFuzzyRuleVariable action = target.FuzzyRules.GetVariable("action") as ContinuousFuzzyRuleVariable;

            foreach(FuzzyRuleVariable variable in target.FuzzyRules.Variables.Values)
            {
                Console.Out.WriteLine(variable.ToString() +"=" + variable.GetNumericValue());
            }
        }

        //Angle (Between Direction and Ship)	Quadrant	Result
        //30 degrees (.52359)	30 degrees (.52359) (Quad I)	29.6875 (LEFT)
        //30 degrees (.52359)	100 degrees (1.7453)  (Quad II)	49.609375 (RIGHT)
        //30 degrees (.52359)	190 degrees (3.3161)  (Quad III)	49.609375 (RIGHT)
        //30 degrees (.52359)	320 degrees (5.5850) (Quad IV)	29.6875 (LEFT)
        //60 degrees (1.0471)	30 degrees (.52359) (Quad I)	29.6875 (LEFT)
        //60 degrees (1.0471)	100 degrees (1.7453)  (Quad II)	49.609375 (RIGHT)
        //60 degrees (1.0471)	210 degrees (3.6651)  (Quad III)	49.609375 (RIGHT)
        //60 degrees (1.0471)	320 degrees (5.5850) (Quad IV)	29.6875 (LEFT)
        //90 degrees (1.5707)	30 degrees (.52359) (Quad I)	29.6875 (LEFT)
        //90 degrees (1.5707)	130 degrees (2.2689) (Quad II)	49.609375 (RIGHT)
        //90 degrees (1.5707)	210 degrees (3.6651)  (Quad III)	49.609375 (RIGHT)
        //90 degrees (1.5707)	320 degrees (5.5850) (Quad IV)	29.6875 (LEFT)
        //120 degrees (2.0943)	30 degrees (.52359) (Quad I)	29.6875 (LEFT)
        //120 degrees (2.0943)	160 degrees (2.7925)  (Quad II)	49.609375 (RIGHT)
        //120 degrees (2.0943)	210 degrees (3.6651)  (Quad III)	49.609375 (RIGHT)
        //120 degrees (2.0943)	320 degrees (5.5850) (Quad IV)	29.6875 (LEFT)
        //175 degrees (3.0543)	30 degrees (.52359) (Quad I)	29.6875 (LEFT)
        //175 degrees (3.0543)	160 degrees (2.7925)  (Quad II)	49.609375 (RIGHT)
        //175 degrees (3.0543)	210 degrees (3.6651)  (Quad III)	49.609375 (RIGHT)
        //175 degrees (3.0543)	320 degrees (5.5850) (Quad IV)	29.6875 (LEFT)

    }
}
