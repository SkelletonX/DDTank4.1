function DDTstartgame(){
	window.open("http://ddt.wan.com/server/index.html");
};
DDT_ACTION.getNewServer = "/index/get_new_server";
DDT_ACTION.rank = "/index/get_rank";

	D2_Util = {
		init : function(){

			/* 左边微信start */
			$(".weixin-b span").on("click",function(){
			  $(".weixin-s").show();
			  $(".weixin-b").hide();
			});
			$(".weixin-s").on("click",function(){
			  $(this).hide();
			  $(".weixin-b").show();
			});
			/* 左边微信end */

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
			$("#userPass").on("keyup", function(e){
                var code = e.keyCode;
                if( code == 13 ){
                    $(".loginbtn").click();
                }
            });
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
                $(".server-plat-menu").find("span").live("click",function(){
                    var index = $(".server-plat-menu").find("span").index(this);
                    $(this).addClass("select").siblings().removeClass("select");
                    $(".server-tab-box").eq(index).show().siblings().hide();
                });
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

			// 载入检测登录
			//DDT_COM.userLoginOperate.checkLogin(servicerUtil.loginEnd,servicerUtil.unLogin);
			// 登录按钮
			

			// 左侧快速选服
			$(".fast-enter").on("click",function(){
				DDT_COM.quickSelectServer($(".fast-input"),"7road");
			});

			$(".die a").on("click",function(event){
		        event.preventDefault();
		        alert("服务器正在维护");
		    });
			// 品专统计代码
			if(D2_Util.getParamValue("pzid")){ 
				$(".forget a:last,.ser-nologin a:last").on("click",function(){ 
					$(this).attr("href",$(this).attr("href")+"?pzid="+D2_Util.getParamValue("pzid"));
					$.getJSON("http://ados.wan.com/stat/pz/?pzid="+D2_Util.getParamValue("pzid")+"&tag=click&callback=?",function(data){ 
						// nothing
					});
				});
				$.getJSON("http://ados.wan.com/stat/pz/?pzid="+D2_Util.getParamValue("pzid")+"&tag=visit&callback=?",function(data){ 
					// nothing
				});
			}

			//引导页点击“弹弹堂3”文字到首页显示公告
			var searchVal = Number(location.search.split("=")[1]);
			if(typeof searchVal !="undefined"){
			    $(".news-tab span").eq(searchVal).addClass("select").siblings().removeClass("select");
			    $(".news-content").eq(searchVal).show().siblings().hide();
			}

		},
		getParamValue : function (name) { 
			var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i"); 
			var r = window.location.search.substr(1).match(reg); 
			if (r != null) return unescape(r[2]); return null; 
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
					$(".rank-menu .c").text("等级");
				}else{
					$(".rank-menu .c").text("战力");
				}
				if(typeof callBack == "function"){
					var sNum = $(".first-val").attr("servernum"),typdId = $(".second-val").text();
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
			/*var typeIdVal = Url.param("typeid");
			if(typeIdVal){
				$(".news-menu span").each(function(){
					if($(this).attr("typeid") == typeIdVal){
						$(this).addClass("select").siblings("span").removeClass("select");
						$(".news-box ").load($(this).attr("data-url"));
					}
				});
			};*/
		},
		loadRankHtml : function(sNum,typeId){
            $.post(DDT_ACTION.rank,{sid:sNum,type:typeId,c:10},function(data){
                var hotName = '',spanName = '';
                var _li = '';
                if(data.data && data.data.length > 1){
                    $(".rank-list").empty();
                    $.each(data.data,function(i,n){
                        if( i==0 || i==1 || i==2){
                            hotName = "hot";
                        }else{
                            hotName = "";
                        }
                        if(n.fightpower == 0 || n.fightpower==0 || n.levels==0){ 
                        	 $(".rank-list").empty().append("暂无数据！");
                        	 return false;
                        }
                        if(typeId == "公会战力排行"){
                            $(".rank-list").append( '<li class="'+hotName+'"><span class="a">'+(i+1)+'</span><span class="b">'+n.consortianame+'</span><span class="c">'+n.fightpower+'</span></li>');
                          }else if(typeId == "公会等级排行"){
                            $(".rank-list").append( '<li class="'+hotName+'"><span class="a">'+(i+1)+'</span><span class="b">'+n.consortianame+'</span><span class="c">'+n.levels+'</span></li>');
                        }else if(typeId == "个人战力排行"){
                            $(".rank-list").append( '<li class="'+hotName+'"><span class="a">'+(i+1)+'</span><span class="b">'+n.nickname+'</span><span class="c">'+n.fightpower +'</span></li>');
                        }
                    });
                }else{ 
                	$(".rank-list").empty().append("暂无数据！");
                }
            });

			//$(".rank-list").load("http://ddt.wan.com/player/"+sNum+"_"+typdId+".html");
		}
		
	};
	var servicerUtil = {
		loginEnd : function(data){
			$(".login-box").show();
			$(".unlogin-box").hide();
			$(".userVal").text(data.nickname);
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
	};
	D2_Util.init();






