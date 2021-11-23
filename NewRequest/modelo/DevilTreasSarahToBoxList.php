<?php
class DevilTreasSarahToBoxList
{
    private $ID, $Exchange;

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
     * Get the value of Exchange
     */ 
    public function getExchange()
    {
        return $this->Exchange;
    }

    /**
     * Set the value of Exchange
     *
     * @return  self
     */ 
    public function setExchange($Exchange)
    {
        $this->Exchange = $Exchange;

        return $this;
    }
}
?>