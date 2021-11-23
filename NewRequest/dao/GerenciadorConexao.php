<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of GerenciadorConexao
 *
 * @author jvbor
 */
class GerenciadorConexao {

    //put your code here
    private static $conexoes = array();

    /**
     * 
     * @param String $dbName
     * @return Conexao
     */
    public static function getConexao($dbName = null) {
        if ($dbName == null) {
            $dbName = dbname;
        }
        if (!isset(GerenciadorConexao::$conexoes[$dbName])) {
            GerenciadorConexao::$conexoes[$dbName] = new Conexao($dbName);
        }
        return GerenciadorConexao::$conexoes[$dbName];
    }

}
