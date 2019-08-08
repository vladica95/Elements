using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Elements
{
    public class ElementP
    {
        private int redniBroj;
        private string identifikacioniKod;
        private ElementC[] elementi;

        private static readonly Random rand = new Random();
        public ElementP()
        {
            identifikacioniKod = "";
        }
        public ElementP(string jSon)
        {
            FromJson(jSon);
        }

        public ElementP(int brojElemenata, int redniBr)
        {
            identifikacioniKod = DateTime.Now.Day.ToString() +
                DateTime.Now.Month.ToString() +
                DateTime.Now.Year.ToString() +
                DateTime.Now.Hour.ToString() +
                DateTime.Now.Minute.ToString() +
                DateTime.Now.Second.ToString() +
                (rand.Next(1000, 9999)).ToString();
            redniBroj = redniBr;

            elementi = new ElementC[brojElemenata];
            for (int i = 0; i < brojElemenata; i++)
            {
                elementi[i] = new ElementC();
            }
        }
        public string IdentifikacioniKod
        {
            get { return this.identifikacioniKod; }
            set { this.identifikacioniKod = value; }
        }
        public int RedniBroj
        {
            get { return this.redniBroj; }
            set { this.redniBroj = value; }
        }
        public ElementC[] Elementi
        {
            get { return this.elementi; }
        }

        public int NadjiSumu()
        {
            int suma = 0;
            for (int i = 0; i < elementi.Length; i++)
            {
                suma = suma + elementi[i].Vrednost;
            }
            return suma;
        }

        public void AddCs(List<ElementC> elC)
        {
            elementi = new ElementC[elC.Count];
            for (int i = 0; i < elC.Count; i++)
            {
                elementi[i] = elC[i];
            }
        }

        public void FromJson(string json)
        {
            var jObject = JObject.Parse(json);

            if (jObject != null)
            {
                redniBroj = (int)jObject["redniBroj"];
                identifikacioniKod = jObject["identifikacioniKod"].ToString();

                JArray elementCs = (JArray)jObject["elementi"];
                if (elementCs != null)
                {
                    List<ElementC> elC = new List<ElementC>();
                    foreach (var item in elementCs)
                    {
                        elC.Add(new ElementC((char)item["grupa"], (int)item["vrednost"]));
                    }
                    elementi = new ElementC[elC.Count];
                    for (int i = 0; i < elC.Count; i++)
                    {
                        elementi[i] = elC[i];
                    }
                }
            }
        }

        public void Stampaj()
        {
            Console.WriteLine("ElementP: ");
            Console.WriteLine("Redni broj: " + redniBroj +
                "\nIdentifikacioni kod: " + IdentifikacioniKod);
            Console.WriteLine("Niz ElementC: ");
            if (elementi != null)
            {
                for (int i = 0; i < elementi.Length; i++)
                {
                    elementi[i].Stampaj();
                }
            }
        }
        public string ToJson()
        {
            var jElP = Newtonsoft.Json.JsonConvert.SerializeObject(this,
                                   Newtonsoft.Json.Formatting.Indented);
            var jObj = JObject.Parse(jElP);
            var jNiz = new JArray();

            foreach (var v in elementi)
            {
                var elC = Newtonsoft.Json.JsonConvert.SerializeObject(v,
                                   Newtonsoft.Json.Formatting.Indented);

                var JObjC = JObject.Parse(elC);
                jNiz.Add(JObjC);
            }
            jObj["ElementC"] = jNiz;

            string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jObj,
                                   Newtonsoft.Json.Formatting.Indented);
            return newJsonResult;
        }
        public void UpisiUFajl(string path)
        {

            string JsonResult = ToJson();

            using (var tw = new StreamWriter(path, true))
            {
                tw.WriteLine(JsonResult);
                tw.Close();
            }
        }
    }
}

