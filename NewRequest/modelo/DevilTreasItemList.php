<?php
class DevilTreasItemList
{
    private $ID, $Type, $TemplateID, $Value;

    /**
     * Get the value of ID
     */ 
    public function getID()
    {
        return $this->ID;
    }

    /**
     * Set the value of ID
     *
     * @return  self
     */ 
    public function setID($ID)
    {
        $this->ID = $ID;

        return $this;
    }

    /**
     * Get the value of Type
     */ 
    public function getType()
    {
        return $this->Type;
    }

    /**
     * Set the value of Type
     *
     * @return  self
     */ 
    public function setType($Type)
    {
        $this->Type = $Type;

        return $this;
    }

    /**
     * Get the value of TemplateID
     */ 
    public function getTemplateID()
    {
        return $this->TemplateID;
    }

    /**
     * Set the value of TemplateID
     *
     * @return  self
     */ 
    public function setTemplateID($TemplateID)
    {
        $this->TemplateID = $TemplateID;

        return $this;
    }

    /**
     * Get the value of Value
     */ 
    public function getValue()
    {
        return $this->Value;
    }

    /**
     * Set the value of Value
     *
     * @return  self
     */ 
    public function setValue($Value)
    {
        $this->Value = $Value;

        return $this;
    }
}
?>