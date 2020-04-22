using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SampleTest {

    [Test]
    public void GameObject_CreatedWithGiven_WillHaveTheName() {
        var go = new GameObject("MyGameObject");
        Assert.AreEqual("MyGameObject", go.name);
    }

    [UnityTest]
    public IEnumerator GameObject_WithRigidBody_WillBeAffectedByPhysics() {
        var go = new GameObject();
        go.AddComponent<Rigidbody>();
        var originalPosition = go.transform.position.y;

        yield return new WaitForFixedUpdate();

        Assert.AreNotEqual(originalPosition, go.transform.position.y);
    }

    [UnityTest]
    public IEnumerator MonoBehaviourTest_Works()
    {
        yield return new MonoBehaviourTest<MyMonoBehaviourTest>();
    }

    public class MyMonoBehaviourTest : MonoBehaviour, IMonoBehaviourTest
    {
        private int frameCount;
        public bool IsTestFinished
        {
            get { return frameCount > 10; }
        }

        void Update()
        {
            frameCount++;
        }
    }
}