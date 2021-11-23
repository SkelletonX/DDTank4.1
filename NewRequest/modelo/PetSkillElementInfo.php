<?php
class PetSkillElementInfo
{
	private $ID, $Name, $EffectPic, $Description, $Pic;

	function getID()
	{
		return $this->ID;
	}
	function getName()
	{
		return $this->Name;
	}
	function getEffectPic()
	{
		return $this->EffectPic;
	}
	function getDescription()
	{
		return $this->Description;
	}
	function getPic()
	{
		return $this->Pic;
	}
	function setID($ID)
	{
		$this->ID = $ID;
	}
	function setName($Name)
	{
		$this->Name = $Name;
	}
	function setEffectPic($EffectPic)
	{
		$this->EffectPic = $EffectPic;
	}
	function setDescription($Description)
	{
		$this->Description = $Description;
	}
	function setPic($Pic)
	{
		$this->Pic = $Pic;
	}
}
?>