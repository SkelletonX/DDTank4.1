<?php
class ControleApprentice
{
    private $conexao, $db;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao();
        $this->db = new Db($this->conexao);
    }

    public function GetApprenticePage($isReturnSelf, $selfid, $appshipStateType, $size, $requestType, $name, $Grade, $sex) {
        $queryWhere = " ApplyFor='true'";
        if($name != null && $name != '')
        {
            $queryWhere = $queryWhere . " and NickName like '%" . $name . "%' ";
        }
        // $queryWhere = " IsExist=1 ";
        // if ($name != null && $name != '') {
        //     $queryWhere = $queryWhere . " and ConsortiaName like '%" . $name . "%' ";
        // }
        // if ($consortiaID != NULL && $consortiaID != -1) {
        //     $queryWhere = $queryWhere . " and ConsortiaID =" . $consortiaID . " ";
        // }
        // if ($level != NULL && $level != -1) {
        //     $queryWhere = $queryWhere . " and Level =" . $level . " ";
        // }
        // if ($openApply != NULL && $openApply != -1) {
        //     $queryWhere = $queryWhere . " and OpenApply =" . $openApply . " ";
        // }
        $query = $this->db->q("select top 1 * from V_ApprenticeShipClubList where {$queryWhere}");
        $academy = array();
        while ($resultado = $query->fetch()) {
            $ApprenticeShipClubList = new ApprenticeShipClubList();
            Utilitarios::autoInitObjectFromResult($ApprenticeShipClubList, $resultado);
            $academy[] = $ApprenticeShipClubList;
        }
        return "tes";
    }
}
?>