using UnityEngine;
using UnityEngine.UI;


public class CellUI : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }
    private bool isEnemyBoard; // Зберігаємо, чиє це поле

    private Button button;
    private Image image;

    // Додали параметр isEnemy
    public void Initialize(int x, int y, bool isEnemy)
    {
        X = x;
        Y = y;
        isEnemyBoard = isEnemy;

        button = GetComponent<Button>();
        image = GetComponent<Image>();

        button.onClick.AddListener(OnCellClicked);
    }

    private void OnCellClicked()
    {
        // Якщо це наше поле (не вороже), стріляти не можна
        if (!isEnemyBoard)
        {
            Debug.Log("Це ваше поле! Стріляйте по ворожому.");
            return;
        }

        Debug.Log($"Гравець стріляє у клітинку ворога: X={X}, Y={Y}");
        GameManager.Instance.ProcessPlayerShot(X, Y);
    }

    public void UpdateVisuals(CellState state)
    {
        switch (state)
        {
            case CellState.Empty:
                image.color = Color.white;
                break;
            case CellState.Occupied:
                // Якщо це поле гравця, показуємо кораблі зеленим, якщо ворога - ховаємо під білим
                image.color = isEnemyBoard ? Color.white : Color.green;
                break;
            case CellState.Miss:
                image.color = Color.gray;
                break;
            case CellState.Hit:
                image.color = Color.red;
                break;
        }
    }
}