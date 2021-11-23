<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ConsortiaEquipControlInfo
 *
 * @author jvbor
 */
class ConsortiaEquipControlInfo {

    //put your code here
    private $ConsortiaID, $Level, $Riches, $Type;

    function getConsortiaID() {
        return $this->ConsortiaID;
    }

    function getLevel() {
        return $this->Level;
    }

    function getRiches() {
        return $this->Riches;
    }

    function getType() {
        return $this->Type;
    }

    function setConsortiaID($ConsortiaID) {
        $this->ConsortiaID = $ConsortiaID;
    }

    function setLevel($Level) {
        $this->Level = $Level;
    }

    function setRiches($Riches) {
        $this->Riches = $Riches;
    }

    function setType($Type) {
        $this->Type = $Type;
    }

}
