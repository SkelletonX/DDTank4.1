<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of PlayerInfo
 *
 * @author jvbor
 */
class PlayerInfo extends AbstractEntity {

    private $ID, $UserName, $NickName, $typeVIP, $VIPLevel, $Grade, $Repute, $Sex, $WinCount, $TotalCount, $ConsortiaName, $ChairmanID, $Rename, $ConsortiaRename, $EscapeCount, $IsFirst, $FightPower, $LastDate;
    private $Date, $IsConsortia, $ConsortiaID, $DutyName, $GP, $Honor, $Style, $Gold, $Colors, $Attack, $Defence, $Agility, $Luck, $Hide, $Offer, $Skin, $ReputeOffer, $ConsortiaHonor, $ConsortiaLevel;
    private $ConsortiaRepute, $Money, $AntiAddiction, $IsMarried, $SpouseID, $SpouseName, $MarryInfoID, $IsCreatedMarryRoom, $IsGotRing, $LoginName, $Nimbus, $AnswerSite, $WeaklessGuildProgressStr, $TotalPrestige, $IsOldPlayer;
    private $pntsBattle2;

    function getPntsBattle2() {
        return $this->pntsBattle2;
    }

    function setPntsBattle2($pntsBattle2) {
        $this->pntsBattle2 = $pntsBattle2;
    }

    function getTotalPrestige() {
        return $this->TotalPrestige;
    }

    function setTotalPrestige($TotalPrestige) {
        $this->TotalPrestige = $TotalPrestige;
    }

    function getID() {
        return $this->ID;
    }

    function getUserName() {
        return $this->UserName;
    }

    function getNickName() {
        return $this->NickName;
    }

    function getTypeVIP() {
        return $this->typeVIP;
    }

    function getVIPLevel() {
        return $this->VIPLevel;
    }

    function getGrade() {
        return $this->Grade;
    }

    function getRepute() {
        return $this->Repute;
    }

    function getSex() {
        return $this->Sex;
    }

    function getWinCount() {
        return $this->WinCount;
    }

    function getTotalCount() {
        return $this->TotalCount;
    }

    function getConsortiaName() {
        return $this->ConsortiaName;
    }

    function getChairmanID() {
        return $this->ChairmanID;
    }

    function getRename() {
        return $this->Rename;
    }

    function getConsortiaRename() {
        return $this->ConsortiaRename;
    }

    function getEscapeCount() {
        return $this->EscapeCount;
    }

    function getIsFirst() {
        return $this->IsFirst;
    }

    function getFightPower() {
        return $this->FightPower;
    }

    function getLastDate() {
        return $this->LastDate;
    }

    function getDate() {
        return $this->Date;
    }

    function getIsConsortia() {
        return $this->IsConsortia;
    }

    function getConsortiaID() {
        return $this->ConsortiaID;
    }

    function getDutyName() {
        return $this->DutyName;
    }

    function getGP() {
        return $this->GP;
    }

    function getHonor() {
        return $this->Honor;
    }

    function getStyle() {
        return $this->Style;
    }

    function getGold() {
        return $this->Gold;
    }

    function getColors() {
        return $this->Colors;
    }

    function getAttack() {
        return $this->Attack;
    }

    function getDefence() {
        return $this->Defence;
    }

    function getAgility() {
        return $this->Agility;
    }

    function getLuck() {
        return $this->Luck;
    }

    function getHide() {
        return $this->Hide;
    }

    function getOffer() {
        return $this->Offer;
    }

    function getSkin() {
        return $this->Skin;
    }

    function getReputeOffer() {
        return $this->ReputeOffer;
    }

    function getConsortiaHonor() {
        return $this->ConsortiaHonor;
    }

    function getConsortiaLevel() {
        return $this->ConsortiaLevel;
    }

    function getConsortiaRepute() {
        return $this->ConsortiaRepute;
    }

    function getMoney() {
        return $this->Money;
    }

    function getAntiAddiction() {
        return $this->AntiAddiction;
    }

    function getIsMarried() {
        return $this->IsMarried;
    }

    function getSpouseID() {
        return $this->SpouseID;
    }

    function getSpouseName() {
        return $this->SpouseName;
    }

    function getMarryInfoID() {
        return $this->MarryInfoID;
    }

    function getIsCreatedMarryRoom() {
        return $this->IsCreatedMarryRoom;
    }

    function getIsGotRing() {
        return $this->IsGotRing;
    }

    function getLoginName() {
        return $this->LoginName;
    }

    function getNimbus() {
        return $this->Nimbus;
    }

    function getAnswerSite() {
        return $this->AnswerSite;
    }

    function getWeaklessGuildProgressStr() {
        return $this->WeaklessGuildProgressStr;
    }

