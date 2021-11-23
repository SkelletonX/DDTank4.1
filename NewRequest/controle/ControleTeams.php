<?php
class ControleTeams
{
    private $conexao, $db;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao();
        $this->db = new Db($this->conexao);
    }

    public function getSquadInfoByTeamID($TeamID){
        $SquadList = array();
        $query = $this->db->q("select * from T_Squad_Users_Info where TeamID = '". $TeamID ."'");
        while ($result = $query->fetch()) {
            $TeamUserInfo = new TeamUserInfo();
            Utilitarios::autoInitObjectFromResult($TeamUserInfo, $result);
            $SquadList[] = $TeamUserInfo;
        }
        return $SquadList;
    }
}
?>