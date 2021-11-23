<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of AuctionPageList
 *
 * @author jvbor
 */
class auctionpagelist {

    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        $controleAuctions = new ControleAuctions();
        $controlePlayers = new ControlePlayers();
        header("Content-type: text/xml");
        $rootNode = new SimpleXMLElement('<Result></Result>');
        $auctions = $controleAuctions->GetAuctionPage($this->parametros['page'], $this->parametros['name'], $this->parametros['type'], $this->parametros['pay'], $this->parametros['userid'], $this->parametros['buyid'], $this->parametros['order'], $this->parametros['sort'], 50, $this->parametros['Auctions']);
        foreach ($auctions->auctions as $auction) {
            $child = XMLSerializer::createGeneric($rootNode, 'Item', $auction);
            $item = $controlePlayers->GetUserItemSingle($auction->getItemID());
            if ($item != null) {
                XMLSerializer::createGeneric($child, 'Item', $item);
            }
        }
        $rootNode->addAttribute("value", 'true');
        $rootNode->addAttribute("total", $auctions->total);
        $rootNode->addAttribute("message", 'Success!');
        echo $rootNode->asXML();
    }

}
