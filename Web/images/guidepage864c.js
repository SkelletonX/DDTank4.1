var SQ_ACTION = {
    checkLogin:"/Index/isLogin",
    register:"http://www.wan.com/Accounts/doRegisterFast",
    loginOut:"/Public/loginout",
    checkUserName:"http://www.wan.com/Accounts/username_check",
    getNewServer:"/index/get_new_server"
};

var flag = false;
var xmlHttpReq = null;
var siteId="";
var adId="";
var isIE = '\v' === 'v',
    isIE6 = isIE && !window.XMLHttpRequest;

var guide_page_g_param = {
    lastTimeServerId : "",
    newServerName : "",
    userName:""
}
function stopEvt(e){
	var e = arguments.callee.caller.arguments[0] || e || window.event;
	if(e.preventDefault){
		e.preventDefault();
		e.stopPropagation();
	}else{
		window.event.returnValue = false;
		window.event.cancelBubble = true;
	}
	return false;
}
var EventUtil = {
    addHandler : function(element,type,handler){
        if(element.addEventListener){
            element.addEventListener(type,handler,false);   
        }else if(element.attachEvent){
            element.attachEvent("on"+type,handler)
        }else{
            element["on"+type]=handler; 
        }
    },
    removeHandler:function(element,type,handler){
        if(element.removeEventListener){
            element.removeEventListener(type,handler,false);    
        }else if(element.detachEvent){
            element.detachEvent("on"+type,handler); 
        }else{
            element["on"+type]=null;    
        }
    },
    getEvent:function(event){
        return event ? event : window.event;
    },
    getTarget:function(event){
        return event.target || event.srcElement;
    },
    preventDefault:function(event){
        if(event.preventDefault){
            event.preventDefault(); 
        }else{
            event.returnValue = false;  
        }
    },
    setCookie : function(name,value,time){
        var strsec = getsec(time);
        var exp = new Date();
        exp.setTime(exp.getTime() + strsec*1);
        document.cookie = name + "="+ escape (value) + ";expires=" + exp.toGMTString();
    },
    getCookie : function(name){
        var arr,reg=new RegExp("(^| )"+name+"=([^;]*)(;|$)");
        if(arr=document.cookie.match(reg)){
            return (arr[2]);
        }else{
            return null;
        }
    },
    findPrevSibling : function(obj){
        obj = obj.previousSibling;
        while(obj.nodeType != 1){
            obj = obj.previousSibling;
        }
        if(obj.nodeType == 1){
            return obj
        }
    },
    findNextSibling : function (obj){
        obj = obj.nextSibling;
        while(obj.nodeType != 1){
            obj = obj.nextSibling;
        }
        if(obj.nodeType == 1){
            return obj
        }
    },
    hide : function(objs){
        for(var i=0;i<arguments.length;i++){
            arguments[i].style.display = "none";
        }
        
    },
    show : function(objs){
        for(var i=0;i<arguments.length;i++){
            arguments[i].style.display = "block";
        }
    }
};


var ldyConfig = {
    gameCode: "sq",
    playInfo: "http://play.wan.com/userinfo/playInfo",          /* ??????????????????????????????????????? */
    serverUrl: "http://play.wan.com/server/lastest",
    quickReg: "http://passport.wan.com/reg/quickReg",           /* ??????????????? */
    //quickReg: "http://passport.wan.com/reg/certReg",           /* ???????????? */
    isReg: "http://passport.wan.com/reg/isReg",                 /* ???????????? */
    server: "http://sq.wan.com/webgame.shtml?server=",          /* ???????????????????????????????????? */
    login: "http://passport.wan.com/login/login",               /* ??????????????? ?????? */
    logoutUrl: "http://passport.wan.com/logout",
    playGame: "http://play.wan.com/playing/playGame?game=sq&server=",    
    gMUrl: "http://log.wan.com/t.htm",
    neverLoginSrc: "http://sq.wan.com/"                         /* ???????????????????????????????????? */
};

