using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests_Unitaire
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            bool result = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.IsCdr("1");
            bool result2 = CookingCore_Alexandre_FORESTIER_Charlotte_PELLERIN.Client.Login(1, "mdp");
            Assert.AreEqual(true, result);
            Assert.AreEqual(false, result2);
        }
    }
}
