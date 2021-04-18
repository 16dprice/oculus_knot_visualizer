using System;
using System.IO;
using Domain;
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

        private float TOL = (float) Math.Pow(10, -4);
        private readonly string _testFilePath = Path.Combine("TestFiles", "SegmentDistanceCalculatorTestCases");
        
        [Test]
        public void TestSegmentDistance()
        {
            foreach(var testCase in GetTestCases().cases)
            {
                var p0Bead = new LinkRelaxingBead(new Bead(testCase.P0), 0, 0, 1);
                var p1Bead = new LinkRelaxingBead(new Bead(testCase.P1), 0, 0, 1);
                var q0Bead = new LinkRelaxingBead(new Bead(testCase.Q0), 0, 0, 1);
                var q1Bead = new LinkRelaxingBead(new Bead(testCase.Q1), 0, 0, 1);

                var S1 = new Segment(p0Bead, p1Bead);
                var S2 = new Segment(q0Bead, q1Bead);

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