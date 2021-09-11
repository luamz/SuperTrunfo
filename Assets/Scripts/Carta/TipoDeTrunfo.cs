using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Estrutura de carta pro tipo de trunfo
namespace Trunfo
{
    [CreateAssetMenu(menuName = "ScriptableObjects/TipoDeCarta", fileName = "new TipoDeTrunfo")]
    public class TipoDeTrunfo : ScriptableObject
    {
        [SerializeField] private string nome;
        [TextArea(2, 5)]
        [SerializeField] private string[] atributos;
        // [SerializeField] private Sprite modeloDeCarta;
        public string[] Atributos { get => (string[])atributos.Clone(); }
#if UNITY_EDITOR
        //>Adiciona o comando "Instancia Carta desse Tipo" no menu do Scriptable Object
        [ContextMenu("Instancia Carta Desse Tipo")]
        private void CriaCarta()
        {
            Card carta = CreateInstance<Card>();
            carta.tipo = this;
            carta.Initialize();
            //Cria o arquivo .asset da carta no projeto com referência à essa instancia
            AssetDatabase.CreateAsset(carta, "Assets/Resources/ScriptableObjects/new Carta de " + nome + ".asset");
            Selection.activeObject = carta;
        }
        //>Adiciona o comando "Instancia Trunfo desse Tipo" no menu do Scriptable Object
        [ContextMenu("Instancia Trunfo Desse Tipo")]
        private void CriaTrunfo()
        {
            Card carta = CreateInstance<Trunfo>();
            carta.tipo = this;
            carta.Initialize();
            //Cria o arquivo .asset da carta no projeto com referência à essa instancia
            AssetDatabase.CreateAsset(carta, "Assets/Resources/ScriptableObjects/new Trunfo de " + nome + ".asset");
            Selection.activeObject = carta;
        }
#endif
    }
}
