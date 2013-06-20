-- --------------------------------------------------------
-- Host:                         localhost
-- Server version:               5.5.24-log - MySQL Community Server (GPL)
-- Server OS:                    Win64
-- HeidiSQL version:             7.0.0.4053
-- Date/time:                    2013-06-12 16:09:54
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET FOREIGN_KEY_CHECKS=0 */;

-- Dumping database structure for hcs-db2
CREATE DATABASE IF NOT EXISTS `hcs-db2` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `hcs-db2`;


-- Dumping structure for table hcs-db2.departments
CREATE TABLE IF NOT EXISTS `departments` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `parentID` int(10) NOT NULL,
  `description` varchar(50) NOT NULL,
  `headID` int(10) NOT NULL DEFAULT '2',
  PRIMARY KEY (`id`),
  UNIQUE KEY `deptID` (`id`),
  KEY `Index 1` (`id`),
  KEY `parentID` (`parentID`),
  KEY `headID` (`headID`),
  CONSTRAINT `FK_departmets_users` FOREIGN KEY (`headID`) REFERENCES `users` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=latin1 COMMENT='various departments and sub-departments across HCS';

-- Dumping data for table hcs-db2.departments: ~15 rows (approximately)
DELETE FROM `departments`;
/*!40000 ALTER TABLE `departments` DISABLE KEYS */;
INSERT INTO `departments` (`id`, `parentID`, `description`, `headID`) VALUES
	(1, 1, 'Printing', 1),
	(2, 2, 'Mid', 1),
	(3, 2, 'Magnetic', 1),
	(4, 2, 'Lamination', 1),
	(5, 2, 'Die Cut', 1),
	(6, 6, 'Personalization', 1),
	(7, 7, 'Quality Assurance', 1),
	(8, 8, 'Assembly', 1),
	(9, 8, 'Multi-Pack', 1),
	(10, 8, 'Heat Seal', 1),
	(11, 11, 'Packing', 1),
	(12, 11, 'Tipping', 1),
	(13, 11, 'Shrink Wrap Packing', 1),
	(14, 14, 'Sales', 1),
	(15, 15, 'Graphics Designing', 1);
/*!40000 ALTER TABLE `departments` ENABLE KEYS */;


-- Dumping structure for table hcs-db2.dept_users
CREATE TABLE IF NOT EXISTS `dept_users` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `deptID` int(10) NOT NULL DEFAULT '1',
  `deptName` varchar(50) NOT NULL DEFAULT '-no name-',
  `userID` int(10) NOT NULL DEFAULT '2',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `Index 1` (`id`),
  KEY `FK_user_groups_users` (`userID`),
  KEY `groupID` (`deptID`),
  CONSTRAINT `FK_user_groups_users` FOREIGN KEY (`userID`) REFERENCES `users` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=latin1 COMMENT='Users assigned for each department';

-- Dumping data for table hcs-db2.dept_users: ~23 rows (approximately)
DELETE FROM `dept_users`;
/*!40000 ALTER TABLE `dept_users` DISABLE KEYS */;
INSERT INTO `dept_users` (`id`, `deptID`, `deptName`, `userID`) VALUES
	(1, 1, 'Printing', 2),
	(2, 1, 'Printing', 1),
	(3, 1, 'Printing', 3),
	(4, 1, 'Printing', 4),
	(5, 2, 'Mid', 2),
	(6, 2, 'Mid', 1),
	(7, 2, 'Mid', 3),
	(8, 2, 'Mid', 4),
	(9, 6, 'Personalization', 6),
	(10, 6, 'Personalization', 7),
	(11, 2, 'Mid', 8),
	(12, 7, 'Quality Assurance', 6),
	(13, 7, 'Quality Assurance', 7),
	(14, 8, 'Assembly', 1),
	(15, 8, 'Assembly', 2),
	(16, 8, 'Assembly', 3),
	(17, 8, 'Assembly', 4),
	(18, 11, 'Packing', 1),
	(19, 11, 'Packing', 2),
	(20, 11, 'Packing', 3),
	(21, 11, 'Packing', 4),
	(22, 14, 'Sales', 10),
	(23, 15, 'Graphics Designing', 5);
/*!40000 ALTER TABLE `dept_users` ENABLE KEYS */;


-- Dumping structure for table hcs-db2.graphics_vars_gang
CREATE TABLE IF NOT EXISTS `graphics_vars_gang` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `user_id` int(10) NOT NULL DEFAULT '2',
  `sales_vars_id` int(10) NOT NULL DEFAULT '1',
  `gang_number` varchar(50) NOT NULL DEFAULT 'Initial',
  `require_qty` int(50) NOT NULL DEFAULT '1',
  `print_qty` int(50) NOT NULL DEFAULT '1',
  `issue_qty` int(50) NOT NULL DEFAULT '1',
  `up_size` int(10) NOT NULL DEFAULT '1',
  `sheet_size` varchar(50) NOT NULL DEFAULT '-no val-',
  `PrintOnFaceFront` varchar(50) NOT NULL DEFAULT '-no val-',
  `PrintOnFaceBack` varchar(50) NOT NULL DEFAULT '-no val-',
  `NumberOfColorFront` varchar(50) NOT NULL DEFAULT '-no val-',
  `NumberOfColorBack` varchar(50) NOT NULL DEFAULT '-no val-',
  `SideGuideFront` varchar(50) NOT NULL DEFAULT '-no val-',
  `SideGuideBack` varchar(50) NOT NULL DEFAULT '-no val-',
  `UVCoatingFront` varchar(50) NOT NULL DEFAULT '-no val-',
  `UVCoatingBack` varchar(50) NOT NULL DEFAULT 'NO',
  `Printing` enum('Y','N') NOT NULL DEFAULT 'Y',
  `PrintingType` varchar(50) NOT NULL DEFAULT '-no val-',
  `MagneticTape` enum('Y','N') NOT NULL DEFAULT 'Y',
  `MagneticTapeType` varchar(50) NOT NULL DEFAULT '-no val-',
  `ColLay` enum('Y','N') NOT NULL DEFAULT 'Y',
  `ColLayType` varchar(50) NOT NULL DEFAULT '-no val-',
  `HydraulicPress` enum('Y','N') NOT NULL DEFAULT 'Y',
  `HydraulicPressType` varchar(50) NOT NULL DEFAULT '-no val-',
  `SheetCut` enum('Y','N') NOT NULL DEFAULT 'Y',
  `SheetCutType` varchar(50) NOT NULL DEFAULT '-no val-',
  `DieCut` enum('Y','N') NOT NULL DEFAULT 'Y',
  `DieCutType` varchar(50) NOT NULL DEFAULT '-no val-',
  `Personalization` enum('Y','N') NOT NULL DEFAULT 'Y',
  `PersonalizationType` varchar(50) NOT NULL DEFAULT '-no val-',
  PRIMARY KEY (`id`),
  UNIQUE KEY `Index 2` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1;

