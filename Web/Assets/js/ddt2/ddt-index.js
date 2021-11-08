function DDTstartgame(){
	window.open("");
};
$(function(){
	var D2_Util = {
		init : function(){
			$(".user-lab,.pass-lab").on("click",function(){
				$(this).hide()
			});
			$("#userPass").on("focus",function(){
				$(".pass-lab").hide();
			});
			$("#user").on("focus",function(){
				$(".user-lab").hide();
			});
			$("#user").on("blur",function(){
				if(!$(this).val().length){
					$(".user-lab").show();
				}
			});
			$("#userPass").on("blur",function(){
				if(!$(this).val().length){
					$(".pass-lab").show();
				}
			});
			$(".fast-input").on("focus",function(){
				$(this).addClass("fast-input-focus");
				if($(this).val() == "快速选服"){
					$(this).val("");
				}
			});
			$(".fast-input").on("blur",function(){
				$(this).removeClass("fast-input-focus");
				if(!$(this).val()){
					$(this).val("快速选服");
				}
			});
			$(".link-down").hover(function(){
				$(this).children("ul").show();
			},function(){
				$(this).children("ul").hide();
			});
			$(".link-down li").hover(function(){
				$(this).addClass("select").siblings().removeClass("select");
			});
			$(".fast-enter").hover(function(){
				$(this).addClass("fast-enter-select");
			},function(){
				$(this).removeClass("fast-enter-select");
			})
			$(".nav").hover(function(){
				$(".submenu").slideDown(300);
			},function(){
				$(".submenu").hide();
			});
			$(".loginbtn").hover(function(){
				$(this).addClass("loginbtn-on");
			},function(){
				$(this).removeClass("loginbtn-on");
			});
			/*$("#userPass").on("keyup", function(e){
                var code = e.keyCode;
                if( code == 13 ){
                    $(".loginbtn").click();
                }
            });*/
			// 下拉框
			D2_Util.imitationSelect($(".default-val"),D2_Util.loadRankHtml);
			// 焦点图&选项卡
			if($(".focus-img").length>0){
				D2_Util.focus($(".focus-menu span"),$(".focus-img  a"));
				D2_Util.tabContent($(".news-tab span"),$(".news-content"));
				D2_Util.dataTab($(".data-menu a"),$(".data-ul"),$(".data-left img"));
				D2_Util.tabContent($(".weapon-menu span"),$(".weapon-ul"),$(".tab-underline"));
				D2_Util.tabContent($(".recreation-menu span"),$(".recreation-ul"),$(".tab-underline2"));
			};
			//资料内页选项卡
			if($(".tab-items").length>0){
				$(".tab-items li").on("mouseover",function(){
					var index = $(".tab-items li").index(this);
					$(this).addClass("tab-item-current").siblings("li").removeClass("tab-item-current");
					$(".tab-cnt-item").eq(index).show().siblings().hide();
				});
				var dataH1Tit = D2_Util.delStrBlank($(".dataTit").text());
				$.each($(".gameData-p a"),function(i,n){
					if(D2_Util.delStrBlank($(".gameData-p a").eq(i).text()) == dataH1Tit){
						$(".tab-cnt-item").hide();
						$(".gameData-p a").eq(i).parent().parent().parent().parent().show();
					}
				});
				$.each($(".tab-cnt-item"),function(i,n){
					if($(".tab-cnt-item").eq(i).is(":visible")){
						$(".tab-item").eq(i).addClass("tab-item-current").siblings().removeClass("tab-item-current");
					}
				});
			};
			// 新闻内页
			if($('.news-list').length>0){		
				D2_Util.clickLoadHtml($(".news-menu span"),$(".news-box"));	
				// 根据url typeid 选中tab 
				D2_Util.autoSelectTabMenu();
			};
			// 玩家风采
			if($('.recreation-list').length>0){		
				D2_Util.clickLoadHtml($(".news-menu span"),$(".recreation-box"),"recreation");	
			};
			// 选服页
			if($('.serverList').length>0){		
				D2_Util.tabContent($(".server-menu span"),$(".plat-tab"));	
				//D2_Util.tabContent($(".server-plat-menu span"),$(".server-tab-box"));	
			};
			// 引导页
			if($('.guide-h2').length>0){
				$(".gameData-list a").on("click",function(event){
					event.preventDefault();
					var url = $(this).attr("href");
					$(this).addClass("current").siblings("a").removeClass("current");
					$.get(url,function(data){
						data = $.trim(data);
						var begin = data.indexOf('<!--guideLoadBegin-->');
						var end = data.indexOf('<!--guideLoadEnd-->');
						$(".guideContent").html(data.substring(begin,end));
					});
				});
				$(".gameData-list a:first").click();
			};
			
			// 字体设置
			$(".data-time span").live("click",function(){
				var _index = $(".data-time span").index(this);
				if(_index == 0){
					$(".news-content,.data-textContent").css("font-size","16px");
				}else if(_index == 1){
					$(".news-content,.data-textContent").css("font-size","14px");
				}else{
					$(".news-content,.data-textContent").css("font-size","12px");
				}
			});
			

			// 注销按钮
			/*$(".loginOut").on("click",function(){
				DDT_COM.userLoginOperate.loginOut(servicerUtil.loginOut);
			});
			// 载入检测登录
			DDT_COM.userLoginOperate.checkLogin(servicerUtil.loginEnd,servicerUtil.unLogin);
			// 登录按钮
			$(".loginbtn").on("click",function(){
				var userval = D2_Util.delStrBlank($("#user").val()),
					userpas = D2_Util.delStrBlank($("#userPass").val());
				if(userval.length<5 || userval.length>50 || userpas.length<4 || userpas.length>20){
					alert("请正确填写账号密码！");
					return false;
				}
				DDT_COM.userLoginOperate.login(userval,userpas);
			});

			// 左侧快速选服
			$(".fast-enter").on("click",function(){
				DDT_COM.quickSelectServer($(".fast-input"),"7road");
			});
			*/
			// 排行榜 ссс менял
			$(".rank-list").load("http://");

			// 填充排行榜区服列表 сьтатус
			$.getJSON("http://",function(data){
				data = data.serverList;
				$.each(data,function(i,n){
					$(".first-val").text(data[0].name).attr("servernum",data[0].index);
					$(".f-first-ul").append("<li servernum="+data[i].index+">"+data[i].name+"</li>");
				});
			});
		},

		focus : function($menuObj,$imgObj){
			var _index = 0,timer;
			$menuObj.on("mouseover",function(){
				_index = $menuObj.index(this);
				$(this).addClass("select").siblings().removeClass("select");
				$imgObj.eq(_index).fadeIn(1200).siblings().hide();
			});
			$imgObj.parent().hover(function(){
				clearInterval(timer);
			},function(){
				timer = setInterval(function(){
					$imgObj.eq(_index).fadeIn(1200).siblings().hide();
					$menuObj.eq(_index).addClass("select").siblings().removeClass("select");
					_index++;
					if(_index == $menuObj.length){
						_index = 0;
					}
				},4000);
			}).trigger("mouseleave");
		},
		imitationSelect : function($menu,callBack){
			$menu.on("click",function(){
				$menu.siblings("ul").hide();
				$(this).siblings("ul").show();
			});
			
			$menu.siblings("ul").children("li").hover(function(){
				$(this).addClass("on").siblings().removeClass("on");
			});
			$(".filtrate-ul").delegate("li","click",function(event){
				event.stopPropagation();
				$(this).parent().siblings(".default-val").children().text($(this).text());
				$(this).parent().siblings(".default-val").children().attr({"typeid":$(this).attr("typeid"),"servernum":$(this).attr("servernum")});
				$(this).parent().hide();
				if($(this).attr("typeid") == "2" || $(".second-val").attr("typeid") == "2"){
					$(".rank-menu .c").text("Força2");
				}else{
					$(".rank-menu .c").text("Força1");
				}
				if(typeof callBack == "function"){
					var sNum = $(".first-val").attr("servernum"),typdId = $(".second-val").attr("typeid");
					callBack(sNum,typdId);
				}
			});
			$("body").on("click",function(event){
				if(event.target.className == "first-val" || event.target.className == "second-val"){

				}else{
					$menu.siblings("ul").hide();
				}
			});
		},
		tabContent : function($tabMenu,$contentObj,$slideObj){
			
			$tabMenu.live("mouseover",function(){
				var index = $tabMenu.index(this);
				$(this).addClass("select").siblings("span").removeClass("select");
				$contentObj.eq(index).show().siblings().hide();
				if($slideObj){
					var slideObjWidth = $slideObj.width();
					var tabMenuMarginVal = Number($tabMenu.css("margin-right").replace("px",""));
					$slideObj.stop(true,true).animate({"left":index*(slideObjWidth+tabMenuMarginVal)+5+"px"},400,"swing",function(){
						$slideObj.stop(true,true).animate({"left":Number($slideObj.css("left").replace('px',''))-5+"px"},200,"swing",function(){
							$slideObj.stop(true,true).animate({"left":Number($slideObj.css("left").replace('px',''))+5+"px"},100,"swing",function(){
								$slideObj.stop(true,true).animate({"left":Number($slideObj.css("left").replace('px',''))-5+"px"},100,"swing")
							});
						});
					});
				}
			});
		},
		dataTab : function($tabMenu,$contentObj,$imgObj){
			$tabMenu.on("mouseover",function(){
				var index = $tabMenu.index(this);
				$(this).css("background-position",-index*100+"px -110px").siblings().removeAttr("style");
				$contentObj.eq(index).show().siblings("ul").hide();
				$imgObj.eq(index).fadeIn().siblings().hide();
			});
		},
		delStrBlank : function(str){
			return str.replace(/\s/gi,'');
		},
		clickLoadHtml : function($btn,$loadObj,type){
			$btn.on("click",function(){
				var _index = $btn.index(this);
				var htmlUrl = $(this).attr("data-url");
				$btn.eq(_index).addClass("select").siblings().removeClass("select");		
				if(type == "recreation"){
					
					if(_index == 0){
						$loadObj.hide();
						$loadObj.eq(_index).show();	
					}else if(_index == 4){
						$loadObj.hide();
						$.get("http://ddt.wan.com/picture/player/1.html",function(data){
							/*data = $.trim(data);
							var begin = data.indexOf('<!--playerLoadBegin-->');
							var end = data.indexOf('<!--playerLoadEnd-->');
							$loadObj.eq(_index).html(data.substring(begin,end)).show();*/
							$loadObj.eq(_index).html(data).show();
						});
					}else{
						$loadObj.hide();
						$loadObj.eq(_index).empty().load(htmlUrl).show();	
					}
					
					
				}else{
					$loadObj.hide();
					$loadObj.show().empty().load(htmlUrl);	
				}
				
			});
		},
		autoSelectTabMenu : function(){
			var typeIdVal = Url.param("typeid");
			if(typeIdVal){
				$(".news-menu span").each(function(){
					if($(this).attr("typeid") == typeIdVal){
						$(this).addClass("select").siblings("span").removeClass("select");
						$(".news-box ").load($(this).attr("data-url"));
					}
				});
			};
		},
		loadRankHtml : function(sNum,typdId){
			$(".rank-list").load("http://ddt.wan.com/player/"+sNum+"_"+typdId+".html");
		}
		
	};
	/*var servicerUtil = {
		loginEnd : function(data){
			$(".login-box").show();
			$(".unlogin-box").hide();
			$(".userVal").text(data.username);
			$(".currencyNum").text(data.currency);
			if(data.recentList.hasOwnProperty(DDT_COM.gameName)){	// 最近登录服务器有弹弹堂
				$(".lastLogin").text(data.recentList['ddt'].name.split("|")[1])
				.attr({"href":DDT_COM.getGameUrl(data.recentList['ddt'].name.split("|")[0]),"target":"_blank"});
			}else{
				$(".lastLogin").text("暂无记录");
			}
		},
		unLogin : function(data){
			$(".unlogin-box").show();
			$(".login-box").hide();
			if($(".my-box").length>0){
				$(".my-box").empty().append("请先登录！");
			};
		},
		loginOut : function(data){
			$(".unlogin-box").show();
			$(".login-box").hide();
		}
	};*/
	D2_Util.init();
});




