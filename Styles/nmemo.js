    var img_L = 0;
    var img_R = 0;
    var img_T = 0;
    var targetObj;
    var sgIng = true;

    function getLeft(o){
        return parseInt(o.style.left.replace('px', ''));
    }
    function getRight(o) {
        return parseInt(o.style.right.replace('px', ''));
    }
    function getTop(o) {
        var retVal = parseInt(o.style.top.replace('px', ''));
        return retVal;
    }

    // 이미지 움직이기
    function moveDrag(e){
        var e_obj = window.event? window.event : e;
        var dmvx = parseInt(img_R + (cx - e_obj.clientX) );
        var dmvy = parseInt(e_obj.clientY + img_T);
        targetObj.style.right = dmvx +"px";
        targetObj.style.top = dmvy + "px";

        return false;
    }

    // 드래그 시작

    var targetObj;
    var targetSeq;
    var stidx = 999;
    var cx = 0;

    function startDrag(e, tobj, obj){
        targetObj = tobj;
        stidx++;
        targetObj.style.zIndex = stidx;
        var e_obj = window.event? window.event : e;
        img_R = getRight(tobj);
        img_T = getTop(tobj) - e_obj.clientY;
        cx = e_obj.clientX;
        document.onmousemove = moveDrag;
        document.onmouseup = stopDrag;

        var tobj = document.getElementById('nmemo_alram_set');
        tobj.style.display = 'none';

        if(e_obj.preventDefault)e_obj.preventDefault(); 
    }

    // 드래그 멈추기
    function stopDrag() {
        document.onmousemove = null;
        document.onmouseup = null;

        sendRequest()
    }

    // 토글
    function togMemo(obj) {

        if (document.getElementById('nmemo_body' + obj.id.replace('nmemo', '')).style.display == '') {
            document.getElementById('nmemo_title' + obj.id.replace('nmemo', '')).style.display = '';
            document.getElementById('nmemo_body' + obj.id.replace('nmemo', '')).style.display = 'none'
            document.getElementById('nmemo_text' + obj.id.replace('nmemo', '')).style.display = 'none';
            document.getElementById('nmemo' + obj.id.replace('nmemo', '')).style.width = '150px';
        } else {

            document.getElementById('nmemo_title' + obj.id.replace('nmemo', '')).style.display = 'none';
            document.getElementById('nmemo_body' + obj.id.replace('nmemo', '')).style.display = '';
            document.getElementById('nmemo_text' + obj.id.replace('nmemo', '')).style.display = '';
            document.getElementById('nmemo' + obj.id.replace('nmemo', '')).style.width = '300px';
        }

        sendRequest();
    }

    function togMemoSmall(obj) {

        document.getElementById('nmemo_title' + obj.id.replace('nmemo', '')).style.display = '';
        document.getElementById('nmemo_body' + obj.id.replace('nmemo', '')).style.display = 'none'
        document.getElementById('nmemo_text' + obj.id.replace('nmemo', '')).style.display = 'none';
        document.getElementById('nmemo' + obj.id.replace('nmemo', '')).style.width = '150px';

        sendRequest();
    }
    
    function fn_resize(obj) {
        obj.style.height = "1px";
        obj.style.height = (20+obj.scrollHeight)+"px";
    }
    
    function sg_replace(str, bc, nc) {
        return str.replace(eval("/\\" + bc + "/g"), nc);
    }

    function getXMLHttpRequest() {
        if (window.ActiveXObject) {
            try {
                return new ActiveXObject("Msxml2.XMLHTTP");
            } catch (e) {
                try {
                    return new ActiveXObject("Microsoft.XMLHTTP");
                } catch (e1) {
                    return null;
                }
            }
        } else if (window.XMLHttpRequest) {
            return new XMLHttpRequest();
        } else {
            return null;
        }
    }

    function randInt() {
        return Math.floor(Math.random() * 9999999999);
    }

    function fn_save(obj) {
        seq = obj.id.replace('nmemo_text', '');

        var strText = obj.value;
        if (sgIng == true) {
            httpRequest = getXMLHttpRequest();
            document.getElementById("nmemo_title" + seq).innerHTML = "<nobr>" + strText + "</nobr>";
            var httpParams = "ver=" + randInt()+"&eType=save&seq=" + seq+"&strText=" + strText;
            var httpUrl = "/Menu/Nmemo.aspx";

            //httpUrl = httpUrl + httpParams;

            httpRequest.open("POST", httpUrl, true);
            httpRequest.onreadystatechange = getRequest;
            httpRequest.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
            httpRequest.send(httpParams);
            sgIng = false;
        }
    }

    function getRequest() {
        sgIng = true;
        var retVal = '';
        if (httpRequest.readyState == 4) {
            if (httpRequest.status != 200) {
                GetRetValueTmp = httpRequest.status + "\n" + httpRequest.responseText;
                alert(GetRetValueTmp);
            }

        }
        document.getElementById('nmemo_alram_set').style.display = 'none';
    }

    function getRequestDelete() {
        sgIng = true;
        var retVal = '';
        if (httpRequest.readyState == 4) {
            if (httpRequest.status != 200) {
                GetRetValueTmp = httpRequest.status + "\n" + httpRequest.responseText;
                alert(GetRetValueTmp);
            } else {
                document.getElementById('nmemo' + targetSeq).style.display = 'none';
            }

        }
        document.getElementById('nmemo_alram_set').style.display = 'none';
       
    }

    function getRequestAlram() {
        sgIng = true;
        var retVal = '';
        if (httpRequest.readyState == 4) {
            if (httpRequest.status != 200) {
                GetRetValueTmp = httpRequest.status + "\n" + httpRequest.responseText;
                alert(GetRetValueTmp);
            } else {
                retVal = httpRequest.responseText;
                if (retVal == "0") {
                    seq = document.getElementById('alram_seq').value;
                    var adate = document.getElementById('alram_date').value;
                    var atime = document.getElementById('alram_time').value;
                    var amin = document.getElementById('alram_min').value;

                    var sendatype = "";
                    if (document.getElementById('alram_pop').checked == true) {
                        sendatype = "팝업";
                    }
                    if (document.getElementById('alram_sms').checked == true) {
                        sendatype = "문자";
                    }
                    if (document.getElementById('alram_mail').checked == true) {
                        sendatype = "메일";
                    }
                    document.getElementById('nmemo_alram_date' + seq).innerHTML = adate + " " + atime + ":" + amin+" " + sendatype;
                } else {
                    alert(retVal);
                }
            }

        }
        document.getElementById('nmemo_alram_set').style.display = 'none';
    }


    function fn_deletememo(seq) {

        if (confirm('메모를 삭제하시겠습니까?')) {
            if (sgIng == true) {
                httpRequest = getXMLHttpRequest();
                targetSeq = seq;
                var httpParams = "eType=delete&seq=" + seq;
                var httpUrl = "/Menu/Nmemo.aspx";

                httpUrl = httpUrl + "?ver=" + randInt() + "&" + httpParams;

                httpRequest.open("GET", httpUrl, true);
                httpRequest.onreadystatechange = getRequestDelete;
                httpRequest.send(null);
                sgIng = false;
            }
        }
    }

    function fn_hidememo(seq) {
        if (sgIng == true) {
            httpRequest = getXMLHttpRequest();
            targetSeq = seq;
            var httpParams = "eType=hide&seq=" + seq;
            var httpUrl = "/Menu/Nmemo.aspx";

            httpUrl = httpUrl + "?ver=" + randInt() + "&" + httpParams;

            httpRequest.open("GET", httpUrl, true);
            httpRequest.onreadystatechange = getRequestDelete;
            httpRequest.send(null);
            sgIng = false;
        }

    }

    function sendRequest() {
        seq = targetObj.id.replace('nmemo','');
        img_R = getRight(targetObj);
        img_T = getTop(targetObj);
        
        httpRequest = getXMLHttpRequest();
        var openyn = 'Y';

        if (document.getElementById("nmemo_body" + seq).style.display == '') {
            openyn = 'N';
        }

        if (sgIng == true) {
            var httpParams = "eType=xy&openyn=" + openyn + "&seq=" + seq + "&xy=right:" + img_R + "px;top:" + img_T + "px;";
            var httpUrl = "/Menu/Nmemo.aspx";

            httpUrl = httpUrl + "?ver=" + randInt() + "&" + httpParams;

            httpRequest.open("GET", httpUrl, true);
            httpRequest.onreadystatechange = getRequest;
            httpRequest.send(null);
            sgIng = false;
        }
    }

    function fn_memoboxfade(obj) {
        var topval = getTop(obj);
        var settop = topval + 22;

        if (topval < 0) {
            obj.style.top = settop +'px';
        }
    }

    function fn_memoboxfadeout(obj) {
        if (getTop(obj) >= 0) {
            obj.style.top = '-22px';
        }

    }
    function getAbsLeft(element){ 
        if(typeof element!='object') 
        element=document.getElementById(element); 
        var LEFT=0; 
        while(element){ 
            LEFT+=element.offsetLeft; 
            element=element.offsetParent; 
        }

        return LEFT;
    }

    function getAbsTop(element) {
        if (typeof element != 'object')
            element = document.getElementById(element);
        var TOP = 0;
        while (element) {
            TOP += element.offsetTop;
            element = element.offsetParent;
        }

        return TOP;
    }
    function fn_popwin() {
        //window.open('/menu/메모관리.aspx', 'nmemo', '');
        var winl = (screen.width - 700) / 2;
        var wint = (screen.height - 500) / 2;
        
        popWIn = window.open('/menu/메모관리.aspx');
        popWIn.focus();
    }

    function fn_alramset(seq,obj) {
        var l = parseInt(getAbsLeft(obj));
        var t = parseInt(getAbsTop(obj));

        document.getElementById('alram_seq').value = seq;
        var tobj = document.getElementById('nmemo_alram_set');
        document.getElementById('nmemo_send_set').style.display = 'none';
        tobj.style.display = '';
        tobj.style.left = (l-250)+'px';
        tobj.style.top = (t + 25)+'px';
        tobj.style.display = '';
    }

    function fn_alramsave() {
    
        seq = document.getElementById('alram_seq').value;
        var adate = document.getElementById('alram_date').value;
        var atime = document.getElementById('alram_time').value;
        var amin = document.getElementById('alram_min').value;

        var sendatype = "";
        if (document.getElementById('alram_pop').checked == true) {
            sendatype = "팝업";
        }
        if (document.getElementById('alram_sms').checked == true) {
            sendatype = "문자";
        }
        if (document.getElementById('alram_mail').checked == true) {
            sendatype = "메일";
        }

        if (sgIng == true) {
            httpRequest = getXMLHttpRequest();

            var httpParams = "eType=alramset&adate=" + adate + "&seq=" + seq + "&atime=" + atime + "&amin=" + amin + "&atype=" + encodeURIComponent(sendatype);
            var httpUrl = "/Menu/Nmemo.aspx";

            httpUrl = httpUrl + "?ver=" + randInt() + "&" + httpParams;

            httpRequest.open("GET", httpUrl, true);
            httpRequest.onreadystatechange = getRequestAlram;
            httpRequest.send(null);
            sgIng = false;
        }
    }

    function fn_alramclose(seq) {
        document.getElementById('nmemo_alram_pop' + seq).style.display = 'none';
    }

    function fn_colorset(seq, clr) {

        if (clr == "" || clr == "y") {
            tb_color = "#d1b124";
            body_color = "#ffdf4f";
        }

        if (clr == "g") {
            tb_color = "#8fd124";
            body_color = "#b9ff4f";
        }

        if (clr == "r") {
            tb_color = "#d14024";
            body_color = "#ff724f";
        }

        if (clr == "p") {
            tb_color = "#d124b4";
            body_color = "#ff4fe2";
        }

        if (clr == "b") {
            tb_color = "#2466d1";
            body_color = "#4f92ff";
        }

        document.getElementById('nmemo' + seq).style.background = tb_color;
        document.getElementById('nmemo_top' + seq).style.background = tb_color;
        document.getElementById('nmemo_title' + seq).style.background = tb_color;
        document.getElementById('nmemo_text' + seq).style.background = body_color;
        document.getElementById('nmemo_footer' + seq).style.background = tb_color;

        if (sgIng == true) {
            httpRequest = getXMLHttpRequest();
            var httpParams = "eType=colorset&scolor=" + clr + "&seq=" + seq;
            var httpUrl = "/Menu/Nmemo.aspx";

            httpUrl = httpUrl + "?ver=" + randInt() + "&" + httpParams;

            httpRequest.open("GET", httpUrl, true);
            httpRequest.onreadystatechange = getRequest;
            httpRequest.send(null);
            sgIng = false;
        }
    }

    function fn_memosend(seq,obj) {
        var l = parseInt(getAbsLeft(obj));
        var t = parseInt(getAbsTop(obj));

        document.getElementById('nmemo_alram_touch_seq').value = seq;
        var tobj = document.getElementById('nmemo_send_set');
        document.getElementById('nmemo_alram_set').style.display = 'none';
        tobj.style.display = '';
        tobj.style.left = (l - 310)+'px';
        tobj.style.top = (t + 25)+'px';
        tobj.style.display = '';
    }

    function fn_send_search(obj,e) {
        if (e.keyCode == 13 && e.srcElement.type != 'textarea') {
            return false;
        }

        if (obj.value.length > 1) {
            if (sgIng == true) {
                httpRequest = getXMLHttpRequest();
                var httpParams = "eType=send_search&ssearch=" + encodeURIComponent(obj.value);
                var httpUrl = "/Menu/Nmemo.aspx";

                httpUrl = httpUrl + "?ver=" + randInt() + "&" + httpParams;

                httpRequest.open("GET", httpUrl, true);
                httpRequest.onreadystatechange = fn_send_search_getRequest;
                httpRequest.send(null);
                sgIng = false;
            }
        }
    }

    function fn_send_search_getRequest() {
        sgIng = true;
        var retVal = '';
        if (httpRequest.readyState == 4) {
            if (httpRequest.status != 200) {
                GetRetValueTmp = httpRequest.status + "\n" + httpRequest.responseText;
                alert(GetRetValueTmp);
            } else {
                var retvals = httpRequest.responseText;
                var rows = retvals.split('|');
                var elSel = document.getElementById('nmemo_send_set_search_result');
                DeleteOptions(elSel);
                for (i = 0; i < rows.length; i++) {
                    var cols = rows[i].split('^');
                    if (cols[1] != "" && cols[1]!=undefined) {
                        var elOptNew = document.createElement('option');
                        elOptNew.text = cols[0];
                        elOptNew.value = cols[1];
                        elSel.add(elOptNew, elSel.length);
                    }
                }
            }

        }
        document.getElementById('nmemo_alram_set').style.display = 'none';

    }

    function DeleteOptions( obj )  
    {  
        var i ;
        for (i = 0; i < obj.length; i++) {
            obj.remove(i);
        }
    }

    function fn_alram_check() {
        sgIng = true;
        var retVal = '';
        if (httpRequest.readyState == 4) {
            if (httpRequest.status != 200) {

            } else {
                var retvals = httpRequest.responseText;
                var vals = retvals.split("|");
                if (vals.length == 2) {
                    if (vals[0] != "0") {
                        document.getElementById('nmemo_alram_touch').style.display = '';
                    }
                    if (vals[1] != "") {
                        document.getElementById('nmemo_send_call').style.display = '';
                    }
                }
            }

        }
    }

    function fn_memo_send_check() {
        sgIng = true;
        var retVal = '';
        if (httpRequest.readyState == 4) {
            if (httpRequest.status != 200) {
                alert(httpRequest.responseText);
            } else {
                var retvals = httpRequest.responseText;

                if (retvals == "0") {
                    document.getElementById('nmemo_send_set').style.display = 'none';
                    alert('메모가 전달되었습니다');
                }
            }

        }
    }

    function fn_memosendok() {
        var seq = document.getElementById('nmemo_alram_touch_seq').value;
        var UserID = document.getElementById('nmemoUserID').value;
        var TargetUserID = document.getElementById('nmemo_send_set_search_result').value;
        if (sgIng == true) {
            httpRequest = getXMLHttpRequest();
            var httpParams = "eType=sendmemo&nmemosendUserID=" + UserID+"&nmemosendSeq="+ seq +"&nmemosendTargetUserID="+ TargetUserID;
            var httpUrl = "/Menu/Nmemo.aspx";

            httpUrl = httpUrl + "?ver=" + randInt() + "&" + httpParams;

            httpRequest.open("GET", httpUrl, true);
            httpRequest.onreadystatechange = fn_memo_send_check;
            httpRequest.send(null);
            sgIng = false;
        }
    }

    //setTimeout(
    //    function () {
    //        // 알람시계가 떠있거나 도착메모 알림이 떠있으면 체크 하지 않음
    //        if (document.getElementById('nmemo_alram_touch').style.display == 'none' && document.getElementById('nmemo_send_set').style.display=='none') {
    //            if (sgIng == true) {
    //                httpRequest = getXMLHttpRequest();
    //                var httpParams = "eType=alramcheck&alramcheck_UserID=" + document.getElementById('nmemoUserID').value;
    //                var httpUrl = "/Menu/Nmemo.aspx";

    //                httpUrl = httpUrl + "?ver=" + randInt() + "&" + httpParams;

    //                httpRequest.open("GET", httpUrl, true);
    //                httpRequest.onreadystatechange = fn_alram_check;
    //                httpRequest.send(null);
    //                sgIng = false;
    //            }
    //        }

    //        setTimeout(arguments.callee, 300000);
    //    }
    //, 300000);