using UnityEngine;

public enum CellState
{
    Empty,    // Порожня вода
    Occupied, // Зайнята цілим кораблем
    Miss,     // Промах (стріляли у воду)
    Hit       // Влучення (підбита частина корабля)
}
