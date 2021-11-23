<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of imlistload
 *
 * @author jvbor
 */
class imlistload {

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
        $chield = $rootNode->addChild('customList');
        $chield->addAttribute('ID', 0);
        $chield->addAttribute('Name', 'Amigos');
        $chield = $rootNode->addChild('customList');
        $chield->addAttribute('ID', 1);
        $chield->addAttribute('Name', 'Bloqueados');
        foreach ($listaFriends as $value) {
            XMLSerializer::createGeneric($rootNode, 'Item', $value);
        }
        $rootNode->addAttribute("value", 'true');
        $rootNode->addAttribute("message", 'Success!');
        if (!isset($this->parametros['debug'])) {
            echo gzcompress($rootNode->asXML());
        } else {
            echo $rootNode->asXML();
        }
    }

}
