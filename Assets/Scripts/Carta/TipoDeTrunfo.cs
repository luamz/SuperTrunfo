using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


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
    }
}
