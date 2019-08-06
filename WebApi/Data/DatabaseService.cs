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
            Console.WriteLine("Provera dal radi xD");
            con.Open();
          
            string sql = "INSERT INTO ElementP (IdentifikacioniKod,RedniBroj,DatumPretrage) values (@ID,@RB,@DP)";
           
            cmd= new SqlCommand(sql, con);
            cmd.Parameters.Add("@ID", SqlDbType.VarChar);
            cmd.Parameters["@ID"].Value = elP.IdentifikacioniKod;

            cmd.Parameters.Add("@RB", SqlDbType.Int);
            cmd.Parameters["@RB"].Value = elP.RedniBroj;

            cmd.Parameters.Add("@DP", SqlDbType.Time);
            cmd.Parameters["@DP"].Value = DateTime.Now.TimeOfDay;

            cmd.ExecuteNonQuery();

            var str = elP.ToJson();

            var jObject = JObject.Parse(str);

            JArray elementCs = (JArray)jObject["ElementC"];
            if (elementCs != null)
            {
                foreach (var item in elementCs)
                {
                    sql = "INSERT INTO ElementC (IDKOD,Grupa,Vrednost) values (@IDK,@G,@V)";

                    cmd = new SqlCommand(sql, con);
                    cmd.Parameters.Add("@IDK", SqlDbType.VarChar);
                    cmd.Parameters["@IDK"].Value = elP.IdentifikacioniKod;

                    cmd.Parameters.Add("@G", SqlDbType.VarChar);
                    cmd.Parameters["@G"].Value = (char)item["Grupa"];

                    cmd.Parameters.Add("@V", SqlDbType.Int);
                    cmd.Parameters["@V"].Value = (int)item["Vrednost"];

                    cmd.ExecuteNonQuery();
                }
            }
            con.Close();
        }
        public List<ElementP> GetData(DateTime dateTime)
        {
            return null;
        }
    }
}
