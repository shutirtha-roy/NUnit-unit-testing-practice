using NUnit.Framework;
using UnitLibrary.ClassesWithNoDependencies;

namespace Unit.NonDependency.Tests
{
    public class FizzBuzzTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(15)]
        [TestCase(30)]
        [TestCase(45)]
        public void GetOutput_WhenDivisibleBy3And5_GetFizzBuzz(int number)
        {
            string expected = "FizzBuzz";

            var result = FizzBuzz.GetOutput(number);

            Assert.AreEqual(expected, result);
        }
    }
}