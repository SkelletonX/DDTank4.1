<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

spl_autoload_register("autoloaderPrivate");

function autoloaderPrivate($classname) {


    $diretorios[] = "funcoes";
    $diretorios[] = "modelo";
    $diretorios[] = "controle";
    $diretorios[] = "dao";

    foreach ($diretorios as $dir) {
        $filename = "./$dir/" . ($classname) . ".php";
        if (file_exists("$filename")) {
            include_once($filename);
            return;
        }
    }
}
