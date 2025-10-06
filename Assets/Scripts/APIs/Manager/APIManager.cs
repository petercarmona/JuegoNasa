using System;
using UnityEngine;
using Farm.APIs.Core;
using Farm.APIs.Services;
using Farm.APIs.Models;

namespace Farm.APIs.Manager
{
    /// <summary>
    /// Gestor central de todas las APIs del juego.
    /// Central manager for all game APIs.
    /// </summary>
    public class APIManager : MonoBehaviour
    {
        [Header("Service References")]
        [SerializeField] private WeatherAPIService weatherService;
        [SerializeField] private NDVIAPIService ndviService;
        
        [Header("Auto-Fetch Settings")]
        [SerializeField] private bool fetchOnStart = false;
        [SerializeField] private float autoUpdateInterval = 3600f; // 1 hora
        
        // Singleton pattern
        public static APIManager Instance { get; private set; }
        
        // Events para notificar cambios
        public event Action<string> OnWeatherDataReceived;
        public event Action<NDVIData> OnNDVIDataReceived;
        public event Action<string> OnAPIError;
        
        private void Awake()
        {
            // Singleton setup
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            // Auto-inicializar servicios si no están asignados
            if (weatherService == null)
                weatherService = gameObject.AddComponent<WeatherAPIService>();
            
            if (ndviService == null)
                ndviService = gameObject.AddComponent<NDVIAPIService>();
            
            // Auto-fetch al inicio si está activado
            if (fetchOnStart)
                FetchAllData();
            
            // Auto-actualización periódica
            if (autoUpdateInterval > 0)
                InvokeRepeating(nameof(FetchAllData), autoUpdateInterval, autoUpdateInterval);
        }

        /// <summary>
        /// Obtiene datos de todas las APIs.
        /// Fetches data from all APIs.
        /// </summary>
        public void FetchAllData()
        {
            Debug.Log("🔄 [APIManager] Fetching all API data...");
            FetchWeatherData();
            FetchNDVIData();
        }
        
        /// <summary>
        /// Obtiene datos del clima.
        /// Fetches weather data.
        /// </summary>
        public void FetchWeatherData()
        {
            weatherService.FetchData(
                onSuccess: (data) => 
                {
                    Debug.Log("✅ [APIManager] Weather data received successfully");
                    OnWeatherDataReceived?.Invoke(data);
                },
                onError: (error) =>
                {
                    Debug.LogError($"❌ [APIManager] Weather API error: {error}");
                    OnAPIError?.Invoke($"Weather API: {error}");
                }
            );
        }

        /// <summary>
        /// Obtiene y procesa los datos NDVI.
        /// Fetches NDVI data and processes it.
        /// </summary>
        public void FetchNDVIData()
        {
            ndviService.FetchData(
                onSuccess: (jsonData) => // jsonData es el JSON devuelto, pero el servicio ya cacheó el objeto
                {
                    Debug.Log("✅ [APIManager] NDVI data received successfully");

                    NDVIData ndviData = ndviService.GetCachedData();

                    if (ndviData != null && ndviData.entries.Count > 0)
                    {
                        // 🚀 PASO CLAVE: Notificar a todos los GameObjects suscritos
                        OnNDVIDataReceived?.Invoke(ndviData);

                        // Lógica de debug y juego (opcional)
                        float currentNDVI = ndviData.GetLatestNDVI();
                        bool isHealthy = ndviData.IsAboveAverage();
                        Debug.Log($"🎮 [Game] NDVI actual: {currentNDVI:F3}");
                        Debug.Log($"🎮 [Game] Salud del terreno: {(isHealthy ? "Buena ✅" : "Necesita atención ⚠️")}");
                    }
                    else
                    {
                        // ⚠️ Notificar error si los datos están vacíos (como en tu error anterior)
                        string errorMsg = "NDVIData está vacío o nulo. No hay entradas para mostrar.";
                        Debug.LogWarning($"⚠️ [APIManager] {errorMsg}");
                        OnAPIError?.Invoke($"NDVI API: {errorMsg}");
                    }
                },
                onError: (error) =>
                {
                    Debug.LogError($"❌ [APIManager] NDVI API error: {error}");
                    OnAPIError?.Invoke($"NDVI API: {error}");
                }
            );
        }
    }
}
