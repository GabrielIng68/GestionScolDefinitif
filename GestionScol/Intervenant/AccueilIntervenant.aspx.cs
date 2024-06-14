using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionScol
{
    public partial class _Intervenant : Page
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
            string query = "SELECT id, libelle FROM enseignement WHERE id_personne=@id";

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

        protected void rptrListeMaj_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            
        }

        protected void rptrListeMaj_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                string id = e.CommandArgument.ToString();

                mvListe.ActiveViewIndex = 1;
                BindGridViewData(id);
            }
        }

        private void BindGridViewData(string id)
        {
            string query = "SELECT id, libelle FROM enseignement WHERE id=@id";
            using (MySqlConnection conn = DataBase.getConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", Int32.Parse(id));
                try
                {
                    DataTable resultTable = DataBase.GetData(cmd);

                    lstEnseignementsEdite.DataSource = resultTable;
                    lstEnseignementsEdite.DataBind();

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

            string queryPersonne = "SELECT id, nom, prenom FROM personne WHERE Droit=0 AND id IN ( SELECT id_personne FROM planning WHERE id_enseignement =@id);";
            using (MySqlConnection conn = DataBase.getConnection())
            {
                MySqlCommand cmdPersonne = new MySqlCommand(queryPersonne, conn);
                cmdPersonne.Parameters.AddWithValue("@id", Int32.Parse(id));
                try
                {
                    DataTable resultTable = DataBase.GetData(cmdPersonne);

                    lstPersonneEdite.DataSource = resultTable;
                    lstPersonneEdite.DataBind();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
                finally
                {
                    cmdPersonne.Connection.Close();
                }
            }

            hidden.Value = id;

            string queryPersonneSelect = "SELECT id, nom FROM personne WHERE Droit=0 AND id NOT IN ( SELECT id_personne FROM planning WHERE id_enseignement =@id);";
            using (MySqlConnection conn = DataBase.getConnection())
            {
                MySqlCommand cmdPersonneSelect = new MySqlCommand(queryPersonneSelect, conn);
                cmdPersonneSelect.Parameters.AddWithValue("@id", Int32.Parse(id));
                try
                {
                    DataTable resultTable = DataBase.GetData(cmdPersonneSelect);

                    lbDocPersonneActive.DataSource = resultTable;
                    lbDocPersonneActive.DataBind();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
                finally
                {
                    cmdPersonneSelect.Connection.Close();
                }
            }
            enregistrerPersonne(id);
        }

        protected void enregistrerPersonne(string id)
        {
            string queryPersonne = "SELECT id, nom, prenom FROM personne WHERE Droit=0 AND id IN ( SELECT id_personne FROM planning WHERE id_enseignement =@id);";
            using (MySqlConnection conn = DataBase.getConnection())
            {
                MySqlCommand cmdPersonne = new MySqlCommand(queryPersonne, conn);
                cmdPersonne.Parameters.AddWithValue("@id", Int32.Parse(id));
                try
                {
                    DataTable resultTable = DataBase.GetData(cmdPersonne);

                    lbPersonneInactive.DataSource = resultTable;
                    lbPersonneInactive.DataBind();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                }
                finally
                {
                    cmdPersonne.Connection.Close();
                }
            }
        }

        protected void lstEnseignements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Supp")
            {
                string id = e.CommandArgument.ToString();
                DeleteEnseignement(id);
            }
        }

        protected void lstEnseignements_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Ajouter du code pour manipuler les données de chaque ligne si nécessaire
        }

        private void DeleteEnseignement(string id)
        {
            string query = "DELETE FROM planning WHERE id = @id";
            using (MySqlConnection conn = DataBase.getConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    //BindRepeaterData(); // Mettre à jour les données après suppression
                }
                catch (Exception ex)
                {
                    lblError.Text = "Erreur : " + ex.Message;
                    lblError.Visible = true;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        protected void lstPersonne_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Supp")
            {
                string id = e.CommandArgument.ToString();
                DeleteEnseignement(id);
            }
        }

        protected void lstPersonne_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Ajouter du code pour manipuler les données de chaque ligne si nécessaire
        }
    }
}
