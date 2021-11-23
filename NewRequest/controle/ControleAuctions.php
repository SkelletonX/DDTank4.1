<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ControleAuctions
 *
 * @author jvbor
 */
class ControleAuctions {

    //put your code here
    //put your code here
    private $conexao, $db;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao();
        $this->db = new Db($this->conexao);
    }

    public function GetAuctionPage($page, $name, $type, $pay, $userID, $buyID, $order, $sort, $size, $AuctionIDs) {
        if($AuctionIDs == null || $AuctionIDs==''){
            $AuctionIDs = '0';
        }
        $queryWhere = " IsExist=1 ";
        if ($name == null || $name == '')
            $queryWhere = $queryWhere . " and Name like '%" . $name . "%' ";
        switch ($type) {
            case -1:
                if ($pay != -1)
                    $queryWhere = $queryWhere . " and PayType =" . $pay . " ";
                if ($userID != -1)
                    $queryWhere = $queryWhere . " and AuctioneerID =" . $userID . " ";
                if ($buyID != -1)
                    $queryWhere = $queryWhere . " and (BuyerID =" . $buyID . " or AuctionID in (" . $AuctionIDs . ")) ";
                break;
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
            case 16:
            case 17:
                $queryWhere = $queryWhere . " and Category =" . $type . " ";
                break;
            case 21:
                $queryWhere .= " and Category in(1,2,5,8,9) ";
                break;
            case 22:
                $queryWhere .= " and Category in(13,15,6,4,3) ";
                break;
            case 23:
                $queryWhere .= " and Category in(16,11,10) ";
                break;
            case 24:
                $queryWhere .= " and Category in(8,9) ";
                break;
            case 25:
                $queryWhere .= " and Category in (7,17) ";
                break;
            case 26:
                $queryWhere .= " and TemplateId>=311000 and TemplateId<=313999";
                break;
            case 27:
                $queryWhere .= " and TemplateId>=311000 and TemplateId<=311999 ";
                break;
            case 28:
                $queryWhere .= " and TemplateId>=312000 and TemplateId<=312999 ";
                break;
            case 29:
                $queryWhere .= " and TemplateId>=313000 and TempLateId<=313999";
                break;
            case 1100:
                $queryWhere .= " and TemplateID in (11019,11021,11022,11023) ";
                break;
            case 1101:
                $queryWhere .= " and TemplateID='11019' ";
                break;
            case 1102:
                $queryWhere .= " and TemplateID='11021' ";
                break;
            case 1103:
                $queryWhere .= " and TemplateID='11022' ";
                break;
            case 1104:
                $queryWhere .= " and TemplateID='11023' ";
                break;
            case 1105:
                $queryWhere .= " and TemplateID in (11001,11002,11003,11004,11005,11006,11007,11008,11009,11010,11011,11012,11013,11014,11015,11016) ";
                break;
            case 1106:
                $queryWhere .= " and TemplateID in (11001,11002,11003,11004) ";
                break;
            case 1107:
                $queryWhere .= " and TemplateID in (11005,11006,11007,11008) ";
                break;
            case 1108:
                $queryWhere .= " and TemplateID in (11009,11010,11011,11012) ";
                break;
            case 1109:
                $queryWhere .= " and TemplateID in (11013,11014,11015,11016) ";
                break;
        }
        $str2 = "Category,Name,Price,dd,AuctioneerID";
        switch ($order) {
            case 0:
                $str2 = "Name";
                break;
            case 2:
                $str2 = "dd";
                break;
            case 3:
                $str2 = "AuctioneerName";
                break;
            case 4:
                $str2 = "Price";
                break;
            case 5:
                $str2 = "BuyerName";
                break;
        }
        $str2 .= $sort ? " desc" : "";
        $str2 .= ",AuctionID ";
        $fdOreder = $str2;
        $listaAuctions = new stdClass();
        $listaAuctions->auctions = array();
        $query = $this->db->q("select * from V_Auction where {$queryWhere} order by {$fdOreder} OFFSET (({$page} - 1) * {$size}) ROWS FETCH NEXT {$size} ROWS ONLY;");
        while ($resultado = $query->fetch()) {
            $consortia = new AuctionInfo();
            Utilitarios::autoInitObjectFromResult($consortia, $resultado);
            $listaAuctions->auctions[] = $consortia;
        }
        $query = $this->db->q("select count(*) as total from V_Auction where {$queryWhere}");
        if($result = $query->fetch()){
            $listaAuctions->total = $result['total'];
        }
        return $listaAuctions;
    }

}
