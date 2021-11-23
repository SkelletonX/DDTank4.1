<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

spl_autoload_register("autoloader");

function autoloader($classname) {


    $diretorios[] = "funcoes";

    foreach ($diretorios as $dir) {
        $filename = "./$dir/" . ($classname) . ".php";
        if (file_exists("$filename")) {
            include_once($filename);
            return;
        }
    }
}