-- Dumping data for table hcs-db2.graphics_vars_gang: ~4 rows (approximately)
DELETE FROM `graphics_vars_gang`;
/*!40000 ALTER TABLE `graphics_vars_gang` DISABLE KEYS */;
INSERT INTO `graphics_vars_gang` (`id`, `user_id`, `sales_vars_id`, `gang_number`, `require_qty`, `print_qty`, `issue_qty`, `up_size`, `sheet_size`, `PrintOnFaceFront`, `PrintOnFaceBack`, `NumberOfColorFront`, `NumberOfColorBack`, `SideGuideFront`, `SideGuideBack`, `UVCoatingFront`, `UVCoatingBack`, `Printing`, `PrintingType`, `MagneticTape`, `MagneticTapeType`, `ColLay`, `ColLayType`, `HydraulicPress`, `HydraulicPressType`, `SheetCut`, `SheetCutType`, `DieCut`, `DieCutType`, `Personalization`, `PersonalizationType`) VALUES
	(1, 2, 10001, '6459', 1, 170, 180, 4, '28x40', 'matte', 'matte', '-no val-', '-no val-', 'operator', 'operator', '-no val-', 'no', 'Y', 'Komori 4', 'Y', 'MTL-700 #1', 'N', '-no val-', 'N', '-no val-', 'N', '-no val-', 'Y', 'Car 25 #2', 'N', '-no val-'),
	(2, 5, 3459, '7000', 1, 15, 20, 60, '28x40', 'matte', 'matte', '-no val-', '-no val-', 'operator', 'operator', '-no val-', 'no', 'Y', 'Komori 4', 'Y', 'MTL-700 #1', 'N', '-no val-', 'Y', '-no val-', 'N', '-no val-', 'Y', 'Car 25 #4', 'N', '-no val-'),
	(13, 5, 1002, '7000', 1, 15, 20, 60, '28x40', 'matte', 'matte', '-no val-', '-no val-', 'operator', 'operator', '-no val-', 'no', 'Y', '-no val-', 'N', '-no val-', 'Y', '-no val-', 'Y', '-no val-', 'Y', '-no val-', 'N', '-no val-', 'Y', '-no val-'),
	(14, 5, 1003, '7001', 1, 15, 20, 10, '28x40', 'matte', 'matte', '-no val-', '-no val-', 'operator', 'operator', '-no val-', 'no', 'Y', '-no val-', 'Y', '-no val-', 'Y', '-no val-', 'Y', '-no val-', 'Y', '-no val-', 'Y', '-no val-', 'Y', '-no val-');
/*!40000 ALTER TABLE `graphics_vars_gang` ENABLE KEYS */;


-- Dumping structure for table hcs-db2.graphics_vars_job
CREATE TABLE IF NOT EXISTS `graphics_vars_job` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `sales_vars_id` int(10) NOT NULL DEFAULT '1',
  `graphics_vars_gang_id` int(10) NOT NULL DEFAULT '1',
  `user_id` int(10) NOT NULL DEFAULT '2',
  `job_number` varchar(50) NOT NULL DEFAULT '-no val-',
  `Retailer` varchar(50) NOT NULL DEFAULT '-no val-',
  `PinCover` varchar(50) NOT NULL DEFAULT '-no val-',
  `CardRatio` varchar(50) NOT NULL DEFAULT '-no val-',
  `Other_Barcode` varchar(50) NOT NULL DEFAULT '-no val-',
  `MagStripe` enum('Y','N') NOT NULL DEFAULT 'Y',
  `Encode` varchar(50) NOT NULL DEFAULT '-no val-',
  `ProjectedDueDate` date NOT NULL,
  `ActualDueDate` date NOT NULL,
  `Personalization_job` enum('Y','N') NOT NULL DEFAULT 'Y',
  `MinPrintQty` int(10) NOT NULL DEFAULT '1',
  `up_size` int(10) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  UNIQUE KEY `Index 2` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- Dumping data for table hcs-db2.graphics_vars_job: ~8 rows (approximately)
DELETE FROM `graphics_vars_job`;
/*!40000 ALTER TABLE `graphics_vars_job` DISABLE KEYS */;
INSERT INTO `graphics_vars_job` (`id`, `sales_vars_id`, `graphics_vars_gang_id`, `user_id`, `job_number`, `Retailer`, `PinCover`, `CardRatio`, `Other_Barcode`, `MagStripe`, `Encode`, `ProjectedDueDate`, `ActualDueDate`, `Personalization_job`, `MinPrintQty`, `up_size`) VALUES
	(1, 1, 6459, 5, '123', 'r123', 'pin123', 'c123', 'o123', 'N', 'e123', '2013-06-30', '2013-06-30', 'Y', 0, 1),
	(3, 4, 6459, 5, '789', 'r789', 'pin789', 'c789', 'o789', 'N', 'e789', '2013-07-23', '2013-07-23', 'N', 789, 1),
	(6, 1, 7000, 5, '1879', 'r456', 'pin234', 'cwtrg', 'sdfsdf', 'N', 'ghgh', '2013-06-30', '2013-06-30', 'Y', 34, 11),
	(7, 6, 7000, 5, '1000', 'r123', 'pin123', 'c123', 'o123', 'Y', 'e123', '0000-00-00', '0000-00-00', 'Y', 0, 20),
	(8, 7, 6459, 5, '1001', 'r456', 'pin456', 'c456', 'o456', 'N', 'e456', '0000-00-00', '0000-00-00', 'Y', 0, 20),
	(15, 2, 7000, 5, 'j678', 'r678', 'p678', 'c678', 'o678', 'N', 'e678', '0000-00-00', '0000-00-00', 'Y', 0, 10),
	(16, 8, 7001, 5, 'j123', 'r123', 'p123', 'c123', 'o123', 'N', 'e123', '0000-00-00', '0000-00-00', 'Y', 12, 1),
	(17, 9, 7000, 5, 'j456', 'r456', 'p456', 'c456', 'o456', 'Y', 'e456', '0000-00-00', '0000-00-00', 'N', 6, 10);
