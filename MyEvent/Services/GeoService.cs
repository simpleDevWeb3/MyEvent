using System.Text.Json;
using MyEvent.Models;

public class GeoService
{
    private readonly HttpClient _http;
    private readonly string _apiKey = "8fc0ccffb1c646c583f403f8e9c167da"; // Replace with actual API key

    public GeoService(HttpClient http)
    {
        _http = http;
    }

    public async Task<(double lat, double lon)?> GetCoordinatesAsync(string address)
    {
        string url = $"https://api.geoapify.com/v1/geocode/search?text={Uri.EscapeDataString(address)}&filter=countrycode:my&apiKey={_apiKey}";

        try
        {
            var response = await _http.GetAsync(url);   //Makes an asynchronous GET request to the API.
            response.EnsureSuccessStatusCode(); //Throws an exception if the response status code is not 200 OK.

            var json = await response.Content.ReadAsStringAsync();  //Reads the raw JSON string from the HTTP response
            var data = JsonSerializer.Deserialize<GeoapifyGeocodingResponseDto>(json);  //Converts the JSON string into a C# object 

            var feature = data?.features?.FirstOrDefault(); /*Safely retrieves the first feature (location result) from the API’s features array.
                                                          Uses null-conditional operators (?.) to avoid null reference errors.*/
            if (feature != null)
            {
                var coords = feature.geometry.coordinates;
                return (coords[1], coords[0]); // swap, Geoapify returns [lon, lat]
            }
            return null;
        }
        catch
        {
            //        throw new Exception("Location not found");
            return null;
        }

    }
}
