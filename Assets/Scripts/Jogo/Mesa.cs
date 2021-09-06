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
                RecebeBaralho();
            }

            // Envia carta na mão quando o critério é escolhido
            // Comparação feita dentro da dita função
            CriterioDisplay.criterioEscolhido += EnviaCartaNaMaoResultado;
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
            List<string> CartasIdsCriador = new List<string>();
            List<string> CartasIdsAdversario = new List<string>();
            
            foreach (var carta in jogador1.Baralho.Cartas)
            {
                CartasIdsCriador.Add(carta.Identificacao.ToString());
            }

            foreach (var carta in jogador2.Baralho.Cartas)
            {
                CartasIdsAdversario.Add(carta.Identificacao.ToString());
            }

            // Converte para formato StructBaralho do Firebase
            StructBaralho baralho = new StructBaralho
            {
                BaralhoCriador = CartasIdsCriador.ToArray(),
                BaralhoAdversario = CartasIdsAdversario.ToArray()
            };

            // Envia baralho para o firebase            
            Gerenciador.enviarProBanco<StructBaralho>(baralho, "salas", "Sala teste2");
        }

        public void RecebeBaralho()
        {
            // Recebe baralho via firebase
            Gerenciador.pegarDoBanco<StructBaralho>("salas", "Sala teste2",
                task =>
                {
                    // Lista do Jogador 1( Não criou a partida)
                    List<string> listJogador1 = new List<string>(task.BaralhoAdversario);

                    // Lista do Jogador 2 ( Criador da Partida )
                    List<string> listJogador2 = new List<string>(task.BaralhoCriador);
                    
                    // Jogador1
                    listJogador1.ForEach(i =>
                    {
                        // Converte string para carta
                        Card CartaAtual = ConverteParaCarta(i);
                        // Insere no baralho do jogador1
                        jogador1.Baralho.InsereCarta(CartaAtual);
                    }
                    );

                    // Jogador2 (Criador)
                    listJogador2.ForEach(i =>
                    {
                        // Converte string para carta
                        Card CartaAtual = ConverteParaCarta(i);
                        // Insere no baralho do jogador1
                        jogador2.Baralho.InsereCarta(CartaAtual);
                    }
                    );
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


        // Jogador do turno (ou seja, aquele que compara) envia ao outro
        // o id da carta da mão dele e o resultado da comparação
        public void EnviaCartaNaMaoResultado(int index){
            bool Ganha = ComparaCriterio(Jogador1.CartaNaMao,Jogador2.CartaNaMao,index);
            TrataGanhador(Ganha);
            
            StructCarta carta = new StructCarta
            {
                Id = Jogador1.CartaNaMao.carta.Identificacao.ToString(),
                JogadorDoTurnoGanha = Ganha
            };

            // Envia baralho para o firebase            
            Gerenciador.enviarProBanco<StructCarta>(carta, "salas", "Sala teste2");
            
        }

        // Jogador que não é do turno (ou seja, aquele que não compara)
        // recebe o id da carta da mão do adversario e o resultado da comparação
        public void RecebeCartaNaMaoAdversario(){
            Gerenciador.pegarDoBanco<StructCarta>("salas", "Sala teste2",
                task =>
                {
                    Card cartaNaMaoAdversario = ConverteParaCarta(task.Id);
                    TrataGanhador(task.JogadorDoTurnoGanha);
                }
            );
        }

        private void TrataGanhador(bool JogadorDoTurnoGanha){
            // Jogador do Turno ganha, logo eu perdi
            if (JogadorDoTurnoGanha)
                InsereCartasNoGanhador(Jogador2,Jogador1);
            // Jogador do turno perde
            else
                InsereCartasNoGanhador(Jogador1,Jogador2);
        }

        private void InsereCartasNoGanhador(Jogador Ganhador, Jogador Perdedor){
            Ganhador.Baralho.InsereCarta(Ganhador.CartaNaMao.carta);
            Ganhador.Baralho.InsereCarta(Perdedor.CartaNaMao.carta);
        }

        bool ComparaCriterio(CardDisplay carta1, CardDisplay carta2, int index)
        {
            // Vence a que tiver maior critério
            return carta1.carta.Compara(carta2.carta, index);
        }
    
    }
}
