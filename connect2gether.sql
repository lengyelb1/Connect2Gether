-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1:3306
-- Létrehozás ideje: 2024. Ápr 30. 15:18
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
CREATE DATABASE IF NOT EXISTS `connect2gether` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `alertmessage`
--

INSERT INTO `alertmessage` (`Id`, `Title`, `UserId`, `Description`) VALUES
(20, 'Asd', 1, 'asd');

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
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `comment`
--

INSERT INTO `comment` (`Id`, `Text`, `PostId`, `UserId`, `CommentId`, `uploadDate`) VALUES
(29, 'Wow, amazing post! Keep up the good work!', 1, 4, 0, '2024-04-09'),
(30, 'This is so insightful, thanks for sharing!', 1, 5, 0, '2024-04-01'),
(31, 'I never thought about it this way before, great perspective!', 2, 4, 0, '2024-02-14'),
(32, 'Interesting topic, I\'ll have to look into it more.', 2, 7, 0, '2023-11-15'),
(33, 'Love the creativity in this post, it\'s truly inspiring!', 3, 2, 0, '2024-02-08'),
(34, 'Thanks for the information, it\'s really helpful!', 5, 1, 0, '2024-03-23'),
(35, 'I can relate so much to what you\'re saying, great job!', 6, 2, 0, '2024-04-11'),
(36, 'The way you explain things is so clear and concise.', 5, 3, 0, '2024-04-14'),
(37, 'Awesome content, I always enjoy reading your posts!', 4, 6, 0, '2024-04-13'),
(38, 'I\'m blown away by how thought-provoking this is, thank you!', 8, 5, 0, '2024-04-18');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `deletedlikes`
--

