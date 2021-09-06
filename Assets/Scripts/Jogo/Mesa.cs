using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Trunfo
{
    public class Mesa : MonoBehaviour
    {
        // Jogadores
        [SerializeField] private Jogador jogador1;
        public Jogador Jogador1 {get=>jogador1;}
        [SerializeField] private Jogador jogador2;
        public Jogador Jogador2 {get=>jogador2;}

        // Baralho do Jogo
        [SerializeField] private List<Card> Baralho;

        // Gerenciador Firestore
        public GerenciadorFirestore Gerenciador;


        // Start is called before the first frame update
        void Start()
        {
            // Se for o criador da sala, embaralha o baralho, 
            // divide baralho e envia para o adversário
            if (jogador1.criador)
            {
                DividirBaralho();
                EnviaBaralho();
            }

            // Se não, recebe baralho
            else
            {
                RecebeDeck();
            }

            // Registra função no clique do critério
            CriterioDisplay.criterioEscolhido += TestaComparaCriterio;
        }

        public void DividirBaralho()
        {

            // Embaralha o baralho
            Baralho = Baralho.OrderBy(seed => UnityEngine.Random.Range(0, 32)).ToList();

            // Atribui a metade do baralho para o Jogador1
            jogador1.Baralho.InsereCartas(Baralho.Take(Baralho.Count() / 2).ToArray());

            // Atribui a outra metade do baralho para o Jogador1
            jogador2.Baralho.InsereCartas(Baralho.Skip(Baralho.Count() / 2).ToArray());
        }


        public void EnviaBaralho()
        {
            // Transformando cartas para String
            List<string> CartasIds = new List<string>();
            foreach (var carta in jogador2.Baralho.Cartas)
            {
                CartasIds.Add(carta.Identificacao.ToString());
            }

            // Converte para formato StructBaralho do Firebase
            StructBaralho baralho = new StructBaralho
            {
                Baralho = CartasIds.ToArray()
            };

            // Envia baralho para o firebase            
            Gerenciador.enviarProBanco<StructBaralho>(baralho, "salas", "Sala teste2");
        }

        public void RecebeDeck()
        {
            // Recebe baralho via firebase
            Gerenciador.pegarDoBanco<StructBaralho>("salas", "Sala teste2",
                task =>
                {
                    List<string> list = new List<string>(task.Baralho);
                    list.ForEach(i =>
                    {
                        // Converte string para carta
                        Card CartaAtual = ConverteParaCarta(i);
                        // Insere no baralho do jogador1
                        jogador1.Baralho.InsereCarta(CartaAtual);
                        // Remove do baralho da mesa
                        Baralho.Remove(CartaAtual);
                    }
                    );
                    // Insere o restante no baralho do jogador2
                    jogador2.Baralho.InsereCartas(Baralho.ToArray());
                }
            );
        }

        public Card ConverteParaCarta(string CartasIds)
        {
            // Checa nas cartas do baralho da mesa
            foreach (Card carta in Baralho)
            {
                // Se a carta for a de mesmo ida que a CartasIds
                if (carta.Identificacao.ToString() == CartasIds)
                {
                    // Retorna a dita carta
                    return carta;
                }
            }
            // Se não encontrar retorna null
            return null;
        }

        void TestaComparaCriterio(int index) => ComparaCriterio(jogador1.CartaNaMao, jogador2.CartaNaMao, index);

        bool ComparaCriterio(CardDisplay carta1, CardDisplay carta2, int index)
        {
            // Vence a que tiver maior critério
            return carta1.carta.Compara(carta2.carta, index);
        }
    
    }
}
