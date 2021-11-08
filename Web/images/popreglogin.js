document.write('<link href="http://www.wan.com/Public/www/platform/style/popReg/popRegLogin.css" rel="stylesheet">'
    +'<div id="login_reg_pop">'
        +'<div class="tab-tit-pop"><a href=".login_wrap_pop" class="pop-login-bt targt">用户登录</a><a class="pop-reg-bt" href=".reg_wrap_pop">用户注册</a>'
            +'<span class="close-bt" onclick="popCloseFn()"></span>'
        +'</div><div class="login_wrap_pop show">'
           +' <form class="login_form_pop">'
    +'<p class="l-pop"><label>账号:</label><input name="cn" class="pop-input" type="text" autocomplete="off" placeholder="用户账号" valida/></p>'
    +'<p class="l-pop"><label>密码:</label><input id="pop-login-pass" name="pwd" class="pop-input" type="password" autocomplete="off" placeholder="登录密码" valida/></p>'
    +'<p class="l-pop"><input class="pop-login-keep" type="checkbox" checked />下次自动登录<a href="http://www.wan.com/accounts/forget_password.html" target="_blank" class="get-pass">忘记密码?</a></p>'
    +'<a href="#" class="submitBt">马上登录</a>'
    +'<p class="err-show"></p>'
    +'</form>'
    +'</div>'


    +'<div class="reg_wrap_pop">'
    +'<form class="reg_form_pop">'
    +' <p class="l-pop"><label>账号:</label><input name="cn" maxlength="25" class="pop-input" type="text" placeholder="用户账号" vaType="uName" valida/></p>'
    +' <p class="l-pop"><label>密码:</label><input name="pwd" maxlength="25" class="pop-input" type="password" placeholder="登录密码" vaType="uPass" valida /></p>'
    +'<p class="l-pop"><label>确认密码:</label><input id="pop-reg-pass" name="pwd2" maxlength="25" class="pop-input" type="password" placeholder="确认登录密码" vaType="rePass" valida /></p>'
    +'<p class="l-pop" style="margin-bottom:0;"><input class="type-agree" type="checkbox" vaType="uAgre" checked />我已阅读并同意<a href="http://www.wan.com/huodong/agreement/index.html" target="_blank" class="deal-link">《用户注册服务协议》</a></p>'
    +'<a href="#" class="submitBt">马上注册</a>'
    +'<p href="#" style="display:none;"><input type="text" name="reg_type" value="1"><input type="text" id="statisId" name="id" value=""></p>'
    +'<p class="err-show"></p>'
    +'</form>'
    +'</div>'

    +'</div>'
    +'<script src="http://www.wan.com/Public/www/platform/style/popReg/validate.js" type="text/javascript">'+'<\/script>'
    +'<script src="http://www.wan.com/Public/www/platform/style/popReg/eventPopReg.js" type="text/javascript">'+'<\/script>'
    +'');



$.extend({
    //url: 接口(必填)，data: 数据(选填)，callback: 成功回调(选填)， options: 可覆盖默认配置(选填)
    doAjax: function(meth, url, data, callback, options) {

        if (typeof(data) == 'function') {
            options = callback;
            callback = data;
            data = {};
        }

        var opt = {
            type: meth,
            url: url,
            data:data,
            //contentType:'text/html',
            //dataType: 'html',
            timeout: 7000
        };

        //扩展参数
        $.extend(opt, options);

        return $.ajax(opt).done(function(result,status,xhr) {
            if(callback){
                callback(result);
            }

        }).fail(function(jqXHR, textStatus, errorThrown) {
            alert("请求超时，请重试")
            /*
            if(jqXHR.hasOwnProperty('responseText')){
                alert(eval('('+jqXHR.responseText+')').msg);
            }else{
                alert(errorThrown);
            }
            */
        });
    }
});



//生成弹框背景
function createPopBk(isHd){
    var $wrap = $("#pop-bkWrap-lay");
    if(isHd==false){$wrap.hide();return;}
    if($wrap.length==0){
        $wrap = $("<div id='pop-bkWrap-lay'>");
    }else{
        $wrap.show();
        return;

    }


    var _h = $(window).height();
    $wrap.height(_h);
    var _ieH = $(document.body).height();
    $wrap.css({"_height":_ieH});
    $(document.body).append($wrap);
}


//选中登录或注册
function selectLoginOrRegister(value){ 
    createPopBk(true);
    if( value == "login"){ 
        $(".pop-login-bt").addClass("targt").siblings("a").removeClass("targt");
        $(".login_wrap_pop").addClass("show");
        $(".reg_wrap_pop").removeClass("show");
    }else{ 
        $(".pop-reg-bt").addClass("targt").siblings("a").removeClass("targt");
        $(".reg_wrap_pop").addClass("show");
        $(".login_wrap_pop").removeClass("show");
    }
    $("#login_reg_pop").show();
}



//弹框登录回调
function popLoginCallFn(data){
    if(data.state == "1"){ 
        $("body").append(data.script);
        setTimeout(function(){
            window.location.reload(true);
        },500);
    }else{ 
        alert(data.msg);
    }
    
}

//弹框注册回调
function popRegCallFn(data){
    if(data.result == "success"){ 
        $("body").append(data.login);
        setTimeout(function(){
            window.location.reload(true);
        },500);
    }else{ 
        alert(data.result);
    }
    
}

//弹框关闭
function popCloseFn(){
    createPopBk(false);
    $("#login_reg_pop").hide();
    $("#login_reg_pop input").val('');
    $("#login_reg_pop .err-show").text('');

}


//脚本注入页面
function evelScript(data){
   $("body").append(data);
/*
    var scriptS = document.getElementById("scriptList");
    if(scriptS===null){
        scriptS = document.createElement("div");
        scriptS.setAttribute("id","scriptList");
        document.body.appendChild(scriptS);
    }
    scriptS.innerHTML = data;
    var scriptTags = scriptS.getElementsByTagName("script");
    for(var i=0; i<scriptTags.length; i++){
        var _script = document.createElement("script");
        _script.src = scriptTags[i].src;
        _script.setAttribute("reload","1");
        document.body.appendChild(_script);
    }
    scriptS.innerHTML="";
*/

};

