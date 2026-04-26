using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Board EnemyBoard { get; private set; }
    public Board PlayerBoard { get; private set; } // Додали логічне поле гравця

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        EnemyBoard = new Board();
        PlayerBoard = new Board();

        // Ворог розставляє свої кораблі випадково
        EnemyBoard.PlaceAllShipsRandomly();

        // ТИМЧАСОВО: Гравець теж розставляє кораблі автоматично, 
        // щоб ми могли протестувати стрільбу бота (пізніше зробимо ручну розстановку)
        PlayerBoard.PlaceAllShipsRandomly();

        // Першим ходить гравець
        CurrentState = GameState.PlayerTurn;
        Debug.Log("Гра почалась! Ваш хід.");
    }

    public void ProcessPlayerShot(int x, int y)
    {
        // Якщо зараз не хід гравця або гра закінчилась — ігноруємо кліки по полю
        if (CurrentState != GameState.PlayerTurn) return;

        Coordinate target = new Coordinate(x, y);
        ShotResult result = EnemyBoard.ReceiveShot(target);

        // Оновлюємо візуал ворожого поля (те що ми бачимо)
        UpdateVisualCell(x, y, EnemyBoard.Grid[x, y]);

        if (result == ShotResult.Miss)
        {
            CurrentState = GameState.EnemyTurn; // Передаємо хід
            Debug.Log("Ви промахнулись. Хід переходить до ворога.");

            // Запускаємо логіку бота з невеликою затримкою
            StartCoroutine(EnemyTurnRoutine());
        }
        else if (EnemyBoard.AreAllShipsSunk())
        {
            CurrentState = GameState.GameOver;
            Debug.Log("ВИ ПЕРЕМОГЛИ! Всі кораблі ворога знищено.");
        }
        else
        {
            Debug.Log("Ви влучили! Стріляйте ще раз.");
        }
    }

    // Логіка ходу комп'ютера
    private IEnumerator EnemyTurnRoutine()
    {
        // Чекаємо 1 секунду для реалізму
        yield return new WaitForSeconds(1f);

        bool validShot = false;
        System.Random random = new System.Random();

        while (!validShot && CurrentState == GameState.EnemyTurn)
        {
            // Бот випадково обирає координати
            int x = random.Next(0, 10);
            int y = random.Next(0, 10);

            // Перевіряємо, чи бот ще НЕ стріляв у цю клітинку (щоб не витрачав хід дарма)
            CellState targetCell = PlayerBoard.Grid[x, y];
            if (targetCell == CellState.Empty || targetCell == CellState.Occupied)
            {
                validShot = true;
                Coordinate target = new Coordinate(x, y);

                // Бот стріляе по логічному полю гравця
                ShotResult result = PlayerBoard.ReceiveShot(target);

                Debug.Log($"Ворог стріляє по координатах X={x}, Y={y}");

                // ЗАМІТКА: Тут пізніше ми додамо оновлення ВІЗУАЛУ поля гравця, коли його створимо

                if (result == ShotResult.Miss)
                {
                    CurrentState = GameState.PlayerTurn;
                    Debug.Log("Ворог промахнувся. Ваш хід!");
                }
                else if (PlayerBoard.AreAllShipsSunk())
                {
                    CurrentState = GameState.GameOver;
                    Debug.Log("ВИ ПРОГРАЛИ! Ворог знищив усі ваші кораблі.");
                }
                else
                {
                    Debug.Log("Ворог влучив у ваш корабель і стріляє ще раз!");

                    // Бот чекає ще секунду і цикл повторюється (він стріляє знову)
                    yield return new WaitForSeconds(1f);
                    validShot = false;
                }
            }
        }
    }

    private void UpdateVisualCell(int x, int y, CellState newState)
    {
        GameObject cellObj = GameObject.Find($"Cell_X{x}_Y{y}");
        if (cellObj != null)
        {
            CellUI cellUI = cellObj.GetComponent<CellUI>();
            cellUI.UpdateVisuals(newState);
        }
    }
}