var ldyResultMsg = {
    "0" : "??????",
    "1" : "??????????????????????????????",
    "2" : "?????????????????????",
    "4" : "???????????????????????????",
    "5" : "???????????????????????????",
    "6" : "???????????????",
    "8" : "??????????????????????????????????????????",
    "9" : "??????????????????????????????",
    "10" : "????????????????????????????????????",
    "11" : "????????????????????????",
    "12" : "????????????????????????",
    "13" : "????????????????????????",
    "14" : "?????????????????????",
    "15" : "????????????????????????",
    "16" : "?????????????????????????????????????????????",???? 
    "22" : "???????????????",
    "23" : "????????????????????????",
    "24" : "??????????????????????????????", ?????? 
    "25" : "?????????????????????????????????",
    "26" : "??????????????????",
    "27" : "?????????????????????",
    "28" : "???????????????",
    "30" : "???????????????",
    "31" : "?????????????????????????????????",
    "36" : "?????????????????????????????????",
    "37" : "??????????????????",
    "38" : "???????????????",
    "39" : "?????????????????????",
    "40" : "??????????????????",
    "41" : "??????????????????",
    "46" : "?????????????????????????????????",
    "47" : "?????????????????????????????????",
    "98" : "?????????????????????????????????",
    "99" : "?????????????????????????????????"
};

try{
    ldyConfig.uf = location.search.split("?")[1].split("=")[1]; 
    if (ldyConfig.uf) {
        EventUtil.setCookie("uf", ldyConfig.uf, "d1");
    }
}catch(e){
    ldyConfig.uf = "";
}

/* ?????? */
function certReg(obj) {
    $.ajax({        
        url: ldyConfig.quickReg,
        data: {
            "cn" : obj.cn, //????????????
            "pwd" : obj.pwd, //????????????
            "pwd2" : obj.pwd2, //????????????
            "name" : encodeURIComponent(obj.name),
            "cert" : obj.cert,
            "code" : obj.code,
            "uf" : ldyConfig.uf
        },
        dataType: "jsonp",
        success: function (data) {          
            if (data.result == 0) { //????????????
                $.ajax({
                    url: ldyConfig.serverUrl,
                    data: {game: ldyConfig.gameCode},
                    dataType: "jsonp",
                    success: function (data) {
                        if (data.result == 0) { //????????????
                            obj.success && obj.success(data);                            
                        } else { //??????????????????
                            statusAlt(data);                  
                        }
                    }
                });

            } else {
                if (data.result == 22) {
                    $(".reg_codeimg").attr("src", "http://passport.wan.com/captcha/gif?r=" + (+new Date)); /* ??????????????? */
                }
                statusAlt(data);
            }
        }
    });
    
}

function send(u, i) {

    var e = document.getElementById(i);

    if (e == null) {

        e = document.createElement("script");

        e.id = i;

        e.type = "text/javascript";

        e.async = true;

        e.src = u;

        var b = document.getElementsByTagName("body")[0];

        b.appendChild(e);

    }

}

function arrive() {

    if (ldyConfig.uf && ldyConfig.uf != "") {

        var u = ldyConfig.gMUrl + "?way=0&uf=" + ldyConfig.uf + "&l=&s=&gameCode=sq&groupIndex=4001&r=" + Math.round(Math.random() * 3364721474);

        send(u, '_yy_a');

    }

}

arrive();

function trigger() {
    if (ldyConfig.uf && ldyConfig.uf != "") {
        var u = ldyConfig.gMUrl + "?way=1&uf=" + ldyConfig.uf + "&l=&s=&gameCode=sq&groupIndex=4001&r=" + Math.round(Math.random() * 3364721474);
        send(u, '_yy_t');
    }

}

/* ??????????????? */
$(".content").one("mousedown", function () {
    trigger();
});

