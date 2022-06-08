using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumTurns
{
    init = 0,
    playerTurn = 1,
    botTurn = 2,
    trucoBot = 3,
    trucoPlayer = 4,
    embate = 5
};

public class TurnManager : MonoBehaviour
{
    public float timeEmbate;
    Baralho baralho;
    Jogador jogador;
    public EnumTurns gameMode;
    public int rodada,
        turno,
        truco;
    bool jogou;
    public bool aguardando,
        botJogou,
        vendo;
    bool wasPlayer;


    void Start()
    {
        wasPlayer = false;
        rodada = 0;
        gameMode = EnumTurns.init; 
        baralho = FindObjectOfType<Baralho>();
        jogador = FindObjectOfType<Jogador>();
        aguardando = false;
        jogador.turnManager = this;
    }

    public void Reset()
    {
        botJogou = false;
        aguardando = false;
        vendo = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameMode)
        {
            case EnumTurns.init:
                baralho.DarCartas();
                if (wasPlayer)
                {
                    wasPlayer = false;
                    gameMode = EnumTurns.botTurn;
                }
                else
                {
                    wasPlayer = true;
                    gameMode = EnumTurns.playerTurn;
                }
                break;
            case EnumTurns.playerTurn:
                if (aguardando)
                {
                    aguardando = false;
                    baralho.JogarCarta(true);
                    gameMode = EnumTurns.embate;
                }
                break;
            case EnumTurns.botTurn:
                if (!botJogou)
                {
                    botJogou = true;
                    baralho.JogarCarta(true);
                }
                if (aguardando)
                {
                    aguardando = false;
                    gameMode = EnumTurns.embate;
                }
                break;
            case EnumTurns.trucoBot:
  
                break;
            case EnumTurns.trucoPlayer:

                break;
            case EnumTurns.embate:
                if (!vendo)
                {
                    vendo = true;
                    Embate();
                }
                break;


        }
    }

    public void Embate()
    {
        if (baralho.jogandoJogador.manilha && !baralho.jogandoBot.manilha)
        {
            Invoke("PlayerGanha", timeEmbate);
        }
        else if (!baralho.jogandoJogador.manilha && baralho.jogandoBot.manilha)
        {
            Invoke("BotGanha", timeEmbate);
        }
        else if (baralho.jogandoJogador.manilha && baralho.jogandoBot.manilha)
        {
            if (((int)baralho.jogandoJogador.naipe) > ((int)baralho.jogandoBot.naipe))
            {
                Invoke("PlayerGanha", timeEmbate);
            }
            else
            {
                Invoke("BotGanha", timeEmbate);
            }
        }
        else if (baralho.jogandoJogador.valor == baralho.jogandoBot.valor)
        {
            Invoke("PlayerGanha", timeEmbate);
            Invoke("BotGanha", timeEmbate);
        }
        else
        {
            if (baralho.jogandoJogador.valor > baralho.jogandoBot.valor)
            {
                Invoke("PlayerGanha", timeEmbate);
            }
            else
            {
                Invoke("BotGanha", timeEmbate);
            }
        }
    }

    public void PlayerGanha()
    {
        baralho.Descarta(baralho.jogandoBot);
        baralho.Descarta(baralho.jogandoJogador);
        jogador.GanhouRodada();
        print("a");

    }
    public void BotGanha()
    {
        baralho.Descarta(baralho.jogandoBot);
        baralho.Descarta(baralho.jogandoJogador);
        baralho.BotGanhou();
        print("b");

    }
}
