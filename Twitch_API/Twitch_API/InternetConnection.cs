using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_API
{
    class InternetConnection
    {
        private bool isConnected = false;
        public bool IsConnected => isConnected;
        public void CheckConnection()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://google.com");
                request.Timeout = 5000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                    isConnected = true;
                else
                    isConnected = false;
            }
            catch
            {
                isConnected = false;
            }
        }
    }
}
