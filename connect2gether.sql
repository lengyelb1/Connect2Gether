-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2024. Ápr 15. 10:12
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

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

CREATE TABLE IF NOT EXISTS `alertmessage` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(128) NOT NULL,
  `UserId` int(11) NOT NULL,
  `Description` varchar(128) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `alertmessage`
--

INSERT INTO `alertmessage` (`Id`, `Title`, `UserId`, `Description`) VALUES
(1, 'string', 17, 'string'),
(2, 'string', 17, 'string'),
(5, 'string', 18, 'string'),
(6, 'string', 19, 'string'),
(7, 'string', 18, 'string'),
(8, 'string', 18, 'string'),
(9, 'string', 18, 'string');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `comment`
--

CREATE TABLE IF NOT EXISTS `comment` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Text` text NOT NULL,
  `PostId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `CommentId` int(11) NOT NULL,
  `uploadDate` date DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CommentId` (`CommentId`),
  KEY `UserId` (`UserId`),
  KEY `PostId` (`PostId`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `images`
--

CREATE TABLE IF NOT EXISTS `images` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Image1` mediumblob NOT NULL,
  `Image2` mediumblob NOT NULL,
  `Image3` mediumblob NOT NULL,
  `Image4` mediumblob NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `images`
--

INSERT INTO `images` (`Id`, `Image1`, `Image2`, `Image3`, `Image4`) VALUES
(1, '', '', '', '');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `liked_posts`
--

CREATE TABLE IF NOT EXISTS `liked_posts` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `PostID` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserID` (`UserID`),
  KEY `PostID` (`PostID`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `liked_posts`
--

INSERT INTO `liked_posts` (`Id`, `UserID`, `PostID`) VALUES
(7, 18, 31);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `permissions`
--

CREATE TABLE IF NOT EXISTS `permissions` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(128) NOT NULL,
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

CREATE TABLE IF NOT EXISTS `ranks` (
  `Id` int(11) NOT NULL,
  `Name` varchar(128) NOT NULL,
  `Min_pont` int(11) NOT NULL,
  `Max_pont` int(11) NOT NULL,
  `Description` text NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `ranks`
--

INSERT INTO `ranks` (`Id`, `Name`, `Min_pont`, `Max_pont`, `Description`) VALUES
(1, 'Noob', 0, 10, 'szoveg');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user`
--

CREATE TABLE IF NOT EXISTS `user` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(128) NOT NULL,
  `HASH` varchar(128) NOT NULL,
  `Email` varchar(320) NOT NULL,
  `ActiveUser` tinyint(1) NOT NULL,
  `RankId` int(11) NOT NULL COMMENT 'Pontszámhoz kötött rangok',
  `RegistrationDate` date NOT NULL,
  `Point` int(11) NOT NULL COMMENT 'Pontszám',
  `PermissionId` int(11) NOT NULL COMMENT 'Felhasználói szint',
  `LastLogin` datetime NOT NULL,
  `ValidatedKey` text NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Username` (`Username`),
  KEY `RankId` (`RankId`),
  KEY `PermissionId` (`PermissionId`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `user`
--

INSERT INTO `user` (`Id`, `Username`, `HASH`, `Email`, `ActiveUser`, `RankId`, `RegistrationDate`, `Point`, `PermissionId`, `LastLogin`, `ValidatedKey`) VALUES
(16, 'bazsi', '$2a$04$Q0C3ffL38mWpwMz1okBehOJxe53Lr8r2dWjyKDJm71av6Ems.43iC', 'jb@gmail.com', 1, 1, '2024-03-18', 0, 3, '2024-04-15 08:06:15', ''),
(17, 'balint', '$2a$04$/quw9t7VqhWaO.tks5Q2L.q3JIFmehNUj5TAbZ6He/aOBoDTfNBeW', 'string', 0, 1, '2024-03-18', 0, 3, '0001-01-01 00:00:00', ''),
(18, 'balintUser', '$2a$04$echo9khsZ8hNJWAITgbcpuUQSxGHdP50U16x1IobDHB2kAi8h6C9O', 'string@gmail.com', 0, 1, '2024-03-18', 0, 1, '2024-03-25 15:35:15', ''),
(19, 'bazsiUser', '$2a$11$CjA9IpkMvxYs9hgjHr4Mk.O.TuW6CvXQXwAlMIBWBFVSfXK3DWqBq', 'juhaszbazsi13@gmail.com', 1, 1, '2024-03-18', 0, 1, '2024-04-09 08:06:45', ''),
(21, 'valami', '$2a$04$I5xhS9IIXDeLgWqUXKTxhuObH1PA7rW453bFqik75FYyIXGESU0he', 'valami@gmail.com', 1, 1, '2024-04-03', 0, 1, '2024-04-03 11:31:30', ''),
(27, 'viktor', '$2a$04$or979DyWbZAUeiC2T9lRW.r1XU.TRmBJd9BdIvyevvMq9MHK5z89u', 'tviktor20000717@gmail.com', 1, 1, '2024-04-11', 0, 2, '2024-04-15 08:13:43', ''),
(28, 'adam', '$2a$04$FL.fQGy/HOLmlZurcZmJtuwSqGusnZNo1ZRwJuf5boEbBc9VCoXB2', 'juhaszbazsi13@gmail.com', 0, 1, '2024-04-15', 0, 1, '0001-01-01 00:00:00', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIwIiwidXNlcm5hbWUiOiJhZGFtIiwibmJmIjoxNzEzMTY2MDI3LCJleHAiOjE3MTMyNTI0MjcsImlhdCI6MTcxMzE2NjAyNywiaXNzIjoiYXV0aC1hcGkiLCJhdWQiOiJhdXRoLWNsaWVudCJ9.hceUTyxQyoz3rYueSmJ9D-qt8PtakCGTVctkyjQDVAI'),
(36, 'jacko', '$2a$04$brzNpsopDGP3dN08eXfhGeDBev97pBP5MuMeUn7MejhNHfuTATllC', 'juhaszbazsi13@gmail.com', 1, 1, '2024-04-15', 0, 1, '0001-01-01 00:00:00', '');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user_post`
--

