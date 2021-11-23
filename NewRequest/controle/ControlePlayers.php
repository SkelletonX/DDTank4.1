<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of ControlePlayers
 *
 * @author jvbor
 */
class ControlePlayers {

    //put your code here
    private $conexao, $db;
    private $conexao2, $db2;

    function __construct() {
        $this->conexao = GerenciadorConexao::getConexao();
        $this->db = new Db($this->conexao);
        $this->conexao2 = GerenciadorConexao::getConexao(dbnamemember);
        $this->db2 = new Db($this->conexao2);
    }

    public function GetPlayerPage($order, $size, $userid = -1) {
        $str = "GP desc";
        switch ($order) {
            case 1:
                $str = "Offer desc";
                break;
            case 2:
                $str = "AddDayGP desc";
                break;
            case 3:
                $str = "AddWeekGP desc";
                break;
            case 4:
                $str = "AddDayOffer desc";
                break;
            case 5:
                $str = "AddWeekOffer desc";
                break;
            case 6:
                $str = "FightPower desc";
                break;
            case 7:
                $str = "AchievementPoint desc";
                break;
            case 8:
                $str = "AddDayAchievementPoint desc";
                break;
            case 9:
                $str = "AddWeekAchievementPoint desc";
                break;
            case 10:
                $str = "GiftGp desc";
                break;
            case 11:
                $str = "AddDayGiftGp desc";
                break;
            case 12:
                $str = "AddWeekGiftGp desc";
                break;
            case 13:
                $str = "GiftGp desc";
                break;
        }
        $order = $str . ',UserID';
        $queryWhere = " IsExist=1 and IsFirst<> 0 ";
        if ($userid != -1) {
            $queryWhere = $queryWhere . " and UserID =" . $userid . " ";
        }
        $q = $this->db->q("select top {$size} * from V_Sys_Users_Detail where {$queryWhere} and nickname!='Adm_Joao' order by {$order}");
        $listaPlayers = array();
        $controleItem = new ControleItens();
        while ($resultado = $q->fetch()) {
            $player = new PlayerInfo();
            Utilitarios::autoInitObjectFromResult($player, $resultado);
            $player->setID($resultado['UserID']);
            $player->setEscapeCount($resultado['Escape']);
            $player->setChairmanID(0);
            $player->setDate(0);
            $player->setLoginName($player->getUserName());
            $player->setWinCount($resultado['Win']);
            $player->setTotalCount($resultado['Total']);
            $player->setConsortiaRename($resultado['ConsortiaRename'] == '1' ? 'true' : 'false');
            $date = date_create($resultado['LastDate']);
            $dateFormated = date_format($date, 'Y-m-d');
            $timeFormated = date_format($date, 'H:i:s');
            $player->setLastDate($dateFormated . 'T' . $timeFormated . '.0000000-03:00');
            $player->setRename($resultado['Rename'] == '1' ? 'true' : 'false');
            $player->setTotalPrestige($player->getPntsBattle2());
            $style = explode(',',$player->getStyle());
            foreach ($style as $index => $item) {
                $itemId = explode("|",$item)[0];
                $item = $controleItem->getItemById($itemId);
                if($item==null){
                    $style[$index] = '';
                }
            }
            $player->setStyle(implode(",",$style));
            $listaPlayers[] = $player;
        }
        return $listaPlayers;
    }

    /**
     * 
     * @param type $id
     * @return PlayerInfo
     */
    public function GetPlayerByID($id) {
        $q = $this->db->q("select * from V_Sys_Users_Detail where userid=?", array($id));
        if ($resultado = $q->fetch()) {
            $player = new PlayerInfo();
            Utilitarios::autoInitObjectFromResult($player, $resultado);
            $player->setID($resultado['UserID']);
            $player->setEscapeCount($resultado['Escape']);
            $player->setChairmanID(0);
            $player->setDate(0);
            $player->setLoginName($player->getUserName());
            $player->setWinCount($resultado['Win']);
            $player->setTotalCount($resultado['Total']);
            $player->setConsortiaRename($resultado['ConsortiaRename'] == '1' ? 'true' : 'false');
            $date = date_create($resultado['LastDate']);
            $dateFormated = date_format($date, 'Y-m-d');
            $timeFormated = date_format($date, 'H:i:s');
            $player->setLastDate($dateFormated . 'T' . $timeFormated . '.0000000-03:00');
            $player->setRename($resultado['Rename'] == '1' ? 'true' : 'false');
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
            return $player;
        }
    }

    public function GetUserItemSingle($itemId) {
        $query = $this->db->q('exec SP_Users_Items_Single ?', array($itemId));
        if ($resultado = $query->fetch()) {
            $obj = new ItemInfo();
            Utilitarios::autoInitObjectFromResult($obj, $resultado);
            return $obj;
        }
    }

    public function GetMailBySenderID($userid) {
        $query = $this->db->q('exec SP_Mail_BySenderID ?', array($userid));
        $listaEmails = array();
        while ($resultado = $query->fetch()) {
            $obj = new MailInfo();
            Utilitarios::autoInitObjectFromResult($obj, $resultado);
            $listaEmails[] = $obj;
        }
        return $listaEmails;
    }

