<?php
class ItemFusion
{
	private $FusionID, $Item1, $Item2, $Item3, $Item4, $Count1, $Count2, $Count3, $Count4, $Formula, $FusionRate, $FusionType, $Reward, $NeedPower;

	function getFusionID()
    {
        return $this->FusionID;
    }

    function getItem1()
    {
    	return $this->Item1;
    }

    function getItem2()
    {
    	return $this->Item2;
    }

    function getItem3()
    {
    	return $this->Item3;
    }

    function getItem4()
    {
    	return $this->Item4;
    }

    function getCount1()
    {
    	return $this->Count1;
    }

    function getCount2()
    {
    	return $this->Count2;
    }

    function getCount3()
    {
    	return $this->Count3;
    }

    function getCount4()
    {
    	return $this->Count4;
    }

    function getFormula()
    {
    	return $this->Formula;
    }

    function getFusionRate()
    {
    	return $this->FusionRate;
    }

    //private $FusionID, $Item1, $Item2, $Item3, $Item4, $Count1, $Count2, $Count3, $Count4, $Formula, $FusionRate, $FusionType, $Reward, $NeedPower;

    function getFusionType()
    {
    	return $this->FusionType;
    }

    function getReward()
    {
    	return $this->Reward;
    }

    function getNeedPower()
    {
    	return $this->NeedPower;
    }

    //OK

    function setFusionID($FusionID)
    {
        $this->FusionID = $FusionID;
    }

    function setItem1($Item1)
    {
    	$this->Item1 = $Item1;
    }

    function setItem2($Item2)
    {
    	$this->Item2 = $Item2;
    }

    function setItem3($Item3)
    {
    	$this->Item3 = $Item3;
    }

    function setItem4($Item4)
    {
    	$this->Item4 = $Item4;
    }

    function setCount1($Count1)
    {
    	$this->Count1 = $Count1;
    }

    function setCount2($Count2)
    {
    	$this->Count2 = $Count2;
    }

    function setCount3($Count3)
    {
    	$this->Count3 = $Count3;
    }

    function setCount4($Count4)
    {
    	$this->Count4 = $Count4;
    }

    function setFormula($Formula)
    {
    	$this->Formula = $Formula;
    }

    function setFusionRate($FusionRate)
    {
    	$this->FusionRate = $FusionRate;
    }

    function setFusionType($FusionType)
    {
    	$this->FusionType = $FusionType;
    }

    function setReward($Reward)
    {
    	 $this->Reward = $Reward;
    }

    function setNeedPower($NeedPower)
    {
    	 $this->NeedPower = $NeedPower;
    }



}
?>