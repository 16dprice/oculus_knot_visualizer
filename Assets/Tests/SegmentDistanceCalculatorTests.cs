using System;
using LinkRelaxing;
using NUnit.Framework;
using UnityEngine;

// pt1 = {1, 2, 1}; pt2 = {-1, -1, -1};
// pt3 = {-1, -1, 3}; pt4 = {2, 1, -1};

namespace Tests
{
    public class SegmentDistanceCalculatorTests
    {
        private const float TOL = 0.0001f;
        
        [Test]
        public void TestGetPDListCount()
        {
            var expected = 0.915737f;

            var P0 = new Vector3(1, 2, 1);
            var P1 = new Vector3(-1, -1, -1);

            var Q0 = new Vector3(-1, -1, 3);
            var Q1 = new Vector3(2, 1, -1);

            var S1 = new Segment(P0, P1);
            var S2 = new Segment(Q0, Q1);

            var actual = SegmentDistanceCalculator.SegmentDistance(S1, S2);
            
            Console.WriteLine(Math.Abs(expected - actual));
            Assert.That(Math.Abs(expected - actual), Is.LessThan(TOL));
        }
    }
}