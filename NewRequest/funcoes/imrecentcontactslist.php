<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of imrecentcontactslist
 *
 * @author jvbor
 */
class imrecentcontactslist {

    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        header("Content-type: text/xml");
        $rootNode = new SimpleXMLElement('<Result></Result>');
        $controle = new ControlePlayers();
        $listaFriends = $controle->GetFriendsAll($this->parametros['selfid']);
        foreach ($listaFriends as $value) {
            XMLSerializer::createGeneric($rootNode, 'Item', $value);
        }
        $rootNode->addAttribute("value", 'true');
        $rootNode->addAttribute("message", 'Success!');
        echo ($rootNode->asXML());
    }

}
