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

            DividirBaralho();
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void CarregarBaralho()
        {
            // A ser implementada
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
        
        public void ChecaTrunfo(){
            // A ser implementada

            /// Casos
            
            // 1 - Jogador da rodada NÃO tem o trunfo, ocorre a comparação criterio
            // Carta 1 - China (Carta do jogador da rodada)
            // Carta 2 - Brasil Trunfo
            // -> Solicita que o Jogador escolha um criterio 
            // Chama comparação de criterio

            // 2 - Jogador da rodada TEM o trunfo, ocorre a comparação de grupo
            // Carta 1 - Brasil Trunfo(Carta do jogador da rodada)
            // Carta 2 - China 
            // Chama comparação de grupo
        }

        bool ComparaGrupo(Card carta1, Card carta2)
        {
            // if (carta2.Identificacao[0] == "A"){
            //     // carta1 perde
            //     return false;
            // }'
            // else{
            //     // carta1 ganha
            //     return true;
            // }   
            return true;
        }

        bool ComparaCriterio(Card carta1, Card carta2, int index)
        {
            // Vence a que tiver maior critério
            return carta1.Pontos[index] > carta2.Pontos[index];
        }

        public void ImprimeBaralhoDividido()
        {
            Debug.Log(Jogador1.Baralho.Cartas.Length);
            Debug.Log(Jogador2.Baralho.Cartas.Length);
        }

    }
}
