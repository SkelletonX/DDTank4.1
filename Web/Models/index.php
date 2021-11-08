<?php
$thispath = "D:\\SKELLETONX\\DDTank\\TrueTank\\site\\Models\\";
$diretorio = dir($thispath);

while($arquivo = $diretorio -> read()){
if($arquivo != "index.php" && $arquivo != "." && $arquivo != "..")
{
    include($diretorio->path.$arquivo);
}
}
$diretorio -> close();
?>