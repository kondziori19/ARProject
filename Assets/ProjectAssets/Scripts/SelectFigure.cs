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
 
        if(obj.GetComponent<MoveData>().canMove)
        {
            MovePiece(obj);
        }
        else if (obj.transform.childCount > 0) // if there is piece on this tile
        {
            //Debug.Log("Child0: " + obj.transform.GetChild(0).name);
            //Debug.Log("Figure Type: " + obj.transform.GetChild(0).GetComponent<Figure>().GetFigureType());
            //Debug.Log("Figure Color: " + obj.transform.GetChild(0).GetComponent<Figure>().GetColor());
            Debug.Log("curr_x: " + obj.GetComponent<TileIndices>().GetX() + " curr_y: " + obj.GetComponent<TileIndices>().GetY());
            ClearTilesHighlight();
            ShowPossibleMoves(obj);

        }



    }

    public void MovePiece(GameObject tile)
    {
        GameObject origin = tile.GetComponent<MoveData>().GetMoveOrigin();
        GameObject piece = origin.transform.GetChild(0).gameObject;

        if(tile.transform.childCount > 0)
        {
            Destroy(tile.transform.GetChild(0).gameObject);
        }

        piece.transform.parent = tile.transform;
        piece.transform.localPosition = new Vector3(0, 0, 0);
        ClearTilesHighlight(); 
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
                HighlightTile(t, tile);
            }    
        }

        if(type == "knight")
        {
            foreach (GameObject t in KnightMoves(tile))
            {
                HighlightTile(t, tile);
            }
        }

        if (type == "king")
        {
            foreach (GameObject t in KingMoves(tile))
            {
                HighlightTile(t, tile);
            }
        }

        if (type == "rook")
        {
            foreach (GameObject t in RookMoves(tile))
            {
                HighlightTile(t, tile);
            }
        }
    }


    public void HighlightTile(GameObject t, GameObject origin)
    {
        t.GetComponent<MeshFilter>().mesh = mymesh;
        t.GetComponent<MoveData>().canMove = true;
        t.GetComponent<MoveData>().SetMoveOrigin(origin);
    }
    public void ClearTilesHighlight() // clears mesh of all tiles
    {
        foreach(GameObject tile in tiles)
        {
            tile.GetComponent<MeshFilter>().mesh = null;
            tile.GetComponent<MoveData>().canMove = false;
            tile.GetComponent<MoveData>().SetMoveOrigin(null);
        }
    }

    public List<GameObject> RookMoves(GameObject current_tile)
    {
        string color = current_tile.transform.GetChild(0).GetComponent<Figure>().GetColor();
        List<GameObject> possibleMoveTiles = new List<GameObject>();

        int x = current_tile.GetComponent<TileIndices>().GetX();
        int y = current_tile.GetComponent<TileIndices>().GetY();

        possibleMoveTiles = AddRookMoves(x, y, possibleMoveTiles, color);

        return possibleMoveTiles;
    }
    public List<GameObject> KingMoves(GameObject current_tile)
    {
        string color = current_tile.transform.GetChild(0).GetComponent<Figure>().GetColor();
        List<GameObject> PossibleMoveTiles = new List<GameObject>();

        int x = current_tile.GetComponent<TileIndices>().GetX();
        int y = current_tile.GetComponent<TileIndices>().GetY();

        foreach (GameObject tile in tiles)
        {
            if((tile.GetComponent<TileIndices>().GetX() == x - 1 ||
               tile.GetComponent<TileIndices>().GetX() == x ||
               tile.GetComponent<TileIndices>().GetX() == x + 1) &&
               (tile.GetComponent<TileIndices>().GetY() == y - 1 ||
               tile.GetComponent<TileIndices>().GetY() == y  ||
               tile.GetComponent<TileIndices>().GetY() == y + 1))
            {
                if (tile.transform.childCount == 0)
                {
                    PossibleMoveTiles.Add(tile);
                }
                else
                {
                    if (tile.transform.GetChild(0).GetComponent<Figure>().GetColor() != color) //if there is the piece with the opposite color
                    {
                        PossibleMoveTiles.Add(tile);
                    }
                }
            }
        }

        return PossibleMoveTiles;

    }
    public List<GameObject> KnightMoves(GameObject current_tile)
    {
        string color = current_tile.transform.GetChild(0).GetComponent<Figure>().GetColor();
        List<GameObject> PossibleMoveTiles = new List<GameObject>();

        int x = current_tile.GetComponent<TileIndices>().GetX();
        int y = current_tile.GetComponent<TileIndices>().GetY();

        foreach (GameObject tile in tiles)
        {
            if ((tile.GetComponent<TileIndices>().GetX() == x - 2 && (tile.GetComponent<TileIndices>().GetY() == y + 1 || tile.GetComponent<TileIndices>().GetY() == y - 1)) ||
               (tile.GetComponent<TileIndices>().GetX() == x - 1 && (tile.GetComponent<TileIndices>().GetY() == y + 2 || tile.GetComponent<TileIndices>().GetY() == y - 2)) ||
               (tile.GetComponent<TileIndices>().GetX() == x + 1 && (tile.GetComponent<TileIndices>().GetY() == y + 2 || tile.GetComponent<TileIndices>().GetY() == y - 2)) ||
               (tile.GetComponent<TileIndices>().GetX() == x + 2 && (tile.GetComponent<TileIndices>().GetY() == y + 1 || tile.GetComponent<TileIndices>().GetY() == y - 1)))
            {
                if (tile.transform.childCount == 0)
                {
                    PossibleMoveTiles.Add(tile);
                }
                else
                {
                    if (tile.transform.GetChild(0).GetComponent<Figure>().GetColor() != color) //if there is the piece with the opposite color
                    {
                        PossibleMoveTiles.Add(tile);
                    }
                }
            }
        }
            return PossibleMoveTiles;
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
                        GameObject t = GetTile(x + 1, y);
                        if (tile.transform.childCount == 0 && t.transform.childCount == 0) // if there is no other piece at this tile and one tile backward
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
        else
        {
            foreach (GameObject tile in tiles)
            {
                if (x == 6) // if pawn stands at starting position
                {
                    if (tile.GetComponent<TileIndices>().GetX() == x - 2 && tile.GetComponent<TileIndices>().GetY() == y)
                    {
                        GameObject t = GetTile(x - 1, y);
                        if (tile.transform.childCount == 0 && t.transform.childCount == 0) // if there is no other piece at this tile and one tile backward
                        {
                            PossibleMoveTiles.Add(tile);
                        }
                    }
                }

                if (tile.GetComponent<TileIndices>().GetX() == x - 1 && tile.GetComponent<TileIndices>().GetY() == y) // move black pawn 1 tile ahead
                {
                    if (tile.transform.childCount == 0) // if there is no other figure at this tile
                    {
                        PossibleMoveTiles.Add(tile);
                    }
                }

                if (tile.GetComponent<TileIndices>().GetX() == x - 1 && (tile.GetComponent<TileIndices>().GetY() == y - 1 || tile.GetComponent<TileIndices>().GetY() == y + 1))
                {
                    if (tile.transform.childCount == 1) // if there is figure at this tile
                    {
                        if (tile.transform.GetChild(0).GetComponent<Figure>().GetColor() == "white")
                        {
                            PossibleMoveTiles.Add(tile);
                        }
                    }
                }
            }
        }
        return PossibleMoveTiles;
    }
    public List<GameObject> AddRookMoves(int x, int y, List<GameObject> possibleMoveTiles, string color)
    {
        for (int i = y + 1; i < 8; i++)
        {
            GameObject t = GetTile(x, i);
            if (t.transform.childCount == 0)
            {
                possibleMoveTiles.Add(t);
            }
            else
            {
                if (t.transform.GetChild(0).GetComponent<Figure>().GetColor() != color)
                {
                    possibleMoveTiles.Add(t);
                }
                break;
            }
        }

        for (int i = y - 1; i >= 0; i--)
        {
            GameObject t = GetTile(x, i);
            if (t.transform.childCount == 0)
            {
                possibleMoveTiles.Add(t);
            }
            else
            {
                if (t.transform.GetChild(0).GetComponent<Figure>().GetColor() != color)
                {
                    possibleMoveTiles.Add(t);
                }
                break;
            }
        }

        for (int i = x + 1; i < 8; i++)
        {
            GameObject t = GetTile(i, y);
            if (t.transform.childCount == 0)
            {
                possibleMoveTiles.Add(t);
            }
            else
            {
                if (t.transform.GetChild(0).GetComponent<Figure>().GetColor() != color)
                {
                    possibleMoveTiles.Add(t);
                }
                break;
            }
        }

        for (int i = x - 1; i >= 0; i--)
        {
            GameObject t = GetTile(i, y);
            if (t.transform.childCount == 0)
            {
                possibleMoveTiles.Add(t);
            }
            else
            {
                if (t.transform.GetChild(0).GetComponent<Figure>().GetColor() != color)
                {
                    possibleMoveTiles.Add(t);
                }
                break;
            }
        }

        return possibleMoveTiles;
    }

    public GameObject GetTile(int x, int  y)
    {
        foreach(GameObject t in tiles)
        {
            if(t.GetComponent<TileIndices>().GetX() == x && t.GetComponent<TileIndices>().GetY() == y)
            {
                return t;
            }
        }
        return null;
    }



}
