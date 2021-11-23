<?php
/**
 * Created by PhpStorm.
 * User: jvbor
 * Date: 22/01/2019
 * Time: 02:39
 */
class PetFormData{
    private $TemplateID,$Appearance,$DamageRecude,$HeathUp,$Name,$Resource;

    /**
     * @return mixed
     */
    public function getTemplateID()
    {
        return $this->TemplateID;
    }

    /**
     * @param mixed $TemplateID
     */
    public function setTemplateID($TemplateID)
    {
        $this->TemplateID = $TemplateID;
    }

    /**
     * @return mixed
     */
    public function getAppearance()
    {
        return $this->Appearance;
    }

    /**
     * @param mixed $Appearance
     */
    public function setAppearance($Appearance)
    {
        $this->Appearance = $Appearance;
    }

    /**
     * @return mixed
     */
    public function getDamageRecude()
    {
        return $this->DamageRecude;
    }

    /**
     * @param mixed $DamageRecude
     */
    public function setDamageRecude($DamageRecude)
    {
        $this->DamageRecude = $DamageRecude;
    }

    /**
     * @return mixed
     */
    public function getHeathUp()
    {
        return $this->HeathUp;
    }

    /**
     * @param mixed $HeathUp
     */
    public function setHeathUp($HeathUp)
    {
        $this->HeathUp = $HeathUp;
    }

    /**
     * @return mixed
     */
    public function getName()
    {
        return $this->Name;
    }

    /**
     * @param mixed $Name
     */
    public function setName($Name)
    {
        $this->Name = $Name;
    }

    /**
     * @return mixed
     */
    public function getResource()
    {
        return $this->Resource;
    }

    /**
     * @param mixed $Resource
     */
    public function setResource($Resource)
    {
        $this->Resource = $Resource;
    }


}