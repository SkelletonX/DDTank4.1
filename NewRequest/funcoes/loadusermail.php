<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of loadusermail
 *
 * @author jvbor
 */
class loadusermail {

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
        $emails = $controle->GetMailByUserID($this->parametros['selfid']);
        if (count($emails) == 60) {
            $msg = 'Olá, seu correio está cheio, utilize o comando @@;email para excluir todos os emaisl que já foram lidos';
        }
        foreach ($emails as $email) {
            if(isset($msg)){
                $email->setContent($msg);
                $email->setTitle('Email CHEIO');
            }
            $node_item = XMLSerializer::createGeneric($rootNode, 'Item', $email);
            $node_item->addAttribute('Annex1ID', $email->getAnnex1());
            $node_item->addAttribute('Annex2ID', $email->getAnnex2());
            $node_item->addAttribute('Annex3ID', $email->getAnnex3());
            $node_item->addAttribute('Annex4ID', $email->getAnnex4());
            $node_item->addAttribute('Annex5ID', $email->getAnnex5());
            if ($email->getAnnex1() != '') {
                $item = $controle->GetUserItemSingle($email->getAnnex1());
                if ($item != null) {
                    XMLSerializer::createGeneric($node_item, 'Item', $item);
                }
            }
            if ($email->getAnnex2() != '') {
                $item = $controle->GetUserItemSingle($email->getAnnex2());
                if ($item != null) {
                    XMLSerializer::createGeneric($node_item, 'Item', $item);
                }
            }
            if ($email->getAnnex3() != '') {
                $item = $controle->GetUserItemSingle($email->getAnnex3());
                if ($item != null) {
                    XMLSerializer::createGeneric($node_item, 'Item', $item);
                }
            }
            if ($email->getAnnex4() != '') {
                $item = $controle->GetUserItemSingle($email->getAnnex4());
                if ($item != null) {
                    XMLSerializer::createGeneric($node_item, 'Item', $item);
                }
            }
            if ($email->getAnnex5() != '') {
                $item = $controle->GetUserItemSingle($email->getAnnex5());
                if ($item != null) {
                    XMLSerializer::createGeneric($node_item, 'Item', $item);
                }
            }
        }
        if (!isset($this->parametros['debug'])) {
            echo gzcompress($rootNode->asXML());
        } else {
            echo $rootNode->asXML();
        }
    }

}
