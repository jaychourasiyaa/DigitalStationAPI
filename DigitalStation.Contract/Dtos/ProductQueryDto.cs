using System;

namespace DigitalStation.Contract.Dtos;

public class ProductQueryDto
{
    // 🔍 Search across all fields (e.g., name, description)
    public string ? SearchTerm { get; set; }

    // 📁 Filter by category (single)
    public int? CategoryId { get; set; }

    // 🏷️ Filter by multiple brands
    public List<int> ? BrandIds { get; set; } = new();

    // 📊 Sorting: "Price", "CreatedDate"
    public string ? SortBy { get; set; } 
    public bool IsSortDescending { get; set; } = false;

    // 📄 Pagination
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
