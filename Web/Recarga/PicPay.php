
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
	<link href="../Assets/Recarga/css/diego.css" rel="stylesheet">
</head>
<body>
    <header class="header-area blue-bg">
        <nav class="navbar navbar-expand-lg main-menu">
            <div class="container">
                <!-- LOGO IMG
				<a class="navbar-brand" href="index.php"><img src="https://i.imgur.com/MmcVc4t.png" height="515" class="d-inline-block align-top" alt=""></a> !-->
		        <h1><font color="white">TrueTank - Recarga PicPay</font></h1>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span class="menu-toggle"></span>
                </button>

                <div class="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul class="navbar-nav ml-auto">


                    </ul>
                    <div class="header-btn justify-content-end">
                        <a href="../index.php" class="bttn-small btn-fill">Inicio</a>
                    </div>
                </div>
            </div>
        </nav>
    </header>
<?php

include('../global.php');

if(!isset($_SESSION['UserData']))
{
    header('Location: index.php');
    die();
}

$UserData = unserialize($_SESSION['UserData']);
if($UserData->isUserIDValid())
{
    $UserData->GameData();
}

$Personal = new PersonalData($UserData->GameID);
$data = $Personal->Get();


if(!$data)
{
  header('Location: ./Profile.php');
  die();
}
$pac = new Pacote(null);
$pic = false;
if(isset($_GET['ID']))
{

    $pac->ID = $_GET['ID'];
    $pic = $pac->Get();
    
}



?>
	<center>
	<footer class="footer-area section-padding-2">
<?php
if($pic)
{
//Token PicPay
$xPicpayToken = '1bbc069d-5a4b-45f6-85fb-ce40416fd998';

//URL da loja
$callbackUrl = "http://truetank.com.br/recarga/callback.php";
$returnUrl = "http://truetank.com.br/recarga/pedido.php?pedido=";


//Dados da fatura
// Padrão
$referenceId = rand(100000, 999999);
$expiresAt = date(DATE_ATOM, strtotime(' + 1 days'));

//Dados do comprador

$dados = [
    "referenceId" => $referenceId,
    "callbackUrl"=> $callbackUrl,
    "returnUrl"=> $returnUrl . $referenceId,
    "value"=> $pac->Value,
    "expiresAt"=> $expiresAt,
    "buyer"=> [
      "firstName"=> $Personal->FirstName,
      "lastName"=> $Personal->LastName,
      "document"=> $Personal->CPF,
      "email"=> $UserData->Email,
      "phone"=> $Personal->Phone
    ]
];
$PicPayment = new PicPayPayment($referenceId);
$PicPayment->Value = $pac->Value;
$PicPayment->PackID = $pac->ID;
$PicPayment->ExpiresAt = $expiresAt;
$PicPayment->UserID = $Personal->UserID;
$data = $PicPayment->Create();






$ch = curl_init('https://appws.picpay.com/ecommerce/public/payments');
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($ch, CURLOPT_POSTFIELDS, http_build_query($dados));
curl_setopt($ch, CURLOPT_HTTPHEADER, ['x-picpay-token: ' . $xPicpayToken]);

$res = curl_exec($ch);
curl_close($ch);

$retorno = json_decode($res);


echo "ID da fatura: " . $retorno->referenceId . "";
echo "<a target=\"_blank\"><a href=\"" . $retorno->paymentUrl . "\" class=\"btn btn-success button\" type=\"button\">Fatura</button></a>";
}
else
{
    echo Alert("Pacote Inexistenete");
}
qc();
?></center>

    

    </footer>
    <div class="copyright">
        <div class="container">
            <div class="row">
                <div class="col-xl-6 col-lg-6 col-md-6">
                    <div class="copy-text">
                        <p>DDtank </p>
                    </div>
                </div>
                <div class="col-xl-6 col-lg-6 col-md-6">
                    <div class="copy-nav">
                        <ul>
                            <li><a href="index.php">Sistema de recarga</a></li>
                            <li><a href="index.php">DDtank ( Name )</a></li>

                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>

