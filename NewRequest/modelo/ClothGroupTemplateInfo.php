<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ClothGroupTemplateInfo
 *
 * @author jvbor
 */
class ClothGroupTemplateInfo {

    //put your code here
    private $ItemID, $ID, $TemplateID, $Sex, $Description, $Cost, $Type, $OtherTemplateID;

    function getItemID() {
        return $this->ItemID;
    }

    function getID() {
        return $this->ID;
    }

    function getTemplateID() {
        return $this->TemplateID;
    }

    function getSex() {
        return $this->Sex;
    }

    function getDescription() {
        return $this->Description;
    }

    function getCost() {
        return $this->Cost;
    }

    function getType() {
        return $this->Type;
    }

    function getOtherTemplateID() {
        return $this->OtherTemplateID;
    }

    function setItemID($ItemID) {
        $this->ItemID = $ItemID;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setTemplateID($TemplateID) {
        $this->TemplateID = $TemplateID;
    }

    function setSex($Sex) {
        $this->Sex = $Sex;
    }

    function setDescription($Description) {
        $this->Description = $Description;
    }

    function setCost($Cost) {
        $this->Cost = $Cost;
    }

    function setType($Type) {
        $this->Type = $Type;
    }

    function setOtherTemplateID($OtherTemplateID) {
        $this->OtherTemplateID = $OtherTemplateID;
    }

}
