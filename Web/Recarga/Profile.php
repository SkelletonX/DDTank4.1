
<?php
include('../global.php');

if(!isset($_SESSION['UserData']))
{
    header('Location: index.php');
}

$UserData = unserialize($_SESSION['UserData']);
if($UserData->isUserIDValid())
{
    $UserData->GameData();
}


$Personal = new PersonalData($UserData->GameID);

if(isset($_POST["Cadastro"]))
{
    $flag = true;
    $cpf = $_POST["CPF"];
    $telefone = $_POST["phone"];
    if(!$Personal->SetCPF($cpf))
    {
        echo Alert("CPF invalido. Tem que ter - e .");
        $flag = false;
    }
    if(!$Personal->SetPhone($telefone))
    {
        echo Alert("Telefone invalido Ex:+540000000");
        $flag = false;
    }
    if(strlen($_POST["first"]) < 3 || !isset($_POST["first"]))
    {
        echo Alert("Nome Invalido");
        $flag = false;
    }
    else
    {
        $Personal->FirstName = $_POST["first"];
    }

    if(strlen($_POST["second"]) < 3 || !isset($_POST["second"]))
    {
        echo Alert("SobreNome Invalido");
        $flag = false;
    }
    else
    {
        $Personal->LastName = $_POST["second"];
    }






    if($flag)
    {
        if($Personal->Create())
        {
            echo Alert("Dados Enviados Com Sucesso");
        }
    }
    
}



$temdados = $Personal->Get();




if(isset($_POST["Update"]))
{
    $telefone = $_POST["phone"];
    if($Personal->Phone != $telefone)
    {
        if($Personal->SetPhone($telefone))
        {
           if($Personal->Update())
           {
             echo Alert("Telefone Alterado Com Sucesso");
           }
           else
           {
            echo Alert("Falha Ao Alterar Telefone");
           }
        }
        else
        {
            echo Alert("Telefone Invalido");
        }
    }
    else
    {
        echo Alert("Você Não Fez Alterações");
    }
}
?>
<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge"> 
	
    <link rel="shortcut icon" href="../Assets/Recarga/images/favicon.ico" type="image/x-icon">
    <link rel="icon" href="../Assets/Recarga/images/favicon.ico" type="image/x-icon">
	
    <title>Recarga</title>
	
    <link href="../Assets/Recarga/css/bootstrap.min.css" rel="stylesheet">      
    <link href="../Assets/Recarga/css/font-awesome.min.css" rel="stylesheet">
    <link href="../Assets/Recarga/css/magnific-popup.css" rel="stylesheet">
    <link href="../Assets/Recarga/css/jquery-ui.css" rel="stylesheet">
    <link href="../Assets/Recarga/css/animate.css" rel="stylesheet">
    <link href="../Assets/Recarga/css/owl.carousel.min.css" rel="stylesheet">
    <link href="../Assets/Recarga/css/main.css" rel="stylesheet">
</head>


