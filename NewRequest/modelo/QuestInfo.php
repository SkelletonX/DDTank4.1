<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of QuestInfo
 *
 * @author jvbor
 */
class QuestInfo {

    private $ID, $QuestID, $Title, $Detail, $Objective, $NeedMinLevel, $NeedMaxLevel, $PreQuestID, $NextQuestID, $IsOther, $CanRepeat, $RepeatInterval, $RepeatMax, $RewardGP, $RewardGold, $RewardBindMoney, $RewardOffer, $RewardRiches, $RewardBuffID, $RewardBuffDate, $RewardMoney, $Rands, $RandDouble, $TimeMode, $StartDate, $EndDate, $MapID, $AutoEquip, $OneKeyFinishNeedMoney, $Rank, $StarLev, $NotMustCount, $Level2NeedMoney, $Level3NeedMoney, $Level4NeedMoney, $Level5NeedMoney, $CollocationCost, $CollocationColdTime, $IsAccept;

    function getID() {
        return $this->ID;
    }

    function getQuestID() {
        return $this->QuestID;
    }

    function getTitle() {
        return $this->Title;
    }

    function getDetail() {
        return $this->Detail;
    }

    function getObjective() {
        return $this->Objective;
    }

    function getNeedMinLevel() {
        return $this->NeedMinLevel;
    }

    function getNeedMaxLevel() {
        return $this->NeedMaxLevel;
    }

    function getPreQuestID() {
        return $this->PreQuestID;
    }

    function getNextQuestID() {
        return $this->NextQuestID;
    }

    function getIsOther() {
        return $this->IsOther;
    }

    function getCanRepeat() {
        return $this->CanRepeat;
    }

    function getRepeatInterval() {
        return $this->RepeatInterval;
    }

    function getRepeatMax() {
        return $this->RepeatMax;
    }

    function getRewardGP() {
        return $this->RewardGP;
    }

    function getRewardGold() {
        return $this->RewardGold;
    }

    function getRewardBindMoney() {
        return $this->RewardBindMoney;
    }

    function getRewardOffer() {
        return $this->RewardOffer;
    }

    function getRewardRiches() {
        return $this->RewardRiches;
    }

    function getRewardBuffID() {
        return $this->RewardBuffID;
    }

    function getRewardBuffDate() {
        return $this->RewardBuffDate;
    }

    function getRewardMoney() {
        return $this->RewardMoney;
    }

    function getRands() {
        return $this->Rands;
    }

    function getRandDouble() {
        return $this->RandDouble;
    }

    function getTimeMode() {
        return $this->TimeMode;
    }

    function getStartDate() {
        return $this->StartDate;
    }

    function getEndDate() {
        return $this->EndDate;
    }

    function getMapID() {
        return $this->MapID;
    }

    function getAutoEquip() {
        return $this->AutoEquip;
    }

    function getOneKeyFinishNeedMoney() {
        return $this->OneKeyFinishNeedMoney;
    }

    function getRank() {
        return $this->Rank;
    }

    function getStarLev() {
        return $this->StarLev;
    }

    function getNotMustCount() {
        return $this->NotMustCount;
    }

    function getLevel2NeedMoney() {
        return $this->Level2NeedMoney;
    }

    function getLevel3NeedMoney() {
        return $this->Level3NeedMoney;
    }

    function getLevel4NeedMoney() {
        return $this->Level4NeedMoney;
    }

    function getLevel5NeedMoney() {
        return $this->Level5NeedMoney;
    }

    function getCollocationCost() {
        return $this->CollocationCost;
    }

    function getCollocationColdTime() {
        return $this->CollocationColdTime;
    }

    function getIsAccept() {
        return $this->IsAccept;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setQuestID($QuestID) {
        $this->QuestID = $QuestID;
    }

    function setTitle($Title) {
        $this->Title = $Title;
    }

    function setDetail($Detail) {
        $this->Detail = $Detail;
    }

    function setObjective($Objective) {
        $this->Objective = $Objective;
    }

    function setNeedMinLevel($NeedMinLevel) {
        $this->NeedMinLevel = $NeedMinLevel;
    }

    function setNeedMaxLevel($NeedMaxLevel) {
        $this->NeedMaxLevel = $NeedMaxLevel;
    }

    function setPreQuestID($PreQuestID) {
        $this->PreQuestID = $PreQuestID;
    }

    function setNextQuestID($NextQuestID) {
        $this->NextQuestID = $NextQuestID;
    }

    function setIsOther($IsOther) {
        $this->IsOther = $IsOther;
    }

    function setCanRepeat($CanRepeat) {
        $this->CanRepeat = $CanRepeat;
    }

    function setRepeatInterval($RepeatInterval) {
        $this->RepeatInterval = $RepeatInterval;
    }

    function setRepeatMax($RepeatMax) {
        $this->RepeatMax = $RepeatMax;
    }

    function setRewardGP($RewardGP) {
        $this->RewardGP = $RewardGP;
    }

    function setRewardGold($RewardGold) {
        $this->RewardGold = $RewardGold;
    }

    function setRewardBindMoney($RewardBindMoney) {
        $this->RewardBindMoney = $RewardBindMoney;
    }

    function setRewardOffer($RewardOffer) {
        $this->RewardOffer = $RewardOffer;
    }

    function setRewardRiches($RewardRiches) {
        $this->RewardRiches = $RewardRiches;
    }

    function setRewardBuffID($RewardBuffID) {
        $this->RewardBuffID = $RewardBuffID;
    }

    function setRewardBuffDate($RewardBuffDate) {
        $this->RewardBuffDate = $RewardBuffDate;
    }

    function setRewardMoney($RewardMoney) {
        $this->RewardMoney = $RewardMoney;
    }

    function setRands($Rands) {
        $this->Rands = $Rands;
    }

    function setRandDouble($RandDouble) {
        $this->RandDouble = $RandDouble;
    }

    function setTimeMode($TimeMode) {
        $this->TimeMode = $TimeMode;
    }

    function setStartDate($StartDate) {
        $this->StartDate = $StartDate;
    }

    function setEndDate($EndDate) {
        $this->EndDate = $EndDate;
    }

    function setMapID($MapID) {
        $this->MapID = $MapID;
    }

    function setAutoEquip($AutoEquip) {
        $this->AutoEquip = $AutoEquip;
    }

    function setOneKeyFinishNeedMoney($OneKeyFinishNeedMoney) {
        $this->OneKeyFinishNeedMoney = $OneKeyFinishNeedMoney;
    }

    function setRank($Rank) {
        $this->Rank = $Rank;
    }

    function setStarLev($StarLev) {
        $this->StarLev = $StarLev;
    }

    function setNotMustCount($NotMustCount) {
        $this->NotMustCount = $NotMustCount;
    }

    function setLevel2NeedMoney($Level2NeedMoney) {
        $this->Level2NeedMoney = $Level2NeedMoney;
    }

    function setLevel3NeedMoney($Level3NeedMoney) {
        $this->Level3NeedMoney = $Level3NeedMoney;
    }

    function setLevel4NeedMoney($Level4NeedMoney) {
        $this->Level4NeedMoney = $Level4NeedMoney;
    }

    function setLevel5NeedMoney($Level5NeedMoney) {
        $this->Level5NeedMoney = $Level5NeedMoney;
    }

    function setCollocationCost($CollocationCost) {
        $this->CollocationCost = $CollocationCost;
    }

    function setCollocationColdTime($CollocationColdTime) {
        $this->CollocationColdTime = $CollocationColdTime;
    }

    function setIsAccept($IsAccept) {
        $this->IsAccept = $IsAccept;
    }

}