/* ?????? */
function wanLogin(obj) {
    obj = obj || {};

    var cn = obj.cn;
    var pwd = obj.pwd;
    //???????????????true???false????????????true??????????????????7??????False????????????????????????????????????
    var remember = typeof obj.remember == "undefined" ? true : obj.remember;
    //captcha ?????????
    var code = obj.code || '';
    //?????????????????????????????????????????????????????????????????????????????????
    var url = obj.url || 'http://www.wan.com/';

    var successCallback = obj.success;
    var failCallback = obj.fail;

    //var interfaceUrl = code == '' ? WAN_COM.config.url.login : WAN_COM.config.url.captLogin;
    $.ajax({
        url: ldyConfig.login,
        data: {
            cn: cn,
            pwd : pwd,
            remember : remember,
            code : code,
            url : url
        },
        dataType: "jsonp",
        success: function (data) {
            var result = data.result;

            if( result == 0 ){
              //??????????????????
              payJsonp();
              
              //????????????                  
              successCallback && successCallback(data);
            }else{
              //????????????
              failCallback && failCallback(data);
            }
        }
    });

}

/* ?????????????????? */
function statusAlt(data) {
    var txt = ldyResultMsg[data.result];
    if (txt) {
       alert(txt);
    }
}

function payJsonp() {
    $.getJSON("http://pay.wan900.com/dologin.jsp?callback=?", {
        "userid" : EventUtil.getCookie("userid"),
        "username" : EventUtil.getCookie("username"),
        "address" : EventUtil.getCookie("address"),
        "nickname" : EventUtil.getCookie("nickname"),
        "userfrom" : EventUtil.getCookie("userfrom"),
        "timeflag" : EventUtil.getCookie("timeflag"),
        "sign" : EventUtil.getCookie("sign")
    }, function(data){});
}

function  $id(obj){
    return document.getElementById(obj);
}
function redColor(obj,color){
    return obj.style.color = color;
}
function promptTxt(obj,msgType){
    if(msgType != "truemsg"){
        obj.style.display = "block";
        EventUtil.findNextSibling(obj).style.display = "block";
        EventUtil.findNextSibling(obj).className = "errorImg";
        return obj.innerHTML=obj.getAttribute(msgType);
    }else{
        obj.style.display = "none";
        EventUtil.findNextSibling(obj).style.display = "block";
        EventUtil.findNextSibling(obj).className = "trueImg";
        
    }
    
    
}
function editClass(obj,className){
    return obj.className = className;
}



function getsec(str){
   var str1=str.substring(1,str.length)*1;
   var str2=str.substring(0,1);
   if (str2=="s"){
        return str1*1000;
   }else if (str2=="h"){
       return str1*60*60*1000;
   }else if (str2=="d"){
       return str1*24*60*60*1000;
   }
}

if(getUrlParam("adId")!=null){
    EventUtil.setCookie("adId",getUrlParam("adId"),"d7");
}
if(getUrlParam("site")!=null){
    EventUtil.setCookie("siteId",getUrlParam("site"),"d7");
}


EventUtil.addHandler($id("btntoReg"),"click",registerLogic);
EventUtil.addHandler($id("reg_pwd"),"keydown",preventKey);
//EventUtil.addHandler($id("recommend-servername"),"click",gotoLastServer);//20150203
EventUtil.addHandler($id("logout"),"click",logout);

$("#regBox .retInput").on("keyup", function (e) {
    if (e.keyCode == 13) {
        $(this).blur();
        registerLogic();
    }
});

//?????????????????????
if($id("reg_confirm_pwd")){
    EventUtil.addHandler($id("reg_uid"),"blur",checkuserName);
    EventUtil.addHandler($id("reg_pwd"),"blur",pwdBlur);
    EventUtil.addHandler($id("reg_confirm_pwd"),"blur",confirmPwdBlur);
}else{
    EventUtil.addHandler($id("reg_uid"),"blur",checkuserName);
    EventUtil.addHandler($id("reg_pwd"),"blur",pwdBlur);
}

function openRegBox(){
    EventUtil.hide($id("btntoLogin"),$id("registerAgain"));
    EventUtil.show($id("btntoReg"),$id("tab2_1"),$id("tab2_2"),$id("tab2_3"));
}

function logout(){  
    $.post(SQ_ACTION.loginOut,{},function(data){
        data = typeof data == "string" ? eval("("+data+")") : data;
        if(data.state == 1){
            $("body").append(data.script);
            setTimeout(function(){
                window.location.reload();
            },500);
        }else{
            alert(data.msg);
        }
    });
}

