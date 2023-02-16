using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestTileds001 : MonoBehaviour
{
    public Grid scenaryGrid;
    public Tilemap[] tilemaps;
    public RuleTile[] tiles;

    [Range(1, 5)]
    public int widthLand = 4; // largura
    [Range(1, 50)]
    public int lengthLand = 5; // comprimento
    [Range(1, 5)]
    private int amountLand = 4; // quantidade

    Vector3Int positionCell;

    void Start()
    {
        CreatedScenary(positionCell);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    positionCell = scenaryGrid.LocalToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //    CreatedScenary(positionCell);
        //}

        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    tilemaps[0].SetTile(positionCell, tiles[1]); // Seta o tile na posicao da celula
        //}

        //if (Input.GetKey(KeyCode.Mouse1))
        //{
        //    tilemaps[0].SetTile(positionCell, tiles[0]); // Seta o tile na posicao da celula
        //}
    }

    public void CreatedScenary(Vector3Int clickPositionCell)
    {
        positionCell = new Vector3Int((int)Random.Range(-16, 16), (int)Random.Range(-16, 16), 0);

        int currentLengthLand = lengthLand / widthLand;

        int currentCellX = clickPositionCell.x;
        int currentCellY = clickPositionCell.y;


        for (int x =0; x < lengthLand; x++) // if numbers the "for" < widthLand = generate
        {
            for (int y = 0; y < lengthLand; y++)
            {
                float randomDirenctionX = Random.Range(-1.9f, 1.9f);
                float randomDirenctionY = Random.Range(-1.9f, 1.9f);

                positionCell = new Vector3Int(positionCell.x + (int)randomDirenctionX, positionCell.y + (int)randomDirenctionY, 0);

                for (int i = 0; i < widthLand; i++)
                {
                    tilemaps[0].SetTile(positionCell, tiles[1]);
                    tilemaps[0].SetTile(new Vector3Int(positionCell.x+1, positionCell.y, 0), tiles[1]);
                    tilemaps[0].SetTile(new Vector3Int(positionCell.x+1, positionCell.y+1, 0), tiles[1]);
                    tilemaps[0].SetTile(new Vector3Int(positionCell.x, positionCell.y+1, 0), tiles[1]);
                } 
            }
        }

        if (amountLand > 0)
        {
            amountLand--;
            CreatedScenary(positionCell);
        } 
    }
}
