using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Description résumée de Data
/// </summary>
public class DataBase
{
    private static List<SqlConnection> m_arSQL = new List<SqlConnection>();
    string dt = "Server=localhost;Database=gestionscol;Uid=Gabriel;Pwd=essai;";
    private static string szDefaultConnection = "Server=localhost;Database=gestionscol;Uid=Gabriel;Pwd=essai;";
    private static bool m_bCheckConnection = false;

    private const int m_nMaxNumberRow = 500000;
    private const long m_nMaxMemoryUsage = 10720641024;

    class DataBaseConnectionInfo
    {
        public string szDataSource = "";
        public string szCatalog = "";
        public string szUserId = "";
        public string szPassword = "";

        public DataBaseConnectionInfo(string szConnectionString)
        {
            string[] arParameters = szConnectionString.Split(';');

            foreach (string szParam in arParameters)
            {
                string[] arTmp = szParam.Split('=');

                switch (arTmp[0])
                {
                    case "Data Source": szDataSource = arTmp[1]; break;
                    case "Initial Catalog": szCatalog = arTmp[1]; break;
                    case "User ID": szUserId = arTmp[1]; break;
                    case "Password": szPassword = arTmp[1]; break;
                };
            }
        }
    }

    public static string getConnectionString()
    {
        if (szDefaultConnection == null) throw new Exception("Chaîne de connexion non paramétrée");

        return szDefaultConnection;
    }

    public static MySqlConnection getConnection()
    {
        if (szDefaultConnection == null) throw new Exception("Chaîne de connexion non paramétrée");

        MySqlConnection mysqlcon = getConnection(szDefaultConnection);

        return mysqlcon;
    }

    public static MySqlConnection getConnection(string szName)
    {
        MySqlConnection mysqlcon = new MySqlConnection();

        try
        {
            mysqlcon.ConnectionString = szDefaultConnection;
        }
        catch { }

        if (m_bCheckConnection)
        {
            CheckConnection(); // Méthode non définie dans votre exemple
        }

        return mysqlcon;
    }

    public static DataTable GetData(MySqlCommand command)
    {
        MySqlDataAdapter da = new MySqlDataAdapter(command);
        DataTable dt = new DataTable();

        command.Connection.Open();

        try
        {
            da.Fill(dt);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            da.Dispose();
        }

        return dt;
    }


    public static object getValue(SqlCommand com)
    {
        com.Connection.Open();

        object o = null;

        try
        {
            o = com.ExecuteScalar();
            com.Connection.Close();
        }
        catch (Exception ex)
        {
            com.Connection.Close();
            throw ex;
        }

        return o;
    }

    public static object getValueNoOpen(SqlCommand com)
    {
        object o = com.ExecuteScalar();

        return o;
    }
    public static int Execute(MySqlCommand com)
    {
        int r = 0;

        try
        {
            com.Connection.Open();
            r = com.ExecuteNonQuery();
        }
        catch (MySqlException ex)
        {
            throw new Exception("Erreur MySQL : " + ex.Message, ex);
        }
        finally
        {
            com.Connection.Close();
        }

        return r;
    }

    public static int executeNoOpen(SqlCommand com)
    {
        int r = com.ExecuteNonQuery();

        return r;
    }

    public static bool BeginTransaction(ref SqlConnection sqlCon)
    {
        bool bResult = false;

        try
        {
            do
            {
                if (sqlCon.State != ConnectionState.Open)
                {
                    sqlCon.Open();
                }
                SqlCommand sqlCmd = new SqlCommand("BEGIN TRANSACTION TDATABASE", sqlCon);
                DataBase.executeNoOpen(sqlCmd);

                bResult = true;
            }
            while (false);
        }
        catch { sqlCon = null; }    //  on force la connection à null pour ne pas exécuter de requête sans transaction

        return bResult;
    }

