using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trunfo
{
    public class Jogador : MonoBehaviour
    {
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


        // Start is called before the first frame update
        void Start()
        {
            CriterioDisplay.criterioEscolhido += LiberaCompra;
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
                CartaNaMao.GetComponent<RotacaoCartas>().Reseta();
                CompraLiberada = false;
            }
        }

        private void LiberaCompra(int index)
        {
            CompraLiberada = true;

            Debug.Log("Vai malandra");
        }
    }
}