/*!40000 ALTER TABLE `graphics_vars_job` ENABLE KEYS */;


-- Dumping structure for table hcs-db2.machines
CREATE TABLE IF NOT EXISTS `machines` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `machine_name` varchar(50) NOT NULL DEFAULT '-no name-',
  `quantity` int(5) NOT NULL DEFAULT '0',
  `UM` int(10) NOT NULL DEFAULT '1' COMMENT 'Unit of Measurement',
  `capacity` int(10) NOT NULL DEFAULT '0' COMMENT 'Average capacity, not considered variables',
  `deptID` int(10) NOT NULL DEFAULT '2',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `Index 1` (`id`),
  KEY `UM` (`UM`),
  KEY `deptID` (`deptID`),
  CONSTRAINT `FK_machines_departments` FOREIGN KEY (`deptID`) REFERENCES `departments` (`id`),
  CONSTRAINT `FK_machines_um` FOREIGN KEY (`UM`) REFERENCES `um` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=latin1 COMMENT='Various machines in the plant';

-- Dumping data for table hcs-db2.machines: ~52 rows (approximately)
DELETE FROM `machines`;
/*!40000 ALTER TABLE `machines` DISABLE KEYS */;
INSERT INTO `machines` (`id`, `machine_name`, `quantity`, `UM`, `capacity`, `deptID`) VALUES
	(1, 'Komori 4', 1, 1, 15000, 1),
	(2, 'Komori 3', 1, 1, 15000, 1),
	(3, 'Silk Screen', 1, 1, 8000, 1),
	(4, 'MagMaster #1', 1, 2, 88000, 3),
	(5, 'MagMaster #2', 1, 2, 88000, 3),
	(6, 'MTL-700 #1', 1, 1, 3500, 3),
	(7, 'MTL-700 #2', 1, 1, 3500, 3),
	(8, 'MTL-700 #3', 1, 1, 3500, 3),
	(9, 'SYSCO', 1, 1, 4000, 4),
	(10, 'Collate #1', 1, 1, 2800, 4),
	(11, 'Collate #2', 1, 1, 2800, 4),
	(12, 'OASYS', 1, 1, 2500, 4),
	(13, 'Car 25 #1', 1, 1, 2400, 5),
	(14, 'Car 25 #2', 1, 1, 2400, 5),
	(15, 'Car 25 #3', 1, 1, 2400, 5),
	(16, 'Car 25 #4', 1, 1, 2400, 5),
	(17, 'Bobst', 1, 1, 13000, 5),
	(18, 'Spartanic', 1, 1, 1600, 5),
	(19, 'Inspection Cards', 1, 2, 135000, 6),
	(20, 'Versa 5', 1, 2, 140000, 6),
	(21, 'Versa 6', 1, 2, 140000, 6),
	(22, 'Versa 7', 1, 2, 140000, 6),
	(23, 'Versa 8', 1, 2, 140000, 6),
	(24, 'Versa 9', 1, 2, 140000, 6),
	(25, 'Cardline 2', 1, 2, 80000, 6),
	(26, 'MagMaster', 1, 2, 80000, 6),
	(27, 'Proof Master 1', 1, 2, 150000, 7),
	(28, 'Proof Master 2', 1, 2, 150000, 7),
	(29, 'Proof Master 3', 1, 2, 150000, 7),
	(30, 'Proof Master 4', 1, 2, 150000, 7),
	(31, 'Proof Master 5', 1, 2, 150000, 7),
	(32, 'Proof Master-MP', 1, 2, 150000, 7),
	(33, 'Assembly', 1, 3, 50000, 8),
	(34, 'Haipai #1', 1, 3, 16000, 8),
	(35, 'Haipai #2', 1, 3, 16000, 8),
	(36, 'Alloyd Heat Seal 1', 1, 3, 12500, 8),
	(37, 'Alloyd Heat Seal 2', 1, 3, 12500, 8),
	(38, 'Alloyd Heat Seal 3', 1, 3, 12500, 8),
	(39, 'Incomm Heat Seal 1', 1, 3, 11000, 8),
	(40, 'Incomm Heat Seal 2', 1, 3, 11000, 8),
	(41, 'Incomm Heat Seal 3', 1, 3, 11000, 8),
	(42, 'Incomm Heat Seal 4', 1, 3, 11000, 8),
	(43, 'Tipping Machine #1', 1, 3, 90000, 11),
	(44, 'Tipping Machine #2', 1, 3, 90000, 11),
	(45, 'Tipping Machine #3', 1, 3, 90000, 11),
	(46, 'Manual Packing 1', 1, 2, 40000, 11),
	(47, 'Manual Packing 2', 1, 2, 40000, 11),
	(48, 'Auto Shrink Wrap 1', 1, 2, 180000, 11),
	(49, 'Auto Shrink Wrap 2', 1, 2, 180000, 11),
	(50, 'Auto Shrink Wrap 3', 1, 2, 180000, 11),
	(51, 'Auto Shrink Wrap 4', 1, 2, 180000, 11),
	(52, 'Multi-pack Auto Pack', 1, 3, 80000, 11);
/*!40000 ALTER TABLE `machines` ENABLE KEYS */;


