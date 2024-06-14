using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Web;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Mail;

/// <summary>
/// Description résumée de Mail
/// </summary>


public class Mail
{

    string m_szSmtpServer = "smtp.gmail.com";
    int m_nSmtpPort = 587;

    private string m_szBody;
    private bool m_bHTML;

    private string m_nUtilisateurEnvoiMail = "";
    private string m_nUtilisateurExpediteur = "";
    private string m_nUtilisateurDestinataire = "";


    private string m_szMailSecours = "gabriel.hauss@lecnam.net";

    private SmtpClient smtpClient = null;


    public Mail()
	{
        // Configuration du client SMTP
        smtpClient = new SmtpClient();
        smtpClient.Host = m_szSmtpServer;
        smtpClient.Port = m_nSmtpPort;
        smtpClient.Credentials = new NetworkCredential("gabriel.hauss10@gmail.com", "gabriel2003");
        smtpClient.EnableSsl = true;
    }

    public string UtilisateurEnvoiMail
    {
        get
        {
            return m_nUtilisateurEnvoiMail;
        }
        set
        {
            m_nUtilisateurEnvoiMail = value;
        }
    }
    public void setExpediteur(string exp)
    {
        m_nUtilisateurExpediteur = exp;
    }    

    public void addDestinataire(string dest)
    {
        m_nUtilisateurDestinataire = dest;
    }


    public void setCorps(string corps)
    {
        m_szBody = corps;
    }
		
    public void setHtmlBody(bool value)
    {
        m_bHTML = value;
    }
    
    public void envoyerMail(MailMessage mail)
    {
        smtpClient.Send(mail);
    }
}
