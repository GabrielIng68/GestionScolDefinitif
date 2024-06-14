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
    public partial class _Ajout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null) { Response.Redirect("~/Default.aspx"); }

            if (!IsPostBack)
            {
                if (Session["Droit"].ToString() == "1")
                    mvListe.ActiveViewIndex = 1;
            }
        }


        protected void btnEnregistrer_Click(object sender, EventArgs e)
        {
            string queryInsertPersonne = "INSERT INTO personne (id, Droit, Nom, Prenom, AdresseMail) VALUES (@id, @Droit, @Nom, @Prenom, AdresseMail)";
            string queryInsertAuthentification = "INSERT INTO authentification (login, mdp) VALUES (@login, @mdp)";
            string querySelectAuthentification = "SELECT id FROM authentification WHERE login=@login AND mdp=@mdp";

            MySqlCommand mysqlCmd = new MySqlCommand(queryInsertPersonne, DataBase.getConnection());
            MySqlCommand mysqlCmdInsert = new MySqlCommand(queryInsertAuthentification, DataBase.getConnection());
            MySqlCommand mysqlCmdSelect = new MySqlCommand(querySelectAuthentification, DataBase.getConnection());


            mysqlCmd.Parameters.AddWithValue("@Droit", 0);
            mysqlCmd.Parameters.AddWithValue("@Nom", txtNom.Text);
            mysqlCmd.Parameters.AddWithValue("@Prenom", txtPrenom.Text);
            mysqlCmd.Parameters.AddWithValue("@AdresseMail", txtAdresseMail.Text);

            mysqlCmdInsert.Parameters.AddWithValue("@login", txtLogin.Text);
            mysqlCmdInsert.Parameters.AddWithValue("@mdp", txtMdp.Text);

            mysqlCmdSelect.Parameters.AddWithValue("@mdp", txtMdp.Text);
            mysqlCmdSelect.Parameters.AddWithValue("@login", txtLogin.Text);

            try
            {
                DataBase.Execute(mysqlCmdInsert);
                try
                {
                    DataTable resultTable = DataBase.GetData(mysqlCmdSelect);

                    if (resultTable.Rows.Count > 0)
                    {
                        mysqlCmd.Parameters.AddWithValue("@id", resultTable.Rows[0]["id"].ToString());
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
                finally
                {
                    mysqlCmd.Connection.Close();
                }
                DataBase.Execute(mysqlCmd);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
            finally
            {
                mysqlCmdInsert.Connection.Close();
                mysqlCmd.Connection.Close();
            }

            Response.Redirect("AccueilAdmin.aspx");
        }

        protected void btnEnregistrerIntervenant_Click(object sender, EventArgs e)
        {
            string queryInsertPersonne = "INSERT INTO personne (id, Droit, Nom, Prenom, AdresseMail) VALUES (@id, @Droit, @Nom, @Prenom, @AdresseMail)";
            string queryInsertAuthentification = "INSERT INTO authentification (login, mdp) VALUES (@login, @mdp)";
            string querySelectAuthentification = "SELECT id FROM authentification WHERE login=@login AND mdp=@mdp";

            MySqlCommand mysqlCmd = new MySqlCommand(queryInsertPersonne, DataBase.getConnection());
            MySqlCommand mysqlCmdInsert = new MySqlCommand(queryInsertAuthentification, DataBase.getConnection());
            MySqlCommand mysqlCmdSelect = new MySqlCommand(querySelectAuthentification, DataBase.getConnection());


            mysqlCmd.Parameters.AddWithValue("@Droit", 1);
            mysqlCmd.Parameters.AddWithValue("@Nom", txtNomIntervenant.Text);
            mysqlCmd.Parameters.AddWithValue("@Prenom", txtPrenomIntervenant.Text);
            mysqlCmd.Parameters.AddWithValue("@Prenom", txtAdresseMailIntervenant.Text);

            mysqlCmdInsert.Parameters.AddWithValue("@login", txtLoginIntervenant.Text);
            mysqlCmdInsert.Parameters.AddWithValue("@mdp", txtMdpIntervenant.Text);

            mysqlCmdSelect.Parameters.AddWithValue("@mdp", txtMdpIntervenant.Text);
            mysqlCmdSelect.Parameters.AddWithValue("@login", txtLoginIntervenant.Text);

            try
            {
                DataBase.Execute(mysqlCmdInsert);
                try
                {
                    DataTable resultTable = DataBase.GetData(mysqlCmdSelect);

                    if (resultTable.Rows.Count > 0)
                    {
                        mysqlCmd.Parameters.AddWithValue("@id", resultTable.Rows[0]["id"].ToString());
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
                finally
                {
                    mysqlCmd.Connection.Close();
                }
                DataBase.Execute(mysqlCmd);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
            finally
            {
                mysqlCmdInsert.Connection.Close();
                mysqlCmd.Connection.Close();
            }

            Response.Redirect("AccueilAdmin.aspx");
        }


    }
}
