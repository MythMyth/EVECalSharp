using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EveCal
{
    public partial class SSO : Form
    {
        HttpListener httpListener;
        string defalutBrowser;
        public SSO()
        {
            InitializeComponent();
            httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:5000/");
            httpListener.Start();

            Thread _responseThread = new Thread(ResponseThread);
            _responseThread.Start();
            defalutBrowser = GetDefaultBrowserName();
        }

        void ResponseThread()
        {
            while (true)
            {
                HttpListenerContext context = httpListener.GetContext(); // get a context
                                                                         // Now, you'll find the request URL in context.Request.Url
                string code = context.Request.Url.Query;
                code = code.Replace("?code=", "");
                byte[] _responseArray = Encoding.UTF8.GetBytes("<html><head><title>Input Character</title></head>" +
                "<body>Character added <strong>---</strong></body></html>"); // get the bytes to response
                context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length); // write bytes to the output stream
                context.Response.KeepAlive = false; // set the KeepAlive bool to false
                context.Response.Close(); // close the connection
                Console.WriteLine("Respone given to a request.");
                
            }
        }
        string login_path = "https://login.eveonline.com/oauth/authorize?response_type=code&redirect_uri=http://localhost:5000/oauth-callback&client_id=bde31e1c883541088a340b124b3734f5";
        string token_path = "https://login.eveonline.com/oauth/token";
        private void add_char_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(defalutBrowser, login_path);
            }
            catch (Exception exc)
            {
                
            }
        }

        private void delete_char_Click(object sender, EventArgs e)
        {

        }

        string GetDefaultBrowserName()
        {
            string ret = "";
            try
            {
                string defaultBrowserName = Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.html\\UserChoice", "ProgId", "").ToString();
                string defaultBrowserKey = "HKEY_CLASSES_ROOT\\" + defaultBrowserName + "\\shell\\open\\command";
                ret = Registry.GetValue(defaultBrowserKey, null, "").ToString();
                ret = ret.Split("\"")[1];
            }
            catch(Exception e)
            {

            }

            return ret;
        }

        async Task getToken(string code)
        {
            Dictionary<string, string> body = new Dictionary<string, string>()
            {
                { "grant_type", "authorization_code" },
                { "code", code }
            };
            HttpClient client = new HttpClient();
            HttpContent content = new FormUrlEncodedContent(body);
            client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            client.DefaultRequestHeaders.Add("Authorization ", "Basic YmRlMzFlMWM4ODM1NDEwODhhMzQwYjEyNGIzNzM0ZjU6QjZPQjdzMXhhSG41WlRDZFBZUE14b2tMYzlkYmF0UkxlblV4THhlUg==");
            var response = await client.PostAsync(token_path, content);
            Dictionary<string, string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>( (await response.Content.ReadAsStringAsync()).ToString());

        }

        async Task getRefreshToken(string code, string refreshToken)
        {

        }
        string verify_url = "https://login.eveonline.com/oauth/verify";
        async Task getCharInfo(string token) { 
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await client.GetAsync(verify_url);
            Dictionary<string, string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>((await response.Content.ReadAsStringAsync()).ToString());
        }
    }
}
