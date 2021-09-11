using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

// Estrutura de dados das cartas
namespace Trunfo
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Card", fileName = "New Card")]
    public class Card : ScriptableObject
    {
        public TipoDeTrunfo tipo;
        [SerializeField] private id identificacao;
        [SerializeField] private string nome;
        [SerializeReference] private float[] pontos;
        [SerializeField] private Sprite artwork;

        [System.Serializable]
        public struct id
        {
            public int numero;
            public char grupo;
            public override string ToString()
            {
                return grupo.ToString() + numero.ToString();
            }
        }
        public string Nome { get => nome; }
        public id Identificacao { get => identificacao; }
        public Sprite Artwork { get => artwork; }
        ///<Summary> retorna uma cópia dos valores </Summary>
        public float[] Pontos { get => (float[])pontos.Clone(); }

        void OnValidate()
        {
            try
            {
                if (pontos.Length != tipo.Atributos.Length)
                    Debug.LogWarning("Não se deve ter um número diferente de atributos e pontos.\n"
                    + "Warning para a carta: "
                    + name);
            }
            catch (NullReferenceException) { }
        }
        internal void Initialize()
        {
            pontos = new float[tipo.Atributos.Length];
        }
        public virtual bool Compara(Card carta, int index)
        {
            // Vence a que tiver maior critério
            return Pontos[index] > carta.Pontos[index];
        }

    }
}