function registerLogic(){
    var userName=$id("reg_uid").value,pwd=$id("reg_pwd").value;
    var confirm_pwd="";
    if($id("reg_confirm_pwd")){confirm_pwd=$id("reg_confirm_pwd").value;}else{confirm_pwd=$id("reg_pwd").value;}
    if($id("agreement").checked!=true){
        alert("??????????????????????????????????????????????????????");
        return false;
    } else if(isRegisterOk(userName,pwd,confirm_pwd)){
        /* 
        //??????????????????
        if (!realReg("real_name")) {
            EventUtil.findNextSibling($id("tab2_4")).style.display = "block";
            EventUtil.findNextSibling($id("tab2_4")).className = "errorImg";
            return false;
        } else if (!numReg("id_number")) {
            EventUtil.findNextSibling($id("tab2_5")).style.display = "block";
            EventUtil.findNextSibling($id("tab2_5")).className = "errorImg";
            return false
        } else if (realReg("real_name")) {
            EventUtil.findNextSibling($id("tab2_4")).style.display = "block";
            EventUtil.findNextSibling($id("tab2_4")).className = "trueImg";
        } else if (numReg("id_number")) {
            EventUtil.findNextSibling($id("tab2_5")).style.display = "block";
            EventUtil.findNextSibling($id("tab2_5")).className = "trueImg";
        } else if ($id("reg_code").value == "") {
            alert("??????????????????");
            return false;
        }
        
        registerFunction(userName,pwd,confirm_pwd, $id("real_name").value ,$id("id_number").value,$id("reg_code").value);
        */

        //????????????ID??????
        /*
        if(getParamValue("pzid")){ 
            $.getJSON("http://ados.wan.com/stat/pz/?pzid="+getParamValue("pzid")+"&tag=reg",function(data){ 
                // nothing
            });
        }
		*/
        registerFunction(userName,pwd,confirm_pwd);  
    }
}


function pwdBlur(){
    if($id("reg_pwd").value.length>3 && $id("reg_pwd").value.length<17 && $id("reg_pwd").value.split(" ").length<2){
        promptTxt($id("tab2_2"),"truemsg");
        editClass($id("tab2_2"),"trueClass");
        flag=true;
    }else{
        promptTxt($id("tab2_2"),"errormsg");
        editClass($id("tab2_2"),"errorClass");
        flag=false;
    }
}


function confirmPwdBlur(){
    var pwd2 = $id("reg_confirm_pwd").value,pwd1 = $id("reg_pwd").value;
    if(pwd1.length>3 && pwd1.length<17){
        if(pwd2!=pwd1){
            promptTxt($id("tab2_3"),"unequallymsg");
            editClass($id("tab2_3"),"errorClass");
            flag=false;
        }else{
            promptTxt($id("tab2_3"),"truemsg");
            editClass($id("tab2_3"),"trueClass");
            flag=true;
        }
    }
}

