<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ActiveConvertItemInfo
 *
 * @author jvbor
 */
class ActiveConvertItemInfo {

    //put your code here
    private $iD, $ActiveID, $TemplateID, $ItemType, $ItemCount, $LimitValue, $IsBind, $ValidDate;

    function getID() {
        return $this->iD;
    }

    function getActiveID() {
        return $this->ActiveID;
    }

    function getTemplateID() {
        return $this->TemplateID;
    }

    function getItemType() {
        return $this->ItemType;
    }

    function getItemCount() {
        return $this->ItemCount;
    }

    function getLimitValue() {
        return $this->LimitValue;
    }

    function getIsBind() {
        return $this->IsBind;
    }

    function getValidDate() {
        return $this->ValidDate;
    }

    function setID($iD) {
        $this->iD = $iD;
    }

    function setActiveID($ActiveID) {
        $this->ActiveID = $ActiveID;
    }

    function setTemplateID($TemplateID) {
        $this->TemplateID = $TemplateID;
    }

    function setItemType($ItemType) {
        $this->ItemType = $ItemType;
    }

    function setItemCount($ItemCount) {
        $this->ItemCount = $ItemCount;
    }

    function setLimitValue($LimitValue) {
        $this->LimitValue = $LimitValue;
    }

    function setIsBind($IsBind) {
        $this->IsBind = $IsBind;
    }

    function setValidDate($ValidDate) {
        $this->ValidDate = $ValidDate;
    }

}
