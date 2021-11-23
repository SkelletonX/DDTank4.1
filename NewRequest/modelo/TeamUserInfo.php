<?php
class TeamUserInfo
{
    private $UserID, $NickName, $Grade, $State, $LastDate, $typeVIP, $VIPLevel;
    private $OldPlayer = 0, $ActiveScore = 0, $ActiveWeekScore = 0, $ActiveTotalScore, $ActiveSeasonScore;
    private $BattleScore = 0, $BattleNum = 0, $FightPower, $DdtKingLevel = 0, $Rank = 0, $Offer;
    private $WinCount = 0, $EscapeCount = 0, $TotalCount = 0, $IsSignToday, $Sex, $IsCaption;


    /**
     * Get the value of NickName
     */ 
    public function getNickName()
    {
        return $this->NickName;
    }

    /**
     * Set the value of NickName
     *
     * @return  self
     */ 
    public function setNickName($NickName)
    {
        $this->NickName = $NickName;

        return $this;
    }

    /**
     * Get the value of Grade
     */ 
    public function getGrade()
    {
        return $this->Grade;
    }

    /**
     * Set the value of Grade
     *
     * @return  self
     */ 
    public function setGrade($Grade)
    {
        $this->Grade = $Grade;

        return $this;
    }

    /**
     * Get the value of State
     */ 
    public function getState()
    {
        return $this->State;
    }

    /**
     * Set the value of State
     *
     * @return  self
     */ 
    public function setState($State)
    {
        $this->State = $State;

        return $this;
    }

    /**
     * Get the value of LastDate
     */ 
    public function getLastDate()
    {
        return $this->LastDate;
    }

    /**
     * Set the value of LastDate
     *
     * @return  self
     */ 
    public function setLastDate($LastDate)
    {
        $this->LastDate = $LastDate;

        return $this;
    }

    /**
     * Get the value of typeVIP
     */ 
    public function getTypeVIP()
    {
        return $this->typeVIP;
    }

    /**
     * Set the value of typeVIP
     *
     * @return  self
     */ 
    public function setTypeVIP($typeVIP)
    {
        $this->typeVIP = $typeVIP;

        return $this;
    }

    /**
     * Get the value of VIPLevel
     */ 
    public function getVIPLevel()
    {
        return $this->VIPLevel;
    }

    /**
     * Set the value of VIPLevel
     *
     * @return  self
     */ 
    public function setVIPLevel($VIPLevel)
    {
        $this->VIPLevel = $VIPLevel;

        return $this;
    }

    /**
     * Get the value of OldPlayer
     */ 
    public function getOldPlayer()
    {
        return $this->OldPlayer;
    }

    /**
     * Set the value of OldPlayer
     *
     * @return  self
     */ 
    public function setOldPlayer($OldPlayer)
    {
        $this->OldPlayer = $OldPlayer;

        return $this;
    }

    /**
     * Get the value of ActiveScore
     */ 
    public function getActiveScore()
    {
        return $this->ActiveScore;
    }

    /**
     * Set the value of ActiveScore
     *
     * @return  self
     */ 
    public function setActiveScore($ActiveScore)
    {
        $this->ActiveScore = $ActiveScore;

        return $this;
    }

    /**
     * Get the value of ActiveWeekScore
     */ 
    public function getActiveWeekScore()
    {
        return $this->ActiveWeekScore;
    }

    /**
     * Set the value of ActiveWeekScore
     *
     * @return  self
     */ 
    public function setActiveWeekScore($ActiveWeekScore)
    {
        $this->ActiveWeekScore = $ActiveWeekScore;

        return $this;
    }

    /**
     * Get the value of ActiveTotalScore
     */ 
    public function getActiveTotalScore()
    {
        return $this->ActiveTotalScore;
    }

    /**
     * Set the value of ActiveTotalScore
     *
     * @return  self
     */ 
    public function setActiveTotalScore($ActiveTotalScore)
    {
        $this->ActiveTotalScore = $ActiveTotalScore;

        return $this;
    }

    /**
     * Get the value of ActiveSeasonScore
     */ 
    public function getActiveSeasonScore()
    {
        return $this->ActiveSeasonScore;
    }

