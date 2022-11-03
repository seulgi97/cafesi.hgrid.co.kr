<%@ Page Title="홈 페이지" Language="C#" MasterPageFile="~/Master/Site.master" AutoEventWireup="true"
    CodeFile="자판기재고상태.aspx.cs" Inherits="Menu_자판기재고상태" Theme="Main" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
	<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
		<ProgressTemplate>
			<div id="div_loading" style="position:absolute;top:0px;left:0px;padding-top:200px;width:100%;height:800px;background:#FFFFFF;text-align:center;"><img alt="" src="/images/loading.gif" /></div>
		</ProgressTemplate>
	</asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <h2>
        &nbsp;자판기 재고 상태
    </h2>

    <div style="border:1px solid #e6e6e6; padding:10px;">
        <div style="padding:5px"> * 단말기전체 
            <asp:DropDownList ID="dd업체명" runat="server" SkinID="dd1" AutoPostBack="true" OnSelectedIndexChanged="bt새로고침_Click"></asp:DropDownList>
            <asp:DropDownList ID="dd대리점명" runat="server" SkinID="dd1" AutoPostBack="true" OnSelectedIndexChanged="bt새로고침_Click"></asp:DropDownList>
            <asp:DropDownList ID="dd자판기명" runat="server" SkinID="dd1" AutoPostBack="true" OnSelectedIndexChanged="bt새로고침_Click"></asp:DropDownList>
            <asp:CheckBox ID="cb상태오류건" runat="server" SkinID="cb1" Text="상태오류건보기" AutoPostBack="true" OnCheckedChanged="bt새로고침_Click" />
            <asp:TextBox ID="tb검색어" runat="server" SkinID="tb1"></asp:TextBox>
            <asp:Button ID="bt새로고침" runat="server" Text="검색" SkinID="bt1" OnClick="bt새로고침_Click" />

            [ <asp:Label ID="lb단말기수" runat="server" Text="0"></asp:Label> 건 ]
        </div>
        <asp:GridView ID="gv단말기리스트" runat="server"
                AutoGenerateColumns="false" GridLines="None" EnableTheming="True"
                Width="100%" CellPadding="4" SkinID="GridView1"
                RowStyle-HorizontalAlign="Center" OnRowCommand="gv단말기리스트_RowCommand"
                AllowPaging="false" EmptyDataText="van_result  없음" OnRowDataBound="gv단말기리스트_RowDataBound"
                >
                <Columns>
                    <asp:BoundField DataField="idx" HeaderText="idx" />
                    <asp:BoundField DataField="자판기번호" HeaderText="자판기번호" />
                    <asp:BoundField DataField="설치일" HeaderText="설치일" />
                    <asp:BoundField DataField="업체명" HeaderText="업체명" />
                    <asp:BoundField DataField="대리점명" HeaderText="대리점명" />
                    <asp:TemplateField HeaderText="설치장소">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("설치장소") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="lte_router" HeaderText="lte_router" />
                    <asp:TemplateField HeaderText="최종보고시간">
                        <ItemTemplate>
                            <asp:Label ID="Label11" runat="server" Text='<%# Eval("최종재고").ToString() %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="VAN상태">
                        <ItemTemplate>
                            <asp:Label ID="Label121" runat="server" Text='<%# Eval("VAN상태") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="태블릿상태">
                        <ItemTemplate>
                            <asp:Label ID="Label122" runat="server" Text='<%# Eval("태블릿상태") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RS232상태">
                        <ItemTemplate>
                            <asp:Label ID="Label123" runat="server" Text='<%# Eval("RS232상태") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="컨트롤러상태">
                        <ItemTemplate>
                            <asp:Label ID="Label124" runat="server" Text='<%# Eval("컨트롤러상태") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="랙1">
                        <ItemTemplate>
                            <asp:Label ID="r0" runat="server" Text='<%# Eval("m9") %>' Visible="false"></asp:Label>
                            <asp:Label ID="r1" runat="server" Text='<%# Convert.ToInt32(Eval("m9").ToString().Substring(6,3)) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="랙2">
                        <ItemTemplate>
                            <asp:Label ID="r2" runat="server" Text='<%# Convert.ToInt32(Eval("m9").ToString().Substring(12,3)) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="랙3">
                        <ItemTemplate>
                            <asp:Label ID="r3" runat="server" Text='<%# Convert.ToInt32(Eval("m9").ToString().Substring(18,3)) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="랙4">
                        <ItemTemplate>
                            <asp:Label ID="r4" runat="server" Text='<%# Convert.ToInt32(Eval("m9").ToString().Substring(24,3)) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="랙5">
                        <ItemTemplate>
                            <asp:Label ID="r5" runat="server" Text='<%# Convert.ToInt32(Eval("m9").ToString().Substring(30,3)) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="랙6">
                        <ItemTemplate>
                            <asp:Label ID="r6" runat="server" Text='<%# Convert.ToInt32(Eval("m9").ToString().Substring(36,3)) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="랙7">
                        <ItemTemplate>
                            <asp:Label ID="r7" runat="server" Text='<%# Convert.ToInt32(Eval("m9").ToString().Substring(42,3)) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="랙8">
                        <ItemTemplate>
                            <asp:Label ID="r8" runat="server" Text='<%# Convert.ToInt32(Eval("m9").ToString().Substring(48,3)) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="랙9">
                        <ItemTemplate>
                            <asp:Label ID="r9" runat="server" Text='<%# Convert.ToInt32(Eval("m9").ToString().Substring(54,3)) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                </asp:GridView>
    </div>
    <br />

    <br />
            
        
        </ContentTemplate>
    </asp:UpdatePanel>

  </asp:Content>
