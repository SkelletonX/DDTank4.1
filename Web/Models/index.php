<?php
$thispath = "E:\\SKELLETONX\\GITHUB\\DDTank4.1\\Web\\Models\\";
$diretorio = dir($thispath);

while($arquivo = $diretorio -> read()){
if($arquivo != "index.php" && $arquivo != "." && $arquivo != "..")
{
    include($diretorio->path.$arquivo);
}
}
$diretorio -> close();
?>