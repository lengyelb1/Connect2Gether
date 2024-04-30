-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: localhost
-- Létrehozás ideje: 2024. Ápr 30. 16:39
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

CREATE TABLE `alertmessage` (
  `Id` int(11) NOT NULL,
  `Title` varchar(128) NOT NULL,
  `UserId` int(11) NOT NULL,
  `Description` varchar(128) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

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
(9, 'string', 18, 'string'),
(10, 'stringdsadsadsad', 18, 'stringdsadsadad'),
(14, 'Kommenteltek!', 19, 'Valaki kommentelt az egyik postod alá!'),
(15, 'asd', 18, 'asd'),
(16, 'asd', 18, 'asd'),
(17, 'Kommenteltek!', 19, 'Valaki kommentelt az egyik postod alá!');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `comment`
--

CREATE TABLE `comment` (
  `Id` int(11) NOT NULL,
  `Text` text NOT NULL,
  `PostId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `CommentId` int(11) NOT NULL,
  `uploadDate` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `comment`
--

INSERT INTO `comment` (`Id`, `Text`, `PostId`, `UserId`, `CommentId`, `uploadDate`) VALUES
(25, 'string', 31, 18, 0, '2024-04-29'),
(26, 'string', 31, 17, 0, '2024-04-29');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `deletedlikes`
--

CREATE TABLE `deletedlikes` (
  `id` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `postId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `disliked_posts`
--

CREATE TABLE `disliked_posts` (
  `id` int(11) NOT NULL,
  `userid` int(11) NOT NULL,
  `postid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `liked_posts`
--

CREATE TABLE `liked_posts` (
  `Id` int(11) NOT NULL,
  `UserID` int(11) NOT NULL,
  `PostID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Eseményindítók `liked_posts`
--
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

CREATE TABLE `permissions` (
  `Id` int(11) NOT NULL,
  `Name` varchar(128) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

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

CREATE TABLE `ranks` (
  `Id` int(11) NOT NULL,
  `Name` varchar(128) NOT NULL,
  `Min_pont` int(11) NOT NULL,
  `Max_pont` int(11) NOT NULL,
  `Description` text NOT NULL
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

CREATE TABLE `user` (
  `Id` int(11) NOT NULL,
  `Username` varchar(128) NOT NULL,
  `HASH` varchar(128) NOT NULL,
  `Email` varchar(320) NOT NULL,
  `ActiveUser` tinyint(1) NOT NULL,
  `RankId` int(11) NOT NULL COMMENT 'Pontszámhoz kötött rangok',
  `RegistrationDate` date NOT NULL,
  `Point` int(11) NOT NULL COMMENT 'Pontszám',
  `PermissionId` int(11) NOT NULL COMMENT 'Felhasználói szint',
  `LastLogin` datetime NOT NULL,
  `ProfileImage` mediumblob DEFAULT NULL,
  `ValidatedKey` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `user`
--

INSERT INTO `user` (`Id`, `Username`, `HASH`, `Email`, `ActiveUser`, `RankId`, `RegistrationDate`, `Point`, `PermissionId`, `LastLogin`, `ProfileImage`, `ValidatedKey`) VALUES
(16, 'bazsi', '$2a$04$Q0C3ffL38mWpwMz1okBehOJxe53Lr8r2dWjyKDJm71av6Ems.43iC', 'jb@gmail.com', 1, 1, '2024-03-18', 0, 3, '2024-04-30 16:16:31', '', ''),
(17, 'balint', '$2a$04$/quw9t7VqhWaO.tks5Q2L.q3JIFmehNUj5TAbZ6He/aOBoDTfNBeW', 'string', 1, 1, '2024-03-18', 0, 3, '2024-04-29 11:47:27', '', ''),
(18, 'balintUser', '$2a$04$echo9khsZ8hNJWAITgbcpuUQSxGHdP50U16x1IobDHB2kAi8h6C9O', 'string@gmail.com', 1, 1, '2024-03-18', 0, 1, '2024-04-18 11:22:01', '', ''),
(19, 'bazsiUser', '$2a$11$CjA9IpkMvxYs9hgjHr4Mk.O.TuW6CvXQXwAlMIBWBFVSfXK3DWqBq', 'juhaszbazsi13@gmail.com', 1, 1, '2024-03-18', 2, 1, '2024-04-29 12:48:58', '', ''),
(21, 'adamTest', '$2a$04$I5xhS9IIXDeLgWqUXKTxhuObH1PA7rW453bFqik75FYyIXGESU0he', 'adam@gmail@com', 1, 1, '2024-04-16', 0, 1, '2024-04-03 11:31:30', '', ''),
(27, 'viktor', '$2a$04$or979DyWbZAUeiC2T9lRW.r1XU.TRmBJd9BdIvyevvMq9MHK5z89u', 'tviktor20000717@gmail.com', 1, 1, '2024-04-11', 0, 2, '2024-04-15 08:13:43', '', ''),
(43, 'adam', '$2a$04$Iktj15rwuuwWECzDpfWvBOx2nYZBTFdgrmyarqN2b/FXG2Aj4stbm', 'kelemena@kkszki.hu', 1, 1, '2024-04-29', 0, 1, '2024-04-29 12:08:39', NULL, NULL);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user_post`
--

CREATE TABLE `user_post` (
  `Id` int(11) NOT NULL,
  `Image` mediumblob DEFAULT NULL,
  `Description` text NOT NULL,
  `Title` varchar(128) NOT NULL,
  `Like` bigint(20) NOT NULL DEFAULT 0,
  `Dislike` int(11) NOT NULL,
  `UserId` int(11) DEFAULT NULL,
  `uploadDate` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `user_post`
--

INSERT INTO `user_post` (`Id`, `Image`, `Description`, `Title`, `Like`, `Dislike`, `UserId`, `uploadDate`) VALUES
(31, NULL, 'Ez egy post!', 'Post címe', 0, 0, 19, '2024-03-25');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user_suspicious`
--

CREATE TABLE `user_suspicious` (
  `Id` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `Description` text NOT NULL,
  `Message` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user_token`
--

CREATE TABLE `user_token` (
  `Id` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `Token` text NOT NULL,
  `Token_expire_date` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

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
(16, 27, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoidmlrdG9yIiwicm9sZSI6Ik1vZGVyYXRvciIsImlkIjoiMjciLCJleHAiOjE3MTMyNDgwMjMsImlzcyI6ImF1dGgtYXBpIiwiYXVkIjoiYXV0aC1jbGllbnQifQ.mFoVsPRmgVoJqZQkuZMM4AuGA2T29kQTWGMFsBvqb9r9IAHSkjp7TJKVsVj5E02LfKO9YmtabYPTDvZmOTdznA', '2024-04-16 08:13:43'),
(18, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzEzMzM3MDAyLCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.9z_KlDOj12aMPjG-xmkdryVH7T4lvVeUwEFigS9t-Moi7gY2XX4ty_Zh8x3KKMo9jTPOkDAe3Tk49f7ZLeozdg', '2024-04-17 08:56:42'),
(19, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzEzMzQ4NDIwLCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.gcAL4O-KwFo09bFY-YhorkwLWnMVKxZZW4mu2gbsC4rIGci-1B1Z9prd_l8jEvgaiw0bK0XgxGt9rYcodXXO4g', '2024-04-17 12:07:00'),
(21, 18, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmFsaW50VXNlciIsInJvbGUiOiJEZWZhdWx0IiwiaWQiOiIxOCIsImV4cCI6MTcxMzUxODUyMSwiaXNzIjoiYXV0aC1hcGkiLCJhdWQiOiJhdXRoLWNsaWVudCJ9.JI4zXVs4MNNehtXl8jdAewF7pG-ZxCWEqio-9Fev0TJ3i5quiWK810xUA1ObIoNl4IHXct63f7rtNRilVSE56g', '2024-04-19 11:22:01'),
(22, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzEzNzk2MjQ0LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.sBrNQ2_vSHy4v8V6TJOKJR-6TYKdMUmS_at949AQH9EPVLjdwAKHFuhITydq6diL8uC63nA_UJvnPsHCLDmkRQ', '2024-04-22 16:30:45'),
(23, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzEzODcwMjk4LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.4EO8raNCj_9DQdwlmgTqN5wa4o-10gC4W-t7SZEAwhRvRhD3Wvc-RzjPpfC-2tNuyNsglXTVHeWcql5jongzyA', '2024-04-23 13:04:59'),
(24, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzEzOTQwMTE4LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.dhv0hMiXeTJO2zNDaBZ5nk2FCyC9FFX6XNFvWRyhcIf_GFeMhPGQg-4asXsVQCCBZiA3u78f6QjsiGVV8H0Fyw', '2024-04-24 08:28:38'),
(25, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzE0MDI3NTM0LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.o2uD4c7TpGV1s4yquYQJTRH9tKnfCmQV9Tm7dkz6t5xMFBsqoXT9cAx3HcaFaElowSqXJwfeFOkKRGhGWzhV9w', '2024-04-25 08:45:34'),
(26, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzE0MTE1NjEzLCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.OBQvwRAVL646cszZzdgmprYGMpwLN51wmF1x0Ctzu2jZL2MwKPptD7e_9F1S66CL1Hpdz9sWeMeA9Ac7YcJBeQ', '2024-04-26 09:13:33'),
(27, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzE0NDU4NTc3LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.dMH-zdCMByIsC3BX36PcAP75NOHPfz4Wm9zVlRFp16foQqOdkgVavr1Ce3FysilaZ6siZnctduWhFElAq6Q7sQ', '2024-04-30 08:29:37'),
(28, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzE0NDY1MDYwLCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.It_ttHnEz6k0S2l4v4UorxQaRx-OvFMTQ0LhTj2lZIpYo1tNGDhC7jjOc17DY8uzkss-Ebwcjazchk-st5I37A', '2024-04-30 10:17:40'),
(29, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzE0NDY4MjEyLCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.7IicRXPimbswa779Y-RBtqwyU8lnwrfGuXVPFyFXxqmlLWSxeJ34DtAGNJgWmFKCPRB9BhC9oa0--DhZ-Dws_Q', '2024-04-30 11:10:12'),
(30, 17, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmFsaW50Iiwicm9sZSI6IkFkbWluIiwiaWQiOiIxNyIsImV4cCI6MTcxNDQ3MDQ0NywiaXNzIjoiYXV0aC1hcGkiLCJhdWQiOiJhdXRoLWNsaWVudCJ9.nG1LqIEnjIdau-YQMojjGffVP1sP8GWzsz8Y8cgmk2OUC0ry2B_AtEnDyNS_-vevgie-3Ajf6bztqMhgkK-isw', '2024-04-30 11:47:27'),
(33, 19, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2lVc2VyIiwicm9sZSI6IkRlZmF1bHQiLCJpZCI6IjE5IiwiZXhwIjoxNzE0NDc0MTM4LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.LEqTPjOoBu7v8ahbC7gkWY5Bzwl_uuH5tXUk6ZPlAkZ3DWyvrYVM1c678sPmGtGSD23YnHJfGNJ0Bpmv-i8yow', '2024-04-30 12:48:58'),
(34, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzE0NDg4Mzg5LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.EEX9j1hO1KCG9kI3Zu7sNyPHbqCZZr5pNjnrCGy2BBREkfiYobx21Bs6bZiN_mXSNZNUqFCdKMX0PEX_HbotNQ', '2024-04-30 16:46:30'),
(35, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzE0NTY1ODYyLCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0._fsiKKCBWplfxJwlb7KEyeKiO6A97o-T6FYJN2Hym4rNNUoOYAVraVm6Y-nkNq6fbX4ku0QGLlB4031lmLypjQ', '2024-05-01 14:17:42'),
(36, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzE0NTY1ODYyLCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0._fsiKKCBWplfxJwlb7KEyeKiO6A97o-T6FYJN2Hym4rNNUoOYAVraVm6Y-nkNq6fbX4ku0QGLlB4031lmLypjQ', '2024-05-01 14:17:42'),
(37, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzE0NTY1OTY0LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.NGXd0Py65gNrPJyXJAVr48Fno9NKS2AGhbDaku8rnsgWinqNA4RekFvEsdiH9MjPsKSXeqJEK4fUv3t4XQZXSw', '2024-05-01 14:19:24'),
(38, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzE0NTY1OTY0LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.NGXd0Py65gNrPJyXJAVr48Fno9NKS2AGhbDaku8rnsgWinqNA4RekFvEsdiH9MjPsKSXeqJEK4fUv3t4XQZXSw', '2024-05-01 14:19:24'),
(39, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzE0NTY2OTg4LCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.qKW_zoT7Y7Hkgc4NY3NyOkdTdi_eHYXQEFfTIW8tOYTJBiXmfYQ-ZkqfLWIS0ckcmF3dbJtraJdvu9_1c5Ic2w', '2024-05-01 14:36:28'),
(40, 16, 'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJOYW1lIjoiYmF6c2kiLCJyb2xlIjoiQWRtaW4iLCJpZCI6IjE2IiwiZXhwIjoxNzE0NTcyOTkyLCJpc3MiOiJhdXRoLWFwaSIsImF1ZCI6ImF1dGgtY2xpZW50In0.Tb4IM7o9p7Mfd42bhbgap9smXXOtF_JdURqlOZ2UiMMIsU0j5lPko1F7q_kW6JvtTQ2gdBmLcdaQVsTqs1ncqA', '2024-05-01 16:16:32');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `alertmessage`
--
ALTER TABLE `alertmessage`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `UserId` (`UserId`);

--
-- A tábla indexei `comment`
--
ALTER TABLE `comment`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `CommentId` (`CommentId`),
  ADD KEY `UserId` (`UserId`),
  ADD KEY `PostId` (`PostId`);

--
-- A tábla indexei `deletedlikes`
--
ALTER TABLE `deletedlikes`
  ADD PRIMARY KEY (`id`),
  ADD KEY `UserId` (`UserId`) USING BTREE,
  ADD KEY `postId` (`postId`) USING BTREE;

--
-- A tábla indexei `disliked_posts`
--
ALTER TABLE `disliked_posts`
  ADD PRIMARY KEY (`id`),
  ADD KEY `userid` (`userid`,`postid`),
  ADD KEY `postid` (`postid`);

--
-- A tábla indexei `liked_posts`
--
ALTER TABLE `liked_posts`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `UserID` (`UserID`),
  ADD KEY `PostID` (`PostID`);

--
-- A tábla indexei `permissions`
--
ALTER TABLE `permissions`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `ranks`
--
ALTER TABLE `ranks`
  ADD PRIMARY KEY (`Id`);

--
-- A tábla indexei `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Username` (`Username`),
  ADD KEY `RankId` (`RankId`),
  ADD KEY `PermissionId` (`PermissionId`);

--
-- A tábla indexei `user_post`
--
ALTER TABLE `user_post`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `UserId` (`UserId`);

--
-- A tábla indexei `user_suspicious`
--
ALTER TABLE `user_suspicious`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `UserId` (`UserId`);

--
-- A tábla indexei `user_token`
--
ALTER TABLE `user_token`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `UserId` (`UserId`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `alertmessage`
--
ALTER TABLE `alertmessage`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT a táblához `comment`
--
ALTER TABLE `comment`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;

--
-- AUTO_INCREMENT a táblához `deletedlikes`
--
ALTER TABLE `deletedlikes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT a táblához `disliked_posts`
--
ALTER TABLE `disliked_posts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT a táblához `liked_posts`
--
ALTER TABLE `liked_posts`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=37;

--
-- AUTO_INCREMENT a táblához `permissions`
--
ALTER TABLE `permissions`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `user`
--
ALTER TABLE `user`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=45;

--
-- AUTO_INCREMENT a táblához `user_post`
--
ALTER TABLE `user_post`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=54;

--
-- AUTO_INCREMENT a táblához `user_suspicious`
--
ALTER TABLE `user_suspicious`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT a táblához `user_token`
--
ALTER TABLE `user_token`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=41;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `alertmessage`
--
ALTER TABLE `alertmessage`
  ADD CONSTRAINT `alertmessage_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `comment`
--
ALTER TABLE `comment`
  ADD CONSTRAINT `comment_ibfk_4` FOREIGN KEY (`PostId`) REFERENCES `user_post` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `comment_ibfk_5` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `deletedlikes`
--
ALTER TABLE `deletedlikes`
  ADD CONSTRAINT `deletedlikes_ibfk_3` FOREIGN KEY (`postId`) REFERENCES `user_post` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `deletedlikes_ibfk_4` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `disliked_posts`
--
ALTER TABLE `disliked_posts`
  ADD CONSTRAINT `disliked_posts_ibfk_2` FOREIGN KEY (`postid`) REFERENCES `user_post` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `disliked_posts_ibfk_3` FOREIGN KEY (`userid`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `liked_posts`
--
ALTER TABLE `liked_posts`
  ADD CONSTRAINT `liked_posts_ibfk_4` FOREIGN KEY (`PostID`) REFERENCES `user_post` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `liked_posts_ibfk_5` FOREIGN KEY (`UserID`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

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
  ADD CONSTRAINT `user_post_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `user` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

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
