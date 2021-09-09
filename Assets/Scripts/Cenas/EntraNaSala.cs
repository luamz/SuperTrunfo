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
        [SerializeField] private TMP_InputField codigoDaSala;
        public string idSala = "";
        void Start()
        {
            DontDestroyOnLoad(gameObject.transform.root);
            //authManager = GameObject.Find("AuthManeger").GetComponent<GameBase>();
            Gerenciador = GetComponent<GerenciadorFirestore>();
        }
        public void EntraButton()
        {
            Entra(codigoDaSala.text);
        }
        /// <summary>Use essa função para entrar na sala</summary>
        public void Entra(string codigo)
        {
            Gerenciador.pegarDoBanco<structSala>("salas", codigo,
            sala =>
            {
                if (sala.Adversario == "")
                {
                    sala.Adversario = "1";
                    Gerenciador.enviarProBanco(sala, "salas", codigo);
                    //StartCoroutine(ChecaSeCriadorEntrouNaMesa());
                    SceneManager.LoadScene("Mesa");
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
