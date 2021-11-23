<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of xmldecrypt
 *
 * @author jvbor
 */
class xmldecrypt {

    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar(){
        header('Content-Type: text/xml');
        if($this->parametros['encrypt']){
            echo gzuncompress(Utilitarios::getFileContents($this->parametros['url2']));
        }else {
            echo Utilitarios::getFileContents($this->parametros['url2']);
        }
    }

}
