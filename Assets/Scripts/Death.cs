using UnityEngine;

public class Death : MonoBehaviour
{
    [Header("Animal Life Settings / Configuración de Vida del Animal")]
    [Tooltip("Maximum age before death / Edad máxima antes de morir")]
    [SerializeField] private int maxAge = 5;

    [Tooltip("How many seconds per age increase / Cuántos segundos por incremento de edad")]
    [SerializeField] private float ageInterval = 5f;

    private int currentAge = 0;
    private Animal animalReference;
    private float ageTimer = 0f;

    private void Start()
    {
        // Get the Animal component reference
        animalReference = GetComponent<Animal>();

        if (animalReference == null)
        {
            Debug.LogWarning($"[WARNING] No 'Animal' component found on {gameObject.name}.");
            return;
        }

        // Initialize this animal's life individually
        currentAge = 1;
        Debug.Log($"[LIFE START] {animalReference.name} (Type: {animalReference.GetType().Name}) born with max age {maxAge}.");
    }

    private void Update()
    {
        // Count time individually for this animal
        ageTimer += Time.deltaTime;

        if (ageTimer >= ageInterval)
        {
            ageTimer = 0f; // reset timer
            AgeUp();
        }
    }

    private void AgeUp()
    {
        currentAge++;
        Debug.Log($"[AGE UPDATE] {animalReference.name} is now {currentAge} years old.");

        if (currentAge >= maxAge)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"💀 [DEATH EVENT] Animal '{animalReference.name}' (Type: {animalReference.GetType().Name}) died at age {currentAge}.");

        // Optional: Notify GameManager or Spawner that this slot is now free
        // GameManager.Instance.FreeSlot(animalReference);

        Destroy(gameObject, 2.0f);
    }
}