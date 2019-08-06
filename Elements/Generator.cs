using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Net.Http;
using System.Data.SqlClient;
using System.Data;

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

        public async void Citanje(DateTime dt)
        {
            HttpClient hC = new HttpClient();
            HttpResponseMessage response= await hC.GetAsync("https://localhost:44312/api/values/" +dt.ToString());
           // List<ElementP> result = response.Content.ToString();
        }

        /*
        public void Proba() //Srediti konekciju sa SQL
        {
            SqlConnection con = new SqlConnection(@"Data Source=VLADICA-PC\SQLEXPRESS;Initial Catalog=NewDatbase;Integrated Security=True;");
            try
            {
                con.Open();
                for (int i = 0; i < elementPs.Length; i++)
                {
                    if (elementPs[i].NadjiSumu() > 15)
                    {
                        string sql = "INSERT INTO [ElementP] (IdentifikacioniKod,RedniBroj,DatumPretrage) values (@ID,@RB,@DP)";

                        SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.Parameters.Add("@ID", SqlDbType.VarChar);
                        cmd.Parameters["@ID"].Value = elementPs[i].IdentifikacioniKod;

                        cmd.Parameters.Add("@RB", SqlDbType.Int);
                        cmd.Parameters["@RB"].Value = elementPs[i].RedniBroj;

                        cmd.Parameters.Add("@DP", SqlDbType.Time);
                        cmd.Parameters["@DP"].Value = DateTime.Now.TimeOfDay;
                        
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Vidi bazu xD");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("prc " + ex.ToString());
            }
            con.Close();
        }
        */


        public async void Pretraga(int p, string output)
        {
           //TO DO
            //konekcija za bazu,slanje i primanje podataka u bazu,da se popravi get i post 
            
            if (output == "baza") 
            {
                Console.WriteLine("Smestanje u bazu.");
                HttpClient hC = new HttpClient();

                for (int i = 0; i < elementPs.Length; i++)
                {
                    if (elementPs[i].NadjiSumu() > p)
                    {
                        var elP=elementPs[i].ToJson();
                        HttpContent content = new StringContent(elP,Encoding.UTF8, "application/json");


                        await hC.PostAsync("https://localhost:44312/api/values", content);
                    }
                }
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
