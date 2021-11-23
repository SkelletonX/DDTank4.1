<?php

date_default_timezone_set('America/Sao_Paulo');
include 'autoloader.php';
include 'config.php';
session_write_close();

if (isset($_GET['debug'])) {
    ini_set('display_errors', 1);
    ini_set('display_startup_errors', 1);
    error_reporting(E_ALL);
}

carregaUrl();

function carregaUrl() {

    $classe = strtolower(explode('.', $_GET['url'])[0]);
    $extension = strtolower(explode('.', $_GET['url'])[1]);
    if ($extension == 'xml') {
        $fileName = './filesRequest/' . $classe . '.xml';
        if (file_exists($fileName)) {
            if (Utilitarios::contains('celebby', strtolower($classe))) {
                if (time() - filemtime($fileName) >= 1800) {
                    include_once './autoloaderPrivate.php';
                    new CreateAllCeleb(array('key' => keyCreateXml, 'internal' => true));
                    header('HTTP/1.1 202');
                } else {
                    header('HTTP/1.1 201');
                    header('Age-File-Request: ' . (time() - filemtime($fileName)));
                }
            } else {
                header('HTTP/1.1 200');
            }
            header('Content-Type: text/xml');
            $file = file_get_contents($fileName);
            echo $file;
        } else if (checkRequest337($classe . '.xml')) {
            header('HTTP/1.1 203');
            saveFile($classe . '.xml');
            header('Content-Type: text/xml');
            readfile($fileName);
        } else {
            header('HTTP/1.1 404');
            die('File Not Found In 337 or Local: ' . $fileName);
        }
    } else {
        $parametros = array();
        foreach ($_GET as $key => $value) {
            if ($key != 'url' && $key != 'rnd') {
                $parametros[strtolower($key)] = EscapeString($value);
            }
        }
        if (class_exists($classe)) {
            include_once './autoloaderPrivate.php';
            sleep(1);
            $classeInstancia = new $classe($parametros);
        } else {
            header('HTTP/1.0 566');
            die('Class not Found: ' . $classe);
        }
    }
}

function EscapeString($string) {
    $string = str_replace("'", '', $string);
    $string = str_replace('"', '', $string);
    $string = str_replace(';', '', $string);
    $string = str_replace(')', '', $string);
    $string = str_replace('(', '', $string);
    $string = trimUnicode($string);
    return $string;
}

function checkRequest337($url) {
    $handle = curl_init('http://quest362-ddt.337.com/' . $url);
    curl_setopt($handle, CURLOPT_RETURNTRANSFER, TRUE);
    $response = curl_exec($handle);
    $httpCode = curl_getinfo($handle, CURLINFO_HTTP_CODE);
    if ($httpCode < 399) {
        curl_close($handle);
        return true;
    }
    return false;
}

function trimUnicode($str) {
    return preg_replace('/^[\pZ\pC]+|[\pZ\pC]+$/u', '', $str);
}

function saveFile($file) {
    try {
        // content file
        $isOkey = false;
        $dirname = dirname('./filesRequest/' . $file);
        if (!is_dir($dirname)) {
            mkdir($dirname, 0777, true);
        }
        $contents = getFileContents('http://quest362-ddt.337.com/' . $file);
        if ($contents != NULL) {
            // save file
            $savefile = fopen('./filesRequest/' . $file, 'w');
            fwrite($savefile, $contents);
            fclose($savefile);
            $isOkey = true;
            error_log($file . PHP_EOL, 3, './filesRequest/logDownload.txt');
        }
    } catch (Exception $e) {
        echo ($e > -getMessage());
    }
    return $isOkey;
}

function getFileContents($source) {
    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, $source);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($ch, CURLOPT_SSLVERSION, 3);
    $data = curl_exec($ch);
    $error = curl_error($ch);
    curl_close($ch);
    return $data;
}

?>
