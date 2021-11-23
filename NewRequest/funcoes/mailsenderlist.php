<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of mailsenderlist
 *
 * @author jvbor
 */
class mailsenderlist {
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
        $controle = new ControlePlayers();
        $emails = $controle->GetMailBySenderID($this->parametros['selfid']);
        foreach ($emails as $email) {
            XMLSerializer::createGeneric($rootNode, 'Item', $email);
        }
        echo gzcompress($rootNode->asXML());
    }
}
