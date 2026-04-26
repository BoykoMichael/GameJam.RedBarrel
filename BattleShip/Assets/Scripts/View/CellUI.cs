using UnityEngine;
using UnityEngine.UI;


public class CellUI : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }

    private Button button;
    private Image image;

    // Метод ініціалізації клітинки після її створення
    public void Initialize(int x, int y)
    {
        X = x;
        Y = y;

        button = GetComponent<Button>();
        image = GetComponent<Image>();

        // Додаємо слухача події натискання на кнопку
        button.onClick.AddListener(OnCellClicked);
    }

    private void OnCellClicked()
    {
        Debug.Log($"Гравець стріляє у клітинку: X={X}, Y={Y}");

        // Викликаємо метод пострілу в GameManager і передаємо координати цієї клітинки
        GameManager.Instance.ProcessPlayerShot(X, Y);
    }

    // Метод для візуального оновлення клітинки
    public void UpdateVisuals(CellState state)
    {
        switch (state)
        {
            case CellState.Empty:
            case CellState.Occupied:
                image.color = Color.white; // Поки не стріляли, кораблі приховуємо (або показуємо для свого поля)
                break;
            case CellState.Miss:
                image.color = Color.gray; // Стріляли у воду
                break;
            case CellState.Hit:
                image.color = Color.red; // Влучили у корабель
                break;
        }
    }
}
