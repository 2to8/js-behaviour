using NUnit.Framework;
using UnityEngine;

namespace UseCase
{
    [TestFixture, Category("UseCase Tests")]
    public class UseCase
    {
        [Test]
        public void TestFirst()
        {
            Debug.Log("test!");
            Assert.IsTrue(true);
        }
    }
}