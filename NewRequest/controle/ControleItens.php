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
class ControleItens {

    //put your code here
    private $conexao, $db;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao(dbnameall);
        $this->db = new Db($this->conexao);
    }

    public function GetAllShopGoodsShowList() {
        $query = $this->db->q('exec SP_ShopGoodsShowList_All');
        $lista = array();
        while ($resultado = $query->fetch()) {
            $obj = new ShopGoodsShowListInfo();
            Utilitarios::autoInitObjectFromResult($obj, $resultado);
            $lista[] = $obj;
        }
        return $lista;
    }

    public function GetALllShop() {
        $query = $this->db->q('exec SP_Shop_All');
        $lista = array();
        while ($resultado = $query->fetch()) {
            $obj = new ShopItemInfo();
            Utilitarios::autoInitObjectFromResult($obj, $resultado);
            $date = date_create($obj->getStartDate());
            $dateFormated = date_format($date, 'Y-m-d\TH:i:s');
            $obj->setStartDate($dateFormated);

            $date = date_create($obj->getEndDate());
            $dateFormated = date_format($date, 'Y-m-d\TH:i:s');
            $obj->setEndDate($dateFormated);

            $lista[] = $obj;
        }
        return $lista;
    }

    public function GetAllMagicStones() {
        $query = $this->db->q('exec SP_MagicStone_All');
        $listaItens = array();
        while ($resultado = $query->fetch()) {
            $item = new MagicStoneInfo();
            Utilitarios::autoInitObjectFromResult($item, $resultado);
            $listaItens[] = $item;
        }
        return $listaItens;
    }

    public function GetAllGoldTemplates() {
        $query = $this->db->q('exec SP_GoldEquipTemplateLoad_All');
        $listaItens = array();
        while ($resultado = $query->fetch()) {
            $item = new GoldEquipTemplateLoad();
            Utilitarios::autoInitObjectFromResult($item, $resultado);
            $listaItens[] = $item;
        }
        return $listaItens;
    }

    public function getItemById($id) {
        $query = $this->db->q('select * from dbo.shop_goods where templateid = ?', array($id));
        if ($result = $query->fetch()) {
            $item = new ItemTemplateInfo();
            Utilitarios::autoInitObjectFromResult($item, $result);
            return $item;
        }
        return null;
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

    public function GetAllShopBox()
    {
        $query = $this->db->q('exec SP_ItemsBox_All');
        $lista = array();
        while ($resultado = $query->fetch()) {
            $obj = new ShopGoodsBox();
            Utilitarios::autoInitObjectFromResult($obj, $resultado);
            $date = date_create($obj->getAddDate());
            $dateFormated = date_format($date, 'Y-m-d\TH:i:s');
            $obj->setAddDate($dateFormated);
            $lista[] = $obj;
        }
        return $lista;
    }

}
