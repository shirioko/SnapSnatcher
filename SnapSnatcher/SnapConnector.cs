using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SnapSnatcher
{
    public class SnapConnector
    {
        protected string username;
        protected string authToken;
        protected Cache cache;
        const string BASE_URL = "https://feelinsonice-hrd.appspot.com/bq";
        const string VERSION = "4.1.07";
        const string SECRET = "iEk21fuwZApXlz93750dmW22pw389dPwOk";
        const string PATTERN = "0001110111101110001111010101111011010001001110011000110001000110";
        const string USER_AGENT = "Snapchat/4.1.07 (Nexus 4; Android 18; gzip)";
        const string STATIC_TOKEN = "m198sOkJEn37DjqZ32lpRu76xmw288xSQ9";

        public SnapConnector(string username, string authToken)
        {
            this.username = username;
            this.authToken = authToken;
            this.cache = new Cache();
        }

        public JsonClasses.Snap[] GetSnaps(string data)
        {
            JObject foo = JObject.Parse(data);
            JToken response = foo["updates_response"];
            JsonClasses.Snap[] snaps = response["snaps"].ToObject<JsonClasses.Snap[]>();
            return snaps;
        }

        public JsonClasses.Story[] GetStories(string data)
        {
            List<JsonClasses.Story> stories = new List<JsonClasses.Story>();
            JObject foo = JObject.Parse(data);
            JToken response = foo["stories_response"];
            JArray allfriendsstories = response["friend_stories"].ToObject<JArray>();
            foreach (JToken friend in allfriendsstories)
            {
                JArray substory = friend["stories"].ToObject<JArray>();
                foreach (JToken substoryy in substory)
                {
                    JsonClasses.Story story = substoryy["story"].ToObject<JsonClasses.Story>();
                    stories.Add(story);
                }
            }
            return stories.ToArray();
        }

        public byte[] GetMedia(string id)
        {
            string timestamp = this.timestamp();
            List<string> data = new List<string>();
            data.AddRange(new[] { string.Format("timestamp={0}", timestamp), string.Format("username={0}", this.username), string.Format("id={0}", id) });
            return this.post_Raw("/blob", data, timestamp);
        }

        public string GetUpdates()
        {
            string timestamp = this.timestamp();
            List<string> data = new List<string>();
            data.AddRange(new [] { string.Format("timestamp={0}", timestamp), string.Format("username={0}", this.username) });
            return this.post("/all_updates", data, timestamp);
        }

        protected readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);
        protected string timestamp()
        {
            TimeSpan ts = DateTime.Now - UnixEpoch;
            return ts.TotalSeconds.ToString(CultureInfo.InvariantCulture);
        }

        protected string getReqToken(string timestamp)
        {
            string tok = this.authToken;
            if (string.IsNullOrEmpty(tok))
            {
                tok = STATIC_TOKEN;
            }
            string first = string.Format("{0}{1}", SECRET, tok);
            string second = string.Format("{0}{1}", timestamp, SECRET);

            first = Sha256(first);
            second = Sha256(second);

            StringBuilder token = new StringBuilder();
            for (int i = 0; i < PATTERN.Length; i++)
            {
                char c = PATTERN[i];
                if (c == '0')
                {
                    token.Append(first[i]);
                }
                else
                {
                    token.Append(second[i]);
                }
            }
            return token.ToString();
        }

        protected byte[] post_Raw(string url, List<string> postData, string timestamp)
        {
            return _post(url, postData, timestamp, true);
        }

        protected string post(string url, List<string> postData, string timestamp)
        {
            return Encoding.UTF8.GetString(this._post(url, postData, timestamp, false));
        }

        protected byte[] _post(string url, List<string> postData, string timestamp, bool raw = false)
        {
            postData.Add(string.Format("req_token={0}", this.getReqToken(timestamp)));
            postData.Add(string.Format("version={0}", VERSION));
            url = string.Format("{0}{1}", BASE_URL, url);
            HttpWebRequest r = WebRequest.Create(url) as HttpWebRequest;

            r.UserAgent = USER_AGENT;
            r.Method = "POST";
            r.Accept = "*/*";
            r.ContentType = "application/x-www-form-urlencoded";

            string postbody = string.Join("&", postData.ToArray());
            using (StreamWriter sw = new StreamWriter(r.GetRequestStream()))
            {
                sw.Write(postbody);
            }

            HttpWebResponse resp = r.GetResponse() as HttpWebResponse;

            using (Stream rs = resp.GetResponseStream())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] buff = new byte[8192];
                    int count = 0;
                    int offset = 0;
                    do
                    {
                        count = rs.Read(buff, 0, buff.Length);
                        ms.Write(buff, 0, count);
                        offset += count;
                    } while (count > 0);
                    resp.Close();
                    return ms.ToArray();
                }
            }
        }

        public string Login(string username, string password)
        {
            string timestamp = this.timestamp();
            List<string> postData = new List<string>();
            postData.AddRange(new string[] {
                string.Format("username={0}", username),
                string.Format("password={0}", password),
                string.Format("timestamp={0}", timestamp)
            });
            string result = this.post("/login", postData, timestamp);

            return result;
        }

        protected byte[] get_Raw(string url)
        {
            url = string.Format("{0}{1}", BASE_URL, url);

            HttpWebRequest r = WebRequest.Create(url) as HttpWebRequest;
            r.UserAgent = USER_AGENT;
            HttpWebResponse resp = r.GetResponse() as HttpWebResponse;

            using (Stream rs = resp.GetResponseStream())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] buff = new byte[8192];
                    int count = 0;
                    int offset = 0;
                    do
                    {
                        count = rs.Read(buff, 0, buff.Length);
                        ms.Write(buff, 0, count);
                        offset += count;
                    } while (count > 0);
                    resp.Close();
                    return ms.ToArray();
                }
            }
        }

        protected string get(string url)
        {
            return Encoding.UTF8.GetString(this.get_Raw(url));
        }

        protected static string Sha256(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            var hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            return hash.Aggregate(string.Empty, (current, x) => current + String.Format("{0:x2}", x));
        }

        public byte[] GetStoryMedia(string media_id, string media_key, string media_iv)
        {
            byte[] data = this.get_Raw(string.Format("/story_blob?story_id={0}", media_id));
            if(data != null && data.Length > 0)
            {
                return Form1.decryptCBC(data, media_key, media_iv);
            }
            return data;
        }
    }
}
