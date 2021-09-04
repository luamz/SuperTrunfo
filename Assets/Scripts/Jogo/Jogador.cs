using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trunfo
{
    public class Jogador : MonoBehaviour
    {
        //Numero do jogador
        public int numeroJogador;

        // É o turno do jogador?
        public bool seuTurno;

        // O jogador é criador?
        public bool criador;


        // Deque do jogador que armazena as cartas deste
        [SerializeField] private Baralho deque;
        public Baralho Deque { get => deque;}
        

        // Start is called before the first frame update
        void Start()
        {

        }
        
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
