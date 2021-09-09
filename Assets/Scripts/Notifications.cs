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

        // Start is called before the first frame update
        void Start()
        {
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



        public class Sala
        {
            public string en;
            public static Sala CreateFromJSON(string jsonString)
            {
                return JsonUtility.FromJson<Sala>(jsonString);
            }
        }

        // Gets called when the player opens a OneSignal notification.
        public static void OneSignalHandleNotificationOpened(OSNotificationOpenedResult result)
        {



            SceneManager.LoadScene("EntrarPartida");
            //string Sala = result.ToString();
            Sala sala = Sala.CreateFromJSON(result.ToString());
            //Sala_id sala_id = JsonSerializer.Deserialize<Sala_id>(result);
            //string Sala = sala.contents["en"];
            TMP_InputField texto = GameObject.Find("Input num_partida").GetComponentInChildren<TMP_InputField>();
            texto.text = sala.en;


            // Place your app specific notification opened logic here.
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