    public static bool CommitTransaction(SqlConnection sqlCon)
    {
        bool bResult = false;

        try
        {
            do
            {
                if (sqlCon.State != ConnectionState.Open) break;
                SqlCommand sqlCmd = new SqlCommand("COMMIT TRANSACTION TDATABASE", sqlCon);
                DataBase.executeNoOpen(sqlCmd);

                bResult = true;
            }
            while (false);
        }
        catch { }

        return bResult;
    }

    // Annulation de la transaction
    public static bool RollbackTransaction(SqlConnection sqlCon)
    {
        bool bResult = false;

        try
        {
            do
            {
                if (sqlCon.State != ConnectionState.Open) break;
                SqlCommand sqlCmd = new SqlCommand("ROLLBACK TRANSACTION TDATABASE", sqlCon);
                DataBase.executeNoOpen(sqlCmd);

                bResult = true;
            }
            while (false);
        }
        catch { }

        return bResult;
    }

    // permet de lire les données sans verrou
    public static bool SetReadUnCommitted(SqlConnection sqlCon, bool bValue)
    {
        bool bResult = false;

        try
        {
            do
            {
                if (sqlCon.State != ConnectionState.Open) break;
                SqlCommand sqlCmd = new SqlCommand(string.Format("SET TRANSACTION ISOLATION LEVEL READ {0}", bValue ? "UNCOMMITTED" : "COMMITTED"), sqlCon);
                DataBase.executeNoOpen(sqlCmd);

                bResult = true;
            }
            while (false);
        }
        catch { }

        return bResult;
    }

    public static void CheckConnection()
    {
        if (!m_bCheckConnection) return;

        try
        {
            if (m_arSQL == null)
            {
                m_arSQL = new List<SqlConnection>();
                return;
            }

            int nNbConnection = 0, iMax = m_arSQL.Count;
            for (int i = iMax - 1; i >= 0; i--)
            {
                if (m_arSQL[i] != null && m_arSQL[i].State != ConnectionState.Closed)
                {
                    nNbConnection++;
                }
                else
                {
                    m_arSQL.RemoveAt(i);
                }
            }

#if DEBUG
            System.Diagnostics.Debug.WriteLine(string.Format("Active Connection : {0} / {1}", nNbConnection, iMax));
            System.Diagnostics.Debug.Flush();
#endif
        }
        catch { }
    }

    public static bool IsTransactionOpened(SqlConnection sqlcon)
    {
        bool bResult = false;

        try
        {
            string szRequete = @"   SELECT  @@TRANCOUNT";
            SqlCommand sqlCmd = new SqlCommand(szRequete, sqlcon);

            return (int)DataBase.getValueNoOpen(sqlCmd) > 0;
        }
        catch { }

        return bResult;
    }

    private static bool IsSelectAutorise(string szTable, string szSuffixeView = "")
    {
        bool bResult = false;

        szTable = szTable.ToUpper();

        if (szTable == "" || szTable[0] == '@' || szTable[0] == '#') bResult = true;
        else
        {
            if (szSuffixeView != "")
            {
                if (m_arViewCCRAutorisees.Exists(delegate (string szTmp) { return szTmp + szSuffixeView == szTable; })
                    ||
                    m_arViewCommunesAutorisees.Exists(delegate (string szTmp) { return szTmp == szTable; })
                    )
                {
                    bResult = true;
                }
            }
            else
            {
                if (m_arTableAutorisees.Exists(delegate (string szTmp) { return szTmp == szTable; })) bResult = true;
            }
        }

        return bResult;
    }

