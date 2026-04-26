using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI statusText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        statusText.text = "";
    }

    // Метод для показу тимчасового повідомлення
    public void ShowMessage(string message, Color color)
    {
        StopAllCoroutines(); // зупинити попередні повідомлення, якщо вони ще тривають
        StartCoroutine(ShowMessageRoutine(message, color));
    }

    private IEnumerator ShowMessageRoutine(string message, Color color)
    {
        statusText.text = message;
        statusText.color = color;
        yield return new WaitForSeconds(1.5f);
        statusText.text = "";
    }

    public void GoToMainMenu()
    {
        // Обов'язково зупиняємо всі процеси (наприклад, хід бота) перед виходом
        StopAllCoroutines();
        SceneManager.LoadScene("MainMenu");
    }
}