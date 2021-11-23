<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of novicenodedata
 *
 * @author jvbor
 */
class novicenodedata {

    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        header("Content-type: text/xml");
        $rootNode = new SimpleXMLElement('<Result></Result>');
        $rootNode->addAttribute("value", "true");
        $rootNode->addAttribute("message", "Success!");
        echo $rootNode->asXML();
    }

}
