<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ItemInfo
 *
 * @author jvbor
 */
class ItemInfo {

    //put your code here
    private $AgilityCompose, $AttackCompose, $Color, $Count, $DefendCompose, $ItemID, $LuckCompose, $Place, $StrengthenLevel;
    private $TemplateID, $UserID, $ValidDate, $IsDirty, $IsExist, $IsBinds, $IsUsed, $BeginDate, $IsJudge, $BagType, $Skin;
    private $RemoveDate, $RemoveType, $Hole1, $Hole2, $Hole3, $Hole4, $Hole5, $Hole6, $StrengthenTimes, $StrengthenExp;
    private $Hole5Exp, $Hole6Exp, $goldBeginTime, $goldValidDate, $beadExp, $beadLevel, $beadIsLock;
    private $isShowBind, $latentEnergyCurStr, $latentEnergyEndTime, $Damage, $Guard, $Blood, $Bless, $AdvanceDate;

    function getAgilityCompose() {
        return $this->AgilityCompose;
    }

    function getAttackCompose() {
        return $this->AttackCompose;
    }

    function getColor() {
        return $this->Color;
    }

    function getCount() {
        return $this->Count;
    }

    function getDefendCompose() {
        return $this->DefendCompose;
    }

    function getItemID() {
        return $this->ItemID;
    }

    function getLuckCompose() {
        return $this->LuckCompose;
    }

    function getPlace() {
        return $this->Place;
    }

    function getStrengthenLevel() {
        return $this->StrengthenLevel;
    }

    function getTemplateID() {
        return $this->TemplateID;
    }

    function getUserID() {
        return $this->UserID;
    }

    function getValidDate() {
        return $this->ValidDate;
    }

    function getIsDirty() {
        return $this->IsDirty;
    }

    function getIsExist() {
        return $this->IsExist;
    }

    function getIsBinds() {
        return $this->IsBinds;
    }

    function getIsUsed() {
        return $this->IsUsed;
    }

    function getBeginDate() {
        return $this->BeginDate;
    }

    function getIsJudge() {
        return $this->IsJudge;
    }

    function getBagType() {
        return $this->BagType;
    }

    function getSkin() {
        return $this->Skin;
    }

    function getRemoveDate() {
        return $this->RemoveDate;
    }

    function getRemoveType() {
        return $this->RemoveType;
    }

    function getHole1() {
        return $this->Hole1;
    }

    function getHole2() {
        return $this->Hole2;
    }

    function getHole3() {
        return $this->Hole3;
    }

    function getHole4() {
        return $this->Hole4;
    }

    function getHole5() {
        return $this->Hole5;
    }

    function getHole6() {
        return $this->Hole6;
    }

    function getStrengthenTimes() {
        return $this->StrengthenTimes;
    }

    function getStrengthenExp() {
        return $this->StrengthenExp;
    }

    function getHole5Exp() {
        return $this->Hole5Exp;
    }

    function getHole6Exp() {
        return $this->Hole6Exp;
    }

    function getGoldBeginTime() {
        return $this->goldBeginTime;
    }

    function getGoldValidDate() {
        return $this->goldValidDate;
    }

    function getBeadExp() {
        return $this->beadExp;
    }

    function getBeadLevel() {
        return $this->beadLevel;
    }

    function getBeadIsLock() {
        return $this->beadIsLock;
    }

    function getIsShowBind() {
        return $this->isShowBind;
    }

    function getLatentEnergyCurStr() {
        return $this->latentEnergyCurStr;
    }

    function getLatentEnergyEndTime() {
        return $this->latentEnergyEndTime;
    }

    function getDamage() {
        return $this->Damage;
    }

    function getGuard() {
        return $this->Guard;
    }

    function getBlood() {
        return $this->Blood;
    }

    function getBless() {
        return $this->Bless;
    }

    function getAdvanceDate() {
        return $this->AdvanceDate;
    }

    function setAgilityCompose($AgilityCompose) {
        $this->AgilityCompose = $AgilityCompose;
    }

    function setAttackCompose($AttackCompose) {
        $this->AttackCompose = $AttackCompose;
    }

