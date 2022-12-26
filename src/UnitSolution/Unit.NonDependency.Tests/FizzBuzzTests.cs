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
            var expected = "FizzBuzz";

            var result = FizzBuzz.GetOutput(number);

            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(3)]
        [TestCase(9)]
        [TestCase(12)]
        public void GetOutput_WhenDivisibleBy3_GetFizz(int number)
        {
            var expected = "Fizz";

            var result = FizzBuzz.GetOutput(number);

            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(5)]
        [TestCase(10)]
        public void GetOutput_WhenDivisibleBy5_GetBuzz(int number)
        {
            var expected = "Buzz";

            var result = FizzBuzz.GetOutput(number);

            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(11)]
        [TestCase(17)]
        [TestCase(12)]
        public void GetOutput_WhenNotDivisibleBy3Or5_GetParameterNumberInString(int number)
        {
            var expected = number.ToString();

            var result = FizzBuzz.GetOutput(number);

            Assert.AreEqual(expected, result);
        }
    }
}