<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of consortiaequipcontrollist
 *
 * @author jvbor
 */
class consortiaequipcontrollist {

    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        $controle = new ControleConsortias();
        $listaConsortia = $controle->GetConsortiaEquipControlPage($this->parametros['consortiaid'], $this->parametros['level'], $this->parametros['type']);
        header("Content-type: text/xml");
        $rootNode = new SimpleXMLElement('<Result></Result>');
        XMLSerializer::createGeneric($rootNode, 'Item', $listaConsortia);
        $rootNode->addAttribute("value", 'true');
        $rootNode->addAttribute("message", 'Success!');
        echo $rootNode->asXML();
    }

}