CREATE TABLE IF NOT EXISTS `user_post` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ImageId` int(11) DEFAULT NULL,
  `Description` text NOT NULL,
  `Title` varchar(128) NOT NULL,
  `Like` bigint(20) NOT NULL DEFAULT 0,
  `UserId` int(11) DEFAULT NULL,
  `uploadDate` date DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `ImageId_2` (`ImageId`,`UserId`),
  KEY `ImageId` (`ImageId`),
  KEY `UserId` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `user_post`
--

INSERT INTO `user_post` (`Id`, `ImageId`, `Description`, `Title`, `Like`, `UserId`, `uploadDate`) VALUES
(31, NULL, 'Ez egy post!', 'Post címe', 1, 19, '2024-03-25');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user_suspicious`
--

CREATE TABLE IF NOT EXISTS `user_suspicious` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserId` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `user_suspicious`
--

INSERT INTO `user_suspicious` (`Id`, `UserId`) VALUES
(5, 21);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user_token`
--

CREATE TABLE IF NOT EXISTS `user_token` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` int(11) NOT NULL,
  `Token` text NOT NULL,
  `Token_expire_date` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `user_token`
--

INSERT INTO `user_token` (`Id`, `UserId`, `Token`, `Token_expire_date`) VALUES
(1, 18, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmFsaW50VXNlciIsInJvbGUiOiJEZWZhdWx0IiwiaWQiOiIxOCIsImV4cCI6MTcxMTQ2MzcxNSwiaXNzIjoiYXV0aC1hcGkiLCJhdWQiOiJhdXRoLWNsaWVudCJ9.scftimYX5y1DLde36AOheUnFbhTOsFw0GflNP21BPb07nhtUKv6huwgxOBJis_mpdqhj_zE2DUFmwyX208SN2Q', '2024-03-26 15:35:15'),
(2, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzExNDY0Mjk1LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.gY33XhAakNLTY-xKKRbsvZsRbAXVt3CDTtmFhxEDt34GyQA_5CrtE8a_HvG0P5FiKO-smuCUFITYFoG0n_fzhQ', '2024-03-26 15:44:55'),
(3, 19, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2lVc2VyIiwicm9sZSI6IkRlZmF1bHQiLCJpZCI6IjE5IiwiZXhwIjoxNzExNzQwNTE0LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.Pm-qO0FkHxUccQDcCoVZi42JA3lQQPpKk2qLNE2dw2aFR_xZeBetPnNSaFvyflivh6bZZbdJ2t7fte400pk1ig', '2024-03-29 20:28:34'),
(4, 19, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2lVc2VyIiwicm9sZSI6IkRlZmF1bHQiLCJpZCI6IjE5IiwiZXhwIjoxNzEyMTM1OTEwLCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.xSgsewQxrpF795BRdRSJsG04ine9zwLtZmgq61ZcSPuVBAGJyGZI_lL2yf1r_xiRztq3hMg1lI3DpVajiBx-Fw', '2024-04-03 11:18:30'),
(5, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzEyMTQ4ODg1LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.aDKv-1mPGAnxQGzXS22Jkg5Wg1WgVeaAs4By6O5EvhUM8L_4E4rZnaRw81T9qarRVJgdM6SUa-0Mlav98WE_Ig', '2024-04-03 14:54:45'),
(6, 19, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2lVc2VyIiwicm9sZSI6IkRlZmF1bHQiLCJpZCI6IjE5IiwiZXhwIjoxNzEyMTUyODAxLCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.Ar_gZ_-1h6eiJmeI_0H0h4ZYtqXMVVeA1N7_utEoATh6zIdYRxx7KdaGCgkMW9BFJA6dd7-DVpHctCfROyjYiw', '2024-04-03 16:00:01'),
(7, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzEyMjExMjI4LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.e85qkOCZ7rzqRZ6lf47C6uyfqHHRcav5vsdnalRh7-aKENELFtMAVaeWWDQSd-BQf8oe_311H72ru8fdQftI_A', '2024-04-04 08:13:48'),
(8, 21, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiVmFsYWtpIiwicm9sZSI6IkRlZmF1bHQiLCJpZCI6IjIxIiwiZXhwIjoxNzEyMjE2NDM2LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.HIiQMpoGSSx2Y2KY9_F3rQlvCP_nKMYdZjgydn9Grd7XPW5K-9iVRmp7WKS6csEWjOZZEnuFyGj1k9EwBaflGA', '2024-04-04 09:40:36'),
(9, 21, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiVmFsYWtpIiwicm9sZSI6IkRlZmF1bHQiLCJpZCI6IjIxIiwiZXhwIjoxNzEyMjIzMDkwLCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.gxlN8ttnIAOTdJCjRjd3FEU8YqmXxLUzeYIexd_Aeq6x0pjzxc8Vms6RtOnQWsoXJlPbJ3yu2g1G6BPD8X4WnA', '2024-04-04 11:31:30'),
(10, 19, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2lVc2VyIiwicm9sZSI6IkRlZmF1bHQiLCJpZCI6IjE5IiwiZXhwIjoxNzEyNjQ4MzE0LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.EJpkH--v-Ljsg7tL9gbQl1GM4ZaVyxbWlTUq3DKRWqENbnPMGoAV5b-6HF8YvueeczRY18cPGTpBhvjUKmUyyA', '2024-04-09 09:38:34'),
(11, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzEyNjUyOTQ1LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.gV9w0aum0dUwMyBbxjFqrSsMutDZN85jm3_3sd7fJxzs_hE58MCzNfSPu7CQpceWGfobLm3PfK6mtwO1SJgl2w', '2024-04-09 10:55:45'),
(12, 19, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2lVc2VyIiwicm9sZSI6IkRlZmF1bHQiLCJpZCI6IjE5IiwiZXhwIjoxNzEyNzI5MjA1LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.aHJMjkQ2N4vmfM0rKlsh_1-PM6WFctHhhjP71blSkmLAFNuEyEYYgZER_9ucwQJvn-ZCMFJyLlep_IaCdsRQXg', '2024-04-10 08:06:45'),
(13, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzEyNzI5MzQyLCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.WwdT4BXpzUrPZFkoR1rK6jFlB92tRIa8N2kV_ckbwyp2FXQRbBGUJ_j7j4RRL5gmKZxM_TN-Z17qLFYDFPQi5w', '2024-04-10 08:09:02'),
(14, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzEyOTA3ODQ0LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.FLt8Fp5l5SXHxkR8TeJUDzau2bh1iuZMK9uedWF6IToodc-MOTLOy-BOjdmsFgxfv0X2_2Cq_EWkjCS7uvo-MQ', '2024-04-12 09:44:04'),
(15, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzEzMjQ3NTc1LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.Oy8pixIxtSs1xH-Digo6cgq5eXBQxGDT2zpb3p-q5TQIUjW_7QelvyOrpGhESP0s5EGjbgqahIzvM17HpxH_aw', '2024-04-16 08:06:15'),
(16, 27, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoidmlrdG9yIiwicm9sZSI6Ik1vZGVyYXRvciIsImlkIjoiMjciLCJleHAiOjE3MTMyNDgwMjMsImlzcyI6ImF1dGgtYXBpIiwiYXVkIjoiYXV0aC1jbGllbnQifQ.mFoVsPRmgVoJqZQkuZMM4AuGA2T29kQTWGMFsBvqb9r9IAHSkjp7TJKVsVj5E02LfKO9YmtabYPTDvZmOTdznA', '2024-04-16 08:13:43');

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
  ADD CONSTRAINT `comment_ibfk_2` FOREIGN KEY (`PostId`) REFERENCES `user_post` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `liked_posts`
--
ALTER TABLE `liked_posts`
  ADD CONSTRAINT `liked_posts_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `user` (`Id`),
  ADD CONSTRAINT `liked_posts_ibfk_2` FOREIGN KEY (`PostID`) REFERENCES `user_post` (`Id`);

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
  ADD CONSTRAINT `user_post_ibfk_3` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`),
  ADD CONSTRAINT `user_post_ibfk_4` FOREIGN KEY (`ImageId`) REFERENCES `images` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

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
CREATE DEFINER=`root`@`localhost` EVENT `token_delete` ON SCHEDULE EVERY 1 HOUR STARTS '2024-03-17 19:33:00' ON COMPLETION NOT PRESERVE ENABLE DO DELETE FROM user_token
WHERE user_token.token_expire_date < NOW()$$

CREATE DEFINER=`root`@`localhost` EVENT `deleteExpiredTokens` ON SCHEDULE EVERY 1 MINUTE STARTS '2024-02-19 08:42:52' ON COMPLETION NOT PRESERVE DISABLE COMMENT 'Clears out sessions table each hour.' DO DELETE FROM user_token WHERE user_token.token_expire_date < NOW()$$

DELIMITER ;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
