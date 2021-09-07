using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trunfo
{
    public class Jogador : MonoBehaviour
    {
        [SerializeField] private Mesa mesa;
        [SerializeField] public CardDisplay CartaNaMao;
        //Numero do jogador
        public int numeroJogador;

        // É o turno do jogador?
        public bool seuTurno;

        // O jogador é criador?
        public bool criador;


        // Deque do jogador que armazena as cartas deste
        [SerializeField] private Baralho baralho;
        public Baralho Baralho { get => baralho; }

        public RotacaoCartas Animacao { get; private set; }
        // Start is called before the first frame update
        void Start()
        {
            CriterioDisplay.criterioEscolhido += LiberaCompra;
            Animacao = CartaNaMao.GetComponent<RotacaoCartas>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private bool CompraLiberada = true;
        public void CompraCarta()
        {
            if (CompraLiberada)
            {
                var carta = baralho.CompraCarta();
                CartaNaMao.carta = carta;
                CartaNaMao.gameObject.SetActive(true);
                Debug.Log(CartaNaMao);
                if (numeroJogador == 1)
                {
                    Animacao.CompraCarta();
                    Animacao.OnTerminaMovimento += DecideLiberarCriterio;
                }
                if (numeroJogador == 2) Animacao.CompraCartaOponente();
                CompraLiberada = false;
            }
        }
        private void ComecaCompraDaCarta()
        {

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
            Animacao.OnTerminaMovimento += setaParaFalsoQuandoTerminaAAnimacao;
        }
        private void setaParaFalsoQuandoTerminaAAnimacao()
        {
            CartaNaMao.gameObject.SetActive(false);
            Animacao.OnTerminaMovimento -= setaParaFalsoQuandoTerminaAAnimacao;
        }
        public void DaParaAdversario()
        {
            Animacao.DaACartaParaAdversario();
            mesa.Jogador2.Animacao.OnTerminaMovimento -= DaParaAdversario;
            Animacao.OnTerminaMovimento += setaParaFalsoQuandoTerminaAAnimacao;
        }
    }
}
