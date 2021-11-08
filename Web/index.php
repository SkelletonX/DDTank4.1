<?php
include('global.php');

if(isset($_POST['login']) && !isset($_SESSION['UserData']))
{
	if(!isset($_POST['UserName']))
	{
		echo Alert("è Nescessario Precher o Usuario");
	}
	else if(!isset($_POST['Password']))
	{
		echo Alert("è Nescessario Precher o Password");
	}
	else
	{
		$user = addslashes($_POST['UserName']);
		$pass = $_POST['Password'];
		$UserInfo = new MemberData($user,$pass);
		if($UserInfo->isAuthenticated())
		{
		getUserData($UserInfo);
		$_SESSION['UserData'] = serialize($UserInfo);
		}
		else
		{
			echo Alert("Dados da Senha Ou Login estão Incorretos");
		}

		
	}
}
if(isset($_POST['logout']) || isset($_GET['logout']))
{
	unset($_SESSION["UserData"]);
	header('Location: index.php');
	
}

?>
<!DOCTYPE HTML>
<html lang="en-US">


<!-- Mirrored from ddt.wan.com/index by HTTrack Website Copier/3.x [XR&CO'2014], Mon, 29 Jun 2020 17:33:25 GMT -->
<!-- Added by HTTrack --><meta http-equiv="content-type" content="text/html;charset=utf-8" /><!-- /Added by HTTrack -->

<head>
  <script type="text/javascript">
    var _speedMark = new Date();
  </script>
  <meta charset="UTF-8">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
  <title><?php echo $titulo; ?></title>
  <meta name="Description" content="<?php echo $description; ?>" />
  <meta name="Keywords" content="<?php echo $KeyWords; ?>" />

  <link rel="shortcut icon" href="#" />
  <link rel="stylesheet" href="images/common236c.css?v=20171024">
  <link rel="stylesheet" type="text/css" media="screen" href="images/cb_style.css">
  <link rel="shortcut icon" href="images/favicon.ico" />
  <script src="images/jquery-1.8.0.min.js"></script>
  
</head>

<body>

  <div class="wrap-box index-bg">
    <div class="w980 clearfix">
		<!--head Start-->
      <div class="hearder p-r">
        <div class="slogan p-a"></div>
        <a class="link" href="#" target="_blank"></a>
        <style type="text/css">
        	.wrap-box{background: url(images/topbg3830.jpg?v=20200618) no-repeat 50% 0;}
        </style>
      </div>
      <!--head end-->
<link rel="stylesheet" href="images/indexaf27.css?ver=20170328">
<div class="w980 clearfix">
	<div class="index-left f-l">
		<div class="login p-r clearfix">
	<div class="play-game p-a">
    <img id="startGameFlash" src="images/data_02.png">
	</div>
	
	<?php
	if(!isset($_SESSION['UserData']))
	{
		echo ('
		
	<div class="unlogin-box clearfix">
		<form method="POST">
		<div class="input-box f-l p-r">
		    <input type="text" name="UserName" placeholder="Usuario">
		    <input type="password" name="Password" placeholder="Senha">
			<div class="blank"></div>

		</div>
		<button class="loginbtn f-l" type="submit" name="login"></button>
	    </form>
		<div class="forget f-l">
			<a href="#" class="reg-btn">Recuperar</a>
			<a href="cadastro.php" class="reg-btn">Cadastro</a>
		</div>
	 </div>');
	}
	else
	{
		$userdata = unserialize($_SESSION['UserData']);
		echo '
	<div class="login-box">
		<div class="input-box clearfix">
		<a href="" class="userImg f-l" target="_blank"></a>
		<div class="f-l">
			<div class="userVal ellipsis">'.$userdata->UserName.'</div>
		</div>
		<a href="?logout=true" class="loginOut f-l">[Sair]</a>
	    </div>
	    <h2>Ultimo Servidor：</h2>
		<a href="play.php" class="lastLogin">Servidor 1</a> 
	</div>
	';
	}
	?>
		

</div>
<div class="recom-ser mt10 clearfix" >
		<h2 class="h2-tit">Facebook</h2>
		<div class="">
			<div class="fb-page" data-href="https://www.facebook.com/DDTankTrue/" data-tabs="timeline" data-width="" data-height="400" data-small-header="false" data-adapt-container-width="true" data-hide-cover="false" data-show-facepile="true"><blockquote cite="https://www.facebook.com/DDTankTrue/" class="fb-xfbml-parse-ignore"><a href="https://www.facebook.com/DDTankTrue/">TrueTank</a></blockquote></div>
		</div>
</div>





		
</div>
	<div class="index-rgt f-r">
		<div class="focus clearfix">
		<div class="focus-img f-l">
				<a target="_blank" title="Evento #1" href="#">
				<img alt="Evento #1" width="503" height="200" src="images/20200629253018.png" />
			</a>
      <a target="_blank" title="Evento #2" href="#">
				<img alt="Evento #2" width="503" height="200" src="images/20200629253018.png" />
			</a>
      <a target="_blank" title="Evento #3" href="#">
				<img alt="Evento #3" width="503" height="200" src="images/20200629253018.png" />
			</a>
      <a target="_blank" title="Evento #4" href="#">
				<img alt="Evento #4" width="503" height="200" src="images/20200629253018.png" />
			</a>
		</div>
		<div class="focus-menu f-r">
						<span class="select">Evento #1</span>
						<span>Evento #2</span>
						<span>Evento #3</span>
						<span>Evento #4</span>
		</div>
	   </div>




		

</div>
</div>
</div>
</div>



<script src="images/ddt_common9b08.js?ver=1418196075193"></script>
<script src="images/d2_common8fa5.js?ver=20150710"></script>

<script src='http://ddt.wan.com/Public/www/ddt/style/scripts/clearbox.js'></script>

<!--百度分享-->


</body>
<div id="fb-root"></div>
<script async defer crossorigin="anonymous" src="https://connect.facebook.net/pt_BR/sdk.js#xfbml=1&version=v7.0&appId=172608803671801&autoLogAppEvents=1" nonce="zTfTRAFM"></script>

<!-- Mirrored from ddt.wan.com/index by HTTrack Website Copier/3.x [XR&CO'2014], Mon, 29 Jun 2020 17:33:25 GMT -->
</html>