<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangerMotDePasse.aspx.cs" Inherits="GestionScol._ChangerMDP" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h1>Modifier mot de passe</h1>

        <div class="form-group">
            <label for="txtNouveauMDP">Nouveau mot de passe :</label>
            <asp:TextBox ID="txtNouveauMDP" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtNouveauMDPConfirm">Confirmer mot de passe :</label>
            <asp:TextBox ID="txtNouveauMDPConfirm" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
        </div>

        <asp:Button ID="btnEnregistrerMdp" runat="server" Text="Enregister" CssClass="btn btn-primary" OnClick="btnEnregistrer_Click" />
    </div>
</asp:Content>
