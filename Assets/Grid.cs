using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    public int gridSizeX;
    public int gridSizeY;
    public GameObject cellPrefab;
    public GemBase gemPrefab;
    private GameObject[,] grid;

    private GemBase[,] gemGrid;


    public GemBase[,] GemGrid => gemGrid;

    public GameObject[,] CellGrid => grid;

    [SerializeField] private Sprite[] _gemSprites;

    private float fallSpeed = 5f;
    private bool isFalling = false;

    public bool IsFalling => isFalling;

    [SerializeField] private Timer _timer;

    private bool _isDestroyed;

    void OnEnable()
    {
        CreateGrid();
        GenerateGems();
        if (_isDestroyed)
        {
            ReshuffleGrid();
        }

        _timer.StartTimer();
        Debug.Log("OnEnable");
    }

    private void OnDisable()
    {
        DestroyGems();
        DestroyGrid();
        _isDestroyed = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void CreateGrid()
    {
        if (_isDestroyed)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    grid[x, y].SetActive(true);
                }
            }
        }

        else
        {
            grid = new GameObject[gridSizeX, gridSizeY];

            // Instantiate cells
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    // Instantiate cell at the calculated position
                    var cell = Instantiate(cellPrefab,
                        new Vector3(x * CustomConstantAttribute.CELL_OFFSET, y * CustomConstantAttribute.CELL_OFFSET,
                            0), Quaternion.identity);
                    grid[x, y] = cell;
                }
            }
        }
    }

    void GenerateGems()
    {
        if (_isDestroyed)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    gemGrid[x, y].gameObject.SetActive(true);
                }
            }
        }

        else
        {
            gemGrid = new GemBase[gridSizeX, gridSizeY];

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    gemGrid[x, y] = InstantiateRandomGem(x, y);
                }
            }

            if (!HasMatch())
            {
                ReshuffleGrid();
            }
        }
    }


    void DestroyGrid()
    {
        // Instantiate cells
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                grid[x, y].gameObject.SetActive(false);

                /*Destroy(grid[x, y].gameObject);
                grid[x, y] = null;*/
            }
        }
    }

    void DestroyGems()
    {
        for (int x = 0; x < gemGrid.GetLength(0); x++)
        {
            for (int y = 0; y < gemGrid.GetLength(1); y++)
            {
                gemGrid[x, y].gameObject.SetActive(false);
                /*Destroy(gemGrid[x, y].gameObject);
                gemGrid[x, y] = null;*/
            }
        }
    }

    public void FallGems()
    {
        isFalling = true;
        StartCoroutine(FallCoroutine());
        _timer.ResetTimer();
    }

    IEnumerator FallCoroutine()
    {
        bool gemsFalling = true;

        while (gemsFalling)
        {
            gemsFalling = false;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 1; y < gridSizeY; y++)
                {
                    var gem = gemGrid[x, y];

                    if (gem == null)
                    {
                        continue;
                    }

                    var gemBelow = gemGrid[x, y - 1];
                    //Debug.Log($"{x} {y-1}");

                    if (gemBelow == null)
                    {
                        // Gem is falling down
                        gemGrid[x, y - 1] = gem;
                        gemGrid[x, y] = null;
                        gem.transform.position -= Vector3.up * CustomConstantAttribute.CELL_OFFSET;
                        gemsFalling = true;
                    }
                }
            }

            yield return new WaitForSeconds(1f / fallSpeed);
        }

        // Generate new gems to fill the top row
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (gemGrid[x, y] == null)
                {
                    var newGem = InstantiateRandomGem(x, y);
                    gemGrid[x, y] = newGem;
                }
            }
        }

        isFalling = false;
    }

    GemBase InstantiateRandomGem(int x, int y)
    {
        var gem = Instantiate(gemPrefab,
            new Vector3(x * CustomConstantAttribute.CELL_OFFSET, y * CustomConstantAttribute.CELL_OFFSET, 0),
            Quaternion.identity);
        var random = Random.Range(0, _gemSprites.Length);
        gem.ChangeGem(_gemSprites[random], random);
        return gem;
    }

    public void ReshuffleGrid()
    {
        StartCoroutine(ReshuffleCoroutine());
    }

    IEnumerator ReshuffleCoroutine()
    {
        yield return new WaitForSeconds(0.3f);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Destroy(gemGrid[x, y].gameObject);
                var gem = InstantiateRandomGem(x, y);
                gemGrid[x, y] = gem;
            }
        }

        if (!HasMatch())
        {
            ReshuffleGrid(); // If still no match, reshuffle again
        }
    }

    public bool HasMatch()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Check horizontally
                if (x < gridSizeX - 2)
                {
                    if (gemGrid[x, y].GetComponent<GemBase>().gemType ==
                        gemGrid[x + 1, y].GetComponent<GemBase>().gemType &&
                        gemGrid[x, y].GetComponent<GemBase>().gemType ==
                        gemGrid[x + 2, y].GetComponent<GemBase>().gemType)
                    {
                        return true;
                    }
                }

                // Check vertically
                if (y < gridSizeY - 2)
                {
                    if (gemGrid[x, y].GetComponent<GemBase>().gemType ==
                        gemGrid[x, y + 1].GetComponent<GemBase>().gemType &&
                        gemGrid[x, y].GetComponent<GemBase>().gemType ==
                        gemGrid[x, y + 2].GetComponent<GemBase>().gemType)
                    {
                        return true;
                    }
                }
            }
        }

        return false; // No matches found
    }
}

public static class CustomConstantAttribute
{
    public const float CELL_OFFSET = 0.75f;
}