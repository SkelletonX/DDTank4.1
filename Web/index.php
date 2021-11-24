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
		echo Alert("è Nescessario Precher o Campo da Senha");
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
	header('Location: .');
}

?>
<script type="text/javascript">
function minuscula(z){
v = z.value.toLowerCase();
z.value = v;
}
</script>
<!DOCTYPE HTML>
<script data-ad-client="ca-pub-4685558064289074" async src="./Assets/pagead/js/f.txt"></script>
<html lang="ru-RU">

<!-- Mirrored from ddtankOld.com.br/ by HTTrack Website Copier/3.x [XR&CO'2014], Sat, 11 Jan 2020 10:27:11 GMT -->
<!-- Added by HTTrack --><meta http-equiv="content-type" content="text/html;charset=UTF-8" /><!-- /Added by HTTrack -->
<head>
<meta charset="UTF-8">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<title><?php echo $titulo; ?></title>
<meta name="Description" content="<?php echo $description; ?>"/>
<meta name="Keywords" content="<?php echo $KeyWords; ?>" />
<link rel="shortcut icon" href="./Assets/images/favicon.ico"/>
<link rel="bookmark" href="./Assets/images/favicon.ico"/> 
<link rel="stylesheet" href="./Assets/css/ddt-index.css">
<link rel="stylesheet" href="./Assets/css/ddt-reset.css" >
<link rel="stylesheet" href="./Assets/css/ddt-style.css">
<link rel="stylesheet" href="./Assets/bootstrap/3.3.7/css/dbootstrap.min.css">
<script src="./Assets/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
<script src="./Assets/bootstrap/3.3.7/js/bootstrap.min.js"></script>
<link rel="stylesheet" type="text/css" media="screen" href="./Assets/js/clearbox/css/cb_style.css">
<script type="text/javascript" src="./Assets/js/jquery-1.7.1.js"></script>   
<script type="text/javascript" src="./Assets/js/webtoolkit.md5.js"></script>
<script type="text/javascript" src="./Assets/js/jquery.corner.js"></script>
<script type="text/javascript" src="./Assets/js/jquery-ui-1.8.21.custom.js"></script>
<script type="text/javascript" src="./Assets/js/jquery.ui.button.js"></script>
<script type="text/javascript" src="./Assets/js/password_ddt.js"></script>
</head>
<body>

<script src="./Assets/js/promo-top-bar.js"></script>
<div class="wrap-box index-bg">
	<div class="w980 clearfix">
<div class="hearder p-r">
	<div class="slogan p-a">
            <img class="ie6png" src="./Assets/images/logo/new-ddt.html" alt="" title="" >
        </a>
    </div>
    <div class="clearfix nav">
	    <ul class="nav-box p-a">    	
			<li class="nav1 ie6png">
				<a href="./Assets/index-2.html">Homem</a>
			</li>
			<li class="nav2 ie6png">
				<a href="./Assets/articles_list/eventos.html" >eventos</a>
			</li>
			<li class="nav3 ie6png">
				<a href="./Assets/articles_list/anuncios.html" >anuncios</a>
			</li>
			<li class="logo">
				<h1>
					<a href="./Assets/index-2.html" title="DDTank Old | versão antiga">DDTank Old | versão antiga</a>
				</h1>
			</li>
			<li class="nav4 ie6png">
				<a href="./Assets/articles_list/news.html" target="_blank">Noticias</a>
			</li>
			<li class="nav5 ie6png">
				<a href="./recarga/" target="_blank">recarga</a>
			</li>
			<li class="nav6 ie6png">
				<a href="./Assets/#" target="_blank">suporte</a>
			</li>		
	    </ul>

	    
	    <div class="submenu" style="display: none;">
			
			</p>
			
		</div>
	</div>
</div>		<div class="w980 clearfix">
			<div class="index-left f-l">
<div class="login p-r clearfix">
	<div class="play-game p-a">
			<embed align="right" id="startGameFlash" src="./images/data_02.png" type="application/x-shockwave-flash" width="256" height="290" quality="high" pluginspage="https://www.macromedia.com/go/getflashplayer" wmode="transparent" allowscriptaccess="always">
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
			<a href="javascript:void(0)" type="button"  data-toggle="modal" data-target="#myModal">Registrar conta</a> | <a href="javascript:void(0)" data-toggle="modal" data-target="#recuperar">Recuperar senha?</a>
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
<div class="modal fade" id="myModal" role="dialog">
    <div class="modal-dialog">
    
      <!-- Registro de Conta-->
      <div class="modal-content">
        <div class="modal-header">
		
                  <button type="button" class="close" data-dismiss="modal">&times;</button>
		          <center>
		        	<p>Criar uma Conta</p>
        
		        	</center>
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
					<div class="modal-footer">
	              
                    <button type="button" class="btn btn-sm btn-danger" data-dismiss="modal">Cancelar</button>
                    <button type="submit" name="register" class="large orangellow button">Cadastrar</button>
                  </div>
				  </form>
							
                  
			</div>
        </div>
    </div>
</div>
 
    
         
	<div class="modal fade" id="recuperar" role="dialog">
    <div class="modal-dialog">
    
      <!-- Registro de recuperação de Conta-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <center>
            <h2>O email tem que ser original para recuperação!</h2>
			<p>Recuperar a sua conta</p>
			</center>
        <p> 
            <center><label for="" class="" >Email</label>
            <center><input id="" name="" type="text" placeholder="Adiciona seu Email"/>
        </p><br>
		<center>
		<a class="large orangellow button">Confirma</a>
        </div>
      </div>
      
    </div>
  </div>
		 
</div>

<div class="ranking mt10 clearfix">
	<h2 class="h2-tit">Facebook</h2>
	<div class="widget">
<iframe src="https://www.facebook.com/plugins/page.php?href=https%3A%2F%2Fwww.facebook.com%2FDDTDEV%2F&tabs=timeline&width=259&height=500&small_header=false&adapt_container_width=true&hide_cover=false&show_facepile=true&appId=172608803671801" width="340" height="500" style="border:none;overflow:hidden" scrolling="no" frameborder="0" allowTransparency="true" allow="encrypted-media"></iframe>
</div>
	<div class="">
		<div class="">
		
			
		</div>
		<ul class="">

		</ul>
	</div>
</div>
      </div>
			<div class="index-rgt f-r">
				<div class="focus clearfix">
					<div class="focus-img f-l">
						
						    <a target="_blank" title="" >
						      <img alt="A volta das Old" width="503" height="200" src="./Assets/images/slider/20000.jpg" />
						    </a>
						    <a target="_blank" title="" >
						      <img alt="O Retorno do labolatorio" width="503" height="200" src="./Assets/images/slider/200001.jpg" />
						    </a>
						    <a target="_blank" title="" >
						      <img alt="A melhor versão está de volta" width="503" height="200" src="./Assets/images/slider/200002.jpg" />
						    </a>
						    <a target="_blank" title="" >
						      <img alt="Uma nova jornada nos Aguarda" width="503" height="200" src="./Assets/images/slider/200003.jpg" />
						    </a>
	           		</div>
					<div class="focus-menu f-r">
							<span class="select">DDtank Na Sua Melhor Forma!</span>
							<span >O Retorno do labolatorio!</span>
							<span >Com Parceiros Fica Mais Divertido!</span>
							<span >Uma nova jornada nos Aguarda!</span>
						
					</div>
				</div>
				<div class="news mt20 clearfix">
					<div class="news-box f-l">
						<div class="news-tab clearfix">
							<span class="select"><cite></cite>Hot</span>
							<span><cite></cite>Noticias</span>
							<span><cite></cite>Eventos</span>
							<span><cite></cite>Anùncios</span>
							<a class="news-more f-r" href="#" target="_blank">Ver Mais</a>
						</div>
						<div class="clearfix">
							<div class="news-content">
								<ul class="news-ul clearfix">
						                        <li class="first">
							                        <h2 class="clearfix">
							                          <a target="_blank" title="" href="#">
					                                      Uma nova jornada nos aguarda!  
							                          </a>
							                        </h2>
							                        <p class="clearfix">
							                          A segurança sempre em primeiro lugar. Sempre coloque uma senha em sua mochila para manter a sua conta mais segura!
							                          <a target="_blank" class="show fn-right" title="" href="./Assets/gamedata/detail/49.html">Ver mais</a>
							                        </p>
												</li>
	
			
	
												
											<li >
												<span>Hot</span>
												
												<a href="#" title="Evento Facebook #2" target="_blank">Em Desenvolvimento</a>
												<em>[17:03]</em>
											</li>                      		
											
	
	
												
											<li >
												<span>Hot</span>
												
												<a href="#" title="Servidor s1" target="_blank">Servidor S1</a>
												<em>[17:41]</em>
											</li>                      		
											
																							
								</ul>
							</div>
							<div class="news-content hidden">
								<ul class="news-ul clearfix">
						                        <li class="first">
							                        <h2 class="clearfix">
							                          <a target="_blank" title="" href="#">
					                                     Em Desenvolvimento
							                          </a>
							                        </h2>
							                        <p class="clearfix">
							                          Em Desenvolvimento
							                          <a target="_blank" class="show fn-right" title="" href="#">Ver detalhes>></a>
							                        </p>
												</li>                     		
	
									
								</ul>
							</div>
							<div class="news-content hidden">
								<ul class="news-ul clearfix">
						                        <li class="first">
							                        <h2 class="clearfix">
							                          <a target="_blank" title="" href="#">
					                                     Em Desenvolvimento
							                          </a>
							                        </h2>
							                        <p class="clearfix">
							                          As novidade de eventos nos servidores está sempre aqui......
							                          <a target="_blank" class="show fn-right" title="" href="#">Ver mais>></a>
							                        </p>
												</li>                     		
	
												
											<li >
												<span>Eventos</span>
												
												<a href="#" title="Evento Facebook #2" target="_blank">Evento Facebook #2</a>
												<em>[17:03]</em>
											</li>                      		
											
												
											<li >
												<span>Eventos</span>
												
												<a href="#" target="_blank">Evento especial de natal</a>
												<em>[22:02]</em>
											</li>                      		
											
												
											<li >
												<span>Eventos</span>
												
												<a href="#" title="Evento Facebook #1" target="_blank">Evento Facebook #1</a>
												<em>[19:09]</em>
											</li>                      		
											
									
											</li>                      		
								</ul>
							</div>
							<div class="news-content hidden">
								<ul class="news-ul clearfix">
						                        <li class="first">
							                        <h2 class="clearfix">
							                          <a target="_blank" title="" href="#">
					                                      Em Desenvolvimento
							                          </a>
							                        </h2>
							                        <p class="clearfix">
							                          Nome da atividade: Você atualizou minhas atividades para presentear: Puxar o vento e andar de bicicleta . 
							                          <a target="_blank" class="show fn-right" title="" href="#">Ver detalhes>></a>
							                        </p>
												</li>                     		
	
												
											<li >
												<span>Anùncios</span>
												
												<a href="#" title="Servidor s1 o retorno em manutenção" target="_blank">Servidor s1 o retorno em manutenção</a>
												<em>[17:41]</em>
											</li>                      		
											
												
											<li >
												<span>Anùncios</span>
												
												<a href="#" title="Notas da manutenção do servidor s1 o retorno " target="_blank">Notas da manutenção do servidor s1 o retorno </a>
												<em>[ 15:36]</em>
											</li>                      		
											
									
								</ul>
							</div>
						</div>
					</div>
					<div class="activity-box f-r">
						
						    <a target="_blank" title="" class="first" href="/recarga/">
						      <img alt="今日活动" src="./Assets/images/shop.jpg" />
						    </a>
						    <a target="_blank" title=""  href="#">
						      <img alt="十一月专题活动" src="./Assets/images/s0.jpg" />
						    </a>
					</div>
				</div>
				<div class="introduce_bg inborder">
                    	<a class="box-more f-r" href="#" target="_blank">Mais</a>
                    	<ul class="introduce_tit">
                                                                                </ul>
                        <ul class="introduce_con">
                        	                        	                            <li style="display: list-item;">
                            	<a target="_blank" >
                            		<img style="width: 706px;" src="./images/dab2.png">
                            	</a>
                            </li>
                                                                                	                            <li style="display: none;">
                            	<a target="_blank" href="#">
                            		<img style="width: 706px;" src="./Assets/images/registro/1565162.png">
                            	</a>
                            </li>
                                                                                </ul>
                    </div>
				
				<div class="weapon mt10">
					<div class="box-tit weapon-tit">
						<a class="box-more f-r" href="./Assets/main.html" target="_blank">Mais</a>
						<strong>Arma de estimação</strong>
					</div>
					<div class="weapon-box">
						<div class="weapon-menu p-r clearfix">
							<span class="select">Armas</span>
							<span>Chapeu</span>
							<span>Roupas</span>
							<span>asas</span>
							<span>Ternos</span>
							<span>Acessórios</span>
							
							<div class="tab-underline p-a"></div>
						</div>
						<div class="clearfix">
							<ul class="weapon-ul clearfix">
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Armas/0001.png" alt="Bumerangue Do Amors">
										<span>Bumerangue Do Amor</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Armas/0002.png" alt="Lança da Antiguidade">
										<span>Lança da Antiguidade</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Armas/0003.png" alt="Cabeça De Boi">
										<span>Cabeça De Boi</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Armas/0004.png" alt="Super - Quebra Tijolos">
										<span>Super - Quebra Tijolos</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Armas/0005.png" alt="Super - Fogo Intenso">
										<span>Super - Fogo Intenso </span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Armas/0006.png" alt="Super - Kit Medico">
										<span>Super - Kit Medico </span>
									</a>
								</li>
							</ul>
							<ul class="weapon-ul clearfix hidden">
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Chapeu/0001.png" alt="Primavera Verde">
										<span>Primavera Verde</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Chapeu/0002.png" alt="Chapéu do Barão">
										<span>Chapéu do Barão</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Chapeu/0003.png" alt="Chapéu do Bugou">
										<span>Chapéu do Bugou</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Chapeu/0004.png" alt="Ouvidos de lobo">
										<span>Ouvidos de lobo</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Chapeu/0005.png" alt="Pequeno Gorro do demônio">
										<span>Pequeno Gorro</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Chapeu/0006.png" alt="Arrependimento Demônio das Trevas">
										<span>Arrependimento</span>
									</a>
								</li>
							</ul>
							<ul class="weapon-ul clearfix hidden">
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/roupas/0001.png" alt="Armadura de Matias">
										<span>Roupa de Matias</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/roupas/0002.png" alt="Raiva de Solaan">
										<span>Raiva de Solaan</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/roupas/0003.png" alt="Guerreiro Demônio">
										<span>Guerreiro </span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/roupas/0004.png" alt="Capa de Matias">
										<span>Capa de Matias</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/roupas/0005.png" alt="Mano">
										<span>Mano</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/roupas/0006.png" alt="Bravo Mago">
										<span>Bravo Mago</span>
									</a>
								</li>
							</ul>
							<ul class="weapon-ul clearfix hidden">
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/asas/0001.png" alt="Asas do Amor Assistente">
										<span>Asas do Amor </span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/asas/0002.png" alt="Lâmpada de Aladim">
										<span>Lâmpada Aladim</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/asas/0003.png" alt="Bola Colorida do Parque de Diversão">
										<span>Bola Colorida</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/asas/0004.png" alt="Previsão de tempo">
										<span>Deus da Guerra</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/asas/0005.png" alt="Encantador e fascinante">
										<span>Previsão de tempo</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/asas/0006.png" alt="Encantador e fascinante">
										<span>Encantadar</span>
									</a>
								</li>
							</ul>
							<ul class="weapon-ul clearfix hidden">
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Ternos/0001.png" alt="Bugou das Profundezas do Mar">
										<span>Bugou do Mar</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Ternos/0002.png" alt="Leão(M)">
										<span>Leão(M)</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Ternos/0003.png" alt="Libra dourada">
										<span>Libra dourada</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Ternos/0004.png" alt="Dama de doce coração">
										<span>Top 1 Hall</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Ternos/0005.png" alt="Uniforme do líder das estudantes (Feminino)">
										<span>Estudantes</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Ternos/0006.png" alt="Bela Princesa Fênix">
										<span>Bela Princesa</span>
									</a>
								</li>
							</ul>
							<ul class="weapon-ul clearfix hidden">
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Acess%c3%b3rios/0001.png" alt="Pedra de fortalecimento nível 1">
										<span>Pedra nível 1</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Acess%c3%b3rios/0002.png" alt="Pedra de fortalecimento nível 2">
										<span>Pedra nível 2</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Acess%c3%b3rios/0003.png" alt="Pedra de fortalecimento nível 3">
										<span>Pedra nível 3</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Acess%c3%b3rios/0004.png" alt="Pedra de fortalecimento nível 4">
										<span>Pedra nível 4</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Acess%c3%b3rios/0005.png" alt="Pedra de fortalecimento nível 5">
										<span>Pedra nível 5</span>
									</a>
								</li>
								<li>
									<a  target="_blank">
										<img src="./Assets/images/img/Acess%c3%b3rios/0006.png" alt="Símbolo dos Deuses">
										<span>Símbolo</span>
									</a>
								</li>
							</ul>
							
						</div>
					</div>
				</div>
				
			</div>
		</div>
	</div>
</div>
<div id="DynamicContent">
    <div id="loading" >Conectando ao servidor!<img alt=""  src="./Assets/images/loading.gif" /></div>               		
</div>
<div id="ErrorContent">
	<div id="content"></div>               		
</div>
    <div id="codeContent">
        <table style="width:100%;">
        <tr>
            <td class="auto-style1">
                <img id="ImageCode" src="./Assets/auth/validatecode.gif" height="32" width ="127" alt="" />  
            </td>
            <td style="text-align: center" >
                 <input type="text"  style="font-size: 14px;width: 120px" name="code" placeholder="Введите код безопасности" id="code"/>
            </td>
            <td>
                <a id="ReLoad" title="" href="./Assets/javascript:href()" style="font-size: 12px; color: blue">Atualizar</a>  
            </td>
        </tr>
		<tr>
		    <td colspan="3" style="text-align: center;">
		       <a class="small green button" type="" id="checkCode" onclick="return checkCode();">Inscreva-se!</a>
		    </td>
		</tr>
        </table>     		
	</div>
<div id="TB_overlayBG"></div>
<div class="footer-img"></div>
<link rel="stylesheet" href="./Assets/js/footer/css/footer.css" />
<script src="./Assets/js/footer/js/footer.js"></script>
<script src="./Assets/js/ddt2/ddt-index.js"></script>
<script src="./Assets/js/clearbox/js/clearbox.js"></script>
<!-- Load Facebook SDK for JavaScript -->

      <!-- Your customer chat code -->
      <div class="fb-customerchat"
        attribution=setup_tool
        page_id="196175584620834"
  logged_in_greeting="Oi! Como podemos te ajudar?"
  logged_out_greeting="Oi! Como podemos te ajudar?">
      </div>
<div id="Div1">

</body>

<!-- Mirrored from ddtankOld.com.br/ by HTTrack Website Copier/3.x [XR&CO'2014], Sat, 11 Jan 2020 10:27:42 GMT -->
</html>
