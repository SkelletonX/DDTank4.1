<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of subactivelist
 *
 * @author jvbor
 */
class subactivelist {

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
        $rootNode->addAttribute("nowTime", date('Y-m-d H:i:s'));
        $controle = new ControleServer();
        $subActives = $controle->GetAllSubActive();
        foreach ($subActives as $value) {
            XMLSerializer::createGeneric($rootNode, 'Active', $value);
            foreach ($controle->GetAllSubActiveCondition($value->getActiveID()) as $value2) {
                XMLSerializer::createGeneric($rootNode, 'Condition', $value2);
            }
        }
        echo $rootNode->asXML();
    }

}
