<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of refreshusermail
 *
 * @author jvbor
 */
class refreshusermail {
    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        $controle = new ControleServer();
        $controle->MailNotice($this->parametros['userid']);
    }
}
