<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of SubActiveConditionInfo
 *
 * @author jvbor
 */
class SubActiveConditionInfo {

    //put your code here
    private $ID, $ActiveID, $SubID, $Type, $Value, $AwardType, $AwardValue, $IsValid, $ConditionID;

    function getConditionID() {
        return $this->ConditionID;
    }

    function setConditionID($ConditionID) {
        $this->ConditionID = $ConditionID;
    }

    function getID() {
        return $this->ID;
    }

    function getActiveID() {
        return $this->ActiveID;
    }

    function getSubID() {
        return $this->SubID;
    }

    function getType() {
        return $this->Type;
    }

    function getValue() {
        return $this->Value;
    }

    function getAwardType() {
        return $this->AwardType;
    }

    function getAwardValue() {
        return $this->AwardValue;
    }

    function getIsValid() {
        return $this->IsValid;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setActiveID($ActiveID) {
        $this->ActiveID = $ActiveID;
    }

    function setSubID($SubID) {
        $this->SubID = $SubID;
    }

    function setType($Type) {
        $this->Type = $Type;
    }

    function setValue($Value) {
        $this->Value = $Value;
    }

    function setAwardType($AwardType) {
        $this->AwardType = $AwardType;
    }

    function setAwardValue($AwardValue) {
        $this->AwardValue = $AwardValue;
    }

    function setIsValid($IsValid) {
        $this->IsValid = $IsValid;
    }

}
