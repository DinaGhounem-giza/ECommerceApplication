using System;
using Shouldly;
using Xunit;

namespace AbpSolution1.Orders;

public class OrderDetailsTests
{
    [Fact]
    public void Should_Create_Valid_OrderDetails()
    {
        // Arrange & Act
        var orderDetails = new OrderDetails(productId: 1, quantity: 5);

        // Assert
        orderDetails.ProductId.ShouldBe(1);
        orderDetails.Quantity.ShouldBe(5);
    }

    [Fact]
    public void SetQuantity_Should_Update_Quantity_When_Valid()
    {
        // Arrange
        var orderDetails = new OrderDetails(productId: 1, quantity: 5);

        // Act
        orderDetails.SetQuantity(10);

        // Assert
        orderDetails.Quantity.ShouldBe(10);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5)]
    public void SetQuantity_Should_Throw_Exception_When_Invalid(int invalidQuantity)
    {
        // Arrange
        var orderDetails = new OrderDetails(productId: 1, quantity: 5);

        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() => orderDetails.SetQuantity(invalidQuantity));
        exception.Message.ShouldBe("Quantity must be greater than zero.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    public void Should_Accept_Valid_Positive_Quantities(int validQuantity)
    {
        // Arrange & Act
        var orderDetails = new OrderDetails(productId: 1, quantity: validQuantity);

        // Assert
        orderDetails.Quantity.ShouldBe(validQuantity);
    }
}
