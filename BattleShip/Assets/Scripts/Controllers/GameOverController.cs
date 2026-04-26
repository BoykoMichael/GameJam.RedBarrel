using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;

    private void Start()
    {
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

    public void QuitApplication()
    {
        Debug.Log("Гра закривається...");
        Application.Quit(); // Працює у зібраній грі (.exe / .apk)
    }
}