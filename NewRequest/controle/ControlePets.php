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
class ControlePets {

    //put your code here
    private $conexao, $db;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao(dbnameall);
        $this->db = new Db($this->conexao);
    }

    public function GetAllPetForm() {
        $lista = array();
        $query = $this->db->q('exec SP_Pet_Form_Data_All');
        while ($result = $query->fetch()) {
            $obj = new PetFormData();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $lista[] = $obj;
        }
        return $lista;
    }

    public function GetAllPetSkillInfo() {
        $lista = array();
        $query = $this->db->q('exec SP_PetSkillInfo_All');
        while ($result = $query->fetch()) {
            $obj = new PetSkillInfo();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $lista[] = $obj;
        }
        return $lista;
    }

    public function GetAllPetTemplate() {
        $lista = array();
        $query = $this->db->q('exec SP_PetTemplateInfo_All');
        while ($result = $query->fetch()) {
            $obj = new PetTemplateInfo();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $lista[] = $obj;
        }
        return $lista;
    }

    public function GetAllPetSkillElementInfo()
    {
        $lista = array();
        $query = $this->db->q('EXECUTE SP_PetSkillElementInfo_All');
        while($result = $query->fetch())
        {
            $obj = new PetSkillElementInfo();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $lista[] = $obj;
        }
        return $lista;
    }

    public function GetAllPetSkillTemplateInfo()
    {
        $lista = array();
        $query = $this->db->q('EXECUTE SP_PetSkillTemplateInfo_All');
        while($result = $query->fetch())
        {
            $obj = new PetSkillTemplateInfo();
            Utilitarios::autoInitObjectFromResult($obj, $result);
            $lista[] = $obj;
        }
        return $lista;
    }

}
