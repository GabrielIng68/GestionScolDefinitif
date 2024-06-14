<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccueilIntervenant.aspx.cs" Inherits="GestionScol._Intervenant" EnableEventValidation="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.1/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.1/themes/base/jquery-ui.css">


    <script language="javascript" type="text/javascript">
        var $j = jQuery.noConflict();
        var popupDoc = null;

        function OnAdd() {
            var pnlAddDoc = $j(".pnlAddDoc"),
                strTitre = "Ajouter un auditeur";

            if (popupDoc == null) popupDoc = $j("<div></div>");

            var corpsElement = $j("#corps");
            if (corpsElement.length === 0) {
                console.error("#corps n'est pas trouvé sur la page.");
                return false; // Sortir de la fonction si #corps n'est pas trouvé
            }

            popupDoc.dialog({
                autoOpen: false,
                modal: true,
                title: strTitre,
                width: 'auto',
                height: 'auto',
                draggable: true,
                resizable: false,
                position: { my: "center", at: "center", of: $j("#corps") },
                open: function (e, ui) {
                    // Code à exécuter lorsque la popup s'ouvre
                },
                close: function (e, ui) {
                    pnlAddDoc.appendTo($j(".pnlAddDocContainer"));
                    pnlAddDoc.css("display", "none");
                }
            });

            pnlAddDoc.appendTo(popupDoc);
            pnlAddDoc.css("display", "block");
            popupDoc.dialog("open");

            return false;
        }

        function selectListBox(add) {
            var lbDocAdminNatureActive; var lbDocAdminNatureInactive; var docnum; var doctit;

            if (add) {
                lbDocPersonne = document.getElementById("<%= lbDocPersonneActive.ClientID %>");
                hidden = document.getElementById("<%= hidden.ClientID %>").value;
                docnum = lbDocPersonne.options[lbDocPersonne.selectedIndex].value;
                doctit = lbDocPersonne.options[lbDocPersonne.selectedIndex].text;
            }

            $j.ajax({
                type: "POST",
                url: add ? "AjoutAuditeur.aspx/btnSelectionnerDoc_Click" : "AccueilIntervenant.aspx",
                data: JSON.stringify({
                    DOCNUM: docnum,
                    DOCTIT: doctit,
                    ID: hidden
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    refreshListBox();
                },
                error: function (xhr, status, error) {
                    alert("Erreur lors de la sélection : " + xhr.responseText);
                }
            });
        }

        function refreshListBox() {
            __doPostBack('btnPostBack');
        }
    </script>

     <asp:MultiView ID="mvListe" runat="server" ActiveViewIndex="0" >
        <asp:View ID="vListe" runat="server">
            <div class="container mt-4">
                <div class="card">
                    <asp:Panel id="pnlTitre" runat="server" GroupingText="Liste des enseignements"/>
                    <table width="100%">
                        <asp:Repeater ID="rptrListeMaj" runat="server" OnItemCommand="rptrListeMaj_ItemCommand" OnItemDataBound="rptrListeMaj_ItemDataBound">
                            <HeaderTemplate>
                                <thead>
                                    <tr>
                                        <th>
                                            <asp:Label ID="lblCodeEnseignementEnTete" runat="server" Text="Code Enseignement"/>
                                        </th>
                                        <th>
                                            <asp:Label ID="lblNomEnseignementEnTete" runat="server" Text="Nom Enseignement"/>
                                        </th>
                                        <th>
                                            <asp:Label ID="lblEdite" runat="server" Text="Editer"/>
                                        </th>
                                    </tr>
                                </thead>
                            </HeaderTemplate>    
                    
                            <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbCodeEnseignement" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNomEnseignement" runat="server" Text='<%# Eval("libelle") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnEdite" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# Eval("id") %>' />
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

         <asp:View ID="vListeEdition" runat="server">
             <div id="corps"/>
             <asp:HiddenField ID="hidden" runat="server" Value=""/>
             <asp:GridView ID="lstEnseignementsEdite" runat="server" AutoGenerateColumns="False" Width="100%" 
                AllowSorting="true" HeaderStyle-HorizontalAlign="Center" AllowPaging="true" PageSize="10">                
                <Columns>
                    <asp:TemplateField HeaderText="Code Enseignement">
                        <ItemTemplate>
                            <asp:Label ID="lbCodeEnseignement" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                        <HeaderStyle Width="150px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nom Enseignement">
                        <ItemTemplate>
                            <asp:Label ID="lblNomEnseignement" runat="server" Text='<%# Eval("libelle") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                        <HeaderStyle Width="150px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:GridView ID="lstPersonneEdite" runat="server" AutoGenerateColumns="False" Width="100%" 
                AllowSorting="true" HeaderStyle-HorizontalAlign="Center" AllowPaging="true" PageSize="10"
                OnRowCommand="lstPersonne_RowCommand" OnRowDataBound="lstPersonne_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText='hidden' Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="hiddenID" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nom">
                        <ItemTemplate>
                            <asp:Label ID="lblNom" runat="server" Text='<%# Eval("nom") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                        <HeaderStyle Width="150px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pénom">
                        <ItemTemplate>
                            <asp:Label ID="lblPrenom" runat="server" Text='<%# Eval("prenom") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                        <HeaderStyle Width="150px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnSupp" runat="server" Text="Supp" CommandName="Supp" CommandArgument='<%# Eval("id") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                        <HeaderStyle Width="150px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

             <asp:Button ID="btnAjout" runat="server" Text="Ajouter" OnClientClick="return OnAdd();" UseSubmitBehavior="false"/>
         </asp:View>
    </asp:MultiView>



<asp:Panel ID="pnlAddDocContainer" runat="server" style="display: none;">
    <asp:Panel ID="pnlAddDoc" runat="server">
        <asp:Panel ID="CustomPanelRecherche" runat="server" GroupingText="Rechercher une documentation" Width="100%">
            <table class="panelTable">
                    <tr>
                        <td class="listBox">
                            <asp:Label ID="lblPersonneDispo" runat="server" SkinID="TitreListBox" Text="Auditeur disponible"></asp:Label><br />
                            <asp:ListBox ID="lbDocPersonneActive" runat="server" DataTextField="Nom" DataValueField="id" SelectionMode="Multiple" Height="110px"></asp:ListBox>
                        </td>
                        <td class="buttonListBox">
                            <br />
                            <asp:button ID="btnPersonneActive" runat="server" OnClientClick="selectListBox(true)" Text="Ajouter"
                                ToolTip="Activer les personnes sélectionnées" CssClass=""/><br />
                        </td>
                        <td class="listBox">
                            <asp:Label ID="lblPersonneInactive" runat="server" SkinID="TitreListBox" Text="Personnes sélectionnées"></asp:Label><br />
                            <asp:ListBox ID="lbPersonneInactive" runat="server" DataTextField="Nom" DataValueField="id" Height="110px"></asp:ListBox>
                        </td>
                    </tr>
            </table>
        </asp:Panel>
        <div class="popupbtn">
            <asp:button ID="btnValidDoc" runat="server" Text="Valider" ValidationGroup="valgrpDoc" CausesValidation="true" ToolTip="Valider" OnClientClick="ValiderAjoutDoc(this); return false;" />
            <asp:button ID="btnCancelDoc" runat="server" Text="Annuler" ValidationGroup="valgrpDoc" CausesValidation="false" ToolTip="Annuler" OnClientClick="AnnulerAjoutDoc(); return false;" />
        </div>
        <asp:ValidationSummary ID="valsumDoc" runat="server" ShowSummary="false" ShowMessageBox="true" ValidationGroup="valgrpDoc" />
    </asp:Panel>
</asp:Panel>


</asp:Content>
