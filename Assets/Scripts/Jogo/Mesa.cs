using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Trunfo
{
    public class Mesa : MonoBehaviour
    {
        // Jogadores
        [SerializeField] private Jogador Jogador1;
        [SerializeField] private Jogador Jogador2;

        // Baralho do Jogo
        [SerializeField] private List<Card> Baralho;

        // Start is called before the first frame update
        void Start()
        {
            // Se o jogador criou a partida

            // if (Jogador1.criador){
            //     // O app dele deve carregar o baralho
            //     Baralho = CarregarBaralho();
            //     // O app dele deve embaralhar e dividir o baralho
            //     DividirBaralho(); 
            // }

            // // Se não criou

            // else if (!Jogador1.criador){
            //     // O app dele deve carregar o baralho
            //     Baralho = CarregarBaralho();
            //     // O app dele deve embaralhar e dividir o baralho
            //     DividirBaralho(); 

            // }
            CriterioDisplay.criterioEscolhido += TestaComparaCriterio;
            DividirBaralho();
        }
        public void DividirBaralho()
        {

            // Embaralha o baralho
            Baralho = Baralho.OrderBy(seed => Random.Range(0, 32)).ToList();

            // Atribui a metade do baralho para o Jogador1
            Jogador1.Baralho.InsereCartas(Baralho.Take(Baralho.Count() / 2).ToArray());

            // Atribui a outra metade do baralho para o Jogador1
            Jogador2.Baralho.InsereCartas(Baralho.Skip(Baralho.Count() / 2).ToArray());
        }

        public void EnviaDeck()
        {
            // A ser implementada
            // Se o jogador foi o criador da partida as cartas são embaralhadas no app dele
            // E ele deve enviar o nº das cartas do Deck do jogador2 para ele via firebase

        }

        public void RecebeDeck()
        {
            // A ser implementada
            // Se o jogador NÃO foi o criador da partida as cartas  NÃO são embaralhadas no app dele
            // E ele deve receber o nº das cartas de seu deque recebidas por ele via firebase
        }
        void TestaComparaCriterio(int index) => ComparaCriterio(Jogador1.CartaNaMao, Jogador2.CartaNaMao, index);

        bool ComparaCriterio(CardDisplay carta1, CardDisplay carta2, int index)
        {
            // Vence a que tiver maior critério
            return carta1.carta.Compara(carta2.carta, index);
        }

        public void ImprimeBaralhoDividido()
        {
            Debug.Log(Jogador1.Baralho.Cartas.Length);
            Debug.Log(Jogador2.Baralho.Cartas.Length);
        }

    }
}
