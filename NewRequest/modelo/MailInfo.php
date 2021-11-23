<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of MailInfo
 *
 * @author jvbor
 */
class MailInfo {

    //put your code here
    private $ID, $SenderID, $Sender, $ReceiverID, $Receiver, $Title, $Content, $Annex1, $Annex2, $Gold, $Money, $IsExist, $Type, $ValidDate, $IsRead;
    private $SendTime, $Annex1Name, $Annex2Name, $Annex3, $Annex4, $Annex5, $Annex3Name, $Annex4Name, $Annex5Name, $AnnexRemark, $GiftToken;

    function getAnnex5() {
        return $this->Annex5;
    }

    function setAnnex5($Annex5) {
        $this->Annex5 = $Annex5;
    }

    function getID() {
        return $this->ID;
    }

    function getSenderID() {
        return $this->SenderID;
    }

    function getSender() {
        return $this->Sender;
    }

    function getReceiverID() {
        return $this->ReceiverID;
    }

    function getReceiver() {
        return $this->Receiver;
    }

    function getTitle() {
        return $this->Title;
    }

    function getContent() {
        return $this->Content;
    }

    function getAnnex1() {
        return $this->Annex1;
    }

    function getAnnex2() {
        return $this->Annex2;
    }

    function getGold() {
        return $this->Gold;
    }

    function getMoney() {
        return $this->Money;
    }

    function getIsExist() {
        return $this->IsExist;
    }

    function getType() {
        return $this->Type;
    }

    function getValidDate() {
        return $this->ValidDate;
    }

    function getIsRead() {
        return $this->IsRead;
    }

    function getSendTime() {
        return $this->SendTime;
    }

    function getAnnex1Name() {
        return $this->Annex1Name;
    }

    function getAnnex2Name() {
        return $this->Annex2Name;
    }

    function getAnnex3() {
        return $this->Annex3;
    }

    function getAnnex4() {
        return $this->Annex4;
    }

    function getAnnex3Name() {
        return $this->Annex3Name;
    }

    function getAnnex4Name() {
        return $this->Annex4Name;
    }

    function getAnnex5Name() {
        return $this->Annex5Name;
    }

    function getAnnexRemark() {
        return $this->AnnexRemark;
    }

    function getGiftToken() {
        return $this->GiftToken;
    }

    function setID($ID) {
        $this->ID = $ID;
    }

    function setSenderID($SenderID) {
        $this->SenderID = $SenderID;
    }

    function setSender($Sender) {
        $this->Sender = $Sender;
    }

    function setReceiverID($ReceiverID) {
        $this->ReceiverID = $ReceiverID;
    }

    function setReceiver($Receiver) {
        $this->Receiver = $Receiver;
    }

    function setTitle($Title) {
        $this->Title = $Title;
    }

    function setContent($Content) {
        $this->Content = $Content;
    }

    function setAnnex1($Annex1) {
        $this->Annex1 = $Annex1;
    }

    function setAnnex2($Annex2) {
        $this->Annex2 = $Annex2;
    }

    function setGold($Gold) {
        $this->Gold = $Gold;
    }

    function setMoney($Money) {
        $this->Money = $Money;
    }

    function setIsExist($IsExist) {
        $this->IsExist = $IsExist;
    }

    function setType($Type) {
        $this->Type = $Type;
    }

    function setValidDate($ValidDate) {
        $this->ValidDate = $ValidDate;
    }

    function setIsRead($IsRead) {
        $this->IsRead = $IsRead;
    }

    function setSendTime($SendTime) {
        $this->SendTime = $SendTime;
    }

    function setAnnex1Name($Annex1Name) {
        $this->Annex1Name = $Annex1Name;
    }

    function setAnnex2Name($Annex2Name) {
        $this->Annex2Name = $Annex2Name;
    }

    function setAnnex3($Annex3) {
        $this->Annex3 = $Annex3;
    }

    function setAnnex4($Annex4) {
        $this->Annex4 = $Annex4;
    }

    function setAnnex3Name($Annex3Name) {
        $this->Annex3Name = $Annex3Name;
    }

    function setAnnex4Name($Annex4Name) {
        $this->Annex4Name = $Annex4Name;
    }

    function setAnnex5Name($Annex5Name) {
        $this->Annex5Name = $Annex5Name;
    }

    function setAnnexRemark($AnnexRemark) {
        $this->AnnexRemark = $AnnexRemark;
    }

    function setGiftToken($GiftToken) {
        $this->GiftToken = $GiftToken;
    }

}
