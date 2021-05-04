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

    public float posY;
    public float velocidadeInicial = 0;
    public float gravidade = 9.8f;

    public float velocidadeFinal;
    public float distancia;
    public float posicao;

    public bool cair = false;
    public bool coloriu = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (cair)
        {
            PodeCair();
        }
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

        cair = true;
    }

    public void PodeCair()
    {
        velocidadeFinal = velocidadeInicial + gravidade * Time.deltaTime;
        distancia = ((velocidadeInicial + velocidadeFinal) * Time.deltaTime) / 2;
        posicao = posY - distancia;

        for (int i = 0; i < count; i++)
        {
            if (gameObjects[i].transform.position.y > chao.transform.position.y + 1)
            {
                gameObjects[i].transform.position += new Vector3(this.transform.position.x, posicao, this.transform.position.z);
            }
            else
            {
                Colorir();
            }
        }

        velocidadeInicial = velocidadeFinal;
        posY = transform.position.y;
    }

    public void Colorir()
    {
        if (!coloriu)
        {
            for (int i = 0; i < count; i++)
            {
                gameObjects[i].GetComponent<MeshRenderer>().material.SetColor("_Color", Random.ColorHSV());
            }
            coloriu = true;
        }
    }
}
