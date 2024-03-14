-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1:3306
-- Létrehozás ideje: 2024. Már 14. 08:24
-- Kiszolgáló verziója: 8.2.0
-- PHP verzió: 8.2.13

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `connect2gether`
--
CREATE DATABASE IF NOT EXISTS `connect2gether` DEFAULT CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci;
USE `connect2gether`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `alertmessage`
--

DROP TABLE IF EXISTS `alertmessage`;
CREATE TABLE IF NOT EXISTS `alertmessage` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` varchar(128) COLLATE utf8mb4_hungarian_ci NOT NULL,
  `UserId` int NOT NULL,
  `Description` varchar(128) COLLATE utf8mb4_hungarian_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `comment`
--

DROP TABLE IF EXISTS `comment`;
CREATE TABLE IF NOT EXISTS `comment` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Text` text COLLATE utf8mb4_hungarian_ci NOT NULL,
  `PostId` int NOT NULL,
  `UserId` int NOT NULL,
  `CommentId` int NOT NULL,
  `uploadDate` date DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CommentId` (`CommentId`),
  KEY `UserId` (`UserId`),
  KEY `PostId` (`PostId`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `images`
--

DROP TABLE IF EXISTS `images`;
CREATE TABLE IF NOT EXISTS `images` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Image1` mediumblob NOT NULL,
  `Image2` mediumblob NOT NULL,
  `Image3` mediumblob NOT NULL,
  `Image4` mediumblob NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `liked_posts`
--

DROP TABLE IF EXISTS `liked_posts`;
CREATE TABLE IF NOT EXISTS `liked_posts` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserID` int NOT NULL,
  `PostID` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserID` (`UserID`),
  KEY `PostID` (`PostID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `permissions`
--

DROP TABLE IF EXISTS `permissions`;
CREATE TABLE IF NOT EXISTS `permissions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(128) COLLATE utf8mb4_hungarian_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `permissions`
--

INSERT INTO `permissions` (`Id`, `Name`) VALUES
(1, 'Default'),
(3, 'Admin');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `ranks`
--

DROP TABLE IF EXISTS `ranks`;
CREATE TABLE IF NOT EXISTS `ranks` (
  `Id` int NOT NULL,
  `Name` varchar(128) COLLATE utf8mb4_hungarian_ci NOT NULL,
  `Min_pont` int NOT NULL,
  `Max_pont` int NOT NULL,
  `Description` text COLLATE utf8mb4_hungarian_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user`
--

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(128) COLLATE utf8mb4_hungarian_ci NOT NULL,
  `HASH` varchar(128) COLLATE utf8mb4_hungarian_ci NOT NULL,
  `Email` varchar(320) COLLATE utf8mb4_hungarian_ci NOT NULL,
  `ActiveUser` tinyint(1) NOT NULL,
  `RankId` int NOT NULL COMMENT 'Pontszámhoz kötött rangok',
  `RegistrationDate` date NOT NULL,
  `Point` int NOT NULL COMMENT 'Pontszám',
  `PermissionId` int NOT NULL COMMENT 'Felhasználói szint',
  `LastLogin` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Username` (`Username`),
  KEY `RankId` (`RankId`),
  KEY `PermissionId` (`PermissionId`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `user`
--

INSERT INTO `user` (`Id`, `Username`, `HASH`, `Email`, `ActiveUser`, `RankId`, `RegistrationDate`, `Point`, `PermissionId`, `LastLogin`) VALUES
(4, 'balint', '$2a$04$DdxDsmRsjB.1e/GDo2gEhOcM57NZjJ0KK15OVPgrth7PJswAaUBHW', 'Balint@gmail.com', 1, 0, '2024-02-20', 0, 3, '2024-03-14 08:26:40'),
(5, 'balazs', '$2a$04$1sGMmURlGttgS0NAzBTGoe/jB18eV2zUM22Lco41xoYNeGwmqYLR6', 'Balazs@gmail.com', 0, 0, '2024-02-20', 0, 1, '0001-01-01 00:00:00'),
(6, 'bazsi', '$2a$04$e3Ejfu8mDeLzv6zw9dn8LORIUa5htWaAhpbBiyEqhcCunG2Z5gudW', 'jb@gmail.com', 0, 0, '2024-02-27', 0, 3, '0001-01-01 00:00:00'),
(11, 'balintUser', '$2a$04$LMBdSbri8cs1lR4OSkCX..SE6WriXrIE.qm4XwbjRJR3rqbyruU9a', 'balintUser@gmail.com', 0, 0, '2024-03-12', 0, 1, '2024-03-14 08:48:40');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user_post`
--

DROP TABLE IF EXISTS `user_post`;
CREATE TABLE IF NOT EXISTS `user_post` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ImageId` int DEFAULT NULL,
  `Description` text COLLATE utf8mb4_hungarian_ci NOT NULL,
  `Title` varchar(128) COLLATE utf8mb4_hungarian_ci NOT NULL,
  `Like` bigint NOT NULL DEFAULT '0',
  `UserId` int DEFAULT NULL,
  `uploadDate` date DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `ImageId_2` (`ImageId`,`UserId`),
  KEY `ImageId` (`ImageId`),
  KEY `UserId` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `user_post`
--

INSERT INTO `user_post` (`Id`, `ImageId`, `Description`, `Title`, `Like`, `UserId`, `uploadDate`) VALUES
(29, NULL, 'dsgdsmvlkdsmcymxlkmymk', 'dyrtewsvdsgvsdvsvxcv', 0, 4, NULL);

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `alertmessage`
--
ALTER TABLE `alertmessage`
  ADD CONSTRAINT `alertmessage_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`);

--
-- Megkötések a táblához `comment`
--
ALTER TABLE `comment`
  ADD CONSTRAINT `comment_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`),
  ADD CONSTRAINT `comment_ibfk_2` FOREIGN KEY (`PostId`) REFERENCES `user_post` (`Id`);

--
-- Megkötések a táblához `images`
--
ALTER TABLE `images`
  ADD CONSTRAINT `images_ibfk_1` FOREIGN KEY (`Id`) REFERENCES `user_post` (`ImageId`);

--
-- Megkötések a táblához `liked_posts`
--
ALTER TABLE `liked_posts`
  ADD CONSTRAINT `liked_posts_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `user` (`Id`),
  ADD CONSTRAINT `liked_posts_ibfk_2` FOREIGN KEY (`PostID`) REFERENCES `user_post` (`Id`);

--
-- Megkötések a táblához `ranks`
--
ALTER TABLE `ranks`
  ADD CONSTRAINT `ranks_ibfk_1` FOREIGN KEY (`Id`) REFERENCES `user` (`RankId`);

--
-- Megkötések a táblához `user`
--
ALTER TABLE `user`
  ADD CONSTRAINT `user_ibfk_2` FOREIGN KEY (`PermissionId`) REFERENCES `permissions` (`Id`);

--
-- Megkötések a táblához `user_post`
--
ALTER TABLE `user_post`
  ADD CONSTRAINT `user_post_ibfk_3` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
