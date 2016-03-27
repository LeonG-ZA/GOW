<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<style type="text/css">
#map
{
background:#ffa url("./maps/Felucca.jpg") no-repeat;
width:12288px;
height:8192px;
}
.player
{
background:url("./mark.gif") no-repeat;
width:35px;
height:34px;
}
</style>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<title>Felucca</title>
</head>
<body>
	<div id="map">
<?php
$xml = simplexml_load_file("playersPosistion.xml");

foreach ($xml->children() as $child)
  {
	  foreach ($child->children() as $child1)
	  {
		switch($child1->getName())
		{
			case "name":
				$name= $child1;
				break;
			case "x":
				$x= $child1*2;
				break;
			case "y":
				$y= $child1*2;
				break;
			case "map":
				$map=$child1;
				break;
			default:
				echo "ololooo error";
		}
	   }
	if($map=="Felucca")
	{
	 echo "<div class=\"player\" title=\"".$name."\" style=\"position:absolute;top:".$y."px;left:".$x."px\"></div>";
	}
  }
 
?>
	</div>
</body>
</html>