    private static List<string> GetListTable(string szQuery)
    {
        List<string> arTable = new List<string>();
        List<string> arKeyWord = new List<string>() { "FROM", "JOIN", "APPLY" };

        foreach (string szKeyWord in arKeyWord)
        {
            int nIndex = 0;
            int nKeyWordLength = szKeyWord.Length;

            do
            {
                nIndex = szQuery.IndexOf(szKeyWord, nIndex);
                if (nIndex == -1) break;

                int nStart = nIndex + nKeyWordLength + 1;
                int nEnd = -1;

                if (nStart > szQuery.Length) break;

                if (szQuery[nIndex - 1] == ' ' && szQuery[nStart - 1] == ' ')
                {
                    nEnd = szQuery.IndexOf(" ", nStart);

                    if (nEnd == -1) nEnd = szQuery.Length;

                    string szTmp = szQuery.Substring(nStart, nEnd - nStart).Trim();
                    if (szTmp != "SELECT")
                    {
                        string[] arTmp = szTmp.Split('.');
                        szTmp = arTmp[arTmp.Length - 1];

                        if (arTable.IndexOf(szTmp) == -1) arTable.Add(szTmp);
                    }
                }

                nIndex += nKeyWordLength + 1;
            }
            while (true);
        }

        return arTable;
    }

    private static bool CheckQuery(string szQuery, string szSuffixe, ref string szErreur, ref List<string> arDetailErreur)
    {
        bool bResult = true;

        do
        {
            szQuery = szQuery.ToUpper();
            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex(@"[\r\n\t]");
            szQuery = pattern.Replace(szQuery, " ");
            pattern = new System.Text.RegularExpressions.Regex(@"\s{2,}");
            szQuery = pattern.Replace(szQuery, " ");

            if (szQuery.IndexOf("@@") != -1)
            {
                szErreur = "Utilisation de fonction système refusée";
                bResult = false;
                break;
            }

            foreach (string szFonction in m_arFonctionInterdite)
            {
                pattern = new System.Text.RegularExpressions.Regex(string.Format(@"{0}[ ]*\(.*\)", szFonction));

                if (pattern.IsMatch(szQuery))
                {
                    szErreur = "Utilisation de fonction système refusée";
                    bResult = false;

#if DEBUG
                    arDetailErreur.Add(szFonction);
#else
                    break;
#endif
                }
            }

            if (!bResult) break;


            pattern = new System.Text.RegularExpressions.Regex(@"[()]");
            szQuery = pattern.Replace(szQuery, " ");
            pattern = new System.Text.RegularExpressions.Regex(@"\s{2,}");
            szQuery = pattern.Replace(szQuery, " ");

            foreach (string szMot in m_arMotClefInterdit)
            {
                pattern = new System.Text.RegularExpressions.Regex(string.Format(@"(^{0}[ ]+|[ ;]{{1,}}{0}[ ]+|[ ;]{{1,}}{0}$)", szMot));

                if (pattern.IsMatch(szQuery))
                {
                    szErreur = "Mot-clef non autorisé";
                    bResult = false;

#if DEBUG
                    arDetailErreur.Add(szMot);
#else
                    break;
#endif
                }
            }

            if (!bResult) break;

            szQuery = szQuery.Replace("''", " ");
            // on retire les caractères entre quote, ils ne rentrent pas dans la vérification des tables
            pattern = new System.Text.RegularExpressions.Regex(@"'[^']{1,}'");
            szQuery = pattern.Replace(szQuery, " ");
            pattern = new System.Text.RegularExpressions.Regex(@"\s{2,}");
            szQuery = pattern.Replace(szQuery, " ");

            List<string> arTable = GetListTable(szQuery);

            foreach (string szTable in arTable)
            {
                if (!IsSelectAutorise(szTable, szSuffixe))
                {
                    szErreur = "Table sélectionnée non autorisée";
                    bResult = false;
#if DEBUG
                    arDetailErreur.Add(szTable);
#else
                    break;
#endif
                }
            }
        }
        while (false);

        return bResult;
    }

