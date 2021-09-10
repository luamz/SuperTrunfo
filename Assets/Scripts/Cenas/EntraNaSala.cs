using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        public Image img;


        void Awake()
        {
            DontDestroyOnLoad(gameObject.transform.root);
            //img.color = Color.green;
            Notifications.Redireciona += SetIdSala;
            //authManager = GameObject.Find("AuthManeger").GetComponent<GameBase>();

        }

        private void SetIdSala(string idSala)
        {
            img.color = Color.red;
            

            this.idSala = idSala;
        }

        public void EntraButton()
        {
            img.color = Color.blue;
            Entra(idSala);
        }

        /// <summary>Use essa função para entrar na sala</summary>
        public void Entra(string codigo)
        {
            Debug.Log(codigo);
            
            img.color = Color.black;
            Gerenciador = GetComponent<GerenciadorFirestore>();
            Debug.Log(Gerenciador);
            Gerenciador.pegarDoBanco<structSala>("salas", codigo,
            sala =>
            {
                Debug.Log("Tá chegando aqui");
                if (sala.Adversario == "")
                {
                    Debug.Log("E aqui");
                    img.color = Color.magenta;
                    sala.Adversario = "2";
                    Gerenciador.enviarProBanco(sala, "salas", codigo);
                    StartCoroutine(ChecaSeCriadorEntrouNaMesa());
                    //SceneManager.LoadScene("Mesa");
                }
            });
        }
        private IEnumerator ChecaSeCriadorEntrouNaMesa()
        {
            img.color = Color.black;
            bool jaEntrou = false;
            //Enquanto ninguém mais entrou, checa a cada segundo
            while (!jaEntrou)
            {
                yield return new WaitForSeconds(1f);
                Gerenciador.pegarDoBanco<structSala>("salas", idSala,
                    sala =>
                    {
                        img.color = Color.yellow;
                        if (sala.MesaCriada)
                            jaEntrou = true;
                    }
                );
            }
            SceneManager.LoadScene("Mesa");
        }
    }
}
