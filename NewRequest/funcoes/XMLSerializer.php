<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of XMLSerializer
 *
 * @author jvbor
 */
class XMLSerializer {

    // functions adopted from http://www.sean-barton.co.uk/2009/03/turning-an-array-or-object-into-xml-using-php/

    public static function generateValidXmlFromObj(stdClass $obj, $node_block = 'Result', $node_name = 'Item') {
        $arr = get_object_vars($obj);
        return self::generateValidXmlFromArray($arr, $node_block, $node_name);
    }

    public static function generateValidXmlFromArray($array, $node_block = 'Result', $node_name = 'Item', $adicionais = null) {
        $xml = '<?xml version="1.0" encoding="UTF-8" ?>';

        if ($adicionais == null) {
            $xml .= '<' . $node_block . ' value="true" message= "Success!">';
        } else {
            $xml .= '<' . $node_block . ' value="true" message= "Success!" ';
            foreach ($adicionais as $key => $value) {
                $xml .= ' ' . $key . '="' . $value . '"';
            }
            $xml .= ">";
        }
        $xml .= self::generateXmlFromArray($array, $node_name);
        $xml .= '</' . $node_block . '>';

        return $xml;
    }

    /**
     * 
     * @param SimpleXMLElement $rootNode
     * @param type $node_name
     * @param type $objs
     * @return SimpleXMLElement
     */
    public static function createGeneric(SimpleXMLElement $rootNode, $node_name, $objs) {
        if (is_array($objs)) {
            foreach ($objs as $obj) {
                $node_item = $rootNode->addChild($node_name);
                $api = new ReflectionClass($obj);
                foreach ($api->getProperties() as $propertie) {
                    $propertie->setAccessible(true);
                    $node_item->addAttribute($propertie->getName(), $propertie->getValue($obj));
                }
            }
        } else {
            $node_item = $rootNode->addChild($node_name);
            $api = new ReflectionClass($objs);
            foreach ($api->getProperties() as $propertie) {
                $propertie->setAccessible(true);
                $node_item->addAttribute($propertie->getName(), $propertie->getValue($objs));
            }
            return $node_item;
        }
    }

    private static function generateXmlFromArray($array, $node_name) {

        $xml = '';
        foreach ($array as $value) {
            $xml .= '<' . $node_name;
            if (is_object($array)) {
                $arr = get_object_vars($obj);
                print_r($arr);
            } else {
                foreach ($value as $key => $value2) {
                    $xml .= ' ' . $key . '="' . htmlspecialchars($value2) . '"';
                }
            }
            $xml .= ' />';
        }

        return $xml;
    }

}
