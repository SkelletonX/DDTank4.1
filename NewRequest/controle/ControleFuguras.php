<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ControleItens
 *
 * @author jvbor
 */
class ControleFuguras {

    //put your code here
    private $conexao, $db;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao(dbnameall);
        $this->db = new Db($this->conexao);
    }

    public function GetAllClothGroup() {
        $query = $this->db->q('exec SP_ClothGroupTemplate_All');
        $lista = array();
        while ($resultado = $query->fetch()) {
            $obj = new ClothGroupTemplateInfo();
            Utilitarios::autoInitObjectFromResult($obj, $resultado);
            $lista[] = $obj;
        }
        return $lista;
    }

    public function GetAllClothProperty() {
        $query = $this->db->q('exec SP_ClothPropertyTemplate_All');
        $lista = array();
        while ($resultado = $query->fetch()) {
            $obj = new ClothPropertyTemplateInfo();
            Utilitarios::autoInitObjectFromResult($obj, $resultado);
            $lista[] = $obj;
        }
        return $lista;
    }

}