-- Dumping structure for table hcs-db2.po_specs
CREATE TABLE IF NOT EXISTS `po_specs` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `group_id` int(10) NOT NULL DEFAULT '1' COMMENT '''Other'' type',
  `member_id` int(10) NOT NULL DEFAULT '1' COMMENT '''Other'' type',
  `member_description` varchar(50) NOT NULL DEFAULT 'Other' COMMENT '''Other'' type',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `FK_po_specs_po_spec_groups` (`group_id`),
  CONSTRAINT `FK_po_specs_po_spec_groups` FOREIGN KEY (`group_id`) REFERENCES `po_spec_groups` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=81 DEFAULT CHARSET=latin1 COMMENT='Various PO specification types, grouped by common group_id';

-- Dumping data for table hcs-db2.po_specs: ~80 rows (approximately)
DELETE FROM `po_specs`;
/*!40000 ALTER TABLE `po_specs` DISABLE KEYS */;
INSERT INTO `po_specs` (`id`, `group_id`, `member_id`, `member_description`) VALUES
	(1, 1, 1, 'Other'),
	(2, 2, 1, 'CR50'),
	(3, 2, 2, 'CR80'),
	(4, 2, 3, '4.875'),
	(5, 2, 4, '5.25'),
	(6, 2, 5, '6.375'),
	(7, 2, 5, '7.125'),
	(8, 3, 1, '10 mil'),
	(9, 3, 2, '12 mil'),
	(10, 3, 3, '15 mil'),
	(11, 3, 4, '20 mil'),
	(12, 3, 5, '24 mil'),
	(13, 3, 6, '26 mil'),
	(14, 3, 7, '28 mil'),
	(15, 3, 8, '30 mil'),
	(16, 4, 1, 'PVC'),
	(17, 4, 2, 'PVC Gold'),
	(18, 4, 3, 'PVC Silver'),
	(19, 4, 4, 'Styrene'),
	(20, 5, 1, '2/1'),
	(21, 5, 2, '4/1'),
	(22, 5, 3, '4/4'),
	(23, 6, 1, '2 Lay 2 bleed'),
	(24, 6, 2, '2 lay 1 bleed'),
	(25, 6, 3, '2 Lay 0 bleed'),
	(26, 6, 4, '1 Lay 1 bleed'),
	(27, 6, 5, '1 Lay 0 bleed'),
	(28, 6, 6, 'Hy Poly'),
	(29, 6, 7, 'UV Coate'),
	(30, 7, 1, 'No Mag'),
	(31, 7, 2, 'HiCo 3 track'),
	(32, 7, 3, 'HiCo 2 track'),
	(33, 7, 4, 'LoCo 3 track'),
	(34, 7, 5, 'LoCo 2 track'),
	(35, 7, 6, 'LoCo 1 track'),
	(36, 8, 1, 'No Pin'),
	(37, 8, 2, '1 Pin #'),
	(38, 8, 3, '2 number'),
	(39, 8, 4, 'special'),
	(40, 9, 1, 'No'),
	(41, 9, 2, '1 BarCode'),
	(42, 9, 3, '2 BarCode'),
	(43, 9, 4, 'Special'),
	(44, 10, 1, 'No'),
	(45, 10, 2, 'SOL'),
	(46, 10, 3, 'HS SOF'),
	(47, 10, 4, 'Special'),
	(48, 11, 1, 'No'),
	(49, 11, 2, 'Gold'),
	(50, 11, 3, 'Silver'),
	(51, 12, 1, 'No'),
	(52, 12, 2, 'Print'),
	(53, 12, 3, 'HotStamp'),
	(54, 13, 1, 'No'),
	(55, 13, 2, '1 Lay'),
	(56, 13, 3, '2 Lay'),
	(57, 13, 4, '3 Lay'),
	(58, 14, 1, 'No'),
	(59, 14, 2, 'Hs Die'),
	(60, 14, 3, 'Cutm Die'),
	(61, 14, 4, 'Special'),
	(62, 15, 1, 'Yes'),
	(63, 15, 2, 'No'),
	(64, 16, 1, '10'),
	(65, 16, 2, '15'),
	(66, 16, 3, '25'),
	(67, 16, 4, '50'),
	(68, 16, 5, '100'),
	(69, 16, 6, '500'),
	(70, 17, 1, '100'),
	(71, 17, 2, '200'),
	(72, 17, 3, '400'),
	(73, 17, 4, '500'),
	(74, 17, 5, '1000'),
	(75, 18, 1, '1000'),
	(76, 18, 2, '2000'),
	(77, 18, 3, '2500'),
	(78, 18, 4, '4000'),
	(79, 18, 5, '5000'),
	(80, 18, 6, '6000');
/*!40000 ALTER TABLE `po_specs` ENABLE KEYS */;


-- Dumping structure for table hcs-db2.po_spec_groups
CREATE TABLE IF NOT EXISTS `po_spec_groups` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `group_name` varchar(50) NOT NULL DEFAULT 'Other',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=latin1 COMMENT='To define PO specification groups\r\n';

-- Dumping data for table hcs-db2.po_spec_groups: ~18 rows (approximately)
DELETE FROM `po_spec_groups`;
/*!40000 ALTER TABLE `po_spec_groups` DISABLE KEYS */;
INSERT INTO `po_spec_groups` (`id`, `group_name`) VALUES
	(1, 'Other'),
	(2, 'Card Size'),
	(3, 'Thickness'),
	(4, 'Material'),
	(5, '# of Color'),
	(6, 'Lamination'),
	(7, 'Magnetic'),
	(8, 'Pin / No'),
	(9, 'BarCode'),
	(10, 'SOL'),
	(11, 'Hot Stamp'),
	(12, 'Signature Panel'),
	(13, 'Silk Screen'),
	(14, 'Hole Punch'),
	(15, 'Single Pack'),
	(16, 'Bundle Pack'),
	(17, 'Inner Box'),
	(18, 'Outer Box');
/*!40000 ALTER TABLE `po_spec_groups` ENABLE KEYS */;


-- Dumping structure for table hcs-db2.sales_vars
CREATE TABLE IF NOT EXISTS `sales_vars` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `user_id` int(10) NOT NULL DEFAULT '2',
  `cust_po_number` varchar(50) NOT NULL DEFAULT '-no val-',
  `notes` varchar(50) NOT NULL DEFAULT '-no val-',
  `cust_name` varchar(50) NOT NULL DEFAULT '-no val-',
  `req_date` date NOT NULL DEFAULT '2013-04-25',
  `order_qty` varchar(50) NOT NULL DEFAULT '1',
  `card_name` varchar(200) NOT NULL DEFAULT '-no val-',
  `card_denom` varchar(50) NOT NULL DEFAULT '-no val-',
  `card_size` varchar(50) NOT NULL DEFAULT '-no val-',
  `card_thickness` varchar(50) NOT NULL DEFAULT '-no val-',
  `card_material` varchar(50) NOT NULL DEFAULT '-no val-',
  `num_colors` varchar(50) NOT NULL DEFAULT '4/1',
  `lamination` varchar(50) NOT NULL DEFAULT '-no val-',
  `magnetic` varchar(50) NOT NULL DEFAULT '-no val-',
  `pin` varchar(50) NOT NULL DEFAULT '-no val-',
  `barcode` varchar(50) NOT NULL DEFAULT '-no val-',
  `single_pack` varchar(50) NOT NULL DEFAULT '-no val-',
  `sol` varchar(50) NOT NULL DEFAULT '-no val-',
  `bundle_pack` varchar(50) NOT NULL DEFAULT '-no val-',
  `hot_stamp` varchar(50) NOT NULL DEFAULT '-no val-',
  `inner_box` varchar(50) NOT NULL DEFAULT '-no val-',
  `signature_panel` varchar(50) NOT NULL DEFAULT '-no val-',
  `outer_box` varchar(50) NOT NULL DEFAULT '-no val-',
  `silk_screen` varchar(50) NOT NULL DEFAULT '-no val-',
  `cvv` varchar(50) NOT NULL DEFAULT '-no val-' COMMENT '4 Digit CVV',
  `hole_punch` varchar(50) NOT NULL DEFAULT '-no val-',
  `DataApproved` enum('Y','N') NOT NULL DEFAULT 'N',
  `ArtApproved` enum('Y','N') NOT NULL DEFAULT 'N',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `Index 2` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

-- Dumping data for table hcs-db2.sales_vars: ~7 rows (approximately)
DELETE FROM `sales_vars`;
/*!40000 ALTER TABLE `sales_vars` DISABLE KEYS */;
INSERT INTO `sales_vars` (`id`, `user_id`, `cust_po_number`, `notes`, `cust_name`, `req_date`, `order_qty`, `card_name`, `card_denom`, `card_size`, `card_thickness`, `card_material`, `num_colors`, `lamination`, `magnetic`, `pin`, `barcode`, `single_pack`, `sol`, `bundle_pack`, `hot_stamp`, `inner_box`, `signature_panel`, `outer_box`, `silk_screen`, `cvv`, `hole_punch`, `DataApproved`, `ArtApproved`, `timestamp`) VALUES
	(1, 2, '10001', '-no val-', 'incomm', '2013-06-30', '100', 'tmobile', '$30', 'CR80', '10 mil', 'PVC', '2/1', '2 Lay 2 bleed', 'No Mag', 'No Pin', 'No', 'Yes', 'No', '10', 'No', '100', 'No', '2000', 'No', 'yes', 'No', 'Y', 'Y', '2013-06-05 15:56:25'),
	(2, 2, '1002', '', 'inc', '2013-06-07', '100', 'starbucks', '$10', 'CR50', '10 mil', 'PVC', '2/1', '2 Lay 2 bleed', 'No Mag', 'No Pin', 'No', 'Yes', 'No', '10', 'No', '100', 'No', '2500', 'No', '', 'No', 'N', 'Y', '2013-06-05 21:14:22'),
	(3, 10, '10001', '-no val-', 'incomm', '2013-06-30', '200', 'tmobile', '$10', 'CR80', '10 mil', 'PVC', '2/1', '2 Lay 2 bleed', 'No Mag', 'No Pin', 'No', 'Yes', 'No', '10', 'No', '100', 'No', '2000', 'No', 'no', 'No', 'Y', 'Y', '2013-06-06 00:59:33'),
	(6, 10, '3459', '', 'blackhawk', '2013-06-14', '100', 'tmobile', '$10', 'M6', '10 mil', 'PVC', '2/1', '2 Lay 2 bleed', 'No Mag', 'No Pin', 'No', 'Yes', 'No', '10', 'No', '100', 'No', '1000', 'No', 'no', 'No', 'N', 'Y', '2013-06-10 05:25:36'),
	(7, 10, '3459', '', 'blackhawk', '2013-06-14', '200', 'tmobile', '$20', 'CR80', '10 mil', 'PVC', '2/1', '2 Lay 2 bleed', 'No Mag', 'No Pin', 'No', 'Yes', 'No', '10', 'No', '100', 'No', '2000', 'No', 'yes', 'No', 'N', 'Y', '2013-06-10 05:26:02'),
	(8, 10, '1003', '', 'nPO', '2013-07-04', '100', 'sb', '$10', 'CR50', '10 mil', 'PVC', '2/1', '2 Lay 2 bleed', 'No Mag', 'No Pin', 'No', 'Yes', 'No', '10', 'No', '100', 'No', '1000', 'No', 'no', 'No', 'N', 'N', '2013-06-11 23:18:34'),
	(9, 10, '1003', '', 'nPO', '2013-07-04', '200', 'sb2', '$30', 'CR50', '10 mil', 'PVC', '2/1', '2 Lay 2 bleed', 'No Mag', 'No Pin', 'No', 'Yes', 'No', '10', 'No', '100', 'No', '1000', 'No', 'yes', 'No', 'N', 'N', '2013-06-11 23:18:57');
/*!40000 ALTER TABLE `sales_vars` ENABLE KEYS */;


-- Dumping structure for table hcs-db2.um
CREATE TABLE IF NOT EXISTS `um` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `description` varchar(50) NOT NULL DEFAULT '-not defined-',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `Index 1` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1 COMMENT='Unit of Measurements, used for machines';

-- Dumping data for table hcs-db2.um: ~3 rows (approximately)
DELETE FROM `um`;
/*!40000 ALTER TABLE `um` DISABLE KEYS */;
INSERT INTO `um` (`id`, `description`) VALUES
	(1, 'SHEET'),
	(2, 'CARD'),
	(3, 'PACK');
/*!40000 ALTER TABLE `um` ENABLE KEYS */;


-- Dumping structure for table hcs-db2.users
CREATE TABLE IF NOT EXISTS `users` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `fName` varchar(50) NOT NULL DEFAULT '0',
  `lName` varchar(50) NOT NULL DEFAULT '0',
  `username` char(10) NOT NULL DEFAULT '0',
  `password` varchar(50) NOT NULL DEFAULT '0',
  `Active` enum('Y','N') NOT NULL DEFAULT 'Y',
  `user_group` int(10) NOT NULL DEFAULT '3',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `Index 1` (`id`),
  KEY `FK_users_user_groups` (`user_group`),
  CONSTRAINT `FK_users_user_groups` FOREIGN KEY (`user_group`) REFERENCES `user_groups` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1 COMMENT='user table to store login info and access rights';

-- Dumping data for table hcs-db2.users: ~10 rows (approximately)
DELETE FROM `users`;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`id`, `fName`, `lName`, `username`, `password`, `Active`, `user_group`) VALUES
	(1, 'Andy', 'Kuo', 'aKuo', 'akuo', 'Y', 2),
	(2, 'Chinmay', 'Muranjan', 'cMuranjan', 'cmuranjan', 'Y', 1),
	(3, 'Richard', 'Ko', 'rKo', 'rko', 'Y', 2),
	(4, 'Vicky', 'Ding', 'vDing', 'vding', 'Y', 2),
	(5, 'Ken', 'Chang', 'kChang', 'kchang', 'Y', 2),
	(6, 'Vincent', 'Lo', 'vLo', 'vlo', 'Y', 2),
	(7, 'Michael', 'Wong', 'mWong', 'mwong', 'Y', 2),
	(8, 'Sunny', 'Dai', 'sDai', 'sdai', 'Y', 2),
	(9, 'Almon', 'Lin', 'aLin', 'alin', 'Y', 1),
	(10, 'Sandy', 'Sandoval', 'sSandoval', 'ssandoval', 'Y', 2);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;


-- Dumping structure for table hcs-db2.user_activity
CREATE TABLE IF NOT EXISTS `user_activity` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `user_id` int(10) NOT NULL DEFAULT '0',
  `table_name` varchar(50) NOT NULL DEFAULT '0',
  `field_name` varchar(50) NOT NULL DEFAULT '0',
  `row_id` int(10) NOT NULL DEFAULT '0',
  `old_value` varchar(50) NOT NULL DEFAULT '0',
  `new_value` varchar(50) NOT NULL DEFAULT '0',
  `operation` enum('UPDATE','INSERT','DELETE') NOT NULL DEFAULT 'UPDATE',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `FKuser_id` (`user_id`),
  CONSTRAINT `FK_user_activity_users` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=155 DEFAULT CHARSET=latin1 COMMENT='monitor user activity';

-- Dumping data for table hcs-db2.user_activity: ~2 rows (approximately)
DELETE FROM `user_activity`;
/*!40000 ALTER TABLE `user_activity` DISABLE KEYS */;
INSERT INTO `user_activity` (`id`, `user_id`, `table_name`, `field_name`, `row_id`, `old_value`, `new_value`, `operation`, `timestamp`) VALUES
	(153, 2, 'graphics_vars_gang', 'sales_vars_id', 1, '1', '10001', 'UPDATE', '2013-06-11 19:44:56'),
	(154, 5, 'graphics_vars_gang', 'sales_vars_id', 2, '6', '3459', 'UPDATE', '2013-06-11 19:45:15');
/*!40000 ALTER TABLE `user_activity` ENABLE KEYS */;


-- Dumping structure for table hcs-db2.user_groups
CREATE TABLE IF NOT EXISTS `user_groups` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `groupName` varchar(50) NOT NULL DEFAULT '-no name-',
  `canModify` enum('Y','N') NOT NULL DEFAULT 'N',
  `canCreate` enum('Y','N') NOT NULL DEFAULT 'N',
  `canView` enum('Y','N') NOT NULL DEFAULT 'N',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `id_2` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1 COMMENT='User groups to manage access rights for data security and application security';

-- Dumping data for table hcs-db2.user_groups: ~5 rows (approximately)
DELETE FROM `user_groups`;
/*!40000 ALTER TABLE `user_groups` DISABLE KEYS */;
INSERT INTO `user_groups` (`id`, `groupName`, `canModify`, `canCreate`, `canView`) VALUES
	(1, 'Admin', 'Y', 'Y', 'Y'),
	(2, 'SuperUser', 'Y', 'Y', 'Y'),
	(3, 'HighUser', 'Y', 'N', 'Y'),
	(4, 'MidUser', 'N', 'N', 'Y'),
	(5, 'LowUser', 'N', 'N', 'N');
/*!40000 ALTER TABLE `user_groups` ENABLE KEYS */;


-- Dumping structure for table hcs-db2.user_sessions
CREATE TABLE IF NOT EXISTS `user_sessions` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `user_id` int(10) NOT NULL,
  `session_id` varchar(50) NOT NULL,
  `datetime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `activity_count` int(10) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  KEY `Index 1` (`id`),
  KEY `FK_user_sessions_users` (`user_id`),
  CONSTRAINT `FK_user_sessions_users` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=298 DEFAULT CHARSET=latin1 COMMENT='To store all user sessions';

-- Dumping data for table hcs-db2.user_sessions: ~46 rows (approximately)
DELETE FROM `user_sessions`;
/*!40000 ALTER TABLE `user_sessions` DISABLE KEYS */;
INSERT INTO `user_sessions` (`id`, `user_id`, `session_id`, `datetime`, `activity_count`) VALUES
	(233, 1, 'f081b511f835adf0202998ebf61b3b30', '2013-06-11 10:27:36', 1),
	(234, 1, '4c785cd6604fcc78aeb29adb8a004db4', '2013-06-11 10:30:57', 23),
	(254, 5, 'c3d1f1a200b50597edfd74e7c402e1f0', '2013-06-11 23:24:38', 51),
	(255, 10, 'd07d3248812caeb156fd706210e62e55', '2013-06-11 23:28:06', 47),
	(256, 1, 'd2caea3be0c02315425b049dd00f786a', '2013-06-12 00:31:38', 1),
	(257, 1, '41bc56f6eb3ab3b300a73a4622bd8fa8', '2013-06-12 00:35:34', 3),
	(258, 1, '820aa9f5d7f3fcd149e1e9cb74e56a93', '2013-06-12 00:46:01', 3),
	(259, 1, '3044ecea03cb7bab7c916079f8174365', '2013-06-12 01:06:32', 3),
	(260, 1, '1252c48b824c65d3c47fa36fa932ebe7', '2013-06-12 01:36:06', 3),
	(261, 1, 'b75e2bf5f9bbadec5f0841a84df06447', '2013-06-12 10:06:58', 3),
	(262, 1, '4c75efd809993315c069fdd8988eb4ea', '2013-06-12 10:09:53', 3),
	(263, 1, '8ef0784436a7cbc30e567f74f078ccf7', '2013-06-12 10:15:42', 3),
	(264, 1, '1b7ee58828a3ea07ab3b7df3cdf949de', '2013-06-12 10:22:01', 3),
	(265, 1, '2c3da9eef0e43a83dfcd42724dd1c217', '2013-06-12 10:52:05', 3),
	(266, 1, 'c602033f6526f7ea792c37c5325b5dcf', '2013-06-12 10:54:02', 3),
	(267, 1, '6ce97f162715bfc69cdf32f4532f112f', '2013-06-12 10:55:14', 3),
	(268, 1, '802f24548fb3e9dab40d107343f3ad0b', '2013-06-12 10:56:07', 3),
	(269, 1, '8b2f23b39a35b3e4f1d4db10939bd7d9', '2013-06-12 10:57:18', 3),
	(270, 1, 'f41625773c4872e4abaebcd8788bda74', '2013-06-12 10:58:50', 3),
	(271, 1, '6cbe5e90bc7f2a1665e14e8fc3e8a9bc', '2013-06-12 11:00:16', 3),
	(272, 1, 'abb8435b755b7907eb1fb26fb896819d', '2013-06-12 11:02:45', 3),
	(273, 1, '266999bc0cdb399210dfa706963c0d3e', '2013-06-12 11:03:56', 3),
	(274, 1, 'e0698a61940f7298f9af86db6d2cc295', '2013-06-12 11:12:58', 3),
	(275, 1, '3d23fc7e534bb76e1be37080e3bb252d', '2013-06-12 11:17:21', 3),
	(276, 1, 'd953e21da7149d29059e89fbe68194a5', '2013-06-12 11:21:59', 3),
	(277, 1, '93e40e53be2f3f50d65c5faa0107715b', '2013-06-12 11:37:54', 3),
	(278, 1, 'b1807fa60fb0e9a3cf734b560e5e8b4e', '2013-06-12 11:42:59', 5),
	(279, 1, 'ae36342ce09292a0d17a9271f43371f6', '2013-06-12 11:44:00', 3),
	(280, 1, '33320ecc74ad4bad3b34c1d2d20b5fb4', '2013-06-12 11:57:33', 3),
	(281, 1, '266bd4ba8dc2e581f8b92be5a54c4343', '2013-06-12 11:59:21', 3),
	(282, 1, 'fc49a4069822ed3dd7b1adbc12dca578', '2013-06-12 12:09:24', 3),
	(283, 1, '97f9ec5d42594228221f264bc345ed1e', '2013-06-12 12:42:23', 3),
	(284, 1, '46c1c6647c4cd9be3c94eb23190a4493', '2013-06-12 12:53:48', 5),
	(285, 1, '7a8d2d34b255b4d84fabe689299a17e8', '2013-06-12 12:57:06', 5),
	(286, 1, 'dbce1fabf3f9284e41eeb6ce9ac768a1', '2013-06-12 12:59:39', 5),
	(287, 1, 'a3d67638ae20ee601a8ff3b481220164', '2013-06-12 13:02:50', 5),
	(288, 1, 'd6dd6dab09f503c97c0a3bf128bb0f55', '2013-06-12 13:03:22', 5),
	(289, 1, '099043f0fe987a2261edb3502e9a237d', '2013-06-12 13:04:20', 5),
	(290, 1, '8536a4e8c476a8879fcbc72e1b81aa37', '2013-06-12 13:23:19', 5),
	(291, 1, 'c31136856617381e8c8821a61c05f82d', '2013-06-12 13:27:05', 3),
	(292, 1, '4f577db51fd9e133dfa2669c18032e8d', '2013-06-12 13:43:05', 5),
	(293, 1, '2aabb837127b1c4b60bae61f82030ccf', '2013-06-12 13:45:56', 5),
	(294, 1, '2d0e6b823374ea447be4b3276c86979e', '2013-06-12 13:48:16', 8),
	(295, 1, 'ed0a2852f95f68622d23acb2749ddb4f', '2013-06-12 13:49:37', 5),
	(296, 1, '2bdb74402424dc5d035f5dd0290a77e8', '2013-06-12 13:52:20', 4),
	(297, 1, 'e6604ad3cea20d2d589414595fe779fb', '2013-06-12 13:55:12', 4);
/*!40000 ALTER TABLE `user_sessions` ENABLE KEYS */;


-- Dumping structure for view hcs-db2.v_departments_subdept_users
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `v_departments_subdept_users` (
	`Dept_ID` INT(10) NOT NULL,
	`Parent_Dept_Name` VARCHAR(50) NOT NULL COLLATE 'latin1_swedish_ci',
	`Sub_Dept_Name` VARCHAR(50) NOT NULL COLLATE 'latin1_swedish_ci',
	`Users_Assigned` TEXT NULL DEFAULT NULL COLLATE 'latin1_swedish_ci',
	`Users_Assigned_ID` TEXT NULL DEFAULT NULL COLLATE 'utf8_general_ci'
) ENGINE=MyISAM;


-- Dumping structure for view hcs-db2.v_departments_users
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `v_departments_users` (
	`Dept_ID` INT(10) NOT NULL DEFAULT '0',
	`Parent_Dept_Name` VARCHAR(50) NOT NULL COLLATE 'latin1_swedish_ci',
	`Sub_Dept_Name` VARCHAR(50) NOT NULL COLLATE 'latin1_swedish_ci',
	`Users_Assigned` TEXT NULL DEFAULT NULL COLLATE 'latin1_swedish_ci',
	`Users_Assigned_ID` TEXT NULL DEFAULT NULL COLLATE 'utf8_general_ci'
) ENGINE=MyISAM;


-- Dumping structure for view hcs-db2.v_usergroup_users
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `v_usergroup_users` (
	`User_Group_ID` INT(10) NOT NULL DEFAULT '0',
	`Group_Name` VARCHAR(50) NOT NULL DEFAULT '-no name-' COLLATE 'latin1_swedish_ci',
	`User_ID` TEXT NULL DEFAULT NULL COLLATE 'utf8_general_ci',
	`Users_Assigned` TEXT NULL DEFAULT NULL COLLATE 'latin1_swedish_ci'
) ENGINE=MyISAM;


-- Dumping structure for view hcs-db2.v_user_depts
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `v_user_depts` (
	`User_ID` INT(10) NOT NULL DEFAULT '0',
	`First_Name` VARCHAR(50) NOT NULL DEFAULT '0' COLLATE 'latin1_swedish_ci',
	`Last_Name` VARCHAR(50) NOT NULL DEFAULT '0' COLLATE 'latin1_swedish_ci',
	`Dept_Name` TEXT NULL DEFAULT NULL COLLATE 'latin1_swedish_ci',
	`Dept_ID` TEXT NULL DEFAULT NULL COLLATE 'utf8_general_ci',
	`User_Group_ID` INT(10) NOT NULL DEFAULT '0',
	`User_Group_name` VARCHAR(50) NOT NULL DEFAULT '-no name-' COLLATE 'latin1_swedish_ci'
) ENGINE=MyISAM;


-- Dumping structure for trigger hcs-db2.graphics_vars_gang_t_au
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='';
DELIMITER //
CREATE TRIGGER `graphics_vars_gang_t_au` AFTER UPDATE ON `graphics_vars_gang` FOR EACH ROW BEGIN
	IF OLD.sales_vars_id <> NEW.sales_vars_id THEN
  		INSERT INTO user_activity (user_id, table_name, field_name, row_id, old_value, new_value, operation) 
  		VALUES (NEW.user_id, "graphics_vars_gang", "sales_vars_id", NEW.id, OLD.sales_vars_id, NEW.sales_vars_id, "UPDATE");
  	END IF;
END//
DELIMITER ;
SET SQL_MODE=@OLD_SQL_MODE;


-- Dumping structure for trigger hcs-db2.graphics_vars_job_t_au
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='';
DELIMITER //
CREATE TRIGGER `graphics_vars_job_t_au` AFTER UPDATE ON `graphics_vars_job` FOR EACH ROW BEGIN
	IF OLD.sales_vars_id <> NEW.sales_vars_id THEN
  		INSERT INTO user_activity (user_id, table_name, field_name, row_id, old_value, new_value, operation) 
  		VALUES (NEW.user_id, "graphics_vars_job", "sales_vars_id", NEW.id, OLD.sales_vars_id, NEW.sales_vars_id, "UPDATE");
  	END IF;
END//
DELIMITER ;
SET SQL_MODE=@OLD_SQL_MODE;


-- Dumping structure for trigger hcs-db2.sales_vars_t_au
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='';
DELIMITER //
CREATE TRIGGER `sales_vars_t_au` AFTER UPDATE ON `sales_vars` FOR EACH ROW BEGIN
	IF OLD.cust_po_number <> NEW.cust_po_number THEN
  		INSERT INTO user_activity (user_id, table_name, field_name, row_id, old_value, new_value, operation) 
  		VALUES (NEW.user_id, "sales_vars", "cust_po_number", NEW.id, OLD.cust_po_number, NEW.cust_po_number, "UPDATE");
  	END IF;
END//
DELIMITER ;
SET SQL_MODE=@OLD_SQL_MODE;


-- Dumping structure for view hcs-db2.v_departments_subdept_users
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `v_departments_subdept_users`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_departments_subdept_users` AS select `d1`.`parentID` AS `Dept_ID`,`d2`.`description` AS `Parent_Dept_Name`,`d1`.`description` AS `Sub_Dept_Name`,group_concat(concat_ws(' ',`hcs-db1`.`users`.`fName`,`hcs-db1`.`users`.`lName`) order by `hcs-db1`.`users`.`id` ASC separator ', ') AS `Users_Assigned`,group_concat(`hcs-db1`.`users`.`id` order by `hcs-db1`.`users`.`id` ASC separator ', ') AS `Users_Assigned_ID` from (((`hcs-db1`.`departments` `d1` join `hcs-db1`.`departments` `d2` on((`d1`.`parentID` = `d2`.`id`))) join `hcs-db1`.`dept_users` on((`hcs-db1`.`dept_users`.`deptID` = `d1`.`parentID`))) join `hcs-db1`.`users` on((`hcs-db1`.`dept_users`.`userID` = `hcs-db1`.`users`.`id`))) group by `d1`.`id`;


-- Dumping structure for view hcs-db2.v_departments_users
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `v_departments_users`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_departments_users` AS select `d2`.`id` AS `Dept_ID`,`d2`.`description` AS `Parent_Dept_Name`,`d1`.`description` AS `Sub_Dept_Name`,group_concat(concat_ws(' ',`hcs-db1`.`users`.`fName`,`hcs-db1`.`users`.`lName`) order by `hcs-db1`.`users`.`id` ASC separator ', ') AS `Users_Assigned`,group_concat(`hcs-db1`.`users`.`id` order by `hcs-db1`.`users`.`id` ASC separator ', ') AS `Users_Assigned_ID` from (((`hcs-db1`.`departments` `d1` join `hcs-db1`.`departments` `d2` on((`d1`.`id` = `d2`.`id`))) join `hcs-db1`.`dept_users` on((`hcs-db1`.`dept_users`.`deptID` = `d1`.`id`))) join `hcs-db1`.`users` on((`hcs-db1`.`dept_users`.`userID` = `hcs-db1`.`users`.`id`))) group by `d1`.`id`;


-- Dumping structure for view hcs-db2.v_usergroup_users
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `v_usergroup_users`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_usergroup_users` AS select `hcs-db1`.`user_groups`.`id` AS `User_Group_ID`,`hcs-db1`.`user_groups`.`groupName` AS `Group_Name`,group_concat(`hcs-db1`.`users`.`id` order by `hcs-db1`.`users`.`id` ASC separator ', ') AS `User_ID`,group_concat(concat_ws(' ',`hcs-db1`.`users`.`fName`,`hcs-db1`.`users`.`lName`) order by `hcs-db1`.`users`.`id` ASC separator ', ') AS `Users_Assigned` from (`hcs-db1`.`user_groups` join `hcs-db1`.`users` on((`hcs-db1`.`user_groups`.`id` = `hcs-db1`.`users`.`user_group`))) group by `hcs-db1`.`user_groups`.`id`;


-- Dumping structure for view hcs-db2.v_user_depts
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `v_user_depts`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_user_depts` AS select `hcs-db1`.`users`.`id` AS `User_ID`,`hcs-db1`.`users`.`fName` AS `First_Name`,`hcs-db1`.`users`.`lName` AS `Last_Name`,group_concat(`hcs-db1`.`departments`.`description` order by `hcs-db1`.`departments`.`id` ASC separator ', ') AS `Dept_Name`,group_concat(`hcs-db1`.`departments`.`id` order by `hcs-db1`.`departments`.`id` ASC separator ', ') AS `Dept_ID`,`hcs-db1`.`user_groups`.`id` AS `User_Group_ID`,`hcs-db1`.`user_groups`.`groupName` AS `User_Group_name` from (((`hcs-db1`.`users` join `hcs-db1`.`dept_users` on((`hcs-db1`.`dept_users`.`userID` = `hcs-db1`.`users`.`id`))) join `hcs-db1`.`departments` on((`hcs-db1`.`dept_users`.`deptID` = `hcs-db1`.`departments`.`id`))) join `hcs-db1`.`user_groups` on((`hcs-db1`.`user_groups`.`id` = `hcs-db1`.`users`.`user_group`))) group by `hcs-db1`.`users`.`id`;
/*!40014 SET FOREIGN_KEY_CHECKS=1 */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
