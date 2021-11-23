<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ControleServer
 *
 * @author jvbor
 */
class ControleServer {

    //put your code here
    //put your code here
    private $conexao, $db;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao(dbnameall);
        $this->db = new Db($this->conexao);
    }

    public function getColummsOfTable($table) {
        $query = $this->db->q('exec sp_columns ?', array($table));
        $nomes = array();
        while ($resultado = $query->fetch()) {
            $nome = $resultado['COLUMN_NAME'];
            $nomes[] = $nome;
        }
        return $nomes;
    }

    public function GetAllFusionList() {
        $query = $this->db->q('exec SP_Fusion_All');
        $listaItens = array();
        while ($resultado = $query->fetch()) {
            $item = new ItemFusion();
            Utilitarios::autoInitObjectFromResult($item, $resultado);
            $listaItens[] = $item;
        }
        return $listaItens;
    }
    
    public function GetAllPveInfo() {
        $query = $this->db->q('exec SP_PveInfos_All');
        $listaItens = array();
        while ($resultado = $query->fetch()) {
            $item = new PveInfo();
            Utilitarios::autoInitObjectFromResult($item, $resultado);
            $listaItens[] = $item;
        }
        return $listaItens;
    }

    public function GetAllBallConfig() {
        $query = $this->db->q('exec SP_Ball_Config_All');
        $listaItens = array();
        while ($resultado = $query->fetch()) {
            $item = new BallConfigInfo();
            Utilitarios::autoInitObjectFromResult($item, $resultado);
            $listaItens[] = $item;
        }
        return $listaItens;
    }

    public function GetAllBall() {
        $query = $this->db->q('exec SP_Ball_All');
        $listaItens = array();
        while ($resultado = $query->fetch()) {
            $item = new BallInfo();
            Utilitarios::autoInitObjectFromResult($item, $resultado);
            $item->setShake($item->getShake() == "1" ? "true":"false");
            $listaItens[] = $item;
        }
        return $listaItens;
    }

    public function GetAllGoods() {
        $query = $this->db->q('exec SP_Items_All');
        $listaItens = array();
        while ($resultado = $query->fetch()) {
            $item = new ItemTemplateInfo();
            Utilitarios::autoInitObjectFromResult($item, $resultado);
            $listaItens[] = $item;
        }
        return $listaItens;
    }

    public function getMapServer() {
        $query = $this->db->q('select * from dbo.map_server');
        $listaMaps = array();
        while ($resultado = $query->fetch()) {
            $map = new MapServer();
            Utilitarios::autoInitObjectFromResult($map, $resultado);
            $listaMaps[] = $map;
        }
        return $listaMaps;
    }

    public function getMapList() {
        $query = $this->db->q('exec dbo.SP_Maps_All');
        $listaMaps = array();
        while ($resultado = $query->fetch()) {
            $map = new MapInfo();
            Utilitarios::autoInitObjectFromResult($map, $resultado);
            $listaMaps[] = $map;
        }
        return $listaMaps;
    }

    public function getServerConfig() {
        $query = $this->db->q('select * from dbo.server_config');
        $listaConfigs = array();
        while ($resultado = $query->fetch()) {
            $listaConfigs[] = $resultado;
        }
        return $listaConfigs;
    }

    /**
     * @return Server[]
     */
    public function getServerList() {
        $soap = new SoapClient('http://' . endPointCenter . '/CenterService/?wsdl');
        try {
            $result = $soap->GetServerList(array('zoneId'=>areaid));
            $serversList = array();
            foreach ($result->GetServerListResult->ServerData as $value) {
                $serve = new Server();
                $serve->setID($value->Id);
                $serve->setIP($value->Ip);
                $serve->setLowestLevel($value->LowestLevel);
                $serve->setMustLevel($value->MustLevel);
                $serve->setName($value->Name);
                $serve->setOnline($value->Online);
                $serve->setPort($value->Port);
                $serve->setRemark($value->Remark);
                $serve->setState($value->State);
                $serversList[] = $serve;
            }
            return $serversList;
        } catch (SoapFault $e) {
            echo "Error: {$e->faultstring}";
        }
    }

    public function MailNotice($userid) {
        $soap = new SoapClient('http://' . endPointCenter . '/CenterService/?wsdl');
        try {
            $result = $soap->MailNotice(array('playerID' => $userid,'zoneId'=>areaid));
            return $result;
        } catch (SoapFault $e) {
            echo "Error: {$e->faultstring}";
        }
    }

    public function createPlayer($id, $username, $password) {
        $soap = new SoapClient('http://' . endPointCenter . '/CenterService/?wsdl');
        try {
            $result = $soap->CreatePlayer(array('id' => $id, 'name' => $username, 'password' => $password, 'isFirst' => false,'zoneId'=>areaid));
            return $result->CreatePlayerResult == 1;
        } catch (SoapFault $e) {
            echo "Error: {$e->faultstring}";
        }
    }

    public function GetAllSubActive() {
        $query = $this->db->q('exec SP_SubActive_All');
        $listaSubActives = array();
        while ($resultado = $query->fetch()) {
            $subActive = new SubActiveInfo();
            $subActive->setActiveID($resultado['ActiveID']);
            $subActive->setActiveInfo($resultado['ActiveInfo']);
            $date = date_create($resultado['EndDate']);
            $dateFormated = date_format($date, 'Y-m-d H:i:s');
            $subActive->setEndDate($dateFormated);
            $date = date_create($resultado['EndTime']);
            $dateFormated = date_format($date, 'Y-m-d H:i:s');
            $subActive->setEndTime($dateFormated);
            $subActive->setID($resultado['ID']);
            $subActive->setIsContinued($resultado['IsContinued'] == '1' ? 'true' : 'false');
            $subActive->setIsOpen($resultado['IsOpen'] == '1' ? 'true' : 'false');
            $date = date_create($resultado['StartDate']);
            $dateFormated = date_format($date, 'Y-m-d H:i:s');
            $subActive->setStartDate($dateFormated);
            $date = date_create($resultado['StartTime']);
            $dateFormated = date_format($date, 'Y-m-d H:i:s');
            $subActive->setStartTime($dateFormated);
            $subActive->setSubID($resultado['SubID']);
            $listaSubActives[] = $subActive;
        }
        return $listaSubActives;
    }

    public function GetAllSubActiveCondition($id) {
        $query = $this->db->q('exec SP_SubActiveCondition_All ?', array($id));
        $listaSubActiveConditions = array();
        while ($resultado = $query->fetch()) {
            $condition = new SubActiveConditionInfo();
            $condition->setActiveID($resultado['ActiveID']);
            $condition->setAwardType($resultado['AwardType']);
            $condition->setAwardValue($resultado['AwardValue']);
            $condition->setID($resultado['ID']);
            $condition->setIsValid($resultado['IsValid'] == '1' ? 'true' : 'false');
            $condition->setSubID($resultado['SubID']);
            $condition->setValue($resultado['Value']);
            $condition->setType($resultado['Type']);
            $condition->setConditionID($resultado['ConditionID']);
            $listaSubActiveConditions[] = $condition;
        }
        return $listaSubActiveConditions;
    }

}
