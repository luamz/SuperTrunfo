using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Estrutura de dados para guardar os dados em fila e escalar o baralho baseado na quantidade de cartas
namespace Trunfo
{
    public class Baralho : MonoBehaviour
    {
        private RectTransform rectTransform;
        [SerializeField] private Vector3 posicaoQuandoInteiro;
        [SerializeField] private Vector3 posicaoQuandoTemUmSobrando;
        [SerializeField] private int quantTotalDeCartas;
        private Queue<Card> cartas = new Queue<Card>();
        public Card[] Cartas { get => cartas.ToArray(); }

        // private void OnValidate()
        // {
        //     rectTransform = GetComponent<RectTransform>();
        //     //para testar o lerp
        //     rectTransform.localPosition = Vector3.Lerp(posicaoQuandoTemUmSobrando, posicaoQuandoInteiro,
        //                                          porcAtual);
        // }

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public Card CompraCarta()
        {
            MoveMascara(-1);
            return cartas.Dequeue();
        }

        private void MoveMascara(int delta)
        {
            if (cartas.Count + delta == 0) GetComponent<Image>().enabled = false;
            float completude = (float)(cartas.Count + delta) / quantTotalDeCartas;
            rectTransform.localPosition = Vector3.Lerp(posicaoQuandoTemUmSobrando, posicaoQuandoInteiro,
                                                       completude);
        }

        public void InsereCarta(Card carta)
        {
            MoveMascara(1);
            cartas.Enqueue(carta);
        }

        public void InsereCartas(Card[] cartas)
        {
            foreach (Card carta in cartas)
                this.cartas.Enqueue(carta);
            MoveMascara(0);
        }
    }
}
