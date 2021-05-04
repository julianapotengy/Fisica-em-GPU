using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubos : MonoBehaviour
{
    public ComputeShader computeShader;
    ComputeBuffer computebuffer;

    int kernel;
    public float valor;
    public float[] arrayFloats;

    public struct Cubo
    {
        public Color cor;
        public Vector3 positionXYZ;
        public float vCubo;
        public float deltaTime;
        public float velocidadeI;
        public float velocidadeF;
        public float posI;
        public float posA;
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

    public bool iniciarGPU = false;

    public int tamanhoAlocado;

    public float time1;
    public float time2;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (cair)
        {
            PodeCair();
        }
        if (iniciarGPU)
        {
            rodarGPU();
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
            if (GUI.Button(new Rect(10, 70, 100, 50), "Teste GPU"))
            {
                time1 = Time.time;
                time2 = Time.time;
                CriarCuboGPU();
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

    public void CriarCuboGPU()
    {
        dado = new Cubo[count];
        gameObjects = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            float offsetX = (-count / 2 + i);

            gameObjects[i] = GameObject.Instantiate(model, new Vector3(offsetX * 1.2f, 0, 0), Quaternion.identity);

            dado[i].positionXYZ.y = gameObjects[i].transform.position.y;
        }

        tamanhoAlocado = sizeof(float) * 3 + sizeof(float) * 6 + sizeof(float) * 4;
        iniciarGPU = true;
    }

    public void rodarGPU()
    {
        if (gameObjects[0].transform.position.y > chao.transform.position.y + 1)
        {
            time2 = Time.time;

            for (int i = 0; i < count; i++)
            {
                dado[i].deltaTime = time2 - time1;
            }

            //Debug.Log(tamanhoAlocado);
            computebuffer = new ComputeBuffer(count, tamanhoAlocado);
            computebuffer.SetData(dado);
            computeShader.SetBuffer(kernel, "cubos", computebuffer);
            computeShader.Dispatch(kernel, Mathf.CeilToInt(dado.Length / 16), 1, 1);
            computebuffer.GetData(dado);


            for (int i = 0; i < count; i++)
            {

                //gameObjects[i].transform.position -= new Vector3(this.transform.position.x, dado[i].positionXYZ.y, this.transform.position.z);
                gameObjects[i].transform.position -= new Vector3(0, dado[i].posI, 0);
                //Debug.Log(dado[i].positionXYZ.y);
                /*dado[i].velocidadeI = dado[i].velocidadeF;
                dado[i].posA = dado[i].positionXYZ.y;*/
            }

            time1 = time2;
            computebuffer.Dispose();
        }
        else if (!coloriu)
        {
            computebuffer = new ComputeBuffer(count, tamanhoAlocado);
            computebuffer.SetData(dado);
            computeShader.SetBuffer(kernel, "cubos", computebuffer);
            computeShader.Dispatch(kernel, Mathf.CeilToInt(dado.Length / 16), 1, 1);
            computebuffer.GetData(dado);

            for (int i = 0; i < count; i++)
            {
                gameObjects[i].GetComponent<MeshRenderer>().material.SetColor("_Color", dado[i].cor);
            }
            coloriu = true;
            computebuffer.Dispose();
        }
    }
}
