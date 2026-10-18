using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ProductDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [Required]
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [Range(0, 5)]
    [JsonPropertyName("rating")]
    public int Rating { get; set; }

    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; set; }

    [JsonPropertyName("comments")]
    public string? Comments { get; set; }

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    // Relaciones
    [JsonPropertyName("categoryId")]
    public int? CategoryId { get; set; }

    [JsonPropertyName("supplierId")]
    public int? SupplierId { get; set; }

    [JsonPropertyName("inventoryId")]
    public int? InventoryId { get; set; }

    public static ProductDTO ConvertFrom(Product product)
    {
        return new ProductDTO
        {
            Id = Guid.NewGuid(),
            ProductId = product.ProductId,
            Name = product.ProductName,
            Description = product.Description,
            Rating = (int)(product.Rating ?? 0),
            ModifiedBy = product.ModifiedBy,
            CreatedBy = product.CreatedBy,
            Comments = string.Empty, // Assuming comments are not present in the Product entity
            CreatedDate = product.LastModified ?? DateTime.Now,
            ModifiedDate = product.LastModified ?? DateTime.Now,
            CategoryId = product.CategoryId,
            SupplierId = product.SupplierId,
            InventoryId = product.InventoryId,
        };
    }

    public static Product ConvertTo(ProductDTO productDTO)
    {
        return new Product
        {
            ProductId = productDTO.ProductId,
            ProductName = productDTO.Name,
            Description = productDTO.Description,
            Rating = productDTO.Rating,
            ModifiedBy = productDTO.ModifiedBy,
            CreatedBy = productDTO.CreatedBy,
            LastModified = productDTO.ModifiedDate,
            CategoryId = productDTO.CategoryId,
            SupplierId = productDTO.SupplierId,
            InventoryId = productDTO.InventoryId,
        };
    }
}
