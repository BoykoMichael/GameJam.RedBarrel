using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;

    private void Start()
    {
        // Визначаємо, який текст показати, виходячи з результату в GameManager
        if (GameManager.IsPlayerWinner)
        {
            resultText.text = "ПЕРЕМОГА!";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = "ВИ ПРОГРАЛИ";
            resultText.color = Color.red;
        }
    }

    // При натисканні "Грати ще" завантажуємо ігрову сцену
    public void RestartGame()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    // При натисканні "Головне меню" повертаємося на початкову сцену
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}