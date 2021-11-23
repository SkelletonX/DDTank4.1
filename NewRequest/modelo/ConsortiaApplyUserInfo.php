<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ConsortiaApplyUserInfo
 *
 * @author jvbor
 */
class ConsortiaApplyUserInfo {

    //put your code here
    private $ID, $ApplyDate, $ConsortiaID, $ConsortiaName, $ChairmanID, $ChairmanName, $IsExist, $Remark, $UserID, $UserName;
    private $typeVIP, $Win, $Total, $Repute, $FigthPower, $IsOld, $Offer;

    function getID() {
        return $this->ID;
    }

    function getApplyDate() {
        return $this->ApplyDate;
    }

    function getConsortiaID() {
        return $this->ConsortiaID;
    }

    function getConsortiaName() {
        return $this->ConsortiaName;
    }

    function getChairmanID() {
        return $this->ChairmanID;
    }

    function getChairmanName() {
        return $this->ChairmanName;
    }

    function getIsExist() {
        return $this->IsExist;
    }

    function getRemark() {
        return $this->Remark;
    }

    function getUserID() {
        return $this->UserID;
    }

    function getUserName() {
        return $this->UserName;
    }

    function getTypeVIP() {
        return $this->typeVIP;
    }

    function getWin() {
        return $this->Win;
    }

    function getTotal() {
        return $this->Total;
    }

    function getRepute() {
        return $this->Repute;
    }

    function getFigthPower() {
        return $this->FigthPower;
    }

    function getIsOld() {
        return $this->IsOld;
    }

    function getOffer() {
        return $this->Offer;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setApplyDate($ApplyDate) {
        $this->ApplyDate = $ApplyDate;
    }

    function setConsortiaID($ConsortiaID) {
        $this->ConsortiaID = $ConsortiaID;
    }

    function setConsortiaName($ConsortiaName) {
        $this->ConsortiaName = $ConsortiaName;
    }

    function setChairmanID($ChairmanID) {
        $this->ChairmanID = $ChairmanID;
    }

    function setChairmanName($ChairmanName) {
        $this->ChairmanName = $ChairmanName;
    }

    function setIsExist($IsExist) {
        $this->IsExist = $IsExist;
    }

    function setRemark($Remark) {
        $this->Remark = $Remark;
    }

    function setUserID($UserID) {
        $this->UserID = $UserID;
    }

    function setUserName($UserName) {
        $this->UserName = $UserName;
    }

    function setTypeVIP($typeVIP) {
        $this->typeVIP = $typeVIP;
    }

    function setWin($Win) {
        $this->Win = $Win;
    }

    function setTotal($Total) {
        $this->Total = $Total;
    }

    function setRepute($Repute) {
        $this->Repute = $Repute;
    }

    function setFigthPower($FigthPower) {
        $this->FigthPower = $FigthPower;
    }

    function setIsOld($IsOld) {
        $this->IsOld = $IsOld;
    }

    function setOffer($Offer) {
        $this->Offer = $Offer;
    }

}
