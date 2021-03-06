using System;
using System.IO;
using LinkRelaxing;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class SegmentDistanceCalculatorTests
    {
        [Serializable]
        private class TestCases
        {
            public TestCase[] cases;
        }
        
        [Serializable]
        private class TestCase
        {
            public Vector3 P0;
            public Vector3 P1;
            public Vector3 Q0;
            public Vector3 Q1;
            public float expected;
        }
        
        private const float TOL = 0.0001f;
        private readonly string _testFilePath = Path.Combine("TestFiles", "SegmentDistanceCalculatorTestCases");
        
        [Test]
        public void TestSegmentDistance()
        {
            foreach(var testCase in GetTestCases().cases)
            {
                var S1 = new Segment(testCase.P0, testCase.P1);
                var S2 = new Segment(testCase.Q0, testCase.Q1);

                var actual = SegmentDistanceCalculator.SegmentDistance(S1, S2);
                
                Assert.That(Math.Abs(testCase.expected - actual), Is.LessThan(TOL));
            }
        }

        private TestCases GetTestCases()
        {
            var jsonFile = Resources.Load(_testFilePath) as TextAsset;
            var cases = JsonUtility.FromJson<TestCases>(jsonFile.text);

            return cases;
        }
    }
}