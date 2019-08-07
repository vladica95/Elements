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
using System.Net.Http;
using System.Net.Http.Headers;

using Newtonsoft.Json;

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
            Console.WriteLine("Stampanje posle generisanja elemenata.");
        }

        public ElementP IDSearch(string ID)
        {
            ElementP el = new ElementP();
            foreach (var v in elementPs)
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

        public async void Citanje(string dt)
        {
            HttpClient hC = new HttpClient();
            HttpResponseMessage response = await hC.GetAsync("https://localhost:5000/api/values" + dt);
            // List<ElementP> result = response.Content.ToString();
        }
        public void ProbaZaCitanje(string s)
        {
            List<ElementP> elementPs = new List<ElementP>();
            SqlConnection con = new SqlConnection(@"Data Source=VLADICA-PC\SQLEXPRESS;Initial Catalog=NewDatbase;Integrated Security=True;");

            try
            {
                con.Open();
                DateTime dateTime = DateTime.Parse(s);
                dateTime = new DateTime(2000, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second);

                string sqlP = "SELECT IdentifikacioniKod,RedniBroj,DatumPretrage FROM ElementP";

                SqlCommand cmdP = new SqlCommand(sqlP, con);

                using (SqlDataReader oReader = cmdP.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        if (dateTime <DateTime.Parse(oReader["DatumPretrage"].ToString()))
                            {
                            ElementP ELP = new ElementP();
                            ELP.IdentifikacioniKod = oReader["IdentifikacioniKod"].ToString();
                            ELP.RedniBroj = Int32.Parse(oReader["RedniBroj"].ToString());
                            elementPs.Add(ELP);
                            Console.WriteLine("Citanje Podataka P");
                        }
                    }
                }

                foreach (var item in elementPs)
                {
                    List<ElementC> elementCs = new List<ElementC>();
                    string IDenKod = item.IdentifikacioniKod;
                    string sqlC = "SELECT Grupa,Vrednost FROM ElementC WHERE IDKOD=@IDenKod";
                    SqlCommand cmdC = new SqlCommand(sqlC, con);
                    cmdC.Parameters.AddWithValue("@IDenKod", IDenKod);
                    using (SqlDataReader oReader = cmdC.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            ElementC ELC = new ElementC(oReader["Grupa"].ToString().ToCharArray()[0], Int32.Parse(oReader["Vrednost"].ToString()));

                            elementCs.Add(ELC);
                            Console.WriteLine("Citanje Podataka C");
                        }
                    }
                    item.AddCs(elementCs);
                }
                foreach (var item in elementPs)
                {
                    item.Stampaj();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                con.Close();
            }
        }

        public void ProbaZaUpis()
        {
            SqlConnection con = new SqlConnection(@"Data Source=VLADICA-PC\SQLEXPRESS;Initial Catalog=NewDatbase;Integrated Security=True;");

            try
            {
                con.Open();
                for (int i = 0; i < elementPs.Length; i++)
                {
                    if (elementPs[i].NadjiSumu() > 20)
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
                        Console.WriteLine("Vidi bazu tabela P xD");
                       // ElementC[] elementCs = elP.Elementi;

                        foreach (var item in elementPs[i].Elementi)
                        {
                            sql = "INSERT INTO ElementC (IDKOD,Grupa,Vrednost) values (@IDK,@G,@V)";

                            cmd = new SqlCommand(sql, con);
                            cmd.Parameters.Add("@IDK", SqlDbType.VarChar);
                            cmd.Parameters["@IDK"].Value = elementPs[i].IdentifikacioniKod;

                            cmd.Parameters.Add("@G", SqlDbType.VarChar);
                            cmd.Parameters["@G"].Value = item.Grupa;

                            cmd.Parameters.Add("@V", SqlDbType.Int);
                            cmd.Parameters["@V"].Value = item.Vrednost;

                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Vidi bazu tabela C xD");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            con.Close();
        }

        public async void Pretraga(int p, string output)
        {
            //TO DO
            //da se popravi get i post 

            if (output == "baza")
            {
                Console.WriteLine("Smestanje u bazu.");
                for (int i = 0; i < elementPs.Length; i++)
                {
                    if (elementPs[i].NadjiSumu() > p)
                    {
                        var elP = elementPs[i].ToJson();

                        using (var client = new HttpClient())
                        {
                            var response = await client.PostAsync(
                                "http://localhost:5000/api/values",
                                 new StringContent(elP, Encoding.UTF8, "application/json"));
                        }

                        Console.WriteLine("Salje se preko api");
                    }
                }
            }
            else if (output == "fajl")
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
                        path = fbd.SelectedPath.ToString() + "\\searchRes.json";
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
