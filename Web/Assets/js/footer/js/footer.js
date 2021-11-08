var topbar = '<div id="footer"></div>'
document.write(topbar);
(function () {
    var pageUrl = document.location.hostname,
        footerWww = "", footerContent, rfidHref = "" ,rfidShow="none";
    /*switch(pageUrl) {
        case "q.wan.com":
            footerWww = "文网游备字[2014]W-RPG165号 | 新出审字[2013]1370号 | ISBN 978-7-89989-806-2";
            rfidHref = "http://182.131.21.137/ccnt-apply/publicservice/applyonline/product/gameBase!gameNetTag.action?oprBean.id=6AF63AB1342C4B1B929E86A87121E652";
            rfidShow="block";
            break;
        case "sq.wan.com":
            footerWww = '文网文[2013]0214-064号 | 增值电信业务经营许可证：粤B2-20100091 | 粤ICP备08006865号-1<br/>文网游备字【2012】W-RPG029号 ISBN号：978-89471-642-2';
            break;
        case "ddt.wan.com":
            footerWww = "文网文[2013]0214-064号 | 文网游备字【2012】W-CSG022号 ISBN号：978-7-89471-636-1";
            break;
        case "xddt.wan.com":
            footerWww = "文网文[2013]0214-064号 | 文网游备字【2013】W-CSG010号 | ISBN号：978-7-89989-543-6";
            break;
        case "jy.wan.com":
            footerWww = "文网文[2013]0214-064号 | 增值电信业务经营许可证：粤B2-20100091 | 粤ICP备08006865号-1";
            break;
        case "nslm.wan.com":
            footerWww = "文网游备字[2013]W-RPG155号 | 新出审字[2013]893号 | ISBN 978-7-89989-728-7";
            break;
        case "lhzs.wan.com":
            footerWww = "文网游备字[2013]W-RPG032号 | 科技与数字[2012]599号 | ISBN 978-7-900795-56-4";
            break;
        case "lwzy.wan.com":
            footerWww = "文网游备字[2013]W-RPG086号 | 新出审字[2013]1216号 | ISBN 978-7-89989-772-0";
            break;
        case "qjll.wan.com":
            footerWww = "文网游备字[2013]W-RPG188号 | 新出审字[2013]1369号";
            break;
        case "jyqz.wan.com":
            footerWww = "文网游备字[2011]W-RPG032号 | 科技与数字[2012]559号 | ISBN号 978-7-900768-91-9";
            break;
        case "qh.wan.com":
            footerWww = "文网游备字[2011]W-RPG032号 | 科技与数字[2012]559号";
            break;
        case "gcld.wan.com":
            footerWww = "文网游备字[2011]W-RPG032号 | 科技与数字[2012]559号";
            break;
        case "gcld.wan.com":
            footerWww = "文网游备字[2011]W-RPG032号 | 科技与数字[2012]559号";
            break;
        case "ah.wan.com":
            footerWww = "文网游备字[2013]W-RPG223号 | 科技与数字0212695号 | ISBN 978-7-900721-79-2";
            break;
        case "xhs.wan.com":
            footerWww = "新出审字 [2013]1560号 | ISBN 978-7-89989-844-4";
            break;
        case "wz.wan.com":
            footerWww = "文网游备字[2013]W-RPG062号 | 科技与数字[2012]528号";
            break;
        case "sctx.wan.com":
            footerWww = "新出审字[2013]1365号 | ISBN号 978-7-89989-803-1";
            break;
        case "sxd.wan.com":
            footerWww = "文网游备字[2012]W-RPG089号 | 科技与数字【2011】208号 | 沪B2-20120024 | 2011SR092748";
            break;
        case "js.wan.com":
            footerWww = "沪B2-20120024 | 软著登字第0356422号 ";
            break;
        case "jlc.wan.com":
            footerWww = "京ICP证100827号 | [2013]W-RPG045号 | 科技与数字【2012】481号 | 软著变补字第201202675号";
            break;
        case "wssg.wan.com":
            footerWww = "文网游备字【2013】W-RPG142号 | 粤新出数字【2012】131号 | 2012SR074104";
            break;
        case "jjsg.wan.com":
            footerWww = "2012SR086511";
            break;
        case "by.wan.com":
            footerWww = "粤ICP备09097877号 | 科技与数字【2012】721号 | 2012SR064709";
            break;
        case "lhsg.wan.com":
            footerWww = "[2012]W-RPG238号 | 科技与数字【2012】703号 | 软著登字第0410683号";
            break;
        case "tj.wan.com":
            footerWww = "京ICP证100827号 | [2013]W-RPG053号 | 科学与数字【2012】455号 | 软著登字第0386591号";
            break;
        case "kdxy.wan.com":
            footerWww = "京ICP证110761号 | 京网文[2012]0243-075号";
            break;
        case "fyws.wan.com":
            footerWww = "沪ICP备14000728号-2 ";
            break;
        case "qs.wan.com":
            footerWww = "文网文[2009]245号 | ICP证：闽B2-20040099 | 软著登字第0469119号 | 文网游备字[2013]W-RPG014号 | 科技与数字[2013]51号";
            break;
        case "lzwz.wan.com":
            footerWww = "沪网文[2014]0024-024号 | 沪ICP备14000728号-2 | 增值电信业务经营许可证沪B2-20140017";
            break;
        case "xxd.wan.com":
            footerWww = "沪网文[2011]0705-082号 | 新出审字[2013]1371号 | 文网游备字[2013]W-RPG194号";
            break;
        case "hg.wan.com":
            footerWww = "京ICP证110756号 | 京网文[2012]0014-013号";
            break;
        case "yxzg.wan.com":
            footerWww = "粤ICP备05102490号-3 | 科技与数字[2012]229号 | 粤网文[2010]0473-027号";
            break;
        case "yxzg.wan.com":
            footerWww = "新出审字[2013] 1370号 | ISBN 978-7-89989-806-2";
            break;
    }*/ 
   footerContent ='<div class="footerWrap"><div class="footer screenCenter"><a class="footerLogo"href="http://#/"target="_blank"></a><div class="footerNav"><p class="wenww">'+footerWww+'</p><p>True Tank <em>Versão 4.1</em></p><div class="footerTips">Servidor Privado<br>Copyright © 2020 True Tank. Todos os direitos reservados.</div></div><a href="'+rfidHref+'" class="game_rfid" style="display:'+rfidShow+'" target="_blank">&#160;</a></div></div>'
    document.getElementById("footer").innerHTML = footerContent;

})();
