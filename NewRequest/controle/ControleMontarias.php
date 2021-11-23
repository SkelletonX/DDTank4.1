<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ControlePets
 *
 * @author jvbor
 */
class ControleMontarias {

    //put your code here
    private $conexao, $db;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao(dbnameall);
        $this->db = new Db($this->conexao);
    }

    /**
     * @return MountDrawTemplate[]
     * @throws Exception
     */
    public function GetAllMountDraw() {
        $lista = array();
        $query = $this->db->q('exec SP_Mount_Draw_Template_All');
        while ($result = $query->fetch()) {
            $obj = new MountDrawTemplate();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $lista[] = $obj;
        }
        return $lista;
    }

}
