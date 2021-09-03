using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trunfo
{
    public class Interface : MonoBehaviour
    {
        public static int QtdeJogador = 16;
        public TextMeshProUGUI QtdeCartasJogador;
        public static int QtdeAdversario = 16;
        public TextMeshProUGUI QtdeCartasAdversario;
        public TextMeshProUGUI Mensagem;
        public int cont;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            QtdeCartasJogador.text = QtdeJogador.ToString();
            QtdeCartasAdversario.text = QtdeAdversario.ToString();
        }


        public void JogadorGanhaCarta()
        {
            if(QtdeJogador!=0)
            {
                if (QtdeJogador < 32)
                {
                    QtdeAdversario--;
                    QtdeJogador++;
                }
                Mensagem.gameObject.SetActive(true);
                Mensagem.text = QtdeJogador < 32 ? "Sua carta venceu!\nPegue uma nova carta!" : "Você venceu!";
            }
            
        }

        public void JogadorPerdeCarta()
        {
            if(QtdeJogador!=32)
            {
                if (QtdeJogador > 0)
                {
                    QtdeJogador--;
                    QtdeAdversario++;
                }
                Mensagem.gameObject.SetActive(true);
                Mensagem.text = QtdeJogador > 0 ? "Sua carta perdeu :(\nPegue uma nova carta!" : "Você perdeu :(";
            }
        }

    }
}
