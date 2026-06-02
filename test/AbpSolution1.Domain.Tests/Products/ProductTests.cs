using System;
using AbpSolution1.Categories;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Modularity;
using Xunit;

namespace AbpSolution1.Products;


public class ProductTests
{
    [Fact]
    public void Should_Create_Valid_Product()
    {
        // Arrange & Act
        var product = new Product(
            nameAr: "لابتوب",
            descriptionAr: "لابتوب للألعاب",
            nameEn: "Laptop",
            descriptionEn: "Gaming Laptop",
            price: 1000m,
            stock: 10,
            categoryId: 1
        );

        // Assert
        product.NameAr.ShouldBe("لابتوب");
        product.DescriptionAr.ShouldBe("لابتوب للألعاب");
        product.NameEn.ShouldBe("Laptop");
        product.DescriptionEn.ShouldBe("Gaming Laptop");
        product.Price.ShouldBe(1000m);
        product.Stock.ShouldBe(10);
        product.CategoryId.ShouldBe(1);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Should_Throw_Exception_When_Price_Is_Zero_Or_Negative(decimal invalidPrice)
    {
        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() =>
        {
            new Product(
                nameAr: "منتج",
                descriptionAr: "وصف",
                nameEn: "Product",
                descriptionEn: "Description",
                price: invalidPrice,
                stock: 10,
                categoryId: 1
            );
        });

        exception.Message.ShouldBe("Price must be greater than zero");
    }

    [Fact]
    public void Should_Allow_Zero_Stock()
    {
        // Arrange & Act
        var product = new Product(
            nameAr: "منتج",
            descriptionAr: "وصف",
            nameEn: "Product",
            descriptionEn: "Description",
            price: 100m,
            stock: 0, // Zero stock should be allowed
            categoryId: 1
        );

        // Assert
        product.Stock.ShouldBe(0);
    }

    [Fact]
    public void SetPrice_Should_Update_Price_When_Valid()
    {
        // Arrange
        var product = new Product(
            nameAr: "منتج",
            descriptionAr: "وصف",
            nameEn: "Product",
            descriptionEn: "Description",
            price: 100m,
            stock: 10,
            categoryId: 1
        );

        // Act
        product.SetPrice(200m);

        // Assert
        product.Price.ShouldBe(200m);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-50.5)]
    public void SetPrice_Should_Throw_Exception_When_Invalid(decimal invalidPrice)
    {
        // Arrange
        var product = new Product(
            nameAr: "منتج",
            descriptionAr: "وصف",
            nameEn: "Product",
            descriptionEn: "Description",
            price: 100,
            stock: 10,
            categoryId: 1
        );

        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() => product.SetPrice(invalidPrice));
        exception.Message.ShouldBe("Price must be greater than zero");
    }

    [Fact]
    public void SetStock_Should_Update_Stock_When_Valid()
    {
        // Arrange
        var product = new Product(
            nameAr: "منتج",
            descriptionAr: "وصف",
            nameEn: "Product",
            descriptionEn: "Description",
            price: 100m,
            stock: 10,
            categoryId: 1
        );

        // Act
        product.SetStock(50);

        // Assert
        product.Stock.ShouldBe(50);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void SetStock_Should_Throw_Exception_When_Negative(int invalidStock)
    {
        // Arrange
        var product = new Product(
            nameAr: "منتج",
            descriptionAr: "وصف",
            nameEn: "Product",
            descriptionEn: "Description",
            price: 100m,
            stock: 10,
            categoryId: 1
        );

        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() => product.SetStock(invalidStock));
        exception.Message.ShouldBe("Stock cannot be negative");
    }


    [Theory]
    [InlineData(10.5)]
    [InlineData(99.99)]
    [InlineData(0.01)]
    [InlineData(1000000)]
    public void Should_Accept_Valid_Decimal_Prices(decimal validPrice)
    {
        // Arrange & Act
        var product = new Product(
            nameAr: "منتج",
            descriptionAr: "وصف",
            nameEn: "Product",
            descriptionEn: "Description",
            price: validPrice,
            stock: 10,
            categoryId: 1
        );

        // Assert
        product.Price.ShouldBe(validPrice);
    }

    [Fact]
    public void Should_Maintain_Category_Relationship()
    {
        // Arrange & Act
        var categoryId = 5;
        var product = new Product(
            nameAr: "منتج",
            descriptionAr: "وصف",
            nameEn: "Product",
            descriptionEn: "Description",
            price: 100m,
            stock: 10,
            categoryId: categoryId
        );

        // Assert
        product.CategoryId.ShouldBe(categoryId);
    }
}
