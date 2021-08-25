using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Trunfo
{
    public class Inicial : MonoBehaviour
    {
        public void botao_jogar()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("exists");
        }
    }
}
