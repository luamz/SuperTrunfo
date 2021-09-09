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
        void instanciaBanco()
        {
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

        public void pegaValor()
        {
            // Exemplo pegar um do tipo Teste
            pegarDoBanco<Teste>("Valores", "Que isso irmao", task => { Debug.Log(task.Valor); });
        }

        public void pegarDoBanco<T>(string Collection, string Document, Action<T> usarDados)
        {
            instanciaBanco();

            db.Collection(Collection).Document(Document).GetSnapshotAsync()
            .ContinueWithOnMainThread(task =>
            {
                T retorno = task.Result.ConvertTo<T>();
                usarDados.Invoke(retorno);
            });
        }


        public void testeColocaFundo()
        {
            StructCarta teste = new StructCarta
            {
                Id = "Baaaaaaaaa",
                Criterio = 1,
                JogadorDoTurnoGanha = true
            };
            colocaCamadaProfunda<StructCarta>(teste, "testesubcolecao", "Sala um", "Colec1", "Doc1");
        }
        public void colocaCamadaProfunda<T>(T dados, string ExtCollection, string ExtDocument, string DenCollection, string DenDocument)
        {
            // db.Collection(ExtCollection).Document(ExtDocument).Collection(DenCollection).Document(DenDocument).CreateAsync(dados);
            instanciaBanco();
            DocumentReference topDoc = db.Collection(ExtCollection).Document(ExtDocument);
            CollectionReference subCollection = topDoc.Collection(DenCollection);
            DocumentReference subDoc = subCollection.Document(DenDocument);
            subDoc.SetAsync(dados).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Sub colecao criada");
            });
        }


        public void testePegaProfundo()
        {
            // Exemplo pegar um do tipo Carta em subcoleções
            pegaProfundo<StructCarta>("testesubcolecao", "Sala um", "Colec1", "Doc1", task => { Debug.Log(task.Id); });
        }

        public void pegaProfundo<T>(string ExtCollection, string ExtDocument, string DenCollection, string DenDocument, Action<T> usarDados)
        {
            instanciaBanco();
            DocumentReference topDoc = db.Collection(ExtCollection).Document(ExtDocument);
            CollectionReference subCollection = topDoc.Collection(DenCollection);
            DocumentReference subDoc = subCollection.Document(DenDocument);
            subDoc.GetSnapshotAsync()
            .ContinueWithOnMainThread(task =>
            {
                T retorno = task.Result.ConvertTo<T>();
                usarDados.Invoke(retorno);
            });
        }

        public void testeCriaDocIdAleatorio()
        {
            StructCarta teste = new StructCarta
            {
                Id = "B5",
                Criterio = 1,
                JogadorDoTurnoGanha = false
            };
            criaDocumentIdAleatorio<StructCarta>(teste, "Valores", retorno => { Debug.Log(retorno); });
        }
        public void criaDocumentIdAleatorio<T>(T dados, string Collection, Action<string> usarDados)
        {

            instanciaBanco();
            CollectionReference valorRef = db.Collection(Collection);
            valorRef.AddAsync(dados).ContinueWithOnMainThread(task =>
            {
                DocumentReference addedDocRef = task.Result;
                usarDados.Invoke(addedDocRef.Id);
            });
        }

        public void enviarProBanco<T>(T dados, string Collection, string Document) where T : struct
        {
            instanciaBanco();

            DocumentReference valorRef = db.Collection(Collection).Document(Document);
            valorRef.SetAsync(dados).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Enviado para o Firebase com Sucesso!");
            });
        }


        public void deletaTeste()
        {
            deletaCampo("Valor", "Valores", "valorzinho");
        }
        public async void deletaCampo(string Campo, string Collection, string Document)
        {
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
