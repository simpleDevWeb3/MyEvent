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

    //public AddressVM Address { get; set; }
    //public CoordinatesVM Coordinates { get; set; }
    //public Feature Address { get; set; }
    [Display(Name = "Location")]

    public string Formatted_address { get; set; }
}
/*
public class AddressVM
{
    [MaxLength(50)]
    [Display(Name = "Premise")]
    public string Premise { get; set; }

    [MaxLength(50)]
    [Display(Name = "Street")]
    public string Street { get; set; }

    [MaxLength(50)]
    [Display(Name = "City")]
    public string City { get; set; }

    [Display(Name = "Postcode")]
    [MaxLength(5)]
    [RegularExpression(@"\d{5}", ErrorMessage = "Invalid {0} format.")]
    public string Postcode { get; set; }

    [MaxLength(50)]
    [Display(Name = "State")]
    //[Remote("CheckAddress", "CreateEvent", AdditionalFields = "Street,City,Postcode", ErrorMessage = "Invalid Address.")]
    public string State { get; set; }
}

public class CoordinatesVM
{
    public double Lat { get; set; }//may be delete
    public double Lon { get; set; }//may be delete
    public string FullAddress { get; set; }
}

*/


