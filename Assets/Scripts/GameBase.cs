using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using Firebase.Firestore;

// Definir o usuario como offline no banco
namespace Trunfo
{
    public class GameBase : AuthManager
    {
        void OnApplicationQuit()
        {
            Debug.Log("Saindo");
            Debug.Log(this.jogadorData.Name);
            if (this.jogadorData.UserToken != null)
            {
                this.status = false;
                UpdateFirestore(this.jogadorData.UserToken);
            }

        }
    }
}
