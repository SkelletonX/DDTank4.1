<?php
$thispath = "D:\\SKELLETONX\\DDTank\\DDTank4.1\\Web\\Models\\";

//D:\SKELLETONX\DDTank\DDTank4.1\Web\Models

$diretorio = dir($thispath);

while($arquivo = $diretorio -> read()){
if($arquivo != "index.php" && $arquivo != "." && $arquivo != "..")
{
    include($diretorio->path.$arquivo);
}
}
$diretorio -> close();
?>