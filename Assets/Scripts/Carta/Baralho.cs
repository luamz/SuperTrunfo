using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Trunfo
{
    public class Baralho : MonoBehaviour
    {
        private RectTransform baralho;
        [SerializeField] private Vector3 posicaoQuandoInteiro;
        [SerializeField] private Vector3 posicaoQuandoTemUmSobrando;
        [SerializeField] private int quantTotalDeCartas;
        [Range(0f, 1f)]
        [SerializeField] private float porcAtual = 0f;
        private Queue<CardDisplay> cartas;


        private void OnValidate()
        {
            baralho = GetComponent<RectTransform>();
            //para testar o lerp
            baralho.localPosition = Vector3.Lerp(posicaoQuandoTemUmSobrando,
                                                        posicaoQuandoInteiro,
                                                        porcAtual);
        }

        public CardDisplay CompraCarta()
        {
            MoveMascara(-1);
            return cartas.Dequeue();
        }
        private void MoveMascara(int delta)
        {
            if (cartas.Count + delta == 0) GetComponent<Image>().enabled = false;
            var completude = (cartas.Count + delta - 1) / quantTotalDeCartas;
            baralho.localPosition = Vector3.Lerp(posicaoQuandoTemUmSobrando,
                                                        posicaoQuandoInteiro,
                                                        completude);
        }
        public void InsereCarta(CardDisplay carta)
        {
            MoveMascara(1);
            cartas.Enqueue(carta);
        }
    }
}
