<?php

include("../db.php");
include("../common.php");


$request = $_SERVER['REQUEST_METHOD'];

switch ($request) {
	case "GET":
		$reservations = getReservations();
		echo json_encode($reservations);
		break;
	case "POST":
		$content = file_get_contents('php://input');
		$data = json_decode($content, true);

		$name = $data["reservator"];
		$seatrow = $data["rownum"];
		$seatcolumn = $data["columnnum"];

		addReservation($name, $seatrow,$seatcolumn);
		break;
	case "PUT":
		$content = file_get_contents('php://input');
		$data = json_decode($content, true);
		$user = checkLoggedIn($data["username"], $data["password"]);
		if (!$user) {
			header('HTTP/1.0 401 Unauthorized ');
			break;
		}
		updReservation($data["id"],$data["reservedBy"],$data["seatRow"],$data["seatColumn"]);
		break;
	case "DELETE":
		$content = file_get_contents('php://input');
		$data = json_decode($content, true);
		$user = checkLoggedIn($data["username"], $data["password"]);
		if (!$user) {
			header('HTTP/1.0 401 Unauthorized ');
			break;
		}
		delReservation($data["id"]);
		break;
	default:
		header('HTTP/1.1 405 Method Not Allowed');
		header('Allow: GET, POST, PUT, DELETE');
		break;
}



function getReservations() {
	global $con;
	
	$result = $con -> query("SELECT id, reservedBy, seatRow, seatColumn FROM reservation");
	
	return $result->fetch_all(MYSQLI_ASSOC);
}

function addReservation($name,$seatrow,$seatcolumn) {
	global $con;
	
	$result = $con -> query("INSERT INTO reservation(reservedBy,seatRow,seatColumn) values('$name',$seatrow,$seatcolumn)");
}

function delReservation($id) {
	global $con;

	$con -> query("DELETE FROM reservation WHERE id = '$id'");
}
function updReservation($id,$name,$seatrow,$seatcolumn) {
	global $con;

	$con -> query("UPDATE reservation SET reservedBy = '$name', seatRow = $seatrow, seatColumn = $seatcolumn  WHERE id = '$id'");
	echo "Successfully modified";
}

?>