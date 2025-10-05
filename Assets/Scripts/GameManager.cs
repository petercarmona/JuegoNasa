using UnityEngine;
using System;

// GameManager class controls the overall game logic.
// La clase GameManager controla la lógica general del juego.
public class GameManager : MonoBehaviour
{
    // Singleton instance (so there's only one GameManager in the scene)
    // Instancia Singleton (para que solo exista un GameManager en la escena)
    public static GameManager Instance { get; private set; }

    // Game time control variables
    // Variables de control del tiempo del juego
    public float elapsedTime; // tiempo total transcurrido
    public float timeInterval = 10f; // intervalo para aumentar edad
    private float timer;

    // Player score
    // Puntaje del jugador
    public int score = 0;

    // Event to notify animals when time interval has passed
    // Evento que notifica a los animales cuando pasa cierto tiempo
    public static event Action OnTimeIntervalPassed;

    private void Awake()
    {
        // Singleton initialization
        // Inicialización del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep between scenes / Mantener entre escenas
        }
    }

    private void Update()
    {
        // Update global game time
        // Actualiza el tiempo global del juego
        elapsedTime += Time.deltaTime;
        timer += Time.deltaTime;

        // If enough time has passed, trigger the event
        // Si ha pasado suficiente tiempo, dispara el evento
        if (timer >= timeInterval)
        {
            timer = 0f;
            Debug.Log("Time interval reached! Increasing animal age...");
            OnTimeIntervalPassed?.Invoke();
        }
    }

    // Function to increase the score
    // Función para aumentar el puntaje
    public void AddScore(int points)
    {
        score += points;
        Debug.Log($"Score increased! Current score: {score}");
    }
}
