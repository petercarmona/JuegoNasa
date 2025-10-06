using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Farm.APIs.Core;
using Farm.APIs.Models;

namespace Farm.APIs.Services
{
    /// <summary>
    /// Servicio para consumir OpenWeatherMap API
    /// Service for consuming OpenWeatherMap API
    /// </summary>
    public class WeatherAPIService : MonoBehaviour, IAPIService
    {
        [Header("API Configuration")]
        [SerializeField] private string apiKey = "9db7ab3e0953eb5c182b6c7fb92ca5e7";
        [SerializeField] private float latitude = -16.4090f;   // Arequipa, Perú
        [SerializeField] private float longitude = -71.5375f;
        
        private const string BASE_URL = "https://api.openweathermap.org/data/2.5/weather";
        private const string UNITS = "metric";  // Para Celsius
        private const string LANG = "es";       // Español
        
        /// <summary>
        /// Realiza la petición a la API del clima
        /// Makes the weather API request
        /// </summary>
        public void FetchData(Action<string> onSuccess, Action<string> onError)
        {
            StartCoroutine(FetchWeatherCoroutine(onSuccess, onError));
        }
        
        private IEnumerator FetchWeatherCoroutine(Action<string> onSuccess, Action<string> onError)
        {
            // Construir URL con parámetros
            // Build URL with parameters
            string url = $"{BASE_URL}?lat={latitude}&lon={longitude}&appid={apiKey}&units={UNITS}&lang={LANG}";
            
            Debug.Log($"[WeatherAPI] 🌐 Requesting: {url}");
            
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                // Enviar petición
                // Send request
                yield return request.SendWebRequest();
                
                // Verificar errores
                // Check for errors
                if (request.result != UnityWebRequest.Result.Success)
                {
                    string error = $"[WeatherAPI] ❌ Error: {request.error}";
                    Debug.LogError(error);
                    onError?.Invoke(request.error);
                }
                else
                {
                    // Éxito - parsear JSON
                    // Success - parse JSON
                    string jsonResponse = request.downloadHandler.text;
                    
                    Debug.Log($"[WeatherAPI] ✅ Response received: {jsonResponse}");
                    
                    // Parsear a objeto
                    // Parse to object
                    try
                    {
                        WeatherData weatherData = JsonUtility.FromJson<WeatherData>(jsonResponse);
                        LogWeatherData(weatherData);
                        onSuccess?.Invoke(jsonResponse);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[WeatherAPI] ❌ JSON Parse Error: {ex.Message}");
                        onError?.Invoke(ex.Message);
                    }
                }
            }
        }
        
        /// <summary>
        /// Registra los datos del clima en consola de forma legible
        /// Logs weather data to console in readable format
        /// </summary>
        private void LogWeatherData(WeatherData data)
        {
            Debug.Log("╔═══════════════════════════════════════╗");
            Debug.Log("║      🌤️  WEATHER DATA RECEIVED       ║");
            Debug.Log("╠═══════════════════════════════════════╣");
            Debug.Log($"║ 📍 Location: {data.name}");
            Debug.Log($"║ 🌡️  Temperature: {data.main.temp}°C");
            Debug.Log($"║ 🤔 Feels Like: {data.main.feels_like}°C");
            Debug.Log($"║ 💧 Humidity: {data.main.humidity}%");
            Debug.Log($"║ 🌬️  Wind Speed: {data.wind.speed} m/s");
            Debug.Log($"║ ☁️  Description: {data.weather[0].description}");
            Debug.Log("╚═══════════════════════════════════════╝");
        }
        
        public string GetServiceName()
        {
            return "OpenWeatherMap API";
        }
    }
}