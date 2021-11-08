<?php
ini_set('display_errors', 1);
$conn                      = null;
$c_host                    = 'SKELLETONX\SQLEXPRESS';
$config['UID']             = 'sa';
$config['PWD']             = '123456';
$config['Database']        = 'Db_Membership';
$config['CharacterSet']    = 'UTF-8';
$dbtank					   = 'Db_Tank';
$dbtank41				   = 'Db_Tank41';
$dbmembership			   = 'Db_Membership';
$RateTimeToCoin  		   = 10; // Quantidade de coins por tempo online.


#--------------------------------------------
#--------------------------------------------
#--------------------------------------------
$LinkLogin				= 'http://127.0.0.1/';
$LinkFlash				= 'http://127.0.0.1/flash/';
$jogando				= 'True Tank'; 
$icon					= '';
$titulo					= 'True Tank';
$description   			= 'Copyright © 2016 DDTankSplush Gamers. Todos os direitos reservados.';
$KeyWords				= 'DDtank,4.1,original';
$grupo					= '#'; 
#--------------------------------------------
#--------------------------------------------
#--------------------------------------------

//add Functions
include('function.php');
co();
//Add Clases
include('./Models/index.php');
#--------------------------------------------
#--------------------------------------------
#--------------------------------------------

#--------------------------------------------
#--------------------------------------------
#--------------------------------------------

$Play[0]			= 'play.php'; //Play com Anuncio
$Play[1]			= 'playvip.php'; //Play com Script

#-----------------------------------------
@session_start();