using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Trunfo
{
    public static class GerenciadorDeSpriteDeCarta
    {
        private const string caminhoBase = "Assets/Sprites/Carta/";
        public static readonly Sprite Frente =
            AssetDatabase.LoadAssetAtPath<Sprite>(caminhoBase + "Carta - Frente.png");
        public static readonly Sprite Verso =
            AssetDatabase.LoadAssetAtPath<Sprite>(caminhoBase + "Carta - Verso.png");

    }
}
