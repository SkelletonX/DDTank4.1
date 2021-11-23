<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of SubActiveInfo
 *
 * @author jvbor
 */
class SubActiveInfo {

    //put your code here
    private $ID, $ActiveID, $SubID, $IsOpen, $StartDate, $StartTime, $EndDate, $EndTime, $IsContinued, $ActiveInfo;

    function getID() {
        return $this->ID;
    }

    function getActiveID() {
        return $this->ActiveID;
    }

    function getSubID() {
        return $this->SubID;
    }

    function getIsOpen() {
        return $this->IsOpen;
    }

    function getStartDate() {
        return $this->StartDate;
    }

    function getStartTime() {
        return $this->StartTime;
    }

    function getEndDate() {
        return $this->EndDate;
    }

    function getEndTime() {
        return $this->EndTime;
    }

    function getIsContinued() {
        return $this->IsContinued;
    }

    function getActiveInfo() {
        return $this->ActiveInfo;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setActiveID($ActiveID) {
        $this->ActiveID = $ActiveID;
    }

    function setSubID($SubID) {
        $this->SubID = $SubID;
    }

    function setIsOpen($IsOpen) {
        $this->IsOpen = $IsOpen;
    }

    function setStartDate($StartDate) {
        $this->StartDate = $StartDate;
    }

    function setStartTime($StartTime) {
        $this->StartTime = $StartTime;
    }

    function setEndDate($EndDate) {
        $this->EndDate = $EndDate;
    }

    function setEndTime($EndTime) {
        $this->EndTime = $EndTime;
    }

    function setIsContinued($IsContinued) {
        $this->IsContinued = $IsContinued;
    }

    function setActiveInfo($ActiveInfo) {
        $this->ActiveInfo = $ActiveInfo;
    }

}
