<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of NewTitleInfo
 *
 * @author jvbor
 */
class NewTitleInfo {

    //put your code here
    private $ID, $Name, $Show, $Pic, $Att, $Def, $Agi, $Luck, $Desc, $Order;

    function getID() {
        return $this->ID;
    }

    function getName() {
        return $this->Name;
    }

    function getShow() {
        return $this->Show;
    }

    function getPic() {
        return $this->Pic;
    }

    function getAtt() {
        return $this->Att;
    }

    function getDef() {
        return $this->Def;
    }

    function getAgi() {
        return $this->Agi;
    }

    function getLuck() {
        return $this->Luck;
    }

    function getDesc() {
        return $this->Desc;
    }

    function getOrder() {
        return $this->Order;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setName($Name) {
        $this->Name = $Name;
    }

    function setShow($Show) {
        $this->Show = $Show;
    }

    function setPic($Pic) {
        $this->Pic = $Pic;
    }

    function setAtt($Att) {
        $this->Att = $Att;
    }

    function setDef($Def) {
        $this->Def = $Def;
    }

    function setAgi($Agi) {
        $this->Agi = $Agi;
    }

    function setLuck($Luck) {
        $this->Luck = $Luck;
    }

    function setDesc($Desc) {
        $this->Desc = $Desc;
    }

    function setOrder($Order) {
        $this->Order = $Order;
    }

}
