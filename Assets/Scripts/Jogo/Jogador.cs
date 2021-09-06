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

        }

        // Update is called once per frame
        void Update()
        {

        }
        private bool primeiraVez = true;
        public void CompraCarta()
        {
            var carta = baralho.CompraCarta();
            CartaNaMao.carta = carta;
            CartaNaMao.gameObject.SetActive(true);
            if (primeiraVez) primeiraVez = false;
            else CartaNaMao.GetComponent<RotacaoCartas>().Reseta();
        }
    }
}
