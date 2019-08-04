using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Elements
{
    class ElementP
    {

        private int redniBroj;
        private string identifikacioniKod;
        private ElementC[] elementi;

        private static readonly Random rand = new Random();
        public ElementP()
        {
            identifikacioniKod = "";
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

        }
        public int RedniBroj

        {
            get { return this.redniBroj; }

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

        public void UpisiUFajl(string path)
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


            using (var tw = new StreamWriter(path, true))
            {
                tw.WriteLine(newJsonResult);
                tw.Close();
            }
        }
    }
}

