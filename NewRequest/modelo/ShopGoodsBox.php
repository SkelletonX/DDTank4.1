<?php
/**
 * Created by PhpStorm.
 * User: jvbor
 * Date: 21/01/2019
 * Time: 00:58
 */
class ShopGoodsBox{
    private $Id,$DataId,$TemplateId,$IsSelect,$IsBind,$ItemValid,$ItemCount;
    private $StrengthenLevel,$AttackCompose,$DefendCompose,$AgilityCompose;
    private $LuckCompose,$MagicAttack,$MagicDefence,$Random;
    private $IsTips,$IsLogs,$addDate;

    /**
     * @return mixed
     */
    public function getId()
    {
        return $this->Id;
    }

    /**
     * @param mixed $Id
     */
    public function setId($Id)
    {
        $this->Id = $Id;
    }

    /**
     * @return mixed
     */
    public function getDataId()
    {
        return $this->DataId;
    }

    /**
     * @param mixed $DataId
     */
    public function setDataId($DataId)
    {
        $this->DataId = $DataId;
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
    public function getisSelect()
    {
        return $this->IsSelect;
    }

    /**
     * @param mixed $IsSelect
     */
    public function setIsSelect($IsSelect)
    {
        $this->IsSelect = $IsSelect;
    }

    /**
     * @return mixed
     */
    public function getisBind()
    {
        return $this->IsBind;
    }

    /**
     * @param mixed $IsBind
     */
    public function setIsBind($IsBind)
    {
        $this->IsBind = $IsBind;
    }

    /**
     * @return mixed
     */
    public function getItemValid()
    {
        return $this->ItemValid;
    }

    /**
     * @param mixed $ItemValid
     */
    public function setItemValid($ItemValid)
    {
        $this->ItemValid = $ItemValid;
    }

    /**
     * @return mixed
     */
    public function getItemCount()
    {
        return $this->ItemCount;
    }

    /**
     * @param mixed $ItemCount
     */
    public function setItemCount($ItemCount)
    {
        $this->ItemCount = $ItemCount;
    }

    /**
     * @return mixed
     */
    public function getStrengthenLevel()
    {
        return $this->StrengthenLevel;
    }

    /**
     * @param mixed $StrengthenLevel
     */
    public function setStrengthenLevel($StrengthenLevel)
    {
        $this->StrengthenLevel = $StrengthenLevel;
    }

    /**
     * @return mixed
     */
    public function getAttackCompose()
    {
        return $this->AttackCompose;
    }

    /**
     * @param mixed $AttackCompose
     */
    public function setAttackCompose($AttackCompose)
    {
        $this->AttackCompose = $AttackCompose;
    }

    /**
     * @return mixed
     */
    public function getDefendCompose()
    {
        return $this->DefendCompose;
    }

    /**
     * @param mixed $DefendCompose
     */
    public function setDefendCompose($DefendCompose)
    {
        $this->DefendCompose = $DefendCompose;
    }

    /**
     * @return mixed
     */
    public function getAgilityCompose()
    {
        return $this->AgilityCompose;
    }

    /**
     * @param mixed $AgilityCompose
     */
    public function setAgilityCompose($AgilityCompose)
    {
        $this->AgilityCompose = $AgilityCompose;
    }

    /**
     * @return mixed
     */
    public function getLuckCompose()
    {
        return $this->LuckCompose;
    }

    /**
     * @param mixed $LuckCompose
     */
    public function setLuckCompose($LuckCompose)
    {
        $this->LuckCompose = $LuckCompose;
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
    public function getRandom()
    {
        return $this->Random;
    }

    /**
     * @param mixed $Random
     */
    public function setRandom($Random)
    {
        $this->Random = $Random;
    }

    /**
     * @return mixed
     */
    public function getisTips()
    {
        return $this->IsTips;
    }

    /**
     * @param mixed $IsTips
     */
    public function setIsTips($IsTips)
    {
        $this->IsTips = $IsTips;
    }

    /**
     * @return mixed
     */
    public function getisLogs()
    {
        return $this->IsLogs;
    }

    /**
     * @param mixed $IsLogs
     */
    public function setIsLogs($IsLogs)
    {
        $this->IsLogs = $IsLogs;
    }

    /**
     * @return mixed
     */
    public function getAddDate()
    {
        return $this->addDate;
    }

    /**
     * @param mixed $addDate
     */
    public function setAddDate($addDate)
    {
        $this->addDate = $addDate;
    }


}