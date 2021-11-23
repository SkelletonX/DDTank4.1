<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

require_once './Crypt/RSA.php';

class serverlist
{

    //put your code here
    //put your code here
    private $parametros;

    function __construct($parametros)
    {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar()
    {
        $RealAddress = "127.0.0.1";
        $RealPort = "8678";
        if(empty($_GET['online']))
        {
            header("Content-type: text/xml");
            echo '<Result value="true" message="Success!" total="0" agentId="1" AreaName="gn">';
            for($i = 0; $i < 500; $i++)
            {
                if($i == 4)
                {
                    echo '<Item ID="4" Name="DDtank 10.6" IP="'. $RealAddress .'" Port="'. $RealPort .'" State="2" MustLevel="100" LowestLevel="0" Online="0" Remark=""/>';
                }
                else
                {
                    $PortRand = rand(8000, 10000);
                    $FirstNumber = rand(1, 255);
                    $SecondNumber = rand(1, 255);
                    $ThreeNumber = rand(1, 255);
                    $FourNumber = rand(1, 255);
                    $GeneraterAddress = $FirstNumber.".".$SecondNumber.".".$ThreeNumber.".".$FourNumber;
                    echo '<Item ID="'. $i .'" Name="DDtank 10.6" IP="'. $GeneraterAddress .'" Port="'. $PortRand .'" State="0" MustLevel="100" LowestLevel="0" Online="0" Remark=""/>';
                }
            }
            echo '</Result>';
        }
        else
        {
            include_once("phpinfo.php");
        }
    }
}
