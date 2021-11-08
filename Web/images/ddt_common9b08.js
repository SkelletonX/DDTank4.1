/*
    新弹弹堂 迁移 wan平台 公共脚本
*/
var DDT_COM = DDT_COM || {};
var D2_Util = {};
DDT_COM.gameName = "ddt";
DDT_COM.ddtServerListMap = {};
var DDT_ACTION = {
	gid : "26",
    checkLogin:"/Index/isLogin",
	checkLogin2:"http://www.wan.com/index.php/accounts/isLogin.html",
	login2 : "http://www.wan.com/index.php/accounts/checklogin.html",
    loginIn:"/Public/Checklogin1",
    gameurl:"http://www.wan.com/game/play?sid=",
    loginOut:"/Public/loginout",
    checkUserName:"http://www.wan.com/platform/checkUserName.action",
    checkVerifyCode:"http://www.wan.com/platform/checkVerifyCode.action",
    CheckLoginGame:"http://www.wan.com/platform/onlineGame/loginGame.action"
};

/*---------------------------------------------------------------------------------------------
    用户 登录，退出 相关操作
---------------------------------------------------------------------------------------------*/
DDT_COM.userLoginOperate = {

     //判断是否登录
    checkLogin : function(){
		
		$.getJSON(DDT_ACTION.checkLogin2+"?jsonpCallback=?",function(data){
			data = typeof data == "string" ? eval("("+data+")") : data;
			if(data.state == "1"){

                DDT_COM.loginBox.logged(data);
            }else{
                DDT_COM.loginBox.loginOut(data);
            }
		});
		
		/*
        $.post(DDT_ACTION.checkLogin,{},function(data){
            data = typeof data == "string" ? eval("("+data+")") : data;
            if(data.state == "1"){

                DDT_COM.loginBox.logged(data);
            }else{
                DDT_COM.loginBox.loginOut(data);
            }
        })*/
    },
    //用户 登录
    login : function( inName, password1,remember,successCallback, errorCallback){
		/*
		$.getJSON(DDT_ACTION.login2+"?cn="+inName+"&pwd="+password1+"&callBack=?",function(){
			data = typeof data == "string" ? eval("("+data+")") : data;
			if(data.state == "1"){
                $("body").append(data.script);
                setTimeout(function(){
                    window.location.reload();
                },500)
            }else{
                alert(data.msg);
            }
		});*/
		
		
		
		password1 = encodeURIComponent(password1);
        $.getJSON(DDT_ACTION.login2+"?cn="+inName+"&pwd="+password1+"&jsonpCallback=?",function(data){
            if(data.state == "1"){
                $("body").append(data.script);
                setTimeout(function(){
                    window.location.reload();
                },500)
            }else{
                alert(data.msg);
            }
        });
    },

    //用户 退出
    loginOut : function( successCallback, errorCallback ){
        $.post(DDT_ACTION.loginOut,{},function(data){
            data = typeof data == "string" ? eval("("+data+")") : data;
            if(data.state == "1"){
                $('body').append(data.script);
                setTimeout(function(){
                    window.location.reload();
                },500);
            }
        });

    },
    //取服务器列表
    getServerList : function(){
        $.post(DDT_ACTION.getNewServer,{},function(data){   //无参数表示获取全部，c:1表示取一条
            $.each(data,function(i,n){
                if(data[i].unstart == 0){
                    if($(".first-val").length > 0){     //填充排行榜服务器列表
                        $(".f-first-ul").append("<li servernum="+data[i].sid+" gameid="+data[i].gid+">"+data[i].servername+"</li>");
                    }
                    DDT_COM.ddtServerListMap[data[i].line] = data[i].url;
                }
            });
            var li = $(".f-first-ul li:first");
            $(".first-val").text(li.text()).attr({"servernum":li.attr("servernum"),"gameid":li.attr("gameid")});
            D2_Util.loadRankHtml($(".first-val").attr("servernum"),$(".second-val").text());
        });
    }
};

