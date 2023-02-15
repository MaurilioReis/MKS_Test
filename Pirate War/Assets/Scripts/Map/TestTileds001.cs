using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestTileds001 : MonoBehaviour
{
    public Grid scenaryGrid;
    public Tilemap[] tilemaps;
    public TileBase[] tiles;
    public Transform positionTest;

    Vector3Int positionCell;

    void Start()
    {
        
    }

    void Update()
    {
        positionCell = scenaryGrid.LocalToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        Debug.Log(scenaryGrid.LocalToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition))); // Pego a posicao do grid em zeros e uns de acordo com posicao de objeto. (bom para personalizar tbm kk)
    
        if (Input.GetKey(KeyCode.Mouse0))
        {
            tilemaps[0].SetTile(positionCell, tiles[1]); // Seta o tile na posicao da celula
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            tilemaps[0].SetTile(positionCell, tiles[0]); // Seta o tile na posicao da celula
        }
    }
}
