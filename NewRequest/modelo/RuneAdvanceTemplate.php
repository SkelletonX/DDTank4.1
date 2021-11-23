<?php

/**
 * Created by PhpStorm.
 * User: jvbor
 * Date: 22/01/2019
 * Time: 03:18
 */
class RuneAdvanceTemplate
{
    private $AdvancedTempId, $RuneName, $MainMaterials, $Quality, $MaxLevelTempRunId, $AuxiliaryMaterials, $AdvanceDesc;

    /**
     * @return mixed
     */
    public function getAdvancedTempId()
    {
        return $this->AdvancedTempId;
    }

    /**
     * @param mixed $AdvancedTempId
     */
    public function setAdvancedTempId($AdvancedTempId)
    {
        $this->AdvancedTempId = $AdvancedTempId;
    }

    /**
     * @return mixed
     */
    public function getRuneName()
    {
        return $this->RuneName;
    }

    /**
     * @param mixed $RuneName
     */
    public function setRuneName($RuneName)
    {
        $this->RuneName = $RuneName;
    }

    /**
     * @return mixed
     */
    public function getMainMaterials()
    {
        return $this->MainMaterials;
    }

    /**
     * @param mixed $MainMaterials
     */
    public function setMainMaterials($MainMaterials)
    {
        $this->MainMaterials = $MainMaterials;
    }

    /**
     * @return mixed
     */
    public function getQuality()
    {
        return $this->Quality;
    }

    /**
     * @param mixed $Quality
     */
    public function setQuality($Quality)
    {
        $this->Quality = $Quality;
    }

    /**
     * @return mixed
     */
    public function getMaxLevelTempRunId()
    {
        return $this->MaxLevelTempRunId;
    }

    /**
     * @param mixed $MaxLevelTempRunId
     */
    public function setMaxLevelTempRunId($MaxLevelTempRunId)
    {
        $this->MaxLevelTempRunId = $MaxLevelTempRunId;
    }

    /**
     * @return mixed
     */
    public function getAuxiliaryMaterials()
    {
        return $this->AuxiliaryMaterials;
    }

    /**
     * @param mixed $AuxiliaryMaterials
     */
    public function setAuxiliaryMaterials($AuxiliaryMaterials)
    {
        $this->AuxiliaryMaterials = $AuxiliaryMaterials;
    }

    /**
     * @return mixed
     */
    public function getAdvanceDesc()
    {
        return $this->AdvanceDesc;
    }

    /**
     * @param mixed $AdvanceDesc
     */
    public function setAdvanceDesc($AdvanceDesc)
    {
        $this->AdvanceDesc = $AdvanceDesc;
    }


}