<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ControleTitulos
 *
 * @author jvbor
 */
class ControleTitulos {

    //put your code here
    private $conexao, $db;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao(dbnameall);
        $this->db = new Db($this->conexao);
    }

    public function GetNewTitle() {
        $lista = array();
        $query = $this->db->q('exec SP_NewTitle_All1');
        while ($result = $query->fetch()) {
            $obj = new NewTitleInfo();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $obj->setShow($obj->getShow() == '1' ? 'true' : 'false');
            $lista[] = $obj;
        }
        return $lista;
    }

}
