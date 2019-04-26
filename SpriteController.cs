using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite darkTileSprite;
    public Sprite mineSprite;
    public Sprite securedTileSprite;
    public Sprite deadlyMineSprite;
    public Sprite securedMineSprite;
    public Sprite[] emptyTileSprites; // 0 - 8

    //Set Sprite based on surrounding mines
    public void SetEmptyTile(int amountOfMines)
    {
        GetComponent<SpriteRenderer>().sprite = emptyTileSprites[amountOfMines];

    }

    public void SetMineSprite()
    {
        GetComponent<SpriteRenderer>().sprite = this.mineSprite;
    }

    public void SetSecuredTileSprite()
    {
        GetComponent<SpriteRenderer>().sprite = this.securedTileSprite;
    }

    public void SetDeadlyMineSprite()
    {
        GetComponent<SpriteRenderer>().sprite = this.deadlyMineSprite;
    }

    public void SetSecuredMineSprite()
    {
        GetComponent<SpriteRenderer>().sprite = this.securedMineSprite;
    }

    public void SetDefaultTileSprite()
    {
        GetComponent<SpriteRenderer>().sprite = this.defaultSprite;
    }

    public void SetDarkTileSprite()
    {
        GetComponent<SpriteRenderer>().sprite = this.darkTileSprite;
    }
}
