using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace SandBox
{
    public class TestInt
    {
        [MenuItem("Tests/Int == float")]
        static void TestIntFloat()
        {
            Debug.Log(0.8 > 0);
        }

        [Test]
        public void TestIntSimplePasses() 
        {
            // Use the Assert class to test conditions.
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator TestIntWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}