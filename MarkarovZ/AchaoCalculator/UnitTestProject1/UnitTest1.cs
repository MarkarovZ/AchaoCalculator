using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AchaoCalculator;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int n = 2;
            Program program = new Program();
            program.FormulaGeneration(n);
            foreach(var res in Program.answers)//对算式结果的测试，确保没有不符合要求的算式
            {
                Assert.AreEqual(res%1, 0);
            }
        }
    }
}
