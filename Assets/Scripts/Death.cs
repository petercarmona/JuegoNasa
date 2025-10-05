using UnityEngine;

public class Death : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Animal Life Settings / Configuración de Vida del Animal")]
     // Max health points / Puntos de vida máximos
    [SerializeField] private int maxAge = 5; // Maximum age before death / Edad máxima antes de morir

    private int currentAge = 0; // Current animal age / Edad actual del animal
    private Animal animalReference; // Reference to the Animal script / Referencia al script del Animal
    void Start()
    {
      // Get reference to the Animal component on this GameObject
        // Obtiene la referencia al componente Animal en este GameObject
        animalReference = GetComponent<Animal>();

        // Subscribe to GameManager event
        // Suscribirse al evento del GameManager
        GameManager.OnTimeIntervalPassed += HandleTimeInterval;

        // Initialize animal life
        // Inicializa la vida del animal
        currentAge = 1;
        Debug.Log($"[LIFE START] Animal: {animalReference.name} ({animalReference.GetType().Name}) ");
    }
  

   private void OnDestroy()
    {
        // Unsubscribe from GameManager event to avoid memory leaks
        // Desuscribirse del evento del GameManager para evitar fugas de memoria
        GameManager.OnTimeIntervalPassed -= HandleTimeInterval;
    }

    // Called every time the GameManager event triggers
    // Se llama cada vez que se activa el evento del GameManager
    private void HandleTimeInterval()
    {
        currentAge++;

        Debug.Log($"[AGE UPDATE] {animalReference.name} is now {currentAge} years old.");

        if (currentAge >= maxAge)
        {
            Die();
        }
    }

    // Handles the death of the animal
    // Maneja la muerte del animal
    private void Die()
    {
        Debug.Log($"💀 [DEATH EVENT] Animal: {animalReference.name} | Type: {animalReference.GetType().Name} | Race: {animalReference.GetType().Name} has died at age {currentAge}.");
        
        // Destroy the GameObject after a short delay to allow the message to show
        // Destruye el GameObject después de un breve retraso para que se muestre el mensaje
        Destroy(this.gameObject, 3.0f);
    }
}
