using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trunfo
{
    public class Interface : MonoBehaviour
    {
        public Mesa Mesa;
        
        // Placar do Jogador 
        public TextMeshProUGUI QtdeCartasJogador;

        // Placar do Adversário
        public TextMeshProUGUI QtdeCartasAdversario;

        // Update is called once per frame
        void Update()
        {
            QtdeCartasJogador.text = Mesa.Jogador1.Baralho.Cartas.Length.ToString();
            QtdeCartasAdversario.text = Mesa.Jogador2.Baralho.Cartas.Length.ToString();
        }
    }
}
