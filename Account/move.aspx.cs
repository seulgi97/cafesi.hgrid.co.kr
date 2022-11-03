using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_Move : System.Web.UI.Page
{
    String sql = "";
    SgFramework.SgUtil su = new SgFramework.SgUtil();
    SgFramework_mysql.SgUtil mu = new SgFramework_mysql.SgUtil();
    protected void Page_Load(object sender, EventArgs e)
    {

        #region agency 이관
        sql = "delete from agency";
        su.SqlNoneQuery(sql);

        sql = "select code, name, bizNo, chief, telNo, mphone, addr from agencies";
        DataView dv1 = mu.SqlDvQuery(sql);

        sql = "insert into agency(company_idx, agency_code, name, bizNo, chief, telNo, mphone, addr1) values ";
        for (int i = 0; i < dv1.Count; i++)
        {
            sql += "('1','" + dv1[i]["code"].ToString() + "','" + dv1[i]["name"].ToString() + "' ,'" + dv1[i]["bizNo"].ToString() + "','" + dv1[i]["chief"].ToString() + "','" + dv1[i]["telNo"].ToString() + "','" + dv1[i]["mphone"].ToString() + "','" + dv1[i]["addr"].ToString() + "') ";
            if (i < dv1.Count - 1)
            {
                sql += ",";
            }
        }
        sql += ";";
        su.SqlNoneQuery(sql);
        #endregion

        #region branch 이관
        sql = "delete from branch";
        su.SqlNoneQuery(sql);

        sql = "select agency_code, code, name, bizNo, chief, telNo, mphone, addr from branches";
        dv1 = mu.SqlDvQuery(sql);

        sql = "insert into branch(company_idx, agency_code, branch_code, name, bizNo, chief, telNo, mphone, addr1) values ";
        for (int i = 0; i < dv1.Count; i++)
        {
            sql += "('1','" + dv1[i]["agency_code"].ToString() + "','" + dv1[i]["code"].ToString() + "','" + dv1[i]["name"].ToString() + "' ,'" + dv1[i]["bizNo"].ToString() + "','" + dv1[i]["chief"].ToString() + "','" + dv1[i]["telNo"].ToString() + "','" + dv1[i]["mphone"].ToString() + "','" + dv1[i]["addr"].ToString() + "') ";
            if (i < dv1.Count - 1)
            {
                sql += ",";
            }
        }
        sql += ";";
        su.SqlNoneQuery(sql);

        sql = "update branch set agency_idx = a.idx from agency a where branch.agency_code = a.agency_code";
        su.SqlNoneQuery(sql);
        #endregion

        #region machine 이관

        sql = "delete from machine";
        su.SqlNoneQuery(sql);

        sql = "select idx, company_code, agency_code, branch_code, pos_number, machine_code, install_type, date_format(install_date,'%Y-%m-%d %H:%i:%s') install_date, install_spot, coffee_price, lte_router, reboot_error, null reboot_time, pay_tid, pay_tid_udate from machines";
        dv1 = mu.SqlDvQuery(sql);

        sql = "insert into machine( company_idx, agency_code, branch_code, pos_number, machine_code, install_type, install_date, install_spot, coffee_price, lte_router, reboot_error, reboot_time, pay_tid, pay_tid_udate) values ";
        for (int i = 0; i < dv1.Count; i++)
        {
            sql += "('1','" + dv1[i]["agency_code"].ToString() + "','" + dv1[i]["branch_code"].ToString() + "','" + dv1[i]["pos_number"].ToString() + "','" + dv1[i]["machine_code"].ToString() + "','" + dv1[i]["install_type"].ToString() + "' ,'" + dv1[i]["install_date"].ToString() + "','" + dv1[i]["install_spot"].ToString() + "','" + dv1[i]["coffee_price"].ToString() + "','" + dv1[i]["lte_router"].ToString() + "','" + dv1[i]["reboot_error"].ToString() + "',null,'" + dv1[i]["pay_tid"].ToString() + "',null) ";
            if (i < dv1.Count - 1)
            {
                sql += ",";
            }
        }
        sql += ";";
        su.SqlNoneQuery(sql);

        sql = "update machine set agency_idx = a.idx from agency a where machine.agency_code = a.agency_code";
        su.SqlNoneQuery(sql);

        sql = "update machine set branch_idx = a.idx from branch a where machine.branch_code = a.branch_code and  machine.agency_code = a.agency_code";
        su.SqlNoneQuery(sql);

        #endregion

        #region coffee_brand 이관

        sql = "delete from coffee_brand";
        su.SqlNoneQuery(sql);

        sql = "select idx, brand_name, date_format(regist_date,'%Y-%m-%d %H:%i:%s') regist_date, date_format(modify_date,'%Y-%m-%d %H:%i:%s') modify_date, is_using, is_show from coffee_brands";
        dv1 = mu.SqlDvQuery(sql);

        sql = "insert into coffee_brand( old_idx, brand_name, regist_date, modify_date, is_using, is_show ) values ";
        for (int i = 0; i < dv1.Count; i++)
        {
            sql += "('" + dv1[i]["idx"].ToString() + "','" + dv1[i]["brand_name"].ToString() + "','" + dv1[i]["regist_date"].ToString() + "','" + dv1[i]["modify_date"].ToString() + "','" + dv1[i]["is_using"].ToString() + "','" + dv1[i]["is_show"].ToString() + "') ";
            if (i < dv1.Count - 1)
            {
                sql += ",";
            }
        }
        sql += ";";
        su.SqlNoneQuery(sql);

        #endregion

        #region coffee 이관

        sql = "delete from coffee";
        su.SqlNoneQuery(sql);

        sql = "select idx, brand, code, price, unit, name, comt, photo, date_format(modify_date,'%Y-%m-%d %H:%i:%s') modify_date from coffees";
        dv1 = mu.SqlDvQuery(sql);

        for (int i = 0; i < dv1.Count; i++)
        {
            try
            {
                sql = "insert into coffee( code, price, unit, name, comt, photo, modify_date ,old_brand_idx) values ";
                sql += "('" + dv1[i]["code"].ToString() + "','" + dv1[i]["price"].ToString() + "','" + dv1[i]["unit"].ToString() + "','" + dv1[i]["name"].ToString() + "','" + dv1[i]["comt"].ToString() + "','" + dv1[i]["photo"].ToString() + "',getdate(),'" + dv1[i]["brand"].ToString() + "') ";
                su.SqlNoneQuery(sql);
            }catch(Exception ex)
            {
                Response.Write(sql);
                return;
            }
        }

        sql = "update coffee set brand_idx = a.idx from coffee_brand a where coffee.old_brand_idx = a.old_idx";
        su.SqlNoneQuery(sql);

        #endregion

        #region member 이관

        sql = "delete from member";
        su.SqlNoneQuery(sql);

        sql = "select  idx, userid, passwd, usernm, usrtel, mphone, level, active, company_code, agency_code, branch_code, pos_number, date_format(rgdate,'%Y-%m-%d %H:%i:%s')  rgdate, usrorg, date_format(locktime,'%Y-%m-%d %H:%i:%s') locktime, lockcnt from members";
        dv1 = mu.SqlDvQuery(sql);

        for (int i = 0; i < dv1.Count; i++)
        {
            try
            {
                sql = "insert into member(  userid, passwd, usernm, usrtel, mphone, lev, active, company_idx, agency_code, branch_code, pos_number, regdate, usrorg, locktime, lockcnt ) values ";
                sql += "('" + dv1[i]["userid"].ToString() + "','" + dv1[i]["passwd"].ToString() + "','" + dv1[i]["usernm"].ToString() + "','" + dv1[i]["usrtel"].ToString() + "','" + dv1[i]["mphone"].ToString() + "','" + dv1[i]["level"].ToString() + "','" + dv1[i]["active"].ToString() + "','1','" + dv1[i]["agency_code"].ToString() + "','" + dv1[i]["branch_code"].ToString() + "','" + dv1[i]["pos_number"].ToString() + "','" + dv1[i]["rgdate"].ToString() + "','" + dv1[i]["usrorg"].ToString() + "',getdate(),'" + dv1[i]["lockcnt"].ToString() + "') ";
                su.SqlNoneQuery(sql);
            }
            catch (Exception ex)
            {
                Response.Write(sql);
                return;
            }
        }

        sql = "update member set agency_idx = a.agency_idx from branch a where member.agency_code = a.agency_code ";
        su.SqlNoneQuery(sql);

        sql = "update member set branch_idx = a.idx, agency_idx = a.agency_idx  from branch a where member.agency_code = a.agency_code and member.branch_code = a.branch_code";
        su.SqlNoneQuery(sql);

        sql = "update member set passwd=dbo.fn_AesEnc('inb5040sr!')";
        su.SqlNoneQuery(sql);

        #endregion

        #region sock_log 이관

        //sql = "delete from socket_log";
        //su.SqlNoneQuery(sql);

        //sql = "select  idx, date_format(req_time,'%Y-%m-%d %H:%i:%s') req_time,company_code,agency_code,branch_code,pos_number,req_cmd,req_data,rsp_data from socket_log where req_time >'2020-01-01' ";
        //dv1 = mu.SqlDvQuery(sql);

        //for (int i = 0; i < dv1.Count; i++)
        //{
        //    try
        //    {
        //        sql = "insert into socket_log(  req_time,company_code,agency_code,branch_code,pos_number,req_cmd,req_data,rsp_data ) values ";
        //        sql += "('" + dv1[i]["req_time"].ToString() + "','" + dv1[i]["company_code"].ToString() + "','" + dv1[i]["agency_code"].ToString() + "','" + dv1[i]["branch_code"].ToString() + "','" + dv1[i]["pos_number"].ToString() + "','" + dv1[i]["req_cmd"].ToString() + "','" + dv1[i]["req_data"].ToString() + "','" + dv1[i]["rsp_data"].ToString() + "'); ";
        //        su.SqlNoneQuery(sql);

        //        Response.Flush();
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(sql);
        //        return;
        //    }
        //}

        #endregion

        #region tbl_pay_log 이관

        //sql = "delete from tbl_pay_log";
        //su.SqlNoneQuery(sql);

        //sql = "select  socketidx, 자판기번호, 총판명, 총판코드, 대리점명, 대리점코드, 포스코드, 설치장소, LTE번호,date_format(결제일시,'%Y-%m-%d %H:%i:%s') 결제일시, 결제일, 결제성공여부, 승인번호, 결제방법코드, 결제방법, 셋팅상품수, 셋팅가격수, 판매갯수, 총결제금액, 상품1, 상품1요청, 상품1투출, 상품1재고, 상품1상태, 상품1가격, 상품1결제, 상품2, 상품2요청, 상품2투출, 상품2재고, 상품2상태, 상품2가격, 상품2결제, 상품3, 상품3요청, 상품3투출, 상품3재고, 상품3상태, 상품3가격, 상품3결제, 상품4, 상품4요청, 상품4투출, 상품4재고, 상품4상태, 상품4가격, 상품4결제, 상품5, 상품5요청, 상품5투출, 상품5재고, 상품5상태, 상품5가격, 상품5결제, 상품6, 상품6요청, 상품6투출, 상품6재고, 상품6상태, 상품6가격, 상품6결제, 상품7, 상품7요청, 상품7투출, 상품7재고, 상품7상태, 상품7가격, 상품7결제, 상품8, 상품8요청, 상품8투출, 상품8재고, 상품8상태, 상품8가격, 상품8결제, 상품9, 상품9요청, 상품9투출, 상품9재고, 상품9상태, 상품9가격, 상품9결제, 투출정보, 가격정보, 송부업체, date_format(송부일시,'%Y-%m-%d %H:%i:%s') 송부일시, date_format(결과처리일시,'%Y-%m-%d %H:%i:%s') 결과처리일시, 결과구분, 카드번호, 카드사명, 카드사코드, 승인pay_seq, 승인van_seq, 취소일시, 취소pay_seq, 취소van_seq, TID, VANTID from tbl_pay_log ";
        //dv1 = mu.SqlDvQuery(sql);

        //for (int i = 0; i < dv1.Count; i++)
        //{
        //    try
        //    {
        //        sql = "insert into tbl_pay_log( socketidx, 자판기번호, 총판명, 총판코드, 대리점명, 대리점코드, 포스코드, 설치장소, LTE번호, 결제일시, 결제일, 결제성공여부, 승인번호, 결제방법코드, 결제방법, 셋팅상품수, 셋팅가격수, 판매갯수, 총결제금액, 상품1, 상품1요청, 상품1투출, 상품1재고, 상품1상태, 상품1가격, 상품1결제, 상품2, 상품2요청, 상품2투출, 상품2재고, 상품2상태, 상품2가격, 상품2결제, 상품3, 상품3요청, 상품3투출, 상품3재고, 상품3상태, 상품3가격, 상품3결제, 상품4, 상품4요청, 상품4투출, 상품4재고, 상품4상태, 상품4가격, 상품4결제, 상품5, 상품5요청, 상품5투출, 상품5재고, 상품5상태, 상품5가격, 상품5결제, 상품6, 상품6요청, 상품6투출, 상품6재고, 상품6상태, 상품6가격, 상품6결제, 상품7, 상품7요청, 상품7투출, 상품7재고, 상품7상태, 상품7가격, 상품7결제, 상품8, 상품8요청, 상품8투출, 상품8재고, 상품8상태, 상품8가격, 상품8결제, 상품9, 상품9요청, 상품9투출, 상품9재고, 상품9상태, 상품9가격, 상품9결제, 투출정보, 가격정보, 송부업체, 송부일시, 결과처리일시, 결과구분, 카드번호, 카드사명, 카드사코드, 승인pay_seq, 승인van_seq, 취소일시, 취소pay_seq, 취소van_seq, TID, VANTID ) values ";
        //        sql += "('" + dv1[i]["socketidx"].ToString() + "','" + dv1[i]["자판기번호"].ToString() + "','" + dv1[i]["총판명"].ToString() + "','" + dv1[i]["총판코드"].ToString() + "','" + dv1[i]["대리점명"].ToString() + "','" + dv1[i]["대리점코드"].ToString() + "','" + dv1[i]["포스코드"].ToString() + "','" + dv1[i]["설치장소"].ToString() + "','" + dv1[i]["LTE번호"].ToString() + "',replace(dbo.isblink('" + dv1[i]["결제일시"].ToString() + "'),'0000-00-00 00:00:00',null),'" + dv1[i]["결제일"].ToString() + "','" + dv1[i]["결제성공여부"].ToString() + "','" + dv1[i]["승인번호"].ToString() + "','" + dv1[i]["결제방법코드"].ToString() + "','" + dv1[i]["결제방법"].ToString() + "','" + dv1[i]["셋팅상품수"].ToString() + "','" + dv1[i]["셋팅가격수"].ToString() + "','" + dv1[i]["판매갯수"].ToString() + "','" + dv1[i]["총결제금액"].ToString() + "','" + dv1[i]["상품1"].ToString() + "','" + dv1[i]["상품1요청"].ToString() + "','" + dv1[i]["상품1투출"].ToString() + "','" + dv1[i]["상품1재고"].ToString() + "','" + dv1[i]["상품1상태"].ToString() + "','" + dv1[i]["상품1가격"].ToString() + "','" + dv1[i]["상품1결제"].ToString() + "','" + dv1[i]["상품2"].ToString() + "','" + dv1[i]["상품2요청"].ToString() + "','" + dv1[i]["상품2투출"].ToString() + "','" + dv1[i]["상품2재고"].ToString() + "','" + dv1[i]["상품2상태"].ToString() + "','" + dv1[i]["상품2가격"].ToString() + "','" + dv1[i]["상품2결제"].ToString() + "','" + dv1[i]["상품3"].ToString() + "','" + dv1[i]["상품3요청"].ToString() + "','" + dv1[i]["상품3투출"].ToString() + "','" + dv1[i]["상품3재고"].ToString() + "','" + dv1[i]["상품3상태"].ToString() + "','" + dv1[i]["상품3가격"].ToString() + "','" + dv1[i]["상품3결제"].ToString() + "','" + dv1[i]["상품4"].ToString() + "','" + dv1[i]["상품4요청"].ToString() + "','" + dv1[i]["상품4투출"].ToString() + "','" + dv1[i]["상품4재고"].ToString() + "','" + dv1[i]["상품4상태"].ToString() + "','" + dv1[i]["상품4가격"].ToString() + "','" + dv1[i]["상품4결제"].ToString() + "','" + dv1[i]["상품5"].ToString() + "','" + dv1[i]["상품5요청"].ToString() + "','" + dv1[i]["상품5투출"].ToString() + "','" + dv1[i]["상품5재고"].ToString() + "','" + dv1[i]["상품5상태"].ToString() + "','" + dv1[i]["상품5가격"].ToString() + "','" + dv1[i]["상품5결제"].ToString() + "','" + dv1[i]["상품6"].ToString() + "','" + dv1[i]["상품6요청"].ToString() + "','" + dv1[i]["상품6투출"].ToString() + "','" + dv1[i]["상품6재고"].ToString() + "','" + dv1[i]["상품6상태"].ToString() + "','" + dv1[i]["상품6가격"].ToString() + "','" + dv1[i]["상품6결제"].ToString() + "','" + dv1[i]["상품7"].ToString() + "','" + dv1[i]["상품7요청"].ToString() + "','" + dv1[i]["상품7투출"].ToString() + "','" + dv1[i]["상품7재고"].ToString() + "','" + dv1[i]["상품7상태"].ToString() + "','" + dv1[i]["상품7가격"].ToString() + "','" + dv1[i]["상품7결제"].ToString() + "','" + dv1[i]["상품8"].ToString() + "','" + dv1[i]["상품8요청"].ToString() + "','" + dv1[i]["상품8투출"].ToString() + "','" + dv1[i]["상품8재고"].ToString() + "','" + dv1[i]["상품8상태"].ToString() + "','" + dv1[i]["상품8가격"].ToString() + "','" + dv1[i]["상품8결제"].ToString() + "','" + dv1[i]["상품9"].ToString() + "','" + dv1[i]["상품9요청"].ToString() + "','" + dv1[i]["상품9투출"].ToString() + "','" + dv1[i]["상품9재고"].ToString() + "','" + dv1[i]["상품9상태"].ToString() + "','" + dv1[i]["상품9가격"].ToString() + "','" + dv1[i]["상품9결제"].ToString() + "','" + dv1[i]["투출정보"].ToString() + "','" + dv1[i]["가격정보"].ToString() + "','" + dv1[i]["송부업체"].ToString() + "',dbo.isblink('" + dv1[i]["송부일시"].ToString() + "'),dbo.isblink('" + dv1[i]["결과처리일시"].ToString() + "'),'" + dv1[i]["결과구분"].ToString() + "','" + dv1[i]["카드번호"].ToString() + "','" + dv1[i]["카드사명"].ToString() + "','" + dv1[i]["카드사코드"].ToString() + "','" + dv1[i]["승인pay_seq"].ToString() + "','" + dv1[i]["승인van_seq"].ToString() + "',dbo.isblink('" + dv1[i]["취소일시"].ToString() + "'),'" + dv1[i]["취소pay_seq"].ToString() + "','" + dv1[i]["취소van_seq"].ToString() + "','" + dv1[i]["TID"].ToString() + "','" + dv1[i]["VANTID"].ToString() +"'); ";
        //        su.SqlNoneQuery(sql);

        //        Response.Flush();
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(i.ToString()+" - "+ex.Message.ToString()+"<br>");
        //        Response.Write(sql);
        //        return;
        //    }
        //}
        #endregion

    }
}