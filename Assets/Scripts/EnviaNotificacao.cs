using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Text;

namespace Trunfo
{
    public class EnviaNotificacao : MonoBehaviour
    {
        public void Envia()
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications")
                        as HttpWebRequest;
                        

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            byte[] byteArray = Encoding.UTF8.GetBytes("{"
                                        + "\"app_id\": \"96f64d30-af66-4072-acbb-a98c3d4e9763\","
                                        + "\"contents\": {\"en\": \"Oi Relâmpago Marquinhos e Rique Seta\"},"
                                        + "\"include_player_ids\": [\"5fb1ac5e-1d95-47d3-bd5e-70bbf87b5ae9\",\"fba9c574-c6fa-4ed2-a0c8-36743c6ae7b0\"]}");

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent);
        }
    }
}
