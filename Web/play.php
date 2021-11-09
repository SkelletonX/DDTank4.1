<?php


#----------------------------------------#
#------------Admin Painel v1.0-----------#
#--------Create by bachugacon122----=----#
#----------------------------------------#
include ('global.php');
$rd = rand(0,9999999).rand(0,9999999).uniqid();


if(isset($_GET['key'])) {
    $k = $_GET['key'];
	$custom = unserialize($_SESSION['UserData']);
	echo '
	<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" class= id="7road-ddt-game"
        codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0"
        name="Main" id="Main">
        <param name="allowScriptAccess" value="always" />
        <param name="movie" value="'.$LinkFlash.'Loading.swf?user='.$custom->LowerName.'&key='.$k.'&config='.$LinkLogin.'config.xml" />
        <param name="quality" value="high" />
        <param name="menu" value="false">
        <param name="bgcolor" value="#000000" />
        <param name="FlashVars" value="editby=" />
        <param name="allowScriptAccess" value="always" />
        <embed flashvars="editby=" src="'.$LinkFlash.'Loading.swf?user='.$custom->LowerName.'&key='.$k.'&config='.$LinkLogin.'config.xml"
            width="1000" height="600" align="middle" quality="high" name="Main" allowscriptaccess="always"
            type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" wmode="direct"/>
    </object>';
	exit();
}

if(!isset($_SESSION['UserData'])) exit('<script type="text/javascript">window.location="index.php";</script>');
?>
<head>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
		<title><?php echo $jogando ?></title>
		<?php echo $icon ?>
		<script src="./Assets/js/jquery-1.11.1.min.js"></script>
<style>
      html, body	{ height:100%; }
      body
        {
        margin: 0px auto;
        padding: 0px;
        background-image: url('./Images/bg_all.jpg');
	    background-repeat: no-repeat;
        background-position: center center;
        overflow:auto; text-align:center;
        }
        *,html,body,embed{cursor:url('images/cursors/default.cur'), default;}
	    a:hover{cursor:url('images/cursors/link.cur'), pointer;}
	    input{cursor:url('images/cursors/input.cur'), text;}
		#playgane {position: relative;};
    </style>	
</head>

<body scroll="no" >
	<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td valign="middle">
                <table border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center">
						<div id="gameOuterContainer"  style="cursor:pointer;width:1000px;height:600px;overflow:hidden;background-image:url('images/gameBackGround.jpg');" onclick="showGame();">
                            <div id="gameContainer"  style="width:1000px;height:600px;overflow:hidden;" >							
                            <div id="playgame" ></div>
							 </div>							 
                        </div>							
                        </td>
						
                    </tr>					
                </table>
            </td>
        </tr>
<div id="loading"><center><img src='./images/gif-load.gif'/></center></div>
<script type="text/javascript">
$.ajax({
    <?php $custom = unserialize($_SESSION['UserData']); ?>
    type: 'GET',
    url: "./checkuser.ashx",
    data: "username=<?php echo $custom->LowerName;?>&password=<?php echo $custom->PassEncrypted; ?>",
    success: function (data_revert) {
        if (data_revert == "ok") {
			$.ajax({
                type: 'GET',
                url: './logingame.aspx',
                success: function (data_revert) {
                    if (data_revert != "0") {
						$.ajax({
							type: 'GET',
							url: 'play.php',
							data: 'key='+data_revert,
							success: function (data) {
								$('#loading').slideUp(function() {
									$('#playgame').html(data).slideDown();
								});
							}
						});
                    }
                     else window.location="index.php?logout=true";
                }
            });
        }
         else window.location="index.php?logout=true";
    }
});
</script>

</body>


