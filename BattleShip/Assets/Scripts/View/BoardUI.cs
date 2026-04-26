using UnityEngine;

public class BoardUI : MonoBehaviour
{
    [Header("Налаштування генерації")]
    [SerializeField] public GameObject cellPrefab;    // Посилання на префаб клітинки
    [SerializeField] public Transform gridContainer;  // Посилання на панель PlayerBoardUI

    // Двовимірний масив для швидкого доступу до візуальних клітинок
    public CellUI[,] visualCells = new CellUI[10, 10];

    public void Start()
    {
        GenerateBoard();
    }

    public void GenerateBoard()
    {
        // Проходимо циклами для створення сітки 10х10
        // Увага: y=0 - це верхній рядок у UI, тому генеруємо зверху вниз
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                // Створюємо копію префабу і робимо її дочірньою для gridContainer
                GameObject cellObj = Instantiate(cellPrefab, gridContainer);

                // Називаємо об'єкт для зручності відлагодження
                cellObj.name = $"Cell_X{x}_Y{y}";

                // Ініціалізуємо скрипт клітинки її координатами
                CellUI cellUI = cellObj.GetComponent<CellUI>();
                cellUI.Initialize(x, y);

                visualCells[x, y] = cellUI;
            }
        }

        Debug.Log("Візуальне ігрове поле 10х10 успішно згенеровано!");
    }
}
