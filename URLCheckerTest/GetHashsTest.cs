using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using URLChecker;

namespace URLCheckerTest
{
    [TestClass]
    public class GetHashsTest
    {
        string startHashString = "36a8Zax7na";
        string url = "https://anonfile.com/";
        [TestMethod]
        public void TestHashGen()
        {
            var countHash = 2000;
            var startHash = HashChecker.GetStartHash(startHashString);

            var hashs = HashChecker.GetHashs(startHash, countHash).Result;
            Assert.AreEqual(hashs.Count, countHash);
        }

        [TestMethod]
        public void TestHttpRequest()
        {
            try
            {
                LowLevelHttpRequest.BrutForceAsync(url + startHashString).Wait();
            }
            catch (Exception e)
            {
                Assert.Fail($"Unexpected exception of type {e.GetType()} caught: {e.Message}");
            }
        }
    }
}
