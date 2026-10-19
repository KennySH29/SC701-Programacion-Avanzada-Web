using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class CategoryDTO
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("categoryId")]
    public int CategoryId { get; set; }

    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [Required]
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    [JsonPropertyName("comments")]
    public string? Comments { get; set; }

    [JsonPropertyName("createdDate")]
    public DateTime? CreatedDate { get; set; }

    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }


    public static CategoryDTO ConvertFrom(Category category)
    {
        return new CategoryDTO
        {
            Id = Guid.NewGuid(),
            CategoryId = category.CategoryId,
            Name = category.CategoryName!,
            Description = category.Description!,
            ModifiedBy = category.ModifiedBy,
            Comments = string.Empty, // Assuming comments are not present in the Category entity
            CreatedDate = category.LastModified ?? DateTime.Now, // Assuming LastModified is used as CreatedDate
            ModifiedDate = category.LastModified ?? DateTime.Now // Assuming LastModified is used as ModifiedDate
        };
    }

    public static Category ConvertTo(CategoryDTO categoryDTO)
    {
        return new Category
        {
            CategoryId = categoryDTO.CategoryId,
            CategoryName = categoryDTO.Name,
            Description = categoryDTO.Description,
            ModifiedBy = categoryDTO.ModifiedBy,
            LastModified = categoryDTO.ModifiedDate // Assuming ModifiedDate is used as LastModified
        };
    }
}
