<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of Utilitarios
 *
 * @author jvbor
 */
require_once './Crypt/RSA.php';

class Utilitarios {

    //put your code here


    public static function autoInitObjectFromResult($obj, $result) {
        $api = new ReflectionClass($obj);
        foreach ($api->getProperties() as $propertie) {
            $propertie->setAccessible(true);
            if (isset($result[$propertie->getName()])) {
                if (substr($propertie->getName(), 0, 2) === "Is" || substr($propertie->getName(), 0, 3) === "Can") {
                    $propertie->setValue($obj, $result[$propertie->getName()] == '1' ? 'true' : 'false');
                } else if (Utilitarios::contains($propertie->getName(), 'Date')) {
                    $date = date_create($result[$propertie->getName()]);
                    $dateFormated = date_format($date, 'Y-m-d H:i:s' . '.000');
                    $propertie->setValue($obj, $dateFormated);
                } else {
                    $propertie->setValue($obj, $result[$propertie->getName()]);
                }
            }
        }
        return $obj;
    }

    public static function contains($needle, $haystack) {
        return strpos($haystack, $needle) !== false;
    }

    public static function decryptValueFromSite($value) {
        $rsa = new Crypt_RSA();
        $rsa->loadKey(CookieKey);
        $p = base64_decode($value);
        $rsa->setEncryptionMode(CRYPT_RSA_ENCRYPTION_PKCS1);
        $p = $rsa->decrypt($p);
        return $p;
    }

    public static function decryptValueFromCenterOrFlash($value) {
        $rsa = new Crypt_RSA();
        $rsa->loadKey(privateKey);
        $p = base64_decode($value);
        $rsa->setEncryptionMode(CRYPT_RSA_ENCRYPTION_PKCS1);
        $p = $rsa->decrypt($p);
        return $p;
    }

    public static function getFileContents($source) {
        $ch = curl_init();
        curl_setopt($ch, CURLOPT_URL, $source);
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
        curl_setopt($ch, CURLOPT_SSLVERSION, 3);
        $data = curl_exec($ch);
        $error = curl_error($ch);
        curl_close($ch);
        return $data;
    }

}
