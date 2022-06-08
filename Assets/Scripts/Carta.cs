using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public enum EnumNaipes
{
    ouros = 1,
    espada = 2,
    copas = 3,
    paus = 4
};

public class Carta : MonoBehaviour
{
    public GameObject botao;
    public EnumNaipes naipe;
    public string numero;
    public int valor;
    public bool manilha;
    public Texture[] imgNaipe;
    public MeshRenderer material;
    public Animator animator;

    public TMP_Text txtNumero;

    public void Reset()
    {
        manilha = false;
        Vector3 pos = this.transform.position + new Vector3(0,
                            1f,
                            0);
        Quaternion rot = Quaternion.Euler(-90, 0, 0);
        transform.position = pos;
        transform.rotation = rot;
    }

    public void Construtor (EnumNaipes enumNaipe, int num)
    {
        naipe = enumNaipe;
        numero = num.ToString();

        valor = num;

        if (num > 7)
        {
            numero = AtribuiRealeza(num);
        }
        else if (num < 4)
        {
            valor += 10;
            if (num == 1)
            {
                numero = "A";
            }
        }
        material.material.mainTexture = imgNaipe[((int)naipe) - 1];
        txtNumero.text = numero;
    }

    public string AtribuiRealeza(int num)
    {
        string realeza = "Coringa";
        if (num == 8)
        {
            realeza = "Q";
        }
        else if (num == 9)
        {
            realeza = "J";
        }
        else if (num == 10)
        {
            realeza = "K";
        }

        return realeza;
    }

    public void DesligaAnim() 
    {
        animator.enabled = false;
    }

    public void SetaManilha()
    {
        manilha = true;
    }
}
