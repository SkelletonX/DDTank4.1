<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of createallxml
 *
 * @author jvbor
 */
 
class createallxml {
    //put your code here
    private $parametros;

    function __construct($parametros) {
        $this->parametros = $parametros;
        $this->executar();
    }

    private function executar() {
        if ($this->parametros['key'] != keyCreateXml) {
            die("Key Invalida");
        }
        $text = '';
        $text .= $this->createServerConfig();
        $text .= $this->createMapServer();
        $text .= $this->createMapList();
        $text .= $this->createTemplateAllList();
        $text .= $this->createActiveList();
        $text .= $this->createAllActiveConvertItems();
        $text .= $this->createAllActivitySystemItem();
        $text .= $this->createShopGoodsShowList();
        $text .= $this->createShopItemList();
        $text .= $this->createPetSkillInfo();
        $text .= $this->createQuestInfo();
        $text .= $this->createTitleInfo();
        $text .= $this->createPetTemplateInfo();
        $text .= $this->createPveInfo();
        $text .= $this->CreateFuguras();
        $text .= $this->CreateFuguras2();
        $text .= $this->CreateMountDraw();
        $text .= $this->CreatePetForm();
        $text .= $this->CreateRune();
        $text .= $this->CreateRuneAdvance();
        $text .= $this->createShopBoxList();
        $text .= $this->CreateGoldTemplates();
        $text .= $this->CreateMagicStone();
        $text .= $this->CreateFusionList();
        $text .= $this->CreatePetSkillElementList();
        $text .= $this->CreateSkillTemplateInfo();
        $text .= $this->createBallConfigInfo();
        $text .= $this->createBallInfo();
        $text .= $this->CreateDevilPointReward();
        $text .= $this->CreateDevilTreasSarahToBoxList();
        $text .= $this->CreateDevilTreasItemList();
        echo $text;
    }

    private function CreateDevilTreasItemList()
    {
        try
        {
            $rootNode = new SimpleXMLElement('<result></result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleActives();
            $devilItemList = $controle->GetAllDevilTreasItemList();
            foreach($devilItemList as $devilItem)
            {
                XMLSerializer::createGeneric($rootNode, 'Item', $devilItem);
            }
            $flag = file_put_contents("./filesRequest/deviltreasitemlist.xml", gzcompress($rootNode->asXML()));
            //$flag2 = file_put_contents("./filesRequest/XMLDecrypt/deviltreaspointslist.xml");
            if($flag)
            {
                return 'Success build: DevilTreasItemList'.PHP_EOL;
            }else{
                return 'Fail Build: DevilTreasItemList' . PHP_EOL;
            }
        }
        catch(Exception $ex)
        {
            die($ex->getMessage());
        }
    }

    private function CreateDevilTreasSarahToBoxList()
    {
        try
        {
            $rootNode = new SimpleXMLElement('<result></result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleActives();
            $devilSarah = $controle->GetAllDevilTreasSarahToBoxList();
            foreach($devilSarah as $devilSarahInfo)
            {
                XMLSerializer::createGeneric($rootNode, 'Item', $devilSarahInfo);
            }
            $flag = file_put_contents("./filesRequest/deviltreassarahtoboxlist.xml", gzcompress($rootNode->asXML()));
            //$flag2 = file_put_contents("./filesRequest/XMLDecrypt/deviltreaspointslist.xml");
            if($flag)
            {
                return 'Success build: DevilTreasSarahToBoxList'.PHP_EOL;
            }else{
                return 'Fail Build: DevilTreasSarahToBoxList' . PHP_EOL;
            }
        }
        catch(Exception $ex)
        {
            die($ex->getMessage());
        }
    }

    private function CreateDevilPointReward()
    {
        try
        {
            $rootNode = new SimpleXMLElement('<result></result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleActives();
            $reward = $controle->GetAllDevilTreasPointsList();
            foreach($reward as $rewardItem)
            {
                XMLSerializer::createGeneric($rootNode, 'Item', $rewardItem);
            }
            $flag = file_put_contents("./filesRequest/deviltreaspointslist.xml", gzcompress($rootNode->asXML()));
            //$flag2 = file_put_contents("./filesRequest/XMLDecrypt/deviltreaspointslist.xml");
            if($flag)
            {
                return 'Success build: deviltreaspointslist'.PHP_EOL;
            }else{
                return 'Fail Build: deviltreaspointslist' . PHP_EOL;
            }
        }
        catch(Exception $ex)
        {
            die($ex->getMessage());
        }
    }

    private function CreateFusionList() {
        try {
            $rootNode = new SimpleXMLElement('<result></result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleServer();
            $items = $controle->GetAllFusionList();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/fusioninfoload.xml", gzcompress($rootNode->asXML()));
             if ($flag) {
                return 'Success Build: fusioninfoload' . PHP_EOL;
            } else {
                return 'Fail Build: fusioninfoload' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function CreateMagicStone() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleItens();
            $items = $controle->GetAllMagicStones();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/magicstonetemplate.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: magicstonetemplate' . PHP_EOL;
            } else {
                return 'Fail Build: magicstonetemplate' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function CreateGoldTemplates() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleItens();
            $items = $controle->GetAllGoldTemplates();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode, 'item', $value);
            }
            $flag = file_put_contents("./filesRequest/goldequiptemplateload.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: goldequiptemplateload' . PHP_EOL;
            } else {
                return 'Fail Build: goldequiptemplateload' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function CreateRuneAdvance() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $child = $rootNode->addChild("RuneAdvanceTemplate");
            $controle = new ControleRunes();
            $items = $controle->GetAllRunesAdvanceTemplates();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($child, 'RuneAdvance', $value);
            }
            $flag = file_put_contents("./filesRequest/runeadvancetemplatelist.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: runeadvancetemplatelist' . PHP_EOL;
            } else {
                return 'Fail Build: runeadvancetemplatelist' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function CreateRune() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $child = $rootNode->addChild("RuneTemplate");
            $controle = new ControleRunes();
            $items = $controle->GetAllRunesTemplates();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($child, 'Rune', $value);
            }
            $flag = file_put_contents("./filesRequest/runetemplatelist.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: runetemplatelist' . PHP_EOL;
            } else {
                return 'Fail Build: runetemplatelist' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function CreatePetForm() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControlePets();
            $items = $controle->GetAllPetForm();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode, 'item', $value);
            }
            $flag = file_put_contents("./filesRequest/LoadPetFormData.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: LoadPetFormData' . PHP_EOL;
            } else {
                return 'Fail Build: LoadPetFormData' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function CreateMountDraw() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleMontarias();
            $items = $controle->GetAllMountDraw();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode, 'item', $value);
            }
            $flag = file_put_contents("./filesRequest/mountdrawtemplate.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: mountdrawtemplate' . PHP_EOL;
            } else {
                return 'Fail Build: mountdrawtemplate' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }
    
    private function CreateFuguras() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleFuguras();
            $items = $controle->GetAllClothGroup();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/clothgrouptemplateinfo.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: clothgrouptemplateinfo' . PHP_EOL;
            } else {
                return 'Fail Build: clothgrouptemplateinfo' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }
    
    private function CreateFuguras2() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleFuguras();
            $items = $controle->GetAllClothProperty();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/clothpropertytemplateinfo.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: clothpropertytemplateinfo' . PHP_EOL;
            } else {
                return 'Fail Build: clothpropertytemplateinfo' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }
    
    private function createPveInfo() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleServer();
            $items = $controle->GetAllPveInfo();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/LoadPVEItems.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: LoadPVEItems' . PHP_EOL;
            } else {
                return 'Fail Build: LoadPVEItems' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }
    
