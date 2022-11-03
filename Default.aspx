<%@ Page Title="홈 페이지" Language="C#" MasterPageFile="~/Master/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" Theme="Main" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script type="text/javascript" src="/amcharts/amcharts.js"></script>
<script type="text/javascript" src="/amcharts/serial.js"></script>
<script src="/amcharts/pie.js" type="text/javascript"></script>
<script src="/amcharts/radar.js" type="text/javascript"></script>
    <style>
        .tb th { padding:10px; font-size:13px; font-weight:bold; border-bottom:1px solid #333; background-color:#3B77AF; color:white;}
        .tb td { padding:10px; font-size:14px; border:0px solid #333; text-align:center; }
        .tb2 th { padding:10px; font-size:12px; font-weight:bold; border-bottom:1px solid #333; background-color:#3B77AF; color:white;}
        .tb2 td { padding:3px; font-size:12px; border:0px solid #333; text-align:center; }
    </style>
    <h2>
        &nbsp;매출 요약 현황 
    </h2>

    <table style="border:0px; width:90%;" class="tb" cellspacing="0" cellpadding="0">
        <tr>
            <th>당일판매</th>
            <th><%=DateTime.Now.AddMonths(-2).ToString("MM") %>월판매 <span style="font-size:11px;">(1~말일)</span></th>
            <th><%=DateTime.Now.AddMonths(-1).ToString("MM") %>월판매 <span style="font-size:11px;">(1~말일)</span></th>
            <th><%=DateTime.Now.AddMonths(0).ToString("MM") %>월판매 <span style="font-size:11px;">(1~당일)</span></th>
            <th><%=DateTime.Now.AddMonths(-2).ToString("MM") %>월일평균 <span style="font-size:11px;">(1~말일)</span></th>
            <th><%=DateTime.Now.AddMonths(-1).ToString("MM") %>월일평균 <span style="font-size:11px;">(1~말일)</span></th>
            <th><%=DateTime.Now.AddMonths(0).ToString("MM") %>월일평균 <span style="font-size:11px;">(1~당일)</span></th>
        </tr>
        <tr>
            <td><asp:Label ID="lb당일판매건수" runat="server" Text=""></asp:Label> 건</td>
            <td><asp:Label ID="lb전전월판매건수" runat="server" Text=""></asp:Label> 건
            </td>
            <td><asp:Label ID="lb전월판매건수" runat="server" Text=""></asp:Label> 건
            </td>
            <td><asp:Label ID="lb당월판매건수" runat="server" Text=""></asp:Label> 건
            </td>
            <td>
                <asp:Label ID="lb전전월일평균건수" runat="server" Text=""></asp:Label> 건
            </td>
            <td>
                <asp:Label ID="lb전월일평균건수" runat="server" Text=""></asp:Label> 건
            </td>
            <td>
                <asp:Label ID="lb당월일평균건수" runat="server" Text=""></asp:Label> 건
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lb당일판매금액" runat="server" Text=""></asp:Label> 원</td>
            <td><asp:Label ID="lb전전월판매금액" runat="server" Text=""></asp:Label> 원</td>
            <td><asp:Label ID="lb전월판매금액" runat="server" Text=""></asp:Label> 원</td>
            <td><asp:Label ID="lb당월판매금액" runat="server" Text=""></asp:Label> 원</td>
            <td><asp:Label ID="lb전전월일평균금액" runat="server" Text=""></asp:Label> 원</td>
            <td><asp:Label ID="lb전월일평균금액" runat="server" Text=""></asp:Label> 원</td>
            <td><asp:Label ID="lb당월일평균금액" runat="server" Text=""></asp:Label> 원</td>
        </tr>

    </table>
    <br />
    <h2>
        &nbsp;1년동안 매출 추이
    </h2>

    <div style="position:relative;width:90%;  ">
        <div style="width:100%; padding-top:10px; padding-bottom:10px;">
                <span class="f13">* 최근 1년간 매출 추이</span><span style="color:#ff6a00; font-size:15px; font-weight:bold;">
                    - 당월예상매출액 : <asp:Label ID="lb당월예상판매건수" runat="server" Text=""></asp:Label>건 <asp:Label ID="lb당월예상판매금액" runat="server" Text=""></asp:Label>원 
                </span>
        </div>
        <% if (arr1년판매추이 != "") { %>
            <div id="chartdiv13" style="width:100%; height:200px;"></div>
      
            <script type="text/javascript">
                    var chart13;

                    var chartData13 = [
                            <%=arr1년판매추이 %>
                    ];

                AmCharts.ready(function () {

                    // SERIAL CHART
                    chart13 = new AmCharts.AmSerialChart();
                    chart13.marginLeft = 0;
                    chart13.marginRight = 0;
                    chart13.marginTop = 0;
                    chart13.dataProvider = chartData13;
                    chart13.categoryField = "date2";

                    // AXES
                    // category
                    var categoryAxis = chart13.categoryAxis;

                    // value axis
                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.inside = true;
                    valueAxis.tickLength = 0;
                    valueAxis.axisAlpha = 0;
                    chart13.addValueAxis(valueAxis);

                    // GRAPH
                    var graph = new AmCharts.AmGraph();
                    graph.dashLength = 12;
                    graph.lineColor = "#7717D7";
                    graph.balloonText = "<span style='font-size:14px'>[[date]]<br>[[value]]원 (<b>[[cnt]]건</b>)</span>";
                    graph.valueField = "visits";
                    graph.bullet = "round";
                    chart13.addGraph(graph);
                        
                    // CURSOR
                    var chartCursor = new AmCharts.ChartCursor();
                    chartCursor.cursorAlpha = 0;
                    chart13.addChartCursor(chartCursor);
                
                    // WRITE
                    chart13.write("chartdiv13");
                });

            </script>
        <% } %>
    </div>
    <% if(dv1년간판매추이!=null && dv1년간판매추이.Count>0){ %>
    <table style="border:0px; width:90%;" class="tb2" cellspacing="0" cellpadding="0">
        <tr>
            <th>항목</th>
            <% for(int i=0;i<dv1년간판매추이.Count;i++){ %>
            <th><%=dv1년간판매추이[i]["매출년월"].ToString().Substring(0,2)+"년 "+dv1년간판매추이[i]["매출월"].ToString()+"월" %></th>
            <% } %>
        </tr>
        <tr>
            <th>캡슐갯수</th>
            <% for(int i=0;i<dv1년간판매추이.Count;i++){ %>
            <td><%=su.FormatNumber(dv1년간판매추이[i]["총판매갯수"].ToString()) + " 건"%></td>
            <% } %>
        </tr>
        <tr>
            <th>결제금액</th>
            <% for(int i=0;i<dv1년간판매추이.Count;i++){ %>
            <td><%=su.FormatNumber(dv1년간판매추이[i]["총결제금액"].ToString()) + " 원"%></td>
            <% } %>
        </tr>
        <tr>
            <th>가동률</th>
            <% 
                for(int i=0;i<dv1년간가동율.Count;i++){ 
                   int 당월전체날짜수 = Convert.ToInt32(Convert.ToDateTime("20"+dv1년간가동율[i]["가동월"].ToString() + "-01").AddMonths(1).AddDays(-1).ToString("dd")); 
            %>
            <td><%=( Math.Round(Convert.ToDouble(dv1년간가동율[i]["가동일"].ToString())/Convert.ToDouble(당월전체날짜수) * 100.0, 2)).ToString()  %>%</td>
            <% } %>
        </tr>

    </table>
    <%} %>
    <br />
</asp:Content>
