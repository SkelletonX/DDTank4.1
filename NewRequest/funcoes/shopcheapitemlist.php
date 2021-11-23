<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of shopcheapitemlist
 *
 * @author jvbor
 */
class shopcheapitemlist {

    //put your code here
    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        header("Content-type: text/xml");
        echo '<Result value="true" message="Success!" />';
    }

}