    private function createBallConfigInfo() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleServer();
            $items = $controle->GetAllBallConfig();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/BombConfig.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: BombConfig' . PHP_EOL;
            } else {
                return 'Fail Build: BombConfig' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }
    
    private function createBallInfo() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleServer();
            $items = $controle->GetAllBall();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/BallList.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: BallList' . PHP_EOL;
            } else {
                return 'Fail Build: BallList' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function createTitleInfo() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleTitulos();
            $items = $controle->GetNewTitle();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/newtitleinfo.xml", ($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: newtitleinfo' . PHP_EOL;
            } else {
                return 'Fail Build: newtitleinfo' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function createQuestInfo() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleQuests();
            $items = $controle->GetALlQuest();
            foreach ($items as $value) {
                $questInfoNode = XMLSerializer::createGeneric($rootNode, 'Item', $value);
                $questsCondictions = $controle->GetAllQuestCondiction($value->getID());
                foreach ($questsCondictions as $value2) {
                    XMLSerializer::createGeneric($questInfoNode, 'Item_Condiction', $value2);
                }
                $questsAwards = $controle->GetAllQuestGoods($value->getID());
                foreach ($questsAwards as $value2) {
                    XMLSerializer::createGeneric($questInfoNode, 'Item_Good', $value2);
                }
            }
            $items2 = $controle->GetAllQuestRate();
            foreach ($items2 as $value) {
                XMLSerializer::createGeneric($rootNode, 'Rate', $value);
            }
            $flag = file_put_contents("./filesRequest/QuestList.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: QuestList' . PHP_EOL;
            } else {
                return 'Fail Build: QuestList' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function createPetTemplateInfo() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControlePets();
            $items = $controle->GetAllPetTemplate();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode, 'item', $value);
            }
            $flag = file_put_contents("./filesRequest/pettemplateinfo.xml", ($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: pettemplateinfo' . PHP_EOL;
            } else {
                return 'Fail Build: pettemplateinfo' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function createPetSkillInfo() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControlePets();
            $items = $controle->GetAllPetSkillInfo();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode, 'item', $value);
            }
            $flag = file_put_contents("./filesRequest/petskillinfo.xml", ($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: petskillinfo' . PHP_EOL;
            } else {
                return 'Fail Build: petskillinfo' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function CreatePetSkillElementList()
    {
        try
        {
            $rootNode = new SimpleXMLElement('<result></result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControlePets();
            $items = $controle->GetAllPetSkillElementInfo();
            foreach ($items as $value) 
            {
                XMLSerializer::createGeneric($rootNode, 'item', $value);
            }
            $flag = file_put_contents("./filesRequest/petskillelementinfo.xml", ($rootNode->asXML()));
            if($flag) 
            {
                return 'Success Build: petskillelementinfo' . PHP_EOL;
            }
            else
            {
                return 'Fail Build: petskillelementinfo' . PHP_EOL;
            }
        }
        catch(Exception $ex)
        {
            die($ex->getMessage());
        }
    }

    private function CreateSkillTemplateInfo()
    {
        try
        {
            $rootNode = new SimpleXMLElement('<result></result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControlePets();
            $items = $controle->GetAllPetSkillTemplateInfo();
            foreach ($items as $value) 
            {
                XMLSerializer::createGeneric($rootNode, 'item', $value);
            }
            $flag = file_put_contents("./filesRequest/petskilltemplateinfo.xml", ($rootNode->asXML()));
            if($flag) 
            {
                return 'Success Build: petskilltemplateinfo' . PHP_EOL;
            }
            else
            {
                return 'Fail Build: petskilltemplateinfo' . PHP_EOL;
            }
        }
        catch(Exception $ex)
        {
            die($ex->getMessage());
        }
    }

    private function createShopBoxList() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $rootNode2 = $rootNode->addChild('ItemTemplate');
            $controle = new ControleItens();
            $items = $controle->GetAllShopBox();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode2, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/shopbox.xml",($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: shopbox' . PHP_EOL;
            } else {
                return 'Fail Build: shopbox' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function createShopItemList() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $rootNode2 = $rootNode->addChild('Store');
            $controle = new ControleItens();
            $items = $controle->GetALllShop();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode2, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/ShopItemList.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: ShopItemList' . PHP_EOL;
            } else {
                return 'Fail Build: ShopItemList' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function createShopGoodsShowList() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $rootNode2 = $rootNode->addChild('Store');
            $controle = new ControleItens();
            $items = $controle->GetAllShopGoodsShowList();
            foreach ($items as $value) {
                XMLSerializer::createGeneric($rootNode2, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/ShopGoodsShowList.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: ShopGoodsShowList' . PHP_EOL;
            } else {
                return 'Fail Build: ShopGoodsShowList' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function createServerConfig() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleServer();
            $configs = $controle->getServerConfig();
            foreach ($configs as $value) {
                $node_item = $rootNode->addChild('Item');
                $node_item->addAttribute('Name', $value['Name']);
                $node_item->addAttribute('Value', $value['Value']);
            }
            $flag = file_put_contents("./filesRequest/serverconfig.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: ServerConfig' . PHP_EOL;
            } else {
                return 'Fail Build: ServerConfig' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function createMapServer() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleServer();
            $maps = $controle->getMapServer();
            foreach ($maps as $value) {
                XMLSerializer::createGeneric($rootNode, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/MapServerList.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: MapServer' . PHP_EOL;
            } else {
                return 'Fail Build: MapServer' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function createMapList() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleServer();
            $maps = $controle->getMapList();
            foreach ($maps as $value) {
                XMLSerializer::createGeneric($rootNode, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/LoadMapsItems.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: MapList' . PHP_EOL;
            } else {
                return 'Fail Build: MapList' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function createTemplateAllList() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $child = $rootNode->addChild('ItemTemplate');
            $controle = new ControleItens();
            $itens = $controle->GetAllGoods();
            foreach ($itens as $value) {
                XMLSerializer::createGeneric($child, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/TemplateAllList.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: TemplateAllList' . PHP_EOL;
            } else {
                return 'Fail Build: TemplateAllList' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function createActiveList() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleActives();
            $actives = $controle->GetAllActives();
            foreach ($actives as $value) {
                XMLSerializer::createGeneric($rootNode, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/ActiveList.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: ActiveList' . PHP_EOL;
            } else {
                return 'Fail Build: ActiveList' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function createAllActivitySystemItem() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleActives();
            $actives = $controle->GetAllActivitySystemItem();
            foreach ($actives as $value) {
                XMLSerializer::createGeneric($rootNode, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/activitysystemitems.xml", gzcompress($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: activitysystemitems' . PHP_EOL;
            } else {
                return 'Fail Build: activitysystemitems' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

    private function createAllActiveConvertItems() {
        try {
            $rootNode = new SimpleXMLElement('<Result></Result>');
            $rootNode->addAttribute("value", "true");
            $rootNode->addAttribute("message", "Success!");
            $controle = new ControleActives();
            $actives = $controle->GetAllActiveConvertItems();
            foreach ($actives as $value) {
                XMLSerializer::createGeneric($rootNode, 'Item', $value);
            }
            $flag = file_put_contents("./filesRequest/ActiveConvertItemInfo.xml", ($rootNode->asXML()));
            if ($flag) {
                return 'Success Build: ActiveConvertItemInfo' . PHP_EOL;
            } else {
                return 'Fail Build: ActiveConvertItemInfo' . PHP_EOL;
            }
        } catch (Exception $ex) {
            die($ex->getMessage());
        }
    }

}