    private static List<string> m_arViewCCRAutorisees = new List<string>() { "A_FACTURER", "AGREMENT", "ANNEE_CENTRE", "ARTICLE", "AUDITEUR", "AUDITEUR_DIPLOME", "CENTRE", "CLIENT", "CLOTURE_EXERCICE",
        "COMMUNE", "CONVENTION", "CONVENTION_UNITE", "DIPLOME_CNAM", "DISPENSE", "DOCUMENT_AUDITEUR", "DUE", "ECHEANCE", "EMARGEMENT", "ENSEIGNANT", "ENSEIGNANT_COMPETENCE", "ENSEIGNANT_MATRICULE",
        "ENTREPRISE", "ENTREPRISE_FINANCEUR", "ENTREPRISE_TYPE_FINANCEUR", "EVENEMENT", "EVENEMENT_AUDITEUR", "EVENEMENT", "EVENEMENT_MATERIEL", "EXAMEN", "FACTURE", "FACTURE_DETAIL", "FACTURE_PIECE",
        "FACTURE_RELANCE", "FORMATION_PROPOSEE", "FORMATION_UNITE_OUVERTE", "FREQUENCE", "INSCRIPTION", "INSCRIPTION_MATIERE", "INSCRIPTION_UNITE", "MATRICULE", "MODALITE_CENTRE", "NOTATION_CENTRE", "NOTE",
        "PERIODE_FERMETURE", "PERIODE_PAIEMENT", "PRISE_EN_CHARGE", "PROGRAMME", "PROSPECT", "PROSPECT_DEMANDE_INFORMATION", "PROSPECT_INSCRIPTION", "PROSPECT_INSCRIPTION_UNITE", "PROSPECT_SUIVI",
        "REGLEMENT", "REMUNERATION_FORFAITAIRE", "REMUNERATION_HORAIRE", "SALLE", "SALLE_CENTRE", "SOUS_GROUPE", "TARIF_DIB", "TARIF_UNITE", "TYPE_EVENEMENT", "TYPE_TARIF", "UNITE_DIPLOME_CNAM", "UNITE",
        "UNITE_OUVERTE"
        };

    private static List<string> m_arViewCommunesAutorisees = new List<string>() {
        "ANNEE", "COMMUNE", "FINANCEMENT", "NOTATION",
        "REF_CIVILITE", "REF_DEPARTEMENT", "REF_DIPLOME_INITIAL", "REF_NATIONALITE", "REF_PAYS", "REF_SITUATION_PROFESSIONNELLE", "REF_SPECIALITE", "REF_STATUT_EMPLOI", "REF_TEMPS_TRAVAIL",
        "SPECIALITE_DU_CNAM"
        };

