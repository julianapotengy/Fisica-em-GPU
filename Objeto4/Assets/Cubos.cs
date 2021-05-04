using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubos : MonoBehaviour
{
    public ComputeShader computeShader;

    public float valor;
    public float[] arrayFloats;

    public struct Cubo
    {
        public Color cor;
        public Vector3 positionXYZ;
    };

    GameObject[] gameObjects;
    public int count = 10;
    Cubo[] dado;
    public GameObject model;
    public GameObject chao;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    void OnGUI()
    {
        if (dado == null)
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "Teste CPU"))
            {
                CriarCuboCPU();
            }
        }
    }

    public void CriarCuboCPU()
    {
        dado = new Cubo[count];
        gameObjects = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            float offsetX = (-count / 2 + i);

            gameObjects[i] = GameObject.Instantiate(model, new Vector3(offsetX * 1.2f, 0, 0), Quaternion.identity);

            dado[i].positionXYZ.y = gameObjects[i].transform.position.y;
        }
    }
}
