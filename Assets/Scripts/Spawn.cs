using UnityEngine;

public class Spawn : MonoBehaviour
{
    [Header("Spawner Settings / Configuración del Spawner")]
    [Tooltip("Prefab to spawn when space key is pressed / Prefab que se instanciará al presionar la barra espaciadora")]
    [SerializeField] public GameObject animalPrefab;

    [Tooltip("Spawn point reference / Referencia al punto de aparición")]
    [SerializeField] private Transform[] spawnPoint;

    [Tooltip("Enable to spawn multiple animals / Habilitar para generar múltiples animales")]
    [SerializeField] private bool allowMultipleSpawn = true;

    private GameObject currentAnimalInstance;

    // 🔹 New array to track if a spawn point is occupied
    private bool[] spawnPointUsed;

    private void Start()
    {
        if (spawnPoint == null || spawnPoint.Length == 0)
        {
            spawnPoint = new Transform[] { this.transform };
        }

        // Initialize slot tracking
        spawnPointUsed = new bool[spawnPoint.Length];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnAtRandomSpawnPoint();
        }
    }

    private void SpawnAtRandomSpawnPoint()
    {
        if (animalPrefab == null)
        {
            Debug.LogError("No animal prefab assigned to the spawner!");
            return;
        }

        // 🔹 Find a free spawn slot
        int freeIndex = GetFreeSpawnPoint();
        if (freeIndex == -1)
        {
            Debug.LogWarning("No available spawn points — all are full!");
            return;
        }

        Vector3 spawnPosition = spawnPoint[freeIndex].position;
        Quaternion spawnRotation = spawnPoint[freeIndex].rotation;

        // Instantiate the prefab
        GameObject newAnimal = Instantiate(animalPrefab, spawnPosition, spawnRotation);

        // Mark the slot as used
        spawnPointUsed[freeIndex] = true;

        // Optional: store reference if single spawn only
        if (!allowMultipleSpawn)
        {
            currentAnimalInstance = newAnimal;
        }

        Debug.Log($"Spawned animal at slot {freeIndex}");
    }

    // 🔹 Finds an available spawn point (returns -1 if none)
    private int GetFreeSpawnPoint()
    {
        for (int i = 0; i < spawnPointUsed.Length; i++)
        {
            if (!spawnPointUsed[i])
                return i;
        }
        return -1;
    }

    // 🔹 Public method you can call when animal dies/sells
    public void FreeSpawnSlot(int index)
    {
        if (index < 0 || index >= spawnPointUsed.Length)
        {
            Debug.LogError("Invalid spawn index!");
            return;
        }

        spawnPointUsed[index] = false;
        Debug.Log($"Slot {index} is now free.");
    }
}