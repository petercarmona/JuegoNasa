using System;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.APIs.Models
{
    /// <summary>
    /// Modelo de datos para información NDVI obtenida desde una API.
    /// Compatible con deserialización JSON.
    /// </summary>
    [Serializable]
    public class NDVIData
    {
        // Si el JSON devuelve algo como { "entries": [ {...}, {...} ] }
        [SerializeField]
        public List<NDVIEntry> entries = new List<NDVIEntry>();

        [Serializable]
        public class NDVIEntry
        {
            [SerializeField] public string date;   // Ejemplo: "2025-10-01"
            [SerializeField] public float ndvi;   // Valor NDVI (0.0 - 1.0)
            [SerializeField] public float min;    // Valor mínimo histórico
            [SerializeField] public float max;    // Valor máximo histórico
            [SerializeField] public float mean;   // Promedio histórico

            public NDVIEntry(string date, float ndvi, float min, float max, float mean)
            {
                this.date = date;
                this.ndvi = ndvi;
                this.min = min;
                this.max = max;
                this.mean = mean;
            }
        }

        // ═══════════════════════════════════════
        // MÉTODOS DE UTILIDAD
        // ═══════════════════════════════════════

        /// <summary>
        /// Devuelve el último valor NDVI recibido.
        /// </summary>
        public float GetLatestNDVI()
        {
            return entries != null && entries.Count > 0 ? entries[^1].ndvi : 0f;
        }

        /// <summary>
        /// Devuelve el promedio del último registro.
        /// </summary>
        public float GetLatestMean()
        {
            return entries != null && entries.Count > 0 ? entries[^1].mean : 0f;
        }

        /// <summary>
        /// Diferencia entre NDVI actual y su promedio histórico.
        /// </summary>
        public float GetDifferenceFromMean()
        {
            if (entries == null || entries.Count == 0) return 0f;
            var latest = entries[^1];
            return latest.ndvi - latest.mean;
        }

        /// <summary>
        /// Devuelve el rango del último periodo (min → max).
        /// </summary>
        public string GetLatestRange()
        {
            if (entries == null || entries.Count == 0) return "N/A";
            var latest = entries[^1];
            return $"{latest.min:F3} → {latest.max:F3}";
        }

        /// <summary>
        /// Indica si el NDVI está por encima del promedio.
        /// </summary>
        public bool IsAboveAverage()
        {
            return GetDifferenceFromMean() > 0;
        }
    }
}
