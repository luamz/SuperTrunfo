using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trunfo
{
    public class Trunfo : Card
    {
        public override bool Compara(Card carta, int index)
        {
            Debug.Log("SuperTrunfo!");
            if (ComparaGrupo(carta)) return true;
            return base.Compara(carta, index);
        }
        bool ComparaGrupo(Card carta) => carta.Identificacao.grupo != 'A';
    }
}
