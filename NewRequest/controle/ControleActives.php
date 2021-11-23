<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ControleActives
 *
 * @author jvbor
 */
class ControleActives {

    //put your code here
    private $conexao, $db;
    private $conexao2, $db2;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao(dbnameall);
        $this->db = new Db($this->conexao);
        $this->conexao2 = GerenciadorConexao::getConexao();
        $this->db2 = new Db($this->conexao2);
    }

    public function PullDown($activeID, $awardID, $userID) {
        $result = 0;
        $q = $this->db2->q("exec SP_Active_PullDown ?,?,?", array($activeID, $awardID, $userID));
        do {
            try {
                $result += $q->fetch()[0];
            } catch (Exception $ex) {

            }
        } while ($q->nextRowset());
        switch ($result) {
            case 0:
                $msg = "Coleta de recompensas sucedida. As recompensas foram mandados para a sua caixa de correio.";
                break;
            case 1:
                $msg = "Erro desconhecido.";
                break;
            case 2:
                $msg = "O usuário não existe.";
                break;
            case 3:
                $msg = "A Coleta de item falhou.";
                break;
            case 4:
                $msg = "O número digitado não existe. Verifique se o número está certo.";
                break;
            case 5:
                $msg = "Este número já retirou o prêmio, você não pode usar o mesmo número duas vezes.";
                break;
            case 6:
                $msg = "Você já retirou o prêmio da atividade.";
                break;
            case 7:
                $msg = "A atividade ainda não começou.";
                break;
            case 8:
                $msg = "A atividade já expirou.";
                break;
            default:
                $msg = "A coleta falhou.";
                break;
        }
        return array($result, $msg);
    }

    /**
     *
     * @return ActiveInfo[]
     */
    public function GetAllActives() {
        $lista = array();
        $query = $this->db->q('exec SP_Active_All');
        while ($result = $query->fetch()) {
            $obj = new ActiveInfo();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $obj->setIsOnly($result['IsOnly']);
            $obj->setHasKey($obj->getHasKey() == 4 ? 1 : $obj->getHasKey());
            $lista[] = $obj;
        }
        return $lista;
    }

    /**
     *
     * @return ActiveConvertItemInfo[]
     */
    public function GetAllActiveConvertItems() {
        $lista = array();
        $query = $this->db->q('exec SP_Active_Convert_Item_All');
        while ($result = $query->fetch()) {
            $obj = new ActiveConvertItemInfo();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $lista[] = $obj;
        }
        return $lista;
    }

    /**
     *
     * @return\ActivitySystemItemInfo[]
     */
    public function GetAllActivitySystemItem() {
        $query = $this->db->q('exec SP_ActivitySystemItem_All');
        $list = array();
        while ($resultado = $query->fetch()) {
            $obj = new ActivitySystemItemInfo();
            Utilitarios::autoInitObjectFromResult($obj, $resultado);
            $list[] = $obj;
        }
        return $list;
    }

    /**
     *
     * @return\DevilTreasPointList[]
     */
    public function GetAllDevilTreasPointsList()
    {
        $DevilReward = array();
        $query = $this->db->q('SELECT * FROM DevilTreasPointsList');
        while($result = $query->fetch())
        {
            $DevilTreasPointsList = new DevilTreasPointsList();
            Utilitarios::autoInitObjectFromResult($DevilTreasPointsList, $result);
            $DevilReward[] = $DevilTreasPointsList;
        }
        return $DevilReward;
    }

    public function GetAllDevilTreasSarahToBoxList()
    {
        $SarahBox = array();
        $query = $this->db->q('SELECT * FROM DevilTreasSarahToBoxList');
        while($result = $query->fetch())
        {
            $DevilTreasSarahToBoxList = new DevilTreasSarahToBoxList();
            Utilitarios::autoInitObjectFromResult($DevilTreasSarahToBoxList, $result);
            $SarahBox[] = $DevilTreasSarahToBoxList;
        }
        return $SarahBox;
    }

    public function GetAllDevilTreasItemList()
    {
        $DevilItemList = array();
        $query = $this->db->q('SELECT * FROM DevilTreasItemList');
        while($result = $query->fetch())
        {
            $DevilTreasItemList = new DevilTreasItemList();
            Utilitarios::autoInitObjectFromResult($DevilTreasItemList, $result);
            $DevilItemList[] = $DevilTreasItemList;
        }
        return $DevilItemList;
    }

}
