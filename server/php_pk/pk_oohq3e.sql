-- MariaDB dump 10.18  Distrib 10.4.17-MariaDB, for Win64 (AMD64)
--
-- Host: localhost    Database: pk_oohq3e
-- ------------------------------------------------------
-- Server version	10.4.17-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `reservation`
--

DROP TABLE IF EXISTS `reservation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `reservation` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `reservedBy` varchar(50) NOT NULL,
  `seatRow` int(5) NOT NULL,
  `seatColumn` int(5) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=144 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reservation`
--

LOCK TABLES `reservation` WRITE;
/*!40000 ALTER TABLE `reservation` DISABLE KEYS */;
INSERT INTO `reservation` VALUES (9,'Teszt Elek',8,9),(10,'Teszt Elek',9,9),(11,'Teszt Elek',10,9),(18,'Teszt Márk',10,11),(19,'Teszt Márk',11,11),(20,'Teszt Márk',9,11),(21,'Teszt Márk',8,11),(34,'Tesztelő Tamás',11,9),(60,'Reserver',14,7),(61,'Reserver',15,7),(62,'Reserver',15,6),(63,'Reserver',16,6),(64,'Reserver',16,7),(68,'Reserver',8,6),(69,'Reserver',7,6),(70,'Reserver',6,6),(71,'Reserver',15,5),(72,'Reserver',15,4),(73,'Reserver',16,4),(74,'Reserver',16,5),(84,'tesztelek',17,7),(85,'tesztelek',17,8),(86,'tesztelek',16,8),(87,'tesztelek',15,8),(88,'tesztelek',14,8),(89,'tesztelek',13,8),(90,'tesztelek',13,7),(91,'tesztelek',17,6),(92,'tesztelek',17,5),(93,'tesztelek',17,4),(94,'tesztelek',18,7),(95,'tesztelek',18,8),(96,'tesztelek',19,7),(97,'tesztelek',19,6),(98,'Tesztelő Tamás',11,8),(99,'Tesztelő Tamás',10,8),(100,'Tesztelő Tamás',10,7),(101,'asd',9,2),(102,'asd',10,1),(103,'asdasd',15,2),(104,'asdasd',15,3),(105,'asdasd',16,3),(106,'asdasd',16,2),(108,'asdasd',0,0),(109,'asdasd',17,0),(110,'asdasd',16,0),(111,'Teszt Elemér',18,5),(112,'Teszt Elemér',19,5),(114,'Teszt Elemér',18,6),(115,'Teszt Elemér',19,4),(116,'Teszt Elemér',18,3),(117,'Teszt Elemér',19,3),(118,'Teszt Elemér',18,4),(119,'Teszt Elemér',18,2),(120,'Teszt Elemér',19,2),(121,'Teszt Elemér',18,1),(122,'Teszt Elemér',5,0),(123,'Teszt Elemér',18,0),(124,'Teszt Elemér',19,1),(125,'Teszt Elemér',17,1),(126,'Teszt Elemér',17,3),(127,'Teszt Elemér',17,2),(128,'Teszt Elemér',15,0),(129,'Teszt Elemér',14,0),(130,'Teszt Elemér',13,0),(131,'Teszt Elemér',14,1),(132,'Teszt Elemér',13,1),(133,'Teszt Elemér',13,2),(134,'Teszt Elemér',14,2),(135,'Teszt Elemér',14,3),(136,'Teszt Elemér',13,3),(137,'Teszt Elemér',13,4),(138,'Teszt Elemér',14,4),(139,'Teszt Elemér',14,5),(140,'Teszt Elemér',13,5),(141,'Teszt Elemér',13,6),(142,'Teszt Elemér',14,6),(143,'asd',19,0);
/*!40000 ALTER TABLE `reservation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `password` varchar(60) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'admin','21232f297a57a5a743894a0e4a801fc3');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-12-23 18:52:41
