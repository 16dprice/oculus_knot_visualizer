using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using PDCodeGeneration;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NewTestScript
    {
        public struct TestCase
        {
            public TestCase(int crossingNumber, int ordering, int numComponents = 1)
            {
                this.crossingNumber = crossingNumber;
                this.ordering = ordering;
                this.numComponents = numComponents;
            }

            public int crossingNumber { get; }
            public int ordering { get; }
            public int numComponents { get; }
        }

        public List<TestCase> GetTestCases()
        {
            var testCases = new List<TestCase>();

            testCases.Add(new TestCase(3, 1));
            testCases.Add(new TestCase(4, 1));
            testCases.Add(new TestCase(5, 1));
            testCases.Add(new TestCase(5, 2));

            return testCases;
        }

        [Test]
        public void NewTestScriptSimplePasses()
        {
            foreach (var testCase in GetTestCases())
            {
                var generator = new PDCodeGenerator(
                    new DefaultFileBeadsProvider(
                        testCase.crossingNumber,
                        testCase.ordering,
                        testCase.numComponents
                    )
                );
                var crossingList = generator.GetPDList();

                Assert.That(
                    crossingList.Count,
                    Is.EqualTo(testCase.crossingNumber)
                );
            }
        }
    }
}