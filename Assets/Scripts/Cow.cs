using UnityEngine;

// The Cow class inherits from Animal and represents a specific animal type.
// La clase Cow hereda de Animal y representa un tipo específico de animal.
public class Cow : Animal
{
    [Header("Cow Attributes")]
    public float milkProduction; // liters of milk per day / litros de leche por día
    public bool isMilking; // whether the cow is currently being milked / si está siendo ordeñada
    public int age; // cow’s age in years / edad de la vaca en años

    // This method is called when the object is created
    // Este método se llama cuando el objeto es creado
    protected override void Start()
    {
        // Basic cow data / Datos básicos de la vaca
        public_name = "Cow";
        animal_race = "Holstein";
        speed_animal = 1.0f;
        milkProduction = 10.5f;
        isMilking = false;
        age = 1;

        base.Start();

        // Subscribe to the GameManager event
        // Suscribirse al evento del GameManager
        GameManager.OnTimeIntervalPassed += IncreaseAge;
    }

    // Called when the object is destroyed to avoid memory leaks
    // Se llama cuando el objeto se destruye para evitar fugas de memoria
    private void OnDestroy()
    {
        GameManager.OnTimeIntervalPassed -= IncreaseAge;
    }

    // Cow eats grass / La vaca come pasto
    public override void Eat()
    {
        Debug.Log($"{public_name} is eating grass.");
    }

    // Cow makes a sound / La vaca hace su sonido
    public override void MakeSound()
    {
        Debug.Log("Moo!");
        if (animal_sound != null)
            AudioSource.PlayClipAtPoint(animal_sound, transform.position);
    }

    // Cow movement / Movimiento de la vaca
    public override void Move()
    {
        transform.Translate(Vector3.forward * speed_animal * Time.deltaTime);
    }

    // Produces milk / Produce leche
    public void ProduceMilk()
    {
        if (!isMilking)
        {
            Debug.Log($"{public_name} is producing {milkProduction} liters of milk.");
            isMilking = true;
        }
    }

    // Increases the age of the cow when time interval passes
    // Aumenta la edad de la vaca cuando pasa un intervalo de tiempo
    private void IncreaseAge()
    {
        age++;
        Debug.Log($"{public_name} has grown older! Current age: {age}");
    }
}
