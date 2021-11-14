using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveData : MonoBehaviour
{
    public bool canMove;
    GameObject moveOrigin;

    // Start is called before the first frame update
    void Start()
    {
        canMove = false; //determines if this tile is one of legal moves by selected piece
    }
    
    public void SetMoveOrigin(GameObject obj)
    {
        moveOrigin = obj;
    }
    public GameObject GetMoveOrigin()
    {
        if(canMove)
        {
            return moveOrigin;
        }
        else
        {
            return null;
        }
    }


}
