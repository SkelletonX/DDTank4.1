<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ShopItemInfo
 *
 * @author jvbor
 */
class ShopItemInfo {

    //put your code here
    private $ID, $ShopID, $GroupID, $TemplateID, $BuyType, $Sort, $IsVouch, $Label, $Beat, $AUnit, $APrice1, $AValue1;
    private $APrice2, $AValue2, $APrice3, $AValue3, $BUnit, $BPrice1, $BValue1, $BPrice2, $BValue2, $BPrice3, $BValue3;
    private $CUnit, $CPrice1, $CValue1, $CPrice2, $CValue2, $CPrice3, $CValue3, $IsContinue, $IsCheap;
    private $LimitCount, $StartDate, $EndDate;

    function getID() {
        return $this->ID;
    }

    function getShopID() {
        return $this->ShopID;
    }

    function getGroupID() {
        return $this->GroupID;
    }

    function getTemplateID() {
        return $this->TemplateID;
    }

    function getBuyType() {
        return $this->BuyType;
    }

    function getSort() {
        return $this->Sort;
    }

    function getIsVouch() {
        return $this->IsVouch;
    }

    function getLabel() {
        return $this->Label;
    }

    function getBeat() {
        return $this->Beat;
    }

    function getAUnit() {
        return $this->AUnit;
    }

    function getAPrice1() {
        return $this->APrice1;
    }

    function getAValue1() {
        return $this->AValue1;
    }

    function getAPrice2() {
        return $this->APrice2;
    }

    function getAValue2() {
        return $this->AValue2;
    }

    function getAPrice3() {
        return $this->APrice3;
    }

    function getAValue3() {
        return $this->AValue3;
    }

    function getBUnit() {
        return $this->BUnit;
    }

    function getBPrice1() {
        return $this->BPrice1;
    }

    function getBValue1() {
        return $this->BValue1;
    }

    function getBPrice2() {
        return $this->BPrice2;
    }

    function getBValue2() {
        return $this->BValue2;
    }

    function getBPrice3() {
        return $this->BPrice3;
    }

    function getBValue3() {
        return $this->BValue3;
    }

    function getCUnit() {
        return $this->CUnit;
    }

    function getCPrice1() {
        return $this->CPrice1;
    }

    function getCValue1() {
        return $this->CValue1;
    }

    function getCPrice2() {
        return $this->CPrice2;
    }

    function getCValue2() {
        return $this->CValue2;
    }

    function getCPrice3() {
        return $this->CPrice3;
    }

    function getCValue3() {
        return $this->CValue3;
    }

    function getIsContinue() {
        return $this->IsContinue;
    }

    function getIsCheap() {
        return $this->IsCheap;
    }

    function getLimitCount() {
        return $this->LimitCount;
    }

    function getStartDate() {
        return $this->StartDate;
    }

    function getEndDate() {
        return $this->EndDate;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setShopID($ShopID) {
        $this->ShopID = $ShopID;
    }

    function setGroupID($GroupID) {
        $this->GroupID = $GroupID;
    }

    function setTemplateID($TemplateID) {
        $this->TemplateID = $TemplateID;
    }

    function setBuyType($BuyType) {
        $this->BuyType = $BuyType;
    }

    function setSort($Sort) {
        $this->Sort = $Sort;
    }

    function setIsVouch($IsVouch) {
        $this->IsVouch = $IsVouch;
    }

    function setLabel($Label) {
        $this->Label = $Label;
    }

    function setBeat($Beat) {
        $this->Beat = $Beat;
    }

    function setAUnit($AUnit) {
        $this->AUnit = $AUnit;
    }

    function setAPrice1($APrice1) {
        $this->APrice1 = $APrice1;
    }

    function setAValue1($AValue1) {
        $this->AValue1 = $AValue1;
    }

    function setAPrice2($APrice2) {
        $this->APrice2 = $APrice2;
    }

    function setAValue2($AValue2) {
        $this->AValue2 = $AValue2;
    }

    function setAPrice3($APrice3) {
        $this->APrice3 = $APrice3;
    }

    function setAValue3($AValue3) {
        $this->AValue3 = $AValue3;
    }

    function setBUnit($BUnit) {
        $this->BUnit = $BUnit;
    }

    function setBPrice1($BPrice1) {
        $this->BPrice1 = $BPrice1;
    }

    function setBValue1($BValue1) {
        $this->BValue1 = $BValue1;
    }

    function setBPrice2($BPrice2) {
        $this->BPrice2 = $BPrice2;
    }

    function setBValue2($BValue2) {
        $this->BValue2 = $BValue2;
    }

    function setBPrice3($BPrice3) {
        $this->BPrice3 = $BPrice3;
    }

    function setBValue3($BValue3) {
        $this->BValue3 = $BValue3;
    }

    function setCUnit($CUnit) {
        $this->CUnit = $CUnit;
    }

    function setCPrice1($CPrice1) {
        $this->CPrice1 = $CPrice1;
    }

    function setCValue1($CValue1) {
        $this->CValue1 = $CValue1;
    }

    function setCPrice2($CPrice2) {
        $this->CPrice2 = $CPrice2;
    }

    function setCValue2($CValue2) {
        $this->CValue2 = $CValue2;
    }

    function setCPrice3($CPrice3) {
        $this->CPrice3 = $CPrice3;
    }

    function setCValue3($CValue3) {
        $this->CValue3 = $CValue3;
    }

    function setIsContinue($IsContinue) {
        $this->IsContinue = $IsContinue;
    }

    function setIsCheap($IsCheap) {
        $this->IsCheap = $IsCheap;
    }

    function setLimitCount($LimitCount) {
        $this->LimitCount = $LimitCount;
    }

    function setStartDate($StartDate) {
        $this->StartDate = $StartDate;
    }

    function setEndDate($EndDate) {
        $this->EndDate = $EndDate;
    }

}