    public function GetMailByUserID($userid) {
        $query = $this->db->q('exec SP_Mail_ByUserID ?', array($userid));
        $listaEmails = array();
        while ($resultado = $query->fetch()) {
            $obj = new MailInfo();
            Utilitarios::autoInitObjectFromResult($obj, $resultado);
            $listaEmails[] = $obj;
        }
        return $listaEmails;
    }

    public function GetFriendsAll($userid) {
        $query = $this->db->q('exec SP_Users_Friends ?', array($userid));
        $controleItem = new ControleItens();
        $listaFriends = array();
        while ($resultado = $query->fetch()) {
            $obj = new FriendInfo();
            Utilitarios::autoInitObjectFromResult($obj, $resultado);
            $obj->setIsShow($obj->getIsShow() == 1 ? 'true':'false');
            $date = date_create($resultado['Birthday']);
            $dateFormated = date_format($date, 'Y-m-d H:i:s' . '.000');
            $obj->setBirthday($dateFormated);
            $obj->setBBSFriends('false');
            $obj->setApprenticeshipState(0);
            $obj->setAchievementPoint(0);
            $obj->setID($obj->getFriendID());
            $style = explode(',',$obj->getStyle());
            foreach ($style as $index => $item) {
                $itemId = explode("|",$item)[0];
                $item = $controleItem->getItemById($itemId);
                if($item==null){
                    $style[$index] = '';
                }
            }
            $obj->setStyle(implode(",",$style));
            $listaFriends[] = $obj;
        }
        return $listaFriends;
    }

    public function UpdateDailyLogList(DailyLogListInfo $daily) {
        $this->db->q('exec SP_DailyLogList_Update ?,?,?,?', array($daily->getUserID(), $daily->getUserAwardLog(), $daily->getDayLog(), $daily->getLastDate()));
    }

    public function GetDailyLogListSingle($userid) {
        $query = $this->db->q('exec SP_DailyLogList_Single ?', array($userid));
        if ($resultado = $query->fetch()) {
            $dailyLog = new DailyLogListInfo();
            Utilitarios::autoInitObjectFromResult($dailyLog, $resultado);
            return $dailyLog;
        }
        return false;
    }

    public function GetUserLoginList($username) {
        $query = $this->db->q('exec SP_Users_LoginList ?', array($username));
        $listaPlayers = array();
        while ($resultado = $query->fetch()) {
            $player = new PlayerInfo();
            $player->setUserName($resultado['UserName']);
            $player->setID($resultado['UserID']);
            $player->setNickName($resultado['NickName']);
            $player->setChairmanID(0);
            $player->setConsortiaName($resultado['ConsortiaName']);
            $player->setConsortiaRename($resultado['ConsortiaRename'] == '1' ? 'true' : 'false');
            $player->setEscapeCount($resultado['Escape']);
            $player->setFightPower($resultado['FightPower']);
            $player->setGrade($resultado['Grade']);
            $player->setIsFirst($resultado['IsFirst'] == '1 ' ? 'true' : 'false');
            $date = date_create($resultado['LastDate']);
            $dateFormated = date_format($date, 'Y-m-d');
            $timeFormated = date_format($date, 'H:i:s');
            $player->setLastDate($dateFormated . 'T' . $timeFormated . '.0000000-03:00');
            $player->setRename($resultado['Rename'] == '1' ? 'true' : 'false');
            $player->setRepute($resultado['Repute']);
            $player->setSex($resultado['Sex']);
            $player->setTotalCount($resultado['Total']);
            $player->setTypeVIP($resultado['typeVIP']);
            $player->setVIPLevel($resultado['VIPLevel']);
            $player->setWinCount($resultado['Win']);
            $listaPlayers[] = $player->toArray();
        } 
        return $listaPlayers;
    }

    public function login($username, $password, $nickname) {
        $query = $this->db2->q('exec dbo.Mem_Users_Accede_New ?,?', array($username, $password));
        if ($resultado = $query->fetch()) {
            $query = $this->db->q('exec SP_Users_LoginWebNew ? , ?', array($username, $nickname));
            $query->nextRowset();
            if ($resultado = $query->fetch()) {
                if ($resultado['IsExist'] == '1') {
                    $player = new PlayerInfo();
                    Utilitarios::autoInitObjectFromResult($player, $resultado);
                    $player->setID($resultado['UserID']);
                    $player->setEscapeCount($resultado['Escape']);
                    $player->setChairmanID(0);
                    $player->setDate(0);
                    $player->setLoginName($player->getUserName());
                    $player->setWinCount($resultado['Win']);
                    $player->setTotalCount($resultado['Total']);
                    $player->setConsortiaRename($resultado['ConsortiaRename'] == '1' ? 'true' : 'false');
                    $date = date_create($resultado['LastDate']);
                    $dateFormated = date_format($date, 'Y-m-d');
                    $timeFormated = date_format($date, 'H:i:s');
                    $player->setLastDate($dateFormated . 'T' . $timeFormated . '.0000000-03:00');
                    $player->setRename($resultado['Rename'] == '1' ? 'true' : 'false');
                    return $player;
                } else {
                    return false;
                }
            }
        } else {
            return false;
        }
        return false;
    }

    public function UpdateUserReputeFightPower() {
        $this->db->q('exec SP_Update_Repute_FightPower');
    }

}
