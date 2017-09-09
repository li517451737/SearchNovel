using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SearchNovel.UI.Core
{
    public class HttpHelper
    {
        public CookieContainer cookie;
        public HttpHelper()
        {
            cookie = new CookieContainer();
        }

        public static string GetHtml(string Url)
        {
            WebClient client = new WebClient();
            return client.DownloadString(Url);
        }

        public string Get_Request(
            string strUrl,
            CookieContainer _cookie = null,
            string strHost = "",
            string strRefer = "",
            string strOrigin = "",
            bool blnHttps = false,
            Dictionary<string, string> lstHeads = null,
            bool blnKeepAlive = false,
            string strEncoding = "utf-8",
            string strContentType = "",
            string strAccept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
            string strUserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36",
            bool blnAllowAutoRedirect = true,
            int intTimeout = 1000 * 30)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            //request = (HttpWebRequest)WebRequest.Create(strUrl);
            if (blnHttps)
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }
            request = (HttpWebRequest)WebRequest.Create(strUrl);
            if (blnHttps)
            {
                request.ProtocolVersion = HttpVersion.Version10;
            }
            request.KeepAlive = blnKeepAlive;
            request.Accept = strAccept;
            request.Timeout = intTimeout;
            request.Method = "GET";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.UserAgent = strUserAgent;
            request.AllowAutoRedirect = blnAllowAutoRedirect;
            //request.Proxy = null;
            if (!string.IsNullOrEmpty(strContentType))
            {
                request.ContentType = strContentType;
            }
            if (_cookie != null)
            {
                request.CookieContainer = _cookie;
            }
            if (!string.IsNullOrEmpty(strHost))
            {
                request.Host = strHost;
            }
            if (!string.IsNullOrEmpty(strRefer))
            {
                request.Referer = strRefer;
            }
            if (!string.IsNullOrEmpty(strOrigin))
            {
                request.Headers.Add("Origin", strOrigin);
            }
            if (lstHeads != null && lstHeads.Count > 0)
            {
                foreach (var item in lstHeads)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            response = (HttpWebResponse)request.GetResponse();
            var sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(strEncoding));
            string strResult = sr.ReadToEnd();
            sr.Close();
            request.Abort();
            response.Close();
            return strResult;

        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }

        public string POST_Request(
            string strUrl,
            string postDataStr,
            CookieContainer _cookie = null,
            string strHost = "",
            string strRefer = "",
            string strOrigin = "",
            bool blnHttps = false,
            Dictionary<string, string> lstHeads = null,
            bool blnKeepAlive = false,
            string strEncoding = "utf-8",
            string strContentType = "",
            string strAccept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
            string strUserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36",
            bool blnAllowAutoRedirect = true,
            int intTimeout = 1000 * 30)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            request = (HttpWebRequest)WebRequest.Create(strUrl);
            if (blnHttps)
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request.ProtocolVersion = HttpVersion.Version10;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }
            request.Accept = strAccept;
            request.Timeout = intTimeout;
            request.Method = "POST";
            request.KeepAlive = blnKeepAlive;
            request.Host = strHost;
            request.UserAgent = strUserAgent;
            request.Proxy = null;
            //if (!string.IsNullOrEmpty(strCertFile))
            //{
            //    X509Certificate cer = X509Certificate.CreateFromCertFile(strCertFile);
            //    request.ClientCertificates.Add(cer);
            //}
            if (_cookie != null)
            {
                request.CookieContainer = _cookie;
            }
            request.AllowAutoRedirect = blnAllowAutoRedirect;
            if (!string.IsNullOrEmpty(strContentType))
            {
                request.ContentType = strContentType;
            }
            if (!string.IsNullOrEmpty(strOrigin))
            {
                request.Headers.Add("Origin", strOrigin);
            }
            if (!string.IsNullOrEmpty(strRefer))
            {
                request.Referer = strRefer;
            }
            if (!string.IsNullOrEmpty(strHost))
            {
                request.Host = strHost;
            }
            if (lstHeads != null && lstHeads.Count > 0)
            {
                foreach (var item in lstHeads)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            if (!string.IsNullOrEmpty(postDataStr))
            {
                request.ContentLength = postDataStr.Length;
                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream);
                myStreamWriter.Write(postDataStr);
                myStreamWriter.Close();
            }

            response = (HttpWebResponse)request.GetResponse();
            var sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(strEncoding));
            string strResult = sr.ReadToEnd();
            sr.Close();
            request.Abort();
            response.Close();
            return strResult;
        }

        public string DownloadFile(
            string strURLAddress,
            string strPath,
            CookieContainer _cookie = null,
            string strHost = "",
            string strRefer = "",
            string strOrigin = "",
            Dictionary<string, string> lstHeads = null,
            string strAccept = "",
            string strUserAgent = "")
        {
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(strURLAddress) as HttpWebRequest;
                if (!string.IsNullOrEmpty(strAccept))
                {
                    request.Accept = strAccept;
                }
                if (!string.IsNullOrEmpty(strUserAgent))
                {
                    request.UserAgent = strUserAgent;
                }
                if (_cookie != null)
                {
                    request.CookieContainer = _cookie;
                }
                if (!string.IsNullOrEmpty(strOrigin))
                {
                    request.Headers.Add("Origin", strOrigin);
                }
                if (!string.IsNullOrEmpty(strRefer))
                {
                    request.Referer = strRefer;
                }
                if (!string.IsNullOrEmpty(strHost))
                {
                    request.Host = strHost;
                }
                if (lstHeads != null && lstHeads.Count > 0)
                {
                    foreach (var item in lstHeads)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                string strReceivePath = string.Empty;

                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                //创建本地文件写入流
                Stream stream = new FileStream(strPath, FileMode.Create);
                byte[] bArr = new byte[204800];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    stream.Flush();
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                responseStream.Close();
                return strPath;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string GetTimerStr()
        {
            return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds).ToString().ToString();
        }

    }
}
