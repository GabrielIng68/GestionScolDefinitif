<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Edition.aspx.cs" Inherits="GestionScol._Edit" EnableEventValidation="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:MultiView ID="mvListe" runat="server" ActiveViewIndex="0">
        <asp:View ID="vListeAuditeur" runat="server">
            <div class="container">
                <h1>Modifier Utilisateur</h1>

                <div class="form-group">
                    <label for="txtNom">Nom :</label>
                    <asp:TextBox ID="txtNom" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtPrenom">Prénom :</label>
                    <asp:TextBox ID="txtPrenom" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtDroit">Droit :</label>
                    <asp:TextBox ID="txtDroit" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtLogin">Login :</label>
                    <asp:TextBox ID="txtLogin" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtMdp">Mot de passe :</label>
                    <asp:TextBox ID="txtMdp" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtAdresseMail">Adresse Mail :</label>
                    <asp:TextBox ID="txtAdresseMail" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <asp:Button ID="btnEnregistrer" runat="server" Text="Enregistrer" CssClass="btn btn-primary" OnClick="btnEnregistrer_Click" />
            </div>
        </asp:View>
        <asp:View ID="vListeIntervenant" runat="server">
            <div class="container">
                <h1>Modifier Intervenant</h1>

                <div class="form-group">
                    <label for="txtNomIntervenant">Nom :</label>
                    <asp:TextBox ID="txtNomIntervenant" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtPrenomIntervenant">Prénom :</label>
                    <asp:TextBox ID="txtPrenomIntervenant" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtDroitIntervenant">Droit :</label>
                    <asp:TextBox ID="txtDroitIntervenant" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtLoginIntervenant">Login :</label>
                    <asp:TextBox ID="txtLoginIntervenant" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtMdpIntervenant">Mot de passe :</label>
                    <asp:TextBox ID="txtMdpIntervenant" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="txtAdresseMailIntervenant">Adresse Mail :</label>
                    <asp:TextBox ID="txtAdresseMailIntervenant" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <asp:Button ID="btnIntervenantEnregistrer" runat="server" Text="Enregistrer" CssClass="btn btn-primary" OnClick="btnIntervenantEnregistrer_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
