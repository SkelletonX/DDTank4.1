<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of QuestRateInfo
 *
 * @author jvbor
 */
class QuestRateInfo {

    //put your code here
    private $BindMoneyRate, $ExpRate, $GoldRate, $ExploitRate, $CanOneKeyFinishTime;

    function getBindMoneyRate() {
        return $this->BindMoneyRate;
    }

    function getExpRate() {
        return $this->ExpRate;
    }

    function getGoldRate() {
        return $this->GoldRate;
    }

    function getExploitRate() {
        return $this->ExploitRate;
    }

    function getCanOneKeyFinishTime() {
        return $this->CanOneKeyFinishTime;
    }

    function setBindMoneyRate($BindMoneyRate) {
        $this->BindMoneyRate = $BindMoneyRate;
    }

    function setExpRate($ExpRate) {
        $this->ExpRate = $ExpRate;
    }

    function setGoldRate($GoldRate) {
        $this->GoldRate = $GoldRate;
    }

    function setExploitRate($ExploitRate) {
        $this->ExploitRate = $ExploitRate;
    }

    function setCanOneKeyFinishTime($CanOneKeyFinishTime) {
        $this->CanOneKeyFinishTime = $CanOneKeyFinishTime;
    }

}
