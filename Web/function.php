<?php
function co()
{
	global $conn,$config,$c_host;
	$conn = sqlsrv_connect($c_host, $config);
	if(!$conn) die('Não é possível conectar o SQL Server, entre em contato com administrador.');
}

function Alert($data)
{
	return '<script>alert("'.$data.'")</script>';
}

function q($q)
{
	global $conn;
	return sqlsrv_query($conn,$q,array(),array("Scrollable"=>SQLSRV_CURSOR_KEYSET));
}
function qn($q)
{
	return @sqlsrv_num_rows($q);
}
function qa($q)
{
	return sqlsrv_fetch_array($q,SQLSRV_FETCH_ASSOC);
}
function qc()
{
	global $conn;
	sqlsrv_close($conn);
}
function loadimage($image,$loaivp,$sex)
{
	switch($sex)
	{
		case 1:
			$ml = 'm';
		break;
		case 2:
			$ml = 'f';
		break;
		default:
			$ml = 'f';
		break;
	}
	switch($loaivp)
	{
		case 1:
			$link = 'equip/'.$ml.'/head/'.$image.'/icon_1.png';
		break;
		case 2:
			$link = 'equip/'.$ml.'/glass/'.$image.'/icon_1.png';
		break;
		case 3:
			$link = 'equip/'.$ml.'/hair/'.$image.'/icon_1.png';
		break;
		case 5:
			$link = 'equip/'.$ml.'/cloth/'.$image.'/icon_1.png';
		break;
		case 6:
			$link = 'equip/'.$ml.'/face/'.$image.'/icon_1.png';
		break;
		case 7:
			$link = 'arm/'.$image.'/00.png';
		break;
		case 8:
			$link = 'equip/armlet/'.$image.'/icon.png';
		break;
		case 9:
			$link = 'equip/ring/'.$image.'/icon.png';
		break;
		case 11:
			$link = 'unfrightprop/'.$image.'/icon.png';
		break;
		case 13:
			$link = 'equip/'.$ml.'/suits/'.$image.'/icon_1.png';
		break;
		case 15:
			$link = 'equip/wing/'.$image.'/icon.png';
		break;
		case 14:
			$link = 'necklace/'.$image.'/icon.png';
		break;
		case 17;
			$link = 'equip/offhand/'.$image.'/icon.png';
		break;
		case 16;
			$link = 'specialprop/chatBall/'.$image.'/icon.png';
		break;
		case 19;
			$link = 'prop/'.$image.'/icon.png';
		break;
		case 20;
			$link = 'prop/'.$image.'/icon.png';
		break;
		case 35;
			$link = 'unfrightprop/'.$image.'/icon.png';
		break;
		case 34;
			$link = 'unfrightprop/'.$image.'/icon.png';
		break;
		case 50;
			$link = 'petequip/arm/'.$image.'/icon.png';
		break;
		case 52;
			$link = 'petequip/cloth/'.$image.'/icon.png';
		break;
		case 51;
			$link = 'petequip/hat/'.$image.'/icon.png';
		break;
		default:
			$link = NULL;
		break;
	}
	return $link;
}
function GetNameItem($id)
{
	$name = 'Không xác định';
	switch($id) {
	case 1:
	$name = 'Nón';
	break;
	case 2:
	$name = 'Tipo: Óculos';
	break;
	case 3:
	$name = 'Tóc';
	break;
	case 4:
	$name = 'Mặt';
	break;
	case 5:
	$name = 'Tipo: Roupa';
	break;
	case 6:
	$name = 'Mắt';
	break;
	case 7:
	$name = 'Vũ khí';
	break;
	case 8:
	$name = 'Vòng tay';
	break;
	case 9:
	$name = 'Nhẫn';
	break;
	case 13:
	$name = 'Tipo: Terno';
	break;
	case 15:
	$name = 'Cánh';
	break;
	case 17:
	$name = 'Vũ khí phụ';
	break;
	case 14:
	$name = 'Dây truyền';
	break;
	case 16:
	$name = 'Bong bóng';
	break;
	case 18:
	$name = 'Hộp thẻ';
	break;
	case 26:
	$name = 'Thẻ bài';
	break;
	case 25:
	$name = 'Quà tặng';
	break;
	case 12:
	$name = 'Item nhiệm vụ';
	break;
	case 11:
	$name = 'Tipo: Itens de auxílio';
	break;
	case 19:
	$name = 'Hỗ trợ';
	break;
	case 20:
	$name = 'Nước tu luyện';
	break;
	case 23:
	$name = 'Sách tu luyện';
	break;
	case 25:
	$name = 'Quà tặng';
	break;
	case 27:
	$name = 'Vũ khí đặc biệt';
	break;
	case 30:
	$name = 'Đạo cụ đặc biệt';
	break;
	case 31:
	$name = 'Vũ khí phụ đặc biệt';
	break;
	case 32:
	$name = 'Hạt giống';
	break;
	case 33:
	$name = 'Phân hóa học';
	break;
	case 34:
	$name = 'Tipo: Alimento';
	break;
	case 35:
	$name = 'Trứng thú cưng';
	break;
	case 36:
	$name = 'Cây trồng';
	break;
	case 52:
	$name = 'Giáp thú cưng';
	break;
	case 51:
	$name = 'Nón thú cưng';
	break;
	case 50:
	$name = 'Vũ khí thú cưng';
	break;
	case 40:
	$name = 'Huy hiệu';
	break;
	case 60:
	$name = 'Chiến hồn';
	break;
	}
	return $name;
}
function getQualityColor($id)
{
	$color = '000000';
	switch($id) {
		case 1:
			$color = 'FFFFFF';
		break;
		case 2:
			$color = '00FF00';
		break;
		case 3:
			$color = '0066FF';
		break;
		case 4:
			$color = 'FF00FF' ;
		break;
		case 5:
			$color = 'FF9900';
		break;
	}
	return $color;
}
function getQualityName($id)
{
	$name = 'Không xác định';
	switch($id) {
		case 1:
			$name = 'Thô';
		break;
		case 2:
			$name = 'Thường';
		break;
		case 3:
			$name = "Qualidade: Bom";
		break;
		case 4:
			$name = "Qualidade: Exelente";
		break;
		case 5:
			$name = "Qualidade: Perfeita";
		break;
	}
	return $name;
}
function loadCoin($uid)
{
	global $conn;
	$q = q("Select TOP 1 Coin from Webshop_Account where UserId = '{$uid}'");
	$r = qa($q);
	return (int)$r['Coin'];
}
function getUserData($userinfo)
{
	global $conn;
	$q = q("Select * from Mem_UserInfo where UserId = '{$userinfo->UserID}'");
	$r = qa($q);
	$userinfo->Email = $r['Email'];
	$userinfo->CreateDate = $r['CreateDate'];
	$userinfo->LoginDate = $r['LastLoginDate'];
	$q = q("Select * from Mem_Users where UserId = '{$userinfo->UserID}'");
	$r = qa($q);
	
	$userinfo->LastActivityDate = $r['LastActivityDate'];
	$userinfo->Point = $r['Point'];
}
function Recharge($ChargeID,$Cupons,$Nickname,$UserName,$UserID,$NeedMoney)
{
    global $conn,$dbtank41,$WSDL;
	$hoje = date('d/m/Y');
	$str = "insert into {$dbtank41}.[dbo].[Charge_Money] values('{$ChargeID}','{$UserName}',{$Cupons},'{$hoje}',1,'PicPay',{$NeedMoney},null,'{$NickName}')";
	$data = sqlsrv_prepare($conn,$str);
	if(!$data)
	{
		die( print_r( sqlsrv_errors(), true));
	}
	$result = sqlsrv_execute($data);
	if(!$result)
	{
		die( print_r( sqlsrv_errors(), true));
    }
    $options = array(
        'soap_version'=> SOAP_1_1, 
       'exceptions'=> true, 
        'trace'=> 1,
        'cache_wsdl'=> 0,
    );
      $client = new SoapClient($WSDL, $options);
      if($client)
      {
      $obj = array('userID' => $UserID ,"chargeID"=>$chargeID);
      $client->ChargeMoney($obj);
      }
}
function PegarPacotes()
{
	global $conn;
	$str = "select * from Pacotes";
	$q = q($str);
	while ($row = sqlsrv_fetch_array($q,SQLSRV_FETCH_ASSOC))
	{
		$pkt = new Pacote(null);
		$pkt->ID = $row['ID'];
		$pkt->Get();
		echo $pkt->GetHTML();
	}

}
function GetPicpayData($Refid,$PicpayToken)
{
$url = 'https://appws.picpay.com/ecommerce/public/payments/'.$Refid.'/status';
$ch = curl_init();
curl_setopt($ch, CURLOPT_URL, $url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 0);
curl_setopt($ch, CURLOPT_HTTPHEADER, [
    'Content-Type: application/json',
    'X-Picpay-Token: ' . $PicpayToken
]);
$result = curl_exec($ch);
curl_close($ch);
return json_decode($result, true);
}
