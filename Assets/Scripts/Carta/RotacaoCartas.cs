using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trunfo
{
    public class RotacaoCartas : MonoBehaviour
    {
        private RectTransform carta;
        [SerializeField] private RectTransform transformFinal;
        public float vel = 0.1f;
        private transformInfo posicaoInicial;
        private transformInfo posicaoInicialAdversario;
        private transformInfo posicaoFinal;
        private float count = 0;
        private CardDisplay card_display;
        private bool frente = false;

        private readonly struct transformInfo
        {
            public readonly Vector3 position;
            public readonly Quaternion rotation;
            public transformInfo(RectTransform transform)
            {
                position = transform.position;
                rotation = transform.rotation;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            card_display = GetComponent<CardDisplay>();
            card_display.SetaVerso();

            carta = GetComponent<RectTransform>();

            posicaoInicial = new transformInfo(carta);
            posicaoFinal = new transformInfo(transformFinal.GetComponent<RectTransform>());

        }

        // Update is called once per frame
        void Update()
        {
            vai_carta();
        }

        public void PermiteCriterio()
        {


        }

        public void Reseta()
        {
            count = 0;
            card_display.SetaVerso();
            frente = false;
        }

        private void vai_carta()
        {
            if (count < 1)
            {
                carta.position = Vector3.Lerp(posicaoInicial.position, posicaoFinal.position, count);
                carta.rotation = Quaternion.Lerp(posicaoInicial.rotation, posicaoFinal.rotation, count);
                count += vel * Time.deltaTime;
            }
            if (count > 0.5f && !frente)
            {
                frente = true;
                card_display.SetaFrente();
            }
        }

        private void VaiProAdversario()
        {
            if (count < 1)
            {
                carta.position = Vector3.Lerp(posicaoInicial.position, posicaoFinal.position, count);
                carta.rotation = Quaternion.Lerp(posicaoInicial.rotation, posicaoFinal.rotation, count);
                count += vel * Time.deltaTime;
            }
            if (count > 0.5f && !frente)
            {
                frente = true;
                card_display.SetaFrente();
            }
        }

        private void VoltaProBaralho()
        {
            if (count < 1)
            {
                carta.position = Vector3.Lerp(posicaoFinal.position, posicaoInicial.position, count);
                carta.rotation = Quaternion.Lerp(posicaoFinal.rotation, posicaoInicial.rotation, count);
                count += vel * Time.deltaTime;
            }
            if (count > 0.5f && frente)
            {
                frente = false;
                card_display.SetaVerso();
            }
        }
    }
}
