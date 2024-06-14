using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web.UI;

namespace GestionScol
{
    public partial class _Inscription : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnInscrire_Click(object sender, EventArgs e)
        {
            string login = tbtInscriptionLogin.Text;
            string mdp = tbtInscriptionMDP.Text;
            string prenom = tbtPrenom.Text;
            string nom = tbtNom.Text;
            string mail = tbtMail.Text;
            int droit = 0;
            string encryptedPassword = Crypto.Encrypt(mdp);
            bool verif = true;

            string queryInsert = "INSERT INTO authentification (login, mdp) VALUES (@login, @password)";
            string queryInsertPersonne = "INSERT INTO personne (Droit, id, Nom, Prenom, AdresseMail) VALUES (@droit, @id, @nom, @prenom, @AdresseMail)";
            string query = "SELECT * FROM authentification WHERE login=@login AND mdp=@mdp";

            MySqlCommand mysqlCmd = new MySqlCommand(queryInsert, DataBase.getConnection());
            MySqlCommand mysqlCmdSelect = new MySqlCommand(query, DataBase.getConnection());
            MySqlCommand mysqlCmdInsertPersonne = new MySqlCommand(queryInsertPersonne, DataBase.getConnection());

            mysqlCmd.Parameters.AddWithValue("@login", login);
            mysqlCmd.Parameters.AddWithValue("@password", encryptedPassword);

            mysqlCmdSelect.Parameters.AddWithValue("@login", login);
            mysqlCmdSelect.Parameters.AddWithValue("@mdp", encryptedPassword);

            mysqlCmdInsertPersonne.Parameters.AddWithValue("@Droit", droit);
            mysqlCmdInsertPersonne.Parameters.AddWithValue("@nom", nom);
            mysqlCmdInsertPersonne.Parameters.AddWithValue("@prenom", prenom);
            mysqlCmdInsertPersonne.Parameters.AddWithValue("@AdresseMail", mail);

            try
            {
                DataTable resultTable = DataBase.GetData(mysqlCmdSelect);

                if (resultTable.Rows.Count > 0)
                {
                    lblErreur.Visible = true;
                    verif = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
            finally
            {
                mysqlCmdSelect.Connection.Close();
            }

            if (verif)
                try
                {
                    DataBase.Execute(mysqlCmd);

                    try
                    {
                        DataTable resultTable = DataBase.GetData(mysqlCmdSelect);

                        foreach (DataRow row in resultTable.Rows)
                        {
                            string value = row["id"].ToString();
                            Session["id"] = Int32.Parse(value);
                        }

                        mysqlCmdInsertPersonne.Parameters.AddWithValue("@id", Session["id"]);

                        DataBase.Execute(mysqlCmdInsertPersonne);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erreur : " + ex.Message);
                    }
                    finally
                    {
                        mysqlCmdSelect.Connection.Close();
                        mysqlCmdInsertPersonne.Connection.Close();
                    }



                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Erreur MySQL : " + ex.Message);
                }
                finally
                {
                    mysqlCmd.Connection.Close();

                    Response.Redirect("Auditeur/AccueilAuditeur.aspx");
                }
        }
    }
}