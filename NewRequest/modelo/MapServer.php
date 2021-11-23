<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of MapServer
 *
 * @author jvbor
 */
class MapServer {

    //put your code here
    private $ServerID, $OpenMap, $IsSpecial;

    function getServerID() {
        return $this->ServerID;
    }

    function getOpenMap() {
        return $this->OpenMap;
    }

    function getIsSpecial() {
        return $this->IsSpecial;
    }

    function setServerID($ServerID) {
        $this->ServerID = $ServerID;
    }

    function setOpenMap($OpenMap) {
        $this->OpenMap = $OpenMap;
    }

    function setIsSpecial($IsSpecial) {
        $this->IsSpecial = $IsSpecial;
    }

}
