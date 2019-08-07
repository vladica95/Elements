using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace Elements
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            int ulaz = 0;
            int n = 0;
            int k = 0;
            while (ulaz < 1)
            {
                Console.WriteLine("Izaberite ulaz: broj 1 za tastaturu , broj 2 za fajl:");
                ulaz = Int32.Parse(Console.ReadLine());
                if (ulaz > 2)
                {
                    ulaz = 0;
                }
            }
            if (ulaz == 1)
            {
                Console.WriteLine("Unesite broj ElemenataP: ");
                n = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Unesite broj ElemenataC: ");
                k = Int32.Parse(Console.ReadLine());
            }
            if (ulaz == 2)
            {
                string path = "";
                while (path == "")
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Choose json (*.json)|*.json";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        path = ofd.FileName.ToString();
                    }
                }
                using (StreamReader r = new StreamReader(path))
                {
                    var input = r.ReadToEnd();
                    var jObj = JObject.Parse(input);

                    n = (int)jObj.First.First;
                    k = (int)jObj.First.Next.First;
                    Console.WriteLine("Broj ElementP: " + n + "\nBroj ElementC je: " + k);
                }
            }
            Generator gen = new Generator(n, k);
            Console.WriteLine("Unesite ID po kome zelite da dobijete Element: ");
            string potraga = Console.ReadLine();
            gen.IDSearch(potraga);
            string opet = "y";
            while (opet == "y")
            {
                Console.WriteLine("Unesite vrednost p: ");
                int p = Int32.Parse(Console.ReadLine());
                string output = "baza";
                while (output != "baza" && output != "fajl")
                {
                    Console.WriteLine("Unesite gde zelite da sacuvate pretragu:" +
                        "baza za cuvanje u bazu ili fajl za cuvanje u fajl. ");

                    output = Console.ReadLine();
                }
                gen.Pretraga(p, output);
                Console.WriteLine("Traziti opet? y/n");
                opet = Console.ReadLine();
            }
            gen.Citanje("02:50:01");
        //    gen.ProbaZaUpis();
        // gen.ProbaZaCitanje("02:50:01");

        }
    }
  
}
