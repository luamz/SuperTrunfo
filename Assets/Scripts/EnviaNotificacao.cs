using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Text;

//Esse modulo envia notificacao para o usuario
namespace Trunfo
{
    public class EnviaNotificacao : MonoBehaviour
    {
        public static void Envia(string idSala)
        {

            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            

            request.Headers.Add("authorization", "Basic YzE4NTU5MjItOGZiMy00MTQ0LTgwZTUtOGZlNTYxMTY2NzUw");

            byte[] byteArray = Encoding.UTF8.GetBytes("{"
                                            + "\"app_id\": \"96f64d30-af66-4072-acbb-a98c3d4e9763\","
                                            + "\"headings\": {\"en\": \"Venha jogar!!\"},"
                                            + "\"contents\": {\"en\": \"Alguém te convidou para jogar Super Trunfo\"},"
                                            + "\"data\": {\"sala\": \""+idSala+"\"},"
                                            + "\"filters\": [{\"field\": \"last_session\", \"key\": \"session_time\", \"relation\": \">\", \"value\": \"30.0\"}]}");
                                        
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
                Debug.Log(responseContent);
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
