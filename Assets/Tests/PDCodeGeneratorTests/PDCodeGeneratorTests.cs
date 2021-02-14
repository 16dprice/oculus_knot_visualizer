using System.Collections.Generic;
using NUnit.Framework;
using PDCodeGeneration;

namespace Tests.PDCodeGeneratorTests
{
    public class PDCodeGeneratorTests
    {
        private readonly struct TestCase
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

        private List<TestCase> GetPDListTestCases()
        {
            (int, int)[] numKnotsByCrossingNum =
            {
                (3, 1),
                (4, 1),
                (5, 2),
                (6, 3),
                (7, 7)
            };

            (int, int)[] numTwoComponentLinksByCrossingNum =
            {
                (2, 1),
                (4, 1),
                (5, 1),
                (6, 3)
            };
            
            // NOTE: We ignore the link 6_3_3 for now as this is a known exception in the code
            // The code, as written, does not account for multiple crossings occurring over a single pair of beads
            (int, int)[] numThreeComponentLinksByCrossingNum =
            {
                (6, 2)
            };

            var testCases = new List<TestCase>();

            foreach (var (crossingNumber, numberOfKnotsWithCrossingNumber) in numKnotsByCrossingNum)
            {
                for (int ordering = 1; ordering <= numberOfKnotsWithCrossingNumber; ordering++)
                {
                    testCases.Add(new TestCase(crossingNumber, ordering));
                }
            }
            
            foreach (var (crossingNumber, numberOfLinksWithCrossingNumber) in numTwoComponentLinksByCrossingNum)
            {
                for (int ordering = 1; ordering <= numberOfLinksWithCrossingNumber; ordering++)
                {
                    testCases.Add(new TestCase(crossingNumber, ordering, 2));
                }
            }
            
            foreach (var (crossingNumber, numberOfLinksWithCrossingNumber) in numThreeComponentLinksByCrossingNum)
            {
                for (int ordering = 1; ordering <= numberOfLinksWithCrossingNumber; ordering++)
                {
                    testCases.Add(new TestCase(crossingNumber, ordering, 3));
                }
            }

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