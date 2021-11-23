<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of loginselectlist
 *
 * @author jvbor
 */
class loginselectlist {

    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        header("Content-type: text/xml");
        $controle = new ControlePlayers();
        $players = $controle->GetUserLoginList($this->parametros['username']);
        $xml = XMLSerializer::generateValidXmlFromArray($players);
        echo $xml;
    }

}
