<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ControleConsortias
 *
 * @author jvbor
 */
class ControleConsortias {

    //put your code here
    private $conexao, $db;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao();
        $this->db = new Db($this->conexao);
    }

    public function GetConsortiaDutyPage($page, $size, $order, $consortiaID, $dutyID) {
        $queryWhere = " IsExist=1 ";
        if ($consortiaID != -1)
            $queryWhere = $queryWhere . " and ConsortiaID =" . $consortiaID . " ";
        if ($dutyID != -1)
            $queryWhere = $queryWhere . " and DutyID =" . $dutyID . " ";
        $str = "Level";
        if ($order == 1)
            $str = "DutyName";
        $fdOreder = $str . ",DutyID ";
        $listaConsortias = array();
        $query = $this->db->q("select top {$size} * from Consortia_Duty where {$queryWhere} order by {$fdOreder}");
        while ($resultado = $query->fetch()) {
            $consortia = new ConsortiaDutyInfo();
            Utilitarios::autoInitObjectFromResult($consortia, $resultado);
            $listaConsortias[] = $consortia;
        }
        return $listaConsortias;
    }

    public function GetConsortiaSingleByName($name) {
        $query = $this->db->q("exec SP_Consortia_CheckByName ?", array($name));
        if ($result = $query->fetch()) {
            $consortia = new ConsortiaInfo();
            Utilitarios::autoInitObjectFromResult($consortia, $result);
            return $consortia;
        } else {
            return null;
        }
    }

    public function updateConsortiaName($ConsortiaName)
    {
        $query = $this->db->q("UPDATE Consortia SET ConsortiaName = '". $ConsortiaName ."' WHERE ChairmanID = '". $SelfID ."'");
        //SoapConnection
    }

    public function GetConsortiaApplyUserPage($page, $size, $order, $consortiaID, $applyID, $userID) {
        $queryWhere = " IsExist=1 ";
        if ($consortiaID != -1)
            $queryWhere = $queryWhere . " and ConsortiaID =" . $consortiaID . " ";
        if ($applyID != -1)
            $queryWhere = $queryWhere . " and ID =" . $applyID . " ";
        if ($userID != -1)
            $queryWhere = $queryWhere . " and UserID ='" . $userID . "' ";
        $fdOreder = "ID";
        switch ($order) {
            case 1:
                $fdOreder = "UserName,ID";
                break;
            case 2:
                $fdOreder = "ApplyDate,ID";
                break;
        }
        $listaConsortias = array();
        $query = $this->db->q("select top {$size} * from V_Consortia_Apply_Users where {$queryWhere} order by {$fdOreder}");
        while ($resultado = $query->fetch()) {
            $consortia = new ConsortiaApplyUserInfo();
            Utilitarios::autoInitObjectFromResult($consortia, $resultado);
            $listaConsortias[] = $consortia;
        }
        return $listaConsortias;
    }

    public function GetConsortiaEquipControlPage($consortiaID, $level, $type) {
        $queryWhere = " IsExist=1 ";
        if ($consortiaID != -1)
            $queryWhere = $queryWhere . " and ConsortiaID =" . $consortiaID . " ";
        if ($level != -1)
            $queryWhere = $queryWhere . " and Level =" . $level . " ";
        if ($type != -1)
            $queryWhere = $queryWhere . " and Type =" . $type . " ";
        $fdOreder = "ConsortiaID ";
        $listaConsortias = array();
        $query = $this->db->q("select top 10 * from Consortia_Equip_Control where {$queryWhere} order by {$fdOreder}");
        while ($resultado = $query->fetch()) {
            $consortia = new ConsortiaEquipControlInfo();
            Utilitarios::autoInitObjectFromResult($consortia, $resultado);
            $listaConsortias[] = $consortia;
        }
        return $listaConsortias;
    }

    public function GetConsortiaUsersPage($size, $order, $consortiaID, $userID, $state) {
        $queryWhere = " IsExist=1 ";
        if ($consortiaID != -1)
            $queryWhere = $queryWhere . " and ConsortiaID =" . $consortiaID . " ";
        if ($userID != -1)
            $queryWhere = $queryWhere . " and UserID =" . $userID . " ";
        if ($state != -1)
            $queryWhere = $queryWhere . " and state =" . $state . " ";
        $str = "UserName";
        switch ($order) {
            case 1:
                $str = "DutyID";
                break;
            case 2:
                $str = "Grade";
                break;
            case 3:
                $str = "Repute";
                break;
            case 4:
                $str = "GP";
                break;
            case 5:
                $str = "State";
                break;
            case 6:
                $str = "Offer";
                break;
        }
        $fdOreder = $str . ",ID ";
        $query = $this->db->q("select top {$size} * from V_Consortia_Users where {$queryWhere} order by {$fdOreder}");
        $listaConsortias = array();
        while ($resultado = $query->fetch()) {
            $consortia = new ConsortiaUserInfo();
            Utilitarios::autoInitObjectFromResult($consortia, $resultado);
            $consortia->setTypeVIP(0);
            $consortia->setVIPLevel($resultado['VIPLevel']);
            $listaConsortias[] = $consortia;
        }
        return $listaConsortias;
    }

    public function GetConsortiaPage($size, $order, $name, $consortiaID, $level, $openApply) {
        $queryWhere = " IsExist=1 ";
        if ($name != null && $name != '') {
            $queryWhere = $queryWhere . " and ConsortiaName like '%" . $name . "%' ";
        }
        if ($consortiaID != NULL && $consortiaID != -1) {
            $queryWhere = $queryWhere . " and ConsortiaID =" . $consortiaID . " ";
        }
        if ($level != NULL && $level != -1) {
            $queryWhere = $queryWhere . " and Level =" . $level . " ";
        }
        if ($openApply != NULL && $openApply != -1) {
            $queryWhere = $queryWhere . " and OpenApply =" . $openApply . " ";
        }
        $str = "ConsortiaName";
        switch ($order) {
            case 1:
                $str = "ReputeSort";
                break;
            case 2:
                $str = "ChairmanName";
                break;
            case 3:
                $str = "Count desc";
                break;
            case 4:
                $str = "Level desc";
                break;
            case 5:
                $str = "Honor desc";
                break;
            case 10:
                $str = "LastDayRiches desc";
                break;
            case 11:
                $str = "AddDayRiches desc";
                break;
            case 12:
                $str = "AddWeekRiches desc";
                break;
            case 13:
                $str = "LastDayHonor desc";
                break;
            case 14:
                $str = "AddDayHonor desc";
                break;
            case 15:
                $str = "AddWeekHonor desc";
                break;
            case 16:
                $str = "level desc,LastDayRiches desc";
                break;
        }
        $fdOreder = $str . ",ConsortiaID ";
        $query = $this->db->q("select top {$size} * from V_Consortia where {$queryWhere} order by {$fdOreder}");
        $listaConsortias = array();
        while ($resultado = $query->fetch()) {
            $consortia = new ConsortiaInfo();
            Utilitarios::autoInitObjectFromResult($consortia, $resultado);
            $consortia->setBufferLevel($resultado['SkillLevel']);
            $listaConsortias[] = $consortia;
        }
        return $listaConsortias;
    }

    public function UpdateConsortiaFightPower() {
        $query = $this->db->q("SP_Consortia_Update_FightPower");
        $listaConsortias = array();
        $query->nextRowset();
        while ($resultado = $query->fetch()) {
            $consortia = new ConsortiaInfo();
            Utilitarios::autoInitObjectFromResult($consortia, $resultado);
            $consortia->setBufferLevel($resultado['SkillLevel']);
            $listaConsortias[] = $consortia;
        }
        return $listaConsortias;
    }

}
