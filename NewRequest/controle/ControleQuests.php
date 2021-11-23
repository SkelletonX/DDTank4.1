<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ControleQuests
 *
 * @author jvbor
 */
class ControleQuests {

    //put your code here
    private $conexao, $db;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao(dbnameall);
        $this->db = new Db($this->conexao);
    }

    public function GetALlQuest() {
        $lista = array();
        $query = $this->db->q('exec SP_Quest_All');
        while ($result = $query->fetch()) {
            $obj = new QuestInfo();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $date = date_create($obj->getStartDate());
            $dateFormated = date_format($date, 'Y-m-d\TH:i:s');
            $obj->setStartDate($dateFormated);

            $date = date_create($obj->getEndDate());
            $dateFormated = date_format($date, 'Y-m-d\TH:i:s');
            $obj->setEndDate($dateFormated);
            $obj->setCanRepeat($obj->getCanRepeat() == '1' ? 'true' : 'false');
            $obj->setTimeMode($obj->getTimeMode() == '1' ? 'true' : 'false');
            $obj->setAutoEquip($obj->getAutoEquip() == '1' ? 'true' : 'false');
            $lista[] = $obj;
        }
        return $lista;
    }

    public function GetAllQuestGoods($questId) {
        $lista = array();
        $query = $this->db->q('select * from Quest_Goods where QuestID = ?', array($questId));
        while ($result = $query->fetch()) {
            $obj = new QuestAwardInfo();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $lista[] = $obj;
        }
        return $lista;
    }

    public function GetAllQuestCondiction($questId) {
        $lista = array();
        $query = $this->db->q('select * from Quest_Condiction where QuestID = ?', array($questId));
        while ($result = $query->fetch()) {
            $obj = new QuestCondictionInfo();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $obj->setIsOpitional($obj->getIsOpitional() == '1' ? 'true' : 'false');
            $lista[] = $obj;
        }
        return $lista;
    }

    public function GetAllQuestRate() {
        $lista = array();
        $query = $this->db->q('exec SP_Quest_Rate_All');
        while ($result = $query->fetch()) {
            $obj = new QuestRateInfo();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $lista[] = $obj;
        }
        return $lista;
    }

}
