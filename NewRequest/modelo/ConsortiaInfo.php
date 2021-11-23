<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ConsortiaInfo
 *
 * @author jvbor
 */
class ConsortiaInfo {

    //put your code here
    private $ConsortiaID, $BuildDate, $CelebCount, $ChairmanID, $ChairmanName, $ChairmanTypeVIP, $ConsortiaName, $ChairmanVIPLevel, $CreatorID, $CreatorName;
    private $Description, $Honor, $IsExist, $Level, $MaxCount, $Placard, $IP, $Port, $Repute, $Count, $Riches, $DeductDate;
    private $AddDayHonor, $AddDayRiches, $AddWeekRiches, $AddWeekHonor, $LastDayRiches, $OpenApply, $StoreLevel, $SmithLevel, $ShopLevel, $SkillLevel, $BufferLevel;
    private $FightPower;

    function getFightPower() {
        return $this->FightPower;
    }

    function setFightPower($FightPower) {
        $this->FightPower = $FightPower;
    }

    function getBufferLevel() {
        return $this->BufferLevel;
    }

    function setBufferLevel($BufferLevel) {
        $this->BufferLevel = $BufferLevel;
    }

    function getConsortiaName() {
        return $this->ConsortiaName;
    }

    function setConsortiaName($ConsortiaName) {
        $this->ConsortiaName = $ConsortiaName;
    }

    function getConsortiaID() {
        return $this->ConsortiaID;
    }

    function getBuildDate() {
        return $this->BuildDate;
    }

    function getCelebCount() {
        return $this->CelebCount;
    }

    function getChairmanID() {
        return $this->ChairmanID;
    }

    function getChairmanName() {
        return $this->ChairmanName;
    }

    function getChairmanTypeVIP() {
        return $this->ChairmanTypeVIP;
    }

    function getChairmanVIPLevel() {
        return $this->ChairmanVIPLevel;
    }

    function getCreatorID() {
        return $this->CreatorID;
    }

    function getCreatorName() {
        return $this->CreatorName;
    }

    function getDescription() {
        return $this->Description;
    }

    function getHonor() {
        return $this->Honor;
    }

    function getIsExist() {
        return $this->IsExist;
    }

    function getLevel() {
        return $this->Level;
    }

    function getMaxCount() {
        return $this->MaxCount;
    }

    function getPlacard() {
        return $this->Placard;
    }

    function getIP() {
        return $this->IP;
    }

    function getPort() {
        return $this->Port;
    }

    function getRepute() {
        return $this->Repute;
    }

    function getCount() {
        return $this->Count;
    }

    function getRiches() {
        return $this->Riches;
    }

    function getDeductDate() {
        return $this->DeductDate;
    }

    function getAddDayHonor() {
        return $this->AddDayHonor;
    }

    function getAddDayRiches() {
        return $this->AddDayRiches;
    }

    function getAddWeekRiches() {
        return $this->AddWeekRiches;
    }

    function getAddWeekHonor() {
        return $this->AddWeekHonor;
    }

    function getLastDayRiches() {
        return $this->LastDayRiches;
    }

    function getOpenApply() {
        return $this->OpenApply;
    }

    function getStoreLevel() {
        return $this->StoreLevel;
    }

    function getSmithLevel() {
        return $this->SmithLevel;
    }

    function getShopLevel() {
        return $this->ShopLevel;
    }

    function getSkillLevel() {
        return $this->SkillLevel;
    }

    function setConsortiaID($ConsortiaID) {
        $this->ConsortiaID = $ConsortiaID;
    }

    function setBuildDate($BuildDate) {
        $this->BuildDate = $BuildDate;
    }

    function setCelebCount($CelebCount) {
        $this->CelebCount = $CelebCount;
    }

    function setChairmanID($ChairmanID) {
        $this->ChairmanID = $ChairmanID;
    }

    function setChairmanName($ChairmanName) {
        $this->ChairmanName = $ChairmanName;
    }

    function setChairmanTypeVIP($ChairmanTypeVIP) {
        $this->ChairmanTypeVIP = $ChairmanTypeVIP;
    }

    function setChairmanVIPLevel($ChairmanVIPLevel) {
        $this->ChairmanVIPLevel = $ChairmanVIPLevel;
    }

    function setCreatorID($CreatorID) {
        $this->CreatorID = $CreatorID;
    }

    function setCreatorName($CreatorName) {
        $this->CreatorName = $CreatorName;
    }

    function setDescription($Description) {
        $this->Description = $Description;
    }

    function setHonor($Honor) {
        $this->Honor = $Honor;
    }

    function setIsExist($IsExist) {
        $this->IsExist = $IsExist;
    }

    function setLevel($Level) {
        $this->Level = $Level;
    }

    function setMaxCount($MaxCount) {
        $this->MaxCount = $MaxCount;
    }

    function setPlacard($Placard) {
        $this->Placard = $Placard;
    }

    function setIP($IP) {
        $this->IP = $IP;
    }

    function setPort($Port) {
        $this->Port = $Port;
    }

    function setRepute($Repute) {
        $this->Repute = $Repute;
    }

    function setCount($Count) {
        $this->Count = $Count;
    }

    function setRiches($Riches) {
        $this->Riches = $Riches;
    }

    function setDeductDate($DeductDate) {
        $this->DeductDate = $DeductDate;
    }

    function setAddDayHonor($AddDayHonor) {
        $this->AddDayHonor = $AddDayHonor;
    }

    function setAddDayRiches($AddDayRiches) {
        $this->AddDayRiches = $AddDayRiches;
    }

    function setAddWeekRiches($AddWeekRiches) {
        $this->AddWeekRiches = $AddWeekRiches;
    }

    function setAddWeekHonor($AddWeekHonor) {
        $this->AddWeekHonor = $AddWeekHonor;
    }

    function setLastDayRiches($LastDayRiches) {
        $this->LastDayRiches = $LastDayRiches;
    }

    function setOpenApply($OpenApply) {
        $this->OpenApply = $OpenApply;
    }

    function setStoreLevel($StoreLevel) {
        $this->StoreLevel = $StoreLevel;
    }

    function setSmithLevel($SmithLevel) {
        $this->SmithLevel = $SmithLevel;
    }

    function setShopLevel($ShopLevel) {
        $this->ShopLevel = $ShopLevel;
    }

    function setSkillLevel($SkillLevel) {
        $this->SkillLevel = $SkillLevel;
    }

}