function preventKey(event){
    var lKeyCode = window.event ? event.keyCode : event.which;  
    if(lKeyCode=="13"){
        EventUtil.preventDefault(event);
        registerLogic();
        
    }
}
function preventKeyDown(event){
    var lKeyCode = window.event ? event.keyCode : event.which;  
    if(lKeyCode=="13"){
        EventUtil.preventDefault(event);
        loginFun();
        
    }
}
function isRegisterOk(userName,pwd,confirm_pwd){
    var usernameReg=/^[a-zA-Z]{1}([a-zA-Z0-9]|[_]){5,19}$/;
    var regRepeat = /([a-z0-9A-Z_])\1{4,}/;
    var flag=true;
    if(!userName){
        promptTxt($id("tab2_1"),"nullmsg");
        editClass($id("tab2_1"),"errorClass");
        flag=false;
        return false;
    }
    /*??????????????????*/
    if(!usernameReg.test(userName)){
        promptTxt($id("tab2_1"),"errormsg");
        editClass($id("tab2_1"),"errorClass");
        flag=false;
        return false;
    }
    if(regRepeat.test(userName)){
        promptTxt($id("tab2_1"),"fiveerrormsg");
        editClass($id("tab2_1"),"errorClass");
        flag=false;
        return false;
    }
    if(pwd.split(" ").length>1){
        promptTxt($id("tab2_2"),"errormsg");
        editClass($id("tab2_2"),"errorClass");
        flag=false; 
        return false;   
    }
    if(confirm_pwd.split(" ").length>1){
        promptTxt($id("tab2_3"),"errormsg");
        editClass($id("tab2_3"),"errorClass");
        flag=false; 
        return false;
    }
    if(!pwd){
        promptTxt($id("tab2_2"),"errormsg");
        editClass($id("tab2_2"),"errorClass");
        flag=false; 
        return false;   
    }
    if($id("reg_confirm_pwd")){//?????????????????????
        if(confirm_pwd!=pwd){
            promptTxt($id("tab2_3"),"unequallymsg");
            editClass($id("tab2_3"),"errorClass");
            flag=false;
            return false;
        }

    }else{
        if(!pwd){
            promptTxt($id("tab2_2"),"nullmsg");
            editClass($id("tab2_2"),"errorClass");
            flag=false;
            return false;
        }
    }   
    return flag;
}



//???????????????????????????
function checkuserName(){
    var that = $id("reg_uid").value;
    var regRepeat = /([a-z0-9A-Z_])\1{4,}/;
    var unauthorizedNames=['gamemaster','gm','shit','bitch','fvc','phuc','fuk','shenqu','fuck','admin','7road'];
    flag=true;
    if(!that){
        promptTxt($id("tab2_1"),"nullmsg");
        editClass($id("tab2_1"),"errorClass");
        return false;	
    }else if(!/^[a-zA-Z]{1}([a-zA-Z0-9]|[_]){5,19}$/.test(that)){
        promptTxt($id("tab2_1"),"errormsg");
        editClass($id("tab2_1"),"errorClass");
        return false;
    }else if(that.length>20 || that.length<6){
        promptTxt($id("tab2_1"),"errormsg");
        editClass($id("tab2_1"),"errorClass");
        return false;
    }else if(regRepeat.test(that)){
        promptTxt($id("tab2_1"),"fiveerrormsg");
        editClass($id("tab2_1"),"errorClass");
        return false;
    }else{
        for(i=0;i<unauthorizedNames.length;i++){
            if(that.indexOf(unauthorizedNames[i])>-1){
                promptTxt($id("tab2_1"),"unrulemsg");
                editClass($id("tab2_1"),"errorClass");
                flag=false;
            }
        };
        if(flag){
            /* ??????????????? */
            userNameCheck({
                cn: $("#reg_uid").val(),
                error: function (data) {
                    promptTxt($id("tab2_1"),"existmsg");
                    editClass($id("tab2_1"),"errorClass");

                },
                success: function (data) {
                    promptTxt($id("tab2_1"),"truemsg");
                    editClass($id("tab2_1"),"trueClass");         
                }
            });

        }
    }
}

function userNameCheck(obj) {
    /* ??????????????? */         
    $.ajax({                
        url: SQ_ACTION.checkUserName,
        data: {"u": obj.cn}, //????????????
        dataType: "jsonp",
        success: function (data) {
            data = typeof data == "string" ? eval("("+data+")") : data;
            if(data.state == 1){
                obj.success && obj.success(data);
            }else{
                alert(data.msg);
                obj.error && obj.error(data);
            }
        }
    });
}

function urlReturnId(){ 
	var _urlobj = { 
		"lyb.7road.com":"4719",
		"sq.7road.com":"4720",
		"ddt.7road.com":"4721",
		"mhtl.7road.com":"4722",
		"sq4.7road.com":"4723",
		"szhg.7road.com":"4724",
		"jy.7road.com":"4725",
		"wzzh.7road.com":"4726",
		"xddt.7road.com":"4727"
	};
	var _hostname = document.location.hostname;
	return  _urlobj[_hostname];
}

