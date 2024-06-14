using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web.UI;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace GestionScol
{
    public partial class _Default : Page
    {
        
        protected int code = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] != null)
            {
                string queryPersonne = "SELECT Droit FROM personne WHERE id=@id";

                MySqlCommand mysqlCmd = new MySqlCommand(queryPersonne, DataBase.getConnection());

                mysqlCmd.Parameters.AddWithValue("@id", Session["id"]);

                DataTable dt = new DataTable();

                try
                {
                    DataTable resultTable = DataBase.GetData(mysqlCmd);

                    foreach (DataRow row in resultTable.Rows)
                    {
                        if(row["Droit"].ToString() == "0")
                        {
                            Response.Redirect("Auditeur/AcceuilAuditeur.aspx");
                        }
                        else if(row["Droit"].ToString() == "1")
                        {
                            Response.Redirect("Intervenant/AccueilIntervenant.aspx");
                        }
                        else if (row["Droit"].ToString() == "3")
                        {
                            Response.Redirect("Admin/AccueilAdmin.aspx");
                        }
                    }

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Erreur MySQL : " + ex.Message);
                }
                finally
                {
                    mysqlCmd.Connection.Close();
                }
            }

            Random random = new Random();
            code = random.Next(100000, 999999);
    }


        protected void btnConnecter_Click(object sender, EventArgs e)
        {
            string login = tbtLogin.Text;
            string mdp = tbtMDP.Text;
            int verif = -1;

            string query = "SELECT * FROM authentification WHERE login=@login";
            string queryPersonne = "SELECT * FROM personne WHERE id=@id";

            MySqlCommand mysqlCmd = new MySqlCommand(query, DataBase.getConnection());
            MySqlCommand mysqlCmdPersonne = new MySqlCommand(queryPersonne, DataBase.getConnection());

            mysqlCmd.Parameters.AddWithValue("@login", login);

            DataTable dt = new DataTable();

            try
            {
                DataTable resultTable = DataBase.GetData(mysqlCmd);

                foreach (DataRow row in resultTable.Rows)
                {
                    string verifMdp = row["mdp"].ToString();
                    if (mdp == Crypto.Decrypt(verifMdp))
                    {
                        mysqlCmdPersonne.Parameters.AddWithValue("@id", Int32.Parse(row["id"].ToString()));
                        Session["id"] = Int32.Parse(row["id"].ToString());
                        break;
                    }
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur MySQL : " + ex.Message);
            }
            finally
            {
                mysqlCmd.Connection.Close();
            }

            try
            {
                DataTable resultTable = DataBase.GetData(mysqlCmdPersonne);

                foreach (DataRow row in resultTable.Rows)
                {
                    verif = Int32.Parse(row["Droit"].ToString());
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur MySQL : " + ex.Message);
            }
            finally
            {
                mysqlCmd.Connection.Close();
                if (verif == 0) // droit unique pour les auditeur
                    Response.Redirect("Auditeur/AccueilAuditeur.aspx");
                if (verif == 1) // droit unique pour les intervants
                    Response.Redirect("Intervenant/AccueilIntervenant.aspx");
                if (verif == 3) // droit admin
                    Response.Redirect("Admin/AccueilAdmin.aspx");
                else
                    lblErreurDefault.Visible = true;
            }
        }

        protected void btnInscrire_Click(object sender, EventArgs e)
        {
            byte[] key = new byte[32]; // 256 bits
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }

            string base64Key = Convert.ToBase64String(key);
            Console.WriteLine(base64Key);

            Response.Redirect("Inscription.aspx");
        }

        protected void btnOublier_Click(object sender, EventArgs e)
        {
            string login = tbtLogin.Text;
            string AdresseMail = "";

            

            string query = "SELECT id FROM authentification WHERE login=@login";
            string queryPersonne = "SELECT AdresseMail FROM personne WHERE id=@id";

            MySqlCommand mysqlCmd = new MySqlCommand(query, DataBase.getConnection());
            MySqlCommand mysqlCmdPersonne = new MySqlCommand(queryPersonne, DataBase.getConnection());

            mysqlCmd.Parameters.AddWithValue("@login", login);

            DataTable dt = new DataTable();

            try
            {
                DataTable resultTable = DataBase.GetData(mysqlCmd);

                foreach (DataRow row in resultTable.Rows)
                {
                    mysqlCmdPersonne.Parameters.AddWithValue("@id", Int32.Parse(row["id"].ToString()));
                    Session["id_nvMDP"] = Int32.Parse(row["id"].ToString());
                    break;

                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur MySQL : " + ex.Message);
            }
            finally
            {
                mysqlCmd.Connection.Close();
            }

            try
            {
                DataTable resultTable = DataBase.GetData(mysqlCmdPersonne);

                foreach (DataRow row in resultTable.Rows)
                {
                    AdresseMail = row["AdresseMail"].ToString();
                    break;
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur MySQL : " + ex.Message);
            }
            finally
            {
                mysqlCmd.Connection.Close();
            }

            try
            {
                SendEmail(AdresseMail, code.ToString());
                valid.Text = "Mail envoyé";
                valid.Visible = true;
                pnlCode.Visible = true;
            }
            catch (Exception ex)
            {
                error.Text = "Error sending email: " + ex.Message;
                error.Visible = true;
            }
        }

        private void SendEmail(string email, string code)
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("gabriel.hauss10@gmail.com", "GestionScol");
            var subject = "Code MDP";
            var to = new EmailAddress(email);
            var plainTextContent = $"Code pour changer le mot de passe : {code}";
            var htmlContent = $"<strong>Code pour changer le mot de passe : {code}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var reponse = client.SendEmailAsync(msg).Result;

            // Log the response status code and body for debugging
            Console.WriteLine(reponse.StatusCode);
            Console.WriteLine(reponse.Body.ReadAsStringAsync().Result);

            if (reponse.StatusCode != System.Net.HttpStatusCode.OK && reponse.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new Exception($"Failed to send email. Status code: {reponse.StatusCode}");
            }
        }

        protected void btnCode_Click(object sender, EventArgs e)
        {
            if(code.ToString() == txtCode.Text)
            {
                Response.Redirect("ChangerMotDePasse.aspx");
            }
            else
            {
                lblErreur.Text = "Mot de passe invalid";
                lblErreur.Visible = true;
            }
        }
    }
}