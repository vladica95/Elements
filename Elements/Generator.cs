using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace Elements
{
    class Generator
    {

        ElementP[] elementPs;

        public Generator(int brElementP, int brElementC)
        {
            elementPs = new ElementP[brElementP];
            for (int i = 0; i < brElementP; i++)
            {
                elementPs[i] = new ElementP(brElementC, i + 1);
                elementPs[i].Stampaj();
            }
        }

        public ElementP IDSearch(string ID)
        {
            ElementP el = new ElementP();
            foreach(var v in elementPs)
            {
                if (v.IdentifikacioniKod == ID)
                {
                    el = v;
                }
            }
            if (el.IdentifikacioniKod != "")
            {
                el.Stampaj();
                return el;
            }
            else
            {
                Console.WriteLine("Nema elemenata za taj ID!");
                return null;
            }
        }


        public void Pretraga(int p, string output)
        {
            if (output == "baza") 
            {
                Console.WriteLine("Smestanje u bazu.");
            }
          else  if (output == "fajl")
            {
                string path = "";
                Console.WriteLine("Smestanje u fajl.");
                while (path == "")
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    //DialogResult result = fbd.ShowDialog();
                    //txtSelectedFolderPath.Text = fbd.SelectedPath.ToString();
                   // OpenFileDialog ofd = new OpenFileDialog();
                   // ofd.Filter = "Choose json (*.json)|*.json";
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        //  path = ofd.FileName.ToString();
                       path= fbd.SelectedPath.ToString()+"\\searchRes.json";
                    }
                }
                for (int i = 0; i < elementPs.Length; i++)
                {
                    if (elementPs[i].NadjiSumu() > p)
                    {
                        elementPs[i].UpisiUFajl(path);
                    }
                }
            }
            else
            {
                Console.WriteLine("Doslo je do greske!");
            }
        }
    }
}
