using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;

public partial class Sample_차트샘플 : System.Web.UI.Page
{

    SgFramework.SgUtil su = new SgFramework.SgUtil();
    
    string AgencyID = "";
    string UserID = "";
    ConnectionStringSettings cts = ConfigurationManager.ConnectionStrings["ANNEXConnectionString"];
    int adminChk = 0;
    String SortString = "id";

    protected void Page_Load(object sender, EventArgs e)
    {
        //su.PermitPage(this.Page, "460", true);

        if (!IsPostBack)
        {
            Button1_Click(null, null);
        }
    }

    // 검색 처리 내역
    protected void Button1_Click(object sender, EventArgs e)
    {

        
    }


}