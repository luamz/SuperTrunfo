using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Trunfo
{
    [SelectionBase]//faz com que esse objeto seja selecionado quando estiver numa hierarquia
    [RequireComponent(typeof(Image))]
    public class CardDisplay : MonoBehaviour
    {
        [SerializeField] private Card card;
        [Header("Modelo do Prefab")]
        [SerializeField] private TextMeshProUGUI identificacao;
        [SerializeField] private TextMeshProUGUI nome;
        [SerializeField] private Image artwork;
        [SerializeField] private GameObject modeloDeCriterio;
        private readonly List<GameObject> PontosDeCriterio = new List<GameObject>();
        private Image ModeloDeFundo;

        void Start()
        {
            Inicializa();
            SetaFrente();
        }
        private void Inicializa()
        {
            ModeloDeFundo = GetComponent<Image>();
        }
        private void SetaFrente()
        {
            ModeloDeFundo.sprite = GerenciadorDeSpriteDeCarta.Frente;
            nome.text = card.Nome;
            identificacao.text = card.Identificacao;
            artwork.gameObject.SetActive(true);
            artwork.sprite = card.Artwork;
            PontosDeCriterio.Clear();
            for (int i = 0; i < card.Pontos.Length; i++)
            {
                var novoCriterio = Instantiate(modeloDeCriterio, GetComponentInChildren<VerticalLayoutGroup>()
                .GetComponent<RectTransform>());
                novoCriterio.GetComponent<CriterioDisplay>().Inicializa(card, i);
                PontosDeCriterio.Add(novoCriterio);
            }
        }
        private void SetaVerso()
        {
            ModeloDeFundo.sprite = GerenciadorDeSpriteDeCarta.Verso;
            nome.text = "";
            identificacao.text = "";
            artwork.gameObject.SetActive(false);
            for (int i = 0; i < PontosDeCriterio.Count; i++)
            {
                Destroy(PontosDeCriterio[i]);
            }
            PontosDeCriterio.Clear();
        }

    }
}