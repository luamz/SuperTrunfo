using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Modulo responsavel pela criacao de sala
namespace Trunfo
{
    [RequireComponent(typeof(GerenciadorFirestore))]
    public class CriacaoDeSala : MonoBehaviour
    {
        //private GameBase authManager;
        private GerenciadorFirestore Gerenciador;
        public string idSala = "";
        void Start()
        {
            DontDestroyOnLoad(gameObject.transform.root);
            //authManager = GameObject.Find("AuthManeger").GetComponent<GameBase>();
            
        }
        public void CriaSala()
        {
            Gerenciador = GetComponent<GerenciadorFirestore>();
            Gerenciador.criaDocumentIdAleatorio<structSala>(new structSala
            {
                Criador = "1",
                MesaCriada = false,
                Adversario = ""
            }, "salas",
            idSala =>
            {
                this.idSala = idSala;
                //Manda notificação aqui com id da sala
                StartCoroutine(ChecaSeOOutroJogadorJaEntrou());
                EnviaNotificacao.Envia(idSala);
            });
        }
        private IEnumerator ChecaSeOOutroJogadorJaEntrou()
        {
            bool jaEntrou = false;
            //Enquanto ninguém mais entrou, checa a cada segundo
            while (!jaEntrou)
            {
                yield return new WaitForSeconds(1f);
                Gerenciador.pegarDoBanco<structSala>("salas", idSala,
                    sala =>
                    {
                        if (sala.Adversario != "")
                        {
                            jaEntrou = true;
                            sala.MesaCriada = true;
                            Gerenciador.enviarProBanco<structSala>(sala, "salas", idSala);
                        }
                    }
                );
            }
            SceneManager.LoadScene("Mesa");
        }
    }
}
