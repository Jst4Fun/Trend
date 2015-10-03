using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Trend
{
    public static class Trend
    {
        static Dictionary<string, float> trends = new Dictionary<string, float>();
        static DateTime timestamp;
        static CultureInfo culture = new CultureInfo("us");
        static public LongRate[] Trends
        {
            get
            {
                if (trends == null)
                {
                    Update();
                }
                LongRate[] data = new LongRate[trends.Count];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = new LongRate(trends.ElementAt(i).Key, trends.ElementAt(i).Value);
                }
                return data;
            }
        }

        public static string GetResponse(string link)
        {
            WebRequest request = WebRequest.Create(link);
            //request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

#if DEBUG
            // Get the status.
            Debug.WriteLine("Status: " + response.StatusDescription);
#endif

            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();

            // return the content.
            return responseFromServer;
        }

        internal static void PrintAll()
        {
            Update();
            StringBuilder str = new StringBuilder(256);
            foreach (var item in trends)
            {
                str.Append(item.Key + " - ");
                str.Append(item.Value);
                str.AppendLine();
            }
            System.Windows.Forms.MessageBox.Show(str.ToString());
        }

        public static void Update()
        {
            if (timestamp != null && (DateTime.Now - timestamp).TotalMinutes < 10)
            {
                return;
            }
            string resp = GetResponse("https://query.yahooapis.com/v1/public/yql?q=SELECT%20*%20FROM%20data.html.cssselect%20WHERE%20url%3D'http%3A%2F%2Ffxtrade.oanda.co.uk%2Flang%2Fru%2Fanalysis%2Fopen-position-ratios'%20AND%20css%3D'.position-ratio-list'&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
            Parse(resp);
            timestamp = DateTime.Now;
        }


        public static float Long(string cur)
        {
            Update();
            if (trends.ContainsKey(cur))
            {
                return trends[cur];
            }
            System.Windows.Forms.MessageBox.Show(cur + " - нет такой пары");
            //throw new ArgumentException();
            return 0;
        }

        static void Parse(string str)
        {
            char[] trim = new char[3] { '%', ' ', '\n' };
            JObject jRoot = JObject.Parse(str);
            IList<JToken> results = jRoot["query"]["results"]["results"]["ol"][0]["li"].Children().ToList();
            foreach (JToken result in results)
            {
                string name = result["name"].ToString();

                string v0 = result["div"]["div"]["span"][0]["content"].ToString();
                string v = v0.Trim(trim);
                float value = float.Parse(v, culture.NumberFormat);
                if (trends.ContainsKey("name"))
                {
                    trends[name] = value;
                }
                else
                {
                    trends.Add(name, value);
                }
            }

        }
    }
        public class LongRate
    {
        string name;
        float value;
        public LongRate (string Name, float Rate)
        {
            name = Name;
            value = Rate;
        }
        public string Name { get { return name; } }
        public float Rate { get { return value; } }
    }

}
