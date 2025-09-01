using System.Text.Json;
using MyEvent.Models;

public class GeoService
{
    private readonly HttpClient _http;
    private readonly string _apiKey = "8fc0ccffb1c646c583f403f8e9c167da"; 

    public GeoService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Feature>?> GetCoordinatesAsync(string address)
    {
        try
        {
            string url = $"https://api.geoapify.com/v1/geocode/search?text={Uri.EscapeDataString(address)}&filter=countrycode:my&limit=10&apiKey={_apiKey}";

            var response = await _http.GetAsync(url);   //Makes an asynchronous GET request to the API.
            response.EnsureSuccessStatusCode(); //Throws an exception if the response status code is not 200 OK.

            var json = await response.Content.ReadAsStringAsync();  //Reads the raw JSON string from the HTTP response
            var data = JsonSerializer.Deserialize<GeoapifyGeocodingResponseDto>(json);  //Converts the JSON string into a C# object 

            // Make sure data is not null
            if (data?.Features != null && data.Features.Any())
            {
                var filtered = data.Features
                                .Where(f => !string.IsNullOrEmpty(f.Properties.Premise) && !string.IsNullOrEmpty(f.Properties.Street))
                                .ToList();
                return filtered;
            }
            return null;
        }
        catch
        {
            return null;
        }

    }
    /*
    public async Task<GeoapifyGeocodingResponseDto?> GetAddressAsync(double lat, double lon)
    {
        string url = $"https://api.geoapify.com/v1/geocode/reverse?lat={lat}&lon={lon}&apiKey={_apiKey}";
        try
        {
            var response = await _http.GetAsync(url);   //Makes an asynchronous GET request to the API.
            response.EnsureSuccessStatusCode(); //Throws an exception if the response status code is not 200 OK.

            var json = await response.Content.ReadAsStringAsync();  //Reads the raw JSON string from the HTTP response
            var data = JsonSerializer.Deserialize<GeoapifyGeocodingResponseDto>(json);  //Converts the JSON string into a C# object 

            // Make sure data is not null
            if (data?.Features != null)
            {
                return data; 
            }
            return null;
        }
        catch
        {
            return null;
        }

    }

    */
}
