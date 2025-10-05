using UnityEngine;
// Quitar "using UnityEditor;" si no es necesario.

public abstract class Animal : MonoBehaviour, IAnimalBehavior
{
    
    [SerializeField] protected int health_points;
    [SerializeField] protected int life_time;
    [SerializeField] protected AudioClip animal_sound;
    [SerializeField] protected string public_name;
    [SerializeField] protected string animal_race;
    [SerializeField] protected float speed_animal;

    // Constructor o inicializador
    protected virtual void Start()
    {
        // ¡Acceso correcto a la constante!
        health_points = AnimalConstants.MAX_HEALTH; 
        
        Debug.Log($"{public_name} ({animal_race}) creado con {health_points} puntos de vida.");
    }

    public abstract void Eat();
    public abstract void MakeSound();
    public abstract void Move();
}