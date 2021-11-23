<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

class consortialist {

    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        $controle = new ControleConsortias();
        $listaConsortia = $controle->GetConsortiaPage($this->parametros['size'], $this->parametros['order'], $this->parametros['name'], $this->parametros['consortiaid'], $this->parametros['level'], $this->parametros['openapply']);
        header("Content-type: text/xml");
        $rootNode = new SimpleXMLElement('<Result></Result>');
        foreach ($listaConsortia as $consortia){
            $node = XMLSerializer::createGeneric($rootNode, 'Item', $consortia);
            $node->addAttribute('ConsortiaGiftGp',940);
            $node->addAttribute('ConsortiaAddDayGiftGp',0);
            $node->addAttribute('ConsortiaAddWeekGiftGp',8);
            $node->addAttribute('IsVoting','false');
            $node->addAttribute('VoteRemainDay',3);
            $node->addAttribute('CharmGP',940);
        }
        $rootNode->addAttribute("value", 'true');
        $rootNode->addAttribute("total", sizeof($listaConsortia));
        $rootNode->addAttribute("message", 'Success!');
        if (isset($this->parametros['debug'])) {
            echo $rootNode->asXML();
        } else {
            echo gzcompress($rootNode->asXML());
        }
    }

}
