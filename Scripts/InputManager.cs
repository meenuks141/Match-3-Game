using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    private Tile firstTile;

    public GridManager gridManager;
    public GameManager gameManager;

    void Update()
    {
        // ❗ STOP INPUT WHEN GAME OVER
        if(gameManager.gameOver) return;

        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if(hit.collider != null)
            {
                Tile clicked = hit.collider.GetComponent<Tile>();

                if(firstTile == null)
                {
                    firstTile = clicked;
                }
                else
                {
                    Swap(firstTile, clicked);
                    firstTile = null;
                }
            }
        }
    }

    void Swap(Tile a, Tile b)
    {
        Vector2 posA = a.transform.position;
        Vector2 posB = b.transform.position;

        // Swap visually
        a.transform.position = posB;
        b.transform.position = posA;

        // Swap coordinates
        int tempX = a.x;
        int tempY = a.y;

        a.SetPosition(b.x, b.y);
        b.SetPosition(tempX, tempY);

        // Update grid
        gridManager.SetTile(a.x, a.y, a.gameObject);
        gridManager.SetTile(b.x, b.y, b.gameObject);

        // Check matches
        List<Tile> matches = gridManager.FindMatches();

        if(matches.Count > 0)
        {
            // ✅ USE MOVE ONLY FOR VALID SWAP
            gameManager.UseMove();

            gridManager.ClearMatches(matches);
            StartCoroutine(ProcessMatches());
        }
        else
        {
            // Swap back if invalid
            a.transform.position = posA;
            b.transform.position = posB;

            a.SetPosition(tempX, tempY);
            b.SetPosition(a.x, a.y);

            gridManager.SetTile(a.x, a.y, a.gameObject);
            gridManager.SetTile(b.x, b.y, b.gameObject);
        }
    }

    IEnumerator ProcessMatches()
    {
        yield return new WaitForSeconds(0.2f);

        gridManager.ApplyGravity();

        yield return new WaitForSeconds(0.2f);

        gridManager.SpawnNewTiles();

        yield return new WaitForSeconds(0.2f);

        List<Tile> newMatches = gridManager.FindMatches();

        if(newMatches.Count > 0)
        {
            gridManager.ClearMatches(newMatches);
            StartCoroutine(ProcessMatches());
        }
    }
}