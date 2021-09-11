using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Responsavel por genrenciar a UI dos menus
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Variaveis do objeto da tela
    public GameObject loginUI;
    public GameObject registerUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    //Funções para mudar a UI da tela de login
    public void LoginScreen() //Botão para voltar para a tela de login
    {
        loginUI.SetActive(true);
        registerUI.SetActive(false);
    }
    public void RegisterScreen() // Botão de cadastro
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
    }
}
