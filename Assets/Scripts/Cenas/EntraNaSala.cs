using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Modulo responsavel por gerenciar a entrada do usuario na sala
namespace Trunfo
{
    [RequireComponent(typeof(GerenciadorFirestore))]
    public class EntraNaSala : MonoBehaviour
    {
        private GameBase authManager;
        private GerenciadorFirestore Gerenciador;
        public string idSala="";
        public Image img;
        public TextMeshProUGUI Mensagem;


        void Awake()
        {
            DontDestroyOnLoad(gameObject.transform.root);
            Notifications.Redireciona += SetIdSala;
            //authManager = GameObject.Find("AuthManeger").GetComponent<GameBase>();

        }

        public void SetIdSala(string idSala)
        {  
            this.idSala = idSala;
        }

        public void EntraButton()
        { 
            if (idSala==""){
                Mensagem.text = "Id sala nulo";
            }
            else if (idSala != ""){
                Mensagem.text = idSala;
            }
            Entra(idSala);
        }

        /// <summary>Use essa função para entrar na sala</summary>
        public void Entra(string codigo)
        {
            Gerenciador = GetComponent<GerenciadorFirestore>();
            Gerenciador.pegarDoBanco<structSala>("salas", codigo,
            sala =>
            {
                if (sala.Adversario == "")
                {
                    sala.Adversario = "2";
                    Gerenciador.enviarProBanco<structSala>(sala, "salas", codigo);
                    StartCoroutine(ChecaSeCriadorEntrouNaMesa());
                    //SceneManager.LoadScene("Mesa");
                }
                else{
                    Mensagem.text = "Alguém já entrou\nantes de você :(";
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
                        Mensagem.text = "Entrando na sala...";
                        if (sala.MesaCriada)
                            jaEntrou = true;
                    }
                );
            }
            SceneManager.sceneLoaded += SetaConfiguracao;
            SceneManager.LoadScene("Mesa");
        }

        private static void SetaConfiguracao(Scene scene,LoadSceneMode mode){
            if (scene.name=="Mesa"){
               Jogador jogador = GameObject.Find("Jogador").GetComponent<Jogador>();
               jogador.criador = false;
               jogador.seuTurno = false;

               Jogador oponente = GameObject.Find("Oponente").GetComponent<Jogador>();
               oponente.criador=true;
               oponente.seuTurno=true;
    
            }
        }
    }
}
