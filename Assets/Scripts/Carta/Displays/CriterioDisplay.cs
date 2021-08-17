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
        internal TextMeshProUGUI Nome { get => nome; }
        internal TextMeshProUGUI Valor { get => valor; }
        public void Inicializa(Card card, int index)
        {
            nome.text = card.tipo.Atributos[index];
            valor.text = card.Pontos[index].ToString();
        }
    }
}
