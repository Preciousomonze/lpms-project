-- phpMyAdmin SQL Dump
-- version 4.6.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jul 05, 2018 at 12:00 AM
-- Server version: 5.7.14
-- PHP Version: 5.6.25

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `patho`
--
CREATE DATABASE IF NOT EXISTS `patho` DEFAULT CHARACTER SET utf8 COLLATE utf8_bin;
USE `patho`;

-- --------------------------------------------------------

--
-- Table structure for table `doctors`
--

CREATE TABLE `doctors` (
  `doctor_id` int(11) NOT NULL,
  `name` varchar(100) COLLATE utf8_bin NOT NULL,
  `address` varchar(150) COLLATE utf8_bin NOT NULL,
  `phone` varchar(100) COLLATE utf8_bin NOT NULL,
  `gender` tinyint(1) NOT NULL,
  `specialisation` varchar(150) COLLATE utf8_bin NOT NULL,
  `date` datetime NOT NULL,
  `_added_by` int(11) NOT NULL,
  `_editors` mediumtext COLLATE utf8_bin NOT NULL,
  `deleted` tinyint(1) DEFAULT '0'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `doctors`
--

INSERT INTO `doctors` (`doctor_id`, `name`, `address`, `phone`, `gender`, `specialisation`, `date`, `_added_by`, `_editors`, `deleted`) VALUES
(1, 'Precious Omonzejele', '2222, Low beans', '1234567', 1, 'IT', '2017-04-12 09:00:00', 1, '', 0),
(2, 'Joke Haastrup', '2,unilag road', '878688888', 0, 'Imbecilism', '2017-04-12 21:02:17', 3, '', 0),
(3, 'Soffiyya Morenikeji', '4, UNILAG ROAD', '812234322', 0, 'Ganicologist', '2017-04-26 21:22:12', 1, '', 0);

-- --------------------------------------------------------

--
-- Table structure for table `items`
--

CREATE TABLE `items` (
  `item_id` int(11) NOT NULL,
  `item_name` varchar(150) COLLATE utf8_bin NOT NULL,
  `item_cost` int(11) NOT NULL,
  `quantity_left` int(11) NOT NULL,
  `quantity_consumed` int(11) NOT NULL DEFAULT '0',
  `date_time` datetime NOT NULL,
  `date_time_updated` datetime DEFAULT NULL,
  `_added_by` int(11) NOT NULL,
  `_editors` mediumtext COLLATE utf8_bin,
  `deleted` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `items`
--

INSERT INTO `items` (`item_id`, `item_name`, `item_cost`, `quantity_left`, `quantity_consumed`, `date_time`, `date_time_updated`, `_added_by`, `_editors`, `deleted`) VALUES
(1, 'Paracetamol', 200, 0, 15, '2017-04-06 15:09:49', '2017-04-28 01:32:10', 2, NULL, 0),
(2, 'Codine', 1000, 4, 2, '2017-04-06 16:41:29', '2017-04-11 12:28:56', 2, NULL, 0),
(3, 'Vitamin C', 50, 110, 10, '2017-04-12 14:13:26', '2017-04-14 18:14:25', 3, NULL, 1),
(4, 'Coflin', 300, 30, 125, '2017-04-13 14:53:00', '2017-04-28 01:32:10', 1, NULL, 0),
(5, 'stuff', 233, 5, 0, '2017-04-27 11:46:15', '2017-04-27 11:46:19', 2, NULL, 1),
(6, 'sample', 200, 5, 0, '2017-05-05 07:47:37', NULL, 1, NULL, 1);

-- --------------------------------------------------------

--
-- Table structure for table `patients`
--

CREATE TABLE `patients` (
  `patient_id` int(11) NOT NULL,
  `name` varchar(150) COLLATE utf8_bin NOT NULL,
  `address` varchar(150) COLLATE utf8_bin NOT NULL,
  `phone` varchar(100) COLLATE utf8_bin NOT NULL,
  `gender` tinyint(1) NOT NULL,
  `dob` date NOT NULL,
  `date` datetime NOT NULL,
  `_added_by` int(11) NOT NULL,
  `_editors` mediumtext COLLATE utf8_bin,
  `deleted` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `patients`
--

INSERT INTO `patients` (`patient_id`, `name`, `address`, `phone`, `gender`, `dob`, `date`, `_added_by`, `_editors`, `deleted`) VALUES
(1, 'Hidayyah Toyin', '45, Oduduwa Street, Surulere', '812345445', 0, '1999-04-10', '2017-03-16 00:00:00', 1, NULL, 0),
(2, 'pp', 'aa', '2222', 0, '2017-03-01', '2017-03-23 00:00:00', 3, NULL, 1),
(3, 'Precious Omonzejele', '34, Apaja Lane', '9021610260', 1, '1998-02-15', '2017-03-16 00:00:00', 2, NULL, 0),
(4, 'Joke Hasstrup', 'aa', '2222', 2, '2017-03-01', '2017-03-23 00:00:00', 2, NULL, 1),
(5, 'Adjharo Afoke', '45, ty lane', '222', 1, '2017-03-01', '2017-03-16 00:00:00', 2, NULL, 1),
(6, 'Jerry Vaaka', '222, kaaka street', '8122332332', 1, '1993-01-03', '2017-03-23 00:00:00', 3, NULL, 0),
(7, 'Akpan Emmanuel', 'Bariga close', '222', 1, '2017-03-01', '2017-03-16 00:00:00', 1, NULL, 0),
(8, 'Tawa Adekemi', '42, odudu street', '8033234232', 0, '1998-02-11', '2017-03-23 00:00:00', 1, NULL, 0),
(9, 'Soffiyya Shoroye', '4, UNILAG Road', '8182114667', 0, '1999-06-07', '2017-04-06 04:08:27', 1, NULL, 0),
(10, 'Oyedele Abdulrahman', 'Ojuelegba, Surulere', '9059624609', 1, '1992-06-27', '2017-04-06 17:16:15', 3, NULL, 0),
(11, 'Peter Afo', '332, Ikorodu road', '8182002221', 1, '1992-08-08', '2017-04-13 14:39:06', 3, NULL, 1),
(12, 'sss ff', '34,dd', '9021610260', 1, '2345-02-12', '2017-04-28 11:22:35', 3, NULL, 1),
(13, 'ds asd', 'sss', '+333333333333234', 0, '2232-12-30', '2017-05-05 14:21:05', 1, '1,2017-05-05 03:30:45|1,2017-05-05 03:30:54|1,2017-05-05 03:31:10', 1),
(14, 'pp', 'qwe223', '+233333333333333333333333', 1, '1999-10-23', '2017-05-08 14:04:12', 1, '1,2017-05-08 02:04:53', 1);

-- --------------------------------------------------------

--
-- Table structure for table `purchases`
--

CREATE TABLE `purchases` (
  `purchase_id` int(11) NOT NULL,
  `item_id` int(11) NOT NULL,
  `quantity` int(11) NOT NULL,
  `patient_id` int(11) NOT NULL,
  `date_time` datetime NOT NULL,
  `_added_by` int(11) NOT NULL,
  `_editors` mediumtext COLLATE utf8_bin,
  `deleted` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `purchases`
--

INSERT INTO `purchases` (`purchase_id`, `item_id`, `quantity`, `patient_id`, `date_time`, `_added_by`, `_editors`, `deleted`) VALUES
(234, 1, 7, 10, '2017-04-11 12:21:00', 1, NULL, 0),
(235, 2, 2, 6, '2017-04-11 12:28:56', 2, NULL, 0),
(236, 1, 7, 1, '2017-04-12 14:12:50', 3, NULL, 0),
(237, 4, 40, 7, '2017-04-13 14:57:09', 2, NULL, 0),
(238, 3, 5, 3, '2017-04-13 14:58:35', 2, NULL, 0),
(239, 4, 3, 9, '2017-04-14 12:10:15', 1, NULL, 0),
(240, 1, 5, 3, '2017-04-27 11:47:51', 2, NULL, 1),
(241, 4, 34, 3, '2017-04-27 12:09:45', 1, NULL, 0);

-- --------------------------------------------------------

--
-- Table structure for table `reports`
--

CREATE TABLE `reports` (
  `report_id` int(11) NOT NULL,
  `report_name` tinytext COLLATE utf8_bin NOT NULL,
  `report` mediumtext COLLATE utf8_bin NOT NULL,
  `test_id` int(11) NOT NULL,
  `date_time` datetime NOT NULL,
  `date_time_updated` datetime DEFAULT NULL,
  `_added_by` int(11) NOT NULL,
  `_editors` mediumtext COLLATE utf8_bin,
  `deleted` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `reports`
--

INSERT INTO `reports` (`report_id`, `report_name`, `report`, `test_id`, `date_time`, `date_time_updated`, `_added_by`, `_editors`, `deleted`) VALUES
(1, 'Sorter', 'Was a really cool one, but some things that happened were really nice and we thank God for the success of this project.\r\n\'NOW()', 7, '2017-04-16 16:12:36', NULL, 2, NULL, 0),
(2, 'Sample', 'nothing really            a , imbe', 17, '2017-04-16 18:08:51', '2017-04-24 18:16:20', 2, NULL, 0),
(3, 'sampl2', 'blabla black cheep,', 16, '2017-04-20 13:23:06', '2017-04-25 14:57:03', 3, NULL, 1),
(4, 'nkdfndf', 'aaaaa', 17, '2017-04-20 13:23:20', NULL, 4, NULL, 0),
(5, 'Surgery report', 'Was a really cool one, but some things that happened were really nice and we thank God for the success of this project.  Was a really cool one, but some things that happened were really nice and we thank God for the success of this project...=', 20, '2017-04-20 21:57:23', '2017-05-05 14:19:05', 1, '1,2017-05-05 12:00:00|1,2017-05-05 02:19:05', 0),
(6, 'PRecious testing', 'O dont know O dont know O dont know O dont know O dont know O dont know O dont know O dont know O dont know O dont know O dont know O dont know O dont know O dont know O dont know O dont knowv ghghh', 7, '2017-04-25 14:57:39', '2017-04-28 12:24:47', 1, NULL, 0),
(7, 'blassss', 'sssssssssssss', 2, '2017-04-25 20:23:12', NULL, 1, NULL, 1),
(8, 'Advanced', 'I did this, I cant beleive, so nice, i should push this to github, although its poor at handling sql injection, but thats on the db query side, nice. i deserve some accolades, besides, the design still looks so lovely, nice,', 3, '2018-06-30 00:21:10', '2018-06-30 00:21:56', 31, '31,2018-06-30 12:21:56', 0);

-- --------------------------------------------------------

--
-- Table structure for table `requests`
--

CREATE TABLE `requests` (
  `request_id` int(11) NOT NULL,
  `_for` int(11) NOT NULL,
  `_by` int(11) NOT NULL,
  `_to` int(11) NOT NULL,
  `allowed` int(11) NOT NULL DEFAULT '0' COMMENT 'the number of times the person is allowed to edit the record',
  `note` tinytext COLLATE utf8_bin NOT NULL COMMENT 'the note the requester sends',
  `type` int(11) NOT NULL,
  `accepted` tinyint(1) DEFAULT NULL,
  `date_time` datetime NOT NULL,
  `date_time_updated` datetime DEFAULT NULL,
  `deleted` tinyint(1) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- Table structure for table `staffs`
--

CREATE TABLE `staffs` (
  `staff_id` int(11) NOT NULL,
  `username` varchar(100) COLLATE utf8_bin NOT NULL,
  `password` varchar(100) COLLATE utf8_bin NOT NULL,
  `name` varchar(100) COLLATE utf8_bin NOT NULL,
  `department` varchar(100) COLLATE utf8_bin DEFAULT NULL,
  `phone` varchar(100) COLLATE utf8_bin DEFAULT NULL,
  `gender` tinyint(1) NOT NULL,
  `dob` date DEFAULT NULL,
  `qualification` varchar(150) COLLATE utf8_bin DEFAULT NULL,
  `tech_skills` varchar(150) COLLATE utf8_bin DEFAULT NULL,
  `date_time` datetime NOT NULL,
  `deleted` tinyint(1) DEFAULT '0'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `staffs`
--

INSERT INTO `staffs` (`staff_id`, `username`, `password`, `name`, `department`, `phone`, `gender`, `dob`, `qualification`, `tech_skills`, `date_time`, `deleted`) VALUES
(1, 'pp', '123', 'Precious Omonzejele', 'Psychology', '+2349021610260', 1, '1998-03-16', 'Specialist', '', '2017-03-17 00:00:00', 0),
(4, 'sam', 'ww', 'aa', NULL, NULL, 1, NULL, NULL, NULL, '2017-04-26 00:17:00', 0),
(23, 'hjj', '6yui', 'yuii', 'hjj', NULL, 1, NULL, NULL, NULL, '2017-04-25 12:50:14', 0),
(2, 'wunmi', '12345', 'afoke Wunmi', 'IT in Aptech', '', 0, '1990-02-12', '', NULL, '2017-04-25 13:01:07', 0),
(27, 'ppp', '123', 'sample', '3', NULL, 1, NULL, NULL, NULL, '2017-04-28 11:37:18', 0),
(28, 'asd', '123', 'Precious Omonzejele', 'Dental', NULL, 1, NULL, NULL, NULL, '2017-04-28 12:13:14', 0),
(3, 'jokefam', '123', 'Joke Haastrup', 'Dental', NULL, 1, NULL, NULL, NULL, '2017-04-28 12:15:34', 0),
(30, 'precious omonze', '123', 'chucks chucker', 'IT', NULL, 0, NULL, NULL, NULL, '2017-05-08 15:13:59', 0),
(31, 'preciousomonze', '12345', 'Precious Omonze', 'asd', NULL, 1, NULL, NULL, NULL, '2018-06-30 00:16:23', 0);

-- --------------------------------------------------------

--
-- Table structure for table `tests`
--

CREATE TABLE `tests` (
  `test_id` int(11) NOT NULL,
  `test_name` varchar(150) COLLATE utf8_bin NOT NULL,
  `amount` bigint(20) NOT NULL,
  `patient_id` int(11) NOT NULL,
  `doctor_id` int(11) NOT NULL,
  `date` datetime NOT NULL,
  `date_time_updated` datetime DEFAULT NULL,
  `_added_by` int(11) NOT NULL,
  `_editors` mediumtext COLLATE utf8_bin,
  `deleted` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `tests`
--

INSERT INTO `tests` (`test_id`, `test_name`, `amount`, `patient_id`, `doctor_id`, `date`, `date_time_updated`, `_added_by`, `_editors`, `deleted`) VALUES
(18, 'srt', 567, 4, 1, '2017-04-10 22:00:38', NULL, 1, NULL, 1),
(2, 'Computer test', 20000, 6, 3, '2017-04-01 12:46:46', '2017-04-28 12:22:23', 2, NULL, 0),
(3, 'Cons RCT', 56000, 8, 1, '2017-04-01 12:55:15', NULL, 3, NULL, 0),
(4, 'Cons RCT', 56000, 8, 1, '2017-04-01 12:55:40', NULL, 1, NULL, 1),
(7, 'Surgery', 10000, 3, 1, '2017-04-01 13:09:55', NULL, 1, NULL, 0),
(17, 'precious', 90, 8, 1, '2017-04-04 16:53:39', NULL, 1, NULL, 1),
(16, 'opera', 89000, 3, 1, '2017-04-03 20:14:06', NULL, 1, NULL, 0),
(19, '4565ytb', 456, 1, 1, '2017-04-11 07:17:05', NULL, 2, NULL, 1),
(20, 'test itself', 20000, 2, 2, '2017-04-12 21:03:23', NULL, 2, NULL, 0),
(21, 'Eye test', 20000, 1, 1, '2017-04-13 14:45:04', NULL, 2, NULL, 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `doctors`
--
ALTER TABLE `doctors`
  ADD PRIMARY KEY (`doctor_id`);

--
-- Indexes for table `items`
--
ALTER TABLE `items`
  ADD PRIMARY KEY (`item_id`);

--
-- Indexes for table `patients`
--
ALTER TABLE `patients`
  ADD PRIMARY KEY (`patient_id`);

--
-- Indexes for table `purchases`
--
ALTER TABLE `purchases`
  ADD PRIMARY KEY (`purchase_id`);

--
-- Indexes for table `reports`
--
ALTER TABLE `reports`
  ADD PRIMARY KEY (`report_id`);

--
-- Indexes for table `requests`
--
ALTER TABLE `requests`
  ADD PRIMARY KEY (`request_id`);

--
-- Indexes for table `staffs`
--
ALTER TABLE `staffs`
  ADD PRIMARY KEY (`staff_id`);

--
-- Indexes for table `tests`
--
ALTER TABLE `tests`
  ADD PRIMARY KEY (`test_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `doctors`
--
ALTER TABLE `doctors`
  MODIFY `doctor_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT for table `items`
--
ALTER TABLE `items`
  MODIFY `item_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;
--
-- AUTO_INCREMENT for table `patients`
--
ALTER TABLE `patients`
  MODIFY `patient_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;
--
-- AUTO_INCREMENT for table `purchases`
--
ALTER TABLE `purchases`
  MODIFY `purchase_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=242;
--
-- AUTO_INCREMENT for table `reports`
--
ALTER TABLE `reports`
  MODIFY `report_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;
--
-- AUTO_INCREMENT for table `requests`
--
ALTER TABLE `requests`
  MODIFY `request_id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `staffs`
--
ALTER TABLE `staffs`
  MODIFY `staff_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;
--
-- AUTO_INCREMENT for table `tests`
--
ALTER TABLE `tests`
  MODIFY `test_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
