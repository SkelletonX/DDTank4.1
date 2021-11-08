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

if(isset($_POST["register"]))
{
	@$username = addslashes($_POST['Username']);
	@$password = $_POST['password'];
	@$Repassword = $_POST['Repassword'];
	@$Nickname = $_POST['Nickname'];
	@$sex = (int)$_POST['sex'];
	@$email = $_POST['Email'];
	$text_r = '';
	if($username == null || $password == null || $Repassword == null || $Nickname == null || $email == null)
	{
        $text_r .= "Todos os Campos precisam ser preenchidos";
	}
	if(!preg_match("/^([a-zA-Z0-9\-\_]*)$/",$username) || !preg_match("/^([a-zA-Z0-9\-\_]*)$/",$Nickname)) {
		$text_r .= 'Login ou Nick invalido';
	}
	if(!filter_var($email,FILTER_VALIDATE_EMAIL)) $text_r .= ' seu email não é valido <br>';
	if($password != $Repassword) $text_r.= 'Suas senhas não são iguais <br>';
	if(strlen($username)  < 6 || strlen($username)  > 30) $text_r .= 'Usuário deve ter  de 6 a 30 caracteres <br>';
	if(strlen($password)  < 6 || strlen($password)  > 30) $text_r .= 'A senha deve ter de 6 a 30 caracteres <br>';
	if(strlen($Nickname)  < 6 || strlen($Nickname)  > 30) $text_r .= 'Nick deve ser de 6 a 30 caracteres <br>';
	if (strpos("1".$Nickname,"GM") or strpos("1".$Nickname,"à¸ˆà¸µà¹€à¸­à¹‡à¸¡") or strpos("1".$Nickname,"Gunny") or strpos("1".$Nickname,"Game Master")or strpos("1".strtolower($Nickname),"adm")or strpos("1".strtolower($Nickname),"gm")or strpos("1".strtolower($Nickname),"mod")) {
		$text_r .="AS PALAVRAS ADM,MOD,GM NAO PODEM SER UTILIZADAS EM SEU NICK";
	}
	if($text_r == '') {
		co();
		$password = strtoupper(md5($password));
		$q = q("Select TOP 1 UserId From Mem_Users Where UserName = '{$username}'");
		if(qn($q) == 0) {
			$q = q("Select TOP 1 UserId From Webshop_Account Where Email = '{$email}'");
			if(qn($q) == 0) {
				$q = q("Select TOP 1 UserId From ".$dbtank41.".dbo.Sys_Users_Detail Where NickName = '{$Nickname}'");
				if(qn($q) == 0) {
					q("exec ".$config['Database'].".dbo.Webshop_Register @ApplicationName=N'DanDanTang',@UserName=N'{$username}',@password=N'{$password}',@email='{$email}',@passtwo = '".strtoupper(md5($password))."',@error = 0");
					q("exec ".$dbtank41.".dbo.SP_Users_Active @UserID='',@Attack=0,@Colors=N',,,,,,',@ConsortiaID=0,@Defence=0,@Gold=100000,@GP=0,@Grade=1,@Luck=0,@Money=0,@Style=N',,,,,,',@Agility=0,@State=0,@UserName=N'{$username}',@PassWord=N'{$password}',@Sex='".$sex."',@Hide=1111111111,@ActiveIP=N'',@Skin=N'',@Site=N''");
					if($sex == 1) {
						q("exec ".$dbtank41.".dbo.SP_Users_RegisterNotValidate @UserName=N'".$username."',@PassWord=N'{$password}',@NickName=N'{$Nickname}',@BArmID=7008,@BHairID=3158,@BFaceID=6103,@BClothID=5160,@BHatID=1142,@GArmID=7008,@GHairID=3158,@GFaceID=6103,@GClothID=5160,@GHatID=1142,@ArmColor=N'',@HairColor=N'',@FaceColor=N'',@ClothColor=N'',@HatColor=N'',@Sex='{$sex}',@StyleDate=0");
					}
					else {
						q ("exec ".$dbtank41.".dbo.SP_Users_RegisterNotValidate @UserName=N'{$username}',@PassWord=N'{$password}',@NickName=N'{$Nickname}',@BArmID=7008,@BHairID=3244,@BFaceID=6204,@BClothID=5276,@BHatID=1214,@GArmID=7008,@GHairID=3244,@GFaceID=6202,@GClothID=5276,@GHatID=1214,@ArmColor=N'',@HairColor=N'',@FaceColor=N'',@ClothColor=N'',@HatColor=N'',@Sex='{$sex}',@StyleDate=0");
					}
					q("exec ".$dbtank41.".dbo.SP_Users_LoginWeb @UserName=N'{$username}',@Password=N'',@FirstValidate=0,@NickName=N'{$Nickname}'");
					
					echo Alert("Registro concluido, por favor logue-se");
				} else echo Alert("Este nick ja esta sendo usado");
			} else  echo Alert("Este email ja esta sendo usado");
		} else echo Alert("Este login ja esta sendo usado");
	}
	else
	{
		echo Alert($text_r);
	}

}

