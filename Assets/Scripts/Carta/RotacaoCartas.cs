using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trunfo
{
    public class RotacaoCartas : MonoBehaviour
    {
        RectTransform carta;
        [SerializeField] private RectTransform transformFinal;
        public float vel = 0.1f;
        private transformInfo posicaoInicial;
        private transformInfo posicaoFinal;
        float count = 0;
        CardDisplay card_display;
        bool frente = false;
        int incremento;

        private struct transformInfo
        {
            public readonly Vector3 position;
            public readonly Quaternion rotation;
            public transformInfo(RectTransform transform)
            {
                this.position = transform.position;
                this.rotation = transform.rotation;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            card_display = GetComponent<CardDisplay>();
            incremento = card_display.jogador ? -78 : +78;
            card_display.SetaVerso();

            carta = GetComponent<RectTransform>();

            posicaoInicial = new transformInfo(carta);
            posicaoFinal = new transformInfo(transformFinal.GetComponent<RectTransform>());
            // pos_inicial = carta.position;
            // pos_final = new Vector3(carta.localPosition.x + incremento, carta.localPosition.y, carta.localPosition.z);
            // pos_final = transformFinal.position;

        }

        // Update is called once per frame
        void Update()
        {
            vai_carta();
        }

        public void vai_carta()
        {
            if (count < 1)
            {
                carta.position = Vector3.Lerp(posicaoInicial.position, posicaoFinal.position, count);
                carta.rotation = Quaternion.Lerp(posicaoInicial.rotation, posicaoFinal.rotation, count);
                count += vel;
            }
            if (count > 0.5f && !frente)
            {
                frente = true;
                card_display.SetaFrente();
            }
        }
    }
}