    function setColor($Color) {
        $this->Color = $Color;
    }

    function setCount($Count) {
        $this->Count = $Count;
    }

    function setDefendCompose($DefendCompose) {
        $this->DefendCompose = $DefendCompose;
    }

    function setItemID($ItemID) {
        $this->ItemID = $ItemID;
    }

    function setLuckCompose($LuckCompose) {
        $this->LuckCompose = $LuckCompose;
    }

    function setPlace($Place) {
        $this->Place = $Place;
    }

    function setStrengthenLevel($StrengthenLevel) {
        $this->StrengthenLevel = $StrengthenLevel;
    }

    function setTemplateID($TemplateID) {
        $this->TemplateID = $TemplateID;
    }

    function setUserID($UserID) {
        $this->UserID = $UserID;
    }

    function setValidDate($ValidDate) {
        $this->ValidDate = $ValidDate;
    }

    function setIsDirty($IsDirty) {
        $this->IsDirty = $IsDirty;
    }

    function setIsExist($IsExist) {
        $this->IsExist = $IsExist;
    }

    function setIsBinds($IsBinds) {
        $this->IsBinds = $IsBinds;
    }

    function setIsUsed($IsUsed) {
        $this->IsUsed = $IsUsed;
    }

    function setBeginDate($BeginDate) {
        $this->BeginDate = $BeginDate;
    }

    function setIsJudge($IsJudge) {
        $this->IsJudge = $IsJudge;
    }

    function setBagType($BagType) {
        $this->BagType = $BagType;
    }

    function setSkin($Skin) {
        $this->Skin = $Skin;
    }

    function setRemoveDate($RemoveDate) {
        $this->RemoveDate = $RemoveDate;
    }

    function setRemoveType($RemoveType) {
        $this->RemoveType = $RemoveType;
    }

    function setHole1($Hole1) {
        $this->Hole1 = $Hole1;
    }

    function setHole2($Hole2) {
        $this->Hole2 = $Hole2;
    }

    function setHole3($Hole3) {
        $this->Hole3 = $Hole3;
    }

    function setHole4($Hole4) {
        $this->Hole4 = $Hole4;
    }

    function setHole5($Hole5) {
        $this->Hole5 = $Hole5;
    }

    function setHole6($Hole6) {
        $this->Hole6 = $Hole6;
    }

    function setStrengthenTimes($StrengthenTimes) {
        $this->StrengthenTimes = $StrengthenTimes;
    }

    function setStrengthenExp($StrengthenExp) {
        $this->StrengthenExp = $StrengthenExp;
    }

    function setHole5Exp($Hole5Exp) {
        $this->Hole5Exp = $Hole5Exp;
    }

    function setHole6Exp($Hole6Exp) {
        $this->Hole6Exp = $Hole6Exp;
    }

    function setGoldBeginTime($goldBeginTime) {
        $this->goldBeginTime = $goldBeginTime;
    }

    function setGoldValidDate($goldValidDate) {
        $this->goldValidDate = $goldValidDate;
    }

    function setBeadExp($beadExp) {
        $this->beadExp = $beadExp;
    }

    function setBeadLevel($beadLevel) {
        $this->beadLevel = $beadLevel;
    }

    function setBeadIsLock($beadIsLock) {
        $this->beadIsLock = $beadIsLock;
    }

    function setIsShowBind($isShowBind) {
        $this->isShowBind = $isShowBind;
    }

    function setLatentEnergyCurStr($latentEnergyCurStr) {
        $this->latentEnergyCurStr = $latentEnergyCurStr;
    }

    function setLatentEnergyEndTime($latentEnergyEndTime) {
        $this->latentEnergyEndTime = $latentEnergyEndTime;
    }

    function setDamage($Damage) {
        $this->Damage = $Damage;
    }

    function setGuard($Guard) {
        $this->Guard = $Guard;
    }

    function setBlood($Blood) {
        $this->Blood = $Blood;
    }

    function setBless($Bless) {
        $this->Bless = $Bless;
    }

    function setAdvanceDate($AdvanceDate) {
        $this->AdvanceDate = $AdvanceDate;
    }

}
