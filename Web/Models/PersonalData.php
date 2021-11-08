<?php
class PersonalData 
{
    public $UserID;
    public $CPF;
    public $FirstName;
    public $LastName;
    public $Phone ;

    public function __construct($UserID)
    {
        $this->UserID = $UserID;
    }

    public function SetPhone($int)
    {
        // Allow +, - and . in phone number
     $filtered_phone_number = filter_var($int, FILTER_SANITIZE_NUMBER_INT);
     $phone_to_check = str_replace("-", "", $filtered_phone_number);
     if (strlen($phone_to_check) < 14 || strlen($phone_to_check) > 14) {
        return false;
     } 
     else {
        $this->Phone = $int;
       return true;
      }
    }

    public function SetCPF($int)
    {
        //957.650.380-93
        // Allow +, - and . in phone number
     $filtered_phone_number = filter_var($int, FILTER_SANITIZE_NUMBER_INT);
     if (strlen($filtered_phone_number) == 14) {
        return false;
     } 
     else {
        $this->CPF = $int;
       return true;
      }
    }


    public function Update()
    {
        global $conn;
        if(!isset($this->UserID) || !isset($this->CPF) || !isset($this->FirstName) || !isset($this->LastName) || !isset($this->Phone))
        {
            return false;
        }

        if(isset($_SESSION['UserData']))
        {

            try
            {
                 $str = "update PersonalData set Phone = '{$this->Phone}' where UserID = {$this->UserID}";
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
        else
        {
            return false;
        }
    }

    public function Get()
    {
        if(!isset($this->UserID))
        {
            return false;
        }

        if(isset($_SESSION['UserData']))
        {
            global $conn;
            $data = unserialize($_SESSION['UserData']);
            $data->GameData();
            if(!$data->isUserIDValid() && $data->GameID == $this->UserID)
            {   try
                {
                  $str = "select * from PersonalData where UserID = {$this->UserID}";
                  $smtp = sqlsrv_query($conn,$str,array(),array("Scrollable"=>SQLSRV_CURSOR_KEYSET));
                  $data = sqlsrv_fetch_array($smtp,SQLSRV_FETCH_ASSOC);
                  if(isset($data["FirstName"]))
                  {
                  $this->FirstName = $data["FirstName"];
                  $this->LastName = $data["LastName"];
                  $this->CPF = $data["CPF"];
                  $this->Phone = $data["Phone"];
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
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public function  Exist()
    {
        if(!isset($this->UserID))
        {
            return false;
        }

        if(isset($_SESSION['UserData']))
        {
            global $conn;
            $data = unserialize($_SESSION['UserData']);
            $data->GameData();
            if(!$data->isUserIDValid() && $data->GameID == $this->UserID)
            {   try
                {
                $str = "select * from PersonalData where UserID = {$this->UserID}";
                $smtp = sqlsrv_query($conn,$str,array(),array("Scrollable"=>SQLSRV_CURSOR_KEYSET));
                $q = @sqlsrv_num_rows($smtp);
                if($q>0)
                {
                    return TRUE;
                }
                else
                {
                    return FALSE;
                }
                }
                catch(Exception $ex)
                {
                    return false;
                } 
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public function Create()
    {
        global $conn;
        if(!isset($this->UserID) || !isset($this->CPF) || !isset($this->FirstName) || !isset($this->LastName) || !isset($this->Phone))
        {
            return false;
        }

        if(isset($_SESSION['UserData']))
        {

               $data = unserialize($_SESSION['UserData']);
               $data->GameData();
               if(!$data->isUserIDValid() && $data->GameID == $this->UserID)
               {
                   try
                   {
                        $str = "insert into PersonalData values({$this->UserID},'{$this->CPF}','{$this->FirstName}','{$this->LastName}','{$this->Phone}')";
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
               else
               {
                   return false;
               }
            
        }
        else
        {
            return false;
        }
    }
}
?>