using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Робимо GameManager доступним з будь-якого скрипта
    public static GameManager Instance { get; private set; }

    // Логічне ігрове поле (наразі це поле ворога, по якому ми стріляємо)
    public Board EnemyBoard { get; private set; }

    private void Awake()
    {
        // Налаштування Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 1. Ініціалізуємо математичну модель поля
        EnemyBoard = new Board();

        // 2. ДЛЯ ТЕСТУ: Розміщуємо один крейсер (3 клітинки) по горизонталі 
        // Починаємо з координат X=1, Y=1 (це клітинка B2)
        List<Coordinate> testShipCells = new List<Coordinate>
        {
            new Coordinate(1, 1), // B2
            new Coordinate(2, 1), // C2
            new Coordinate(3, 1)  // D2
        };

        EnemyBoard.PlaceShip(ShipType.Cruiser, testShipCells);
        Debug.Log("Тестовий корабель розміщено на координатах: B2, C2, D2");
    }

    // Метод, який викликається, коли гравець клікає по візуальній клітинці
    public void ProcessPlayerShot(int x, int y)
    {
        Coordinate target = new Coordinate(x, y);

        // Передаємо постріл у логічну модель і отримуємо результат
        ShotResult result = EnemyBoard.ReceiveShot(target);

        // Отримуємо новий статус клітинки з логічного масиву
        CellState newState = EnemyBoard.Grid[x, y];

        // Оновлюємо візуальне відображення цієї клітинки
        UpdateVisualCell(x, y, newState);
    }

    // Пошук візуальної клітинки на сцені та зміна її кольору
    private void UpdateVisualCell(int x, int y, CellState newState)
    {
        // Шукаємо об'єкт за іменем (ми задавали це ім'я в BoardUI)
        GameObject cellObj = GameObject.Find($"Cell_X{x}_Y{y}");

        if (cellObj != null)
        {
            CellUI cellUI = cellObj.GetComponent<CellUI>();
            cellUI.UpdateVisuals(newState);
        }
        else
        {
            Debug.Log($"Помилка: Не знайдено візуальну клітинку Cell_X{x}_Y{y}");
        }
    }
}
