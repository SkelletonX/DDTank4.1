<?php
/**
 * Created by PhpStorm.
 * User: jvbor
 * Date: 21/01/2019
 * Time: 00:26
 */

class MountDrawTemplate{
    private $ID,$TemplateId,$AddHurt,$AddGuard,$MagicAttack,$MagicDefence,$AddBlood,$Name;

    /**
     * @return mixed
     */
    public function getID()
    {
        return $this->ID;
    }

    /**
     * @param mixed $ID
     */
    public function setID($ID)
    {
        $this->ID = $ID;
    }

    /**
     * @return mixed
     */
    public function getTemplateId()
    {
        return $this->TemplateId;
    }

    /**
     * @param mixed $TemplateId
     */
    public function setTemplateId($TemplateId)
    {
        $this->TemplateId = $TemplateId;
    }

    /**
     * @return mixed
     */
    public function getAddHurt()
    {
        return $this->AddHurt;
    }

    /**
     * @param mixed $AddHurt
     */
    public function setAddHurt($AddHurt)
    {
        $this->AddHurt = $AddHurt;
    }

    /**
     * @return mixed
     */
    public function getAddGuard()
    {
        return $this->AddGuard;
    }

    /**
     * @param mixed $AddGuard
     */
    public function setAddGuard($AddGuard)
    {
        $this->AddGuard = $AddGuard;
    }

    /**
     * @return mixed
     */
    public function getMagicAttack()
    {
        return $this->MagicAttack;
    }

    /**
     * @param mixed $MagicAttack
     */
    public function setMagicAttack($MagicAttack)
    {
        $this->MagicAttack = $MagicAttack;
    }

    /**
     * @return mixed
     */
    public function getMagicDefence()
    {
        return $this->MagicDefence;
    }

    /**
     * @param mixed $MagicDefence
     */
    public function setMagicDefence($MagicDefence)
    {
        $this->MagicDefence = $MagicDefence;
    }

    /**
     * @return mixed
     */
    public function getAddBlood()
    {
        return $this->AddBlood;
    }

    /**
     * @param mixed $AddBlood
     */
    public function setAddBlood($AddBlood)
    {
        $this->AddBlood = $AddBlood;
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



}