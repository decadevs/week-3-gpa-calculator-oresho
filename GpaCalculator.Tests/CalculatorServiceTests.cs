using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GpaCalculator.Tests
{
    public class CalculatorServiceTests
    {
        [Fact]
        public void IsInRange_ShouldReturnFalse()
        {
            // Arrange
            CalculatorService calculatorService = new CalculatorService();

            // Act

            // Assert
            Assert.False(calculatorService.IsInRange(-1, 0, 100));
            Assert.False(calculatorService.IsInRange(101, 0, 100));
        }

        [Theory]
        [InlineData(33, 0, 40)]
        [InlineData(55, 50, 55)]
        public void IsInRange_ShouldReturnTrue(int score, int lowerBound, int upperBound)
        {
            // Arrange
            CalculatorService calculatorService = new CalculatorService();

            // Act

            // Assert
            Assert.True(calculatorService.IsInRange(score, lowerBound, upperBound));
        }

        [Theory]
        [InlineData(70, 5)]
        [InlineData(60, 4)]
        [InlineData(50, 3)]
        [InlineData(45, 2)]
        [InlineData(40, 1)]
        [InlineData(33, 0)]
        public void GetGradeUnit_ShouldReturnCorrectGradeUnit(double score, int expectedGradeUnit)
        {
            // Arrange
            CalculatorService calculatorService = new CalculatorService();

            // Act
            int actualGradeUnit = calculatorService.GetGradeUnit(score);

            // Assert
            Assert.Equal(expectedGradeUnit, actualGradeUnit);
        }
    }
}
