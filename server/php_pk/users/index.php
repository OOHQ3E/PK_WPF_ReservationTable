<?php

include("../db.php");


$request = $_SERVER['REQUEST_METHOD'];

switch ($request) {
	case "GET":
		if (!empty($_GET["username"])) {
			$users = login($_GET["username"], $_GET["password"]);
		}
		else {
			$users = getUsers();
		}
		echo json_encode($users);
		break;
	default:
		header('HTTP/1.1 405 Method Not Allowed');
		header('Allow: GET, POST, PUT, DELETE');
		break;
}
function getUsers(){
	global $con;
	$result = $con -> query("SELECT id, name, password FROM user");
	return $result->fetch_all(MYSQLI_ASSOC);
}

function login($u, $p) {
	global $con;
	
	$result = $con -> query("SELECT id, name, password FROM user WHERE name = '$u' AND password = MD5('$p')");
	
	return $result->fetch_all(MYSQLI_ASSOC)[0];
}


?>