using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
Essa classe é representa o jogador, que tem como atributos a mesa, 
seu baralhos (que contém as cartas desse) e métodos relacionados
à compra de carta e suas animações
*/

namespace Trunfo
{
    public class Jogador : MonoBehaviour
    {
        [SerializeField] private Mesa mesa;
        [SerializeField] public CardDisplay CartaNaMao;
        public int numeroJogador;
        public bool seuTurno; 
        public bool criador;
        private bool CompraLiberada = true; // Controla se o jogador pode comprar
        [SerializeField] private Baralho baralho; // Baralho do jogador
        public Baralho Baralho { get => baralho; }
        public Animacao Animacao { get; private set; } // Animação das cartas do jogador


        // Start is called before the first frame update
        void Start()
        {
            CriterioDisplay.criterioEscolhido += LiberaCompra;
            Animacao = CartaNaMao.GetComponent<Animacao>();
        }
        
        public void CompraCarta()
        {
            if (CompraLiberada &&
                mesa.Jogador1.Baralho.Cartas.Length > 0 &&
                mesa.Jogador2.Baralho.Cartas.Length > 0)
            {
                var carta = baralho.CompraCarta();
                CartaNaMao.carta = carta;
                CartaNaMao.gameObject.SetActive(true);
                StartCoroutine(ComecaCompraDaCarta());
                CompraLiberada = false;
            }
        }

        private IEnumerator ComecaCompraDaCarta()
        {       
            if (numeroJogador == 1)
                {
                    do{
                    yield return null;
                    Animacao.CompraCarta();
                    
                } while (!Animacao.enabled);
                    
                    Animacao.OnTerminaMovimento += DecideLiberarCriterio;
                }
            else {
                do{
                    yield return null;
                    Animacao.CompraCartaOponente();
                } while (!Animacao.enabled);
            }
        }

        private void LiberaCompra(int index)
        {
            CompraLiberada = true;
            if (numeroJogador == 2) Animacao.ViraCartaOponente();
        }

        private void DecideLiberarCriterio()
        {
            if (seuTurno && numeroJogador == 1)
            {
                CartaNaMao.PontosDeCriterio.ForEach(i => i.LiberaCriterio());
                mesa.Mensagem("Escolha um critério");
            }
            Animacao.OnTerminaMovimento -= DecideLiberarCriterio;
        }

        public void RetornaCarta()
        {
            Animacao.RetornaCarta();
            mesa.Jogador2.Animacao.OnTerminaMovimento -= RetornaCarta;
            Animacao.OnTerminaMovimento += resetaNoFimDoTurno;
        }

        private void resetaNoFimDoTurno()
        {
            CartaNaMao.gameObject.SetActive(false);
            Animacao.OnTerminaMovimento -= resetaNoFimDoTurno;
            CompraLiberada = true;
            CompraCarta();
        }

        public void DaParaAdversario()
        {
            Animacao.DaACartaParaAdversario();
            mesa.Jogador2.Animacao.OnTerminaMovimento -= DaParaAdversario;
            Animacao.OnTerminaMovimento += resetaNoFimDoTurno;
        }
    }
}
