
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

$temdados = $Personal->Get();

if(!$Personal->Get())
{
  header('Location: ./Profile.php');
}

$pacote = new Pacote(null);
$ped = null;

if(isset($_GET["pedido"]))
{
    $ped = new PicPayPayment($_GET["pedido"]) ;
	$ped->Get();
	$pacote->ID = $ped->PackID;
    $pacote->Get();
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
                        <h4>Pedido</h4>
                        <h2></h2>
                    </div>
                </div>
            </div>
              <div class="card">
                <div class="card-body mx-auto">
                <?php 
                if(!isset($ped->Status) || $ped->Status == ""|| $ped->Status == " ")
                {
                    $xPicpayToken = '1bbc069d-5a4b-45f6-85fb-ce40416fd998';
                    $decoded = GetPicpayData($ped->ReferenceId,$xPicpayToken);
                    $ped->Status = $decoded['status'];
                    $ped->AuthId = $decoded['authorizationId'];
                    $ped->Update();
                    if($ped->UserID == $Personal->UserID)
                    {
                       if($decoded['status'] == "paid")
                       {
                           $UserData->SendRecharge($ped->ReferenceId,$pacote->Coupons*2,$ped->Value);
                       }
                    }

                }       

                if($ped->Get() && $ped->UserID == $Personal->UserID)
                {

                    echo '
                  <h5 class="card-title">ID:'.$ped->ReferenceId.'</h5>
                  <h5 class="card-title">Status:'.$ped->Status.'</h5>
                  <h5 class="card-title">Cupons: '.$pacote->Coupons.'</h5>
                  <h5 class="card-title">Valor: '. $ped->Value.' BRL</h5>';
                }
                else if($ped->Get() && $ped->UserID != $Personal->UserID)
                {
                    echo '
                    <h5 class="card-title">Esse Pedido Não è Seu</h5>
                    ';
                }
                else
                {
                    echo '
                  <h5 class="card-title">Pedido Invalido</h5>
                  ';
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