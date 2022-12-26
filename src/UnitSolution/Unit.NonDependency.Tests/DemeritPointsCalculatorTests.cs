using NUnit.Framework;
using Shouldly;
using System;
using UnitLibrary.ClassesWithNoDependencies;

namespace Unit.NonDependency.Tests
{
    public class DemeritPointsCalculatorTests
    {
        private DemeritPointsCalculator _demirPointsCalculator;

        [SetUp]
        public void Setup()
        {
            _demirPointsCalculator = new DemeritPointsCalculator();
        }

        [Test]
        [TestCase(302)]
        [TestCase(-2)]
        public void CalculateDemeritPoints_WhenSpeedLessThanZeroOrGreaterThanMaxSpeed_ThrowArgumentOutOfRangeException(int speed)
        {
            Should.Throw<ArgumentOutOfRangeException>(() =>
                _demirPointsCalculator.CalculateDemeritPoints(speed)
            );
        }

        [Test]
        [TestCase(65)]
        [TestCase(32)]
        [TestCase(0)]
        [TestCase(66)]
        public void CalculateDemeritPoints_WhenSpeedIsLessThanEqualToSpeedLimit_ReturnZero(int speed)
        {
            var expected = 0;

            var result = _demirPointsCalculator.CalculateDemeritPoints(speed);

            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(165, 20)]
        [TestCase(165, 10)]
        public void CalculateDemitPoints_WhenSpeedIsGreaterThanSpeedLimitAndLessThanMaxSpeed_ReturnPoints(int speed, int expected)
        {
            var result = _demirPointsCalculator.CalculateDemeritPoints(speed);

            Assert.AreEqual(expected, result);
        }

    }
}
