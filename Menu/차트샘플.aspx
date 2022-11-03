<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Site.master" AutoEventWireup="true" CodeFile="차트샘플.aspx.cs" Inherits="Sample_차트샘플"  EnableEventValidation="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
  <h2> 차트샘플 </h2>
    
    <table>
        <tr>
            <td>

              <script type="text/javascript" language="JavaScript" src="/flashchart/JSClass/FusionCharts.js"></script>
              <div id="chartdiv1" align="center">  </div>
              <script type="text/javascript">
                  // 차트 종류 선택 가능
                  var chart = new FusionCharts("/flashchart/Charts/Column2D.swf", "ChartId", "260", "250", "0", "0");
                  // 차트 데이터는 반드시 /flashchart/Data/ 폴더 안에 존재 하여야 한다.
                  chart.setDataURL("/flashchart/Data/Line2D.aspx");
                  // 차트를 보여줄 object명
                  chart.render("chartdiv1");
              </script>
            </td> 
            <td>
              <script type="text/javascript" language="JavaScript" src="/flashchart/JSClass/FusionCharts.js"></script>
              <div id="chartdiv2" align="center">  </div>
              <script type="text/javascript">
                  // 차트 종류 선택 가능
                  var chart = new FusionCharts("/flashchart/Charts/Column3D.swf", "ChartId", "260", "250", "0", "0");
                  // 차트 데이터는 반드시 /flashchart/Data/ 폴더 안에 존재 하여야 한다.
                  chart.setDataURL("/flashchart/Data/Line2D.aspx");
                  // 차트를 보여줄 object명
                  chart.render("chartdiv2");
              </script>
            </td>
            <td>
      
              <script type="text/javascript" language="JavaScript" src="/flashchart/JSClass/FusionCharts.js"></script>
              <div id="Div1" align="center">  </div>
              <script type="text/javascript">
                  // 차트 종류 선택 가능
                  var chart = new FusionCharts("/flashchart/Charts/Area2D.swf", "ChartId", "260", "250", "0", "0");
                  // 차트 데이터는 반드시 /flashchart/Data/ 폴더 안에 존재 하여야 한다.
                  chart.setDataURL("/flashchart/Data/Line2D.aspx");
                  // 차트를 보여줄 object명
                  chart.render("Div1");
              </script>
            </td>
        </tr>
        <tr>
            <td>
              <script type="text/javascript" language="JavaScript" src="/flashchart/JSClass/FusionCharts.js"></script>
              <div id="Div2" align="center">  </div>
              <script type="text/javascript">
                  // 차트 종류 선택 가능
                  var chart = new FusionCharts("/flashchart/Charts/Doughnut3D.swf", "ChartId", "260", "250", "0", "0");
                  // 차트 데이터는 반드시 /flashchart/Data/ 폴더 안에 존재 하여야 한다.
                  chart.setDataURL("/flashchart/Data/Line2D.aspx");
                  // 차트를 보여줄 object명
                  chart.render("Div2");
              </script>
            </td>
            <td>
              <script type="text/javascript" language="JavaScript" src="/flashchart/JSClass/FusionCharts.js"></script>
              <div id="Div3" align="center">  </div>
              <script type="text/javascript">
                  // 차트 종류 선택 가능
                  var chart = new FusionCharts("/flashchart/Charts/Pie3D.swf", "ChartId", "260", "250", "0", "0");
                  // 차트 데이터는 반드시 /flashchart/Data/ 폴더 안에 존재 하여야 한다.
                  chart.setDataURL("/flashchart/Data/Line2D.aspx");
                  // 차트를 보여줄 object명
                  chart.render("Div3");
              </script>
            </td>
            <td>
              <script type="text/javascript" language="JavaScript" src="/flashchart/JSClass/FusionCharts.js"></script>
              <div id="Div4" align="center">  </div>
              <script type="text/javascript">
                  // 차트 종류 선택 가능
                  var chart = new FusionCharts("/flashchart/Charts/Line.swf", "ChartId", "260", "250", "0", "0");
                  // 차트 데이터는 반드시 /flashchart/Data/ 폴더 안에 존재 하여야 한다.
                  chart.setDataURL("/flashchart/Data/Line2D.aspx");
                  // 차트를 보여줄 object명
                  chart.render("Div4");
              </script>
            </td>
        </tr>
     </table>

    </p>

</asp:Content>

