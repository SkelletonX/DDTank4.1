<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of votesubmit
 *
 * @author jvbor
 */
class votesubmit {
    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }
    
    private function executar(){
        echo '1';
    }
}
