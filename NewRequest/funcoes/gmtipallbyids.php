<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of gmtipallbyids
 *
 * @author jvbor
 */
class gmtipallbyids {

    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        $controle = new ControlePlayers();
        $ids = explode(',', $this->parametros['ids']);
        if (count($ids) == 2) {
            $player = $controle->GetPlayerByID($ids[1]);
        }
        header("Content-type: text/xml");
        $rootNode = new SimpleXMLElement('<Result></Result>');
        $chield = $rootNode->addChild('Item');
        $chield->addAttribute('Title', 'Atualização 1-5');
        $chield->addAttribute('BeginDate', date('d/m/Y'));
        $chield->addAttribute('EndDate', date('d/m/Y'));
        $chield->addAttribute('BeginTime', date('d/m/Y'));
        $chield->addAttribute('EndTime', date('d/m/Y'));
        $text = '- Livro mágico' . PHP_EOL . ' - Novo sistema de cartas '. PHP_EOL .' - Sublimar de Cartas '. PHP_EOL .' Release by Jefferson Ataa';
        if (isset($player)) {
            $text = 'Bem Vindo ' . $player->getNickName() . '.' . PHP_EOL . $text;
        }
        $chield->addAttribute('Text', ($text));
        $chield->addAttribute('IsExist', 'true');
        $rootNode->addAttribute("value", 'true');
        $rootNode->addAttribute("message", 'Success!');
        echo ($rootNode->asXML());
    }

}
