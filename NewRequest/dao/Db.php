<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of Db
 *
 * @author jvbor
 */
class Db {

    private $conn;

    function __construct($conn) {
        $this->conn = $conn->getConexao();
    }

    /**
     * 
     * @param String $query
     * @param array() $args
     * @return PDOStatement
     * @throws Exception
     */
    public function q($query, $args = null) {
        try {
            if ($args == null) {
                $result = $this->conn->query($query);
            } else {
                $result = $this->conn->prepare($query);
                $result->execute($args);
            }
            if (!$result) {
                die("Query failed: " . $this->conn->error);
            }
        } catch (Exception $e) {
            throw $e;
        }
        return $result;
    }

    public function qn(PDOStatement $query) {
        $result;
        if ($query) {
            $result = $query->rowCount();
        }
        return $result;
    }

    /**
     * 
     * @param PDOStatement $query
     * @return mixed
     */
    public function qa(PDOStatement $query) {
        $result;
        if ($query) {
            $result = $query->fetch();
            return $result;
        }
        return null;
    }

    public function lastID() {
        $q = $this->q("select LAST_INSERT_ID() as id;");
        return $this->qa($q)['id'];
    }
    
    /**
     * 
     * @return PDO
     */
    function getConn() {
        return $this->conn;
    }



}
