<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of consortiauserslist
 *
 * @author jvbor
 */
class consortiauserslist {

    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        $controle = new ControleConsortias();
        $listaConsortia = $controle->GetConsortiaUsersPage($this->parametros['size'], $this->parametros['order'], $this->parametros['consortiaid'], $this->parametros['userid'], $this->parametros['state']);
        header("Content-type: text/xml");
        $rootNode = new SimpleXMLElement('<Result></Result>');
        foreach ($listaConsortia as $consortia) {
            $node = XMLSerializer::createGeneric($rootNode, 'Item', $consortia);
            $node->addAttribute('DutyLevel', $consortia->getLevel());
            $node->addAttribute('Rank', 'honor');
            $node->addAttribute('IsCandidate', 'false');
            $node->addAttribute('IsVote', 'false');
            $node->addAttribute('LastWeekRichesOffer', 0);
            $node->addAttribute('TotalRichesOffer', $consortia->getRichesOffer());
            $node->addAttribute('AchievementPoint', 1);
        }
        $rootNode->addAttribute("value", 'true');
        $rootNode->addAttribute("message", 'Success!');
        $dateFormated = date('Y-m-d H:i:s');
        $rootNode->addAttribute("currentDate", $dateFormated);
        if (isset($this->parametros['debug'])) {
            echo $rootNode->asXML();
        } else {
            echo gzcompress($rootNode->asXML());
        }
    }

}
