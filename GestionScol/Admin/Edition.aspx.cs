using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionScol
{
    public partial class _Edit : Page
    {
        protected string userId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null) { Response.Redirect("~/Default.aspx"); }

            if (!IsPostBack)
            {
                string param1 = (string)Session["id_Edit"];
                userId = param1;
                BindData(param1);
            }
        }

        private void BindData(string id)
        {
            if (Session["Droit"].ToString() == "0")
                ChargerUtilisateur(id);

            if (Session["Droit"].ToString() == "1")
                ChargerIntervenant(id);
        }

        protected void ChargerUtilisateur(string id)
        {
            int utilisateurId = Int32.Parse(id);

            string query = "SELECT Nom, Prenom, Droit, AdresseMail, authentification.login, authentification.mdp FROM personne INNER JOIN authentification ON authentification.id = personne.id WHERE personne.id=@id";
            using (MySqlConnection conn = DataBase.getConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", utilisateurId);
                try
                {
                    DataTable resultTable = DataBase.GetData(cmd);

                    if (resultTable.Rows.Count > 0)
                    {
                        txtNom.Text = resultTable.Rows[0]["Nom"].ToString();
                        txtPrenom.Text = resultTable.Rows[0]["Prenom"].ToString();
                        txtDroit.Text = resultTable.Rows[0]["Droit"].ToString();
                        txtLogin.Text = resultTable.Rows[0]["login"].ToString();
                        txtMdp.Text = Crypto.Decrypt(resultTable.Rows[0]["mdp"].ToString());
                        txtAdresseMail.Text = resultTable.Rows[0]["AdresseMail"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
        }

        protected void ChargerIntervenant(string id)
        {
            int utilisateurId = Int32.Parse(id);
            mvListe.ActiveViewIndex = 1;

            string query = "SELECT Nom, Prenom, Droit, AdresseMail, authentification.login, authentification.mdp FROM personne INNER JOIN authentification ON authentification.id = personne.id WHERE personne.id=@id";
            using (MySqlConnection conn = DataBase.getConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", utilisateurId);
                try
                {
                    DataTable resultTable = DataBase.GetData(cmd);

                    if (resultTable.Rows.Count > 0)
                    {
                        txtNomIntervenant.Text = resultTable.Rows[0]["Nom"].ToString();
                        txtPrenomIntervenant.Text = resultTable.Rows[0]["Prenom"].ToString();
                        txtDroitIntervenant.Text = resultTable.Rows[0]["Droit"].ToString();
                        txtLoginIntervenant.Text = resultTable.Rows[0]["login"].ToString();
                        txtMdpIntervenant.Text = Crypto.Decrypt(resultTable.Rows[0]["mdp"].ToString());
                        txtAdresseMailIntervenant.Text = resultTable.Rows[0]["AdresseMail"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
        }

        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            int utilisateurId = Int32.Parse((string)Session["id_Edit"]);

            string queryUpdatePersonne = "UPDATE personne SET Droit=@Droit, Nom=@NOM, Prenom=@Prenom, AdresseMail=@AdresseMail WHERE id=@id";
            string queryUpdateAuthentification = "UPDATE authentification SET login=@login, mdp=@mdp WHERE id=@id";

            MySqlCommand mysqlCmd = new MySqlCommand(queryUpdatePersonne, DataBase.getConnection());
            MySqlCommand mysqlCmdUpdate = new MySqlCommand(queryUpdateAuthentification, DataBase.getConnection());

            mysqlCmd.Parameters.AddWithValue("@id", utilisateurId);
            mysqlCmd.Parameters.AddWithValue("@Droit", txtDroit.Text);
            mysqlCmd.Parameters.AddWithValue("@Nom", txtNom.Text);
            mysqlCmd.Parameters.AddWithValue("@Prenom", txtPrenom.Text);
            mysqlCmd.Parameters.AddWithValue("@AdresseMail", txtAdresseMail.Text);

            mysqlCmdUpdate.Parameters.AddWithValue("@id", utilisateurId);
            mysqlCmdUpdate.Parameters.AddWithValue("@login", txtLogin.Text);
            mysqlCmdUpdate.Parameters.AddWithValue("@mdp", Crypto.Encrypt(txtMdp.Text));

            try
            {
                DataBase.Execute(mysqlCmd);
                DataBase.Execute(mysqlCmdUpdate);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
            finally
            {
                mysqlCmdUpdate.Connection.Close();
                mysqlCmd.Connection.Close();
            }

            Response.Redirect("AccueilAdmin.aspx");
        }

        protected void btnIntervenantEnregistrer_Click(object sender, EventArgs e)
        {
            int utilisateurId = Int32.Parse((string)Session["id_Edit"]);

            string queryUpdatePersonne = "UPDATE personne SET Droit=@Droit, Nom=@NOM, Prenom=@Prenom, AdresseMail=@AdresseMail WHERE id=@id";
            string queryUpdateAuthentification = "UPDATE authentification SET login=@login, mdp=@mdp WHERE id=@id";

            MySqlCommand mysqlCmd = new MySqlCommand(queryUpdatePersonne, DataBase.getConnection());
            MySqlCommand mysqlCmdUpdate = new MySqlCommand(queryUpdateAuthentification, DataBase.getConnection());

            mysqlCmd.Parameters.AddWithValue("@id", utilisateurId);
            mysqlCmd.Parameters.AddWithValue("@Droit", txtDroitIntervenant.Text);
            mysqlCmd.Parameters.AddWithValue("@Nom", txtNomIntervenant.Text);
            mysqlCmd.Parameters.AddWithValue("@Prenom", txtPrenomIntervenant.Text);
            mysqlCmd.Parameters.AddWithValue("@AdresseMail", txtAdresseMailIntervenant.Text);

            mysqlCmdUpdate.Parameters.AddWithValue("@id", utilisateurId);
            mysqlCmdUpdate.Parameters.AddWithValue("@login", txtLoginIntervenant.Text);
            mysqlCmdUpdate.Parameters.AddWithValue("@mdp", Crypto.Encrypt(txtMdpIntervenant.Text));

            try
            {
                DataBase.Execute(mysqlCmd);
                DataBase.Execute(mysqlCmdUpdate);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
            finally
            {
                mysqlCmdUpdate.Connection.Close();
                mysqlCmd.Connection.Close();
            }

            Response.Redirect("AccueilAdmin.aspx");
        }


    }
}
