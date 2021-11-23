<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of consortiaapplyuserslist
 *
 * @author jvbor
 */
class consortiaapplyuserslist {

    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        $controle = new ControleConsortias();
        $listaConsortia = $controle->GetConsortiaApplyUserPage($this->parametros['page'], $this->parametros['size'], $this->parametros['order'], $this->parametros['consortiaid'], $this->parametros['applyid'], $this->parametros['userid']);
        header("Content-type: text/xml");
        $rootNode = new SimpleXMLElement('<Result></Result>');
        XMLSerializer::createGeneric($rootNode, 'Item', $listaConsortia);
        $rootNode->addAttribute("value", 'true');
        $rootNode->addAttribute("message", 'Success!');
        if (isset($this->parametros['debug'])) {
            echo $rootNode->asXML();
        } else {
            echo ($rootNode->asXML());
        }
    }

}