    /**
     * Set the value of ActiveSeasonScore
     *
     * @return  self
     */ 
    public function setActiveSeasonScore($ActiveSeasonScore)
    {
        $this->ActiveSeasonScore = $ActiveSeasonScore;

        return $this;
    }

    /**
     * Get the value of BattleScore
     */ 
    public function getBattleScore()
    {
        return $this->BattleScore;
    }

    /**
     * Set the value of BattleScore
     *
     * @return  self
     */ 
    public function setBattleScore($BattleScore)
    {
        $this->BattleScore = $BattleScore;

        return $this;
    }

    /**
     * Get the value of BattleNum
     */ 
    public function getBattleNum()
    {
        return $this->BattleNum;
    }

    /**
     * Set the value of BattleNum
     *
     * @return  self
     */ 
    public function setBattleNum($BattleNum)
    {
        $this->BattleNum = $BattleNum;

        return $this;
    }

    /**
     * Get the value of FightPower
     */ 
    public function getFightPower()
    {
        return $this->FightPower;
    }

    /**
     * Set the value of FightPower
     *
     * @return  self
     */ 
    public function setFightPower($FightPower)
    {
        $this->FightPower = $FightPower;

        return $this;
    }

    /**
     * Get the value of DdtKingLevel
     */ 
    public function getDdtKingLevel()
    {
        return $this->DdtKingLevel;
    }

    /**
     * Set the value of DdtKingLevel
     *
     * @return  self
     */ 
    public function setDdtKingLevel($DdtKingLevel)
    {
        $this->DdtKingLevel = $DdtKingLevel;

        return $this;
    }

    /**
     * Get the value of Rank
     */ 
    public function getRank()
    {
        return $this->Rank;
    }

    /**
     * Set the value of Rank
     *
     * @return  self
     */ 
    public function setRank($Rank)
    {
        $this->Rank = $Rank;

        return $this;
    }

    /**
     * Get the value of Offer
     */ 
    public function getOffer()
    {
        return $this->Offer;
    }

    /**
     * Set the value of Offer
     *
     * @return  self
     */ 
    public function setOffer($Offer)
    {
        $this->Offer = $Offer;

        return $this;
    }

    /**
     * Get the value of WinCount
     */ 
    public function getWinCount()
    {
        return $this->WinCount;
    }

    /**
     * Set the value of WinCount
     *
     * @return  self
     */ 
    public function setWinCount($WinCount)
    {
        $this->WinCount = $WinCount;

        return $this;
    }

    /**
     * Get the value of EscapeCount
     */ 
    public function getEscapeCount()
    {
        return $this->EscapeCount;
    }

    /**
     * Set the value of EscapeCount
     *
     * @return  self
     */ 
    public function setEscapeCount($EscapeCount)
    {
        $this->EscapeCount = $EscapeCount;

        return $this;
    }

    /**
     * Get the value of TotalCount
     */ 
    public function getTotalCount()
    {
        return $this->TotalCount;
    }

    /**
     * Set the value of TotalCount
     *
     * @return  self
     */ 
    public function setTotalCount($TotalCount)
    {
        $this->TotalCount = $TotalCount;

        return $this;
    }

    /**
     * Get the value of IsSignToday
     */ 
    public function getIsSignToday()
    {
        return $this->IsSignToday;
    }

    /**
     * Set the value of IsSignToday
     *
     * @return  self
     */ 
    public function setIsSignToday($IsSignToday)
    {
        $this->IsSignToday = $IsSignToday;

        return $this;
    }

    /**
     * Get the value of Sex
     */ 
    public function getSex()
    {
        return $this->Sex;
    }

    /**
     * Set the value of Sex
     *
     * @return  self
     */ 
    public function setSex($Sex)
    {
        $this->Sex = $Sex;

        return $this;
    }

    /**
     * Get the value of IsCaption
     */ 
    public function getIsCaption()
    {
        return $this->IsCaption;
    }

    /**
     * Set the value of IsCaption
     *
     * @return  self
     */ 
    public function setIsCaption($IsCaption)
    {
        $this->IsCaption = $IsCaption;

        return $this;
    }
}
?>