DROP TABLE IF EXISTS `deletedlikes`;
CREATE TABLE IF NOT EXISTS `deletedlikes` (
  `id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `postId` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `UserId` (`UserId`) USING BTREE,
  KEY `postId` (`postId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `disliked_posts`
--

DROP TABLE IF EXISTS `disliked_posts`;
CREATE TABLE IF NOT EXISTS `disliked_posts` (
  `id` int NOT NULL AUTO_INCREMENT,
  `userid` int NOT NULL,
  `postid` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `userid` (`userid`,`postid`),
  KEY `postid` (`postid`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

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
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=utf8mb3;

--
-- A tábla adatainak kiíratása `liked_posts`
--

INSERT INTO `liked_posts` (`Id`, `UserID`, `PostID`) VALUES
(44, 5, 6),
(45, 7, 6),
(46, 2, 8);

--
-- Eseményindítók `liked_posts`
--
DROP TRIGGER IF EXISTS `likeDelete`;
DELIMITER $$
CREATE TRIGGER `likeDelete` BEFORE DELETE ON `liked_posts` FOR EACH ROW INSERT INTO `deletedlikes` (`UserId`, `postId`) 
SELECT OLD.UserId, OLD.postId FROM DUAL 
WHERE NOT EXISTS (SELECT * FROM `deletedLikes` 
      WHERE `UserId`=old.UserID AND `postId`=OLD.postId LIMIT 1)
$$
DELIMITER ;

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
(2, 'Moderator'),
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

--
-- A tábla adatainak kiíratása `ranks`
--

INSERT INTO `ranks` (`Id`, `Name`, `Min_pont`, `Max_pont`, `Description`) VALUES
(1, 'Kezdő', 0, 10, 'Kezdő rank'),
(2, 'Haladó', 11, 20, 'Haladó rank'),
(3, 'Fél profi', 21, 30, 'Fél profi rank'),
(4, 'Profi', 31, 40, 'Profi rank'),
(5, 'Legenda', 41, 50, 'Legenda rank');

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
  `ProfileImage` mediumblob,
  `ValidatedKey` text COLLATE utf8mb4_hungarian_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Username` (`Username`),
  KEY `RankId` (`RankId`),
  KEY `PermissionId` (`PermissionId`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `user`
--

INSERT INTO `user` (`Id`, `Username`, `HASH`, `Email`, `ActiveUser`, `RankId`, `RegistrationDate`, `Point`, `PermissionId`, `LastLogin`, `ProfileImage`, `ValidatedKey`) VALUES
(1, 'felhasznalo1', '$2a$04$/quw9t7VqhWaO.tks5Q2L.q3JIFmehNUj5TAbZ6He/aOBoDTfNBeW', 'felhasznalo1@example.com', 1, 2, '2024-04-30', 100, 1, '2024-04-30 14:05:55', NULL, NULL),
(2, 'felhasznalo2', '$2a$04$/quw9t7VqhWaO.tks5Q2L.q3JIFmehNUj5TAbZ6He/aOBoDTfNBeW', 'felhasznalo2@example.com', 1, 1, '2024-04-29', 50, 1, '2024-04-29 10:15:30', NULL, NULL),
(3, 'felhasznalo3', '$2a$04$/quw9t7VqhWaO.tks5Q2L.q3JIFmehNUj5TAbZ6He/aOBoDTfNBeW', 'felhasznalo3@example.com', 1, 3, '2024-04-28', 150, 1, '2024-04-28 16:25:10', NULL, NULL),
(4, 'username4', '$2a$04$/quw9t7VqhWaO.tks5Q2L.q3JIFmehNUj5TAbZ6He/aOBoDTfNBeW', 'email1@example.com', 1, 1, '2024-04-30', 100, 1, '2024-04-30 15:17:29', NULL, NULL),
(5, 'username5', '$2a$04$/quw9t7VqhWaO.tks5Q2L.q3JIFmehNUj5TAbZ6He/aOBoDTfNBeW', 'email2@example.com', 1, 1, '2024-04-30', 100, 1, '2024-04-30 00:00:00', NULL, NULL),
(6, 'username6', '$2a$04$/quw9t7VqhWaO.tks5Q2L.q3JIFmehNUj5TAbZ6He/aOBoDTfNBeW', 'email3@example.com', 1, 1, '2024-04-30', 100, 1, '2024-04-30 00:00:00', NULL, NULL),
(7, 'username7', '$2a$04$/quw9t7VqhWaO.tks5Q2L.q3JIFmehNUj5TAbZ6He/aOBoDTfNBeW', 'email4@example.com', 1, 1, '2024-04-30', 100, 1, '2024-04-30 00:00:00', NULL, NULL),
(45, 'balint', '$2a$04$RRo1qJgnIwS6mdJprBUENOv2D.DSRRkjnB6eI01dPzgs6PkHITcmi', 'lengyelb@kkszki.hu', 1, 1, '2024-04-30', 0, 3, '2024-04-30 15:19:12', NULL, NULL);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user_post`
--

DROP TABLE IF EXISTS `user_post`;
CREATE TABLE IF NOT EXISTS `user_post` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Image` mediumblob,
  `Description` text COLLATE utf8mb4_hungarian_ci NOT NULL,
  `Title` varchar(128) COLLATE utf8mb4_hungarian_ci NOT NULL,
  `Like` bigint NOT NULL DEFAULT '0',
  `Dislike` int NOT NULL,
  `UserId` int DEFAULT NULL,
  `uploadDate` date DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `user_post`
--

INSERT INTO `user_post` (`Id`, `Image`, `Description`, `Title`, `Like`, `Dislike`, `UserId`, `uploadDate`) VALUES
(1, NULL, 'Description of post 1', 'Title of post 1', 10, 2, 1, '2024-04-30'),
(2, NULL, 'Description of post 2', 'Title of post 2', 8, 1, 2, '2024-04-30'),
(3, NULL, 'Description of post 3', 'Title of post 3', 15, 3, 3, '2024-04-30'),
(4, NULL, 'Description of post 4', 'Title of post 4', 20, 5, 4, '2024-04-30'),
(5, NULL, 'Description of post 5', 'Title of post 5', 17, 2, 1, '2023-07-30'),
(6, NULL, 'Description of post 6', 'Title of post 6', 4, 1, 2, '2022-03-30'),
(7, NULL, 'Description of post 7', 'Title of post 7', 12, 3, 3, '2023-04-30'),
(8, NULL, 'Description of post 8', 'Title of post 8', 26, 5, 4, '2022-04-22');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user_suspicious`
--

DROP TABLE IF EXISTS `user_suspicious`;
CREATE TABLE IF NOT EXISTS `user_suspicious` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `Description` text NOT NULL,
  `Message` text NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserId` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb3;

--
-- A tábla adatainak kiíratása `user_suspicious`
--

INSERT INTO `user_suspicious` (`Id`, `UserId`, `Description`, `Message`) VALUES
(9, 1, 'spam post', '');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user_token`
--

DROP TABLE IF EXISTS `user_token`;
CREATE TABLE IF NOT EXISTS `user_token` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `Token` text COLLATE utf8mb4_hungarian_ci NOT NULL,
  `Token_expire_date` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `user_token`
--

INSERT INTO `user_token` (`Id`, `UserId`, `Token`, `Token_expire_date`) VALUES
(42, 4, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoidXNlcm5hbWU0Iiwicm9sZSI6IkRlZmF1bHQiLCJpZCI6IjQiLCJleHAiOjE3MTQ1Njk0NDksImlzcyI6ImF1dGgtYXBpIiwiYXVkIjoiYXV0aC1jbGllbnQifQ.bHTvmpMEEUohAR3C_7zOpXT6snHbSnJV-WUxZMb4JARrO8O2uIUA2KlL9WicNWU0-nh0UlWACYmrPAawteeRww', '2024-05-01 15:17:29'),
(43, 45, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmFsaW50Iiwicm9sZSI6IkRlZmF1bHQiLCJpZCI6IjQ1IiwiZXhwIjoxNzE0NTY5NTA5LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.h7tcp_N9FHJXmyrxSxtxWo5fEgFliofXT534SIwV6kv0eyeccSzZCTWM_MC0-dbIlwe4wUoRpQjVEDiGYeQhZw', '2024-05-01 15:18:29'),
(44, 45, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmFsaW50Iiwicm9sZSI6IkFkbWluIiwiaWQiOiI0NSIsImV4cCI6MTcxNDU2OTUzMSwiaXNzIjoiYXV0aC1hcGkiLCJhdWQiOiJhdXRoLWNsaWVudCJ9.2MhEwMCpq9ttFiDrlbyl4Rd_o6YLi0SLTD-Z71eEjYkFdFAfBbIeBDyHgeHLf-mJWBJ4XnWcHEYa9pC9146aYA', '2024-05-01 15:18:52'),
(45, 45, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmFsaW50Iiwicm9sZSI6IkFkbWluIiwiaWQiOiI0NSIsImV4cCI6MTcxNDU2OTU1MSwiaXNzIjoiYXV0aC1hcGkiLCJhdWQiOiJhdXRoLWNsaWVudCJ9.Lz7mGCIn7iqNxVS6hnmbdUYOcw7Gfda-xdrm0C74uIrJyQi9NIXGBvRcjIJsTd_w0N76TW6p0TwrITEKQMgN3g', '2024-05-01 15:19:12');

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
  ADD CONSTRAINT `comment_ibfk_3` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `comment_ibfk_4` FOREIGN KEY (`PostId`) REFERENCES `user_post` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `deletedlikes`
--
ALTER TABLE `deletedlikes`
  ADD CONSTRAINT `deletedlikes_ibfk_2` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `deletedlikes_ibfk_3` FOREIGN KEY (`postId`) REFERENCES `user_post` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `disliked_posts`
--
ALTER TABLE `disliked_posts`
  ADD CONSTRAINT `disliked_posts_ibfk_1` FOREIGN KEY (`userid`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `disliked_posts_ibfk_2` FOREIGN KEY (`postid`) REFERENCES `user_post` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `liked_posts`
--
ALTER TABLE `liked_posts`
  ADD CONSTRAINT `liked_posts_ibfk_3` FOREIGN KEY (`UserID`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `liked_posts_ibfk_4` FOREIGN KEY (`PostID`) REFERENCES `user_post` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `user`
--
ALTER TABLE `user`
  ADD CONSTRAINT `user_ibfk_2` FOREIGN KEY (`PermissionId`) REFERENCES `permissions` (`Id`),
  ADD CONSTRAINT `user_ibfk_3` FOREIGN KEY (`RankId`) REFERENCES `ranks` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `user_post`
--
ALTER TABLE `user_post`
  ADD CONSTRAINT `user_post_ibfk_3` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`);

--
-- Megkötések a táblához `user_suspicious`
--
ALTER TABLE `user_suspicious`
  ADD CONSTRAINT `user_suspicious_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `user_token`
--
ALTER TABLE `user_token`
  ADD CONSTRAINT `user_token_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

DELIMITER $$
--
-- Események
--
DROP EVENT IF EXISTS `token_delete`$$
CREATE DEFINER=`root`@`localhost` EVENT `token_delete` ON SCHEDULE EVERY 1 HOUR STARTS '2024-03-17 19:33:00' ON COMPLETION NOT PRESERVE ENABLE DO DELETE FROM user_token
WHERE user_token.token_expire_date < NOW()$$

DROP EVENT IF EXISTS `deleteExpiredTokens`$$
CREATE DEFINER=`root`@`localhost` EVENT `deleteExpiredTokens` ON SCHEDULE EVERY 1 MINUTE STARTS '2024-02-19 08:42:52' ON COMPLETION NOT PRESERVE DISABLE COMMENT 'Clears out sessions table each hour.' DO DELETE FROM user_token WHERE user_token.token_expire_date < NOW()$$

DELIMITER ;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
