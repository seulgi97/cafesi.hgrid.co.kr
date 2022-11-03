using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HomeSiteMaster : System.Web.UI.MasterPage
{
    SgFramework.SgUtil su = new SgFramework.SgUtil();
    protected void Page_Load(object sender, EventArgs e)
    {
        String AccessClientip = su.GetIP(this.Page);
        //String cnt = su.SqlFieldQuery("select COUNT(*) from tbl_접속허용IP where 허용시작IP <= dbo.f_IP변환('" + AccessClientip + "') and 허용종료IP >= dbo.f_IP변환('" + AccessClientip + "') and 만료일시 >= convert(varchar(10),GETDATE(),121) ");
        //if (cnt == "0")
        //{
        //    Response.Redirect("/Error/dnserror.aspx");
        //}
    }
}
