<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ActivitySystemItemInfo
 *
 * @author jvbor
 */
class ActivitySystemItemInfo {

    //put your code here
    private $ID, $ActivityType, $Quality, $TemplateID, $Count, $ValidDate, $IsBinds, $StrengthenLevel;
    private $AttackCompose, $DefendCompose, $AgilityCompose, $LuckCompose, $Random;

    function getID() {
        return $this->ID;
    }

    function getActivityType() {
        return $this->ActivityType;
    }

    function getQuality() {
        return $this->Quality;
    }

    function getTemplateID() {
        return $this->TemplateID;
    }

    function getCount() {
        return $this->Count;
    }

    function getValidDate() {
        return $this->ValidDate;
    }

    function getIsBinds() {
        return $this->IsBinds;
    }

    function getStrengthenLevel() {
        return $this->StrengthenLevel;
    }

    function getAttackCompose() {
        return $this->AttackCompose;
    }

    function getDefendCompose() {
        return $this->DefendCompose;
    }

    function getAgilityCompose() {
        return $this->AgilityCompose;
    }

    function getLuckCompose() {
        return $this->LuckCompose;
    }

    function getRandom() {
        return $this->Random;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setActivityType($ActivityType) {
        $this->ActivityType = $ActivityType;
    }

    function setQuality($Quality) {
        $this->Quality = $Quality;
    }

    function setTemplateID($TemplateID) {
        $this->TemplateID = $TemplateID;
    }

    function setCount($Count) {
        $this->Count = $Count;
    }

    function setValidDate($ValidDate) {
        $this->ValidDate = $ValidDate;
    }

    function setIsBinds($IsBinds) {
        $this->IsBinds = $IsBinds;
    }

    function setStrengthenLevel($StrengthenLevel) {
        $this->StrengthenLevel = $StrengthenLevel;
    }

    function setAttackCompose($AttackCompose) {
        $this->AttackCompose = $AttackCompose;
    }

    function setDefendCompose($DefendCompose) {
        $this->DefendCompose = $DefendCompose;
    }

    function setAgilityCompose($AgilityCompose) {
        $this->AgilityCompose = $AgilityCompose;
    }

    function setLuckCompose($LuckCompose) {
        $this->LuckCompose = $LuckCompose;
    }

    function setRandom($Random) {
        $this->Random = $Random;
    }

}
