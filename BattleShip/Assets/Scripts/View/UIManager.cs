using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI statusText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Початковий текст порожній
        statusText.text = "";
    }

    // Метод для показу тимчасового повідомлення
    public void ShowMessage(string message, Color color)
    {
        StopAllCoroutines(); // Зупиняємо попередні повідомлення, якщо вони ще тривають
        StartCoroutine(ShowMessageRoutine(message, color));
    }

    private IEnumerator ShowMessageRoutine(string message, Color color)
    {
        statusText.text = message;
        statusText.color = color;

        // Чекаємо 1.5 секунди
        yield return new WaitForSeconds(1.5f);

        // Очищуємо текст
        statusText.text = "";
    }
}