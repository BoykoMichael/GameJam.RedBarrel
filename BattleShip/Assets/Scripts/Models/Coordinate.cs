using System;
using UnityEngine;

public struct Coordinate
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    // Парсинг рядка типу "A1", "J10" у координати масиву 0-9
    public static Coordinate Parse(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length < 2 || input.Length > 3)
        {
            Debug.Log("Некоректний формат координат!");
            throw new ArgumentException("Некоректний формат координат");
        }

        input = input.ToUpper();
        char columnChar = input[0];
        string rowString = input.Substring(1);

        if (columnChar < 'A' || columnChar > 'J')
        {
            Debug.Log("Стовпець має бути від A до J!");
            throw new ArgumentException("Некоректний стовпець");
        }

        if (!int.TryParse(rowString, out int row) || row < 1 || row > 10)
        {
            Debug.Log("Рядок має бути від 1 до 10!");
            throw new ArgumentException("Некоректний рядок");
        }

        int x = columnChar - 'A';
        int y = row - 1;

        return new Coordinate(x, y);
    }
}
