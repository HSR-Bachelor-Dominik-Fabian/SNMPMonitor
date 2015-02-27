using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SNMPMonitor.PresentationLayer.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            bool expected = true;
            bool actual = true;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethod2()
        {
            double result = Math.Cos(Math.PI);
            double expected = -1;
            Assert.AreEqual(expected, result);
        }
    }
}
