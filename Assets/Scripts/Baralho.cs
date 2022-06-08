using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Baralho : MonoBehaviour
{
    public List<GameObject> cartas = new List<GameObject>();
    public List<Carta> descarte = new List<Carta>();
    int manilha;
    public Carta tombo,
        jogandoJogador,
        jogandoBot;
    public GameObject cartaPrefab, 
        gameObjectTombo,
        gameObjectBot,
        gameObjectPlayer;
    public Jogador[] jogadores;
    public int truco;
    public TurnManager turnManager;

    public TMP_Text txtPontos;

    int rodada;
    int pontos;
    
    void Start()
    {
        pontos = 0;
        txtPontos.text = pontos.ToString();
        truco = 1;
        for (int i = 0; i < 41; i++)
        {
            if(i > 0)
            {
                Vector3 pos = this.transform.position + new Vector3(0,
                    0.005f * i,
                    0);
                Quaternion rot = Quaternion.Euler(-90, 0, 0);
                GameObject gameObject = (GameObject)Instantiate(cartaPrefab, pos, rot);
                gameObject.transform.parent = this.gameObject.transform;
                Carta carta = gameObject.GetComponent<Carta>();

                if (i < 11)
                {
                    carta.Construtor(EnumNaipes.ouros, i);
                    cartas.Add(carta.gameObject);
                }
                else if (i > 10 && i < 21)
                {
                    carta.Construtor(EnumNaipes.espada, i - 10);
                    cartas.Add(carta.gameObject);
                }
                else if (i > 20 && i < 31)
                {
                    carta.Construtor(EnumNaipes.copas, i - 20);
                    cartas.Add(carta.gameObject);

                }
                else if (i > 30 && i < 41)
                {
                    carta.Construtor(EnumNaipes.paus, i - 30);
                    cartas.Add(carta.gameObject);

                }
            }
        }
    }

    public void DarCartas()
    {
        Quaternion rot = Quaternion.Euler(90, 0, 0);
        tombo = GetCarta();
        tombo.transform.position = gameObjectTombo.transform.position;
        tombo.transform.rotation = Quaternion.Euler(90, 90, 0);
        manilha = tombo.valor;
        if(manilha == 13)
        {
            manilha = 4;
        }
        else
        {
            manilha++;
        }
        cartas.Remove(tombo.gameObject);
        foreach (Jogador jogador in jogadores)
        {
            for (int i = 0; i < 3; i++)
            {
                Carta carta = GetCarta();
                jogador.cartas[i] = carta;

                if (jogador.cartas[i].valor == manilha)
                {
                    carta.SetaManilha();
                }
                carta.transform.position = jogador.slots[i].transform.position;
                carta.transform.rotation = rot;
                carta.botao.SetActive(true);
            }
        }
        turnManager.enabled = true;
    }

    public void Descarta(Carta carta)
    {
        descarte.Add(carta);
        Vector3 pos = this.transform.position + new Vector3(0,
            0,
            1f);
        carta.transform.position = pos;
        carta.botao.SetActive(false);
    }

    public Carta GetCarta()
    {
        int index = Random.Range(0, cartas.Count);
        Carta carta = cartas[index].GetComponent<Carta>();
        if (tombo != null)
        {
            int manilha = tombo.valor;
            manilha++;
            if (carta.valor == manilha)
            {
                carta.manilha = true;
            }
        }
        cartas.Remove(carta.gameObject);
        return carta;
    }

    public void BotGanhou()
    {
        rodada++;
        if(rodada == 2)
        {
            pontos+= truco;
            Descarta(tombo);
            Descarta(jogandoBot);
            Descarta(jogandoJogador);
            txtPontos.text = pontos.ToString();
            tombo = null;
            foreach (Jogador jogador in jogadores)
            {
                jogador.TerminaRodada();
                jogador.Reset();
            }
        }
        else
        {
            turnManager.gameMode = EnumTurns.botTurn;
            turnManager.Reset();
        }
    }

    public void TerminaRodada()
    {
        foreach(Carta carta in descarte)
        {
            if(carta != null)
            {
                bool tem = false;
                foreach (GameObject card in cartas)
                {
                    if (carta.gameObject == card)
                    {
                        tem = true;
                    }
                }
                if (!tem)
                {
                    cartas.Add(carta.gameObject);
                    carta.Reset();
                }
            }
        }
        descarte.Clear();
        truco = 1;
        rodada = 0;
        turnManager.Reset();
        turnManager.gameMode = EnumTurns.init;
    }

    public void ChamaTruco(Jogador jogador)
    {
        int chanceDeAceitar = Random.Range(1, 10);
        if(chanceDeAceitar > 6)
        {
            if(truco < 12 && truco > 1)
            {
                truco += 3;
            }
            else
            {
                truco = 3;
            }
            print("TRUUUUUUUUUUUCO!!!!, valendo: " + truco);
        }
        else
        {
            
            jogador.Pontuo();
            print("Oponente fugiu, ganhou: " + truco );
        }
    }

    public void JogarCarta(bool bot)
    {
        jogandoBot = GetCarta();
        CartaNaMesa(jogandoBot, bot);
    }

    public void CartaNaMesa(Carta carta, bool bot)
    {
        Quaternion rot = Quaternion.Euler(90, 0, 0);
        carta.transform.rotation = rot;

        if (bot)
        {
            carta.transform.position = gameObjectBot.transform.position;
        }
        else
        {
            carta.transform.position = gameObjectPlayer.transform.position;
            turnManager.aguardando = true;
        }
    }
}
