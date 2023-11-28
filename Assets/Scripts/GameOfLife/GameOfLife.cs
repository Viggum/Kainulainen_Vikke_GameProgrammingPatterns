using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public enum startSeed
    {
        Blinker,
        Toad,
        Beacon,
        Pentadecathlon,
        Glider
    }

    public startSeed seed;

    public GameObject TileObject;
    private static readonly int Width = 50;
    private static readonly int Height = 50;
    bool[,] current_grid = new bool[Width, Height];
    bool[,] next_grid = new bool[Width, Height];
    public GameObject[,] tiles = new GameObject[Width, Height];

    private float timeAccu = 0.0f;
    [Range(0, 2f)]public float timeDelay = 0.0f;

    public Color cellColor;

    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                current_grid[x, y] = false;

                tiles[x, y] = Instantiate(TileObject, new Vector3(x, 0 , y) * 1.05f, TileObject.transform.rotation, transform);

                //tiles[x, y].SetActive(false);
                tiles[x, y].GetComponent<Renderer>().material.color = Color.black;
            }               
        }

        // Set start pattern
        switch (seed)
        {
            case startSeed.Blinker:
                Blinker();
                break;
            case startSeed.Toad:
                Toad(); 
                break;
            case startSeed.Beacon: 
                Beacon(); 
                break;               
            case startSeed.Pentadecathlon: 
                Pentadecathlon(); 
                break;
            case startSeed.Glider:
                Glider();
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        timeAccu += Time.deltaTime;

        if (timeAccu > timeDelay)
        {
            // RULES:
            // 1. Fewer than 2 live neighbours --> die
            // 2. 2 or 3 live neighbours -> live on
            // 3. more than 3 live neighbours -> die
            // 4. dead cell with 3 live neighbours -> rebirth

            UpdateNextGrid();
            StepGrids();
            UpdateTiles();           

            timeAccu = 0;
        }
    }

    public void UpdateNextGrid()
    {
        // P‰ivitt‰‰ nykyisest‰ gridist‰ (grid_current[,]) kaikki solut seuraavaa tilaa kuvaavaan gridiin (grid_next[,]).
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Debug.Log("For " + x + ", " + y);
                int live = getLiveNeighbours(x, y);

                // 1. Fewer than 2 live neighbours --> die
                if (live < 2)
                    next_grid[x, y] = false;
                // 2. 2 or 3 live neighbours -> live on
                else if (live < 4 && current_grid[x, y] == true)
                {
                    next_grid[x, y] = true;
                    // live on... do nothing
                }
                // 3. more than 3 live neighbours -> die
                else if (live > 3 && next_grid[x, y] == true)
                    next_grid[x, y] = false;
                // 4. dead cell with 3 live neighbours -> rebirth
                else if (live == 3 && next_grid[x, y] == false)
                    next_grid[x, y] = true;

            }
        }
    }

    public void StepGrids()
    {
        // kopioi grid_next[,]-muuttujan datan grid_current[,]-muuttujaan solu kerrallaan.
        for(int x = 0; x < Width; x++)
        {
            for(int y = 0;  y < Height; y++)
                current_grid[x, y] = next_grid[x, y];
        }
    }

    public void UpdateTiles()
    {
        // k‰y l‰pi gridin ja p‰ivitt‰‰ tiles[,]-muuttujassa olevien peliobjektien v‰rin.

        // Update tiles
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (current_grid[x, y] == true)
                    tiles[x, y].GetComponent<Renderer>().material.color = cellColor;
                else
                    tiles[x, y].GetComponent<Renderer>().material.color = Color.black;
            }
        }
    }

    public int getLiveNeighbours(int x, int y)
    {
        int liveNeighbours = 0;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                
                if (!(i == x && j == y) && i >= 0 && j >= 0 && i < Width && j < Height)
                {
                    // Current i,j is not x,y
                    if (current_grid[i, j] == true)
                    {
                        liveNeighbours++;
                        Debug.Log(i + ", " + j + " is alive");
                    }
                }
            }
        }

        return liveNeighbours;
    }

    public void SetCell(int x, int y)
    {
        current_grid[x, y] = true;
        tiles[x, y].GetComponent<Renderer>().material.color = cellColor;
    }

    public void Blinker()
    {
        SetCell(4, 5);
        SetCell(5, 5);
        SetCell(6, 5);

    }

    public void Toad()
    {
        SetCell(5, 3);
        SetCell(5, 4);
        SetCell(5, 5);
        SetCell(4, 4);
        SetCell(4, 5);
        SetCell(4, 6);
    }

    public void Beacon()
    {
        SetCell(3, 5);
        SetCell(4, 5);
        SetCell(3, 6);
        SetCell(4, 6);
        SetCell(5, 4);
        SetCell(6, 4);
        SetCell(5, 3);
        SetCell(6, 3);
    }

    public void Pentadecathlon()
    {
        SetCell(5, 4);
        SetCell(5, 5);
        SetCell(4, 6);
        SetCell(6, 6);
        SetCell(5, 7);
        SetCell(5, 8);
        SetCell(5, 9);
        SetCell(5, 10);
        SetCell(4, 11);
        SetCell(6, 11);
        SetCell(5, 12);
        SetCell(5, 13);
    }

    public void Glider()
    {
        SetCell(1, 47);
        SetCell(2, 46);
        SetCell(3, 46);
        SetCell(3, 47);
        SetCell(3, 48);
    }
}


