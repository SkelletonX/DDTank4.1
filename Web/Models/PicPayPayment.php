<?php
class PicPayPayment
{
    Public $AuthId;
    Public $ReferenceId;
    Public $Status;
    Public $Value;
    Public $ExpiresAt;
    Public $UserID;
    Public $PackID;

    public function __construct($ReferenceId)
    {
        $this->ReferenceId = $ReferenceId;
    }

    public function Get()
    {
     global $conn;
     $str = "select * from PicPayPayment where referenceId = '{$this->ReferenceId}'";
     try
     {
       $smtp = sqlsrv_query($conn,$str,array(),array("Scrollable"=>SQLSRV_CURSOR_KEYSET));
       $data = sqlsrv_fetch_array($smtp,SQLSRV_FETCH_ASSOC);
       if(isset($data["referenceId"]))
       {
         $this->AuthId = $data["AuthID"];
         $this->Status = $data["status"];
         $this->UserID = $data["UserID"];
         $this->Value = (float)$data["Value"];
         $this->expiresAt = $data["expiresAt"];
         $this->PackID = $data["PacID"];
       }
       else
       {
           return false;
       }
       return true;
     }
     catch(Exception $ex)   
     {
         return false;
     }

     return true;
         
    }

    public function Create()
    {
        global $conn;
        if(!isset($this->ReferenceId) ||!isset($this->ExpiresAt) || !isset($this->UserID))
        {
            return false;
        }

        if($this->Get())
        {
            return false;
        }
        else
        {
            try
            {
                 $str = "insert into PicPayPayment values('{$this->AuthId}','{$this->ReferenceId}','{$this->Status}',{$this->UserID},{$this->Value},Convert(varchar(33),'{$this->ExpiresAt}',126),{$this->PackID});";
                 $p = sqlsrv_prepare($conn,$str);

                 if(!sqlsrv_execute($p))
                 {
                     throw new Exception;
                 }
            }
            catch(Exception $ex)
            {
            return false;
            }
            return true;
        }
    }

    public function Update()
    {
        
        if(!isset($this->ReferenceId))
        {
            return false;
        }
        global $conn;
         try
         {
              $str = "update PicPayPayment set AuthID = '{$this->AuthId}',status='{$this->Status}' where referenceId = '{$this->ReferenceId}'";
              $p = sqlsrv_prepare($conn,$str);
              if(!sqlsrv_execute($p))
              {
                  throw new Exception;
              }
         }
         catch(Exception $ex)
         {
         return false;
         }
         return true;

    }


}
?>