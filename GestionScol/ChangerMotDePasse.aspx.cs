using MySql.Data.MySqlClient;
using System;
using System.Web.UI;

namespace GestionScol
{
    public partial class _ChangerMDP : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            if (txtNouveauMDP == txtNouveauMDPConfirm)
            {
                string nvMDP = Crypto.Encrypt(txtNouveauMDP.Text);

                string queryUpdateAuthentification = "UPDATE authentification SET mdp=@mdp WHERE id=@id";

                MySqlCommand mysqlCmd = new MySqlCommand(queryUpdateAuthentification, DataBase.getConnection());

                mysqlCmd.Parameters.AddWithValue("@id", Session["id_nvMDP"].ToString());

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

                Response.Redirect("Default.aspx");
            }
        }
    }
}