    private static List<string> m_arTableAutorisees = new List<string>() { "A_FACTURER", "ACTIONS_REMUNERATION", "AGREMENT", "ANNEE_UNIVERSITAIRE", "ANNEE_UNIVERSITAIRE_CENTRE", "ARTICLE", "AUDITEUR", "AUDITEUR_CENTRE", "AUDITEUR_DIPLOME",
        "BANQUE", "BORDEREAU_HEURES_A_PAYER",
        "CATEGORIE_ENSEIGNANT", "CENTRE", "CLIENT", "CLOTURE_EXERCICE", "CLOTURE_MOTIF", "COMPETENCE_ENSEIGNANT", "COMPTE_ANALYTIQUE", "CONNAITRE_CNAM", "CONVENTION", "CONVENTION_UNITE", "CRA",
        "DEMANDE_INFORMATION", "DEPARTEMENT", "DEPARTEMENT_CNAM", "DIPLOME", "DIPLOME_INITIAL", "DIPLOME_INITIAL_PREPARE", "DIPLOME_NIVEAU", "DISPENSE", "DISPOSITIF_FINANCEMENT", "DOCUMENT_AUDITEUR", "DUE",
        "ECHEANCE", "ECOLE_CNAM", "EMARGEMENT", "ENSEIGNANT", "ENSEIGNANT_COMPETENCE", "ENSEIGNANT_AGREMENT", "ENSEIGNANT_MATRICULE",  "ENSEIGNANT_TYPE", "ENTREPRISE",  "ENTREPRISE_FINANCEUR", "ENTREPRISE_TYPE_FINANCEUR", "EQUIPE_PEDAGOGIQUE", "ETAT_INSCRIPTION", "ETAT_OUVERTURE", "EVENEMENT", "EVENEMENT_AUDITEUR", "EVENEMENT", "EVENEMENT_MATERIEL", "EVENEMENT_NATURE", "EXAMEN", "EXPERIENCE_PROFESSIONNELLE",
        "FACTURE", "FACTURE_DETAIL", "FACTURE_PIECE", "FACTURE_RELANCE", "FINANCEUR", "FORMATION", "FORMATION_CENTRE", "FORMATION_CENTRE_ANNEE", "FORMATION_PROPOSEE", "FORMATION_UNITE", "FORMATION_CENTRE_UNITE_OUVERTE", "FORMATION_UNITE_OUVERTE", "FREQUENCE", "GROUPE",
        "GENRE_UNITE", "GROUPE_OPTIONS",
        "INSCRIPTION_FORMATION", "INDEMNISATION",  "INSCRIPTION", "INSCRIPTION_MATIERE", "INSCRIPTION_UNITE", "INSCRIPTION_UNITE_DISPENSE",
        "LIEU",
        "MATERIEL", "MATRICULE", "META_MODALITE", "MODE_TRANSPORT", "MINIMA_SOCIAUX", "MODALITE", "MOTIF_ABANDON", "MOYEN_COMMUNICATION",
        "NATIONALITE", "NOTATION_CENTRE", "NOTE",
        "OPTION_FORMATION",
        "PERIODE_FERMETURE", "PERIODE_PAIEMENT", "PERSONNE", "PROFIL_ENSEIGNANT", "PRISE_EN_CHARGE",  "PROFESSION_INSEE",  "PROGRAMME", "PROSPECT", "PROSPECT_DEMANDE_INFORMATION", "PROSPECT_INSCRIPTION", "PROSPECT_INSCRIPTION_UNITE", "PROSPECT_SUIVI", "POLE", "PROMOTION",
        "QUALIFICATION_EMPLOI",
        "RAISON_ABANDON", "REGLEMENT", "REGLEMENT_INSCRIPTION", "REGROUPEMENT_COMPTABLE", "REGROUPEMENT_PROGRAMME", "RELANCE", "RELANCE_NIVEAU", "REMUNERATION", "REMUNERATION_FORFAITAIRE", "REMUNERATION_FORFAITAIRE_BORDEREAU", "REMUNERATION_HORAIRE",
        "SALLE", "SALLE_CENTRE", "SECTEUR_ACTIVITE", "SECTEUR_ENTREPRISE", "SEMESTRE", "SEXE", "SITUATION_FAMILLE", "SITUATION_GEOGRAPHIQUE", "SITUATION_PROFESSIONNELLE_2007",  "SOUS_GROUPE", "SPECIALITE", "SPECIALITE_CNAM", "STATUT_AUDITEUR", "STATUT_EMPLOI_2007", "SUIVI",
        "TAILLE_ENTREPRISE", "TARIF_INSCRIPTION", "TARIF_UNITE", "TEMPORALITE", "TEMPS_TRAVAIL", "TYPE_ACTIVITE", "TYPE_CAUTION", "TYPE_CLIENT", "TYPE_CODE_UNITE", "TYPE_DIPLOME_INITIAL", "TYPE_EMARGEMENT", "TYPE_EXAMEN", "TYPE_OUVERTURE", "TITRE_PERSONNE", "TYPE_CENTRE", "TYPE_COMPETENCE", "TYPE_CONTRAT", "TYPE_CONTRAT_ENSEIGNANT", "TYPE_DIPLOME_CNAM", "TYPE_EVENEMENT", "TYPE_FINANCEMENT", "TYPE_FINANCEMENT_REGLEMENT", "TYPE_FINANCEUR", "TYPE_FORMATION", "TYPE_OBTENTION", "TYPE_MATERIEL", "TYPE_NOTATION", "TYPE_NOTATION_CENTRE", "TYPE_PARCOURS", "TYPE_PROSPECT", "TYPE_REGLEMENT", "TYPE_RENDEZ_VOUS", "TYPE_RESULTAT", "TYPE_SUIVI", "TYPE_TARIF", "TYPE_UNITE", "TYPE_UNITE_FORMATION", "TVA",
        "UNITE_DIPLOME_CNAM", "UNITE", "UNITE_CONVENTION", "UNITE_OUVERTE", "UTILISATEUR",
        "VACANCES", "VILLE"
         };

