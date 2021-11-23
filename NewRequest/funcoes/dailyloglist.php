<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of dailyloglist
 *
 * @author jvbor
 */
class dailyloglist {

    //put your code here
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
        $dailyLogListSingle = $controle->GetDailyLogListSingle($this->parametros['selfid']);
        $flag = 'true';
        $msg = 'Success!';
        if ($dailyLogListSingle) {
            $date = date_create($dailyLogListSingle->getLastDate());
            $diaAtual = date('d');
            $mesAtual = date('m');
            $anoAtual = date('Y');
            $length = sizeof(explode(',', $dailyLogListSingle->getDayLog()));
            $qtdDiasMesAtual = date('t');
            if ($mesAtual != date_format($date, 'm') || $anoAtual != date_format($date, 'Y')) {
                $dailyLogListSingle->setDayLog('');
                $dailyLogListSingle->setUserAwardLog(0);
                $dailyLogListSingle->setLastDate(date('Y-m-d H:i:s') . '.000');
            }
            if ($length < $qtdDiasMesAtual) {
                if ($dailyLogListSingle->getDayLog() == '' && $length > 1) {
                    $dailyLogListSingle->setDayLog('False');
                    for ($x = $length; $x < $diaAtual - 1; $x++) {
                        $dailyLogListSingle->setDayLog($dailyLogListSingle->getDayLog() . ',False');
                    }
                }
            }
            $controle->UpdateDailyLogList($dailyLogListSingle);
            XMLSerializer::createGeneric($rootNode, 'DailyLogList', $dailyLogListSingle);
        } else {
            $flag = 'false';
            $msg = 'Error';
        }
        $rootNode->addAttribute("value", $flag);
        $rootNode->addAttribute("message", $msg);
        $rootNode->addAttribute('nowDate', date('Y-m-d H:i:s'));
        echo gzcompress($rootNode->asXML());
    }

}
