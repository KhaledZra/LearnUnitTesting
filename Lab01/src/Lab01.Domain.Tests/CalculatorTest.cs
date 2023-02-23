using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace Lab01.Domain.Tests;

public class CalculatorTest
{
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(-1, 2, 1)]
    [InlineData(0, 0, 0)]
    public void Add_ShouldReturnCorrectSum(int a, int b, int expected)
    {
        // Arrange
        Calculator calculator = new Calculator();

        // Act
        int result = calculator.Add(a, b);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Khaled", "Zraiqi", "KhaledZraiqi")]
    [InlineData("123", "456", "123456")]
    public void Calculator_Should_add_strings(string a, string b, string expected)
    {
        // Arrange
        Calculator calculator = new Calculator();

        // Act
        string result = calculator.AddString(a, b);

        // Assert
        result.Should().Be(expected);
    }

    public static IEnumerable<object[]> GetNumbers()
    {
        yield return new object[] {5, 2, 7};
        yield return new object[] {10, 10, 20};
        yield return new object[] {15, 15, 30};
    }

    [Theory]
    [MemberData(nameof(GetNumbers))]
    public void Add_two_numbers_member_data(int a, int b, int expectedResult)
    {
        // Arrange
        var sut = new Calculator();
        
        // Act
        int result = sut.Add(a, b);

        // Assert
        result.Should().Be(expectedResult);
    }
    
    [Theory]
    [ClassData(typeof(NumberGenerator))]
    public void Add_two_numbers_class_data(int a, int b, int expectedResult)
    {
        // Arrange
        var sut = new Calculator();
        
        // Act
        int result = sut.Add(a, b);

        // Assert
        result.Should().Be(expectedResult);
    }
}

public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }

    public string AddString(string a, string b)
    {
        return a + b;
    }
}