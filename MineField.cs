using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineField : MonoBehaviour
{
    public int amountOfMines;
    public int amountUnrevealed;
    public bool hasGameStarted = false;

    public GameObject winObject;

    public int yTotal;
    public int xTotal;

    public Tile[,] tiles;

    public void CreateMineField(int xTotal, int yTotal, int amountOfMines)
    {
        winObject.SetActive(false);

        this.xTotal = xTotal;
        this.yTotal = yTotal;
        this.amountOfMines = amountOfMines;
        this.amountUnrevealed = xTotal * yTotal;
        this.hasGameStarted = false;

        if(this.tiles != null)
        {
            foreach(Tile t in tiles)
            {
                Destroy(t.gameObject);
            }
        }

        this.tiles = new Tile[xTotal, yTotal];

        for(int x = 0; x < xTotal; x++)
            {
            for(int y = 0; y < yTotal; y++)
            {
                tiles[x, y] = Tile.CreateNewTile(x, y);
            }
        }

    }

    public bool IsGameWon()
    {
        if(amountUnrevealed == amountOfMines)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LoseGame()
    {
        foreach(Tile t in tiles)
        {
            t.clickMechanics.RevealIfMine();
            t.isRevealed = true; //Sets all tiles to unclickable
        }
    }

    public void WinGame()
    {
        Debug.Log("Win Game");

        foreach (Tile t in tiles)
        {
            t.isRevealed = true; //Sets all tiles to unclickable
        }

        winObject.SetActive(true);
    }

    public void SetHeight(int height)
    {
        yTotal = height;
    }

    public void SetWidth(int width)
    {
        xTotal = width;
    }

    public void SetAmountOfMines(int amount)
    {
        amountOfMines = amount;
    }
}
