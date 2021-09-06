using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Trunfo
{
    public class CriterioDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nome;
        [SerializeField] private TextMeshProUGUI valor;
        public int numCriterio { get; private set; }
        internal TextMeshProUGUI Nome { get => nome; }
        internal TextMeshProUGUI Valor { get => valor; }

        public static Action<int> criterioEscolhido;
        public void Inicializa(Card card, int index)
        {
            nome.text = card.tipo.Atributos[index];
            valor.text = card.Pontos[index].ToString();
            numCriterio = index;
        }

        public void Aaaaaa()
        {
            criterioEscolhido?.Invoke(numCriterio);
            Debug.Log(nome.text);
            Debug.Log(valor.text);
            Debug.Log(numCriterio);
        }
    }
}
