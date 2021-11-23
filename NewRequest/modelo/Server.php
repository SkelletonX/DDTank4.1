<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of Server
 *
 * @author jvbor
 */
class Server extends AbstractEntity {

    //put your code here
    private $ID, $Name, $IP, $Port, $State, $MustLevel, $LowestLevel, $Online, $Remark;

    function getID() {
        return $this->ID;
    }

    function getName() {
        return $this->Name;
    }

    function getIP() {
        return $this->IP;
    }

    function getPort() {
        return $this->Port;
    }

    function getState() {
        return $this->State;
    }

    function getMustLevel() {
        return $this->MustLevel;
    }

    function getLowestLevel() {
        return $this->LowestLevel;
    }

    function getOnline() {
        return $this->Online;
    }

    function getRemark() {
        return $this->Remark;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setName($Name) {
        $this->Name = $Name;
    }

    function setIP($IP) {
        $this->IP = $IP;
    }

    function setPort($Port) {
        $this->Port = $Port;
    }

    function setState($State) {
        $this->State = $State;
    }

    function setMustLevel($MustLevel) {
        $this->MustLevel = $MustLevel;
    }

    function setLowestLevel($LowestLevel) {
        $this->LowestLevel = $LowestLevel;
    }

    function setOnline($Online) {
        $this->Online = $Online * 2;
    }

    function setRemark($Remark) {
        $this->Remark = $Remark;
    }

}
