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
using System.Net.Http.Headers;
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
        string login_path = "https://login.eveonline.com/oauth/authorize?response_type=code&redirect_uri=http://localhost:5000/oauth-callback&client_id=";
        string token_path = "https://login.eveonline.com/oauth/token";
        string autho_code;
        bool thread_run;
        List<string> charIds;
        Thread _responseThread;
        public SSO()
        {
            InitializeComponent();
            httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:5000/");
            httpListener.Start();
            charIds = new List<string>();
            thread_run = true;
            _responseThread = new Thread(ResponseThread);
            _responseThread.Start();
            defalutBrowser = GetDefaultBrowserName();
            try
            {
                string[] keys = File.ReadAllLines("key.cfg");
                keys[0] = keys[0].Trim();
                keys[1] = keys[1].Trim();
                autho_code = Convert.ToBase64String(Encoding.UTF8.GetBytes(keys[0] + ":" + keys[1]));
                login_path += keys[0];
            }
            catch(Exception ex)
            {
                autho_code = "";
            }
            ShowCharacterList();
        }

        void ResponseThread()
        {
            while (thread_run)
            {
                HttpListenerContext context;
                try
                {
                    context = httpListener.GetContext();
                }
                catch (Exception ex)
                {
                    return;
                }
                 // get a context
                                                                         // Now, you'll find the request URL in context.Request.Url
                if (!context.Request.Url.ToString().Contains("oauth-callback")) continue;
                string code = context.Request.Url.Query;
                code = code.Replace("?code=", "");
                byte[] _responseArray = Encoding.UTF8.GetBytes("<html><head><title>Input Character</title></head>" +
                "<body>Character added <strong>---</strong></body></html>"); // get the bytes to response
                context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length); // write bytes to the output stream
                context.Response.KeepAlive = false; // set the KeepAlive bool to false
                context.Response.Close(); // close the connection
                Console.WriteLine("Respone given to a request.");
                getToken(code);
            }
        }
        private void add_char_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(defalutBrowser, login_path);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void delete_char_Click(object sender, EventArgs e)
        {
            if(charList.SelectedItems.Count > 0)
            {
                string id = charIds[charList.Items.IndexOf(charList.SelectedItems[0])];
                CharacterManager.RemoveCharacter(id);
                ShowCharacterList();
            }
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
                errorLbl.Text = e.Message;
            }

            return ret;
        }

        async Task getToken(string code)
        {
            var body = new Dictionary<string, string>()
            {
                { "grant_type", "authorization_code" },
                { "code", code }
            };
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(body);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", autho_code);
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(token_path, content);
            Dictionary<string, string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>( (await response.Content.ReadAsStringAsync()).ToString());
            if(res.ContainsKey("access_token"))
            {
                await getCharInfo(code, res["access_token"], res["refresh_token"]);
                errorLbl.Text = "";
            } else
            {
                errorLbl.Text = "Get token failed";
            }
        }

        string verify_url = "https://login.eveonline.com/oauth/verify";
        async Task getCharInfo(string code, string token, string refresh) { 
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(verify_url);
            Dictionary<string, string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>((await response.Content.ReadAsStringAsync()).ToString());
            if(res.ContainsKey("CharacterID"))
            {
                CharacterManager.AddCharacter(new CharInfo(res["CharacterName"], res["CharacterID"], token, refresh, code));
                ShowCharacterList();
                errorLbl.Text = "";
            } else
            {
                errorLbl.Text = "Get character info failed";
            }
        }

        private void ShowCharacterList()
        {
            charList.Items.Clear();
            Dictionary<string, CharInfo> chars = CharacterManager.GetCharList();
            charIds.Clear();
            foreach(string cid in chars.Keys)
            {
                charList.Items.Add(new ListViewItem(cid + " - " + chars[cid].Name));
                charIds.Add(cid);
            }
        }

        private void SSO_FormClosed(object sender, FormClosedEventArgs e)
        {
            httpListener.Prefixes.Remove("http://localhost:5000/");
            httpListener.Stop();
        }
    }
}
