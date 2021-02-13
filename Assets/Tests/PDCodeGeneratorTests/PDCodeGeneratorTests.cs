using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
                CrossingNumber = crossingNumber;
                Ordering = ordering;
                NumComponents = numComponents;
            }

            public int CrossingNumber { get; }
            public int Ordering { get; }
            public int NumComponents { get; }
        }

        public List<TestCase> GetPDListTestCases()
        {
            var testCases = new List<TestCase>();

            List<TestCase> items;
            using (StreamReader r = new StreamReader("Assets/Tests/PDCodeGeneratorTests/pdCodeGeneratorTestCases.json"))
            { 
                var json = r.ReadToEnd();
                items = JsonUtility.FromJson<List<TestCase>>(json);
            }

            Console.WriteLine(items);
            
            testCases.Add(new TestCase(3, 1));
            testCases.Add(new TestCase(4, 1));
            testCases.Add(new TestCase(5, 1));
            testCases.Add(new TestCase(5, 2));

            return testCases;
        }

        [Test]
        public void TestGetPDListCount()
        {
            foreach (var testCase in GetPDListTestCases())
            {
                var generator = new PDCodeGenerator(
                    new DefaultFileBeadsProvider(
                        testCase.CrossingNumber,
                        testCase.Ordering,
                        testCase.NumComponents
                    )
                );
                var crossingList = generator.GetPDList();

                Assert.That(
                    crossingList.Count,
                    Is.EqualTo(testCase.CrossingNumber)
                );
            }
        }
    }
}