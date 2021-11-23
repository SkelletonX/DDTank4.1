<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of PetSkillInfo
 *
 * @author jvbor
 */
class PetSkillInfo {

    //put your code here
    private $ID, $Name, $ElementIDs, $Description, $BallType, $NewBallID, $CostMP, $Pic, $Action, $EffectPic;
    private $Delay, $ColdDown, $GameType, $Probability, $Damege, $DamageCrit;

    function getID() {
        return $this->ID;
    }

    function getName() {
        return $this->Name;
    }

    function getElementIDs() {
        return $this->ElementIDs;
    }

    function getDescription() {
        return $this->Description;
    }

    function getBallType() {
        return $this->BallType;
    }

    function getNewBallID() {
        return $this->NewBallID;
    }

    function getCostMP() {
        return $this->CostMP;
    }

    function getPic() {
        return $this->Pic;
    }

    function getAction() {
        return $this->Action;
    }

    function getEffectPic() {
        return $this->EffectPic;
    }

    function getDelay() {
        return $this->Delay;
    }

    function getColdDown() {
        return $this->ColdDown;
    }

    function getGameType() {
        return $this->GameType;
    }

    function getProbability() {
        return $this->Probability;
    }

    function getDamege() {
        return $this->Damege;
    }

    function getDamageCrit() {
        return $this->DamageCrit;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setName($Name) {
        $this->Name = $Name;
    }

    function setElementIDs($ElementIDs) {
        $this->ElementIDs = $ElementIDs;
    }

    function setDescription($Description) {
        $this->Description = $Description;
    }

    function setBallType($BallType) {
        $this->BallType = $BallType;
    }

    function setNewBallID($NewBallID) {
        $this->NewBallID = $NewBallID;
    }

    function setCostMP($CostMP) {
        $this->CostMP = $CostMP;
    }

    function setPic($Pic) {
        $this->Pic = $Pic;
    }

    function setAction($Action) {
        $this->Action = $Action;
    }

    function setEffectPic($EffectPic) {
        $this->EffectPic = $EffectPic;
    }

    function setDelay($Delay) {
        $this->Delay = $Delay;
    }

    function setColdDown($ColdDown) {
        $this->ColdDown = $ColdDown;
    }

    function setGameType($GameType) {
        $this->GameType = $GameType;
    }

    function setProbability($Probability) {
        $this->Probability = $Probability;
    }

    function setDamege($Damege) {
        $this->Damege = $Damege;
    }

    function setDamageCrit($DamageCrit) {
        $this->DamageCrit = $DamageCrit;
    }

}
