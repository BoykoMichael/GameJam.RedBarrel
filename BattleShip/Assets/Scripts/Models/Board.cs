using UnityEngine;

public class Board
{
    private const int SIZE = 10;
    public CellState[,] Grid { get; private set; }
    public List<Ship> Ships { get; private set; }

    public Board()
    {
        Grid = new CellState[SIZE, SIZE];
        Ships = new List<Ship>();

        // Ініціалізація поля порожньою водою
        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                Grid[x, y] = CellState.Empty;
            }
        }
    }

    // Перевірка можливості розміщення корабля (правило: без дотиків та перетинів)
    public bool CanPlaceShip(List<Coordinate> cells)
    {
        foreach (var cell in cells)
        {
            // Перевірка виходу за межі ігрового поля
            if (cell.X < 0 || cell.X >= SIZE || cell.Y < 0 || cell.Y >= SIZE)
            {
                Debug.Log("Помилка: Корабель виходить за межі поля!");
                return false;
            }

            // Сканування зони 3х3 навколо поточної клітинки корабля
            // Це гарантує перевірку на дотик по горизонталі, вертикалі та діагоналі
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int checkX = cell.X + dx;
                    int checkY = cell.Y + dy;

                    // Якщо сусідня клітинка знаходиться в межах поля
                    if (checkX >= 0 && checkX < SIZE && checkY >= 0 && checkY < SIZE)
                    {
                        if (Grid[checkX, checkY] == CellState.Occupied)
                        {
                            Debug.Log("Помилка: Не можна ставити корабель впритул до іншого!");
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    // Розміщення корабля на полі
    public bool PlaceShip(ShipType type, List<Coordinate> cells)
    {
        if (!CanPlaceShip(cells))
        {
            return false;
        }

        Ship newShip = new Ship(type, cells);
        Ships.Add(newShip);

        // Змінюємо статус клітинок на зайняті
        foreach (var cell in cells)
        {
            Grid[cell.X, cell.Y] = CellState.Occupied;
        }

        Debug.Log($"Корабель {type} успішно розміщено на полі.");
        return true;
    }

    // Обробка пострілу по полю
    public ShotResult ReceiveShot(Coordinate target)
    {
        if (target.X < 0 || target.X >= SIZE || target.Y < 0 || target.Y >= SIZE)
        {
            Debug.Log("Постріл за межі поля!");
            return ShotResult.Miss;
        }

        CellState currentState = Grid[target.X, target.Y];

        if (currentState == CellState.Empty)
        {
            Grid[target.X, target.Y] = CellState.Miss;
            Debug.Log("Мимо!");
            return ShotResult.Miss;
        }
        else if (currentState == CellState.Occupied)
        {
            Grid[target.X, target.Y] = CellState.Hit;

            // Пошук корабля, у який влучили
            foreach (var ship in Ships)
            {
                foreach (var cell in ship.OccupiedCells)
                {
                    if (cell.X == target.X && cell.Y == target.Y)
                    {
                        ship.TakeDamage();
                        if (ship.IsDestroyed())
                        {
                            Debug.Log("Потопив!");
                            return ShotResult.Destroyed;
                        }
                        else
                        {
                            Debug.Log("Попав!");
                            return ShotResult.Hit;
                        }
                    }
                }
            }
        }
        else if (currentState == CellState.Miss || currentState == CellState.Hit)
        {
            Debug.Log("У цю клітинку вже стріляли!");
            // Повертаємо промах, або у контролері можна зробити так, 
            // щоб гравець не втрачав хід за помилковий клік у ту саму клітинку
            return ShotResult.Miss;
        }

        return ShotResult.Miss;
    }

    // Перевірка чи всі кораблі на полі потоплені (умова завершення гри)
    public bool AreAllShipsSunk()
    {
        foreach (var ship in Ships)
        {
            if (!ship.IsDestroyed())
            {
                return false;
            }
        }
        return true;
    }
}