    private static List<string> m_arFonctionInterdite = new List<string>() { 
    //  fonctions système
        "$PARTITION", "CONNECTIONPROPERTY", "CONTEXT_INFO", "CURRENT_REQUEST_ID", "CURRENT_TRANSACTION_ID", "ERROR_LINE", "ERROR_PROCEDURE", "ERROR_SEVERITY", "GET_FILESTREAM_TRANSACTION_CONTEXT", "GETANSINULL", "HOST_ID", "HOST_NAME", "MIN_ACTIVE_ROWVERSION", "ROWCOUNT_BIG", "SESSION_CONTEXT", "XACT_STATE",
    //  fonctions sécurité
        "CERTENCODED", "CERTPRIVATEKEY", "CURRENT_USER", "HAS_DBACCESS", "HAS_PERMS_BY_NAME", "IS_MEMBER", "IS_ROLEMEMBER", "IS_SRVROLEMEMBER", "LOGINPROPERTY", "ORIGINAL_LOGIN", "PERMISSIONS", "PWDENCRYPT", "PWDCOMPARE", "SESSION_USER", "SESSIONPROPERTY", "SUSER_ID", "SUSER_NAME", "SUSER_SID", "SUSER_SNAME", "SYSTEM_USER", "Utilisateur", "USER_ID", "USER_NAME",
    //  fonctions méta-données
        "APP_NAME", "APPLOCK_MODE", "APPLOCK_TEST", "ASSEMBLYPROPERTY", "COL_LENGTH", "COL_NAME", "COLUMNPROPERTY", "DATABASE_PRINCIPAL_ID", "DATABASEPROPERTYEX", "DB_ID", "DB_NAME", "FILE_ID", "FILE_IDEX", "FILE_NAME", "FILEGROUP_ID", "FILEGROUP_NAME", "FILEGROUPPROPERTY", "FILEPROPERTY", "FULLTEXTCATALOGPROPERTY", "FULLTEXTSERVICEPROPERTY", "INDEX_COL", "INDEXKEY_PROPERTY", "INDEXPROPERTY", "NEXT VALUE FOR", "OBJECT_DEFINITION", "OBJECT_ID", "OBJECT_NAME", "OBJECT_SCHEMA_NAME", "OBJECTPROPERTY", "OBJECTPROPERTYEX", "ORIGINAL_DB_NAME", "PARSENAME", "SCHEMA_ID", "SCHEMA_NAME", "SCOPE_IDENTITY", "SERVERPROPERTY", "STATS_DATE", "TYPE_ID", "TYPE_NAME", "TYPEPROPERTY",
    //  fonctions chiffrement
        "ASYMKEY_ID", "ASYMKEYPROPERTY", "CERTPROPERTY", "CERT_ID", "CRYPT_GEN_RANDOM", "DECRYPTBYASYMKEY", "DECRYPTBYCERT", "DECRYPTBYKEY", "DECRYPTBYKEYAUTOASYMKEY", "DECRYPTBYKEYAUTOCERT", "DECRYPTBYPASSPHRASE", "ENCRYPTBYASYMKEY", "ENCRYPTBYCERT", "ENCRYPTBYKEY", "ENCRYPTBYPASSPHRASE", "HASHBYTES", "IS_OBJECTSIGNED", "KEY_GUID", "KEY_ID", "KEY_NAME", "SIGNBYASYMKEY", "SIGNBYCERT", "SYMKEYPROPERTY", "VERIFYSIGNEDBYCERT", "VERIFYSIGNEDBYASYMKEY",
    //  autres
        "PUBLISHINGSERVERNAME"
    };

    private static List<string> m_arMotClefInterdit = new List<string>() { "ALTER", "BEGIN", "COMMIT", "DROP", "GRANT", "ROLLBACK" };
}
