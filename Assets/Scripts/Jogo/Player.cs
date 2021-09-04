using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trunfo
{
    public class Player : MonoBehaviour
    {
        //N�mero do jogador
        public int numeroJogador;

        //Vari�vel para a vez do jogador
        public bool seuTurno;

        //mao(cartas) do jogador
        [SerializeField] private Baralho mao;
        public Baralho Mao { get => mao;}
        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        bool compCriterio(Card carta1, Card carta2, int index)
        {
            return carta1.Pontos[index] > carta2.Pontos[index];
            
        }
    }
}