if(isset($_POST['logout']) || isset($_GET['logout']))
{
	unset($_SESSION["UserData"]);
	header('Location: cadastro.php');
}

?>
<!DOCTYPE HTML>
<html lang="en-US">


<!-- Mirrored from ddt.wan.com/article/read/id/7594.html by HTTrack Website Copier/3.x [XR&CO'2014], Mon, 29 Jun 2020 17:43:19 GMT -->
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
  <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css" integrity="sha384-9aIt2nRpC12Uk9gS9baDl411NQApFmC26EwAOH8WgZl5MYYxFfc+NcPb1dKGj7Sk" crossorigin="anonymous">
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

	<div class="pageTit f-l">
		<div class="bread f-r">Cadastro</div>	
	</div>
	<br/>
	<br/>
	<div class="pageBox">
		<div class="share-bar clearfix">
		<div class="search-bd f-r clearfix">

        </div>
        </div>
		<div class="news-content">
			<p><strong><span style="font-size:14px;"><span style="color:#7F7F7F;font-size:14px;background-color:#FFFF00;">
		    <div class="card mx-auto my-2" style="max-width: 400px;">
				<form class="mx-2 my-2" method="POST">
					<div class="form-group">
					  <label for="exampleInputEmail1">Usuario</label>
					  <input name="Username" type="text" class="form-control"aria-describedby="Username">
					</div>
					<div class="form-row">
						<div class="col">
						  <label for="exampleInputEmail1">Senha</label>
						  <input name="password" type="text" class="form-control" placeholder="Senha">
						</div>
						<div class="col">
							<label for="exampleInputEmail1">Confirmar Senha</label>
						  <input name="Repassword" type="text" class="form-control" placeholder="Confirmar Senha">
						</div>
					</div>
					<div class="form-row">
						<div class="col">
							<label for="exampleInputEmail1">Nome Da Conta</label>
						  <input name="Nickname" type="text" class="form-control" placeholder="Nome Da Conta">
						</div>
						<div class="col">
							<label for="exampleInputEmail1">Sexo Da Conta</label>
							<select name="sex" id="inputState" class="form-control">
							  <option selected value="1">Masculino</option>
							  <option value="0">Feminino</option>
							</select>
						</div>
					</div>
					<div class="form-group my-2">
						<label for="exampleInputEmail1">Email</label>
						<input type="email" name="Email" class="form-control" id="exampleInputEmail1" aria-describedby="emailHelp">
					</div>
					<div class="form-group my-2">
					<button name="register" type="submit" class="btn btn-warning">Confirmar Cadastro</button>
					</div>
				  </form>
		    </div>
		</div>
		<br/>
		<br/>
	</div>
</div>
<!--footer begin-->
 </div>
</div>


<script src="images/ddt_common.js?ver=1418196075193"></script>
<script src="images/d2_Common.js?ver=20150710"></script>


<script src='http://ddt.wan.com/Public/www/ddt/style/scripts/clearbox.js'></script>

<div id="fb-root"></div>
<script async defer crossorigin="anonymous" src="https://connect.facebook.net/pt_BR/sdk.js#xfbml=1&version=v7.0&appId=172608803671801&autoLogAppEvents=1" nonce="zTfTRAFM"></script>

</body>

<!-- Mirrored from ddt.wan.com/article/read/id/7594.html by HTTrack Website Copier/3.x [XR&CO'2014], Mon, 29 Jun 2020 17:43:19 GMT -->
</html>