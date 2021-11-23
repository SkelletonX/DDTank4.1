<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of activepulldown
 *
 * @author jvbor
 */
class activepulldown {

    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        $userid = $this->parametros['selfid'];
        $activeid = $this->parametros['activeid'];
        $activekey = $this->parametros['activekey'];
        $activekeyDecrypt = "";
        if ($activekey != '') {
            $activekeyDecrypt = Utilitarios::decryptValueFromCenterOrFlash($activekey);
        }
        $controle = new ControleActives();
        $msg = '';
        $retorno = $controle->PullDown($activeid, $activekeyDecrypt, $userid);
        header("Content-type: text/xml");
        $rootNode = new SimpleXMLElement('<Result></Result>');
        $rootNode->addAttribute("value", 'true');
        $rootNode->addAttribute("message", $retorno[1]);
        echo $rootNode->asXML();
        $controle = new ControleServer();
        $controle->MailNotice($userid);
    }

}
