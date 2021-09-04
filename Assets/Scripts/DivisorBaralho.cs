using System.Collections;
using System.Collections.Generic;
using System.Linq;  
using UnityEngine;

namespace Trunfo
{
    public class DivisorBaralho : MonoBehaviour
    {
        [SerializeField] private Player Player1;
        [SerializeField] private Player Player2;
        [SerializeField] private List<CardDisplay> ListaDeCartas;

        // Start is called before the first frame update
        void Start()
        {
            DividirBaralho();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void DividirBaralho(){
            ListaDeCartas = ListaDeCartas.OrderBy(seed=>Random.Range(0,32)).ToList();
    
            Player1.Mao.InsereCartas(ListaDeCartas.Take(ListaDeCartas.Count()/2).ToArray());
            Player2.Mao.InsereCartas(ListaDeCartas.Skip(ListaDeCartas.Count()/2).ToArray());
        }

        public void ImprimeBaralhoDividido(){
             Debug.Log(Player1.Mao.cartas.Count);
             Debug.Log(Player2.Mao.cartas.Count);
        }

    }
}
