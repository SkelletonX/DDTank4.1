<?php
/**
 * Created by PhpStorm.
 * User: jvbor
 * Date: 10/03/2019
 * Time: 18:52
 */

class MagicStoneInfo
{
    private $ID, $TemplateID, $Level, $Exp, $Attack, $Defence, $Agility, $Luck, $MagicAttack, $MagicDefence;

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
    public function getLevel()
    {
        return $this->Level;
    }

    /**
     * @param mixed $Level
     */
    public function setLevel($Level)
    {
        $this->Level = $Level;
    }

    /**
     * @return mixed
     */
    public function getExp()
    {
        return $this->Exp;
    }

    /**
     * @param mixed $Exp
     */
    public function setExp($Exp)
    {
        $this->Exp = $Exp;
    }

    /**
     * @return mixed
     */
    public function getAttack()
    {
        return $this->Attack;
    }

    /**
     * @param mixed $Attack
     */
    public function setAttack($Attack)
    {
        $this->Attack = $Attack;
    }

    /**
     * @return mixed
     */
    public function getDefence()
    {
        return $this->Defence;
    }

    /**
     * @param mixed $Defence
     */
    public function setDefence($Defence)
    {
        $this->Defence = $Defence;
    }

    /**
     * @return mixed
     */
    public function getAgility()
    {
        return $this->Agility;
    }

    /**
     * @param mixed $Agility
     */
    public function setAgility($Agility)
    {
        $this->Agility = $Agility;
    }

    /**
     * @return mixed
     */
    public function getLuck()
    {
        return $this->Luck;
    }

    /**
     * @param mixed $Luck
     */
    public function setLuck($Luck)
    {
        $this->Luck = $Luck;
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


}