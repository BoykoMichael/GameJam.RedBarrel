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
        yield return new WaitForSeconds(1f);

        bool validShot = false;
        System.Random random = new System.Random();

        while (!validShot && CurrentState == GameState.EnemyTurn)
        {
            int x = random.Next(0, 10);
            int y = random.Next(0, 10);

            CellState targetCell = PlayerBoard.Grid[x, y];
            if (targetCell == CellState.Empty || targetCell == CellState.Occupied)
            {
                validShot = true;
                Coordinate target = new Coordinate(x, y);

                ShotResult result = PlayerBoard.ReceiveShot(target);
                Debug.Log($"Ворог стріляє по координатах X={x}, Y={y}");

                // Оновлюємо візуал поля гравця через пряме посилання
                playerBoardUI.UpdateCellVisual(x, y, PlayerBoard.Grid[x, y]);

                if (result == ShotResult.Miss)
                {
                    UIManager.Instance.ShowMessage("Ворог промахнувся!", Color.white);
                    CurrentState = GameState.PlayerTurn;
                    Debug.Log("Ворог промахнувся. Ваш хід!");
                }
                else
                {
                    UIManager.Instance.ShowMessage("Ворог влучив!", Color.red);
                    if (PlayerBoard.AreAllShipsSunk())
                    {
                        IsPlayerWinner = false;
                        Invoke("LoadGameOver", 2f);
                    }
                }
            }
        }
    }

    private void LoadGameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
    }
}