<?php
/**
 * @author Jefferson Ataa
 * 2020-08-19
 */
class BattleTeamIMLoadServLet
{
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        $ControleTeams = new ControleTeams();
        $squadList = $ControleTeams->getSquadInfoByTeamID($this->parametros['teamid']);
        header("Content-type: text/xml");
        $rootNode = new SimpleXMLElement('<Result></Result>');
        foreach ($squadList as $squadInfo){
            $node->addAttribute("teamID", $this->parametros['teamid']);
            $node = XMLSerializer::createGeneric($rootNode, 'Item', $squadInfo);
        }
        $rootNode->addAttribute("value", 'true');
        $rootNode->addAttribute("total", sizeof($squadList));
        $rootNode->addAttribute("message", 'Success!');
        if (isset($this->parametros['debug'])) {
            echo $rootNode->asXML();
        } else {
            echo gzcompress($rootNode->asXML());
        }
    }
}
?>