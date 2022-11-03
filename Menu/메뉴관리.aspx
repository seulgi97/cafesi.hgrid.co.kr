<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.master" AutoEventWireup="true" CodeFile="메뉴관리.aspx.cs" Inherits="Menu_메뉴관리"  EnableEventValidation="false" Theme="Main" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h2> 메뉴 관리 </h2>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
    <asp:Panel ID="Panel2" runat="server" BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" Style="padding: 5px; margin-top:5px;">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
            <td style="padding-left:15px">
                검색어 : <asp:TextBox ID="tb검색어" runat="server" MaxLength="100" SkinID="tb1" Style="width:170px" ValidationGroup="se"></asp:TextBox>
                <asp:Button ID="Button3" runat="server" OnClick="Button1_Click" SkinID="bt1" Text="검색" ValidationGroup="se" />
                <asp:Button ID="Button4" runat="server" SkinID="bt2" Text="엑셀" onclick="Button4_Click" CausesValidation="false" />
            </td>
            <td align="right" style="padding-right:10px">
                <asp:Button ID="bt추가" runat="server" Text="추가" CausesValidation="false" SkinID="bt2" onclick="bt추가_Click"  />
            </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:GridView ID="GridView1" runat="server" DataKeyNames="id"
    AutoGenerateColumns="False" GridLines="None" EnableTheming="True"
    Width="100%" CellPadding="4" SkinID="GridView1" AllowPaging="true" PageSize="30"
    onpageindexchanging="GridView1_PageIndexChanging" 
    RowStyle-HorizontalAlign="Center" onrowcommand="GridView1_RowCommand"
    >
    <Columns>
    <asp:BoundField DataField="id" HeaderText="id" />
    <asp:BoundField DataField="메뉴그룹" HeaderText="메뉴그룹" />
    <asp:BoundField DataField="메뉴명" HeaderText="메뉴명" />
    <asp:BoundField DataField="메뉴경로" HeaderText="메뉴경로" />
    <asp:BoundField DataField="구분" HeaderText="구분" />
    <asp:BoundField DataField="등록일시" HeaderText="등록일시" DataFormatString="{0:d}" />
    <asp:BoundField DataField="공통메뉴여부" HeaderText="공통메뉴여부" />
    <asp:TemplateField>
        <ItemTemplate>
            <asp:Button ID="Button1" runat="server" Text="부여" CausesValidation="false" CommandName="ngo" CommandArgument='<%# Eval("id") %>' SkinID="bt1" />
            <asp:Button ID="Button2" runat="server" Text="수정" CausesValidation="false" CommandName="up" CommandArgument='<%# Eval("id") %>' SkinID="bt1" />
            <asp:Button ID="Button5" runat="server" Text="삭제" CausesValidation="false" CommandName="del" CommandArgument='<%# Eval("id") %>' OnClientClick="if(!confirm('복구 하실 수 없습니다. 삭제하시겠습니까?')){ return false; } " SkinID="bt2" />
        </ItemTemplate>
    </asp:TemplateField>
    </Columns>
    </asp:GridView>

    <div id="Div_InsertForm" runat="server" visible="false" style="position:fixed;top:100px;left:50px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px">
        <table cellpadding="1" cellspacing="1" style="width:650px;background:#151521;">
            <tr style="height:25px;">
                <td colspan="2" align="center"> 메뉴 추가 </td>
            </tr>
            <tr style="height:25px;">
                <td align="center">메뉴그룹</td>
                <td style="padding-left:10px">
                    <asp:TextBox ID="in메뉴그룹" runat="server" SkinID="tb1" MaxLength="10" Width="180px"></asp:TextBox>
                    <asp:RequiredFieldValidator  ID="RequiredFieldValidator2" runat="server" ErrorMessage=" * 필수" ForeColor="Red" ControlToValidate="in메뉴그룹" ValidationGroup="in"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="height:25px;">
                <td align="center">메뉴명</td>
                <td style="padding-left:10px">
                    <asp:TextBox ID="in메뉴명" runat="server" SkinID="tb1" MaxLength="30" Width="250px"></asp:TextBox>
                    <asp:RequiredFieldValidator  ID="RequiredFieldValidator1" runat="server" ErrorMessage=" * 필수" ForeColor="Red" ControlToValidate="in메뉴명" ValidationGroup="in"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="height:25px;">
                <td align="center">메뉴경로 </td>
                <td style="padding-left:10px">
                    <asp:TextBox ID="in메뉴경로" runat="server" SkinID="tb1" MaxLength="150" Width="350px"></asp:TextBox>
                    <asp:RequiredFieldValidator  ID="RequiredFieldValidator3" runat="server" ErrorMessage=" * 필수" ForeColor="Red" ControlToValidate="in메뉴경로" ValidationGroup="in"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="height:25px;">
                <td align="center">구분 </td>
                <td style="padding-left:10px">
                    <asp:DropDownList ID="in구분" runat="server" SkinID="dd1">
                        <asp:ListItem  Value="메뉴" Text="메뉴" Selected="True"></asp:ListItem>
                        <asp:ListItem  Value="업무" Text="업무"></asp:ListItem>
                        <asp:ListItem  Value="권한" Text="권한"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="height:25px;">
                <td align="center">공통메뉴여부 </td>
                <td style="padding-left:10px">
                    <asp:DropDownList ID="in공통메뉴여부" runat="server" SkinID="dd1">
                        <asp:ListItem  Value="N" Text="N" Selected="True"></asp:ListItem>
                        <asp:ListItem  Value="Y" Text="Y"></asp:ListItem>
                    </asp:DropDownList> * Y 일경우 권한에 관계없이 노출됨
                </td>
            </tr>
            <tr style="height:25px;">
                <td colspan="2" align="center" style="padding-bottom:20px;">
                    <asp:Button ID="bt입력" runat="server" Text="등록완료" OnClick="bt입력_Click" SkinID="bt1" ValidationGroup="in"/>
                    <asp:Button ID="bt입력닫기" runat="server" Text="닫기" OnClick="bt입력닫기_Click" SkinID="bt2"/>
                </td>
            </tr>
        </table>
    </div>

    <div id="Div_updateForm" runat="server" visible="false" style="position:fixed;top:100px;left:50px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px">
        <table cellpadding="1" cellspacing="1" style="width:650px;background:#151521;">
            <tr style="height:25px;">
                <td colspan="2" align="center"> <asp:Label ID="upseq" runat="server" Text=""></asp:Label> 메뉴 수정 </td>
            </tr>
            <tr style="height:25px;">
                <td align="center">메뉴그룹</td>
                <td style="padding-left:10px">
                    <asp:TextBox ID="up메뉴그룹" runat="server" SkinID="tb1" MaxLength="10" Width="180px"></asp:TextBox>
                    <asp:RequiredFieldValidator  ID="RequiredFieldValidator4" runat="server" ErrorMessage=" * 필수" ForeColor="Red" ControlToValidate="up메뉴그룹" ValidationGroup="up"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="height:25px;">
                <td align="center">메뉴명</td>
                <td style="padding-left:10px">
                    <asp:TextBox ID="up메뉴명" runat="server" SkinID="tb1" MaxLength="30" Width="250px"></asp:TextBox>
                    <asp:RequiredFieldValidator  ID="RequiredFieldValidator5" runat="server" ErrorMessage=" * 필수" ForeColor="Red" ControlToValidate="up메뉴명" ValidationGroup="up"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="height:25px;">
                <td align="center">메뉴경로 </td>
                <td style="padding-left:10px">
                    <asp:TextBox ID="up메뉴경로" runat="server" SkinID="tb1" MaxLength="150" Width="350px"></asp:TextBox>
                    <asp:RequiredFieldValidator  ID="RequiredFieldValidator6" runat="server" ErrorMessage=" * 필수" ForeColor="Red" ControlToValidate="up메뉴경로" ValidationGroup="up"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="height:25px;">
                <td align="center">구분 </td>
                <td style="padding-left:10px">
                    <asp:DropDownList ID="up구분" runat="server" SkinID="dd1">
                        <asp:ListItem  Value="메뉴" Text="메뉴"></asp:ListItem>
                        <asp:ListItem  Value="업무" Text="업무"></asp:ListItem>
                        <asp:ListItem  Value="권한" Text="권한"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="height:25px;">
                <td align="center">공통메뉴여부 </td>
                <td style="padding-left:10px">
                    <asp:DropDownList ID="up공통메뉴여부" runat="server" SkinID="dd1">
                        <asp:ListItem  Value="N" Text="N" Selected="True"></asp:ListItem>
                        <asp:ListItem  Value="Y" Text="Y"></asp:ListItem>
                    </asp:DropDownList> * Y 일경우 권한에 관계없이 노출됨
                </td>
            </tr>
            <tr style="height:25px;">
                <td colspan="2" align="center" style="padding-bottom:20px;">
                    <asp:Button ID="bt수정" runat="server" Text="수정완료" OnClick="bt수정_Click" SkinID="bt1" ValidationGroup="up"/>
                    <asp:Button ID="bt수정닫기" runat="server" Text="닫기" OnClick="bt수정닫기_Click" CausesValidation="false" SkinID="bt2"/>
                </td>
            </tr>
        </table>
    </div>
    
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

