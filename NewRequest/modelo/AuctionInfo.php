<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of AuctionInfo
 *
 * @author jvbor
 */
class AuctionInfo {

    //put your code here
    private $AuctionID, $Name, $Category, $AuctioneerID, $AuctioneerName, $ItemID, $PayType, $Price;
    private $Rise, $Mouthful, $BeginDate, $ValidDate, $BuyerID, $BuyerName, $IsExist, $TemplateID;
    private $Random, $goodsCount;

    function getAuctionID() {
        return $this->AuctionID;
    }

    function getName() {
        return $this->Name;
    }

    function getCategory() {
        return $this->Category;
    }

    function getAuctioneerID() {
        return $this->AuctioneerID;
    }

    function getAuctioneerName() {
        return $this->AuctioneerName;
    }

    function getItemID() {
        return $this->ItemID;
    }

    function getPayType() {
        return $this->PayType;
    }

    function getPrice() {
        return $this->Price;
    }

    function getRise() {
        return $this->Rise;
    }

    function getMouthful() {
        return $this->Mouthful;
    }

    function getBeginDate() {
        return $this->BeginDate;
    }

    function getValidDate() {
        return $this->ValidDate;
    }

    function getBuyerID() {
        return $this->BuyerID;
    }

    function getBuyerName() {
        return $this->BuyerName;
    }

    function getIsExist() {
        return $this->IsExist;
    }

    function getTemplateID() {
        return $this->TemplateID;
    }

    function getRandom() {
        return $this->Random;
    }

    function getGoodsCount() {
        return $this->goodsCount;
    }

    function setAuctionID($AuctionID) {
        $this->AuctionID = $AuctionID;
    }

    function setName($Name) {
        $this->Name = $Name;
    }

    function setCategory($Category) {
        $this->Category = $Category;
    }

    function setAuctioneerID($AuctioneerID) {
        $this->AuctioneerID = $AuctioneerID;
    }

    function setAuctioneerName($AuctioneerName) {
        $this->AuctioneerName = $AuctioneerName;
    }

    function setItemID($ItemID) {
        $this->ItemID = $ItemID;
    }

    function setPayType($PayType) {
        $this->PayType = $PayType;
    }

    function setPrice($Price) {
        $this->Price = $Price;
    }

    function setRise($Rise) {
        $this->Rise = $Rise;
    }

    function setMouthful($Mouthful) {
        $this->Mouthful = $Mouthful;
    }

    function setBeginDate($BeginDate) {
        $this->BeginDate = $BeginDate;
    }

    function setValidDate($ValidDate) {
        $this->ValidDate = $ValidDate;
    }

    function setBuyerID($BuyerID) {
        $this->BuyerID = $BuyerID;
    }

    function setBuyerName($BuyerName) {
        $this->BuyerName = $BuyerName;
    }

    function setIsExist($IsExist) {
        $this->IsExist = $IsExist;
    }

    function setTemplateID($TemplateID) {
        $this->TemplateID = $TemplateID;
    }

    function setRandom($Random) {
        $this->Random = $Random;
    }

    function setGoodsCount($goodsCount) {
        $this->goodsCount = $goodsCount;
    }

}
