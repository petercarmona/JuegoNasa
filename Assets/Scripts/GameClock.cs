using System;
using UnityEngine;
using TMPro;

/// <summary>
/// Controla la fecha y hora del juego. Simula el paso del tiempo y actualiza la UI.
/// </summary>
public class GameClock : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("El TextMeshProUGUI que muestra la HORA (00:00).")]
    [SerializeField] private TextMeshProUGUI timerValueText; 
    
    [Tooltip("El TextMeshProUGUI que muestra la FECHA (D/M/A).")]
    [SerializeField] private TextMeshProUGUI dateValueText; 

    [Header("Time & Date State")]
    // ⚠️ La fecha inicial del juego (5 de octubre de 2025)
    [SerializeField] private DateTime gameDate = new DateTime(2025, 10, 5);
    
    // El tiempo actual dentro del día (en segundos).
    private float timeOfDaySeconds = 0f; 
    
    [Tooltip("La duración real en segundos que representa 1 segundo de juego.")]
    [SerializeField] private float gameTimeScale = 60f; // 60.0f = 1 minuto de juego por segundo real (Simulación 1:60)
    
    // Constantes para el reloj
    private const float SECONDS_IN_DAY = 86400f; // 24 * 60 * 60

    void Start()
    {
        if (timerValueText == null || dateValueText == null)
        {
            Debug.LogError("❌ [GameClock] Referencias de texto (Hora/Fecha) no asignadas. Asegúrate de arrastrar 'Timer_text_value' y 'Date_text_value'.");
            enabled = false; 
            return;
        }

        // 1. Iniciar el temporizador a las 12:00 PM (Mediodía)
        // 12 horas * 3600 segundos/hora = 43200 segundos.
        timeOfDaySeconds = 12 * 3600f; 
        
        UpdateDisplay();
    }

    void Update()
    {
        // 1. Acumular tiempo dentro del día
        // Time.deltaTime * gameTimeScale nos da cuántos segundos de juego han pasado.
        timeOfDaySeconds += Time.deltaTime * gameTimeScale; 

        // 2. Verificar si ha pasado un día completo (24:00:00)
        if (timeOfDaySeconds >= SECONDS_IN_DAY)
        {
            timeOfDaySeconds -= SECONDS_IN_DAY; // Reiniciar el contador de tiempo (que ahora tiene el remanente)
            IncrementGameDay(); // Aumentar la fecha
        }

        // 3. Actualizar la UI
        UpdateDisplay();
    }

    /// <summary>
    /// Aumenta la fecha del juego en un día.
    /// </summary>
    private void IncrementGameDay()
    {
        gameDate = gameDate.AddDays(1);
        Debug.Log($"🗓️ Nuevo Día: {gameDate:yyyy-MM-dd}");
        
        // Aquí puedes agregar eventos de juego que ocurren al inicio de un nuevo día.
        // Ejemplo: APIManager.Instance.FetchAllData();
    }

    /// <summary>
    /// Actualiza ambos campos de texto (Hora y Fecha).
    /// </summary>
    private void UpdateDisplay()
    {
        // Convertir segundos a objeto TimeSpan para el formato de Hora
        TimeSpan currentTime = TimeSpan.FromSeconds(timeOfDaySeconds);

        // A. Formato de HORA (00:00) -> Horas y Minutos (HH:MM)
        string timeFormat = string.Format("{0:00}:{1:00}", currentTime.Hours, currentTime.Minutes);
        timerValueText.text = timeFormat;
        
        // B. Formato de FECHA (D/M/A)
        string dateFormat = gameDate.ToString("dd/MM/yyyy");
        dateValueText.text = dateFormat;
    }

    // Método público para obtener la fecha actual (útil para la API)
    public DateTime GetCurrentGameDate()
    {
        return gameDate;
    }
}