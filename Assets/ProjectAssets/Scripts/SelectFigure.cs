using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectFigure : MonoBehaviour
{
    private Color startcolor;
    public GameObject[] tiles;
    private int turn = 1; // 1 - white's turn, 2 - black's turn
    GameObject tileRef;
    Mesh mymesh;
    GameObject textControl;
    private bool isEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        tileRef = GameObject.Find("0,0");
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        mymesh = tileRef.GetComponent<MeshFilter>().mesh;
        tileRef.GetComponent<MeshFilter>().mesh = null;
        textControl = GameObject.Find("TextController");
        textControl.GetComponent<SetText>().textVal = "White on Move";
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
                if(!isEnded)
                {
                    OnTileHit(hit.collider.gameObject);
                }
               
            }
        }

    }

    public void OnTileHit(GameObject obj)
    {
 
        if(obj.GetComponent<MoveData>().canMove)
        {
            MovePiece(obj);
            if (isEnded) return;

            if(turn == 1)
            {
                turn = 2;
                textControl.GetComponent<SetText>().textVal = "Black on Move";
            }
            else
            {
                turn = 1;
                textControl.GetComponent<SetText>().textVal = "White on Move";
            }
        }
        else if (obj.transform.childCount > 0) // if there is piece on this tile
        {
            if(obj.transform.GetChild(0).GetComponent<Figure>().GetColor() == "white" && turn == 1)
            {
                ClearTilesHighlight();
                ShowPossibleMoves(obj);
            }else if(obj.transform.GetChild(0).GetComponent<Figure>().GetColor() == "black" && turn == 2)
            {
                ClearTilesHighlight();
                ShowPossibleMoves(obj);
            }
        }
    }

    public void MovePiece(GameObject tile)
    {
        GameObject origin = tile.GetComponent<MoveData>().GetMoveOrigin();
        GameObject piece = origin.transform.GetChild(0).gameObject;

        if(origin.transform.childCount > 0 && tile.transform.childCount > 0)
        {
            if(piece.GetComponent<Figure>().GetFigureType() == "king" && piece.GetComponent<Figure>().didMove == false && tile.transform.GetChild(0).GetComponent<Figure>().GetFigureType() == "rook")
            {
                GameObject rook = tile.transform.GetChild(0).gameObject;
                if (piece.GetComponent<Figure>().GetColor() == "white" && tile.transform.GetChild(0).GetComponent<Figure>().GetColor() == "white")
                {
                    if (tile.GetComponent<TileIndices>().GetY() == 7) // white short castle
                    {
                        GameObject tK = GetTile(0, 6);
                        GameObject tR = GetTile(0, 5);
                        piece.transform.parent = tK.transform;
                        piece.transform.localPosition = new Vector3(0, 0, 0);
                        piece.GetComponent<Figure>().didMove = true;
                        rook.transform.parent = tR.transform;
                        rook.transform.localPosition = new Vector3(0, 0, 0);
                        ClearTilesHighlight();
                        return;

                    }
                    else if (tile.GetComponent<TileIndices>().GetY() == 0) // white long castle
                    {
                        GameObject tK = GetTile(0, 2);
                        GameObject tR = GetTile(0, 3);
                        piece.transform.parent = tK.transform;
                        piece.transform.localPosition = new Vector3(0, 0, 0);
                        piece.GetComponent<Figure>().didMove = true;
                        rook.transform.parent = tR.transform;
                        rook.transform.localPosition = new Vector3(0, 0, 0);
                        ClearTilesHighlight();
                        return;
                    }
                }
                else if(piece.GetComponent<Figure>().GetColor() == "black" && tile.transform.GetChild(0).GetComponent<Figure>().GetColor() == "black")
                {
                    if (tile.GetComponent<TileIndices>().GetY() == 7) // black short castle
                    {
                        GameObject tK = GetTile(7, 6);
                        GameObject tR = GetTile(7, 5);
                        piece.transform.parent = tK.transform;
                        piece.transform.localPosition = new Vector3(0, 0, 0);
                        piece.GetComponent<Figure>().didMove = true;
                        rook.transform.parent = tR.transform;
                        rook.transform.localPosition = new Vector3(0, 0, 0);
                        ClearTilesHighlight();
                        return;

                    }
                    else if (tile.GetComponent<TileIndices>().GetY() == 0) // white long castle
                    {
                        GameObject tK = GetTile(7, 2);
                        GameObject tR = GetTile(7, 3);
                        piece.transform.parent = tK.transform;
                        piece.transform.localPosition = new Vector3(0, 0, 0);
                        piece.GetComponent<Figure>().didMove = true;
                        rook.transform.parent = tR.transform;
                        rook.transform.localPosition = new Vector3(0, 0, 0);
                        ClearTilesHighlight();
                        return;
                    }
                }
            }
        }

        if(tile.transform.childCount > 0)
        {
            if(tile.transform.GetChild(0).GetComponent<Figure>().GetFigureType() == "king")
            {
                if(tile.transform.GetChild(0).GetComponent<Figure>().GetColor() == "black")
                {
                    textControl.GetComponent<SetText>().textVal = "White Wins!!!";               
                }
                else
                {
                    textControl.GetComponent<SetText>().textVal = "Black Wins!!!";
                }
                isEnded = true;
            }
            Destroy(tile.transform.GetChild(0).gameObject);
        }

        piece.transform.parent = tile.transform;
        piece.GetComponent<Figure>().didMove = true;
        piece.transform.localPosition = new Vector3(0, 0, 0);
        
       
        ClearTilesHighlight(); 
    }

    public void ShowPossibleMoves(GameObject tile)
    {
        string type = tile.transform.GetChild(0).GetComponent<Figure>().GetFigureType();
        string color = tile.transform.GetChild(0).GetComponent<Figure>().GetColor();
        Debug.Log("DidMove: " + tile.transform.GetChild(0).GetComponent<Figure>().didMove);
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

        if (type == "bishop")
        {
            foreach (GameObject t in BishopMoves(tile))
            {
                HighlightTile(t, tile);
            }
        }
        if (type == "queen")
        {
            foreach (GameObject t in QueenMoves(tile))
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
    public List<GameObject> BishopMoves(GameObject current_tile)
    {
        string color = current_tile.transform.GetChild(0).GetComponent<Figure>().GetColor();
        List<GameObject> possibleMoveTiles = new List<GameObject>();

        int x = current_tile.GetComponent<TileIndices>().GetX();
        int y = current_tile.GetComponent<TileIndices>().GetY();

        possibleMoveTiles = AddBishopMoves(x, y, possibleMoveTiles, color);

        return possibleMoveTiles;
    }
    public List<GameObject> QueenMoves(GameObject current_tile)
    {
        string color = current_tile.transform.GetChild(0).GetComponent<Figure>().GetColor();
        List<GameObject> possibleMoveTiles = new List<GameObject>();

        int x = current_tile.GetComponent<TileIndices>().GetX();
        int y = current_tile.GetComponent<TileIndices>().GetY();

        possibleMoveTiles = AddBishopMoves(x, y, possibleMoveTiles, color);
        possibleMoveTiles = AddRookMoves(x, y, possibleMoveTiles, color);
      

        return possibleMoveTiles;
    }
    public List<GameObject> KingMoves(GameObject current_tile)
    {
        string color = current_tile.transform.GetChild(0).GetComponent<Figure>().GetColor();
        List<GameObject> PossibleMoveTiles = new List<GameObject>();

        int x = current_tile.GetComponent<TileIndices>().GetX();
        int y = current_tile.GetComponent<TileIndices>().GetY();

        if(current_tile.transform.GetChild(0).GetComponent<Figure>().didMove == false) // if king didnt move this game -> castle
        {
            if(color == "black")
            {
                GameObject t = GetTile(7, 7);
                if (t.transform.childCount > 0)
                {
                    if(t.transform.GetChild(0).GetComponent<Figure>().didMove == false && GetTile(7,6).transform.childCount == 0 && GetTile(7, 5).transform.childCount == 0) 
                    {
                        PossibleMoveTiles.Add(t);
                    }
                }
                GameObject t1 = GetTile(7, 0);
                if (t1.transform.childCount > 0)
                {
                    if (t1.transform.GetChild(0).GetComponent<Figure>().didMove == false && GetTile(7, 1).transform.childCount == 0 && GetTile(7, 2).transform.childCount == 0 && GetTile(7, 3).transform.childCount == 0)
                    {
                        PossibleMoveTiles.Add(t1);
                    }
                }
            }
            else
            {
                GameObject t = GetTile(0, 7);
                if (t.transform.childCount > 0)
                {
                    if (t.transform.GetChild(0).GetComponent<Figure>().didMove == false && GetTile(0, 6).transform.childCount == 0 && GetTile(0, 5).transform.childCount == 0)
                    {
                        PossibleMoveTiles.Add(t);
                    }
                }
                GameObject t1 = GetTile(0, 0);
                if (t1.transform.childCount > 0)
                {
                    if (t1.transform.GetChild(0).GetComponent<Figure>().didMove == false && GetTile(0, 1).transform.childCount == 0 && GetTile(0, 2).transform.childCount == 0 && GetTile(0, 3).transform.childCount == 0)
                    {
                        PossibleMoveTiles.Add(t1);
                    }
                }
            }
        }

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
    public List<GameObject> AddBishopMoves(int x, int y, List<GameObject> possibleMoveTiles, string color)
    {
        for (int i = x + 1; i < 8; i++)
        {
            GameObject t = GetTile(i, y + i - x);
            if (t != null)
            {
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
            else
            {
                break;
            }
        }

        for (int i = x + 1; i < 8; i++)
        {
            GameObject t = GetTile(i, y - i + x);
            if (t != null)
            {
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
            else
            {
                break;
            }
        }

        for (int i = x - 1; i >= 0; i--)
        {
            GameObject t = GetTile(i, y + i - x);
            if (t != null)
            {
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
            else
            {
                break;
            }
        }

        for (int i = x - 1; i >= 0; i--)
        {
            GameObject t = GetTile(i, y - i + x);
            if (t != null)
            {
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
            else
            {
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
