using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Material squareMaterial;
    [SerializeField] private float yAdjust = 0F;
    [SerializeField] private Vector3 boardCenter;
    //private float boardheight = GameObject.Find("Interaction").GetComponent<TapToPlaceObject>().placementPose.position.y;

    private GameObject[,] squares;
    private Vector3 adjust;
    private void Awake()
    {
        GenerateSquares(0.060F);
    }

    private void GenerateSquares(float squareSize)  // Trzeba przesun¹æ i obróciæ :)
    {
        adjust = new Vector3(-4 * squareSize, 0, -4 * squareSize) + boardCenter;  
    
        squares = new GameObject[8,8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                squares[i, j] = CreateSquare(squareSize,i,j);
            }
        }
    }

    private GameObject CreateSquare(float squareSize, int i, int j)
    {
        GameObject square = new GameObject(string.Format("{0},{1}", i, j));
        square.transform.parent = transform;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(i * squareSize, yAdjust, j * squareSize) + adjust;
        vertices[1] = new Vector3(i * squareSize, yAdjust, (j + 1) * squareSize) + adjust;
        vertices[2] = new Vector3((i + 1) * squareSize, yAdjust, j * squareSize) + adjust;
        vertices[3] = new Vector3((i + 1) * squareSize, yAdjust, (j + 1) * squareSize) + adjust;

        int[] triangles = new int[] { 0, 1, 2, 1, 3, 2 };

        Mesh mesh = new Mesh();
        square.AddComponent<MeshFilter>().mesh = mesh;
        square.AddComponent<MeshRenderer>().material = squareMaterial;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        square.AddComponent<BoxCollider>();

        return square;
    }
}
