<?php
class MemberData{
    Public $AppID;
    Public $UserID;
    Public $UserName;
    Public $LowerName;
    Public $MobileAlias;
    Public $IsAnonymous;
    Public $LastActivityDate;
    Public $LoginDate;
    Public $Point;
    Public $Password;
    Public $PassEncrypted;
    Public $Email;
    Public $CreateDate;
    Public $GameID;
    
    function __construct($UserName,$Password) {
        $this->UserName = $UserName;
        $this->LowerName = strtolower($UserName);
        $this->Password = $Password;
        $this->PassEncrypted = strtoupper(md5($Password));
        $this->GameID = -1;
    }


    Public function isAuthenticated()
    {
        global $conn;
        $applicationName = 'DanDanTang';
        $id=0;
        $data = array(
            array($applicationName, SQLSRV_PARAM_IN),
            array($this->UserName, SQLSRV_PARAM_IN),
            array($this->PassEncrypted, SQLSRV_PARAM_IN),
            array(&$id, SQLSRV_PARAM_OUT)
        );
        $retun = sqlsrv_prepare($conn,"{call Mem_Users_Accede (?,?,?,?)}",$data);
        if( $retun ) {
       }else{
            echo "Algum Erro Ocorreu por favor contate um Administrador.<br />";
            die( print_r( sqlsrv_errors(), true));
       }
        if(!sqlsrv_execute($retun))
        {
            die( print_r( sqlsrv_errors(), true));
        }
        sqlsrv_next_result($retun);
        $this->UserID = $id;
        if($this->UserID >= 0 && isset($id))
        {
            return true;
        }
        else
        {
            return false;
        }
        return false;
    }

    Public function GameData()
    {
        global $conn;
        $str = "select * from Webshop_Account where UserId = {$this->UserID}";

        $q = sqlsrv_query($conn,$str,array(),array("Scrollable"=>SQLSRV_CURSOR_KEYSET));
        $data = sqlsrv_fetch_array($q,SQLSRV_FETCH_ASSOC);
        $this->GameID = $data["ID"];
    }

}
?>