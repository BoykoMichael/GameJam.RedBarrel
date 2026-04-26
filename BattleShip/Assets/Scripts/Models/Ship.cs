using UnityEngine;

public class Ship
{
    public ShipType Type { get; private set; }
    public List<Coordinate> OccupiedCells { get; private set; }
    public int Health { get; private set; }

    public Ship(ShipType type, List<Coordinate> cells)
    {
        Type = type;
        OccupiedCells = cells;
        Health = (int)type; // Кількість життів дорівнює довжині корабля
    }

    // Метод обробки влучення по кораблю
    public void TakeDamage()
    {
        Health--;
        if (Health <= 0)
        {
            Debug.Log("Корабель повністю знищено!");
        }
    }

    // Перевірка чи корабель потоплений
    public bool IsDestroyed()
    {
        return Health <= 0;
    }
}
