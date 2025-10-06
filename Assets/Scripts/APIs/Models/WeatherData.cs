using System;

namespace Farm.APIs.Models
{
    /// <summary>
    /// Modelo de datos para la respuesta de OpenWeatherMap API
    /// Data model for OpenWeatherMap API response
    /// </summary>
    [Serializable]
    public class WeatherData
    {
        public Coord coord;
        public Weather[] weather;
        public Main main;
        public Wind wind;
        public string name;
        
        [Serializable]
        public class Coord
        {
            public float lon;
            public float lat;
        }
        
        [Serializable]
        public class Weather
        {
            public int id;
            public string main;
            public string description;
            public string icon;
        }
        
        [Serializable]
        public class Main
        {
            public float temp;           // Temperatura en Celsius
            public float feels_like;     // Sensación térmica
            public float temp_min;
            public float temp_max;
            public int pressure;         // Presión atmosférica
            public int humidity;         // Humedad en %
        }
        
        [Serializable]
        public class Wind
        {
            public float speed;          // Velocidad del viento m/s
            public int deg;              // Dirección en grados
        }
    }
}