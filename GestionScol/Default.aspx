<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GestionScol._Default" %>

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
                                <label for="tbtLogin">Login :</label>
                                <asp:TextBox ID="tbtLogin" runat="server" CssClass="form-control" ToolTip="identifiant" />
                            </div>
                            <div class="form-group">
                                <label for="tbtMDP">Mot de passe :</label>
                                <asp:TextBox ID="tbtMDP" runat="server" CssClass="form-control" ToolTip="mot de passe" TextMode="Password" />
                            </div>
                            <asp:Button ID="btnMotDePasseOublie" runat="server" CssClass="btn btn-primary" Text="Mot de passe oublié" OnClick="btnOublier_Click" />
                            <asp:Label ID="lblErreurDefault" runat="server" Text="Mot de passe invalid" Visible="false" style="color: red;"/>
                            <div class="text-center">
                                <asp:Button ID="btnConnexion" runat="server" CssClass="btn btn-primary" Text="Connecter" OnClick="btnConnecter_Click" />
                                <asp:Button ID="btnInscrire" runat="server" CssClass="btn btn-primary" Text="Inscription" OnClick="btnInscrire_Click" />
                            </div>
                        </asp:Panel>
                        <asp:Label ID="error" runat="server" Visible="false" style="color: red;" Text="" />
                        <asp:Label ID="valid" runat="server" Visible="false" style="color: green;" Text="" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="pnlCode" runat="server" Visible="false">
        <asp:Label ID="lblCode" runat="server" />
        <asp:TextBox ID="txtCode" runat="server" ToolTip="Saisir le code" />
        <asp:button ID="btnCode" runat="server" OnClick="btnCode_Click" />

        <asp:Label ID="lblErreur" runat="server" Text="" Visible="false" style="color: red;"/>
    </asp:Panel>
</asp:Content>
