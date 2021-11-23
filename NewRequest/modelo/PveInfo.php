<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of PveInfo
 *
 * @author SYSTEM
 */
class PveInfo
{

    //put your code here
    private $ID, $Name, $Type, $LevelLimits, $SimpleTemplateIds, $NormalTemplateIds, $HardTemplateIds, $TerrorTemplateIds, $NightmareTemplateIds;
    private $EpicTemplateIds, $Pic, $Description, $Ordering, $AdviceTips, $BossFightNeedMoney, $FightPower, $Missions;

    /**
     * @return mixed
     */
    public function getNightmareTemplateIds()
    {
        return $this->NightmareTemplateIds;
    }

    /**
     * @param mixed $NightmareTemplateIds
     */
    public function setNightmareTemplateIds($NightmareTemplateIds)
    {
        $this->NightmareTemplateIds = $NightmareTemplateIds;
    }


    function getID()
    {
        return $this->ID;
    }

    function getName()
    {
        return $this->Name;
    }

    function getType()
    {
        return $this->Type;
    }

    function getLevelLimits()
    {
        return $this->LevelLimits;
    }

    function getSimpleTemplateIds()
    {
        return $this->SimpleTemplateIds;
    }

    function getNormalTemplateIds()
    {
        return $this->NormalTemplateIds;
    }

    function getHardTemplateIds()
    {
        return $this->HardTemplateIds;
    }

    function getTerrorTemplateIds()
    {
        return $this->TerrorTemplateIds;
    }

    function getEpicTemplateIds()
    {
        return $this->EpicTemplateIds;
    }

    function getPic()
    {
        return $this->Pic;
    }

    function getDescription()
    {
        return $this->Description;
    }

    function getOrdering()
    {
        return $this->Ordering;
    }

    function getAdviceTips()
    {
        return $this->AdviceTips;
    }

    function getBossFightNeedMoney()
    {
        return $this->BossFightNeedMoney;
    }

    function getFightPower()
    {
        return $this->FightPower;
    }

    function getMission()
    {
        return $this->Missions;
    }

    function setID($ID)
    {
        $this->ID = $ID;
    }

    function setName($Name)
    {
        $this->Name = $Name;
    }

    function setType($Type)
    {
        $this->Type = $Type;
    }

    function setLevelLimits($LevelLimits)
    {
        $this->LevelLimits = $LevelLimits;
    }

    function setSimpleTemplateIds($SimpleTemplateIds)
    {
        $this->SimpleTemplateIds = $SimpleTemplateIds;
    }

    function setNormalTemplateIds($NormalTemplateIds)
    {
        $this->NormalTemplateIds = $NormalTemplateIds;
    }

    function setHardTemplateIds($HardTemplateIds)
    {
        $this->HardTemplateIds = $HardTemplateIds;
    }

    function setTerrorTemplateIds($TerrorTemplateIds)
    {
        $this->TerrorTemplateIds = $TerrorTemplateIds;
    }

    function setEpicTemplateIds($EpicTemplateIds)
    {
        $this->EpicTemplateIds = $EpicTemplateIds;
    }

    function setPic($Pic)
    {
        $this->Pic = $Pic;
    }

    function setDescription($Description)
    {
        $this->Description = $Description;
    }

    function setOrdering($Ordering)
    {
        $this->Ordering = $Ordering;
    }

    function setAdviceTips($AdviceTips)
    {
        $this->AdviceTips = $AdviceTips;
    }

    function setBossFightNeedMoney($BossFightNeedMoney)
    {
        $this->BossFightNeedMoney = $BossFightNeedMoney;
    }

    function setFightPower($FightPower)
    {
        $this->FightPower = $FightPower;
    }

    function setMissions($Missions)
    {
        $this->Missions = $Missions;
    }

}
