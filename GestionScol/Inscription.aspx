<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inscription.aspx.cs" Inherits="GestionScol._Inscription" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header text-center">
                        <h3>Authentification</h3>
                    </div>
                    <div class="card-body">
                        <asp:Panel ID="pnlMain" runat="server">
                            <div class="form-group">
                                <asp:Label ID="lblErreur" runat="server" CssClass="form-control" Text="Utilisateur déjà créer" Visible="false" />
                            </div>
                            <div class="form-group">
                                <label for="tbtInscriptionLogin">Login :</label>
                                <asp:TextBox ID="tbtInscriptionLogin" runat="server" CssClass="form-control" ToolTip="identifiant" />
                            </div>
                            <div class="form-group">
                                <label for="tbtMDP">Mot de passe :</label>
                                <asp:TextBox ID="tbtInscriptionMDP" runat="server" CssClass="form-control" ToolTip="mot de passe" TextMode="Password" />
                            </div>
                            <div class="form-group">
                                <label for="tbtMail">Adresse mail :</label>
                                <asp:TextBox ID="tbtMail" runat="server" CssClass="form-control" ToolTip="adresse mail" />
                            </div>
                            <div class="form-group">
                                <label for="tbtPrenom">Prénom :</label>
                                <asp:TextBox ID="tbtPrenom" runat="server" CssClass="form-control" ToolTip="Prénom"/>
                            </div>
                            <div class="form-group">
                                <label for="tbtNom">Nom :</label>
                                <asp:TextBox ID="tbtNom" runat="server" CssClass="form-control" ToolTip="Nom"/>
                            </div>
                            <div class="text-center">
                                <asp:Button ID="btnInscrire" runat="server" CssClass="btn btn-primary" Text="S'inscrire" OnClick="btnInscrire_Click" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
