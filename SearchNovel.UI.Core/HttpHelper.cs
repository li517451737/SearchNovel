using HtmlAgilityPack;
using SearchNovel.Model.Models;
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
        public static string GetHtml(string url)
        {
            string html = string.Empty;
            bool retry = false;
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", "Microsoft Internet Explorer");
                client.Headers.Add("Host", "www.b5200.org");
                var myProxy = new WebProxy();
                myProxy.Credentials = CredentialCache.DefaultCredentials;
                myProxy.Address = new Uri(url);
                client.Proxy = myProxy;
                while (!retry)
                {
                    try
                    {
                        html = client.DownloadString(url);
                        retry = true;
                    }
                    catch
                    {
                        retry = false;
                    }
                }
            }
            return html;
        }

        public static string Get_Request(
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
            bool blnAllowAutoRedirect = false,
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
        /// <summary>
        /// 获取小说基本信息
        /// </summary>
        /// <param name="href"></param>
        /// <param name="novel"></param>
        /// <returns></returns>
        public static HtmlNodeCollection GetNovelHtml(string href, ref Novel novel)
        {
            var desDoc = new HtmlDocument();
            string html = GetHtml(href);
            if (string.IsNullOrEmpty(html))
                throw new Exception("小说内容为空");
            desDoc.LoadHtml(html);
            var mainInfo = desDoc.DocumentNode.SelectSingleNode("//*[@id=\"wrapper\"]/div[5]");
            novel.Title = mainInfo.SelectSingleNode("//*[@id=\"info\"]/h1").InnerText;
            novel.Author = mainInfo.SelectSingleNode("//*[@id=\"info\"]/p[1]").InnerText.Replace("作&nbsp;&nbsp;&nbsp;&nbsp;者：", "");
            novel.LastUpdate = Convert.ToDateTime(mainInfo.SelectSingleNode("//*[@id=\"info\"]/p[3]").InnerText.Replace("最后更新：", ""));
            novel.Intro = mainInfo.SelectSingleNode("//*[@id=\"intro\"]/p").InnerText;
            novel.CoverImg = mainInfo.SelectSingleNode("//*[@id=\"fmimg\"]/img")?.Attributes["src"].Value.ToString();
            novel.SourceUrl = href;
            novel.NovelState = "完结";
            var chapters = desDoc.DocumentNode.SelectNodes("//*[@id=\"list\"]/dl/dd/a");

            return chapters;
        }
    }
}
