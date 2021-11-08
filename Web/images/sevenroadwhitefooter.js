var beian = '34';
if(location.hostname == 'www.wan.cn'){
	beian = '33';
}
document.write('<style type="text/css">' +
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
    '.plat-footer{'+
    '	width:100%;'+
    '	height:118px;'+
    '	text-align:center;'+
    '	font-size:12px;'+
    '	font-family:SimSun;'+
    '}'+
    '.p-f-main{'+
    '	margin:0 auto;'+
    '	padding-top:20px;'+
    '   line-height: 20px;'+
    '}'+
    '.p-f-main a{ text-decoration: none;}'+
    '.plat-footer p{'+
    '	margin:0;'+
    '	padding:0;'+
    '}'+
    '.p-f-txt span{'+
    '	color:#787878;'+
    '	margin:0 6px;'+
    '}'+
    '.p-f-link em{ '+
    '	color:#555;'+
    '	margin:0 6px;'+
    '}'+
    '.p-f-link a{'+
    '	color:#555;'+
    '}'+
    '.p-f-game{'+
    '	color:#e55f4f;'+
    '	font-size:14px;'+
    '	line-height:54px;'+
    '}'+
    '.plat-footer p.p-f-game{' +
    '   padding-top:7px;' +
    '}' +
    '.p-f-game a{ '+
    '	color:#e55f4f;'+
    '	margin-right:6px;'+
    '}'+
    '.p-f-company,.p-f-company a{'+
    '	color:#787878'+
    '}'+
    '.p-f-record{'+
    '	color:#555;'+
    '}'+
    '.plat-black{'+
    '	background:#171717;'+
    '}'+
    '.plat-black .p-f-txt span{ '+
    '	color:#3c3c3c'+
    '}'+
    '.plat-black .p-f-link em,.plat-black .p-f-link a{'+
    '	color:#4f4e4e;'+
    '}'+
    '.plat-black .p-f-game,.plat-black .p-f-game a{'+
    '	color:#e55f4f;'+
    '}'+
    '.plat-black .p-f-company,.plat-black .p-f-company a{'+
    '	color:#3c3c3c'+
    '}'+
    '.plat-black .p-f-record{'+
    '	color:#4f4f4f;'+
    '}'+
    '</style>'+
    '<div id="plat-footer" class="plat-footer">'+
    '	<div class="p-f-main">'+
    '		<p class="p-f-txt">'+
    '			<span>抵制不良游戏</span>'+
    '			<span>拒绝盗版游戏</span>'+
    '			<span>注意自我保护</span>'+
    '			<span>谨防受骗上当</span>'+
    '			<span>适度游戏益脑</span>'+
    '			<span>沉迷游戏伤身</span>'+
    '			<span>合理安排时间</span>'+
    '			<span>享受健康生活</span>'+
    '		</p>'+
    '		<p class="p-f-link">'+
    '			<a href="http://www.7road.com/" target="_blank">关于七道</a>'+
    '			<em>-</em>'+
    '			<a href="http://www.7road.com/about?contactUs" target="_blank">联系我们</a>'+
    '			<em>-</em>'+
    '			<a href="http://www.7road.com/hrs/" target="_blank">公司招聘</a>'+
    '			<em>-</em>'+
    '			<a href="http://www.wan.com/service" target="_blank">客户服务</a>'+
    '			<em>-</em>'+
    '			<a href="http://www.7road.com/communication?business" target="_blank">商务合作</a>'+
    '		</p>'+
    '		<p class="p-f-company">深圳第七大道科技有限公司（ <a href="http://www.7road.com">www.7road.com</a> ）</p>'+
    '		<p class="p-f-record"><a target="_blank" href="http://www.miitbeian.gov.cn">粤ICP备08006865号-'+beian+'</a>　《网络文化经营许可证》粤网文〔2013〕0214-064号'+
	'		<script>(function(){'+
	'			var _host=window.location.hostname;'+
	'			var _html="";'+
	'			if(_host=="sq.7road.com"||_host=="sq.wan.com"){'+
	'				_html="批准文号 新广出审[2014]434号 出版物号 ISBN978-7-89989-952-6 <a target=_blank href=\\\"http://sq.ccm.gov.cn:80/ccnt/sczr/service/business/emark/gameNetTag/CA0277E57C8D4EEE80E31C41A76D8C8C\\\"><img src=\\\"/Public/img/icp.png\\\" style=\\\"width:24px;height:24px\\\"></a>文网游备字〔2015〕Ｗ-RPG 0039 号"'+
	'			}else if(_host=="ddt.7road.com"||_host=="ddt.wan.com"){'+
	'				_html="批准文号 科技与数字[2010]005号 出版物号 ISBN978-7-89471-636-1 <a target=_blank href=\\\"http://sq.ccm.gov.cn:80/ccnt/sczr/service/business/emark/gameNetTag/F35FC9BEF91252AEE040007F01003007\\\"><img src=\\\"/Public/img/icp.png\\\" style=\\\"width:24px;height:24px\\\"></a>文网游备字〔2010〕W-CSG002号"'+
	'			}else if(_host=="xddt.7road.com"||_host=="xddt.wan.com"){'+
	'				_html="批准文号 科技与数字[2010]005号 出版物号 ISBN978-7-89989-543-6 文网游备字（2013）W-CSG010号"'+
	'			}else if(_host=="mhtl.7road.com"||_host=="mhtl.wan.com"){'+
	'				_html="批准文号 新广出审[2015]830号 出版物号 ISBN978-789988-388-4 <a target=_blank href=\\\"http://sq.ccm.gov.cn:80/ccnt/sczr/service/business/emark/gameNetTag/4028c08b4f268bca014f26964b56000e\\\"><img src=\\\"/Public/img/icp.png\\\" style=\\\"width:24px;height:24px\\\"></a>文网游备字〔2015〕Ｗ-RPG 0614 号"'+
	'			}else if(_host=="sq4.7road.com"||_host=="sq4.wan.com"){'+
	'				_html="批准文号 新广出审[2015]973号 出版物号 ISBN 978-7-89988-405-8 <a target=_blank href=\\\"http://sq.ccm.gov.cn:80/ccnt/sczr/service/business/emark/gameNetTag/4028c08b51af781b01520b7bf8e92d6a\\\"><img src=\\\"/Public/img/icp.png\\\" style=\\\"width:24px;height:24px\\\"></a>文网游备字〔2016〕Ｗ-RPG 0023 号"'+
	'			}'+
	'			document.write(_html)'+
	'		})()'+
	'		<\/script></p>'+
    '	</div>'+
    '</div>'+
    '<script type="text\/javascript">' +
    '	var sevenRoadFooterPahtUrl = document.location.pathname.toLowerCase();' +
    '	if(sevenRoadFooterPahtUrl.indexOf("/server") > -1 || sevenRoadFooterPahtUrl.indexOf("/article/server") > -1){' +
    '		document.getElementById("plat-footer").style.display = "none";' +
    '	}' +
    '<\/script>' +

    '<!--百度统计-->'+
    '<script type="text\/javascript">' +
    '   var _hmt = _hmt || [];'+
    '   (function() {'+
    '       var hm = document.createElement("script");'+
    '       hm.src = "//hm.baidu.com/hm.js?d7eadad6fe31f3a2dfdf8049152793e4";'+
    '       var s = document.getElementsByTagName("script")[0];'+
    '       s.parentNode.insertBefore(hm, s);'+
    '   })();'+
    '<\/script>' +
    '');
	
    var bp = document.createElement('script');
    bp.src = 'http://push.zhanzhang.baidu.com/push.js';
    var s = document.getElementsByTagName("script")[0];
    s.parentNode.insertBefore(bp, s);
