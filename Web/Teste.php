<?php
include('global.php');
ini_set('display_errors', 1);

if(isset($_POST['data']))
{
$rd = rand(0,32768).rand(0,32768).uniqid();


if(isset($_SESSION['UserData']))
{
$userdata = unserialize($_SESSION['UserData']);

if($userdata->GameID == -1 || isset($userdata->GameID))
{
	$userdata->GameData();
}
echo $userdata->GameID;


Recharge($rd,$_POST['Quantidade'],$userdata->UserName,$userdata->UserName);
 $options = array(
 		'soap_version'=> SOAP_1_1, 
		'exceptions'=> true, 
 		'trace'=> 1,
 		'cache_wsdl'=> 0,
 	);
 $client = new SoapClient('http://127.0.0.1:2008/CenterService/?Wsdl', $options);
 if($client)
 {
 $obj = array('userID' => $userdata->GameID ,"chargeID"=>$rd);
 $client->ChargeMoney($obj);
 }
 
}


}

?>
<!DOCTYPE html>
<html lang="en">
<head>
	<meta charset="UTF-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0">
	
</head>
<body>
	<form method="POST">
		<input name="Quantidade">
		<button name="data" type="submit">
	</form>
	<a>teste</a>
</body>
</html>



