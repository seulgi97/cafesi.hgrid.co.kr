<%@ Page Title="홈 페이지" Language="C#" MasterPageFile="~/Master/Site.master" AutoEventWireup="true"
    CodeFile="사용자관리.aspx.cs" Inherits="Menu_사용자관리" Theme="Main" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    
    <div id="div_사용자등록" runat="server" visible="false" style="position:absolute;top:0px;left:30px;background-color:White;padding:5px 5px 5px 5px" >
        <table cellpadding="1" cellspacing="1" style="width:400px;background:#d6d6d6;">
            <tr style="background:#FFFFFF;height:45px;">
                <td align="center" colspan="2">
                    * 사용자(신규등록)
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#FFFFFF;" colspan="2">
                    <table>
                        <tr>
                            <td> 아이디 : </td>
                            <td><asp:TextBox ID="사용자등록_아이디" runat="server" SkinID="tb1" MaxLength="10" Width="150px"></asp:TextBox> 
                        </tr>
                        <tr>
                            <td> 이름 : </td>
                            <td> <asp:TextBox ID="사용자등록_이름" runat="server" SkinID="tb1" MaxLength="20" Width="150px"></asp:TextBox> </td>
                        </tr>
                        <tr>
                            <td> 일반전화 : </td>
                            <td>
                                <asp:TextBox ID="사용자등록_일반전화" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 휴대폰 : </td>
                            <td>
                                <asp:TextBox ID="사용자등록_휴대폰" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 소속 :</td>
                            <td>
                                 <asp:TextBox ID="사용자등록_소속" runat="server" SkinID="tb1" MaxLength="14" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 총판권한 :</td>
                            <td>
                                <asp:DropDownList ID="사용자등록_총판선택" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 대리점권한 :</td>
                            <td>
                                <asp:DropDownList ID="사용자등록_대리점선택" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 자판기권한 :</td>
                            <td>
                                <asp:DropDownList ID="사용자등록_자판기선택" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#FFFFFF;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="사용자등록_결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt사용자등록완료" runat="server" OnClick="bt사용자등록완료_Click" CausesValidation="false" Text="등록하기" SkinID="bt1" />
                    <asp:Button ID="bt사용자등록닫기" runat="server" OnClick="bt사용자등록닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
      
    <div id="div_사용자수정" runat="server" visible="false" style="position:absolute;top:0px;left:30px;background-color:White;padding:5px 5px 5px 5px" >
        <table cellpadding="1" cellspacing="1" style="width:400px;background:#d6d6d6;">
            <tr style="background:#FFFFFF;height:45px;">
                <td align="center" colspan="2">
                    * 사용자(수정/삭제)
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#FFFFFF;" colspan="2">
                    <table>
                        <tr>
                            <td> 아이디 : </td>
                            <td>
                                <asp:Label ID="사용자수정_아이디" runat="server" Text=""></asp:Label>
                        </tr>
                        <tr>
                            <td> 이름 : </td>
                            <td> <asp:TextBox ID="사용자수정_이름" runat="server" SkinID="tb1" MaxLength="20" Width="150px"></asp:TextBox> </td>
                        </tr>
                        <tr>
                            <td> 일반전화 : </td>
                            <td>
                                <asp:TextBox ID="사용자수정_일반전화" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 휴대폰 : </td>
                            <td>
                                <asp:TextBox ID="사용자수정_휴대폰" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 소속 :</td>
                            <td>
                                 <asp:TextBox ID="사용자수정_소속" runat="server" SkinID="tb1" MaxLength="14" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 총판권한 :</td>
                            <td>
                                <asp:DropDownList ID="사용자수정_총판선택" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 대리점권한 :</td>
                            <td>
                                <asp:DropDownList ID="사용자수정_대리점선택" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 자판기권한 :</td>
                            <td>
                                <asp:DropDownList ID="사용자수정_자판기선택" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#FFFFFF;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="사용자수정_결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt사용자삭제" runat="server" OnClick="bt사용자삭제_Click" CausesValidation="false" Text="삭제하기" SkinID="bt3" OnClientClick="return confirm('사용자를 삭제하시겠습니까?'); " />
                    <asp:Button ID="bt사용자수정완료" runat="server" OnClick="bt사용자수정완료_Click" CausesValidation="false" Text="수정하기" SkinID="bt1" />
                    <asp:Button ID="bt사용자수정닫기" runat="server" OnClick="bt사용자수정닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
      

    <h2>
        &nbsp;사용자 관리
        <asp:TextBox ID="tbexcel" runat="server" SkinID="tb1" Visible="false"></asp:TextBox>

        <asp:TextBox ID="선택_사용자아이디" runat="server" Visible="false"></asp:TextBox>
    </h2>
    <div class="formlayout">
        <table>
            <tr>
                <td style="vertical-align:top">
                    <div style="padding:5px">

                        * 사용자 
                        <asp:DropDownList ID="dd총판" runat="server" SkinID="dd1" AutoPostBack="true" OnSelectedIndexChanged="dd총판_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:DropDownList ID="dd대리점" runat="server" SkinID="dd1" AutoPostBack="true" OnSelectedIndexChanged="dd총판_SelectedIndexChanged">
                        </asp:DropDownList>

                        <asp:DropDownList ID="dd사용여부" runat="server" SkinID="dd1">
                            <asp:ListItem Value="1" Text="사용"></asp:ListItem>
                            <asp:ListItem Value="0" Text="미사용"></asp:ListItem>
                            <asp:ListItem Value="" Text="전체"></asp:ListItem>
                        </asp:DropDownList>

                        <asp:TextBox ID="tb검색어" runat="server" SkinID="tb1" MaxLength="30" placeHolder="검색어 입력"></asp:TextBox>
                        <asp:Button ID="bt새로고침" runat="server" Text="새로고침" SkinID="bt1" OnClick="bt새로고침_Click" />
                        <asp:Button ID="bt사용자등록" runat="server" Text="사용자등록" SkinID="bt2" OnClick="bt사용자등록_Click" />
                    </div>
                    <asp:GridView ID="gv사용자" runat="server"
                        AutoGenerateColumns="false" GridLines="None" EnableTheming="True" DataKeyNames="idx"
                        CellPadding="4" SkinID="GridView1"
                        RowStyle-HorizontalAlign="Center" OnRowCommand="gv사용자_RowCommand"
                        AllowPaging="true" PageSize="20" EmptyDataText="사용자 없음"  
                         OnPageIndexChanging="gv사용자_PageIndexChanging"
                        >
                        <Columns>
                            
                            <asp:BoundField DataField="총판명" HeaderText="총판명" />
                            <asp:BoundField DataField="대리점명" HeaderText="대리점명" />
                            <asp:BoundField DataField="usrorg" HeaderText="소속" />
                            <asp:TemplateField HeaderText="아이디" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt2"  Text='<%# Eval("userid") %>' CommandName="Select" CommandArgument='<%# Eval("userid") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="usernm" HeaderText="이름" />
                            <asp:BoundField DataField="usrtel" HeaderText="연락처" />
                            <asp:BoundField DataField="mphone" HeaderText="핸드폰" />
                            <asp:BoundField DataField="active" HeaderText="사용" />
                            <asp:BoundField DataField="lst_logintime" HeaderText="마지막접속" />
                            <asp:TemplateField HeaderText="수정" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt_up"  Text="수정" CommandName="Select" CommandArgument='<%# Eval("userid") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        </asp:GridView>

                </td>
            </tr>
        </table>

    </div>

  </asp:Content>