//公共左上角登录框的相关操作
DDT_COM.loginBox = {
    
    //登录了，改变状态
    logged : function( data ){
        var $userLogin = $(".unlogin-box");
        var $userLogged = $(".login-box");
        
        var $username = $("#user");
        var $pwd = $("#userPass");
        
        var $dataUser = $(".userVal");
        
        var $latestSeverLink = $(".lastLogin");

        $dataUser.html(data.datas.nickname);
        var latestSeverHtml= "暂无记录！";
        if( typeof data.datas.recently == "object"){
			$.each(data.datas.recently,function(i,n){
				if(n.gid == DDT_ACTION.gid){
					latestSeverHtml = n.servername;
					$latestSeverLink.attr({"href":DDT_ACTION.gameurl+n.sid}).html(latestSeverHtml);
					if($(".sub-my-ser").length>0){ // 选服页，我的服务器
						$(".my-box").html('<a href="'+DDT_ACTION.gameurl+n.sid+'" target="_blank">'+latestSeverHtml+'</a>');
					}
					return false;
				}
			});
		}else{
            $latestSeverLink.attr({"href":"javascript:void(0)"}).html(latestSeverHtml);
            if($(".sub-my-ser").length>0){ // 选服页，我的服务器
                $(".my-box").html(latestSeverHtml);
            }
        }



        
        //清掉密码
        $pwd.val("");
    },
    
    //退出了，改变状态
    loginOut : function(data){
        var $userLogin = $(".unlogin-box");
        var $userLogged = $(".login-box");
        this.inputState();
    },
    
    //两个输入框的状态
    inputState : function(){
        var $loginUsername = $("#uid");
        var $loginPassword = $("#pwd");
        var $loginUsernameLabel = $loginUsername.prev();
        var $loginPasswordLabel = $loginPassword.prev();
        
        //绑定 帐号，密码input框事件
		/*
        if( WAN_COM.formCheck.isEmpty( $loginUsername.val() ) ){
            $loginUsernameLabel.show();
            $loginPasswordLabel.show();
        }*/
    },
    
    //初始化
    init : function(){
        DDT_COM.userLoginOperate.checkLogin(function(data){
            DDT_COM.loginBox.logged(data);
        }, function(data){
            DDT_COM.loginBox.loginOut(data);
        });
    }//end init

};

//根据返回值，得到最近进游戏的游戏地址
DDT_COM.getGameUrl = function(serverId){

    // 平台所有进游戏，都是这种写法。
    return DDT_ACTION.gameurl+"?serverid="+serverId;

};

// 生成快速选服映射对象
DDT_COM.numberServerMap = {
  "7road":{},
  "37ww":{}
};



/*---------------------------------------------------------------------------------------------
    左侧 服务器列表 初始化
---------------------------------------------------------------------------------------------*/
DDT_COM.sideServerListInit = function(){

  
    var $serverListWrap = $(".recom-box ul");

};

/*---------------------------------------------------------------------------------------------
    快速选服
---------------------------------------------------------------------------------------------*/
DDT_COM.quickSelectServer = function( $input,platName ){
    var v = $.trim($input.val());

    if( !/^\d+$/.test( v ) ){
        alert("请输入数字，进行选服");
        return false;
    }else if(!DDT_COM.ddtServerListMap[v]){
        alert("您输入的区服不存在!");
        return false;
    }
    //进游戏
    window.open(DDT_COM.ddtServerListMap[v]);


   /* var maxVal = 0;
    if($('.recom-box').length>0){
      maxVal = Number($('.recom-box li:first').children('a').text().match(/\d+/)[0]);
    }else{
      maxVal =  Number($(".recommend-box").children('a').text().match(/\d+/)[0]);
    }
   
    if( !/^\d+$/.test( v ) ){
        alert("请输入数字，进行选服");
        return false;
    }else if(Number(v) > maxVal){
        alert("您输入的区服不存在!");
        return false;
    }*/
    /*v = DDT_COM.numberServerMap[platName][v];
    var gameurl = DDT_COM.getGameUrl(v);*/
    

};



//初始化
$(function(){
  DDT_COM.loginBox.init();
  //DDT_COM.sideServerListInit();
});



// 添加分割数组函数
DDT_COM.sliceArray = function(arr,size){
  var arrNum = Math.ceil(arr.length/size);
  var newArr = [];
  for(var i=0;i<arrNum;i++){
    newArr.push(arr.slice(size*i,size*(i+1)));
  };
  return newArr;
}

// 自动生成选项卡tab切换菜单(1-50服，51-100服..) (50,123,"span")
DDT_COM.autoCreateMenuObj = function (count,tagName){

  var num = Number(DDT_COM.sliceTabVal);
  count = Number(count);
  var y = Math.ceil(count/num);
  var z = [];
  for(var i =0; i<y;i++){
    if(i==y-1){
      z.push("<"+tagName+" class='select'>"+(num*i+1)+"-"+num*(i+1)+"服</"+tagName+">");
    }else{
      z.push("<"+tagName+">"+(num*i+1)+"-"+num*(i+1)+"服</"+tagName+">");
    }
    
  };
  return z.reverse().join('');
};





// 设置选服页选项卡tab菜单分割值
DDT_COM.sliceTabVal = 10;


/* 调用wanCommon.js统计代码 */
$(document).ready(function () {

  //WAN_COM.gameCounts && WAN_COM.gameCounts({gameCode: "ddt"});
    
});











