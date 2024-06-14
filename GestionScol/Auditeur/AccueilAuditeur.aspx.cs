using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionScol
{
    public partial class _Auditeur : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null) { Response.Redirect("~/Default.aspx"); }

            if (!IsPostBack)
            {
                try
                {
                    BindData();
                }
                catch (Exception ex)
                {
                    lblError.Text = "Erreur : " + ex.Message;
                }
            }
        }
        private void BindData()
        {
            string query = "SELECT date, heure_debut, heure_fin, enseignement.libelle FROM planning INNER JOIN enseignement ON id_enseignement = enseignement.id WHERE planning.id_personne = @id";

            MySqlCommand mysqlCmd = new MySqlCommand(query, DataBase.getConnection());

            mysqlCmd.Parameters.AddWithValue("@id", Session["id"]);

            try
            {
                DataTable resultTable = DataBase.GetData(mysqlCmd);

                rptrListeMaj.DataSource = resultTable;
                rptrListeMaj.DataBind();

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