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

        public TextMeshProUGUI Mensagem;

        // Update is called once per frame
        void Update()
        {
            QtdeCartasJogador.text = Mesa.Jogador1.Baralho.Cartas.Length.ToString();
            QtdeCartasAdversario.text = Mesa.Jogador2.Baralho.Cartas.Length.ToString();
        }


        // public void JogadorGanhaCarta()
        // {
        //         Mensagem.gameObject.SetActive(true);
        //         Mensagem.text = QtdeJogador < 32 ? "Sua carta venceu!\nPegue uma nova carta!" : "Você venceu!";            
        // }

        // public void JogadorPerdeCarta()
        // {
        //     if(QtdeJogador!=32)
        //     {
        //         if (QtdeJogador > 0)
        //         {
        //             QtdeJogador--;
        //             QtdeAdversario++;
        //         }
        //         Mensagem.gameObject.SetActive(true);
        //         Mensagem.text = QtdeJogador > 0 ? "Sua carta perdeu :(\nPegue uma nova carta!" : "Você perdeu :(";
        //     }
        // }

    }
}
