using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionScol
{
    public partial class _AjoutAuditeur : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static void btnSelectionnerDoc_Click(string DOCNUM, string DOCTIT, string ID)
        {
            string queryInsert = "INSERT INTO planning (date, heure_deb, heure_fin, id_enseignement, id_personne) VALUES (@date, @heuredeb, @heurefin, @iden, @idpe)";
            string query = "SELECT * FROM planning WHERE id_enseignement=@id";

            MySqlCommand mysqlCmd = new MySqlCommand(queryInsert, DataBase.getConnection());
            MySqlCommand mysqlCmdSelect = new MySqlCommand(query, DataBase.getConnection());

            mysqlCmdSelect.Parameters.AddWithValue("@id", Int32.Parse(ID));

            string date = "", heuredeb = "", heurefin = "";
            try
            {
                DataTable resultTable = DataBase.GetData(mysqlCmdSelect);

                date = resultTable.Rows[0]["date"].ToString();
                heuredeb = resultTable.Rows[0]["heure_debut"].ToString();
                heurefin = resultTable.Rows[0]["heure_fin"].ToString();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
            finally
            {
                mysqlCmdSelect.Connection.Close();
            }

            mysqlCmd.Parameters.AddWithValue("@date", DateTime.Parse(date));
            mysqlCmd.Parameters.AddWithValue("@heuredeb", Int32.Parse(heuredeb));
            mysqlCmd.Parameters.AddWithValue("@heurefin", Int32.Parse(heurefin));
            mysqlCmd.Parameters.AddWithValue("@iden", Int32.Parse(ID));
            mysqlCmd.Parameters.AddWithValue("@idpe", Int32.Parse(DOCNUM));

            try
            {
                DataBase.Execute(mysqlCmd);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
            finally
            {
                mysqlCmd.Connection.Close();
            }
        }
    }
}
