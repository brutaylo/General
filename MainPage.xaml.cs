using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        private const string ApiKey = "your_openweathermap_api_key";
        
        public MainPage()
        {
            InitializeComponent();
            UpdateWeatherAsync();
        }

        private async void OnUpdateWeatherClicked(object sender, EventArgs e)
        {
            await UpdateWeatherAsync();
        }

        private async Task UpdateWeatherAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(
                        new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)));
                }

                if (location == null)
                    return;

                string url = $"https://api.openweathermap.org/data/2.5/weather?lat={location.Latitude}&lon={location.Longitude}&appid={ApiKey}&units=metric";
                using var client = new HttpClient();
                var response = await client.GetStringAsync(url);

                var weatherData = JObject.Parse(response);
                var temperature = weatherData["main"]["temp"].ToString();
                var weatherCondition = weatherData["weather"][0]["description"].ToString();

                TempLabel.Text = $"Temperature: {temperature}°C";
                ConditionLabel.Text = $"Condition: {weatherCondition}";
            }
            catch (Exception ex)
            {
                // Handle exceptions
                TempLabel.Text = "Error fetching weather data.";
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
