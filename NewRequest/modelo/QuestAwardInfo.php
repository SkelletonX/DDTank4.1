<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of QuestAwardInfo
 *
 * @author jvbor
 */
class QuestAwardInfo {

    //put your code here
    private $QuestID, $RewardItemID, $IsSelect, $RewardItemValid, $RewardItemCount1, $RewardItemCount2, $RewardItemCount3, $RewardItemCount4, $RewardItemCount5, $StrengthenLevel, $AttackCompose, $DefendCompose, $AgilityCompose, $LuckCompose, $IsCount, $IsBind, $MagicAttack, $MagicDefence;

    function getQuestID() {
        return $this->QuestID;
    }

    function getRewardItemID() {
        return $this->RewardItemID;
    }

    function getIsSelect() {
        return $this->IsSelect;
    }

    function getRewardItemValid() {
        return $this->RewardItemValid;
    }

    function getRewardItemCount1() {
        return $this->RewardItemCount1;
    }

    function getRewardItemCount2() {
        return $this->RewardItemCount2;
    }

    function getRewardItemCount3() {
        return $this->RewardItemCount3;
    }

    function getRewardItemCount4() {
        return $this->RewardItemCount4;
    }

    function getRewardItemCount5() {
        return $this->RewardItemCount5;
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

    function getIsCount() {
        return $this->IsCount;
    }

    function getIsBind() {
        return $this->IsBind;
    }

    function getMagicAttack() {
        return $this->MagicAttack;
    }

    function getMagicDefence() {
        return $this->MagicDefence;
    }

    function setQuestID($QuestID) {
        $this->QuestID = $QuestID;
    }

    function setRewardItemID($RewardItemID) {
        $this->RewardItemID = $RewardItemID;
    }

    function setIsSelect($IsSelect) {
        $this->IsSelect = $IsSelect;
    }

    function setRewardItemValid($RewardItemValid) {
        $this->RewardItemValid = $RewardItemValid;
    }

    function setRewardItemCount1($RewardItemCount1) {
        $this->RewardItemCount1 = $RewardItemCount1;
    }

    function setRewardItemCount2($RewardItemCount2) {
        $this->RewardItemCount2 = $RewardItemCount2;
    }

    function setRewardItemCount3($RewardItemCount3) {
        $this->RewardItemCount3 = $RewardItemCount3;
    }

    function setRewardItemCount4($RewardItemCount4) {
        $this->RewardItemCount4 = $RewardItemCount4;
    }

    function setRewardItemCount5($RewardItemCount5) {
        $this->RewardItemCount5 = $RewardItemCount5;
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

    function setIsCount($IsCount) {
        $this->IsCount = $IsCount;
    }

    function setIsBind($IsBind) {
        $this->IsBind = $IsBind;
    }

    function setMagicAttack($MagicAttack) {
        $this->MagicAttack = $MagicAttack;
    }

    function setMagicDefence($MagicDefence) {
        $this->MagicDefence = $MagicDefence;
    }

}
