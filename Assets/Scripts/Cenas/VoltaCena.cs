using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Botao para voltar para o menu
namespace Trunfo
{

    public class VoltaCena : MonoBehaviour
    {
        public void voltar()
        {
            SceneManager.LoadScene("Menu");

        }
    }
}
