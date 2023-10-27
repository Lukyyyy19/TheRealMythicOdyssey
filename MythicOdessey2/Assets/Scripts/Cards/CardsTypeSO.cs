using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Type", menuName = "Card Type")]
public class CardsTypeSO : ScriptableObject
{
    public string nameString;
    public Trap prefab;
    public Transform box;
    public Transform prefabGhost;
    public int width;
    public int height;
    public int manaCost;

    public List<Vector2Int> GetGridPositionList(Vector2Int offset)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridPositionList.Add(new Vector2Int(x, y) + offset);
            }
        }

        return gridPositionList;
    }
}
