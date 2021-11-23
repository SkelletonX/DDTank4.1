<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of CreateAllCeleb
 *
 * @author jvbor
 */
class CreateAllCeleb {

    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        if ($this->parametros['key'] != keyCreateXml) {
            die("Key Invalida");
        }
        $text = '';
        $text .= $this->BuildCelebUsers('CelebByGpList', 0);
        $text .= $this->BuildCelebUsers('CelebByDayGPList', 2);
        $text .= $this->BuildCelebUsers('CelebByWeekGPList', 0);
        $text .= $this->BuildCelebUsers('CelebByOfferList', 1);
        $text .= $this->BuildCelebUsers('CelebByDayOfferList', 4);
        $text .= $this->BuildCelebUsers('CelebByWeekOfferList', 5);
        $text .= $this->BuildCelebUsers('CelebByDayFightPowerList', 6);
        $text .= $this->BuildCelebUsers('celebbytotalprestige', 13);
        $text .= $this->BuildCelebUsers('celebbyweekprestige', 13);
        $text .= $this->BuildCelebUsers('celebbydayprestige', 13);

        $text .= $this->BuildCelebConsortias('CelebByConsortiaRiches', 10);
        $text .= $this->BuildCelebConsortias('CelebByConsortiaDayRiches', 11);
        $text .= $this->BuildCelebConsortias('CelebByConsortiaWeekRiches', 12);
        $text .= $this->BuildCelebConsortias('CelebByConsortiaHonor', 13);
        $text .= $this->BuildCelebConsortias('CelebByConsortiaDayHonor', 14);
        $text .= $this->BuildCelebConsortias('CelebByConsortiaWeekHonor', 15);
        $text .= $this->BuildCelebConsortias('CelebByConsortiaLevel', 16);
        $text .= $this->BuildCelebConsortiasFightPower('celebbyconsortiafightpower');
        if (!isset($this->parametros['internal'])) {
            echo $text;
        }
    }

    private function BuildCelebConsortiasFightPower($file) {
        $rootNode = new SimpleXMLElement('<Result></Result>');
        $flag = 'true';
        $msg = 'Success!';
        $controle = new ControleConsortias();
        $consortias = $controle->UpdateConsortiaFightPower();
        $controlePlayer = new ControlePlayers();
        foreach ($consortias as $consortia) {
            $child = XMLSerializer::createGeneric($rootNode, 'Item', $consortia);
            if ($consortia->getChairmanID() != -1) {
                $player = $controlePlayer->GetPlayerByID($consortia->getChairmanID());
                XMLSerializer::createGeneric($child, 'Item', $player);
            }
        }
        $rootNode->addAttribute("value", $flag);
        $rootNode->addAttribute("message", $msg);
        $rootNode->addAttribute("date", date('Y-m-d'));
        $result = $rootNode->asXML();
        $flag = file_put_contents('./filesRequest/' . $file . '.xml', gzcompress($result));
        $flag = file_put_contents('./filesRequest/' . $file . '_out.xml', ($result));
        if ($flag) {
            return 'Success Build: ' . $file . PHP_EOL;
        } else {
            return 'Fail Build: ' . $file . PHP_EOL;
        }
    }

    private function BuildCelebConsortias($file, $order) {
        $rootNode = new SimpleXMLElement('<Result></Result>');
        $flag = 'true';
        $msg = 'Success!';
        $controle = new ControleConsortias();
        $consortias = $controle->GetConsortiaPage(50, $order, '', -1, -1, -1);
        $controlePlayer = new ControlePlayers();
        foreach ($consortias as $consortia) {
            if ($consortia->getChairmanID() != -1) {
                $player = $controlePlayer->GetPlayerByID($consortia->getChairmanID());
                if($player==null){
                    continue;
                }
                $child = XMLSerializer::createGeneric($rootNode, 'Item', $consortia);
                XMLSerializer::createGeneric($child, 'Item', $player);
            }
        }
        $rootNode->addAttribute("value", $flag);
        $rootNode->addAttribute("message", $msg);
        $rootNode->addAttribute("date", date('Y-m-d'));
        $result = $rootNode->asXML();
        $flag = file_put_contents('./filesRequest/' . $file . '.xml', gzcompress($result));
        $flag = file_put_contents('./filesRequest/' . $file . '_out.xml', ($result));
        if ($flag) {
            return 'Success Build: ' . $file . PHP_EOL;
        } else {
            return 'Fail Build: ' . $file . PHP_EOL;
        }
    }

    private function BuildCelebUsers($file, $order) {
        $rootNode = new SimpleXMLElement('<Result></Result>');
        $flag = 'true';
        $msg = 'Success!';
        $controlePlayers = new ControlePlayers();
        if ($file == "CelebByGpList") {
            $controlePlayers->UpdateUserReputeFightPower();
        }
        $players = $controlePlayers->GetPlayerPage($order, 50);
        XMLSerializer::createGeneric($rootNode, 'Item', $players);
        $rootNode->addAttribute("value", $flag);
        $rootNode->addAttribute("message", $msg);
        $rootNode->addAttribute("date", date('Y-m-d'));
        $result = $rootNode->asXML();
        $flag = file_put_contents('./filesRequest/' . $file . '.xml', gzcompress($result));
        $flag = file_put_contents('./filesRequest/' . $file . '_out.xml', ($result));
        if ($flag) {
            return 'Success Build: ' . $file . PHP_EOL;
        } else {
            return 'Fail Build: ' . $file . PHP_EOL;
        }
    }

}
