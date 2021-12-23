<?php

$host="localhost";
$port=3306;
$socket="";
$user="root";
$password = "";
$dbname="pk_oohq3e";

// CONNECT TO DATABASE

$con = new mysqli($host, $user, $password, $dbname, $port, $socket)
    or die ("Connect failed: ".mysqli_connect_error());
    
if ($con->connect_errno > 0) {
    printf("Connect failed: %s\n", $con->connect_error);
    exit();
}

?>