using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFigure : MonoBehaviour
{
    private Color startcolor;
    GameObject queen;
    // Start is called before the first frame update
    void Start()
    {
       queen = GameObject.Find("Chess Queen White");
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

                Debug.Log("Object HIT");
                HighlightTile(hit.collider.gameObject);
               
            }
        }

    }

    public void HighlightTile(GameObject obj)
    {
       
        Debug.Log(obj.name);
        Debug.Log(queen.transform.position);
        Debug.Log(obj.transform.position);
        queen.transform.position = obj.transform.position;
        
    }


}
