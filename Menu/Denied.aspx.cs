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

public partial class Menu_Denied : System.Web.UI.Page
{

    SgFramework.SgUtil su = new SgFramework.SgUtil();
    String AgencyID = "";
    String UserID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

}