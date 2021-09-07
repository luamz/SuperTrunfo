using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;

namespace Trunfo
{
    public class GerenciadorFirestore : MonoBehaviour
    {
        FirebaseFirestore db;
        void Start()
        {
            
        }
        void instanciaBanco(){
            db = FirebaseFirestore.DefaultInstance;
        }

        // public void mandaString(){
        //     StructCarta teste = new StructCarta{
        //         Id = "B5",
        //         Baralho = new string[] {"Primeiro","Segundo","Terceiro"}
        //     };
            
        //     enviarProBanco<StructCarta>(teste, "salas", "Sala teste");
        // }

       

        // public void pegaString(){
            
        //     pegarDoBanco("Valores", "valorzinho", (Task<T> task)=>{
        //         Debug.Log(task.ConvertTo<Teste>());
        //         });
        // }
        
        public void pegaValor(){
            // Exemplo pegar um do tipo Teste
            pegarDoBanco<Teste>("Valores", "Que isso irmao", task=>{Debug.Log(task.Valor);});
        }
        
        public void pegarDoBanco<T>(string Collection, string Document, Action<T> usarDados) {
            instanciaBanco();
            
            db.Collection(Collection).Document(Document).GetSnapshotAsync()
            .ContinueWithOnMainThread(task=>{
                T retorno = task.Result.ConvertTo<T>();
                usarDados.Invoke(retorno);
            });
        }

        public void enviarProBanco<T>(T dados,string Collection, string Document) where T : struct{
            instanciaBanco();
            
            DocumentReference valorRef = db.Collection(Collection).Document(Document);
            valorRef.SetAsync(dados).ContinueWithOnMainThread(task => {
                Debug.Log("Foi sim mano");
            });
        }

        // public void pegarDoBanco<T>(string Collection, string Document, Action<Task<T>> usarDados) {
        //     instanciaBanco();
        //     db.Collection(Collection).Document(Document).GetSnapshotAsync()
        //     .ContinueWithOnMainThread(task=>{
        //         usarDados?.Invoke(task);
        //     });

        // }

        public void deletaTeste(){
            deletaCampo("Valor", "Valores", "valorzinho");
        }
        public async void deletaCampo(string Campo, string Collection, string Document){
            instanciaBanco();
            DocumentReference cityRef = db.Collection(Collection).Document(Document);
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { Campo, FieldValue.Delete }
            };
            await cityRef.UpdateAsync(updates);
        }

        
        void Update()
        {
        
        }
    }
}
