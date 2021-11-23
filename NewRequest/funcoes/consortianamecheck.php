<?php
class consortianamecheck {

    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        $controle = new ControleConsortias();
        $consortia = $controle->GetConsortiaSingleByName($this->parametros['nickname']);
        header("Content-type: text/xml");
        $rootNode = new SimpleXMLElement('<Result></Result>');
        $rootNode->addAttribute("value", $consortia == null ? 'true' : 'false');
        $rootNode->addAttribute("message", $consortia == null ? 'Sociedade Criada Com Sucesso' : 'Esse Nome Já Está Em Uso');
        if(empty($consortia))
        {
            $controle->updateConsortiaName($this->parametros['NickName'], $this->parametros['selfid']);
        }
        echo ($rootNode->asXML());
    }

}
