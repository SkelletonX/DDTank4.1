<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of QuestCondictionInfo
 *
 * @author jvbor
 */
class QuestCondictionInfo {

    //put your code here
    private $QuestID, $CondictionID, $CondictionType, $CondictionTitle, $Para1, $Para2, $isOpitional;

    function getQuestID() {
        return $this->QuestID;
    }

    function getCondictionID() {
        return $this->CondictionID;
    }

    function getCondictionType() {
        return $this->CondictionType;
    }

    function getCondictionTitle() {
        return $this->CondictionTitle;
    }

    function getPara1() {
        return $this->Para1;
    }

    function getPara2() {
        return $this->Para2;
    }

    function getIsOpitional() {
        return $this->isOpitional;
    }

    function setQuestID($QuestID) {
        $this->QuestID = $QuestID;
    }

    function setCondictionID($CondictionID) {
        $this->CondictionID = $CondictionID;
    }

    function setCondictionType($CondictionType) {
        $this->CondictionType = $CondictionType;
    }

    function setCondictionTitle($CondictionTitle) {
        $this->CondictionTitle = $CondictionTitle;
    }

    function setPara1($Para1) {
        $this->Para1 = $Para1;
    }

    function setPara2($Para2) {
        $this->Para2 = $Para2;
    }

    function setIsOpitional($isOpitional) {
        $this->isOpitional = $isOpitional;
    }

}
