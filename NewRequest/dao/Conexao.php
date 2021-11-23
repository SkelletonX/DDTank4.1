<?php

class Conexao {

    private $conn = null;

    function __construct($dbname2 = null) {
        try {
            $this->conn = new PDO("sqlsrv:server=" . servername . ";Database=" . ($dbname2 == null ? dbname : $dbname2 ), username, password);
            $this->conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        } catch (PDOException $e) {
            die("Connection failed: " . $e->getMessage());
        }
    }

    /**
     * 
     * @return PDO
     */
    function getConexao() {
        return $this->conn;
    }

}

?>