//????????????
function registerFunction(userName,pwd,confirm_pwd, name, cert, code){
	var _adid = getParamValue("pzid") ? getParamValue("pzid") : urlReturnId();
    $.ajax({        
        url: SQ_ACTION.register,
        data: {
            "username" : userName, //????????????
            "password" : encodeURIComponent(pwd), //????????????
            "password2" : encodeURIComponent(confirm_pwd), //????????????
            "from_uid" : $("#from_uid").val(),
            "adid" : _adid
        },
        dataType: "jsonp",
        success: function (data) {          
            data = typeof data == "string" ? eval("("+data+")") : data;
            if(data.state == "1"){
                $("body").append(data.script);
                setTimeout(function(){
                    window.location.reload();
                },500);
            }else{
                alert(data.msg);
            }
        }
    });
}

function registerSucceed(data,userName){
    //*********START*********
    (function(){   
        var __UX = {
            id: 800004250,
            sendQuery: "http://",
            script: "cmsg.php",
            type: 2,
            from: "client", //????????????????????????????????????????????????client????????????ip???????????????
            ip:"",
            defaultAid: "d41d8cd98f00b204e9800998ecf8427e",
            defaultServer: "s1.uxfenxi.com"
        };

        if (typeof UX_params != 'undefined' && UX_params && typeof UX_params.aid != 'undefined' && typeof UX_params.server != 'undefined') {
            __UX.defaultAid = UX_params.aid;
            __UX.defaultServer = UX_params.server;
        }
        __UX.sendQuery += __UX.defaultServer + "/" + __UX.script + "?cid=" + __UX.id + "&aid=" + __UX.defaultAid;
        __UX.sendQuery += "&type=" + __UX.type;
        __UX.sendQuery += "&ip=" + __UX.ip;
        __UX.sendQuery += "&from=" + __UX.from;
        __UX.sendQuery += "&rnd=" + Math.random();
        var a = new Image();
        a.src = __UX.sendQuery;  //send
    })();
    //*********END*********    
    $id("syncBbs").innerHTML = data.bbsData;
    EventUtil.setCookie("userInputVal",userName,"d7");
    EventUtil.setCookie("_niepslength",$id('reg_pwd').value.length,"d7");
    //????????????
    gotoLastServer();
}


function gotoLastServer(){
    createXHR();
    if(xmlHttpReq!=null){
        xmlHttpReq.open("get",urlAction.getLatestServerUrl,false);
        xmlHttpReq.onreadystatechange = function(){
            if(xmlHttpReq.readyState==4){
                if(xmlHttpReq.status>=200 && xmlHttpReq.status <300 || xmlHttpReq.status == 304){
                    var data = eval("("+xmlHttpReq.responseText+")")
                    if(Number(data.id)>0){
                        var checkServerId = data.id;
                        guide_page_g_param.newServerId = data.id;
                        checkGameLogin(checkServerId);
                    }else{
                        
                    }
                }else{
                    alert("??????????????????????????????") 
                }
            }   
        }
        xmlHttpReq.send(null);
    }
}

function checkGameLogin(checkServerId){
    createXHR();
    //var noUriGameUrl =gameUrl;
    gameUrl = addURI(urlAction.checkLoginGame,"serverId",checkServerId);
    if(xmlHttpReq!=null){
        xmlHttpReq.open("get",gameUrl,false);
        xmlHttpReq.onreadystatechange = function(){
            if(xmlHttpReq.readyState==4){
                if(xmlHttpReq.status>=200 && xmlHttpReq.status <300 || xmlHttpReq.status == 304){
                    var data = eval("("+xmlHttpReq.responseText+")")
                    if(Number(data.code)==0){//????????????
                        setTimeout(function(){document.location.href =  urlAction.newgameurl+checkServerId.toString()+"&timestamp="+(+new Date());},500)
                    }else{//?????????????????????????????????
                        setTimeout(function(){document.location.href =  "http://sq.wan.com/";},500)
                    }
                }else{
                    alert("??????????????????????????????") 
                }
            }   
        }
        xmlHttpReq.send(null);
    }
}

function checkLoginFun(success){
    $.post(SQ_ACTION.checkLogin,{},function(data){
        data = typeof data == "string" ? eval("("+data+")") : data;
        if(data.state == 1){
            success(data);
        }
    });
}

