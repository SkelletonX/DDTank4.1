<?php
/**
 * @author Jefferson Ataa
 * Evento do Capeta
*/
class DevilTreasPointsList
{
    private $ID, $Points, $TemplateID;

    /**
     * Get the value of ID
     */ 
    function getID()
    {
        return $this->ID;
    }

    /**
     * Set the value of ID
     *
     * @return  self
     */ 
    function setID($ID)
    {
        $this->ID = $ID;

        return $this;
    }

    /**
     * Get the value of Points
     */ 
    function getPoints()
    {
        return $this->Points;
    }

    /**
     * Set the value of Points
     *
     * @return  self
     */ 
    function setPoints($Points)
    {
        $this->Points = $Points;

        return $this;
    }

    /**
     * Get the value of TemplateID
     */ 
    function getTemplateID()
    {
        return $this->TemplateID;
    }

    /**
     * Set the value of TemplateID
     *
     * @return  self
     */ 
    function setTemplateID($TemplateID)
    {
        $this->TemplateID = $TemplateID;

        return $this;
    }
}
?>