using UnityEngine;

public class ClickMechanics : MonoBehaviour
{
    MineField minefield;
    public SpriteController spriteController;
    Tile tile;

    bool selected;

    // Start is called before the first frame update
    void Start()
    {
        this.minefield = GameObject.FindGameObjectWithTag("Minefield").GetComponent<MineField>();
        this.spriteController = this.GetComponent<SpriteController>();
        this.tile = this.GetComponent<Tile>();
    }

    private void OnMouseDown()
    {
        if (tile.isRevealed)
        {
            ShowSurroundingUnknownTiles();
            selected = true;
        }
    }

    private void OnMouseUpAsButton()
    {
        if(!tile.isSecured && !tile.isRevealed)
        {
            ClickTile();
        }

        //If over the tile selected and amount secured is equal to amount of mines -- reveal all unsecuredMines
        if(tile.isRevealed && selected)
        {
            if (GetAmountNeighbourSecured() == GetAmountNeighbourMines())
            {
                RevealSurroundingUnsecuredTiles();
            }
        }
    }

    private void OnMouseUp()
    {
        if(selected)
        {
            ResetSurroundingUnknownTiles();
            selected = false;
        }
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1) && !tile.isRevealed)
        {
            if(tile.isSecured)
            {
                spriteController.SetDefaultTileSprite();
                tile.isSecured = false;
            }
            else
            {
                spriteController.SetSecuredTileSprite();
                tile.isSecured = true;
            }
        }
    }

    void ClickTile()
    {
        if(!minefield.hasGameStarted)
        {
            minefield.hasGameStarted = true;

            CreateMines();
        }


        if(tile.isMine)
        {
            minefield.LoseGame();

            //Set deadlyMine after all mine sprites have been set
            spriteController.SetDeadlyMineSprite();
        }
        else
        {
            RevealTile();
        }
    }

    void CreateMines()
    {
        Debug.Log("Create Mines");

        int minesLeft = minefield.amountOfMines;
        int tilesLeft = minefield.amountUnrevealed;

        for(int x = 0; x < minefield.xTotal; x++)
        {
            for(int y = 0; y < minefield.yTotal; y++)
            {
                if(!(x == tile.x && y == tile.y))
                {
                    Tile aTile = minefield.tiles[x, y];

                    float chanceForMine = (float)minesLeft / (float)tilesLeft;

                    if(Random.value <= chanceForMine)
                    {
                        aTile.isMine = true;
                        minesLeft--;
                    }
                }

                tilesLeft--;
            }
        }
    }


    public void RevealTile()
    {
        if(!tile.isRevealed && !tile.isMine)
        {
            tile.isRevealed = true;
            minefield.amountUnrevealed--;

            int amountOfNeighbouringMines = GetAmountNeighbourMines();

            spriteController.SetEmptyTile(amountOfNeighbouringMines);

            //Empty Tile
            if(amountOfNeighbouringMines == 0)
            {
                //Reveal all 8 surounding tiles
                //Left Coloum
                RevealIfValid(tile.x - 1, tile.y - 1);
                RevealIfValid(tile.x - 1, tile.y);
                RevealIfValid(tile.x - 1, tile.y + 1);

                //Mid
                RevealIfValid(tile.x , tile.y - 1);
                RevealIfValid(tile.x, tile.y + 1);

                //Right
                RevealIfValid(tile.x + 1, tile.y - 1);
                RevealIfValid(tile.x + 1, tile.y);
                RevealIfValid(tile.x + 1, tile.y + 1);
            }
        }

        if(tile.isMine)
        {
            minefield.LoseGame();

            //Set deadlyMine after all mine sprites have been set
            spriteController.SetDeadlyMineSprite();
        }
        else
        {
            if(minefield.IsGameWon())
            {
                minefield.WinGame();
            }
        }
    }

    void RevealIfValid(int x, int y)
    {
        if (x >= 0 && x < minefield.xTotal && y >= 0 && y < minefield.yTotal)
        {
            minefield.tiles[x, y].clickMechanics.RevealTile();
        }
    }

    public int GetAmountNeighbourMines()
    {
        int mineCounter = 0;

        if (HasMine(tile.x - 1, tile.y - 1)) mineCounter++;
        if (HasMine(tile.x - 1, tile.y)) mineCounter++;
        if (HasMine(tile.x - 1, tile.y + 1)) mineCounter++;

        if (HasMine(tile.x, tile.y - 1)) mineCounter++;
        if (HasMine(tile.x, tile.y + 1)) mineCounter++;

        if (HasMine(tile.x + 1, tile.y - 1)) mineCounter++;
        if (HasMine(tile.x + 1, tile.y)) mineCounter++;
        if (HasMine(tile.x + 1, tile.y + 1)) mineCounter++;

        return mineCounter;


    }

    public int GetAmountNeighbourSecured()
    {
        int securedCounter = 0;

        if (IsSecured(tile.x - 1, tile.y - 1)) securedCounter++;
        if (IsSecured(tile.x - 1, tile.y)) securedCounter++;
        if (IsSecured(tile.x - 1, tile.y + 1)) securedCounter++;

        if (IsSecured(tile.x, tile.y - 1)) securedCounter++;
        if (IsSecured(tile.x, tile.y + 1)) securedCounter++;

        if (IsSecured(tile.x + 1, tile.y - 1)) securedCounter++;
        if (IsSecured(tile.x + 1, tile.y)) securedCounter++;
        if (IsSecured(tile.x + 1, tile.y + 1)) securedCounter++;

        return securedCounter;
    }

    //Darkens Surrounding non revealed or secured Tiles
    void ShowSurroundingUnknownTiles()
    {
        ShowIfUnknown(tile.x - 1, tile.y - 1);
        ShowIfUnknown(tile.x - 1, tile.y);
        ShowIfUnknown(tile.x - 1, tile.y + 1);

        ShowIfUnknown(tile.x, tile.y - 1);
        ShowIfUnknown(tile.x, tile.y + 1);

        ShowIfUnknown(tile.x + 1, tile.y - 1);
        ShowIfUnknown(tile.x + 1, tile.y);
        ShowIfUnknown(tile.x + 1, tile.y + 1);
    }

    void RevealSurroundingUnsecuredTiles()
    {
        if (!IsSecured(tile.x - 1, tile.y - 1)) RevealIfValid(tile.x - 1, tile.y - 1);
        if (!IsSecured(tile.x - 1, tile.y))     RevealIfValid(tile.x - 1, tile.y);
        if (!IsSecured(tile.x - 1, tile.y + 1)) RevealIfValid(tile.x - 1, tile.y + 1);

        if (!IsSecured(tile.x, tile.y - 1))     RevealIfValid(tile.x, tile.y - 1);
        if (!IsSecured(tile.x, tile.y + 1))     RevealIfValid(tile.x, tile.y + 1);

        if (!IsSecured(tile.x + 1, tile.y - 1)) RevealIfValid(tile.x + 1, tile.y - 1);
        if (!IsSecured(tile.x + 1, tile.y))     RevealIfValid(tile.x + 1, tile.y);
        if (!IsSecured(tile.x + 1, tile.y + 1)) RevealIfValid(tile.x + 1, tile.y + 1);
    }

    void ResetSurroundingUnknownTiles()
    {
        ResetIfUnknown(tile.x - 1, tile.y - 1);
        ResetIfUnknown(tile.x - 1, tile.y);
        ResetIfUnknown(tile.x - 1, tile.y + 1);

        ResetIfUnknown(tile.x, tile.y - 1);
        ResetIfUnknown(tile.x, tile.y + 1);

        ResetIfUnknown(tile.x + 1, tile.y - 1);
        ResetIfUnknown(tile.x + 1, tile.y);
        ResetIfUnknown(tile.x + 1, tile.y + 1);
    }

    bool HasMine(int x, int y)
    {
        bool hasMine = false;

        if(x >= 0 && x < minefield.xTotal && 
            y >= 0 && y < minefield.yTotal)
            { 
                hasMine = minefield.tiles[x, y].isMine;
            }
        return hasMine;
    }

    bool IsSecured(int x, int y)
    {
        bool isSecured = false;

        if (x >= 0 && x < minefield.xTotal &&
            y >= 0 && y < minefield.yTotal)
        {
            isSecured = minefield.tiles[x, y].isSecured;
        }
        return isSecured;
    }

    void ShowIfUnknown(int x, int y)
    {
        if (x >= 0 && x < minefield.xTotal &&
            y >= 0 && y < minefield.yTotal)
        {
            if(!minefield.tiles[x,y].isRevealed && !minefield.tiles[x,y].isSecured)
            {
                //This is really ugly
                minefield.tiles[x,y].clickMechanics.spriteController.SetDarkTileSprite();
            }
        }
    }
    void ResetIfUnknown(int x, int y)
    {
        if (x >= 0 && x < minefield.xTotal &&
              y >= 0 && y < minefield.yTotal)
        {
            if (!minefield.tiles[x, y].isRevealed && !minefield.tiles[x, y].isSecured)
            {
                //This is really ugly
                minefield.tiles[x, y].clickMechanics.spriteController.SetDefaultTileSprite();
            }
        }
    }

    public void RevealIfMine()
    {
        if (tile.isMine)
        {
            if (!tile.isSecured)
            {
                spriteController.SetMineSprite();
            }
        }
    }
}
