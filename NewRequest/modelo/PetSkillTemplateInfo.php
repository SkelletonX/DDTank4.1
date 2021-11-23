<?php
class PetSkilLTemplateInfo
{
	private $PetTemplateID, $KindID, $GetType, $SkillID, $SkillBookID, $MinLevel, $DeleteSkillIDs;

	function getPetTemplateID()
	{
		return $this->PetTemplateID;
	}
	function getKindID()
	{
		return $this->KindID;
	}
	function getGetType()
	{
		return $this->GetType;
	}
	function getSkillID()
	{
		return $this->SkillID;
	}
	function getSkillBookID()
	{
		return $this->SkillBookID;
	}
	function getMinLevel()
	{
		return $this->MinLevel;
	}
	function getDeleteSkillIDs()
	{
		return $this->DeleteSkillIDs;
	}
	function setPetTemplateID($PetTemplateID)
	{
		$this->PetTemplateID = $PetTemplateID;
	}
	function setKindID($KindID)
	{
		$this->KindID = $KindID;
	}
	function setGetType($GetType)
	{
		$this->GetType = $GetType;
	}
	function setSkillID($SkillID)
	{
		$this->SkillID = $SkillID;
	}
	function setSkillBookID($SkillBookID)
	{
		$this->SkillBookID = $SkillBookID;
	}
	function setMinLevel($MinLevel)
	{
		$this->MinLevel = $MinLevel;
	}
	function setDeleteSkillIDs($DeleteSkillIDs)
	{
		$this->DeleteSkillIDs = $DeleteSkillIDs;
	}
}
?>