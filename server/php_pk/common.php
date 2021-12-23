<?php

// Check if user logged in
// Parameters: username, hashed pw
function checkLoggedIn($u, $p) {
	global $con;
	
	// Perform query
	$result = $con -> query("SELECT id, name, password FROM user WHERE name = '$u' AND password = '$p'");
	
	return $result->fetch_all(MYSQLI_ASSOC)[0];
}

?>