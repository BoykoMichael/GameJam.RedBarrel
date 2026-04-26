using UnityEngine;

public class BoardUI : MonoBehaviour
{
    [Header("Налаштування генерації")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform gridContainer;
    [SerializeField] private bool isEnemyBoard;

    // посилання на згенеровані клітинки для швидкого доступу
    private CellUI[,] visualCells = new CellUI[10, 10];

    private void Awake()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        string prefix = isEnemyBoard ? "Enemy" : "Player";

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                GameObject cellObj = Instantiate(cellPrefab, gridContainer);
                cellObj.name = $"{prefix}Cell_X{x}_Y{y}";

                CellUI cellUI = cellObj.GetComponent<CellUI>();
                // Передаємо параметр isEnemyBoard у клітинку
                cellUI.Initialize(x, y, isEnemyBoard);

                visualCells[x, y] = cellUI;
            }
        }
        Debug.Log($"Візуальне поле {prefix} успішно згенеровано.");
    }

    // Зручний метод для оновлення кольору конкретної клітинки
    public void UpdateCellVisual(int x, int y, CellState state)
    {
        if (visualCells[x, y] != null)
        {
            visualCells[x, y].UpdateVisuals(state);
        }
    }
}