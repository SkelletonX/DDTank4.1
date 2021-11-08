<?php
class Pacote
{
    Public $Value;
    Public $Coupons;
    Public $Link;
    Public $ID;

    public function __construct($Data)
    {
        if(isset($Data))
        {
        $this->Value = $Data['Value'];
        $this->Coupons = $Data['Coupons'];
        $this->Link = $Data['Link'];
        $this->ID = $Data['ID'];
        }
    }

    public function Get()
    {
        if(!isset($this->ID))
        {
            return false;
        }

        global $conn;
        try
        {
          $str = "select * from Pacotes where ID = {$this->ID}";
          $smtp = sqlsrv_query($conn,$str,array(),array("Scrollable"=>SQLSRV_CURSOR_KEYSET));
          $data = sqlsrv_fetch_array($smtp,SQLSRV_FETCH_ASSOC);
          if(isset($data["ID"]))
          {
          $this->Value = (float)$data["Value"];
          $this->Coupons = $data["Coupons"];
          $this->Link = $data["Link"];
          }
          else
          {
              return false;
          }
        }
        catch(Exception $ex)
        {
            return false;
        } 
        return true;
    }

     public function GetHTML()
    {
        if(!isset($this->ID) || !isset($this->Coupons) || !isset($this->Link) || !isset($this->Value))
        {
            return false;
        }

        else
        {
            return '
            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6">
                    <div class="single-jackpot">
                        <div class="jackpot-thumb">
                            <img src="/assets/recarga/icon.png" alt=""> 
							<a>'.$this->Value.' BRL = '.$this->Coupons.' Cupons</a>
                            <p></p>

							<a href="picpay.php?ID='.$this->ID.'">
							<img src="https://www.windowsteam.com.br/wp-content/uploads/2020/05/picpay-logo-1.png" alt="PicPay" width="75" height="35">
							</a>

                        </div>
                    </div>
                </div>';
        }

        // <script src="https://www.mercadopago.com.br/integrations/v1/web-payment-checkout.js"
        // data-preference-id="'.$this->Link.'">
        // </script> ou
    }


    
}
?>