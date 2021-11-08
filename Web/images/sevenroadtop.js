document.write('<style type="text/css">' +
//'html { 	-webkit-filter: grayscale(100%); 	-moz-filter: grayscale(100%); 	-ms-filter: grayscale(100%); 	-o-filter: grayscale(100%); 	filter:progid:DXImageTransform.Microsoft.BasicImage(grayscale=1); 	_filter:none; } '+

'body,div,h2,p,em,span,a,ul,li{ margin: 0; padding: 0;}' +
'ul,li{ list-style: none;}' +
'em,cite{ font-style: normal;}' +
'img{ border: none;}' +
'.clearfix{' +
'    zoom: 1;' +
'}' +
'.clearfix:after{' +
'    visibility: hidden;' +
'    clear: both;' +
'    font-size: 0;' +
'    display: block;' +
'    content: "";' +
'    height: 0;' +
'}' +
'.sevenRoad-top{' +
'    height: 43px;' +
'    background: #fff;' +
'    font-size: 12px;' +
'    font-family: "Microsoft YaHei";' +
'}' +
'.sR-top{' +
'    height: 41px;' +
'}' +
'.sR-bottom{' +
'    height: 1px;' +
'    border-top: 1px solid #e9e9e9;' +
'    overflow: hidden;' +
'}' +
'.sR-nav{' +
'    width: 1000px;' +
'    margin: 0 auto;' +
'}' +
'.sR-logo,.sR-banner,.sR-login,.sR-shortcut{float: left;}' +
'.sR-logo{' +
'    position: relative;' +
'    z-index: 901;' +
'    margin: 6px 0 0 6px;' +
'    _display: inline;' +
'}' +
'.sR-logo a{' +
'    width: 104px;' +
'    height: 33px;' +
'    background: url(/Public/www/platform/style/images/topnav/sRicon.png) no-repeat 0 0;' +
'    display: block;' +
'    text-indent: -9999px;' +
'}' +
'.sR-banner{' +
'    margin-left: 27px;' +
'    width: 244px;' +
'    height: 41px;' +
'    position: relative;' +
'    z-index: 900;' +
'    cursor: pointer;' +
'}' +
'.sR-topimg{' +
'    width: 244px;' +
'    height: 41px;' +
'}' +
'.sR-downimg{' +
'    position: absolute;' +
'    top: 0;' +
'    left: -137px;' +
'    display: none;' +
'}' +
'.sR-downimg img{' +
'    width: 1000px;' +
'    height: 188px;' +
'}' +
'.sR-login{' +
'    width: 332px;' +
'    position: relative;' +
'    z-index: 901;' +
'}' +
'.sR-unlogin{' +
'    line-height: 41px;' +
'    color: #808080;' +
'}' +
'.sR-unlogin a{' +
'    color: #808080;' +
'    float: right;' +
'    text-decoration: none;' +
'}' +
'.sR-unlogin a:hover{' +
'    color:#e55f4f;' +
'    text-decoration:underline;' +
'}' +
'#sR-reg{' +
'    margin-right: 6px;' +
'}' +
'.sR-unlogin em{' +
'    margin: 0 6px;' +
'    float: right;' +
'}' +
'.sR-logined{' +
'    display: none;' +
'}' +
'.sR-logined{' +
'    line-height:41px;' +
'    color:#808080;' +
'    text-align:right;' +
'}' +
'.sR-loginout{' +
'    cursor:pointer;' +
'}' +
'.sR-shortcut {' +
'    line-height: 41px;' +
'    width: 228px;' +
'    margin-left: 38px;' +
'    position: relative;' +
'    z-index: 901;' +
'}' +
'.sR-shortcut em{' +
'    float: left;' +
'    background: url(/Public/www/platform/style/images/topnav/sRicon.png) no-repeat;' +
'    margin: 14px 2px 0 0;' +
'}' +
'.sR-shortcut span{' +
'    float: left;' +
'    cursor: pointer;' +
'}' +
'.sR-shortcut span em{' +
'    -webkit-transition: -webkit-transform .4s ease-out;' +
'    -moz-transition: -moz-transform .4s ease-out;' +
'    transition: transform .4s ease-out;' +
'}' +
'.sR-shortcut span:hover{' +
'    color:#4d4d4d;' +
'}' +
'.sR-shortcut span:hover em{' +
'    transform: rotate(360deg);' +
'    -ms-transform: rotate(360deg);' +
'    -moz-transform: rotate(360deg);' +
'    -webkit-transform: rotate(360deg);' +
'    -o-transform: rotate(360deg);' +
'    opacity: 1;' +
'}' +
'.sR-shortcut em.sR-payIcon{' +
'    width: 12px;' +
'    height: 12px;' +
'    background-position: 0 -40px;' +
'}' +
'.sR-shortcut em.sR-accountIcon{' +
'    width: 10px;' +
'    height: 12px;' +
'    background-position: -16px -40px;' +
'}' +
'.sR-shortcut em.sR-collectIcon{' +
'    width: 13px;' +
'    height: 12px;' +
'    background-position: -32px -40px;' +
'}' +
'.sR-shortcut em.sR-gameIcon{' +
'    width: 12px;' +
'    height: 12px;' +
'    background-position: -48px -40px;' +
'}' +
'.sR-shortcut a{' +
'    float: left;' +
'    text-decoration: none;' +
'    color: #6d6a8b;' +
'    margin-right: 17px;' +
'}' +
'.sR-shortcut a#sR-game{' +
'    margin-right: 0;' +
'}' +
'.sR-gamelist{' +
'    width: 415px;' +
'    height: 117px;' +
'    border: 1px solid #e9e9e9;' +
'    padding: 10px;' +
'    background: #fff;' +
'   position: absolute;' +
'    right: 0;' +
'    top: 41px;' +
'    display: none;' +
'}' +
'.sR-arrow{' +
'    position: absolute;' +
'    width: 10px;' +
'    height: 7px;' +
'    overflow: hidden;' +
'    background: url(/Public/www/platform/style/images/topnav/sRicon.png) no-repeat 0 -60px;' +
'    right: 8px;' +
'    top: -6px;' +
'}' +
'.sR-list{' +
'    width: 413px;' +
'    height: 115px;' +
'    border: 1px solid #eee;' +
'}' +
'.sR-pagegame{' +
'    float: left;' +
'    width: 276px;' +
'}' +
'.sR-mobilegame{' +
'    float: left;' +
'    width: 137px;' +
'}' +
'.sR-pagetit{' +
'    height: 27px;' +
'    border-bottom: 1px solid #eee;' +
'    border-right: 1px solid #eee;' +
'    line-height: 27px;' +
'    font-size: 12px;' +
'    color: #b2b2b2;' +
'    background-color: #f6f6f6;' +
'    width: 275px;' +
'    text-align: center;' +
'}' +
'.sR-mobiletit{' +
'    width: 137px;' +
'}' +
'.sR-pagegame ul a,.sR-mobilegame ul a{' +
'    margin-right: 0;' +
'    float: none;' +
'    color: #4d4d4d;' +
'}' +
'.sR-pagegame ul a:hover,.sR-mobilegame ul a:hover{' +
'    color: #e55f4f;' +
'    text-decoration: underline;' +
'}' +
'.sR-pagegame ul li,.sR-mobilegame ul li{' +
'    float: left;' +
'    width: 137px;' +
'    border-bottom: 1px solid #eee;' +
'    border-right: 1px solid #eee;' +
'    text-align: center;' +
'    line-height: 28px;' +
'}' +
'.sR-pagegame ul li.last,.sR-mobilegame ul li.last{ border-bottom: none;}' +
'.sR-gamelist li cite{' +
'    display: inline-block;' +
'    height: 9px;' +
'    width: 9px;' +
'    overflow: hidden;' +
'    background: url(/Public/www/platform/style/images/topnav/sRicon.png) no-repeat;' +
'}' +
'.sR-gamelist li cite.new{ background-position: -80px -40px;}' +
'.sR-gamelist li cite.hot{ background-position: -64px -40px;}' +
'.sR-mobilegame ul li{' +
'    border-right: none;' +
'}' +
'.sR-coll-down{' +
'    position: absolute;' +
'    top: 41px;' +
'    left: 108px;' +
'    border:1px solid #e9e9e9;' +
'    width:70px;' +
'    height:82px;' +
'    background: #fff;' +
'    display:none;' +
'}' +
'.sR-coll-down .sR-arrow{' +
'    right:46px;' +
'}' +
'.sR-coll-down a{' +
'    float:left;' +
'    width:70px;' +
'    margin-right:0;' +
'    line-height:26px;' +
'    height:26px;' +
'    text-align: center;' +
'    border-bottom:1px solid #e9e9e9;' +
'    color:#9d9d9d;' +
'}' +
'.sR-coll-down a:hover{ color:#717171;}' +
'.sR-coll-down a.last{ border-bottom:none;}' +
'</style>'+
'<div class="sevenRoad-top" id="sevenRoad-top">' +
'    <div class="sR-top">' +
'        <div class="sR-nav clearfix">' +
'            <div class="sR-logo">' +
'                <a href="http://www.wan.com/">第七大道</a>' +
'            </div>' +
'            <div class="sR-banner" id="sR-banner">' +
'                <img src="/Public/www/platform/style/images/topnav/sRbanner1.jpg" class="sR-topimg" id="sR-topimg"  />' +
'                <a href="#" target="_blank" class="sR-downimg" id="sR-downimg">' +
'                    <img src="" alt="" >' +
'                </a>' +
'            </div>' +
'            <div class="sR-login">' +
'                <div class="sR-unlogin clearfix" id="sR-unlogin">' +
'                    <a href="javascript:void(0)"  id="sR-reg">注册</a>' +
'                    <em>|</em>' +
'                    <a href="javascript:void(0)"   id="sR-login">登录</a>' +
'                </div>' +
'                <div class="sR-logined clearfix" id="sR-logined">' +
'                    <span id="sR-user">a624208572</span>，您好！' +
'                    <span class="sR-loginout" id="sR-loginout">[注销]</span>' +
'                </div>' +
'            </div>' +
'            <div class="sR-shortcut">' +
'                <span>' +
'                    <em class="sR-payIcon"></em>' +
'                    <a href="http://www.wan.com/pay" target="_blank">充值</a>' +
'                </span>' +
'                <span>' +
'                    <em class="sR-accountIcon"></em>' +
'                    <a href="http://www.wan.com/service" target="_blank">客服</a>' +
'                </span>' +
'                <span id="sR-collectdown">' +
'                    <em class="sR-collectIcon"></em>' +
'                    <a href="javascript:void(0)">收藏</a>' +
'                    <div class="sR-coll-down" id="sR-coll-down">' +
'                        <span class="sR-arrow"></span>' +
'                        <a href="javascript:void(0)" id="addfav">收藏本站</a>' +
'                        <a href="javascript:void(0)" id="sethome">设为首页</a>' +
'                        <a href="javascript:void(0)" target="_blank" class="last" id="save-desk">存至桌面</a>' +
'                    </div>' +
'                </span>' +
'                <span class="sR-gamedown" id="sR-gamedown">' +
'                    <em class="sR-gameIcon"></em>' +
'                    <a href="javascript:void(0)" id="sR-game">全部游戏</a>' +
'                    <div class="sR-gamelist" id="sR-gamelist">' +
'                        <span class="sR-arrow"></span>' +
'                        <div class="sR-list clearfix">' +
'                            <div class="sR-pagegame">' +
'                                <h2 class="sR-pagetit">网页游戏（<cite>6</cite>）</h2>' +
'                                <ul class="clearfix">' +
'                                    <li>' +
'                                        <a href="http://mhtl.wan.com/" target="_blank">梦幻天龙</a>' +
'                                        <cite class="hot"></cite>' +
'                                    </li>' +
'                                    <li>' +
'                                       <a href="http://sq.wan.com/" target="_blank">神曲II</a>' +
'                                        <cite class="hot"></cite>' +
'                                    </li>' +
'                                    <li>' +
'                                        <a href="http://ddt.wan.com/" target="_blank">弹弹堂3</a>' +
'                                        <cite class="hot"></cite>' +
'                                    </li>' +
'                                    <li>' +
'                                        <a href="http://xddt.wan.com/" target="_blank">新弹弹堂</a>' +
'                                    </li>' +
'                                    <li class="last">' +
'                                        <a href="http://jy.wan.com/" target="_blank">剑影</a>' +
'                                    </li>' +
'                                    <li class="last">' +
'                                        <a href="http://wzzh.wan.com/" target="_blank">王者召唤</a>' +
'                                        <cite class="new"></cite>' +
'                                    </li>' +
'                                </ul>' +
'                            </div>' +
'                            <div class="sR-mobilegame">' +
'                                <h2 class="sR-pagetit sR-mobiletit">手机游戏（<cite>2</cite>）</h2>' +
'                                <ul class="clearfix">' +
'                                    <li>' +
'                                        <a href="javascript:void(0)">王者召唤</a>' +
'                                        <cite class="new"></cite>' +
'                                    </li>' +
'                                    <li>' +
'                                        <a href="javascript:void(0)">弹弹堂</a>' +
'                                        <cite class="new"></cite>' +
'                                    </li>' +
									'<li>' +
'                                        <a href="http://sqm.wan.com/" target="_blank" >神曲世界</a>' +
'                                        <cite class="hot"></cite>' +
'                                    </li>' +
'                                </ul>' +
'                            </div>' +
'                        </div>' +
'                    </div>' +
'                </span>' +
'            </div>' +
'        </div>' +
'    </div>' +
'    <div class="sR-bottom"></div>' +
'</div>' +
'<script type="text\/javascript">' +
'var sevenRoadUtil = {' +
'    url : {' +
'        isLogin : "http://www.wan.com/index.php/accounts/isLogin.html",' +
'        loginOut : "http://www.wan.com/index.php/accounts/loginout",' +
'        getImg : "http://www.wan.com/index/get_ad"' +
'    },' +
'    $ : function(id){' +
'        return document.getElementById(id);' +
'    },' +
'    addHandler : function(element,type,handler){' +
'        if(element.addEventListener){' +
'            element.addEventListener(type,handler,false);' +
'        }else if(element.attachEvent){' +
'            element.attachEvent("on"+type,handler);' +
'        }else{' +
'            element["on"+type]=handler;' +
'        }' +
'    },' +
'    hide : function(objs){' +
'        if(objs){' +
'            for(var i=0;i<arguments.length;i++){' +
'                arguments[i].style.display = "none";' +
'            }' +
'        }' +
'    },' +
'    show : function(objs){' +
'        if(objs){' +
'            for(var i=0;i<arguments.length;i++){' +
'                arguments[i].style.display = "block";' +
'            }' +
'        }' +
'    },' +
'    getScriptStringSrc : function(str){' +
'        var str1 = str.split("</scr"+"ipt>");' +  
'        var srcArr = [];' +
'        for(var i= 0; i<str1.length; i++){' +
'            if(str1[i].length>1){' +
'                var str2 = str1[i].split("src=")[1].split("reload")[0].replace(/ /g,"");' +
'                str2 = str2.substr(1,str2.length-2);' +
'                srcArr.push(str2);' +
'            }' +
'        }' +
'        return srcArr;' +
'    },' +
'    getTopImg : function(){' +
'        var ajaxLogoutScript=document.createElement("script");' +
'        ajaxLogoutScript.src= this.url.getImg+"?callback=sevenRoadUtil.myGetimgBack";' +
'        document.body.appendChild(ajaxLogoutScript);' +
'    },' +
'    loginOut : function(){' +
'        var ajaxLogoutScript=document.createElement("script");' +
'        ajaxLogoutScript.src= sevenRoadUtil.url.loginOut+"?callback=sevenRoadUtil.myLogoutcallback";' +
'        document.body.appendChild(ajaxLogoutScript);' +
'    },' +
'    myGetimgBack : function(data){' +
'		if(data){' +
'        if(data.img){' +
'            this.$("sR-topimg").src = data.s_img;' +
'            this.$("sR-downimg").href = data.url;' +
'            this.$("sR-downimg").getElementsByTagName("img")[0].src = data.img;' +
'        }' +
'		}' +
'    },' +
'    checkLogin : function(){' +
'        var ajaxScript=document.createElement("script");' +
'        ajaxScript.src=this.url.isLogin+"?jsonpCallback=sevenRoadUtil.myCheckLoginBack";' +
'        document.body.appendChild(ajaxScript);' +
'    },' +
'    myCheckLoginBack : function(data){' +
'        if(data.state == 1){' +
'            this.hide(this.$("sR-unlogin"));' +
'            this.show(this.$("sR-logined"));' +
'            this.$("sR-user").innerHTML = data.datas.nickname;' +
'        }else{' +
'            this.show(this.$("sR-unlogin"));' +
'            this.hide(this.$("sR-logined"));' +
'        }' +
'    },' +
'    myLogoutcallback : function(data){' +
'        data = typeof data == "string" ? eval("("+data+")") : data;' +
'        if(data.state == "1"){' +
'            var scriptSrcArr = sevenRoadUtil.getScriptStringSrc(data.script);  /*取得data.script字符串中的各个script的url数组*/' +
'            for(var i = 0; i < scriptSrcArr.length; i++){' +
'                var scriptObj = document.createElement("script");' +
'                scriptObj.type = "text/javascript";' +
'                scriptObj.reload = "1";' +
'                scriptObj.src = scriptSrcArr[i];' +
'                document.body.appendChild(scriptObj);' +
'           }' +
'            setTimeout(function(){' +
'                window.location.reload();' +
'            },500);' +
'        }' +
'    },' +
'    addFavorite : function(){' +
'        if (document.all){' +
'            try{' +
'                window.external.addFavorite(window.location.href,document.title);' +
'            }catch(e){' +
'                alert( "加入收藏失败，请使用Ctrl+D进行添加" );' +
'            }' +
'        }else if (window.sidebar){' +
'            window.sidebar.addPanel(document.title, window.location.href, "");' +
'        }else{' +
'            alert( "加入收藏失败，请使用Ctrl+D进行添加" );' +
'        }' +
'    },' +
'    setHome : function(){' +
'        var obj = this.$("sethome"),vrl=document.location;' +
'        try{' +
'            obj.style.behavior="url(#default#homepage)";obj.setHomePage(vrl);' +
'        }' +
'        catch(e){' +
'            if(window.netscape) {' +
'                try {' +
'                    netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");' +
'                }' +
'                catch (e) {' +
'                    alert("此操作被浏览器拒绝！请在浏览器地址栏输入“about:config”并回车然后将 [signed.applets.codebase_principal_support]的值设置为true,双击即可。");' +
'                }' +
'                var prefs = Components.classes["@mozilla.org/preferences-service;1"].getService(Components.interfaces.nsIPrefBranch);' +
'               prefs.setCharPref("browser.startup.homepage",vrl);' +
'            }else{' +
'                alert("抱歉，您所使用的浏览器无法完成此操作。您需要手动将http:/"+"/"+document.location.hostname+"设置为首页。");' +
'            };' +
'        }' +
'    },' +
'	 saveDesk : function(){' +
'		 var hostName = document.location.hostname;' +
'		 sevenRoadUtil.$("save-desk").href = "http:/" + "/" + hostName + "/index/getShortcuts";' +
'	 },' +
'	 HideTop : function(){'+
'		var sevenRoadFooterPahtUrl = document.location.pathname.toLowerCase();' +
'		if(sevenRoadFooterPahtUrl.indexOf("/server") > -1 || sevenRoadFooterPahtUrl.indexOf("/article/server") > -1){' +
'			sevenRoadUtil.hide(sevenRoadUtil.$("sevenRoad-top"));' +
'		}' +					
'	 },'+
'    dispatch : function (el){' +
'        if(el.click) {' +
'            el.click();' +
'        }else{' +
'            try{' +
'                var evt = document.createEvent("Event");' +
'                evt.initEvent("click",true,true);' +
'                el.dispatchEvent(evt);' +
'            }catch(e){};' +
'        }' +
'    },' +
'    init : function(){' +
'        this.addHandler(this.$("sR-banner"),"mouseover",function(){' +
'            sevenRoadUtil.hide(sevenRoadUtil.$("sR-topimg"));' +
'            sevenRoadUtil.show(sevenRoadUtil.$("sR-downimg"));' +
'        });' +
'        this.addHandler(this.$("sR-banner"),"mouseout",function(){' +
'            sevenRoadUtil.show(sevenRoadUtil.$("sR-topimg"));' +
'            sevenRoadUtil.hide(sevenRoadUtil.$("sR-downimg"));' +
'        });' +
'        this.addHandler(this.$("sR-gamedown"),"mouseover",function(){' +
'            sevenRoadUtil.show(sevenRoadUtil.$("sR-gamelist"));' +
'        });' +
'        this.addHandler(this.$("sR-gamedown"),"mouseout",function(){' +
'            sevenRoadUtil.hide(sevenRoadUtil.$("sR-gamelist"));' +
'        });' +
'        this.addHandler(this.$("sR-collectdown"),"mouseover",function(){' +
'            sevenRoadUtil.show(sevenRoadUtil.$("sR-coll-down"));' +
'        });' +
'        this.addHandler(this.$("sR-collectdown"),"mouseout",function(){' +
'            sevenRoadUtil.hide(sevenRoadUtil.$("sR-coll-down"));' +
'        });' +
'        this.addHandler(this.$("addfav"),"click",function(){    /*绑定收藏*/' +
'            sevenRoadUtil.addFavorite();' +
'        });' +
'        this.addHandler(this.$("sethome"),"click",function(){   /*绑定设为首页*/' +
'            sevenRoadUtil.setHome();' +
'        });' +
'        this.addHandler(this.$("sR-login"),"click",function(){   /*绑定登录回调*/' +
'	         if( typeof selectLoginOrRegister == "function"){ ' +
'			    selectLoginOrRegister("login");	' +							
'			 }else{' +
'            	var _url = "http://www.wan.com/accounts/login" + "?url=" + document.location.href;' +
'            	sevenRoadUtil.$("sR-login").href = _url;' +
'				sevenRoadUtil.$("sR-login").target = "_blank";'+
'			 }' +
'        });' +
'        this.addHandler(this.$("sR-reg"),"click",function(){   ' +
'	         if( typeof selectLoginOrRegister == "function"){ ' +
'			    selectLoginOrRegister("register");	' +							
'			 }else{' +
'            	var _url = "http://www.wan.com/accounts/register/from_uid"; ' +
'            	sevenRoadUtil.$("sR-reg").href = _url;' +
'				sevenRoadUtil.$("sR-reg").target = "_blank";'+
'			 }' +
'        });' +
'		 this.addHandler(this.$("save-desk"),"click",this.saveDesk);' +
'        this.addHandler(this.$("sR-loginout"),"click",this.loginOut);   /*绑定注销*/' +
'        this.checkLogin();  /*载入检测登录*/' +
'        this.getTopImg();   /*载入获取图片*/' +
'		 this.HideTop();' +
'    }' +
'};' +
'sevenRoadUtil.init();' +
'<\/script>' +
'')
