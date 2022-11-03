<%@ Page Title="홈 페이지" Language="C#" MasterPageFile="~/Master/Site.master" AutoEventWireup="true"
    CodeFile="브랜드상품관리.aspx.cs" Inherits="Menu_브랜드상품관리" Theme="Main" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    
    <div id="div_브랜드등록" runat="server" visible="false" style="position:absolute;top:0px;left:30px;border:5px solid #aaa;background:#151521;;padding:5px 5px 5px 5px" >
        <table cellpadding="0" cellspacing="0" style="width:400px;background:#151521;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 브랜드(신규등록)
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 브랜드명 : </td>
                            <td> <asp:TextBox ID="브랜드등록_브랜드명" runat="server" SkinID="tb1" MaxLength="100" Width="150px"></asp:TextBox> </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center; padding-bottom:20px;">
                    <div runat="server" id="브랜드등록_결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt브랜드등록완료" runat="server" OnClick="bt브랜드등록완료_Click" CausesValidation="false" Text="등록하기" SkinID="bt1" />
                    <asp:Button ID="bt브랜드등록닫기" runat="server" OnClick="bt브랜드등록닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
      
    <div id="div_브랜드수정" runat="server" visible="false" style="position:absolute;top:0px;left:30px;border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px" >
        <table cellpadding="0" cellspacing="0" style="width:400px;background:#151521;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 브랜드(수정)<asp:Label ID="브랜드수정_idx" runat="server" Text="" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 브랜드명 : </td>
                            <td> <asp:TextBox ID="브랜드수정_브랜드명" runat="server" SkinID="tb1" MaxLength="100" Width="150px"></asp:TextBox> </td>
                        </tr>
                        <tr>
                            <td colspan="2"> * 브랜드를 삭제하는 경우 자판기에 신규로 추가할수 없으며 기존 상품은 그대로 유지 됩니다. </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;padding-bottom:20px;">
                    <div runat="server" id="브랜드수정_결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt브랜드수정완료" runat="server" OnClick="bt브랜드수정완료_Click" CausesValidation="false" Text="수정하기" SkinID="bt1" />
                    <asp:Button ID="bt브랜드삭제완료" runat="server" OnClick="bt브랜드삭제완료_Click" CausesValidation="false" Text="삭제하기" SkinID="bt3" OnClientClick="return confirm('삭제하시겠습니까? 하위 데이터가 있는경우 삭제되지 않을 수 있습니다. '); " />
                    <asp:Button ID="bt브랜드수정닫기" runat="server" OnClick="bt브랜드수정닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>

    <div id="div_상품등록" runat="server" visible="false" style="position:absolute;top:0px;left:330px;border:5px solid #aaa;background:#151521; padding:5px 5px 5px 5px" >
        <table cellpadding="1" cellspacing="1" style="width:400px;background:#151521;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 상품(신규등록)
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 브랜드 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="상품등록_브랜드코드"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 상품명 : </td>
                            <td><asp:TextBox ID="상품등록_상품명" runat="server" SkinID="tb1" MaxLength="30" Width="200px"></asp:TextBox> 
                        </tr>
                        <tr>
                            <td> 투출타입 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="상품등록_투출타입">
                                    <asp:ListItem Value="투출"></asp:ListItem>
                                    <asp:ListItem Value="전시"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 단위 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="상품등록_단위">
                                    <asp:ListItem Value="pcs" Text="낱개"></asp:ListItem>
                                    <asp:ListItem Value="box" Text="박스"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 비고 : </td>
                            <td>
                                <asp:TextBox ID="상품등록_비고" runat="server" SkinID="tb1" MaxLength="100" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 이미지 :</td>
                            <td>
                                <asp:FileUpload ID="상품등록_이미지" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><div>(이미지 권장사이즈: 160px x 160px .png)</div></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;padding-bottom:20px;">
                    <div runat="server" id="상품등록_결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>                    
                    <asp:Button ID="bt상품등록완료" runat="server" OnClick="bt상품등록완료_Click" CausesValidation="false" Text="등록하기" SkinID="bt1" />
                    <asp:Button ID="bt상품등록닫기" runat="server" OnClick="bt상품등록닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
      
    <div id="div_상품수정" runat="server" visible="false" style="position:absolute;top:0px;left:330px;border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px" >
        <table cellpadding="0" cellspacing="0" style="width:400px;background:#d6d6d6;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 상품(수정/삭제)
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 상품코드 : </td>
                            <td>
                                <asp:Label ID="상품수정_상품코드" runat="server" Text=""></asp:Label> 
                            </td>
                        </tr>
                        <tr>
                            <td> 브랜드 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="상품수정_브랜드코드"></asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td> 상품명 : </td>
                            <td><asp:TextBox ID="상품수정_상품명" runat="server" SkinID="tb1" MaxLength="30" Width="200px"></asp:TextBox> 
                        </tr>
                        <tr>
                            <td> 투출타입 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="상품수정_투출타입">
                                    <asp:ListItem Value="투출"></asp:ListItem>
                                    <asp:ListItem Value="전시"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 단위 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="상품수정_단위">
                                    <asp:ListItem Value="pcs" Text="낱개"></asp:ListItem>
                                    <asp:ListItem Value="box" Text="박스"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 비고 : </td>
                            <td>
                                <asp:TextBox ID="상품수정_비고" runat="server" SkinID="tb1" MaxLength="100" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 이미지 :</td>
                            <td>
                                <asp:FileUpload ID="상품수정_이미지" runat="server" Width="200px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;padding-bottom:20px;">
                    <div runat="server" id="div_상품수정결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt상품삭제완료" runat="server" OnClick="bt상품삭제완료_Click" CausesValidation="false" Text="삭제하기" SkinID="bt2" OnClientClick="return confirm('정말 삭제하시겠습니까?');" />
                    <asp:Button ID="bt상품수정완료" runat="server" OnClick="bt상품수정완료_Click" CausesValidation="false" Text="수정하기" SkinID="bt1" />
                    <asp:Button ID="bt상품수정닫기" runat="server" OnClick="bt상품수정닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
      
    <h2>
        &nbsp 기본 브랜드/상품관리
        <asp:TextBox ID="tbexcel" runat="server" SkinID="tb1" Visible="false"></asp:TextBox>

        <asp:TextBox ID="선택_브랜드코드" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="선택_상품코드" runat="server" Visible="false"></asp:TextBox>
    </h2>
    <div class="formlayout">

        <table>
            <tr>
                <td style="vertical-align:top">
                    <div style="padding:5px">

                        * 브랜드 <asp:Button ID="bt브랜드등록" runat="server" Text="브랜드등록" SkinID="bt2" OnClick="bt브랜드등록_Click" />
                        <asp:Button ID="bt새로고침" runat="server" Text="새로고침" SkinID="bt1" OnClick="bt새로고침_Click" />

                    </div>
                    <asp:GridView ID="gv브랜드" runat="server"
                        AutoGenerateColumns="false" GridLines="None" EnableTheming="True" DataKeyNames="idx"
                        CellPadding="4" SkinID="GridView1"
                        RowStyle-HorizontalAlign="Center" OnRowCommand="gv브랜드_RowCommand"
                        AllowPaging="false" EmptyDataText="브랜드 없음"  
                        width="250px"
                        >
                        <Columns>
                            <asp:TemplateField HeaderText="브랜드명" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt2"  Text='<%# Eval("brand_name") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="cnt" HeaderText="상품" />
                            <asp:TemplateField HeaderText="수정" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt_up"  Text="수정" CommandName="brand_up" CommandArgument='<%# Eval("idx") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        </asp:GridView>
                </td>
                <td style="vertical-align:top">
                    <div style="padding:5px">
                        * 상품 <asp:Button ID="bt상품등록" runat="server" Text="상품등록" SkinID="bt2" OnClick="bt상품등록_Click" />
                    </div>
                        <asp:GridView ID="gv상품" runat="server"
                            AutoGenerateColumns="false" GridLines="None" EnableTheming="True"
                            CellPadding="4" SkinID="GridView1"
                            RowStyle-HorizontalAlign="Center" OnRowCommand="gv상품_RowCommand"
                            AllowPaging="false" EmptyDataText="상품 없음"  
                            width="600px"
                            >
                            <Columns>
                                <asp:TemplateField HeaderText="상품명" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" id="bt2"  Text='<%# Eval("name") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="unit" HeaderText="단위" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" Width="100px" ImageUrl='<%# "data:image/png;base64,"+Eval("imagebase64_s") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="imagesize" HeaderText="용량(Kb)" />
                                <asp:BoundField DataField="comt" HeaderText="비고" />
                                <asp:TemplateField HeaderText="수정" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" id="bt_up"  Text="수정" CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                </td>
            </tr>
        </table>

    </div>

  </asp:Content>
