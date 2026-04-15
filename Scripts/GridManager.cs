using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;

    public GameObject[] tilePrefabs;

    private GameObject[,] grid;

    void Start()
    {
        grid = new GameObject[width, height];
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                SpawnTile(x, y);
            }
        }
    }

void SpawnTile(int x, int y)
{
    int rand = 0;
    int attempts = 0;

    do
    {
        rand = Random.Range(0, tilePrefabs.Length);
        attempts++;
    }
    while (CreatesMatchAt(x, y, rand) && attempts < 10);

    Vector2 position = new Vector2(
        x - width / 2f + 0.5f,
        y - height / 2f + 0.5f
    );

    GameObject tile = Instantiate(tilePrefabs[rand], position, Quaternion.identity);

    grid[x, y] = tile;

    Tile t = tile.GetComponent<Tile>();
    t.SetPosition(x, y);
    t.gridManager = this;
}
    // ✅ PREVENT INITIAL MATCHES
    bool CreatesMatchAt(int x, int y, int tileIndex)
    {
        string tagToCheck = tilePrefabs[tileIndex].tag;

        // Horizontal check
        if (x >= 2)
        {
            if (grid[x - 1, y] != null && grid[x - 2, y] != null)
            {
                if (grid[x - 1, y].tag == tagToCheck &&
                    grid[x - 2, y].tag == tagToCheck)
                {
                    return true;
                }
            }
        }

        // Vertical check
        if (y >= 2)
        {
            if (grid[x, y - 1] != null && grid[x, y - 2] != null)
            {
                if (grid[x, y - 1].tag == tagToCheck &&
                    grid[x, y - 2].tag == tagToCheck)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // ✅ FIND MATCHES
    public List<Tile> FindMatches()
    {
        List<Tile> matches = new List<Tile>();

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(grid[x,y] == null) continue;

                // Horizontal
                if(x < width - 2)
                {
                    if(grid[x+1,y] != null && grid[x+2,y] != null)
                    {
                        if(grid[x,y].tag == grid[x+1,y].tag &&
                           grid[x,y].tag == grid[x+2,y].tag)
                        {
                            matches.Add(grid[x,y].GetComponent<Tile>());
                            matches.Add(grid[x+1,y].GetComponent<Tile>());
                            matches.Add(grid[x+2,y].GetComponent<Tile>());
                        }
                    }
                }

                // Vertical
                if(y < height - 2)
                {
                    if(grid[x,y+1] != null && grid[x,y+2] != null)
                    {
                        if(grid[x,y].tag == grid[x,y+1].tag &&
                           grid[x,y].tag == grid[x,y+2].tag)
                        {
                            matches.Add(grid[x,y].GetComponent<Tile>());
                            matches.Add(grid[x,y+1].GetComponent<Tile>());
                            matches.Add(grid[x,y+2].GetComponent<Tile>());
                        }
                    }
                }
            }
        }

        return matches;
    }

  public GameManager gameManager;

public void ClearMatches(List<Tile> matches)
{
    foreach(Tile t in matches)
    {
        if(t != null)
        {
            grid[t.x, t.y] = null;
            Destroy(t.gameObject);
        }
    }

    // 🔥 ADD SCORE
    gameManager.AddScore(matches.Count * 10);
}

    // ✅ UPDATE GRID
    public void SetTile(int x, int y, GameObject tile)
    {
        if(x >= 0 && x < width && y >= 0 && y < height)
        {
            grid[x, y] = tile;
        }
    }

    // ✅ GRAVITY
    public void ApplyGravity()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(grid[x,y] == null)
                {
                    for(int k = y + 1; k < height; k++)
                    {
                        if(grid[x,k] != null)
                        {
                            grid[x,y] = grid[x,k];
                            grid[x,k] = null;

                            grid[x,y].transform.position = new Vector2(
                                x - width / 2f + 0.5f,
                                y - height / 2f + 0.5f
                            );

                            grid[x,y].GetComponent<Tile>().SetPosition(x,y);

                            break;
                        }
                    }
                }
            }
        }
    }

    // ✅ SPAWN NEW TILES
    public void SpawnNewTiles()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(grid[x,y] == null)
                {
                    SpawnTile(x, y);
                }
            }
        }
    }
}