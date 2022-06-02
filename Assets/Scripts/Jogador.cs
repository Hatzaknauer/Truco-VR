using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Jogador : MonoBehaviour
{
    public int pontos;
    public Carta[] cartas;
    int rodada;
    public GameObject[] slots;
    public Baralho baralho;
    public TMP_Text txtPontos,
        txtDebug;

    void Start()
    {
        pontos = 0; 
        txtPontos.text = pontos.ToString();
        baralho = FindObjectOfType<Baralho>();
    }

    public void Reset()
    {
        rodada = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Ao clicar pega a posição do mouse no clique e passa como destino para o agente
       
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Carta carta = hit.collider.GetComponentInParent<Carta>();
            if(carta != null)
            {
                JogaCarta(carta);
            }
        }    
    } 
   
    public void JogaCarta(Carta carta)
    {
        
        Carta vindo = baralho.GetCarta();
        txtDebug.text = ("Vindo " + vindo.numero + "  " + vindo.naipe);
        baralho.Descarta(carta);
        baralho.Descarta(vindo);
        for (int i = 0; i < 3; i++)
        {
            if (cartas[i] == carta)
            {
                cartas[i] = null;
            }
        }
        if (carta.manilha && !vindo.manilha)
        {
            txtDebug.text = ("Ganhou com manilha de " + carta.naipe);
            GanhouRodada();
        }
        else if (!carta.manilha && vindo.manilha)
        {
            txtDebug.text = ("Perdeu para manilha de " + vindo.naipe);
            baralho.BotGanhou();
        }
        else if (carta.manilha && vindo.manilha)
        {
            if (((int)carta.naipe) > ((int)vindo.naipe))
            {
                txtDebug.text = ("Ganhou com manilha de " + carta.naipe);
                GanhouRodada();
            }
            else
            {
                txtDebug.text = ("Perdeu para manilha de " + vindo.naipe);
            }
        }
        else if (carta.valor == vindo.valor)
        {
            txtDebug.text = (carta.numero + "  " + carta.naipe);
            txtDebug.text = ("Melou");
            GanhouRodada();
            baralho.BotGanhou();
        }
        else
        {
            if (carta.valor > vindo.valor)
            {
                txtDebug.text = (carta.numero + "  " + carta.naipe);
                txtDebug.text = ("Ganhou");
                GanhouRodada();
            }
            else
            {
                txtDebug.text = (carta.numero + "  " + carta.naipe);
                txtDebug.text = ("Perdeu");
                baralho.BotGanhou();
            }
        }
    }
    
    public void GanhouRodada()
    {
        rodada++;
        if (rodada == 2)
        {
            Pontuo();
        }
    }
    public void Pontuo()
    {
        baralho.Descarta(baralho.tombo);
        baralho.tombo = null;
        pontos += baralho.truco;
        Reset();
        txtPontos.text = pontos.ToString();
        TerminaRodada();
    }

    public void TerminaRodada()
    {
        for (int i = 0; i < 3; i++)
        {
            if (cartas[i] != null)
            {
                baralho.Descarta(cartas[i]);
                cartas[i] = null;
            }
        }
        baralho.TerminaRodada();
    }

    public void Truco()
    {
        if(baralho.truco >= 12)
        {
            txtDebug.text = ("Tá valendo o jogo");
        }
        else
        {
            baralho.ChamaTruco(this);
        }
    }
}
