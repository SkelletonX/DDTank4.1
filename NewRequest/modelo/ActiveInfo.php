<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ActiveInfo
 *
 * @author jvbor
 */
class ActiveInfo {

    //put your code here
    private $ActiveID, $Title, $Description, $Content, $AwardContent, $HasKey, $StartDate, $EndDate, $IsOnly, $Type, $ActionTimeContent, $IsAdvance, $GoodsExchangeType;
    private $GoodsExchangeNum, $limitType, $limitValue, $IsShow, $IconID, $ActiveType;

    function getActiveID() {
        return $this->ActiveID;
    }

    function getTitle() {
        return $this->Title;
    }

    function getDescription() {
        return $this->Description;
    }

    function getContent() {
        return $this->Content;
    }

    function getAwardContent() {
        return $this->AwardContent;
    }

    function getHasKey() {
        return $this->HasKey;
    }

    function getStartDate() {
        return $this->StartDate;
    }

    function getEndDate() {
        return $this->EndDate;
    }

    function getIsOnly() {
        return $this->IsOnly;
    }

    function getType() {
        return $this->Type;
    }

    function getActionTimeContent() {
        return $this->ActionTimeContent;
    }

    function getIsAdvance() {
        return $this->IsAdvance;
    }

    function getGoodsExchangeType() {
        return $this->GoodsExchangeType;
    }

    function getGoodsExchangeNum() {
        return $this->GoodsExchangeNum;
    }

    function getLimitType() {
        return $this->limitType;
    }

    function getLimitValue() {
        return $this->limitValue;
    }

    function getIsShow() {
        return $this->IsShow;
    }

    function getIconID() {
        return $this->IconID;
    }

    function getActiveType() {
        return $this->ActiveType;
    }

    function setActiveID($ActiveID) {
        $this->ActiveID = $ActiveID;
    }

    function setTitle($Title) {
        $this->Title = $Title;
    }

    function setDescription($Description) {
        $this->Description = $Description;
    }

    function setContent($Content) {
        $this->Content = $Content;
    }

    function setAwardContent($AwardContent) {
        $this->AwardContent = $AwardContent;
    }

    function setHasKey($HasKey) {
        $this->HasKey = $HasKey;
    }

    function setStartDate($StartDate) {
        $this->StartDate = $StartDate;
    }

    function setEndDate($EndDate) {
        $this->EndDate = $EndDate;
    }

    function setIsOnly($IsOnly) {
        $this->IsOnly = $IsOnly;
    }

    function setType($Type) {
        $this->Type = $Type;
    }

    function setActionTimeContent($ActionTimeContent) {
        $this->ActionTimeContent = $ActionTimeContent;
    }

    function setIsAdvance($IsAdvance) {
        $this->IsAdvance = $IsAdvance;
    }

    function setGoodsExchangeType($GoodsExchangeType) {
        $this->GoodsExchangeType = $GoodsExchangeType;
    }

    function setGoodsExchangeNum($GoodsExchangeNum) {
        $this->GoodsExchangeNum = $GoodsExchangeNum;
    }

    function setLimitType($limitType) {
        $this->limitType = $limitType;
    }

    function setLimitValue($limitValue) {
        $this->limitValue = $limitValue;
    }

    function setIsShow($IsShow) {
        $this->IsShow = $IsShow;
    }

    function setIconID($IconID) {
        $this->IconID = $IconID;
    }

    function setActiveType($ActiveType) {
        $this->ActiveType = $ActiveType;
    }

}
