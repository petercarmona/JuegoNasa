using UnityEngine;

public interface IAnimalBehavior
{

    // Métodos que todos los animales deben implementar
    void Eat();
    void MakeSound();
    void Move();
}
