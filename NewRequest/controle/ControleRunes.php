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
class ControleRunes {

    //put your code here
    private $conexao, $db;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao(dbnameall);
        $this->db = new Db($this->conexao);
    }

    public function GetAllRunesTemplates() {
        $lista = array();
        $query = $this->db->q('exec SP_RuneTemplate_All');
        while ($result = $query->fetch()) {
            $obj = new RuneTemplate();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $lista[] = $obj;
        }
        return $lista;
    }

    public function GetAllRunesAdvanceTemplates() {
        $lista = array();
        $query = $this->db->q('exec SP_Rune_Advance_Template_All');
        while ($result = $query->fetch()) {
            $obj = new RuneAdvanceTemplate();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $lista[] = $obj;
        }
        return $lista;
    }

}
