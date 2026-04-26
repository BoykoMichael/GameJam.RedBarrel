using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Посилання на UI для виведення повідомлень гравцю
    //[SerializeField] private UIManager uiManager;

    // Метод обробки пострілу по заданих координатах
    public void ProcessShot(Board targetBoard, Coordinate targetCoords)
    {
        // Отримуємо результат пострілу з моделі ігрового поля
        ShotResult result = targetBoard.ReceiveShot(targetCoords);

        switch (result) 
        {
            case ShotResult.Miss:
                Debug.Log("Промах! Хід переходить до суперника.");
                //uiManager.ShowMessage("Мимо!");
                SwitchTurn(); // Передача ходу
                break;

            case ShotResult.Hit:
                Debug.Log("Влучення! Гравець отримує додатковий хід.");
                //uiManager.ShowMessage("Попал!");
                // Хід не передається, гравець стріляє ще раз
                break;

            case ShotResult.Sunk:
                Debug.Log("Корабель знищено! Гравець отримує додатковий хід.");
                //uiManager.ShowMessage("Потопил!");

                // Перевірка на завершення гри (чи всі кораблі знищені)
                if (targetBoard.AreAllShipsSunk())
                {
                    Debug.Log("Гру завершено. Перемога!");
                    //uiManager.ShowMessage("Победа!");
                    EndGame();
                }
                // Інакше хід залишається у поточного гравця
                break;
        }
    }

    // Метод передачі ходу
    private void SwitchTurn()
    {
        // Логіка зміни активного гравця
    }

    // Метод завершення гри
    private void EndGame()
    {
        // Зупинка ігрового циклу
    }
}
