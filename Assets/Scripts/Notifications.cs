using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


namespace Trunfo
{
    public class Notifications : MonoBehaviour
    {
        //private static EntraNaSala Sala;
        //public EntraNaSala sala;
        public static Action<string> Redireciona;
        private static string salaId="";
    

        // Start is called before the first frame update
        void Start()
        {
            //Sala = sala;
            //DontDestroyOnLoad(gameObject.transform.root);
            
            // Uncomment this method to enable OneSignal Debugging log output 
            // OneSignal.SetLogLevel(OneSignal.LOG_LEVEL.VERBOSE, OneSignal.LOG_LEVEL.NONE);

            // Replace 'YOUR_ONESIGNAL_APP_ID' with your OneSignal App ID.
            OneSignal.StartInit("96f64d30-af66-4072-acbb-a98c3d4e9763")
                .HandleNotificationOpened(OneSignalHandleNotificationOpened)
                .Settings(new Dictionary<string, bool>() {
                { OneSignal.kOSSettingsAutoPrompt, false },
                { OneSignal.kOSSettingsInAppLaunchURL, false } })
                .EndInit();

            OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;

            // iOS - Shows the iOS native notification permission prompt.
            //   - Instead we recomemnd using an In-App Message to prompt for notification 
            //     permission to explain how notifications are helpful to your users.
            OneSignal.PromptForPushNotificationsWithUserResponse(OneSignalPromptForPushNotificationsReponse);
        }

        // Gets called when the player opens a OneSignal notification.
        public static void OneSignalHandleNotificationOpened(OSNotificationOpenedResult result)
        {
            try
            {
                string cod_sala = result.notification.payload.additionalData["sala"].ToString();

                if (cod_sala != "")
                {
                    salaId=cod_sala;
                    SceneManager.sceneLoaded += SetaIdSala; 
                    SceneManager.LoadScene("EntrarPartida");
                    //Redireciona?.Invoke();
                    // GameObject.Find("Notifications").GetComponent<Notifications>()
                    // .StartCoroutine(setaId(cod_sala));

                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }


        private static void SetaIdSala(Scene scene,LoadSceneMode mode){
            if (scene.name=="EntrarPartida"){
               Redireciona?.Invoke(salaId); 
            }
        }

        private static IEnumerator setaId(string codigo)
        {
            yield return null;
            Redireciona?.Invoke(codigo);
        }

        // iOS - Fires when the user anwser the notification permission prompt.
        private void OneSignalPromptForPushNotificationsReponse(bool accepted)
        {
            // Optional callback if you need to know when the user accepts or declines notification permissions.
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

