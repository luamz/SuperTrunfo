using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Trunfo
{
    public class Mesa : MonoBehaviour
    {
        // Jogadores
        [SerializeField] private Jogador jogador1;
        public Jogador Jogador1 { get => jogador1; }
        [SerializeField] private Jogador jogador2;
        public Jogador Jogador2 { get => jogador2; }
        
        //Sala
        public string idDaSala = "Sala de teste id gerado";

        // Baralho do Jogo
        [SerializeField] private List<Card> Baralho;
        public bool RecebeuBaralho = false;

        // Mensagem
        public TextMeshProUGUI DisplayMensagem;
        public Button BotaoPaginaInicial;

        // Gerenciador Firestore
        public GerenciadorFirestore Gerenciador;

        // Start is called before the first frame update
        void Start()
        {
            Mensagem("Seja Bem-Vindo!\nPegue uma carta");
            // Se for o criador da sala, embaralha, divide baralho e envia para o adversário
            if (jogador1.criador)
            {
                idDaSala = GameObject.Find("CriacaoDeSala").GetComponent<CriacaoDeSala>().idSala;
                DividirBaralho();
                EnviaBaralho();
            }

            // Se não, recebe baralho
            else
            {
                idDaSala = GameObject.Find("EntraNaSala").GetComponent<EntraNaSala>().idSala;
                StartCoroutine(ChecaSeMesaCarregou());
            }
            StartCoroutine(ChecaSeOOponeteJogouACarta());
            // Envia carta na mão quando o critério é escolhido (Onde a comparação é feita)
            CriterioDisplay.criterioEscolhido += EnviaCartaNaMaoResultado;
        }
        
        /// <summary>À cada 2 segundos checa se o oponente jogou a carta</summary>
        private IEnumerator ChecaSeOOponeteJogouACarta()
        {
            while (true)
            {
                yield return new WaitForSeconds(2.5f);
                if (jogador2.seuTurno)
                {
                    var idNaMaoDoAdversario = jogador2.CartaNaMao.carta.Identificacao.ToString();
                    RecebeCartaNaMaoAdversario(idNaMaoDoAdversario);
                }
            }
        }

        private IEnumerator ChecaSeMesaCarregou()
        {
           
            while (!RecebeuBaralho)
            {
                yield return new WaitForSeconds(2.5f);
                RecebeBaralho();
            }
        }
        private void DividirBaralho()
        {

            // Embaralha o baralho
            Baralho = Baralho.OrderBy(seed => UnityEngine.Random.Range(0, 32)).ToList();

            // Atribui a metade do baralho para o Jogador1
            jogador1.Baralho.InsereCartas(Baralho.Take(Baralho.Count() / 2).ToArray());

            // Atribui a outra metade do baralho para o Jogador1
            jogador2.Baralho.InsereCartas(Baralho.Skip(Baralho.Count() / 2).ToArray());
        }

        private void EnviaBaralho()
        {
            IniciaAPartidaComCartaVazia();
            // Transformando cartas para String
            List<string> CartasIdsCriador = new List<string>();
            List<string> CartasIdsAdversario = new List<string>();

            foreach (var carta in jogador1.Baralho.Cartas)
            {
                CartasIdsCriador.Add(carta.Identificacao.ToString());
            }

            foreach (var carta in jogador2.Baralho.Cartas)
            {
                CartasIdsAdversario.Add(carta.Identificacao.ToString());
            }

            // Converte para formato StructBaralho do Firebase
            StructBaralho baralho = new StructBaralho
            {
                BaralhoCriador = CartasIdsCriador.ToArray(),
                BaralhoAdversario = CartasIdsAdversario.ToArray()
            };
            
            // Envia baralho para o firebase       
            Gerenciador.colocaCamadaProfunda<StructBaralho>(baralho
            , "salas"
            , idDaSala
            , "InfoDaPartida"
            , "Baralho");
            jogador2.CompraCarta();
            jogador1.CompraCarta();
            

            void IniciaAPartidaComCartaVazia()
            {
                StructCarta cartaVazia = new StructCarta
                {
                    Id = "",
                    Criterio = 0,
                    JogadorDoTurnoGanha = false
                };

                // Envia baralho para o firebase      
                Gerenciador.colocaCamadaProfunda<StructCarta>(cartaVazia
                    , "salas"
                    , idDaSala
                    , "InfoDaPartida"
                    , "Carta");
            }
        }

        private void RecebeBaralho()
        {
            // Recebe baralho via firebase
            Gerenciador.pegaProfundo<StructBaralho>("salas"
            , idDaSala
            , "InfoDaPartida"
            , "Baralho"
            , baralho =>
               {
                   // Lista do Jogador 1( Não criou a partida)
                   List<string> listJogador1 = new List<string>(baralho.BaralhoAdversario);

                   // Lista do Jogador 2 ( Criador da Partida )
                   List<string> listJogador2 = new List<string>(baralho.BaralhoCriador);

                   // Jogador1
                   listJogador1.ForEach(i =>
                  {
                      // Converte string para carta
                      Card CartaAtual = ConverteParaCarta(i);
                      // Insere no baralho do jogador1
                      jogador1.Baralho.InsereCarta(CartaAtual);
                  }
                  );

                   // Jogador2 (Criador)
                   listJogador2.ForEach(i =>
                  {
                      // Converte string para carta
                      Card CartaAtual = ConverteParaCarta(i);
                      // Insere no baralho do jogador1
                      jogador2.Baralho.InsereCarta(CartaAtual);
                  }
                  );
                   jogador2.CompraCarta();
                   jogador1.CompraCarta();
               }
            );
            RecebeuBaralho = true;
        }

        private Card ConverteParaCarta(string CartasIds)
        {
            // Checa nas cartas do baralho da mesa
            foreach (Card carta in Baralho)
            {
                // Se a carta for a de mesmo ida que a CartasIds
                if (carta.Identificacao.ToString() == CartasIds)
                {
                    // Retorna a dita carta
                    return carta;
                }
            }
            // Se não encontrar retorna null
            return null;
        }

        // Jogador do turno (ou seja, aquele que compara) envia ao outro
        // o id da carta da mão dele e o resultado da comparação
        public void EnviaCartaNaMaoResultado(int index)
        {
            bool Ganha = ComparaCriterio(Jogador1.CartaNaMao, Jogador2.CartaNaMao, index);
            StructCarta carta = new StructCarta
            {
                Id = Jogador1.CartaNaMao.carta.Identificacao.ToString(),
                Criterio = index,
                JogadorDoTurnoGanha = Ganha
            };

            Gerenciador.colocaCamadaProfunda<StructCarta>(carta
                    , "salas"
                    , idDaSala
                    , "InfoDaPartida"
                    , "Carta");
            TrataGanhador(Ganha);

        }

        // Jogador que não é do turno (ou seja, aquele que não compara)
        // recebe o id da carta da mão do adversario e o resultado da comparação
        // checando se o valor mudou
        private void RecebeCartaNaMaoAdversario(string idNaMaoDoAdversario)
        {
            Gerenciador.pegaProfundo<StructCarta>("salas"
                , idDaSala
                , "InfoDaPartida"
                , "Carta"
                , carta =>
                 {
                     string idNoBanco = carta.Id;
                     if (idNoBanco.Equals(idNaMaoDoAdversario))
                     {
                         Card cartaNaMaoAdversario = ConverteParaCarta(idNoBanco);
                         TrataGanhador(!carta.JogadorDoTurnoGanha);
                         Jogador2.Animacao.ViraCartaOponente();
                     }
                 }
            );
        }

        private void TrataGanhador(bool JogadorLocal)
        {
            // Jogador do Turno ganha, logo eu perdi
            if (JogadorLocal)
            {
                // Se ganhou, o proximo turno é seu
                jogador1.seuTurno = true;
                jogador2.seuTurno = false;

                // Insere cartas no perdedor
                InsereCartasNoGanhador(Jogador1, Jogador2);
                // Mensagem Display
                if (Jogador2.Baralho.Cartas.Length > 0)
                    Mensagem("Sua carta ganhou!");
                else
                {
                    Mensagem("Você ganhou o jogo!");
                    BotaoPaginaInicial.gameObject.SetActive(true);
                }
            }
            else
            {
                // Se perdeu, o proximo turno é do oponente
                Jogador1.seuTurno = false;
                Jogador2.seuTurno = true;

                // Insere cartas no ganhador
                InsereCartasNoGanhador(Jogador2, Jogador1);

                // Mensagem display
                if (Jogador1.Baralho.Cartas.Length > 0)
                    Mensagem("Sua carta perdeu:(");
                else if (Jogador1.Baralho.Cartas.Length == 0)
                {
                    Mensagem("Você perdeu o jogo :(");
                    BotaoPaginaInicial.gameObject.SetActive(true);
                }
            }
        }
        private void InsereCartasNoGanhador(Jogador Ganhador, Jogador Perdedor)
        {
            Ganhador.Baralho.InsereCarta(Ganhador.CartaNaMao.carta);
            Ganhador.Baralho.InsereCarta(Perdedor.CartaNaMao.carta);
            Jogador2.Animacao.OnTerminaMovimento += Ganhador.RetornaCarta;
            Jogador2.Animacao.OnTerminaMovimento += Perdedor.DaParaAdversario;
        }

        public void Mensagem(string texto)
        {
            DisplayMensagem.text = texto;
        }

        private bool ComparaCriterio(CardDisplay carta1, CardDisplay carta2, int index)
        {
            Debug.LogFormat
            (
                "Critério jogador local: {0}, Critério jogador remoto: {1}, no index: {2}",
                jogador1.CartaNaMao.carta.Pontos[index],
                jogador2.CartaNaMao.carta.Pontos[index],
                index
            );
            
            // Vence a que tiver maior critério
            return carta1.carta.Compara(carta2.carta, index);
        }

        public void VoltaPaginaInicial()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
