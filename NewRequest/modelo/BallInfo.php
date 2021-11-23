<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of BallInfo
 *
 * @author JeffzAtaa
 */
class BallInfo {

    //put your code here
    private $ID, $Power, $Radii, $FlyingPartical, $BombPartical, $Crater, $AttackResponse, $IsSpin, $SpinV, $SpinVA, $Amout, $Win, $DragIndex, $Weight, $Shake, $ShootSound, $BombSound, $ActionType, $Mass, $BombType;

    function getID()
    {
        return $this->ID;
    }
    function getPower()
    {
        return $this->Power;
    }
    function getRadii()
    {
        return $this->Radii;
    }
    function getFlyingPartical()
    {
        return $this->FlyingPartical;
    }
    function getBombPartical()
    {
        return $this->BombPartical;
    }
    function getCrater()
    {
        return $this->Crater;
    }
    function getAttackResponse()
    {
        return $this->AttackResponse;
    }
    function getIsSpin()
    {
        return $this->IsSpin;
    }
    function getSpinV()
    {
        return $this->SpinV;
    }
    function getSpinVA()
    {
        return $this->SpinVA;
    }
    function getAmount()
    {
        return $this->Amount;
    }
    function getWind()
    {
        return $this->Wind;
    }
    function getDragIndex()
    {
        return $this->DragIndex;
    }
    function getWeight()
    {
        return $this->Weight;
    }
    function getShake()
    {
        return $this->Shake;
    }
    function getShootSound()
    {
        return $this->ShootSound;
    }
    function getBombSound()
    {
        return $this->BombSound;
    }
    function getActionType()
    {
        return $this->ActionType;
    }
    function getMass()
    {
        return $this->Mass;
    }
    function getBombType()
    {
        return $this->BombType;
    }
    function setID($ID)
    {
        $this->ID = $ID;
    }
    function setPower($Power)
    {
        $this->Power = $Power;
    }
    function setRadii($Radii)
    {
        $this->Radii = $Radii;
    }
    function setFlyingPartical($FlyingPartical)
    {
        $this->FlyingPartical = $FlyingPartical;
    }
    function setBombPartical($BombPartical)
    {
        $this->BombPartical = $BombPartical;
    }
    function setCrater($Crater)
    {
        $this->Crater = $Crater;
    }
    function setAttackResponse($AttackResponse)
    {
        $this->AttackResponse = $AttackResponse;
    }
    function setIsSpin($IsSpin)
    {
        $this->IsSpin = $IsSpin;
    }
    function setSpinV($SpinV)
    {
        $this->SpinV = $SpinV;
    }
    function setSpinVA($SpinVA)
    {
        $this->SpinVA = $SpinVA;
    }
    function setAmount($Amount)
    {
        $this->Amount = $Amount;
    }
    function setWind($Wind)
    {
        $this->Wind = $Wind;
    }
    function setDragIndex($DragIndex)
    {
        $this->DragIndex = $DragIndex;
    }
    function setWeight($Weight)
    {
        $this->Weight = $Weight;
    }
    function setShake($Shake)
    {
        $this->Shake = $Shake;
    }
    function setShootSound($ShootSound)
    {
        $this->ShootSound = $ShootSound;
    }
    function setBombSound($BombSound)
    {
        $this->BombSound = $BombSound;
    }
    function setActionType($ActionType)
    {
        $this->ActionType = $ActionType;
    }
    function setMass($Mass)
    {
        $this->Mass = $Mass;
    }
    function setBombType($BombType)
    {
        $this->BombType;
    }
}
