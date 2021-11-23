<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of PetTemplateInfo
 *
 * @author jvbor
 */
class PetTemplateInfo {

    //put your code here
    private $TemplateID, $Name, $KindID, $Description, $Pic, $RareLevel, $MP, $StarLevel, $GameAssetUrl;
    private $HighAgility;
    private $HighAgilityGrow;
    private $HighAttack;
    private $HighAttackGrow;
    private $HighBlood;
    private $HighBloodGrow;
    private $HighDamage;
    private $HighDamageGrow;
    private $HighDefence;
    private $HighDefenceGrow;
    private $HighGuard;
    private $HighGuardGrow;
    private $HighLuck;
    private $HighLuckGrow;
    private $EvolutionID;
    private $WashGetCount, $LowBloodGrow, $LowAttackGrow, $LowDefenceGrow, $LowAgilityGrow, $LowLuckGrow;


    function getTemplateID() {
        return $this->TemplateID;
    }

    function getName() {
        return $this->Name;
    }

    function getKindID() {
        return $this->KindID;
    }

    function getDescription() {
        return $this->Description;
    }

    function getPic() {
        return $this->Pic;
    }

    function getRareLevel() {
        return $this->RareLevel;
    }

    function getMP() {
        return $this->MP;
    }

    function getStarLevel() {
        return $this->StarLevel;
    }

    function getGameAssetUrl() {
        return $this->GameAssetUrl;
    }

    function getHighAgility() {
        return $this->HighAgility;
    }

    function getHighAgilityGrow() {
        return $this->HighAgilityGrow;
    }

    function getHighAttack() {
        return $this->HighAttack;
    }

    function getHighAttackGrow() {
        return $this->HighAttackGrow;
    }

    function getHighBlood() {
        return $this->HighBlood;
    }

    function getHighBloodGrow() {
        return $this->HighBloodGrow;
    }

    function getHighDamage() {
        return $this->HighDamage;
    }

    function getHighDamageGrow() {
        return $this->HighDamageGrow;
    }

    function getHighDefence() {
        return $this->HighDefence;
    }

    function getHighDefenceGrow() {
        return $this->HighDefenceGrow;
    }

    function getHighGuard() {
        return $this->HighGuard;
    }

    function getHighGuardGrow() {
        return $this->HighGuardGrow;
    }

    function getHighLuck() {
        return $this->HighLuck;
    }

    function getHighLuckGrow() {
        return $this->HighLuckGrow;
    }

    function getEvolutionID() {
        return $this->EvolutionID;
    }

    function getLowBloodGrow()
    {
        return $this->LowBloodGrow;
    }

    function getLowAttackGrow()
    {
        return $this->LowAttackGrow;
    }

    function getLowDefenceGrow()
    {
        return $this->LowDefenceGrow;
    }

    function getLowAgilityGrow()
    {
        return $this->LowAgilityGrow;
    }

    function getLowLuckGrow()
    {
        return $this->LowLuckGrow;
    }

    function setTemplateID($TemplateID) {
        $this->TemplateID = $TemplateID;
    }

    function setName($Name) {
        $this->Name = $Name;
    }

    function setKindID($KindID) {
        $this->KindID = $KindID;
    }

    function setDescription($Description) {
        $this->Description = $Description;
    }

    function setPic($Pic) {
        $this->Pic = $Pic;
    }

    function setRareLevel($RareLevel) {
        $this->RareLevel = $RareLevel;
    }

    function setMP($MP) {
        $this->MP = $MP;
    }

    function setStarLevel($StarLevel) {
        $this->StarLevel = $StarLevel;
    }

    function setGameAssetUrl($GameAssetUrl) {
        $this->GameAssetUrl = $GameAssetUrl;
    }

    function setHighAgility($HighAgility) {
        $this->HighAgility = $HighAgility;
    }

    function setHighAgilityGrow($HighAgilityGrow) {
        $this->HighAgilityGrow = $HighAgilityGrow;
    }

    function setHighAttack($HighAttack) {
        $this->HighAttack = $HighAttack;
    }

    function setHighAttackGrow($HighAttackGrow) {
        $this->HighAttackGrow = $HighAttackGrow;
    }

    function setHighBlood($HighBlood) {
        $this->HighBlood = $HighBlood;
    }

    function setHighBloodGrow($HighBloodGrow) {
        $this->HighBloodGrow = $HighBloodGrow;
    }

    function setHighDamage($HighDamage) {
        $this->HighDamage = $HighDamage;
    }

    function setHighDamageGrow($HighDamageGrow) {
        $this->HighDamageGrow = $HighDamageGrow;
    }

    function setHighDefence($HighDefence) {
        $this->HighDefence = $HighDefence;
    }

    function setHighDefenceGrow($HighDefenceGrow) {
        $this->HighDefenceGrow = $HighDefenceGrow;
    }

    function setHighGuard($HighGuard) {
        $this->HighGuard = $HighGuard;
    }

    function setHighGuardGrow($HighGuardGrow) {
        $this->HighGuardGrow = $HighGuardGrow;
    }

    function setHighLuck($HighLuck) {
        $this->HighLuck = $HighLuck;
    }

    function setHighLuckGrow($HighLuckGrow) {
        $this->HighLuckGrow = $HighLuckGrow;
    }

    function setEvolutionID($EvolutionID) {
        $this->EvolutionID = $EvolutionID;
    }

    function setLowBloodGrow($LowBloodGrow)
    {
        $this->LowBloodGrow = $LowBloodGrow;
    }

    function setLowAttackGrow($LowAttackGrow)
    {
        $this->LowAttackGrow = $LowAttackGrow;
    }

    function setLowDefenceGrow($LowDefenceGrow)
    {
        $this->LowDefenceGrow = $LowDefenceGrow;
    }

    function setLowAgilityGrow($LowAgilityGrow)
    {
        $this->LowAgilityGrow = $LowAgilityGrow;
    }

    function setLowLuckGrow($LowLuckGrow)
    {
        $this->LowLuckGrow = $LowLuckGrow;
    }

}
