using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/* 
Essa classe é responsável por adicionar e configurar o OneSignal no App
*/

namespace Trunfo
{
    public class Notifications : MonoBehaviour
    {
        private static string salaId = "";
        public static Action<string> Redireciona;

        void Start()
        {
            // Torna um aplicativo notificável pelo One Signal
            OneSignal.StartInit("96f64d30-af66-4072-acbb-a98c3d4e9763")
                .HandleNotificationOpened(OneSignalHandleNotificationOpened)
                .Settings(new Dictionary<string, bool>() {
                { OneSignal.kOSSettingsAutoPrompt, false },
                { OneSignal.kOSSettingsInAppLaunchURL, false } })
                .EndInit();

            OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;
            OneSignal.PromptForPushNotificationsWithUserResponse(OneSignalPromptForPushNotificationsReponse);
        }

        // Método chamado quando um jogador clica numa notificação
        public static void OneSignalHandleNotificationOpened(OSNotificationOpenedResult result)
        {
            try
            {
                // Pega o código da sala que chega pela notificação
                string cod_sala = result.notification.payload.additionalData["sala"].ToString();

                if (cod_sala != "")
                {
                    salaId = cod_sala;

                    // Abre a cena Entrar em Partida
                    SceneManager.sceneLoaded += SetaIdSala;
                    SceneManager.LoadScene("EntrarPartida");

                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        private static void SetaIdSala(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "EntrarPartida")
            {
               Redireciona?.Invoke(salaId); 
            }
        }

        // iOS - É iniciada quando o usuario responde o prompt de permissão de notificação
        private void OneSignalPromptForPushNotificationsReponse(bool accepted)
        {
        }

        void Update()
        {

        }
    }
}

