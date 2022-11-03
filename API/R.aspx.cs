using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Json;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Collections;

public partial class API_R : System.Web.UI.Page
{
    SgFramework.SgUtil su = new SgFramework.SgUtil();

    public String sql = "";

    protected JsonObjectCollection j = null;
    protected JsonObjectCollection getData = null;
    protected JsonTextParser parser = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        String ip = su.GetIP(this.Page);
        String body = su.getRawBody(this.Page);
        String retVal = "";
        String eType = "";
        String machine_idx = "";
        String log_idx = "";
        String tmp = "";
        String clientip = su.GetIP(this.Page);

        j = new JsonObjectCollection();
        parser = new JsonTextParser();
        //getData = new JsonObjectCollection();

        //Response.ContentType = "application/json";
        //Response.Headers.Add("Content-type", "text/json");
        //Response.Headers.Add("Content-type", "application/json");
        //Response.Headers.Add("Content-type", "charset=utf-8");

        #region eType = 0 서버체크
        if (body == "")
        {
            j.Add(new JsonStringValue("RETCODE", "0"));
            j.Add(new JsonStringValue("RETMSG", "정상"));
            j.Add(new JsonStringValue("eType", "0"));

            Response.Write(j.ToString());
            return;
        }
        #endregion

        #region body 체크 
        if (body == "")
        {
            j.Clear();

            j.Add(new JsonStringValue("RETCODE", "-4"));
            j.Add(new JsonStringValue("RETMSG", "body 값이 없습니다"));

            Response.Write(j.ToString());
            return;
        }

        try
        {
            getData = (JsonObjectCollection)parser.Parse(body);

        }
        catch(Exception ex)
        {
            j.Clear();

            j.Add(new JsonStringValue("RETCODE", "-3"));
            j.Add(new JsonStringValue("RETMSG", ex.Message.ToString() +"-"+ body));

            Response.Write(j.ToString());
            return;
        }
        #endregion

        body = body.Replace(Environment.NewLine, "").Replace("\\","");
        eType = su.sg_db_query((String)getData["eType"].GetValue());
        machine_idx = su.Decrypt_AES(su.sg_db_query((String)getData["mid"].GetValue())).Trim();

        #region eType 체크 
        if (eType == "" || su.isNumeric(eType) == false)
        {
            j.Clear();

            j.Add(new JsonStringValue("RETCODE", "-1"));
            j.Add(new JsonStringValue("RETMSG", "eType 값은 숫자로 입력해야 합니다"));

            Response.Write(j.ToString());
            return;
        }
        #endregion

        #region 자판기번호체크 

        if (machine_idx == "" || su.isNumeric(machine_idx) == false)
        {
            j.Clear();

            j.Add(new JsonStringValue("RETCODE", "-2"));
            j.Add(new JsonStringValue("RETMSG", "mid(자판기번호) 값은 숫자로 입력해야 합니다"));

            Response.Write(j.ToString());
            return;
        }

        try
        {
            sql = "select count(*) from machine with(nolock) where idx='" + machine_idx + "' ";
            String mid_check = su.SqlFieldQuery(sql);

            if (mid_check != "1")
            {
                j.Clear();

                j.Add(new JsonStringValue("RETCODE", "-3"));
                j.Add(new JsonStringValue("RETMSG", "mid(자판기번호) 가 존재하지 않습니다"));

                Response.Write(j.ToString());
                return;
            }

        }catch(Exception ex)
        {
            j.Clear();

            j.Add(new JsonStringValue("RETCODE", "-3"));
            j.Add(new JsonStringValue("RETMSG", ex.Message.ToString() + sql));

            Response.Write(j.ToString());
            return;
        }
        #endregion

        #region machine_log 기록
        try
        {
            sql = "insert into machine_log(etype,getdata,machine_idx,clientip) values( '" + eType + "','"+su.sg_db_query(body) + "','" + machine_idx + "','" + ip + "'); select @@identity; ";
            log_idx = su.SqlFieldQuery(sql);
        }
        catch(Exception ex)
        {

        }
        #endregion

        #region 자판기 최종 보고시간 update

        sql = "update machine set 최종보고일시=getdate() where idx='" + machine_idx + "' ";
        su.SqlNoneQuery(sql);

        #endregion