    function getIsOldPlayer() {
        return $this->IsOldPlayer;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setUserName($UserName) {
        $this->UserName = $UserName;
    }

    function setNickName($NickName) {
        $this->NickName = $NickName;
    }

    function setTypeVIP($typeVIP) {
        $this->typeVIP = $typeVIP;
    }

    function setVIPLevel($VIPLevel) {
        $this->VIPLevel = $VIPLevel;
    }

    function setGrade($Grade) {
        $this->Grade = $Grade;
    }

    function setRepute($Repute) {
        $this->Repute = $Repute;
    }

    function setSex($Sex) {
        $this->Sex = $Sex;
    }

    function setWinCount($WinCount) {
        $this->WinCount = $WinCount;
    }

    function setTotalCount($TotalCount) {
        $this->TotalCount = $TotalCount;
    }

    function setConsortiaName($ConsortiaName) {
        $this->ConsortiaName = $ConsortiaName;
    }

    function setChairmanID($ChairmanID) {
        $this->ChairmanID = $ChairmanID;
    }

    function setRename($Rename) {
        $this->Rename = $Rename;
    }

    function setConsortiaRename($ConsortiaRename) {
        $this->ConsortiaRename = $ConsortiaRename;
    }

    function setEscapeCount($EscapeCount) {
        $this->EscapeCount = $EscapeCount;
    }

    function setIsFirst($IsFirst) {
        $this->IsFirst = $IsFirst;
    }

    function setFightPower($FightPower) {
        $this->FightPower = $FightPower;
    }

    function setLastDate($LastDate) {
        $this->LastDate = $LastDate;
    }

    function setDate($Date) {
        $this->Date = $Date;
    }

    function setIsConsortia($IsConsortia) {
        $this->IsConsortia = $IsConsortia;
    }

    function setConsortiaID($ConsortiaID) {
        $this->ConsortiaID = $ConsortiaID;
    }

    function setDutyName($DutyName) {
        $this->DutyName = $DutyName;
    }

    function setGP($GP) {
        $this->GP = $GP;
    }

    function setHonor($Honor) {
        $this->Honor = $Honor;
    }

    function setStyle($Style) {
        $this->Style = $Style;
    }

    function setGold($Gold) {
        $this->Gold = $Gold;
    }

    function setColors($Colors) {
        $this->Colors = $Colors;
    }

    function setAttack($Attack) {
        $this->Attack = $Attack;
    }

    function setDefence($Defence) {
        $this->Defence = $Defence;
    }

    function setAgility($Agility) {
        $this->Agility = $Agility;
    }

    function setLuck($Luck) {
        $this->Luck = $Luck;
    }

    function setHide($Hide) {
        $this->Hide = $Hide;
    }

    function setOffer($Offer) {
        $this->Offer = $Offer;
    }

    function setSkin($Skin) {
        $this->Skin = $Skin;
    }

    function setReputeOffer($ReputeOffer) {
        $this->ReputeOffer = $ReputeOffer;
    }

    function setConsortiaHonor($ConsortiaHonor) {
        $this->ConsortiaHonor = $ConsortiaHonor;
    }

    function setConsortiaLevel($ConsortiaLevel) {
        $this->ConsortiaLevel = $ConsortiaLevel;
    }

    function setConsortiaRepute($ConsortiaRepute) {
        $this->ConsortiaRepute = $ConsortiaRepute;
    }

    function setMoney($Money) {
        $this->Money = $Money;
    }

    function setAntiAddiction($AntiAddiction) {
        $this->AntiAddiction = $AntiAddiction;
    }

    function setIsMarried($IsMarried) {
        $this->IsMarried = $IsMarried;
    }

    function setSpouseID($SpouseID) {
        $this->SpouseID = $SpouseID;
    }

    function setSpouseName($SpouseName) {
        $this->SpouseName = $SpouseName;
    }

    function setMarryInfoID($MarryInfoID) {
        $this->MarryInfoID = $MarryInfoID;
    }

    function setIsCreatedMarryRoom($IsCreatedMarryRoom) {
        $this->IsCreatedMarryRoom = $IsCreatedMarryRoom;
    }

    function setIsGotRing($IsGotRing) {
        $this->IsGotRing = $IsGotRing;
    }

    function setLoginName($LoginName) {
        $this->LoginName = $LoginName;
    }

    function setNimbus($Nimbus) {
        $this->Nimbus = $Nimbus;
    }

    function setAnswerSite($AnswerSite) {
        $this->AnswerSite = $AnswerSite;
    }

    function setWeaklessGuildProgressStr($WeaklessGuildProgressStr) {
        $this->WeaklessGuildProgressStr = $WeaklessGuildProgressStr;
    }

    function setIsOldPlayer($IsOldPlayer) {
        $this->IsOldPlayer = $IsOldPlayer;
    }

}
