<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ClothPropertyTemplateInfo
 *
 * @author jvbor
 */
class ClothPropertyTemplateInfo {

    //put your code here
    private $ID, $Sex, $Name, $Attack, $Defend, $Agility, $Luck, $Blood, $Damage, $Guard, $Cost, $Type;

    function getID() {
        return $this->ID;
    }

    function getSex() {
        return $this->Sex;
    }

    function getName() {
        return $this->Name;
    }

    function getAttack() {
        return $this->Attack;
    }

    function getDefend() {
        return $this->Defend;
    }

    function getAgility() {
        return $this->Agility;
    }

    function getLuck() {
        return $this->Luck;
    }

    function getBlood() {
        return $this->Blood;
    }

    function getDamage() {
        return $this->Damage;
    }

    function getGuard() {
        return $this->Guard;
    }

    function getCost() {
        return $this->Cost;
    }

    function getType() {
        return $this->Type;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setSex($Sex) {
        $this->Sex = $Sex;
    }

    function setName($Name) {
        $this->Name = $Name;
    }

    function setAttack($Attack) {
        $this->Attack = $Attack;
    }

    function setDefend($Defend) {
        $this->Defend = $Defend;
    }

    function setAgility($Agility) {
        $this->Agility = $Agility;
    }

    function setLuck($Luck) {
        $this->Luck = $Luck;
    }

    function setBlood($Blood) {
        $this->Blood = $Blood;
    }

    function setDamage($Damage) {
        $this->Damage = $Damage;
    }

    function setGuard($Guard) {
        $this->Guard = $Guard;
    }

    function setCost($Cost) {
        $this->Cost = $Cost;
    }

    function setType($Type) {
        $this->Type = $Type;
    }

}
