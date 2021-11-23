<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of BallConfigInfo
 *
 * @author SYSTEM
 */
class BallConfigInfo {

    //put your code here
    private $TemplateID, $Common, $CommonAddWound, $CommonMultiBall, $Special;

    function getTemplateID() {
        return $this->TemplateID;
    }

    function getCommon() {
        return $this->Common;
    }

    function getCommonAddWound() {
        return $this->CommonAddWound;
    }

    function getCommonMultiBall() {
        return $this->CommonMultiBall;
    }

    function getSpecial() {
        return $this->Special;
    }

    function setTemplateID($TemplateID) {
        $this->TemplateID = $TemplateID;
    }

    function setCommon($Common) {
        $this->Common = $Common;
    }

    function setCommonAddWound($CommonAddWound) {
        $this->CommonAddWound = $CommonAddWound;
    }

    function setCommonMultiBall($CommonMultiBall) {
        $this->CommonMultiBall = $CommonMultiBall;
    }

    function setSpecial($Special) {
        $this->Special = $Special;
    }

}
