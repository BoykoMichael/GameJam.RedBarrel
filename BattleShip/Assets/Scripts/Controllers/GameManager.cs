using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Board EnemyBoard { get; private set; }
    public Board PlayerBoard { get; private set; }

    public GameState CurrentState { get; private set; }

    public static bool IsPlayerWinner;

    [Header("UI Полів")]
    [SerializeField] private BoardUI playerBoardUI; // Пряме посилання на поле гравця
    [SerializeField] private BoardUI enemyBoardUI;  // Пряме посилання на поле ворога

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        EnemyBoard = new Board();
        PlayerBoard = new Board();

        EnemyBoard.PlaceAllShipsRandomly();
        PlayerBoard.PlaceAllShipsRandomly();

        // Одразу після розстановки показуємо кораблі гравця на його полі
        ShowPlayerShips();

        CurrentState = GameState.PlayerTurn;
        Debug.Log("Гра почалась! Ваш хід.");
    }

    // Метод, який відкриває кораблі на полі гравця
    private void ShowPlayerShips()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (PlayerBoard.Grid[x, y] == CellState.Occupied)
                {
                    playerBoardUI.UpdateCellVisual(x, y, CellState.Occupied);
                }
            }
        }
    }

    public void ProcessPlayerShot(int x, int y)
    {
        if (CurrentState != GameState.PlayerTurn) return;

        Coordinate target = new Coordinate(x, y);
        ShotResult result = EnemyBoard.ReceiveShot(target);

        // Оновлюємо візуал ворожого поля через пряме посилання
        enemyBoardUI.UpdateCellVisual(x, y, EnemyBoard.Grid[x, y]);

        if (result == ShotResult.Miss)
        {
            CurrentState = GameState.EnemyTurn;
            Debug.Log("Ви промахнулись. Хід переходить до ворога.");
            UIManager.Instance.ShowMessage("Мимо!", Color.white);
            StartCoroutine(EnemyTurnRoutine());
        }
        else if (result == ShotResult.Hit)
        {
            UIManager.Instance.ShowMessage("Попав! Стріляйте ще", Color.yellow);
            Debug.Log("Ви влучили! Стріляйте ще раз.");
        }
        else if (result == ShotResult.Destroyed)
        {
            UIManager.Instance.ShowMessage("Потопив!", Color.red);
            if (EnemyBoard.AreAllShipsSunk())
            {
                IsPlayerWinner = true;
                Invoke("LoadGameOver", 2f); // Завантаження через 2 секунди
            }
        }
        else
        {
            Debug.Log("Ви влучили! Стріляйте ще раз.");
        }
    }

    private IEnumerator EnemyTurnRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        bool canShootAgain = true;

        while (canShootAgain && CurrentState == GameState.EnemyTurn)
        {
            int x = Random.Range(0, 10);
            int y = Random.Range(0, 10);

            // перевірка чи вже стріляли в цю клітинку
            CellState targetState = PlayerBoard.Grid[x, y];
            if (targetState == CellState.Miss || targetState == CellState.Hit)
            {
                // Якщо випадково обрали вже обстріляну клітинку — пропускаємо ітерацію і шукаємо нову
                continue;
            }

            // Робимо постріл
            Coordinate target = new Coordinate(x, y);
            ShotResult result = PlayerBoard.ReceiveShot(target);

            Debug.Log($"Ворог вистрілив у {x}:{y}. Результат: {result}");

            // Оновлюємо візуал (ОБОВ'ЯЗКОВО перевірте, чи підключено playerBoardUI в інспекторі!)
            if (playerBoardUI != null)
            {
                playerBoardUI.UpdateCellVisual(x, y, PlayerBoard.Grid[x, y]);
            }

            // Повідомлення через UIManager (з безпечною перевіркою на null)
            if (UIManager.Instance != null)
            {
                if (result == ShotResult.Miss)
                    UIManager.Instance.ShowMessage("Ворог промахнувся!", Color.white);
                else if (result == ShotResult.Hit)
                    UIManager.Instance.ShowMessage("Ворог влучив!", Color.red);
                else
                    UIManager.Instance.ShowMessage("Ворог потопив ваш корабель!", Color.red);
            }

            if (result == ShotResult.Miss)
            {
                // Промах — виходимо з циклу бота, передаємо хід гравцю
                CurrentState = GameState.PlayerTurn;
                canShootAgain = false;
                Debug.Log("Хід перейшов до Гравця");
            }
            else
            {
                // Влучив або потопив
                if (PlayerBoard.AreAllShipsSunk())
                {
                    IsPlayerWinner = false;
                    CurrentState = GameState.GameOver;
                    canShootAgain = false;
                    Invoke("LoadGameOver", 2f);
                }
                else
                {
                    // Чекаємо перед наступним пострілом, щоб гравець встиг побачити результат
                    yield return new WaitForSeconds(1.5f);
                    // canShootAgain залишається true, цикл while йде на нове коло
                }
            }
        }
    }

    private void LoadGameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
    }
}