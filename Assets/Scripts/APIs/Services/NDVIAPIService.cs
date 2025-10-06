using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Farm.APIs.Core;
using Farm.APIs.Models;

namespace Farm.APIs.Services
{
    /// <summary>
    /// Simplified service for NASA NDVI API (CSV format converted to JSON)
    /// </summary>
    public class NDVIAPIService : MonoBehaviour, IAPIService
    {
        [Header("🛰️ API Configuration")]
        [Tooltip("Country ID: 28434 = Peru")]
        [SerializeField] private int countryId = 28434;

        [Tooltip("Year to query")]
        [SerializeField] private int year = 2024;

        [Tooltip("Start month (1-12)")]
        [SerializeField] private int startMonth = 1;

        [Tooltip("Number of months (1-12)")]
        [SerializeField] private int numMonths = 12;  // Only 1 for simplicity

        private const string BASE_URL = "https://glam1.gsfc.nasa.gov/api/gettbl/v4";
        private const string SATELLITE = "MOD";
        private const string VERSION = "v16.1";
        private const string LAYER = "NDVI";
        private const string SHAPE = "ADM";
        private const string TS_TYPE = "seasonal";

        private NDVIData cachedData;

        public void FetchData(Action<string> onSuccess, Action<string> onError)
        {
            StartCoroutine(FetchNDVICoroutine(onSuccess, onError));
        }

        private IEnumerator FetchNDVICoroutine(Action<string> onSuccess, Action<string> onError)
        {
            string url = $"{BASE_URL}?sat={SATELLITE}&version={VERSION}&layer={LAYER}&shape={SHAPE}" +
                         $"&ids={countryId}&ts_type={TS_TYPE}&years={year}&start_month={startMonth}" +
                         $"&num_months={numMonths}&format=csv";

            Debug.Log($"[NDVI] 🛰️ Fetching: {url}");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"[NDVI] ❌ Error: {request.error}");
                    onError?.Invoke(request.error);
                    yield break;
                }

                string csvResponse = request.downloadHandler.text;

                try
                {
                    // Parse CSV and convert to JSON
                    cachedData = ParseCSV(csvResponse);
                    string jsonData = JsonUtility.ToJson(cachedData, true);

                    LogNDVIShort(cachedData);

                    // Return JSON instead of CSV
                    onSuccess?.Invoke(jsonData);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[NDVI] ❌ Parse error: {ex.Message}");
                    onError?.Invoke(ex.Message);
                }
            }
        }

        /// <summary>
        /// Parses CSV into NDVIData (robust version with error handling)
        /// Expected CSV format:
        /// "Date","NDVI","Min","Max","Mean"
        /// "2024-01-01",0.566,0.327,0.652,0.518
        /// </summary>
/// <summary>
/// Parses CSV into NDVIData (robust version with error handling)
/// </summary>
private NDVIData ParseCSV(string csv)
{
    NDVIData data = new NDVIData();
    // ... [Tu Debug.Log inicial] ...

    // Split CSV into lines (mantener tu lógica de split original)
    string[] lines = csv.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

    if (lines.Length < 2)
    {
        Debug.LogWarning("[NDVI] ⚠️ CSV is empty or incomplete");
        return data;
    }

    // 1️⃣ Encuentra la línea de encabezado (que contiene los títulos de las columnas de datos)
    int headerIndex = -1;
    // Buscamos la línea que empieza con "ORDINAL DATE" o que contiene "START DATE"
    for (int i = 0; i < lines.Length; i++)
    {
        // Usamos ToUpperInvariant para hacer la búsqueda insensible a mayúsculas/minúsculas
        if (lines[i].ToUpperInvariant().Contains("ORDINAL DATE") && lines[i].ToUpperInvariant().Contains("START DATE"))
        {
            headerIndex = i;
            Debug.Log($"📋 Detected header line at index {i}: {lines[i]}");
            break;
        }
    }

    if (headerIndex == -1)
    {
        Debug.LogWarning("[NDVI] ⚠️ Could not find the data header line (starting with ORDINAL DATE).");
        return data;
    }

    // 2️⃣ Parse solo las líneas debajo del encabezado
    for (int i = headerIndex + 1; i < lines.Length; i++)
    {
        string line = lines[i].Trim();
        if (string.IsNullOrWhiteSpace(line)) continue;

        try
        {
            // La línea real de datos ya no debería tener comillas, pero lo mantenemos por seguridad
            line = line.Replace("\"", "").Trim();
            string[] values = line.Split(',');

            // Contamos las columnas: 1.OrdinalDate, 2.StartDate, 3.EndDate, 4.Source, 5.SampleValue (NDVI), 6.SampleCount, 7.MeanValue, 8.MeanCount, 9.AnomValue, 10.MinValue, 11.MaxValue
            // Necesitamos al menos 11 valores
            if (values.Length < 11) continue; 

            // Los índices son ahora diferentes:
            string date = values[1].Trim(); // START DATE está en el índice [1]
            
            // SAMPLE VALUE (NDVI) está en [4]
            if (!float.TryParse(values[4], System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float ndvi)) continue;

            // MIN VALUE está en [9]
            if (!float.TryParse(values[9], System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float min)) continue;

            // MAX VALUE está en [10]
            if (!float.TryParse(values[10], System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float max)) continue;

            // MEAN VALUE está en [6]
            if (!float.TryParse(values[6], System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float mean)) continue;

            data.entries.Add(new NDVIData.NDVIEntry(date, ndvi, min, max, mean));
            Debug.Log($"✅ Parsed line {i}: {date} | NDVI={ndvi:F3} | Min={min:F3} | Max={max:F3} | Mean={mean:F3}");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"⚠️ Error parsing line {i}: {ex.Message} | Raw Line: {line}");
        }
    }

    Debug.Log($"📊 Total parsed entries: {data.entries.Count}");
    Debug.Log("═══════════════════════════════════════════");
    return data;
}

private void LogNDVIShort(NDVIData data)
{
    Debug.Log("═══════════════════════════════════════════");
    Debug.Log("🌿 [NDVI] DATA SUMMARY");
    Debug.Log("═══════════════════════════════════════════");

    if (data.entries.Count == 0)
    {
        Debug.LogWarning("⚠️ No NDVI data to display");
        return;
    }

    var latest = data.entries[^1];
    float diff = latest.ndvi - latest.mean;
    string status = diff >= 0 ? "✅ Above Average" : "⚠️ Below Average";

    Debug.Log($"🗓️ Date: {latest.date}");
    Debug.Log($"🌱 NDVI: {latest.ndvi:F3}");
    Debug.Log($"📊 Mean: {latest.mean:F3}");
    Debug.Log($"📉 Range: {latest.min:F3} → {latest.max:F3}");
    Debug.Log($"📈 Difference: {diff:F3} ({status})");
    Debug.Log("═══════════════════════════════════════════");

    // Print JSON version
    string jsonPreview = JsonUtility.ToJson(data, true);
    Debug.Log("📦 JSON PREVIEW:\n" + jsonPreview);
}

        public NDVIData GetCachedData() => cachedData;

        public string GetServiceName() => "NASA NDVI API";
    }
}
