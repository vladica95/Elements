using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elements;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;

namespace WebApi.Data
{
    public class DatabaseService : IDatabaseService
    {
        SqlCommand cmd;
        SqlConnection con;
        public DatabaseService()
        {
            con = new SqlConnection(@"Data Source=VLADICA-PC\SQLEXPRESS;Initial Catalog=NewDatbase;Integrated Security=True;");
        }

        public void AddData(ElementP elP)
        {

            Console.WriteLine("ADDDING DATA IN TO THE DATABASE >>> \n\n\n");
            try
            {
                con.Open();

                string sql = "INSERT INTO ElementP (IdentifikacioniKod,RedniBroj,DatumPretrage) values (@ID,@RB,@DP)";

                cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@ID", SqlDbType.VarChar);
                cmd.Parameters["@ID"].Value = elP.IdentifikacioniKod;

                cmd.Parameters.Add("@RB", SqlDbType.Int);
                cmd.Parameters["@RB"].Value = elP.RedniBroj;

                cmd.Parameters.Add("@DP", SqlDbType.Time);
                cmd.Parameters["@DP"].Value = DateTime.Now.TimeOfDay;

                cmd.ExecuteNonQuery();

                ElementC[] elementCs = elP.Elementi;

                foreach (var item in elementCs)
                {
                    sql = "INSERT INTO ElementC (IDKOD,Grupa,Vrednost) values (@IDK,@G,@V)";

                    cmd = new SqlCommand(sql, con);
                    cmd.Parameters.Add("@IDK", SqlDbType.VarChar);
                    cmd.Parameters["@IDK"].Value = elP.IdentifikacioniKod;

                    cmd.Parameters.Add("@G", SqlDbType.VarChar);
                    cmd.Parameters["@G"].Value = item.Grupa;

                    cmd.Parameters.Add("@V", SqlDbType.Int);
                    cmd.Parameters["@V"].Value = item.Vrednost;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                con.Close();

                Console.WriteLine("DONE WITH DATA ... ");
            }

        }
        public List<ElementP> GetData(string time)
        {
            List<ElementP> elementPs = new List<ElementP>();
            try
            {
                con.Open();
                DateTime dateTime = DateTime.Parse(time);
                dateTime = new DateTime(2000, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second);

                string sqlP = "SELECT IdentifikacioniKod,RedniBroj,DatumPretrage FROM ElementP";

                SqlCommand cmdP = new SqlCommand(sqlP, con);

                using (SqlDataReader oReader = cmdP.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        if (dateTime < DateTime.Parse(oReader["DatumPretrage"].ToString()))
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
                    cmdC.Parameters.AddWithValue("@IDenKod", dateTime);
                    using (SqlDataReader oReader = cmdC.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            ElementC ELC = new ElementC(oReader["Grupa"].ToString().ToCharArray()[0], Int32.Parse(oReader["Vrednost"].ToString()));
                            elementCs.Add(ELC);
                        }
                    }
                    item.AddCs(elementCs);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                con.Close();
            }
            return elementPs;
        }
    }
}
