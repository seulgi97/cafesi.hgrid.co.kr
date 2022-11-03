using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Menu_기본 : System.Web.UI.Page
{
    SgFramework.SgUtil su = new SgFramework.SgUtil();

    public String idx = "";
    public String userid = "";
    public String level = "";
    public String passwd = "";
    public String usrtel = "";
    public String company_code = "";
    public String agency_code = "";
    public String branch_code = "";
    public String pos_number = "";
    public String usrorg = "";
    public String grp_code = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        idx = su.ReqS(this.Page, "idx");
        userid = su.ReqS(this.Page, "userid");
        level = su.ReqS(this.Page, "level");
        passwd = su.ReqS(this.Page, "passwd");
        usrtel = su.ReqS(this.Page, "usrtel");
        company_code = su.ReqS(this.Page, "company_code");
        agency_code = su.ReqS(this.Page, "agency_code");
        branch_code = su.ReqS(this.Page, "branch_code");
        pos_number = su.ReqS(this.Page, "pos_number");
        usrorg = su.ReqS(this.Page, "usrorg");
        grp_code = su.ReqS(this.Page, "grp_code");

        if (!IsPostBack)
        {
        }
    }
}
