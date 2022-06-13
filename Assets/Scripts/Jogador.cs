using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Jogador : MonoBehaviour
{
    public int pontos,
        rodada;
    public Carta[] cartas;
    public GameObject[] slots;
    public Baralho baralho;
    public TMP_Text txtPontos,
        txtDebug;
    public TurnManager turnManager;

    void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
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
        baralho.CartaNaMesa(carta, false);
        baralho.jogandoJogador = carta;

        for (int i = 0; i < 3; i++)
        {
            if (cartas[i] == carta)
            {
                cartas[i] = null;
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
        else
        {
            turnManager.gameMode = EnumTurns.playerTurn;
            turnManager.Reset();
        }
    }
    public void Pontuo()
    {
        baralho.Descarta(baralho.tombo);
        baralho.Descarta(baralho.jogandoJogador);
        baralho.Descarta(baralho.jogandoBot);
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
        else if (turnManager.playerTrucou)
        {
            txtDebug.text = ("Já truco");
        }
        else
        {
            turnManager.playerTrucou = true;
            turnManager.botTrucou = false;
            baralho.ChamaTruco(this);
        }
    }
}
