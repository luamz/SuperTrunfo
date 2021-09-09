using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Trunfo
{
    [RequireComponent(typeof(GerenciadorFirestore))]
    public class EntraNaSala : MonoBehaviour
    {
        private GameBase authManager;
        private GerenciadorFirestore Gerenciador;
        public string idSala;


        void Start()
        {
            DontDestroyOnLoad(gameObject.transform.root);
            Notifications.Redireciona += SetIdSala;
            //authManager = GameObject.Find("AuthManeger").GetComponent<GameBase>();

        }

        private void SetIdSala(string idSala)
        {
            this.idSala = idSala;
        }

        public void EntraButton()
        {
            Entra(idSala);
        }

        /// <summary>Use essa função para entrar na sala</summary>
        public void Entra(string codigo)
        {
            Debug.Log(codigo);
            Gerenciador = GetComponent<GerenciadorFirestore>();
            Debug.Log(Gerenciador);
            Gerenciador.pegarDoBanco<structSala>("salas", codigo,
            sala =>
            {
                Debug.Log("Tá chegando aqui");
                if (sala.Adversario == "")
                {
                    Debug.Log("E aqui");
                    sala.Adversario = "1";
                    Gerenciador.enviarProBanco(sala, "salas", codigo);
                    StartCoroutine(ChecaSeCriadorEntrouNaMesa());
                    //SceneManager.LoadScene("Mesa");
                }
            });
        }
        private IEnumerator ChecaSeCriadorEntrouNaMesa()
        {
            bool jaEntrou = false;
            //Enquanto ninguém mais entrou, checa a cada segundo
            while (!jaEntrou)
            {
                yield return new WaitForSeconds(1f);
                Gerenciador.pegarDoBanco<structSala>("salas", idSala,
                    sala =>
                    {
                        if (sala.MesaCriada)
                            jaEntrou = true;
                    }
                );
            }
            SceneManager.LoadScene("Mesa");
        }
    }
}