<body>

    <!-- Preloader -->
    <div class="preloader">
        <div class="lds-ring"><div></div><div></div><div></div><div></div></div>
    </div><!--/Preloader -->
    <header class="header-area blue-bg">
        <nav class="navbar navbar-expand-lg main-menu">
            <div class="container">

                <!-- LOGO IMG
				<a class="navbar-brand" href=""><img src="https://i.imgur.com/MmcVc4t.png" height="515" class="d-inline-block align-top" alt=""></a> !-->
		        <h1><font color="white">TrueTank</font></h1>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="menu-toggle"></span>
                </button>

                <div class="collapse navbar-collapse" id="navbarSupportedContent">
                <ul class="navbar-nav ml-auto">
				<li class="nav-item"><a class="nav-link" href="../index.php">Inicio</a></li>
                <li class="nav-item"><a class="nav-link" href="./index.php">Recarga</a></li>
				<!-- <li class="nav-item"><a class="nav-link" href="matches.html">Noticias</a></li> -->
				<li class="nav-item"><a class="nav-link" href="https://www.facebook.com/DDTankTrue/">Facebook</a></li>
				<!-- <li class="nav-item"><a class="nav-link" href="matches.html">Discord</a></li>
				<li class="nav-item"><a class="nav-link" href="matches.html">WhatsApp</a></li> -->

                    </ul>
                    <div class="header-btn justify-content-end">
                        <a href="./Profile.php" class="bttn-small btn-fill">Perfil</a>
                    </div>
                </div>
            </div>
        </nav>
    </header>
    <section class="section-padding-2 blue-bg shaded-bg">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-xl-6 centered">
                    <div class="section-title cl-white">
                        <h4>Configure Sua Conta</h4>
                         <h2>SERVIDOR 01 - ILHA</h2>
                    </div>
                </div>
            </div>
              <div class="card">
                <div class="card-body mx-auto">
                <?php
               
                if($temdados)
                {
                    echo '
                  <h5 class="card-title">Dados Do usuario:</h5>
                  <form method="Post">
                   <div class="form-group">
                     <label for="exampleInputEmail1">Nome</label>
                     <input name="first" type="text" value="'.$Personal->FirstName.'" class="form-control"  aria-describedby="emailHelp" disabled>
                   </div>
                   <div class="form-group">
                     <label for="exampleInputPassword1">Sobrenome</label>
                     <input name="second" type="text" value="'.$Personal->LastName.'" class="form-control" disabled>
                   </div>
                   <div class="form-group">
                     <label for="exampleInputPassword1">CPF</label>
                     <input name="CPF" type="text" value="'.$Personal->CPF.'" class="form-control" disabled>
                   </div>
                   <div class="form-group">
                     <label for="exampleInputPassword1">Numero De Telefone</label>
                     <input name="phone" type="text" value="'.$Personal->Phone.'" class="form-control">
                   </div>
                   <button type="submit" name="Update" class="btn btn-primary">Atualizar Dados</button>
                 </form>';
                }
                else
                {
                    echo '
                  <h5 class="card-title">é Nescessario Completar Seus Dados Pessoais primeiro</h5>
                  <form method="Post"> 
                   <div class="form-group">
                     <label for="exampleInputEmail1">Nome</label>
                     <input name="first" type="text" class="form-control" id="exampleInputEmail1" aria-describedby="emailHelp">
                   </div>
                   <div class="form-group">
                     <label for="exampleInputPassword1">Sobrenome</label>
                     <input name="second" type="text" class="form-control" id="exampleInputPassword1">
                   </div>
                   <div class="form-group">
                     <label for="exampleInputPassword1">CPF</label>
                     <input name="CPF" type="text" class="form-control" id="exampleInputPassword1">
                   </div>
                   <div class="form-group">
                     <label for="exampleInputPassword1">Numero De Telefone</label>
                     <input name="phone" type="text" class="form-control" id="exampleInputPassword1">
                   </div>
                   <button type="submit" name="Cadastro" class="btn btn-primary">Cadastrar Dados</button>
                 </form>';
                }
                ?>  
                </div>
              </div>
            </div>
    </section>
    <footer class="footer-area section-padding-2">
        <div class="container">
            <div class="row">
                <div class="col-xl-3 col-lg-3 col-md-6 col-sm-6">
                    <div class="footer-widget">
                        <h3>Area De Recarga</h3>
                        <p>Olá Jogador(a) Você esta efetuando uma recarga dentro do servidor</p>
                        <div class="social">
                            <a href="https://www.facebook.com/DDTankTrue/" class="facebook-bg"><i class="fa fa-facebook"></i></a>
                            <a href="https://www.youtube.com/channel/UCOOsEuUchRxol0I3vqRm2rg" class="youtube-bg"><i class="fa fa-youtube-play"></i></a>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-lg-3 col-md-6 col-sm-6">
                    <div class="footer-widget footer-nav">
                        <h3>Atalhos</h3>
                        <ul>
                            <li><a href="/index.php">Inicio</a></li>
                            <li><a href="/play.php">Jogar</a></li>
                            <li><a href="https://chat.whatsapp.com/F0LBJNXYynA4YR7yiGphVo">WhatsApp</a></li>
                            <li><a href="https://www.facebook.com/DDTankTrue/">Facebook</a></li>
                        </ul>
                    </div>
                </div>

            </div>
        </div>
    </footer>
    <script src="../Assets/Recarga/js/jquery-3.2.1.min.js"></script>
    <script src="../Assets/Recarga/js/jquery-migrate.js"></script>
    <script src="../Assets/Recarga/js/jquery-ui.js"></script>
    <script src="../Assets/Recarga/js/popper.js"></script>
    <script src="../Assets/Recarga/js/bootstrap.min.js"></script>
    <script src="../Assets/Recarga/js/owl.carousel.min.js"></script>
    <script src="../Assets/Recarga/js/magnific-popup.min.js"></script>
    <script src="../Assets/Recarga/js/imagesloaded.pkgd.min.js"></script>
    <script src="../Assets/Recarga/js/isotope.pkgd.min.js"></script>
    <script src="../Assets/Recarga/js/waypoints.min.js"></script>
    <script src="../Assets/Recarga/js/jquery.counterup.min.js"></script>
    <script src="../Assets/Recarga/js/wow.min.js"></script>
    <script src="../Assets/Recarga/js/scrollUp.min.js"></script>
    <script src="../Assets/Recarga/js/script.js"></script>
</body>
</html>