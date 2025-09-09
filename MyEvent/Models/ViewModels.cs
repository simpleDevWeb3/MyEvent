using Microsoft.AspNetCore.Mvc;
using MyEvent.Migrations;
using System.ComponentModel.DataAnnotations;

namespace MyEvent.Models;

#nullable disable warnings

public class EventVM
{
    [MaxLength(100)]
    public string Title { get; set; }

    [Remote("CheckDate", "CreateEvent", ErrorMessage = "Invalid {0}.")]
    public DateOnly Date { get; set; }

    [Display(Name = "Start Time")]
    [Remote("CheckTime", "CreateEvent", AdditionalFields = "EndTime", ErrorMessage = "Invalid time range.")]
    public TimeOnly StartTime { get; set; }

    [Display(Name = "End Time")]
    [Remote("CheckTime", "CreateEvent", AdditionalFields = "StartTime", ErrorMessage = "Invalid time range.")]
    public TimeOnly EndTime { get; set; }

    [Display(Name = "Category")]
    [StringLength(8)]
    [RegularExpression(@"^CAT\d{5}$", ErrorMessage = "Invalid {0}.")]
    [Remote("CheckCategoryId", "CreateEvent", ErrorMessage = "Invalid {0}.")]
    public string CategoryId { get; set; }

    [Range(0, 200, ErrorMessage = "Price must be between 0 and 200")]
    [Remote("CheckPrice", "CreateEvent", ErrorMessage = "Invalid {0}.")]
    public decimal Price { get; set; }

    [MaxLength(100)]
    public string Description { get; set; }

    [Display(Name = "Contact Email")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid {0}.")]
    public string ContactEmail { get; set; }

    [Display(Name = "Image")]
    public IFormFile ImageUrl { get; set; }

    [Display(Name = "Location")]
    public string Formatted_address { get; set; }
}

public class UpdateEventVM
{
    public string Id { get; set; }

    [MaxLength(100)]
    public string Title { get; set; }

    [Remote("CheckDate", "CreateEvent", ErrorMessage = "Invalid {0}.")]
    public DateOnly Date { get; set; }

    [Display(Name = "Start Time")]
    [Remote("CheckTime", "CreateEvent", AdditionalFields = "EndTime", ErrorMessage = "Invalid time range.")]
    public TimeOnly StartTime { get; set; }

    [Display(Name = "End Time")]
    [Remote("CheckTime", "CreateEvent", AdditionalFields = "StartTime", ErrorMessage = "Invalid time range.")]
    public TimeOnly EndTime { get; set; }

    [Display(Name = "Category")]
    [StringLength(8)]
    [RegularExpression(@"^CAT\d{5}$", ErrorMessage = "Invalid {0}.")]
    [Remote("CheckCategoryId", "CreateEvent", ErrorMessage = "Invalid {0}.")]
    public string CategoryId { get; set; }

    [Range(0, 200, ErrorMessage = "Price must be between 0 and 200")]
    [Remote("CheckPrice", "CreateEvent", ErrorMessage = "Invalid {0}.")]
    public decimal Price { get; set; }

    [MaxLength(100)]
    public string Description { get; set; }

    [Display(Name = "Contact Email")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid {0}.")]
    public string ContactEmail { get; set; }

    [Display(Name = "Image")]
    public IFormFile? Image { get; set; }
    
    public string? ImageUrl { get; set; }

    public string? Address { get; set; }

    [Display(Name = "Location")]
    public string? Formatted_address { get; set; }
}
