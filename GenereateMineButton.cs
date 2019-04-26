using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenereateMineButton : MonoBehaviour
{
    public InputField heightField;
    public InputField widthField;
    public InputField minesField;

    MineField minefield;

    private void Start()
    {
        minefield = GameObject.FindGameObjectWithTag("Minefield").GetComponent<MineField>();
    }

    public void GenerateMine()
    {
        minefield.CreateMineField(int.Parse(widthField.text),int.Parse(heightField.text), int.Parse(minesField.text));
    }

}
