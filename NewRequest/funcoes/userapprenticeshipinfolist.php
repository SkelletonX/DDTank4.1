<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of userapprenticeshipinfolist
 *
 * @author jvbor
 */
class userapprenticeshipinfolist {
    //put your code here
     private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        echo '<Result total="0" value="true" message="Success!" isPlayerRegeisted="false" isSelfPublishEquip="false" />';
    }
}
