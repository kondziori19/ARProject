using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFigure : MonoBehaviour
{
    private Color startcolor;
    public GameObject[] tiles;
    GameObject tileRef;
    Mesh mymesh;

    // Start is called before the first frame update
    void Start()
    {
        tileRef = GameObject.Find("0,0");
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        mymesh = tileRef.GetComponent<MeshFilter>().mesh;
        tileRef.GetComponent<MeshFilter>().mesh = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                OnTileHit(hit.collider.gameObject);     
            }
        }

    }

    public void OnTileHit(GameObject obj)
    {
      
        Debug.Log("Tile: " + obj.name);
        // Debug.Log("ChildCount: " + obj.transform.childCount);
        if(obj.transform.childCount > 0)
        {
            //Debug.Log("Child0: " + obj.transform.GetChild(0).name);
            //Debug.Log("Figure Type: " + obj.transform.GetChild(0).GetComponent<Figure>().GetFigureType());
            //Debug.Log("Figure Color: " + obj.transform.GetChild(0).GetComponent<Figure>().GetColor());
            Debug.Log("curr_x: " + obj.GetComponent<TileIndices>().GetX() + " curr_y: " + obj.GetComponent<TileIndices>().GetY());
            ClearTilesHighlight();
            ShowPossibleMoves(obj);

        }

    }

    public void ShowPossibleMoves(GameObject tile)
    {
        string type = tile.transform.GetChild(0).GetComponent<Figure>().GetFigureType();
        string color = tile.transform.GetChild(0).GetComponent<Figure>().GetColor();

        if(type == "pawn")
        {
            foreach(GameObject t in PawnMoves(tile))
            {
                Debug.Log("x: " + t.GetComponent<TileIndices>().GetX() + " y: " + t.GetComponent<TileIndices>().GetY());
                t.GetComponent<MeshFilter>().mesh = mymesh;
            }    
        }
    }

    public void ClearTilesHighlight()
    {
        foreach(GameObject tile in tiles)
        {
            tile.GetComponent<MeshFilter>().mesh = null;
        }
    }    

    public List<GameObject> PawnMoves(GameObject current_tile)
    {
        string type = current_tile.transform.GetChild(0).GetComponent<Figure>().GetFigureType();
        string color = current_tile.transform.GetChild(0).GetComponent<Figure>().GetColor();

        List<GameObject> PossibleMoveTiles = new List<GameObject>();

        int x = current_tile.GetComponent<TileIndices>().GetX();
        int y = current_tile.GetComponent<TileIndices>().GetY();

        if (color == "white")
        {
            foreach(GameObject tile in tiles)
            {
                if(x == 1) // if pawn stands at starting position
                {
                    if(tile.GetComponent<TileIndices>().GetX() == x + 2 && tile.GetComponent<TileIndices>().GetY() == y)
                    {
                        if (tile.transform.childCount == 0) // if there is no other figure at this tile
                        {
                            PossibleMoveTiles.Add(tile);
                        }
                    }    
                }

                if (tile.GetComponent<TileIndices>().GetX() == x + 1 && tile.GetComponent<TileIndices>().GetY() == y) // move white pawn 1 tile ahead
                {
                    if(tile.transform.childCount == 0) // if there is no other figure at this tile
                    {
                        PossibleMoveTiles.Add(tile);
                    }
                }

                if (tile.GetComponent<TileIndices>().GetX() == x + 1 && (tile.GetComponent<TileIndices>().GetY() == y - 1 || tile.GetComponent<TileIndices>().GetY() == y + 1))
                {
                    if (tile.transform.childCount == 1) // if there is figure at this tile
                    {
                        if(tile.transform.GetChild(0).GetComponent<Figure>().GetColor() == "black")
                        {
                            PossibleMoveTiles.Add(tile);
                        }
                    }
                }
            }
        }
        return PossibleMoveTiles;
    }




}
