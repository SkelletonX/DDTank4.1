<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of DailyLogListInfo
 *
 * @author jvbor
 */
class DailyLogListInfo {

    //put your code here
    private $ID, $UserID, $UserAwardLog, $DayLog, $LastDate;

    function getID() {
        return $this->ID;
    }

    function getUserID() {
        return $this->UserID;
    }

    function getUserAwardLog() {
        return $this->UserAwardLog;
    }

    function getDayLog() {
        return $this->DayLog;
    }

    function getLastDate() {
        return $this->LastDate;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setUserID($UserID) {
        $this->UserID = $UserID;
    }

    function setUserAwardLog($UserAwardLog) {
        $this->UserAwardLog = $UserAwardLog;
    }

    function setDayLog($DayLog) {
        $this->DayLog = $DayLog;
    }

    function setLastDate($LastDate) {
        $this->LastDate = $LastDate;
    }

}
