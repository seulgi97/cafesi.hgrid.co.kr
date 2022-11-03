<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.master" AutoEventWireup="true" CodeFile="메뉴권한관리.aspx.cs" Inherits="Menu_사용자메뉴권한관리"  EnableEventValidation="false" Theme="Main" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2> 사용자 메뉴 권한 관리 </h2>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

    <asp:Panel ID="Panel2" runat="server" BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" Style="padding: 5px; margin-top:5px;">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
            <td style="padding-left:15px">
                상호검색 : <asp:TextBox ID="tb업체명검색어" runat="server" MaxLength="100" SkinID="tb1" Style="width:170px" ValidationGroup="se"></asp:TextBox>
                <asp:Button ID="Button3" runat="server" OnClick="Button1_Click" SkinID="bt1" Text="검색" ValidationGroup="se" />
            </td>
            </tr>
        </table>
    </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
    <br />

    <table border="0" cellpadding="10" cellspacing="1" style="background:#50505d; height:300px;">
        <tr style="background:#151521">
            <td>
                ID/이름 검색어 : <asp:TextBox ID="tb사용자" runat="server" SkinID="tb1"></asp:TextBox>
            </td>
            <td>
                메뉴명/그룹검색어 : <asp:TextBox ID="tb메뉴" runat="server" SkinID="tb1"></asp:TextBox>
                <asp:Button ID="bt사용자일괄부여" runat="server" OnClick="bt사용자일괄부여_OnClick" SkinID="bt1" Text="일괄부여" OnClientClick="return confirm('일괄부여 하시겠습니까?');" />
            </td>
            <td>
                메뉴 <asp:TextBox ID="tb부여메뉴" runat="server" SkinID="tb1"></asp:TextBox>
                <asp:Button ID="bt사용자일괄회수" runat="server" OnClick="bt사용자일괄회수_OnClick" SkinID="bt1" Text="일괄회수" OnClientClick="return confirm('일괄회수 하시겠습니까?');" />
            </td>
        </tr>
        <tr style="background:#151521">
            <td valign="top" style="vertical-align:top;">
                <div style="width:420px; height:300px; overflow:auto ">
                                
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>

                    <asp:GridView ID="gv사용자" runat="server" DataKeyNames="idx"
                    AutoGenerateColumns="False" GridLines="None" EnableTheming="True"
                    Width="400px" CellPadding="4" SkinID="GridView1"
                    RowStyle-HorizontalAlign="Center" onrowcommand="gv사용자_RowCommand"
                    AllowPaging="false" onpageindexchanging="gv사용자_PageIndexChanging" 
                
                    >
                    <Columns>
                        <asp:BoundField DataField="총판명" HeaderText="총판명" />
                        <asp:BoundField DataField="userid" HeaderText="id" />
                        <asp:BoundField DataField="usernm" HeaderText="이름" />
                        <asp:TemplateField ItemStyle-Width="50px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" id="bt1" Text="선택" CommandName="ok" CommandArgument='<%# Container.DataItemIndex  %>'  />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>

                </div>
            </td>
            <td valign="top">
                            
                <div style="width:500px; height:300px; overflow:auto ">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>


                    <asp:GridView ID="gv선택" runat="server" DataKeyNames="id"
                    AutoGenerateColumns="False" GridLines="None" EnableTheming="True"
                    Width="98%" CellPadding="4" SkinID="GridView1"
                    RowStyle-HorizontalAlign="Center" onrowcommand="gv선택_RowCommand"
                    >
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="cb모두체크" runat="server" AutoPostBack="True" oncheckedchanged="gv선택_on모두체크change" />
                            </HeaderTemplate>                                    
                            <ItemTemplate>
                                <asp:CheckBox ID="cb체크" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="id">
                            <ItemTemplate>
                                <asp:Label ID="lbid" runat="server" Text='<%# Eval("ID") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="메뉴그룹" HeaderText="메뉴그룹" />
                        <asp:BoundField DataField="메뉴명" HeaderText="부여가능한메뉴" />
                        <asp:TemplateField ItemStyle-Width="50px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" id="bt1" Text="추가" CommandName="ok" CommandArgument='<%#Eval("id")%>'  />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                </div>
            </td>
            <td valign="top">
                <div style="width:500px; height:300px; overflow:auto ">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>


                    <asp:GridView ID="gv메뉴" runat="server" DataKeyNames="id"
                    AutoGenerateColumns="False" GridLines="None" EnableTheming="True"
                    Width="98%" CellPadding="4" SkinID="GridView1"
                    RowStyle-HorizontalAlign="Center" onrowcommand="gv메뉴_RowCommand"
                    >
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <asp:CheckBox ID="cb모두체크" runat="server" AutoPostBack="True" oncheckedchanged="gv메뉴_on모두체크change" />
                            </HeaderTemplate>                                    
                            <ItemTemplate>
                                <asp:CheckBox ID="cb체크" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="id">
                            <ItemTemplate>
                                <asp:Label ID="lbid" runat="server" Text='<%# Eval("ID") %>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="메뉴그룹" HeaderText="메뉴그룹" />
                        <asp:BoundField DataField="메뉴명" HeaderText="부여된메뉴" />
                        <asp:TemplateField ItemStyle-Width="50px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" id="bt1" Text="제거" CommandName="ok" CommandArgument='<%#Eval("id")%>'  />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                </div>
            </td>
        </tr>
    </table>

            
</asp:Content>

