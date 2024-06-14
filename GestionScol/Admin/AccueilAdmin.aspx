<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccueilAdmin.aspx.cs" Inherits="GestionScol._Admin" EnableEventValidation="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <div class="card">
            <asp:Panel id="pnlTitreIntervenant" runat="server" GroupingText="Liste des Intervenants"/>
            <table width="100%">
                <asp:Repeater ID="rptrListeIntervenant" runat="server" OnItemCommand="rptrListeIntervenant_ItemCommand">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th>
                                    <asp:Label ID="lblIdIntervenant" runat="server" Text="Code Intervenant"/>
                                </th>
                                <th>
                                    <asp:Label ID="lblNomIntervenant" runat="server" Text="Nom Intervenant"/>
                                </th>
                                <th>
                                    <asp:Label ID="lblPrenomIntervenant" runat="server" Text="Prenom Intervenant"/>
                                </th>
                                <th>
                                    <asp:Label ID="lblEditIntervenant" runat="server" Text="Editer"/>
                                </th>
                                <th>
                                    <asp:Label ID="lblSupprimerIntervenant" runat="server" Text="Supp"/>
                                </th>
                            </tr>
                        </thead>
                    </HeaderTemplate>    
        
                    <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="lbIdIntervenant" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbNomIntervenant" runat="server" Text='<%# Eval("Nom") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbPrenomIntervenant" runat="server" Text='<%# Eval("Prenom") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="btnEditIntervenant" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# Eval("id") %>' />
                                </td>
                                <td>
                                    <asp:Button ID="btnSuppIntervenant" runat="server" Text="Supp" CommandName="Supp" CommandArgument='<%# Eval("id") %>' />
                                </td>
                            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Button ID="btnAjouterIntervenant" runat="server" Text="Ajouter" CommandName="Ajout"  CommandArgument="I"/>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            <asp:Label ID="lblVerif" runat="server" ForeColor="Green" Visible="false"></asp:Label>
        </div>
    </div>

    <div class="container mt-4">
        <div class="card">
            <asp:Panel id="pnlAuditeur" runat="server" GroupingText="Liste des Auditeurs"/>
            <table width="100%">
                <asp:Repeater ID="rptrListeAuditeur" runat="server" OnItemCommand="rptrListeAuditeur_ItemCommand">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th>
                                    <asp:Label ID="lblIdAuditeur" runat="server" Text="Code Auditeur"/>
                                </th>
                                <th>
                                    <asp:Label ID="lblNomAuditeur" runat="server" Text="Nom Auditeur"/>
                                </th>
                                <th>
                                    <asp:Label ID="lblPrenomAuditeur" runat="server" Text="Prenom Auditeur"/>
                                </th>
                                <th>
                                    <asp:Label ID="lblEditAuditeur" runat="server" Text="Editer"/>
                                </th>
                                <th>
                                    <asp:Label ID="lblSupprimerAuditeur" runat="server" Text="Supp"/>
                                </th>
                            </tr>
                        </thead>
                    </HeaderTemplate>    
    
                    <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label ID="lbIdAuditeur" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbNomAuditeur" runat="server" Text='<%# Eval("Nom") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbPrenomAuditeur" runat="server" Text='<%# Eval("Prenom") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="btnEditAuditeur" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# Eval("id") %>' />
                                </td>
                                <td>
                                    <asp:Button ID="btnSuppAuditeur" runat="server" Text="Supp" CommandName="Supp" CommandArgument='<%# Eval("id") %>' />
                                </td>
                            </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Button ID="btnAjouterAuditeur" Text="Ajouter" runat="server" CommandName="Ajout"  CommandArgument="A"/>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
            <asp:Label ID="lblErreurAuditeur" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            <asp:Label ID="lblReussi" runat="server" ForeColor="Green" Visible="false"></asp:Label>
        </div>
    </div>

</asp:Content>
