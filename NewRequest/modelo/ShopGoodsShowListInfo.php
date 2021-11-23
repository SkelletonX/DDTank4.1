<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ShopGoodsShowListInfo
 *
 * @author jvbor
 */
class ShopGoodsShowListInfo {

    //put your code here
    private $Type, $ShopId;

    function getType() {
        return $this->Type;
    }

    function getShopId() {
        return $this->ShopId;
    }

    function setType($Type) {
        $this->Type = $Type;
    }

    function setShopId($ShopId) {
        $this->ShopId = $ShopId;
    }

}
