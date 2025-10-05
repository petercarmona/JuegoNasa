using UnityEngine;

public class Spawn : MonoBehaviour
{
    [Header("Spawner Settings / Configuración del Spawner")]
    [Tooltip("Prefab to spawn when space key is pressed / Prefab que se instanciará al presionar la barra espaciadora")]
    [SerializeField] public GameObject animalPrefab;

    [Tooltip("Spawn point reference / Referencia al punto de aparición")]
    [SerializeField] private Transform spawnPoint;

    [Tooltip("Enable to spawn multiple animals / Habilitar para generar múltiples animales")]
    [SerializeField] private bool allowMultipleSpawn = true;

    private GameObject currentAnimalInstance;

    private void Start()
    {
        // If no spawnPoint is set, use this GameObject's transform
        // Si no se define un spawnPoint, usa el transform del GameObject actual
        if (spawnPoint == null)
        {
            spawnPoint = this.transform;
        }

        Debug.Log($"Spawner initialized at position: {spawnPoint.position}");
    }

    private void Update()
    {
        // Detects space key press to trigger spawn
        // Detecta si se presiona la tecla Espacio para generar un objeto
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnObject();
        }
    }

    // Function that creates an animal prefab at the spawn point position
    // Función que crea un prefab de animal en la posición del punto de aparición
    private void SpawnObject()
    {
        // Prevent spawning multiple animals if not allowed
        // Previene crear múltiples instancias si no está permitido
        if (!allowMultipleSpawn && currentAnimalInstance != null)
        {
            Debug.LogWarning("An animal is already spawned! Enable 'Allow Multiple Spawn' if needed.");
            return;
        }

        if (animalPrefab == null)
        {
            Debug.LogError("No animal prefab assigned to the spawner!");
            return;
        }

        // Get the position and rotation of the spawn point
        // Obtiene la posición y rotación del punto de aparición
        Vector3 spawnPosition = spawnPoint.position;
        Quaternion spawnRotation = Quaternion.identity;

        // Instantiate the prefab at the position and rotation
        // Instancia el prefab en la posición y rotación especificadas
        currentAnimalInstance = Instantiate(animalPrefab, spawnPosition, spawnRotation);

        Debug.Log($"[SPAWN] Created new animal '{animalPrefab.name}' at position {spawnPosition}");
    }
}