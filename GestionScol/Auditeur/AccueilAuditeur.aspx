<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccueilAuditeur.aspx.cs" Inherits="GestionScol._Auditeur" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:MultiView ID="mvListe" runat="server" ActiveViewIndex="0" >
       <asp:View ID="vListe" runat="server">
           <div class="container mt-4">
               <div class="card">
                   <asp:Panel id="pnlTitre" runat="server" GroupingText="Liste des cours"/>
                   <table width="100%">
                       <asp:Repeater ID="rptrListeMaj" runat="server">
                           <HeaderTemplate>
                               <thead>
                                   <tr>
                                       <th>
                                           <asp:Label ID="lblDate" runat="server" Text="Date"/>
                                       </th>
                                       <th>
                                           <asp:Label ID="lblHeureDeb" runat="server" Text="Heure début"/>
                                       </th>
                                       <th>
                                            <asp:Label ID="lblHeurefin" runat="server" Text="Heure Fin"/>
                                        </th>
                                       <th>
                                           <asp:Label ID="lblNomCours" runat="server" Text="Description"/>
                                       </th>
                                   </tr>
                               </thead>
                           </HeaderTemplate>    
               
                           <ItemTemplate>
                                   <tr>
                                        <td>
                                            <asp:Label ID="lbDate" runat="server" Text='<%# Eval("date", "{0:yyyy-MM-dd}") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbHeureDeb" runat="server" Text='<%# Eval("heure_debut") %>'></asp:Label>
                                        </td>
                                        <td>
                                             <asp:Label ID="lbHeureFin" runat="server" Text='<%# Eval("heure_fin") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbDescription" runat="server" Text='<%# Eval("libelle") %>'></asp:Label>
                                        </td>
                                   </tr>
                           </ItemTemplate>
                           <FooterTemplate>
                           </FooterTemplate>
                       </asp:Repeater>
                   </table>
                   <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
               </div>
           </div>
       </asp:View>
    </asp:MultiView>


</asp:Content>
