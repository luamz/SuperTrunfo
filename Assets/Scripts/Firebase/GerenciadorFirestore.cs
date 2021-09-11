using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;

// O GerenciadorFirestore é responsavel pelo CRUD do firestore

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

        public void pegaValor() 
        {
            // Exemplo pegar um do tipo Teste
            pegarDoBanco<Teste>("Valores", "Que isso irmao", task => { Debug.Log(task.Valor); });
        }

        public void pegarDoBanco<T>(string Collection, string Document, Action<T> usarDados)
        {   // Essa função recebe uma nome de coleção, um nome de documento e uma função para utilizar os dados recuperados
            instanciaBanco();

            db.Collection(Collection).Document(Document).GetSnapshotAsync()
            .ContinueWithOnMainThread(task =>
            {
                T retorno = task.Result.ConvertTo<T>();
                usarDados.Invoke(retorno);
            });
        }


        public void testeColocaFundo()
        {   // função exemplo para uso da função colocaCamadaProfunda
            StructCarta teste = new StructCarta
            {
                Id = "Baaaaaaaaa",
                Criterio = 1,
                JogadorDoTurnoGanha = true
            };
            colocaCamadaProfunda<StructCarta>(teste, "testesubcolecao", "Sala um", "Colec1", "Doc1");
        }
        public void colocaCamadaProfunda<T>(T dados, string ExtCollection, string ExtDocument, string DenCollection, string DenDocument)
        {// Essa função recebe uma nome de coleção, um nome de documento, sub coleção e sub documento e uma dados para serem inseridos
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
        {   // função exemplo para uso da função pegaProfundo 
            // Exemplo pegar um do tipo Carta em subcoleções
            pegaProfundo<StructCarta>("testesubcolecao", "Sala um", "Colec1", "Doc1", task => { Debug.Log(task.Id); });
        }

        public void pegaProfundo<T>(string ExtCollection, string ExtDocument, string DenCollection, string DenDocument, Action<T> usarDados)
        {// Essa função recebe uma nome de coleção, um nome de documento, sub coleção e sub documento e uma função para utilizar os dados recuperados
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
        {  // função exemplo para uso da função criaDocumentIdAleatorio
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
            //Essa função recebe um nome de coleção e cria um document com nome aleatorio com os dados inseridos retornando o id do document
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
            //Essa função envia dados para o banco na coleção e document especificados
            instanciaBanco();

            DocumentReference valorRef = db.Collection(Collection).Document(Document);
            valorRef.SetAsync(dados).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Enviado para o Firebase com Sucesso!");
            });
        }

        public void deletaTeste()
        {   // função exemplo para uso da função  deletaCampo
            deletaCampo("Valor", "Valores", "valorzinho");
        }
        public async void deletaCampo(string Campo, string Collection, string Document)
        {   // deleta um campo de um document de uma collection
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
