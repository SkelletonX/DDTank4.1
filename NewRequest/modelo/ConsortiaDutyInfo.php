<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ConsortiaDutyInfo
 *
 * @author jvbor
 */
class ConsortiaDutyInfo {

    //put your code here
    private $DutyID, $ConsortiaID, $DutyName, $IsExist, $Right, $Level;

    function getDutyID() {
        return $this->DutyID;
    }

    function getConsortiaID() {
        return $this->ConsortiaID;
    }

    function getDutyName() {
        return $this->DutyName;
    }

    function getIsExist() {
        return $this->IsExist;
    }

    function getRight() {
        return $this->Right;
    }

    function getLevel() {
        return $this->Level;
    }

    function setDutyID($DutyID) {
        $this->DutyID = $DutyID;
    }

    function setConsortiaID($ConsortiaID) {
        $this->ConsortiaID = $ConsortiaID;
    }

    function setDutyName($DutyName) {
        $this->DutyName = $DutyName;
    }

    function setIsExist($IsExist) {
        $this->IsExist = $IsExist;
    }

    function setRight($Right) {
        $this->Right = $Right;
    }

    function setLevel($Level) {
        $this->Level = $Level;
    }

}
