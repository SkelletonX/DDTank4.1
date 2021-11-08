<?php
include('../global.php');
$PicpayToken = '1bbc069d-5a4b-45f6-85fb-ce40416fd998';

$payload = json_decode(file_get_contents('php://input'), true);

$data = new PicPayPayment($payload['referenceId']);
$valid = $data->Get();

$decoded = GetPicpayData($payload['referenceId'],$PicpayToken);
$linha = date('d/m/Y H:i:s') . ' - ' . json_encode($decoded) . "\n";
// armazena a resposta
file_put_contents('notificacoes.txt', $linha, FILE_APPEND);
$data->Status = $decoded['status'];
$data->AuthId = $decoded['authorizationId'];
if(!$valid)
{
    $data->UserID = -1;
    $data->Value = -1;
    $data->ExpiresAt = date(DATE_ATOM, strtotime(' - 5 days'));
    $data->Create();
}
else
{
    $data->Update();
    if($data->Status == "paid")
    {
         
        if($data->PackID != null && $data->UserID != -1)
        {
         $pacote = new Pacote(null);
         $pacote->ID = $data->PackID;
         $pacote->Get();

         $mb = new MemberData(null,null);
         $mb->GetByID($data->UserID);

        Recharge($data->ReferenceId,$pacote->Coupons,$mb->NickName,$mb->UserName,$data->UserID,$pacote->Value);
        }
    }
}
?>