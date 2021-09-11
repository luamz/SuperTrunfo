using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Modulo responsavel por mostrar e definir o criterio
namespace Trunfo
{
    public class CriterioDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nome;
        [SerializeField] private TextMeshProUGUI valor;
        private Button botao;
        public int numCriterio { get; private set; }
        internal TextMeshProUGUI Nome { get => nome; }
        internal TextMeshProUGUI Valor { get => valor; }

        public static Action<int> criterioEscolhido;
        void Start()
        {
            botao = GetComponent<Button>();
            botao.enabled = false;
        }
        public void Inicializa(Card card, int index)
        {
            nome.text = card.tipo.Atributos[index];
            valor.text = card.Pontos[index].ToString();
            numCriterio = index;
        }

        public void EscolheCriterio()
        {
            criterioEscolhido?.Invoke(numCriterio);
        }
        public void LiberaCriterio()
        {
            botao.enabled = true;
        }
    }
}
