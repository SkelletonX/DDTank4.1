<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of login
 *
 * @author Jefferson Ataa
 */
require_once './Crypt/RSA.php';

class login {

    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        header("Content-type: text/xml");
        $rootNode = new SimpleXMLElement('<Result></Result>');
        $p = Utilitarios::decryptValueFromCenterOrFlash($this->parametros['p']);
        if($p == null && $p == ""){
            echo "Ops Senha Fail";
        }
        $valores = explode(",", substr($p, 7));
        $valoresSenha = explode(";", Utilitarios::decryptValueFromSite($valores[1]));
        $senha = $valoresSenha[0];
  
        $validadeSenha = $valoresSenha[1];
        $flag = 'true';
        $msg = 'Bem Vindo';
        $result = '';
        //  if ($validadeSenha >= time()) {
            
        if ($validadeSenha >= time()) {
            $controle = new ControlePlayers();
            $player = $controle->login($valores[0], md5($senha), $valores[3]);
            if ($player) {
                $style = explode(',',$player->getStyle());
                $controleItem = new ControleItens();
                foreach ($style as $index => $item) {
                    $itemId = explode("|",$item)[0];
                    $item = $controleItem->getItemById($itemId);
                    if($item==null){
                        $style[$index] = '';
                    }
                }
                $player->setStyle(implode(",",$style));
                XMLSerializer::createGeneric($rootNode, 'Item', $player);
                $controle2 = new ControleServer();
                $result = $controle2->createPlayer($player->getID(), $valores[0], $valores[2]);
                if(!$result){
                    $x = 0;
                    while($x<10 && !$result){
                        $result = $controle2->createPlayer($player->getID(), $valores[0], $valores[2]);
                        $x++;
                    }
                }
                if(!$result){
                    $flag = 'false';
                }
            } else {
                $flag = 'false';
                $msg = 'Nome de Usuario ou Senha Incorretos: ';
            }
        } else {
            $flag = 'false';
            $msg = 'Sessão Expirada';
        }
        $rootNode->addAttribute("value", $flag);
        $rootNode->addAttribute("message", $msg);
        $rootNode->addAttribute("validade", $validadeSenha);
        $rootNode->addAttribute("time", time());
        echo $rootNode->asXML();
    }

}