        switch (eType)
        {

            case "1":
                #region 버젼체크
                try
                {
                    j.Clear();

                    String ver = su.sg_db_query((String)getData["ver"].GetValue());
                    String localip = su.sg_db_query((String)getData["localip"].GetValue());
                    String ctver = su.sg_db_query((String)getData["ctver"].GetValue());

                    sql = "update machine set 최종재부팅일시=getdate(),최종소프트웨어버젼='" + ver + "', 자판기공인IP='"+ clientip +"', 자판기로컬IP='"+ localip+"', 컨트롤러버젼='"+ ctver + "' ";
                    sql += " where idx='"+ machine_idx +"' ";
                    su.SqlNoneQuery(sql);

                    j.Add(new JsonStringValue("RETCODE", "0"));
                    j.Add(new JsonStringValue("RETMSG", "정상"));
                    j.Add(new JsonStringValue("newver", "1.0")); // 최신 버젼
                    j.Add(new JsonStringValue("newctver", "1.009.101.001.00")); // 최신 컨트롤러 버젼
                    j.Add(new JsonStringValue("glovalip", clientip));
                    j.Add(new JsonStringValue("eType", eType));

                    retVal = j.ToString();
                }
                catch(Exception ex)
                {
                    j.Clear();

                    j.Add(new JsonStringValue("RETCODE", "-99"));
                    j.Add(new JsonStringValue("RETMSG", ex.Message.ToString()));
                    j.Add(new JsonStringValue("eType", eType));

                    retVal = j.ToString();
                }
                #endregion
                break;

            case "2":
                #region 상품조회
                try
                {

                    sql = "update machine set 최종보고일시=getdate(),최종상품정보일시=getdate(),자판기공인IP='" + clientip + "' ";
                    sql += " where idx='" + machine_idx + "' ";
                    su.SqlNoneQuery(sql);

                    String prhash = su.sg_db_query((String)getData["prhash"].GetValue());
                    String usepridx = su.sg_db_query((String)getData["usepridx"].GetValue());
                    String updateyn = "N";
                    String prdata = "";

                    if (prhash == "") { updateyn = "Y"; }

                    j.Clear();

                    j.Add(new JsonStringValue("RETCODE", "0"));
                    j.Add(new JsonStringValue("RETMSG", "정상"));
                    j.Add(new JsonStringValue("eType", eType));


                    sql = "";
                    JsonArrayCollection arr_menu = new JsonArrayCollection();
                    JsonArrayCollection arr_brand = new JsonArrayCollection();
                    JsonArrayCollection arr_coffee = new JsonArrayCollection();
                    DataView dv = null;

                    #region 메뉴 추출

                    sql = "select distinct d.idx,d.name,d.dno,b.pr_type  ";
                    sql += " from machine_menu a with(nolock) ";
                    sql += " left outer join coffee b with(nolock) on a.coffee_idx = b.idx ";
                    sql += " left outer join coffee_brand c with(nolock) on a.coffee_brand_idx = c.idx ";
                    sql += " left outer join coffee_menu d with(nolock) on a.coffee_menu_idx = d.idx ";
                    sql += " where a.machine_idx = '"+ machine_idx+"' and ( a.isusing = 1 ";
                    if (usepridx != "")
                    {
                        sql += " or a.coffee_idx in (" + usepridx + ") ";
                    }
                    sql += " ) ";
                    dv = su.SqlDvQuery(sql);
                    
                    for (int i=0;i<dv.Count;i++)
                    {
                        JsonObjectCollection col = new JsonObjectCollection();
                        col.Add(new JsonStringValue("idx", dv[i]["idx"].ToString()));
                        col.Add(new JsonStringValue("name", dv[i]["name"].ToString()));
                        col.Add(new JsonStringValue("dno", dv[i]["dno"].ToString()));
                        col.Add(new JsonStringValue("pr_type", dv[i]["pr_type"].ToString()));
                        arr_menu.Add(col);
                    }

                    prdata += arr_menu.ToString();

                    #endregion

                    #region 브랜드 추출


                    sql = "select distinct c.idx,c.brand_name name,b.pr_type  ";
                    sql += " from machine_menu a with(nolock) ";
                    sql += " left outer join coffee b with(nolock) on a.coffee_idx = b.idx ";
                    sql += " left outer join coffee_brand c with(nolock) on a.coffee_brand_idx = c.idx ";
                    sql += " left outer join coffee_menu d with(nolock) on a.coffee_menu_idx = d.idx ";
                    sql += " where a.machine_idx = '" + machine_idx + "' and ( a.isusing = 1 ";
                    if (usepridx != "")
                    {
                        sql += " or a.coffee_idx in (" + usepridx + ") ";
                    }
                    sql += " ) ";
                    dv = su.SqlDvQuery(sql);

                    for (int i = 0; i < dv.Count; i++)
                    {
                        JsonObjectCollection col = new JsonObjectCollection();
                        col.Add(new JsonStringValue("idx", dv[i]["idx"].ToString()));
                        col.Add(new JsonStringValue("name", dv[i]["name"].ToString()));
                        col.Add(new JsonStringValue("pr_type", dv[i]["pr_type"].ToString()));
                        arr_brand.Add(col);
                    }


                    prdata += arr_brand.ToString();

                    #endregion

                    #region 상품 추출

                    sql = "select b.idx, b.name, b.imagebase64, b.pr_type,a.coffee_menu_idx,a.coffee_brand_idx  ";
                    sql += " from machine_menu a with(nolock) ";
                    sql += " left outer join coffee b with(nolock) on a.coffee_idx = b.idx ";
                    sql += " left outer join coffee_brand c with(nolock) on a.coffee_brand_idx = c.idx ";
                    sql += " left outer join coffee_menu d with(nolock) on a.coffee_menu_idx = d.idx ";
                    sql += " where a.machine_idx = '" + machine_idx + "' and ( a.isusing = 1 ";
                    if (usepridx != "")
                    {
                        sql += " or a.coffee_idx in (" + usepridx + ") ";
                    }
                    sql += " ) ";

                    dv = su.SqlDvQuery(sql);

                    for (int i = 0; i < dv.Count; i++)
                    {
                        JsonObjectCollection col = new JsonObjectCollection();
                        col.Add(new JsonStringValue("idx", dv[i]["idx"].ToString()));
                        col.Add(new JsonStringValue("brand_idx", dv[i]["coffee_brand_idx"].ToString()));
                        col.Add(new JsonStringValue("menu_idx", dv[i]["coffee_menu_idx"].ToString()));
                        col.Add(new JsonStringValue("name", dv[i]["name"].ToString()));
                        col.Add(new JsonStringValue("imagebase64", dv[i]["imagebase64"].ToString()));
                        col.Add(new JsonStringValue("pr_type", dv[i]["pr_type"].ToString()));
                        arr_coffee.Add(col);
                    }


                    prdata += arr_coffee.ToString();

                    #endregion

                    byte[] byteArray = Encoding.UTF8.GetBytes(prdata);
                    MemoryStream stream1 = new MemoryStream(byteArray);
                    string chksum = BitConverter.ToString(System.Security.Cryptography.SHA1.Create().ComputeHash(stream1)).Replace("-","");

                    if (chksum != prhash)
                    {
                        updateyn = "Y";
                    }

                    j.Add(new JsonStringValue("UPDATEYN", updateyn));
                    j.Add(new JsonStringValue("PRHASH", chksum));

                    if (updateyn == "Y")
                    {
                        j.Add(new JsonArrayCollection("pr_menu", arr_menu));
                        j.Add(new JsonArrayCollection("pr_brand", arr_brand));
                        j.Add(new JsonArrayCollection("pr", arr_coffee));
                    }
                    else
                    {
                        arr_menu.Clear();
                        arr_brand.Clear();
                        arr_coffee.Clear();
                        j.Add(new JsonArrayCollection("pr_menu", arr_menu));
                        j.Add(new JsonArrayCollection("pr_brand", arr_brand));
                        j.Add(new JsonArrayCollection("pr", arr_coffee));
                    }

                    retVal = j.ToString();
                }
                catch (Exception ex)
                {
                    j.Clear();

                    j.Add(new JsonStringValue("RETCODE", "-99"));
                    j.Add(new JsonStringValue("RETMSG", ex.Message.ToString()));
                    j.Add(new JsonStringValue("eType", eType));

                    retVal = j.ToString();
                }
                #endregion
                break;

            case "3":
                #region 결제보고

                String pay_idx = "";
                try
                {

                    j.Clear();

                    String pay_id = su.sg_db_query((String)getData["pay_id"].GetValue());
                    String total_amount = su.sg_db_query((String)getData["total_amount"].GetValue());
                    String coupon_use_yn = su.sg_db_query((String)getData["coupon_use_yn"].GetValue());
                    String coupon_no = su.Decrypt_AES(su.sg_db_query((String)getData["coupon_num"].GetValue()));
                    String coupon_amount = su.sg_db_query((String)getData["coupon_amount"].GetValue());
                    String pay_amount = su.sg_db_query((String)getData["pay_amount"].GetValue());
                    String pay_type = su.sg_db_query((String)getData["pay_type"].GetValue());
                    String pay_tid = su.sg_db_query((String)getData["pay_tid"].GetValue());
                    String pay_result = su.sg_db_query((String)getData["pay_result"].GetValue());
                    String pay_result_msg = su.sg_db_query((String)getData["pay_result_msg"].GetValue());
                    String pay_result_no = su.sg_db_query((String)getData["pay_result_no"].GetValue());
                    String pay_date = su.sg_db_query((String)getData["pay_date"].GetValue());
                    String out_result = su.sg_db_query((String)getData["out_result"].GetValue());

                    if (pay_id == "")
                    {
                        j.Add(new JsonStringValue("RETCODE", "-2"));
                        j.Add(new JsonStringValue("RETMSG", "PAY_ID는 자판기 고유 결제 번호를 넣어주셔야 합니다."));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    sql = "select agency_idx,branch_idx from machine with(Nolock) where idx='" + machine_idx + "' ";
                    DataView dv2 = su.SqlDvQuery(sql);
                    String agency_idx = "0";
                    String branch_idx = "0";

                    if (dv2==null || dv2.Count==0)
                    {
                        j.Add(new JsonStringValue("RETCODE", "-1"));
                        j.Add(new JsonStringValue("RETMSG", "자판기번호 가 존재하지 않습니다"));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }
                    agency_idx = dv2[0]["agency_idx"].ToString();
                    branch_idx = dv2[0]["branch_idx"].ToString();

                    sql = "update machine set 최종보고일시=getdate() where idx='" + machine_idx + "' ";
                    su.SqlNoneQuery(sql);

                    sql = "select count(*) from machine_pay with(Nolock) where machine_idx='"+ machine_idx+"' and pay_id='"+ pay_id +"' ";
                    String chk = su.SqlFieldQuery(sql);
                    if (chk != "0")
                    {
                        j.Add(new JsonStringValue("RETCODE", "0"));
                        j.Add(new JsonStringValue("RETMSG", "정상(pay_id기보고)"));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    if (coupon_use_yn == "Y")
                    {
                        sql = "select 쿠폰번호, 발행금액, 최소구매금액, convert(varchar(10),만료일,121) 만료일,  Datediff(d, convert(varchar(10), getdate(), 121), 만료일) 잔여일자, case when Datediff(d, convert(varchar(10), getdate(), 121), 만료일) < 0 then 'Y' else 'N' end [만료여부] , convert(varchar(19),usedate,121) usedate,useyn, machine_idx ";
                        sql += " from coupon with(nolock) where isusing= 1 and 쿠폰번호 ='" + coupon_no + "' ";
                        DataView dv = su.SqlDvQuery(sql);

                        if (dv == null || dv.Count == 0)
                        {
                            j.Add(new JsonStringValue("RETCODE", "-2"));
                            j.Add(new JsonStringValue("RETMSG", "사용할 수 없는 쿠폰번호입니다"));
                            j.Add(new JsonStringValue("eType", eType));
                            retVal = j.ToString();
                            break;
                        }
                    }

                    sql = "insert into machine_pay ( machine_idx,agency_idx,branch_idx,pay_id,total_amount,coupon_use_yn,coupon_num,coupon_amount ";
                    sql += " ,pay_amount,pay_type,pay_tid,pay_result,pay_result_msg,pay_result_no,pay_date,out_result,client_ip ) values(";
                    sql += " '" + machine_idx + "' ";
                    sql += " ,'" + agency_idx + "' ";
                    sql += " ,'" + branch_idx + "' ";
                    sql += " ,'" + pay_id + "' ";
                    sql += " ,'" + total_amount + "' ";
                    sql += " ,'" + coupon_use_yn + "' ";
                    sql += " ,'" + coupon_no + "' ";
                    sql += " ,'" + coupon_amount + "' ";
                    sql += " ,'" + pay_amount + "' ";
                    sql += " ,'" + pay_type + "' ";
                    sql += " ,'" + pay_tid + "' ";
                    sql += " ,'" + pay_result + "' ";
                    sql += " ,'" + pay_result_msg + "' ";
                    sql += " ,'" + pay_result_no + "' ";
                    sql += " ,'" + pay_date + "' ";
                    sql += " ,'" + out_result + "' ";
                    sql += " ,'" + clientip + "'); select @@identity ";
                    pay_idx = su.SqlFieldQuery(sql);

                    if (pay_idx == "")
                    {
                        j.Clear();

                        j.Add(new JsonStringValue("RETCODE", "-99"));
                        j.Add(new JsonStringValue("RETMSG", "결제보고 정보를 다시 확인해주세요 "));
                        j.Add(new JsonStringValue("eType", eType));
                        j.Add(new JsonStringValue("body", body));

                        retVal = j.ToString();
                        break;
                    }

                    if (su.CntBytesLen(out_result) > 20)
                    {
                        sql = "update machine set machine_stat='" + out_result + "', machine_stat_date=getdate() where idx='" + machine_idx + "' ";
                        su.SqlNoneQuery(sql);
                    }
                    String pr_ok = "Y";

                    JsonArrayCollection pay_list = new JsonArrayCollection();
                    JsonArrayCollection collection = (JsonArrayCollection)getData["pay_list"];
                    for(int i = 0; i < collection.Count; i++)
                    {
                        JsonObjectCollection row = (JsonObjectCollection)parser.Parse(collection[i].ToString());
                        String pr_idx = (String)row["pr_idx"].GetValue();
                        String menu_idx = (String)row["menu_idx"].GetValue();
                        String brand_idx = (String)row["brand_idx"].GetValue();
                        String amount = (String)row["amount"].GetValue();
                        String ice_yn = (String)row["ice_yn"].GetValue();
                        String ice_amount = (String)row["ice_amount"].GetValue();
                        String pr_cnt = (String)row["pr_cnt"].GetValue();
                        String pr_total_amount = (String)row["total_amount"].GetValue();

                        sql = "insert into machine_pay_pr(pay_idx,pr_idx,menu_idx,brand_idx,amount,ice_yn,ice_amount,pr_cnt,pr_total_amount) values(";
                        sql += " '" + pay_idx + "' ";
                        sql += " ,'" + pr_idx + "' ";
                        sql += " ,'" + menu_idx + "' ";
                        sql += " ,'" + brand_idx + "' ";
                        sql += " ,'" + amount + "' ";
                        sql += " ,'" + ice_yn + "' ";
                        sql += " ,'" + ice_amount + "' ";
                        sql += " ,'" + pr_cnt + "' ";
                        sql += " ,'" + pr_total_amount + "' ";
                        sql += "); ";
                        su.SqlNoneQuery(sql);
                    }

                    if (pay_result == "00")
                    {
                        sql = "update machine set 최종결제일시=getdate(),자판기공인IP='" + clientip + "' ";
                        sql += " where idx='" + machine_idx + "' ";
                        su.SqlNoneQuery(sql);
                    }

                    j.Add(new JsonStringValue("RETCODE", "0"));
                    j.Add(new JsonStringValue("RETMSG", "정상"));
                    j.Add(new JsonStringValue("eType", eType));
                    retVal = j.ToString();
                }
                catch (Exception ex)
                {
                    sql = "delete from machine_pay where idx='"+ pay_idx+"'; delete from machine_pay_pr where pay_idx='"+ pay_idx +"'; ";
                    su.SqlNoneQuery(sql);

                    j.Clear();

                    j.Add(new JsonStringValue("RETCODE", "-99"));
                    j.Add(new JsonStringValue("RETMSG", ex.Message.ToString() ));
                    j.Add(new JsonStringValue("eType", eType));
                    j.Add(new JsonStringValue("body", body));

                    retVal = j.ToString();
                }
                #endregion
                break;

            case "4":
                #region 쿠폰조회
                try
                {
                    j.Clear();

                    String coupon_no = su.Decrypt_AES(su.sg_db_query((String)getData["coupon_no"].GetValue()));
                    String total_amount = su.sg_db_query((String)getData["total_amount"].GetValue());

                    sql = "update machine set 최종보고일시=getdate() where idx='" + machine_idx + "' ";
                    su.SqlNoneQuery(sql);

                    sql = "select 쿠폰번호, 발행금액, 최소구매금액, convert(varchar(10),만료일,121) 만료일,  Datediff(d, convert(varchar(10), getdate(), 121), 만료일) 잔여일자, case when Datediff(d, convert(varchar(10), getdate(), 121), 만료일) < 0 then 'Y' else 'N' end [만료여부] , convert(varchar(19),usedate,121) usedate,useyn, machine_idx ";
                    sql += " from coupon with(nolock) where isusing= 1 and 쿠폰번호 ='" + coupon_no + "' ";
                    DataView dv = su.SqlDvQuery(sql);

                    if (dv==null || dv.Count==0)
                    {
                        j.Add(new JsonStringValue("RETCODE", "-2"));
                        j.Add(new JsonStringValue("RETMSG", "사용할 수 없는 쿠폰번호입니다. 쿠폰번호를 확인해주세요"));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    if (dv[0]["만료여부"].ToString() == "Y")
                    {
                        j.Add(new JsonStringValue("RETCODE", "-2"));
                        j.Add(new JsonStringValue("RETMSG", dv[0]["만료일"].ToString() +"일에 만료된 쿠폰번호입니다."));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    if (dv[0]["useyn"].ToString() == "Y")
                    {
                        j.Add(new JsonStringValue("RETCODE", "-1"));
                        j.Add(new JsonStringValue("RETMSG", dv[0]["usedate"].ToString()+"에 이미 사용된 쿠폰입니다"));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    if (dv[0]["machine_idx"].ToString() != machine_idx)
                    {
                        j.Add(new JsonStringValue("RETCODE", "-3"));
                        j.Add(new JsonStringValue("RETMSG", "해당 쿠폰은 발행된 가게에서만 사용가능합니다"));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    if (su.sg_int(dv[0]["최소구매금액"].ToString()) > su.sg_int(total_amount) )
                    {
                        j.Add(new JsonStringValue("RETCODE", "-4"));
                        j.Add(new JsonStringValue("RETMSG", "해당 쿠폰은 "+ dv[0]["최소구매금액"].ToString()+"원 이상 구매하여야 사용할 수 있습니다"+ total_amount));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    j.Add(new JsonStringValue("RETCODE", "0"));
                    j.Add(new JsonStringValue("RETMSG", "정상"));
                    j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES(dv[0]["발행금액"].ToString()) ));
                    j.Add(new JsonStringValue("eType", eType));
                    retVal = j.ToString();
                }
                catch (Exception ex)
                {
                    j.Clear();

                    j.Add(new JsonStringValue("RETCODE", "-99"));
                    j.Add(new JsonStringValue("RETMSG", ex.Message.ToString()));
                    j.Add(new JsonStringValue("eType", eType));

                    retVal = j.ToString();
                }
                #endregion
                break;

            case "5":
                #region 쿠폰사용
                try
                {
                    j.Clear();

                    String coupon_no = su.Decrypt_AES( su.sg_db_query((String)getData["coupon_no"].GetValue()) );
                    String pay_id = su.sg_db_query((String)getData["pay_id"].GetValue());
                    String total_amount = su.sg_db_query((String)getData["total_amount"].GetValue());

                    sql = "update machine set 최종보고일시=getdate() where idx='" + machine_idx + "' ";
                    su.SqlNoneQuery(sql);

                    sql = "select 쿠폰번호, 발행금액, 최소구매금액, convert(varchar(10),만료일,121) 만료일,  Datediff(d, convert(varchar(10), getdate(), 121), 만료일) 잔여일자, case when Datediff(d, convert(varchar(10), getdate(), 121), 만료일) < 0 then 'Y' else 'N' end [만료여부] , convert(varchar(19),usedate,121) usedate,useyn, machine_idx ";
                    sql += " from coupon with(nolock) where isusing= 1 and 쿠폰번호 ='" + coupon_no + "' ";
                    DataView dv = su.SqlDvQuery(sql);

                    if (dv == null || dv.Count == 0)
                    {
                        j.Add(new JsonStringValue("RETCODE", "-2"));
                        j.Add(new JsonStringValue("RETMSG", "사용할 수 없는 쿠폰번호입니다. 쿠폰번호를 확인해주세요"));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    if (dv[0]["만료여부"].ToString() == "Y")
                    {
                        j.Add(new JsonStringValue("RETCODE", "-2"));
                        j.Add(new JsonStringValue("RETMSG", dv[0]["만료일"].ToString() + "일에 만료된 쿠폰번호입니다."));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    if (dv[0]["useyn"].ToString() == "Y")
                    {
                        j.Add(new JsonStringValue("RETCODE", "-1"));
                        j.Add(new JsonStringValue("RETMSG", dv[0]["usedate"].ToString() + "에 이미 사용된 쿠폰입니다"));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    if (dv[0]["machine_idx"].ToString() != machine_idx)
                    {
                        j.Add(new JsonStringValue("RETCODE", "-3"));
                        j.Add(new JsonStringValue("RETMSG", "해당 쿠폰은 발행된 가게에서만 사용가능합니다"));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    if (su.sg_int(dv[0]["최소구매금액"].ToString()) > su.sg_int(total_amount))
                    {
                        j.Add(new JsonStringValue("RETCODE", "-4"));
                        j.Add(new JsonStringValue("RETMSG", "해당 쿠폰은 " + dv[0]["최소구매금액"].ToString() + "원 이상 구매하여야 사용할 수 있습니다" + total_amount));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }
                    sql = "update coupon set usedate=getdate(),useyn='Y',pay_id='"+ pay_id +"'  where 쿠폰번호='"+ coupon_no + "' and machine_idx='"+ machine_idx+"' ";
                    if (su.SqlNoneQuery(sql) == true)
                    {
                        j.Add(new JsonStringValue("RETCODE", "0"));
                        j.Add(new JsonStringValue("RETMSG", "정상"));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES(dv[0]["발행금액"].ToString())));
                        j.Add(new JsonStringValue("eType", eType));
                    }
                    else
                    {
                        j.Add(new JsonStringValue("RETCODE", "-9"));
                        j.Add(new JsonStringValue("RETMSG", "서버 장애입니다.잠시후 다시 시도해 주세요"));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES(dv[0]["발행금액"].ToString())));
                        j.Add(new JsonStringValue("eType", eType));
                    }

                    retVal = j.ToString();
                }
                catch (Exception ex)
                {
                    j.Clear();

                    j.Add(new JsonStringValue("RETCODE", "-99"));
                    j.Add(new JsonStringValue("RETMSG", ex.Message.ToString()));
                    j.Add(new JsonStringValue("eType", eType));

                    retVal = j.ToString();
                }
                #endregion
                break;

            case "6":
                #region 쿠폰사용취소
                try
                {
                    j.Clear();

                    String coupon_no = su.Decrypt_AES( su.sg_db_query((String)getData["coupon_no"].GetValue()) );
                    String pay_id = su.sg_db_query((String)getData["pay_id"].GetValue());
                    String coupon_amount = su.sg_db_query((String)getData["coupon_amount"].GetValue());

                    sql = "update machine set 최종보고일시=getdate() where idx='" + machine_idx + "' ";
                    su.SqlNoneQuery(sql);

                    sql = "select 쿠폰번호, 발행금액, 최소구매금액, convert(varchar(10),만료일,121) 만료일,  Datediff(d, convert(varchar(10), getdate(), 121), 만료일) 잔여일자, case when Datediff(d, convert(varchar(10), getdate(), 121), 만료일) < 0 then 'Y' else 'N' end [만료여부] , convert(varchar(19),usedate,121) usedate,useyn, machine_idx ";
                    sql += " from coupon with(nolock) where isusing= 1 and 쿠폰번호 ='" + coupon_no + "' ";
                    DataView dv = su.SqlDvQuery(sql);

                    if (dv == null || dv.Count == 0)
                    {
                        j.Add(new JsonStringValue("RETCODE", "-2"));
                        j.Add(new JsonStringValue("RETMSG", "취소할 수 없는 쿠폰번호입니다"));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    if (dv[0]["만료여부"].ToString() == "Y")
                    {
                        j.Add(new JsonStringValue("RETCODE", "-2"));
                        j.Add(new JsonStringValue("RETMSG", dv[0]["만료일"].ToString() + "일에 만료된 쿠폰번호입니다."));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    if (dv[0]["useyn"].ToString() == "N")
                    {
                        j.Add(new JsonStringValue("RETCODE", "-1"));
                        j.Add(new JsonStringValue("RETMSG", "이미 취소된 쿠폰입니다"));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    if (dv[0]["machine_idx"].ToString() != machine_idx)
                    {
                        j.Add(new JsonStringValue("RETCODE", "-3"));
                        j.Add(new JsonStringValue("RETMSG", "해당 쿠폰은 발행된 가게에서만 사용가능합니다"));
                        j.Add(new JsonStringValue("COUPON_AMOUNT", su.Encrypt_AES("0")));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    sql = "update coupon set usedate=null,useyn='N',pay_id=null,usecanceldate=getdate()  where 쿠폰번호='" + coupon_no + "' and machine_idx='" + machine_idx + "' ";
                    if (su.SqlNoneQuery(sql) == true)
                    {
                        j.Add(new JsonStringValue("RETCODE", "0"));
                        j.Add(new JsonStringValue("RETMSG", "정상"));
                        j.Add(new JsonStringValue("eType", eType));
                    }
                    else
                    {
                        j.Add(new JsonStringValue("RETCODE", "-9"));
                        j.Add(new JsonStringValue("RETMSG", "서버 장애입니다.잠시후 다시 시도해 주세요"));
                        j.Add(new JsonStringValue("eType", eType));
                    }

                    retVal = j.ToString();
                }
                catch (Exception ex)
                {
                    j.Clear();

                    j.Add(new JsonStringValue("RETCODE", "-99"));
                    j.Add(new JsonStringValue("RETMSG", ex.Message.ToString()));
                    j.Add(new JsonStringValue("eType", eType));

                    retVal = j.ToString();
                }
                #endregion
                break;

            case "7":
                #region 자판기번호조회
                try
                {
                    j.Clear();

                    sql = "update machine set 최종보고일시=getdate() where idx='" + machine_idx + "' ";
                    su.SqlNoneQuery(sql);

                    sql = "select a.idx, a.install_type, a.결제구분,a.결제코드,a.결제업체,a.관리자연락처,dbo.fn_AesDec(isnull(a.관리자비밀번호,'')) 관리자비밀번호, b.name [총판명], c.name [대리점명] , a.자판기공인IP, a.자판기로컬IP, a.최종소프트웨어버젼, a.컨트롤러버젼, a.랙수 ";
                    sql += " from machine a with(nolock) left outer join agency b with(nolock) on a.agency_idx = b.idx ";
                    sql += " left outer join branch c with(nolock) on a.branch_idx = c.idx ";
                    sql += " where a.idx='" + machine_idx +"' ";
                    DataView dv = su.SqlDvQuery(sql);

                    if(dv==null || dv.Count == 0)
                    {
                        j.Add(new JsonStringValue("RETCODE", "-1"));
                        j.Add(new JsonStringValue("RETMSG", "자판기번호가 존재하지 않습니다"));
                    }
                    else
                    {
                        j.Add(new JsonStringValue("RETCODE", "0"));
                        j.Add(new JsonStringValue("RETMSG", "정상"));

                        j.Add(new JsonStringValue("mid", dv[0]["idx"].ToString()));
                        j.Add(new JsonStringValue("pay_type", dv[0]["결제구분"].ToString()));
                        j.Add(new JsonStringValue("pay_code", dv[0]["결제코드"].ToString()));
                        j.Add(new JsonStringValue("pay_com", dv[0]["결제업체"].ToString()));
                        j.Add(new JsonStringValue("agency_name", dv[0]["총판명"].ToString()));
                        j.Add(new JsonStringValue("branch_name", dv[0]["대리점명"].ToString()));
                        j.Add(new JsonStringValue("currentver", dv[0]["최종소프트웨어버젼"].ToString()));
                        j.Add(new JsonStringValue("localip", dv[0]["자판기로컬IP"].ToString()));
                        j.Add(new JsonStringValue("globalip", dv[0]["자판기공인IP"].ToString()));
                        j.Add(new JsonStringValue("ctver", dv[0]["컨트롤러버젼"].ToString()));

                        j.Add(new JsonStringValue("install_type", "구형(MINI)"));
                        j.Add(new JsonStringValue("amdinhp", dv[0]["관리자연락처"].ToString()));
                        j.Add(new JsonStringValue("adminpasswd", dv[0]["관리자비밀번호"].ToString()));
                        j.Add(new JsonStringValue("rackusecnt", dv[0]["랙수"].ToString()));
                    }

                    j.Add(new JsonStringValue("eType", eType));
                    retVal = j.ToString();
                }
                catch (Exception ex)
                {
                    j.Clear();

                    j.Add(new JsonStringValue("RETCODE", "-99"));
                    j.Add(new JsonStringValue("RETMSG", ex.Message.ToString()));
                    j.Add(new JsonStringValue("eType", eType));

                    retVal = j.ToString();
                }
                #endregion
                break;

            case "8":
                #region 자판기정보수정
                try
                {
                    j.Clear();

                    String 관리자연락처 = (String)getData["adminhp"].GetValue();
                    String 로컬IP = (String)getData["localip"].GetValue();
                    String 컨트롤러버젼 = (String)getData["ctver"].GetValue();

                    j.Add(new JsonStringValue("RETCODE", "0"));
                    j.Add(new JsonStringValue("RETMSG", "정상"));

                    sql = "update machine set 관리자연락처='"+ 관리자연락처+"' ";
                    sql += " ,자판기로컬IP='" + 로컬IP + "' ";
                    sql += " ,컨트롤러버젼='" + 컨트롤러버젼 + "' ";
                    sql += " ,최종보고일시=getdate() ";
                    sql += " where idx='"+ machine_idx + "' ";
                    su.SqlNoneQuery(sql);


                    j.Add(new JsonStringValue("eType", eType));
                    retVal = j.ToString();
                }
                catch (Exception ex)
                {
                    j.Clear();

                    j.Add(new JsonStringValue("RETCODE", "-99"));
                    j.Add(new JsonStringValue("RETMSG", ex.Message.ToString()));
                    j.Add(new JsonStringValue("eType", eType));
                    j.Add(new JsonStringValue("body", body));

                    retVal = j.ToString();
                }
                #endregion
                break;

            case "9":
                #region 상태보고
                try
                {
                    j.Clear();

                    String rack_stat = su.sg_db_query((String)getData["rack_stat"].GetValue());

                    if (su.CntBytesLen(rack_stat) > 20)
                    {
                        sql = "update machine set machine_stat='" + rack_stat + "', machine_stat_date=getdate(), 최종보고일시=getdate() where idx='" + machine_idx + "' ";
                        su.SqlNoneQuery(sql);

                        sql = "delete from machine_rack_prinfo where machine_idx='"+ machine_idx+"' ";
                        su.SqlNoneQuery(sql);

                        JsonArrayCollection rack_prinfo = (JsonArrayCollection)getData["rack_prinfo"];
                        for (int i = 0; i < rack_prinfo.Count; i++)
                        {
                            JsonObjectCollection row = (JsonObjectCollection)parser.Parse(rack_prinfo[i].ToString());
                            String rack_name = (String)row["rack_name"].GetValue();
                            String pr_idx = (String)row["pr_idx"].GetValue();
                            String pr_brand_idx = (String)row["pr_brand_idx"].GetValue();
                            String pr_menu_idx = (String)row["pr_menu_idx"].GetValue();
                            String pr_cnt = (String)row["pr_cnt"].GetValue();
                            String pr_price = (String)row["pr_price"].GetValue();

                            if (pr_idx == "") { pr_idx = "0"; }
                            if (pr_brand_idx == "") { pr_brand_idx = "0"; }
                            if (pr_menu_idx == "") { pr_menu_idx = "0"; }
                            if (pr_cnt == "") { pr_cnt = "0"; }
                            if (pr_price == "") { pr_price = "0"; }

                            sql = "insert into machine_rack_prinfo(machine_idx,rack_name,pr_idx,pr_brand_idx,pr_menu_idx,pr_cnt,pr_price)";
                            sql += " values('" + machine_idx + "','" + rack_name + "','" + pr_idx + "','" + pr_brand_idx + "','" + pr_menu_idx + "','" + pr_cnt + "','" + pr_price + "');";
                            su.SqlNoneQuery(sql);
                        }

                        j.Add(new JsonStringValue("RETCODE", "0"));
                        j.Add(new JsonStringValue("RETMSG", "정상"));
                        j.Add(new JsonStringValue("eType", eType));

                        retVal = j.ToString();

                    }
                    else
                    {
                        j.Clear();

                        j.Add(new JsonStringValue("RETCODE", "-99"));
                        j.Add(new JsonStringValue("RETMSG", "rack_stat의 길이는 22Byte이어야 합니다. ["+ rack_stat + "] "+ su.CntBytesLen(rack_stat).ToString()+"bytes"));
                        j.Add(new JsonStringValue("eType", eType));

                        retVal = j.ToString();

                    }

                }
                catch (Exception ex)
                {
                    j.Clear();

                    j.Add(new JsonStringValue("RETCODE", "-99"));
                    j.Add(new JsonStringValue("RETMSG", ex.Message.ToString()));
                    j.Add(new JsonStringValue("eType", eType));

                    retVal = j.ToString();
                }
                #endregion
                break;

            case "10":
                #region 라이브체크
                try
                {
                    sql = "update machine set machine_stat_date=getdate(), 최종보고일시=getdate() where idx='" + machine_idx + "' ";
                    su.SqlNoneQuery(sql);

                    j.Clear();

                    j.Add(new JsonStringValue("RETCODE", "0"));
                    j.Add(new JsonStringValue("eType", eType));
                    retVal = j.ToString();
                }
                catch (Exception ex)
                {
                    j.Clear();
                    j.Add(new JsonStringValue("RETCODE", "-99"));
                    j.Add(new JsonStringValue("eType", eType));

                    retVal = j.ToString();
                }
                #endregion
                break;

            case "11":
                #region 자판기오픈기록
                try
                {
                    j.Clear();

                    String open_date =su.sg_db_query((String)getData["opentime"].GetValue());

                    if (su.isDate(open_date) == false)
                    {
                        j.Add(new JsonStringValue("RETCODE", "-2"));
                        j.Add(new JsonStringValue("RETMSG", "opentime 은 yyyy-MM-dd HH:mm:ss 형식으로 입력되어야 합니다."));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }                    

                    sql = "update machine set 최종보고일시=getdate(),최종오픈일시='"+ open_date +"' where idx='" + machine_idx + "' ";
                    su.SqlNoneQuery(sql);

                    sql = "insert into machine_open_log(machine_idx,opentime) values('"+ machine_idx+"', '"+ open_date+"'); ";
                    su.SqlNoneQuery(sql);

                    j.Add(new JsonStringValue("RETCODE", "0"));
                    j.Add(new JsonStringValue("RETMSG", "정상"));
                    j.Add(new JsonStringValue("eType", eType));
                    retVal = j.ToString();
                }
                catch (Exception ex)
                {
                    j.Clear();

                    j.Add(new JsonStringValue("RETCODE", "-99"));
                    j.Add(new JsonStringValue("RETMSG", ex.Message.ToString()));
                    j.Add(new JsonStringValue("eType", eType));

                    retVal = j.ToString();
                }
                #endregion
                break;

            case "12":
                #region 결제취소보고

                try
                {
                    j.Clear();

                    String pay_id = su.sg_db_query((String)getData["pay_id"].GetValue());
                    String pay_cdate = su.sg_db_query((String)getData["pay_cdate"].GetValue());
                    String pay_ctid = su.sg_db_query((String)getData["pay_ctid"].GetValue());

                    if (pay_id == "")
                    {
                        j.Add(new JsonStringValue("RETCODE", "-2"));
                        j.Add(new JsonStringValue("RETMSG", "PAY_ID는 자판기 고유 결제 번호를 넣어주셔야 합니다."));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    if (pay_ctid == "")
                    {
                        j.Add(new JsonStringValue("RETCODE", "-2"));
                        j.Add(new JsonStringValue("RETMSG", "pay_ctid는 취소승인ID를 넣어주셔야 합니다"));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }
                    if (pay_cdate == "" || su.isDate(pay_cdate) == false )
                    {
                        j.Add(new JsonStringValue("RETCODE", "-2"));
                        j.Add(new JsonStringValue("RETMSG", "pay_cdate는 승인취소 일시(yyyy-MM-dd HH:mm:ss:fff) 를 넣어주셔야 합니다."));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }

                    sql = "select agency_idx,branch_idx from machine with(Nolock) where idx='" + machine_idx + "' ";
                    DataView dv2 = su.SqlDvQuery(sql);
                    String agency_idx = "0";
                    String branch_idx = "0";

                    if (dv2 == null || dv2.Count == 0)
                    {
                        j.Add(new JsonStringValue("RETCODE", "-1"));
                        j.Add(new JsonStringValue("RETMSG", "자판기번호 가 존재하지 않습니다"));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }
                    agency_idx = dv2[0]["agency_idx"].ToString();
                    branch_idx = dv2[0]["branch_idx"].ToString();

                    sql = "update machine set 최종보고일시=getdate() where idx='" + machine_idx + "' ";
                    su.SqlNoneQuery(sql);

                    sql = "select machine_idx,agency_idx,branch_idx,pay_id,total_amount,coupon_use_yn,coupon_num,coupon_amount ";
                    sql += " ,pay_amount,pay_type,pay_tid,pay_result,pay_result_msg,pay_result_no,pay_date,out_result,client_ip from machine_pay with(Nolock) where machine_idx='" + machine_idx + "' and pay_id='" + pay_id + "' ";

                    DataView dv = su.SqlDvQuery(sql);
                    if(dv!=null && dv.Count>0)
                    {
                        String coupon_use_yn = dv[0]["coupon_use_yn"].ToString();
                        String coupon_num = dv[0]["coupon_num"].ToString();

                        sql = "update machine_pay set pay_cdate='" + pay_cdate + "', pay_ctid='" + pay_ctid + "'  where machine_idx = '" + machine_idx + "' and pay_id = '" + pay_id + "' ";
                        su.SqlNoneQuery(sql);

                        sql = "update coupon set usedate=null,useyn='N',pay_id='" + pay_id + "'  where 쿠폰번호='" + coupon_num + "' and machine_idx='" + machine_idx + "' ";
                        su.SqlNoneQuery(sql);

                        j.Add(new JsonStringValue("RETCODE", "0"));
                        j.Add(new JsonStringValue("RETMSG", "정상"));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }
                    else
                    {
                        // 결제보고가 올라오기 전에 취소요청이 오는 경우 일단 보고 실패로 떨구고 
                        // 결제보고 올라온후 다시 처리 하게 함
                        j.Add(new JsonStringValue("RETCODE", "-4"));
                        j.Add(new JsonStringValue("RETMSG", "해당 payid는 아직 결제보고 전으로 보입니다. 잠시후 다시 시도해 주시기 바랍니다."));
                        j.Add(new JsonStringValue("eType", eType));
                        retVal = j.ToString();
                        break;
                    }


                }
                catch (Exception ex)
                {
                    j.Clear();

                    j.Add(new JsonStringValue("RETCODE", "-99"));
                    j.Add(new JsonStringValue("RETMSG", ex.Message.ToString()));
                    j.Add(new JsonStringValue("eType", eType));
                    j.Add(new JsonStringValue("body", body));

                    retVal = j.ToString();
                }
                #endregion
                break;
        }


        try
        {
            sql = "update machine_log set senddata='" + su.sg_db_query(retVal) + "' where idx='" + log_idx + "'; ";
            su.SqlNoneQuery(sql);
        }
        catch (Exception ex) { }

        Response.Write(retVal);

    }

}
