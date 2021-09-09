using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Trunfo
{
    [RequireComponent(typeof(GerenciadorFirestore))]
    public class CriacaoDeSala : MonoBehaviour
    {
        private GameBase authManager;
        private GerenciadorFirestore Gerenciador;
        public string idSala = "";
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            authManager = GameObject.Find("AuthManeger").GetComponent<GameBase>();
            Gerenciador = GetComponent<GerenciadorFirestore>();
        }
        void OnEnable()
        {
            Gerenciador.criaDocumentIdAleatorio<structSala>(new structSala
            {
                Criador = authManager.jogadorData.UserToken,
                MesaCriada = false,
                Adversario = ""
            }, "salas",
            idSala =>
            {
                this.idSala = idSala;
                //Manda notificação aqui com id da sala
                StartCoroutine(ChecaSeOOutroJogadorJaEntrou());
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
                        }
                    }
                );
            }
            SceneManager.LoadScene("Mesa");
        }
    }
}
