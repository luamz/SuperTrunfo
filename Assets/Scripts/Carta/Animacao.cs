using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trunfo
{
    public class Animacao : MonoBehaviour
    {
        private RectTransform carta;
        [SerializeField] private RectTransform transformFinal;
        [SerializeField] private RectTransform transformBaralhoAdversario;
        [SerializeField] private float vel;
        private transformInfo posicaoInicial;
        private transformInfo posicaoInicialAdversario;
        private transformInfo posicaoFinal;
        private CardDisplay card_display;
        private MoveStrategy moveStrategyAtual;
        private ViraNaMetadeStrategy compraCarta;
        private DefaultMoveStrategy compraOponente;
        private DesviraNaMetadeStrategy daACartaParaAdversario;
        private ViraNaMetadeStrategy revelaCartaOponente;
        private DesviraNaMetadeStrategy retornaCarta;
        private NaoMoveStrategy cartaParada;
        public Action OnTerminaMovimento;

        private struct transformInfo
        {
            public Vector3 position;
            public Quaternion rotation;
            public transformInfo(RectTransform transform)
            {
                position = transform.position;
                rotation = transform.rotation;
            }
        }

        void Start()
        {
            card_display = GetComponent<CardDisplay>();
            card_display.SetaVerso();

            carta = GetComponent<RectTransform>();

            posicaoInicial = new transformInfo(carta);
            posicaoFinal = new transformInfo(transformFinal);
            posicaoInicialAdversario = new transformInfo(transformBaralhoAdversario);

            compraCarta = new ViraNaMetadeStrategy(posicaoInicial, posicaoFinal, vel, card_display);
            compraOponente = new DefaultMoveStrategy(posicaoInicial, posicaoFinal, vel, card_display);
            retornaCarta = new DesviraNaMetadeStrategy(posicaoFinal, posicaoInicial, vel, card_display);
            daACartaParaAdversario = new DesviraNaMetadeStrategy(posicaoFinal, posicaoInicialAdversario, vel, card_display);
            revelaCartaOponente = new ViraNaMetadeStrategy(new transformInfo
            { position = transformFinal.position, rotation = carta.rotation },
                 posicaoFinal, vel, card_display);
            cartaParada = new NaoMoveStrategy(posicaoInicial, posicaoFinal, vel, card_display);

            moveStrategyAtual = cartaParada;
        }

        void Update()
        {
            moveStrategyAtual.moveCarta();
        }
        public void CompraCarta()
        {
            compraCarta.Reseta();
            moveStrategyAtual = compraCarta;
        }
        public void ParaCarta()
        {
            cartaParada.Reseta();
            moveStrategyAtual = cartaParada;
        }
        public void CompraCartaOponente()
        {
            compraOponente.Reseta();
            moveStrategyAtual = compraOponente;
        }
        public void ViraCartaOponente()
        {
            revelaCartaOponente.Reseta();
            moveStrategyAtual = revelaCartaOponente;
        }
        public void DaACartaParaAdversario()
        {
            daACartaParaAdversario.Reseta();
            moveStrategyAtual = daACartaParaAdversario;
        }
        public void RetornaCarta()
        {
            retornaCarta.Reseta();
            moveStrategyAtual = retornaCarta;
        }


        private abstract class MoveStrategy
        {
            protected readonly CardDisplay card_display;
            protected readonly RectTransform carta;
            protected readonly transformInfo posicaoInicial;
            protected readonly transformInfo posicaoFinal;
            private readonly float vel;
            protected float count = 0;
            protected Animacao rotacao;
            public MoveStrategy(transformInfo posicaoInicial,
             transformInfo posicaoFinal,
             float vel,
             CardDisplay cardDisplay)
            {
                this.posicaoInicial = posicaoInicial;
                this.posicaoFinal = posicaoFinal;
                this.vel = vel;
                this.card_display = cardDisplay;
                this.carta = card_display.GetComponent<RectTransform>();
                rotacao = card_display.GetComponent<Animacao>();
            }
            public virtual void Reseta()
            {
                count = 0;
                card_display.SetaVerso();
                jaDisparouEvento = false;
            }
            private bool jaDisparouEvento = false;
            public virtual void moveCarta()
            {
                if (count < 1)
                {
                    carta.position = Vector3.Lerp(posicaoInicial.position, posicaoFinal.position, count);
                    carta.rotation = Quaternion.Lerp(posicaoInicial.rotation, posicaoFinal.rotation, count);
                    count += vel * Time.deltaTime;
                }
                else if (!jaDisparouEvento)
                {
                    jaDisparouEvento = true;
                    carta.position = Vector3.Lerp(posicaoInicial.position, posicaoFinal.position, 1);
                    carta.rotation = Quaternion.Lerp(posicaoInicial.rotation, posicaoFinal.rotation, 1);
                    rotacao.OnTerminaMovimento?.Invoke();
                }
            }
        }
        private class DefaultMoveStrategy : MoveStrategy
        {
            public DefaultMoveStrategy(transformInfo posicaoInicial, transformInfo posicaoFinal, float vel, CardDisplay cardDisplay) : base(posicaoInicial, posicaoFinal, vel, cardDisplay)
            {
            }
        }
        private class ViraNaMetadeStrategy : MoveStrategy
        {
            public ViraNaMetadeStrategy(transformInfo posicaoInicial, transformInfo posicaoFinal,
                float vel, CardDisplay cardDisplay) : base(posicaoInicial, posicaoFinal, vel, cardDisplay)
            { }
            public override void Reseta()
            {
                base.Reseta();
                frente = false;
            }
            private bool frente = false;
            public override void moveCarta()
            {
                base.moveCarta();
                if (count > 0.5f && !frente)
                {
                    frente = true;
                    card_display.SetaFrente();
                }
            }
        }
        private class DesviraNaMetadeStrategy : MoveStrategy
        {
            public DesviraNaMetadeStrategy(transformInfo posicaoInicial, transformInfo posicaoFinal, float vel, CardDisplay cardDisplay) : base(posicaoInicial, posicaoFinal, vel, cardDisplay)
            {
            }
            public override void Reseta()
            {
                base.Reseta();
                card_display.SetaFrente();
                verso = false;
            }
            private bool verso = false;
            public override void moveCarta()
            {
                base.moveCarta();
                if (count > 0.5f && !verso)
                {
                    verso = true;
                    card_display.SetaVerso();
                }
            }

        }
        private class NaoMoveStrategy : MoveStrategy
        {
            public NaoMoveStrategy(transformInfo posicaoInicial, transformInfo posicaoFinal,
            float vel, CardDisplay cardDisplay) : base(posicaoInicial, posicaoFinal, vel, cardDisplay)
            {
            }
            public override void moveCarta() { }
        }
    }
}
