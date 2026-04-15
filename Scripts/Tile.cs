using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    public GridManager gridManager;

    public void SetPosition(int newX, int newY)
    {
        x = newX;
        y = newY;
    }
}
