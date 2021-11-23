<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of advancequesttime
 *
 * @author jvbor
 */
class advancequesttime {
    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        echo '0,'.date('Y-m-d H:i:s') . '.000'.',0';
    }
}
