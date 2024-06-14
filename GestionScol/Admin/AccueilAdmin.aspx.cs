using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestionScol
{
    public partial class _Admin : Page
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
            chargerIntervenant();
            chargerAuditeur();
        }
        protected void rptrListeIntervenant_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Ajout")
            {
                string id = e.CommandArgument.ToString();
                if (id == "I")
                    AjouterIntervenant();
            }
            if (e.CommandName == "Edit")
            {
                string id = e.CommandArgument.ToString();
                Session["id_Edit"] = id;
                Session["Droit"] = 1;
                Response.Redirect("Edition.aspx");

            }
            if (e.CommandName == "Supp")
            {
                string id = e.CommandArgument.ToString();
                supprimerIntervenant(id);
                lblReussi.Text = "Suppression réussi !";
                lblReussi.Visible = true;
                lblErreurAuditeur.Visible = false;
            }
        }

        protected void rptrListeAuditeur_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Ajout")
            {
                string id = e.CommandArgument.ToString();

                if (id == "A")
                    AjouterAuditeur();

            }
            if (e.CommandName == "Edit")
            {
                string id = e.CommandArgument.ToString();
                Session["id_Edit"] = id;
                Session["Droit"] = 0;
                Response.Redirect("Edition.aspx");

            }
            if (e.CommandName == "Supp")
            {
                string id = e.CommandArgument.ToString();
                supprimerAuditeur(id);
                lblReussi.Text = "Suppression réussi !";
                lblReussi.Visible = true;
                lblErreurAuditeur.Visible = false;
            }
        }

        protected void AjouterAuditeur()
        {
            Session["Droit"] = 0;
            Response.Redirect("Ajout.aspx");
        }

        protected void AjouterIntervenant()
        {
            Session["Droit"] = 1;
            Response.Redirect("Ajout.aspx");
        }

        protected void chargerIntervenant()
        {
            string query = "SELECT id, Nom, Prenom FROM personne WHERE Droit=1";

            MySqlCommand mysqlCmd = new MySqlCommand(query, DataBase.getConnection());

            try
            {
                DataTable resultTable = DataBase.GetData(mysqlCmd);

                rptrListeIntervenant.DataSource = resultTable;
                rptrListeIntervenant.DataBind();

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

        protected void chargerAuditeur()
        {
            string query = "SELECT id, Nom, Prenom FROM personne WHERE Droit=0";

            MySqlCommand mysqlCmd = new MySqlCommand(query, DataBase.getConnection());

            try
            {
                DataTable resultTable = DataBase.GetData(mysqlCmd);

                rptrListeAuditeur.DataSource = resultTable;
                rptrListeAuditeur.DataBind();

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

        protected void supprimerAuditeur(string id)
        {
            string query = "DELETE * FROM personne WHERE id=@id";
            using (MySqlConnection conn = DataBase.getConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", Int32.Parse(id));
                try
                {
                    DataBase.Execute(cmd);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                    lblErreurAuditeur.Text = ex.Message;
                    lblErreurAuditeur.Visible = true;
                    lblReussi.Visible = false;
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
        }

        protected void supprimerIntervenant(string id)
        {
            string query = "DELETE * FROM personne WHERE id=@id";
            using (MySqlConnection conn = DataBase.getConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", Int32.Parse(id));
                try
                {
                    DataBase.Execute(cmd);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur : " + ex.Message);
                    lblErreurAuditeur.Text = ex.Message;
                    lblErreurAuditeur.Visible = true;
                    lblReussi.Visible = false;
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
        }
    }
}