checkLoginFun(function (data) {
    var last_server = $("#last-server");
    var latestSeverHtml;
    $("#welBox").show();
    $("#regBox").hide();
    $("#user-name").html(data.datas.nickname);
   
    var newServerUrl,newServerName;
    $.post(SQ_ACTION.getNewServer,{c:5},function(obj){
        if(obj.length > 0){
            for(var i=0;i<obj.length;i++){
                if(obj[i].unstart == 0){
                    newServerUrl = obj[i].url;
                    newServerName = obj[i].servername;
                    break;
                }
            }

        }
        if( typeof data.datas.recently == "object"){
			var url = "http://www.wan.com/game/play/id/"+data.datas.recently.sid+".html";
            latestSeverHtml = data.datas.recently.servername;
            last_server.html(latestSeverHtml).attr("href", url);
            $("#recommend-servername").html(newServerName).attr("href",newServerUrl);
        }else{
            if(newServerUrl){
                window.location.href = newServerUrl;
            }
        }
    });   
});

/* ??????????????? */
function getServer(callback) {
    $.ajax({
        url: ldyConfig.serverUrl,
        data: {game: ldyConfig.gameCode},
        dataType: "jsonp",
        success: function (json) {
            if (json.result == 0) {                 
                callback && callback(json);
            }
        }
    });
}






function createXHR(){
    if(window.ActiveXObject){
        xmlHttpReq = new ActiveXObject("Microsoft.XMLHTTP");
    }else if(window.XMLHttpRequest){
        xmlHttpReq =  new XMLHttpRequest();
    }
}

function addURI(url,name,val){
    url += (url.indexOf("?")==-1 ? "?" : "&");
    url += encodeURIComponent(name) + "=" + encodeURIComponent(val);
    return url;
}

function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function createRecommendAccount(str){
    var _randomLen = 20 - str.length,
        _loopLen = _randomLen>3 ? 3 : _randomLen;
    if(_randomLen==0){
        str = str.substring(0,str.length-3);
        _loopLen = 3;
    }
    for(var i=0;i<_loopLen;i++){
        str = str + Math.floor(Math.random()*10); 
    }
    return str;
};


var titleOne=document.title,titleTwo=window.location.href,titleIndex=0;;
titleTwo=titleTwo.replace("http://","");
function showhTitle(){
    if(titleIndex==0) {
        document.title = titleOne;
        titleIndex = 1 ;
    }else{
        document.title = titleTwo;
        titleIndex = 0 ;
    }
}
//setInterval(showhTitle,500);20150202

function setObjFocus(obj){
    return obj.focus();
}

function createRandomPwd(len){
    len = Number(len);
    var val = "";
    for(var i=0; i<len; i++){
        val += Math.floor(Math.random()*10);
    }
    return val;
}


var _t = false,
    showDivBorder;

function DDTstartgame(){
    
    if($id("welBox").style.display != "none"){  //??????????????????,????????????
        window.open( $("#last-server").attr("href") );
    }else{
        window.open("http://ddt.wan.com/server/index.html");
    }
}

//EventUtil.addHandler($id("start-flash"),"mouseup",forGameStart);
EventUtil.addHandler($id("reg_uid"),"keypress",nime);

function nime(){
    
    clearInterval(showDivBorder);
    $id("regBox").className = "regBox";

}





// ??????????????????
function getParamValue  (name) { 
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i"); 
    var r = window.location.search.substr(1).match(reg); 
    if (r != null) return unescape(r[2]); return null; 
}
if(getParamValue("pzid")){ 
    $.getJSON("http://ados.wan.com/stat/pz/?pzid="+getParamValue("pzid")+"&tag=visit&callback=?",function(data){ 
        // nothing
    });
    $.getJSON("http://ados.wan.com/stat/pz/?pzid="+getParamValue("pzid")+"&tag=click&callback=?",function(data){ 
        // nothing
    });

    $(".enter-home").attr("href","http://ddt.wan.com/index?pzid="+getParamValue("pzid"));
}

