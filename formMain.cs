/*
 *  EQEmuItemEditor - Application for customizing EQEmu server items
 *  
 *  By Shendare (Jon D. Jackson)
 * 
 *  Portions of this code not covered by another author's or entity's copyright are released under
 *  the Creative Commons Zero (CC0) public domain license.
 *  
 *  To the extent possible under law, Shendare (Jon D. Jackson) has waived all copyright and
 *  related or neighboring rights to this EQEmuItemEditor application.
 *  This work is published from: The United States. 
 *  
 *  You may copy, modify, and distribute the work, even for commercial purposes, without asking permission.
 * 
 *  For more information, read the CC0 summary and full legal text here:
 *  
 *  https://creativecommons.org/publicdomain/zero/1.0/
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace EQEmuItemEditor
{
    public partial class formMain : Form
    {
        #region Lists and Constants

        public static string[] SizeNames = new string[] {
            "TINY", "SMALL", "MEDIUM", "LARGE", "GIANT", "GIGANTIC" };

        public static string[] LDoNThemes = new string[] {
            "Guk", "Miragul's", "Mistmoore", "Rujarkan", "Takish-Hiz" };

        public static string[] Currencies = new string[] {
            "Coins", "LDoN Points", "Discord Points", "Unknown (3)", "Radiant Crystals", "Ebon Crystals" };

        public static string[] Resists = new string[] {
            "Unresistable", "Magic", "Fire", "Cold", "Poison", "Disease", "Chromatc", "Prismatic", "Physical", "Corruption" };

        public static string[] ClassNicks = new string[] {
            "WAR", "CLR", "PAL", "RNG", "SHD", "DRU", "MNK", "BRD", "ROG", "SHM", "NEC", "WIZ", "MAG", "ENC", "BST", "BER" };

        public static string[] RaceNicks = new string[] {
            "HUM", "BAR", "ERU", "ELF", "HIE", "DEF", "HEF", "DWF", "TRL", "OGR", "HLF", "GNM", "IKS", "VAH", "FRG", "DRK", "+ Shroud" };

        // Toggles in the Basic tab
        public static string[] ItemFlagFields = new string[] {
            "magic", 
            "loregroup", 
            "-nodrop", 
            "fvnodrop", 
            "notransfer",
            "-norent", 
            "summonedflag", 
            "questitemflag", 
            "artifactflag", 
            "nopet", 
            "attuneable", 
            "tradeskills", 
            "stackable",
            "book",
            "elitematerial", 
            "charmfileid",
            "potionbelt",
            "benefitflag" };

        // Toggles in the Effects tab
        public static string[] ItemFlagFields2 = new string[] {
            "potionbeltslots", 
            "expendablearrow" };

        public static string[] Deities = new string[] {
            "Agnostic",
            "Bertoxxulous",
            "Brell Serilis",
            "Cazic Thule",
            "Erollisi Marr",
            "Bristlebane",
            "Innoruuk",
            "Karana",
            "Mithaniel Marr",
            "Prexus",
            "Quellious",
            "Rallos Zek",
            "Rodcet Nife",
            "Solusek Ro",
            "The Tribunal",
            "Tunare",
            "Veeshan",
        };

		public static string[] SlotNames = new string[] {
            "Charm",
            "Ear",
            "Head",
            "Face",
            "Ear",
            "Neck",
            "Shoulders",
            "Arms",
            "Back",
            "Wrist",
            "Wrist",
            "Range",
            "Hands",
            "Primary",
            "Secondary",
            "Fingers",
            "Fingers",
            "Chest",
            "Legs",
            "Feet",
            "Waist",
            "Ammo"
        };

		public static int[] SlotFlags = new int[] {
			1 << 0,				// Charm
			1 << 1 | 1 << 4,	// Ear1 + Ear2
			1 << 2,				// Head
			1 << 3,				// Face
			1 << 5,				// Neck
			1 << 6,				// Shoulders
			1 << 7,				// Arms
			1 << 8,				// Back
			1 << 9 | 1 << 10,	// Wrist1 + Wrist2
			1 << 11,			// Range
			1 << 12,			// Hands
			1 << 13,			// Primary
			1 << 14,			// Secondary
			1 << 15 | 1 << 16,	// Fingers1 + Fingers2
			1 << 17,			// Chest
			1 << 18,			// Legs
			1 << 19,			// Feet
			1 << 20,			// Waist
			1 << 21				// Ammo
		};

        public static string[] Languages = new string[] {
            "Common",
            "Barbarian",
            "Erudian",
            "Elvish",
            "Dark Elvish",
            "Dwarvish",
            "Troll",
            "Ogre",
            "Gnomish",
            "Halfling",
            "Thieves Cant",
            "Old Erudian",
            "Elder Elvish",
            "Froglok",
            "Goblin",
            "Gnoll",
            "Combine",
            "Elder Teir`Dal",
            "Lizardman",
            "Orcish",
            "Faerie",
            "Dragon",
            "Elder Dragon",
            "Dark Speech",
            "Vah Shir",
            "Alaran",
            "Hadal"
        };
        
        public static string[] BagTypes = new string[] {
            "None",
            "General",
            "Quiver",
            "Belt Pouch",
            "Pouch",
            "Backpack",
            "Small Box",
            "Medium Box",
            "Bandolier",
            "Medicine Bag",
            "Toolbox",
            "Research Tome",
            "Mortar and Pestle",
            "Quest Container",
            "Mixing Bowl",
            "Spit",
            "Sewing Kit",
            "Unknown (17)",
            "Fletching Kit",
            "Distillery",
            "Jeweler's Kit",
            "Pottery Wheel",
            "Kiln",
            "Unknown (23)",
            "Lexicon",
            "Grimoire",
            "Binding",
            "Tome",
            "Unknown (28)",
            "Research Tome 2",
            "Quest Container 2",
            "Tackle Box",
            "Trader's Satchel",
            "Augmentation Sealer",
            "Ice Cream Churn",
            "Ornamentation",
            "Ornamentation Stripper",
            "Unattuner",
            "Tradeskill"
        };
        
        public static string[] AugTypes = new string[] { 
            "0 (None)",
            "1 (General: Single Stat)", 
            "2 (General: Multiple Stat)", 
            "3 (General: Spell Effect)", 
            "4 (Weapon: General)", 
            "5 (Weapon: Elem Damage)", 
            "6 (Weapon: Base Damage)", 
            "7 (General: Group)", 
            "8 (General: Raid)", 
            "9 (General: Dragons Points)", 
            "10 (Crafted: Common)", 
            "11 (Crafted: Group)", 
            "12 (Crafted: Raid)", 
            "13 (Energeiac: Group)", 
            "14 (Energeiac: Raid)", 
            "15 (Emblem)", 
            "16 (Crafted: Group Symbol)", 
            "17 (Crafted: Raid Foci)", 
            "18 (Unknown)",
            "19 (Unknown)",
            "20 (Ornamentation)", 
            "21 (Special Ornamentation)",
            "22 (Unknown)",
            "23 (Unknown)",
            "24 (Unknown)",
            "25 (Unknown)",
            "26 (Unknown)",
            "27 (Unknown)",
            "28 (Unknown)",
            "29 (Unknown)",
            "30 (Invisible: Epic Upgrade)"
        };

        public static string[] AugRestrictions = new string[] {
            "None",
            "Armor Only",
            "Weapons Only",
            "One-Handed Weapons Only",
            "Two-Handed Weapons Only",
            "One-Handed Slashing Weapons Only",
            "One-Handed Blunt Weapons Only",
            "Piercing Only",
            "Hand-to-Hand Weapons Only",
            "Two-Handed Slashing Weapons Only",
            "Two-Handed Blunt Weapons Only",
            "Two-Handed Piercing Weapons Only",
            "Ranged Weapons Only",
            "Shields Only",
            "1H Slashing, 1H Blunt, or H2H Weapons Only",
            "1H Blunt or Hand-to-Hand Weapons Only",
            "Unknown (16)",
            "1H Blunt or 2H Blunt Weapons Only"
        };

        public static string[] LightTypes = new string[] {
            "None",
            "1 Lux - Candle ",
            "2 Lux - Torch ",
            "3 Lux - Blue Glow ",
            "6 Lux - Small Lantern ",
            "7 Lux - Blue Glow ",
            "8 Lux - Large Lantern ",
            "9 Lux - Flameless Lantern ",
            "9 Lux - Globe of Stars ",
            "3 Lux - Light Globe ",
            "7 Lux - Lightstone ",
            "9 Lux - Greater Lightstone ",
            "4 Lux - Red Glow ",
            "5 Lux - Blue Glow ",
            "4 Lux - Red Glow ",
            "5 Lux - Blue Glow "
        };

        public static string[] Materials = new string[] {
            "Cloth / None",
            "Leather",
            "Chain",
            "Plate",
            "Monk",
            "Unknown (5)",
            "Unknown (6)",
            "Kunark Chain",
            "Unknown (8)",
            "Unknown (9)",
            "Crimson Robe",
            "Flowing Black Robe",
            "Cryosilk Robe",
            "Robe of the Oracle",
            "Robe of the Kedge",
            "Shining Metallic Robes",
            "Plain Robe",
            "Velious Leather 1",
            "Velious Chain 1",
            "Velious Plate 1",
            "Velious Leather 2",
            "Velious Chain 2",
            "Velious Plate 2",
            "Velious Monk"
        };

        public static int[] BardTypes = new int[] { 
            23, 24, 25, 26, 50, 51
        };
        
        public static string[] ItemTypes = new string[] {
            "1H Slashing",
            "2H Slashing",
            "1H Piercing",
            "1H Blunt",
            "2H Blunt",
            "Archery - Bow", // Bow
            "Unknown (6)",
            "Throwing - Large", // Large
            "Shield",
            "Scroll",
            "Armor",
            "Inventory",
            "Lockpicks",
            "Unknown (13)",
            "Food",
            "Drink",
            "Light",
            "Combinable",
            "Bandage",
            "Throwing - Small", // Small
            "Spell",			// spells and tomes
            "Potion",
            "Unknown (22)",
            "Wind",
            "Stringed",
            "Brass",
            "Percussion",
            "Archery - Arrow", // Arrow
            "Unknown (28)",
            "Jewelry",
            "Skull",
            "Book",			// skill-up tomes/books? (would probably need a pp flag if true...)
            "Note",
            "Key",
            "Coin",
            "2H Piercing",
            "Fishing Pole",
            "Fishing Bait",
            "Alcohol",
            "Key",			// keys and satchels?? (questable keys?)
            "Compass",
            "Unknown (41)",
            "Poison",			// might be wrong, but includes poisons
            "Unknown (43)",
            "Unknown (44)",
            "Hand/Hand",
            "Unknown (46)",
            "Unknown (47)",
            "Unknown (48)",
            "Unknown (49)",
            "Singing",
            "All Instruments",
            "Charm",
            "Dye",
            "Augmentation",
            "Augmentation Solvent",
            "Augmentation Distiller",
            "Unknown (57)",
            "Banner/Fellowship Kit",
            "Unknown (59)",
            "Recipe Book",
            "Advanced Recipe",
            "Journal",		// only one(1) database entry
            "Alt Currency",	// alt-currency (as opposed to coinage)
            "Perfected Augmentation Distiller",
            "Unknown (65)",
            "Unknown (66)",
            "Unknown (67)",
            "Mount",
            "Unknown (69)"
        };

		public const int WAR = 1 << 0;
		public const int CLR = 1 << 1;
		public const int PAL = 1 << 2;
		public const int RNG = 1 << 3;
		public const int SHD = 1 << 4;
		public const int DRU = 1 << 5;
		public const int MNK = 1 << 6;
		public const int BRD = 1 << 7;
		public const int ROG = 1 << 8;
		public const int SHM = 1 << 9;
		public const int NEC = 1 << 10;
		public const int WIZ = 1 << 11;
		public const int MAG = 1 << 12;
		public const int ENC = 1 << 13;
		public const int BST = 1 << 14;
		public const int BER = 1 << 15;
		public const int ALL = 0xFFFF;
		
		public static int[] ItemTypeClasses = new int[] {
			WAR + PAL + RNG + SHD + DRU + BRD + ROG, // 1HS
            WAR + PAL + RNG + SHD + BER, // 2HS
			WAR + RNG + SHD + BRD + ROG + NEC + WIZ + MAG + ENC + BST, // Pierce
            ALL - BER, // 1HB
            ALL - BRD - ROG, // 2HB
            WAR + PAL + RNG + SHD + ROG, // Bow
            0, // Unknown (6)
            ALL - CLR - PAL - DRU - SHM, // Throwing
            WAR + CLR + PAL + RNG + SHD + DRU + BRD + ROG + SHM, // Shield
            ALL, // Scroll
            ALL, // Armor
            ALL, // Inventory
            BRD + ROG, // Lockpicks
            0, // Unknown (13)
            ALL, // Food
            ALL, // Drink
            ALL, // Light
            ALL, // Combinable
            ALL, // Bandage
            ALL - CLR - PAL - DRU - SHM, // Throwing - Small // Small
            ALL, // Spell			// spells and tomes
            ALL, // Potion
            ALL, // Unknown (22)
            BRD, // Wind
            BRD, // Stringed
            BRD, // Brass
            BRD, // Percussion
            WAR + PAL + RNG + SHD + ROG, // Archery - Arrow // Arrow
            ALL, // Unknown (28)
            ALL, // Jewelry
            ALL, // Skull
            ALL, // Book			// skill-up tomes/books? (would probably need a pp flag if true...)
            ALL, // Note
            ALL, // Key
            ALL, // Coin
            WAR + RNG + SHD + BRD + ROG + SHM + BST, // 2H Piercing
            ALL, // Fishing Pole
            ALL, // Fishing Bait
            ALL, // Alcohol
            ALL, // Key			// keys and satchels?? (questable keys?)
            ALL, // Compass
            ALL, // Unknown (41)
            ROG, // Poison			// might be wrong, but includes poisons
            ALL, // Unknown (43)
            ALL, // Unknown (44)
            MNK + BST, // Hand/Hand
            ALL, // Unknown (46)
            ALL, // Unknown (47)
            ALL, // Unknown (48)
            ALL, // Unknown (49)
            BRD, // Singing
            BRD, // All Instruments
            ALL, // Charm
            ALL, // Dye
            ALL, // Augmentation
            ALL, // Augmentation Solvent
            ALL, // Augmentation Distiller
            ALL, // Unknown (57)
            ALL, // Banner/Fellowship Kit
            ALL, // Unknown (59)
            ALL, // Recipe Book
            ALL, // Advanced Recipe
            ALL, // Journal (only one database entry)
            ALL, // Alt Currency (as opposed to coinage)
            ALL, // Perfected Augmentation Distiller
            ALL, // Unknown (65)
            ALL, // Unknown (66)
            ALL, // Unknown (67)
            ALL, // Mount
            ALL  // Unknown (69)
        };
		
		public string[] BodyTypes = new string[] {
            "None",
            "Humanoid",
            "Lycanthrope",
            "Undead",
            "Giant",
            "Construct",
            "Extraplanar",
            "Magical",
            "Summoned Undead",
            "Raid Giant",
            "Unused (10)",
            "Untargetable",
            "Vampyre",
            "Aten ha Ra",
            "Greater Akheva",
            "Khati_Sha",
            "Seru",
            "Unused (17)",
            "Draz Nurakk",
            "Zek",
            "Luggald",
            "Animal",
            "Insect",
            "Monster",
            "Elemental",
            "Plant",
            "Dragon",
            "Summoned 2",
            "Summoned Creatures",
            "Unused (29)",
            "Velious Dragon",
            "Unused (31)",
            "Dragon 3",
            "Boxes",
            "Muramite",
            "Unused (35)",
            "Unused (36)",
            "Unused (37)",
            "Unused (38)",
            "Unused (39)",
            "Unused (40)",
            "Unused (41)",
            "Unused (42)",
            "Unused (43)",
            "Unused (44)",
            "Unused (45)",
            "Unused (46)",
            "Unused (47)",
            "Unused (48)",
            "Unused (49)",
            "Unused (50)",
            "Unused (51)",
            "Unused (52)",
            "Unused (53)",
            "Unused (54)",
            "Unused (55)",
            "Unused (56)",
            "Unused (57)",
            "Unused (58)",
            "Unused (59)",
            "Untargetable2",
            "Unused (61)",
            "Unused (62)",
            "Swarm Pet",
            "Unused (64)",
            "Unused (65)",
            "Invisible Man",
            "Special Invisible"
        };

        public string[] Skills = new string[] {
            "1H Blunt",
            "1H Slash",
            "2H Blunt",
            "2H Slash",
            "Abjuration",
            "Alteration",
            "Apply Poison",
            "Archery",
            "Backstab",
            "Bind Wound",
            "Bash",
            "Block",
            "Brass",
            "Channeling",
            "Conjuration",
            "Defense",
            "Disarm",
            "Disarm Traps",
            "Divination",
            "Dodge",
            "Double Attack",
            "Dragon Punch",
            "Dual Wield",
            "Eagle Strike",
            "Evocation",
            "Feign Death",
            "Flying Kick",
            "Forage",
            "Hand to Hand",
            "Hide",
            "Kick",
            "Meditate",
            "Mend",
            "Offense",
            "Parry",
            "Pick Locks",
            "Pierce",
            "Riposte",
            "Round Kick",
            "Safe Fall",
            "Sense Heading",
            "Singing",
            "Sneak",
            "Specialize Abjuration",
            "Specialize Alteration",
            "Specialize Conjuration",
            "Specialize Divination",
            "Specialize Evocation",
            "Pick Pockets",
            "Strings",
            "Swimming",
            "Throwing",
            "Tiger Claw",
            "Tracking",
            "Winds",
            "Fishing",
            "Make Poison",
            "Tinkering",
            "Research",
            "Alchemy",
            "Baking",
            "Tailoring",
            "Sense Traps",
            "Blacksmithing",
            "Fletching",
            "Brewing",
            "Alchemy",
            "Begging",
            "Jewelcrafting",
            "Pottery",
            "Percussion",
            "Intimidate",
            "Berzerk",
            "Taunt",
            "Frenzy"
        };

		// UF+ Only:
		// 11208 pitcher
		// 11486 a bowl
		// 11487 A FLAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAME!!!!!!!!!!!!!!!!!!!!!!!!!!
		// 11488 smoke
		// 11489 blue smoke
		// 11503 yellow glory smoke
		// 11504 red globe and glory smoke
		// 11505 blue globe and glory smoke
		// 11506 orange/firey globe and glory smoke
		// 11507 green globe and glory smoke
		// 11508 purple globe and glory smoke
		// 11509 yellow ball of lightning
		// 11510 red ball of lightning
		// 11 blue, 12 orange, 13 green, 14 purple
		// 11515 mud splashing
		// 11516 white water splashes
		// 11517 water pouring
		// 11518 black globe and glory smoke
		// 11519 poison green globe and glory smoke
		// 11526 toolbox
		// 11527 hairbrush / horse brush
		// 11528 chest of rocks
		// 11529 cement brick
		// 11540 student's table
		// 11541 chemist's table
		// 11542 fletcher's table
		// 11543 jeweler's table
		// 11544 tinker's table, 45 fisher, 46 = bowl, 47 = ice cream churn, 

		public Dictionary<int, int> IconModels = new Dictionary<int, int>()
		{
			// Blunts
			{ 567, 24 },
			{ 578, 37 },
			{ 581, 33 },
			{ 601, 10938 },
			{ 602, 126 },
			{ 686, 75 },
			{ 737, 18 },
			{ 738, 31 },
			{ 741, 19 },
			{ 809, 46 },	
			{ 810, 47 },
			{ 811, 45 },
			{ 821, 35 },
			{ 822, 51 },
			{ 887, 29 },
			{ 889, 32 },
			{ 891, 52 },
			{ 903, 49 },
			{ 973, 72 },
			{ 1083, 75 },
			{ 1117, 178 },
			{ 1172, 96 },
			{ 1175, 111 },
			{ 1187, 103 },
			{ 1188, 102 },
			{ 1189, 101 },
			{ 1194, 112 },
			{ 1216, 130 },
			{ 1249, 134 },
			{ 1262, 185 },
			{ 1263, 177 },
			{ 1265, 169 },
			{ 1268, 10932 },
			{ 1274, 10406 },
			{ 1275, 10407 },
			{ 1277, 10410 },
			{ 1279, 10501 },
			{ 1280, 10502 },
			{ 1281, 10503 },
			{ 1282, 10663 }, // 10504 = blue sparkles. 10663 = purple.
			{ 1283, 10505 },
			{ 1288, 10517 },
			{ 1289, 10518 },
			{ 1290, 10519 },
			{ 1291, 10520 },
			{ 1292, 10521 },
			{ 1293, 10522 },
			{ 1294, 10523 },
			{ 1295, 10524 },
			{ 1296, 10525 },
			{ 1320, 10604 },
			{ 1321, 10605 },
			{ 1322, 10606 },
			{ 1323, 10607 },
			{ 1324, 10608 },
			{ 1325, 10609 },
			{ 1347, 10634 },
			{ 1348, 10635 },
			{ 1349, 10636 },
			{ 1350, 10637 },
			{ 1351, 10638 },
			{ 1352, 10639 },
			{ 1354, 10642 },
			{ 1355, 10643 },
			{ 1359, 10647 },
			{ 1376, 10506 },
			{ 1377, 10507 },
			{ 1378, 10508 },
			{ 1391, 10672 },
			{ 1393, 10675 },
			{ 1395, 10677 }, // Another standing torch!
			{ 1399, 10681 },
			{ 1401, 10683 },
			{ 1405, 10688 },
			{ 1412, 10695 },
			{ 1421, 10706 },
			{ 1422, 10707 },
			{ 1428, 10713 },
			{ 1447, 10716 },
			{ 1449, 10718 },
			{ 1459, 10731 },
			{ 1460, 10732 },
			{ 1508, 10736 }, // 10752 = No Particles
			{ 1511, 10739 }, // 10755 = No Particles
			{ 1512, 10740 }, // 10756 = No Particles
			{ 1513, 10741 }, // 10757 = No Particles
			{ 1515, 10743 }, // 10759 = No Particles
			{ 1520, 10748 }, // 10764 = No Particles
			{ 1522, 10750 }, // 10766 = No Particles
			{ 1523, 10767 },
			{ 1524, 10768 },
			{ 1530, 10774 },
			{ 1532, 10777 },
			{ 1540, 10783 },
			{ 1544, 10788 },
			{ 1550, 10794 },
			{ 1551, 10795 },
			{ 1552, 10796 },
			{ 1661, 10817 },
			{ 1662, 10818 },
			{ 1663, 10819 },
			{ 1664, 10820 },
			{ 1666, 10822 },
			{ 1667, 10823 },
			{ 1673, 10829 },
			{ 1674, 10830 },
			{ 1678, 10834 },
			{ 1679, 10835 },
			{ 1680, 10836 },
			{ 1681, 10837 },
			{ 1682, 10838 },
			{ 1683, 10839 },
			{ 1701, 10843 },
			{ 1703, 10845 },
			{ 1704, 10846 },
			{ 1705, 10847 },
			{ 1709, 10851 },
			{ 1710, 10852 },
			{ 1711, 10853 },
			{ 1712, 10854 },
			{ 1717, 10859 },
			{ 1782, 10924 },
			{ 1783, 10925 },
			{ 1784, 10926 },
			{ 1785, 10927 },
			{ 1786, 10928 },
			{ 1787, 10929 },
			{ 1788, 10930 },
			{ 1789, 10931 },
			{ 1790, 10932 },
			{ 1791, 10933 },
			{ 1792, 10934 },
			{ 1793, 10935 },
			{ 1794, 10936 },
			{ 1795, 10937 },
			{ 1796, 10938 },
			{ 1797, 10939 },
			{ 1798, 10940 },
			{ 1799, 10941 },
			{ 1800, 10942 },
			{ 1801, 10943 },
			{ 1802, 10944 },
			{ 1803, 10945 },
			{ 1804, 10946 },
			{ 1805, 10947 },
			{ 1806, 10948 },
			{ 1807, 10949 },
			{ 1858, 11031 },
			{ 1859, 11032 },
			{ 1860, 11033 },
			{ 1861, 11034 },
			{ 1862, 11035 },
			{ 1871, 11044 },
			{ 1872, 11045 },
			{ 1879, 11052 },
			{ 2154, 11087 },
			{ 2164, 11097 },
			{ 2165, 11098 },
			{ 2171, 11104 },
			{ 2172, 11105 },
			{ 2173, 11106 },
			{ 2174, 11107 },
			{ 2184, 11117 },
			{ 2186, 11119 },
			{ 2229, 11130 },
			{ 2232, 11133 },

			// Books
			{  504,    28 },
			{  682,    65 },
			{  683,    28 },
			{  777,    27 },
			{  778,    27 },	
			{  789,    27 },	
			{  860,    28 },	
			{  861,    65 },	
			{  862,    28 },	
			{  863,    65 },	
			{  865,    27 },	
			{ 1357, 10645 },	
			{ 1358, 10646 },	
			{ 1485,   400 },	
			{ 1497,    27 },	
			{ 2017, 11059 },	
			{ 2031, 11073 },

			// Bows & Arrows

			{  597, 10412 },
			{  598,    10 },
			{  681, 10725 },
			{  725,    64 },
			{  726,    64 },
			{ 1024, 10300 },
			{ 1039,    10 },
			{ 1104, 10300 },
			{ 1254,   199 },
			{ 1330, 10614 },
			{ 1379, 10641 },
			{ 1448, 10717 },
			{ 1453, 10725 },
			{ 1545, 10789 },
			{ 1628, 10641 },
			{ 1629, 10717 },
			{ 1630, 10789 },
			{ 1855, 10997 },
			{ 1856, 10998 },

			// Containers

			{  557,    64 },
			{  565,    64 },
			{  608, 11054 },
			{  609, 11054 },
			{  689,    64 },
			{  690,    64 },
			{  691,    64 },
			{  723, 11054 },
			{  724, 11054 },
			{  730, 11054 },
			{  836, 11054 },
			{  837, 11054 },
			{  883, 11054 },
			{  884,    64 },
			{  892, 10802 },
			{  979,    64 },
			{ 1016,    64 },
			{ 1017,    64 },
			{ 1112, 10800 },
			{ 1113, 10801 },
			{ 1114, 10803 },
			{ 1115, 10804 },
			{ 1116, 10805 },
			{ 1142,    64 },
			{ 1144,    64 },
			{ 1239,    64 },
			{ 1444,    64 },
			{ 1921,    64 },
			{ 1922,    64 },
			{ 1923,    64 },
			{ 1924,    64 },
			{ 1925,    64 },
			{ 1926,    64 },
			{ 1927,    64 },
			{ 1928,    64 },
			{ 1929,    64 },
			{ 1930,    64 },
			{ 1931,    64 },
			{ 1932,    64 },
			{ 1933,    64 },
			{ 1934,    64 },
			{ 1935,    64 },
			{ 1936,    64 },
			{ 1937,    64 },
			{ 1938,    64 },
			{ 2010, 11063 },
			{ 2011, 11062 },
			{ 2012, 11054 },
			{ 2020, 11062 },
			{ 2021, 11063 },

			// Drink

			{  692, 10808 },
			{  708,    56 },
			{  709,    56 },
			{  710, 10861 },
			{  711, 10861 },
			{  829, 10861 },
			{ 1557, 10806 },
			{ 1558, 10807 },
			{ 1559, 10808 },
			{ 1719, 10861 },
			{ 1939, 10861 },

			// Food
			{  673, 11068 },
			{  784, 11068 },
			{  922, 11070 },
			{ 1000, 11068 },
			{ 1021, 11068 },
			{ 1086, 11068 },
			{ 1105, 11068 },
			{ 1108, 11068 },
			{ 1111, 11068 },
			{ 1688,   250 },
			{ 1691, 11071 },
			{ 1693, 11055 },
			{ 1696, 11071 },
			{ 2006, 11069 },
			{ 2008, 11069 },
			{ 2027, 11069 },
			{ 2028, 11070 },
			{ 2029, 11071 },

			// Gear - Hands
			{  971,    68 },
			{ 1878, 11051 },

			// Gear - Head
			{  511,  5003 },
			{  523,  5003 },
			{  536,  5808 },
			{  550,   625 },
			{  625,  5002 },
			{  628,  5328 },
			{  629,  5423 },
			{  639,  5003 },
			{  640,  5361 },
			{  641,  5301 },
			{  642,  5151 },
			{  664,  5480 },
			{  745,  5538 },
			{  746,  5148 },
			{  747,  5538 },
			{ 1608,  5838 },
			{ 1609,  5001 },
			{ 1610,  5361 },
			{ 1611,  5361 },
			{ 1612,  5032 },
			{ 1613,  5031 },
			{ 1614,  5032 },
			{ 1615,  5838 },
			{ 2113,  5031 },
			{ 2114,  5781 },
			{ 2115,  5151 },
			{ 2116,  5003 },

			// Gear - Chest
			{  527,  6300 },
			{  538,  6300 },
			{  621,  6450 },
			{  624,  6000 },
			{  632,    64 },
			{  678,    64 },
			{  712,  4366 },
			{  713,  4366 },
			{  714,    64 },
			{  838,  4363 },
			{  928,  4365 },
			{  929,  4365 },
			{  930,  4360 },
			{  931,  4360 },
			{  940,  4364 },
			{  941,  4362 },
			{  942,  4362 },
			{ 1126,  4361 },
			{ 1149,    64 },
			{ 1568,  6660 },
			{ 1569,  6060 },
			{ 1570,  6210 },
			{ 1571,  6540 },
			{ 1572,  6180 },
			{ 1573,  6060 },
			{ 1574,    64 },
			{ 1575,  4363 },
			{ 2105,  4363 },
			{ 2106,  4366 },
			{ 2107,  4362 },
			{ 2108,  4362 },
			{ 2109,  6300 },
			{ 2110,  6420 },
			{ 2111,  6060 },
			{ 2112,  6300 },

			// Gear - Waist
			{  843,  4562 },
			{ 1119,  4560 },
			{ 1120,  4372 },
			{ 1150,  4710 },

			// Gear - Arms
			{  543,  8036 },
			{  546,  8007 },
			{  622,  8246 },
			{  623,  8187 },
			{  634,  7067 },
			{  669,  8186 },
			{  670,  8577 },
			{ 1592,  9666 },
			{ 1593,  9666 },
			{ 1594,  9666 },
			{ 1595,  9666 },
			{ 1596,  9666 },
			{ 1597,  9666 },
			{ 1598,  9666 },
			{ 1599,  9666 },
			{ 2117,  9547 },
			{ 2118,  9097 },
			{ 2119,  7067 },
			{ 2120,  9547 },

			// Gear - Shoulders
			{  798,  7397 },

			// Gear - Wrists
			{  516,  8097 },
			{  520,  8097 },
			{  521,  8247 },
			{  620,  8607 },
			{  637,  8757 },
			{  638,  9367 },
			{  671,  8037 },
			{ 1040,  8097 },
			{ 1044,  8157 },
			{ 1055,  8097 },
			{ 1057,  8037 },
			{ 1235,  8007 },
			{ 1584,  8067 },
			{ 1585,  8757 },
			{ 1586,  8727 },
			{ 1587,  8187 },
			{ 1588,  8157 },
			{ 1589,  8727 },
			{ 1590,  8667 },
			{ 1591,  8757 },
			{ 2121,  8187 },
			{ 2122,  9217 },
			{ 2123,  8067 },
			{ 2124,  8247 },

			// Gemstones
			{  507, 10611 },
			{  767, 11167 },
			{  944, 11163 },
			{  945, 11165 },
			{  946, 11163 },
			{  947, 11167 },
			{  948, 11163 },
			{  949, 11164 },
			{  950, 11164 },
			{  951, 11164 },
			{  952, 11163 },
			{  953, 11164 },
			{  954, 11165 },
			{  955, 11124 },
			{  956, 11167 },
			{  957, 11163 },
			{  958, 11163 },
			{  959, 11164 },
			{  960, 11164 },
			{  961, 11164 },
			{  962, 11163 },
			{  963, 11165 },
			{  964, 11164 },
			{  965, 11165 },
			{  966, 11168 },
			{  967, 11057 },
			{  968, 11167 },
			{  969, 11068 },
			{ 1253, 11168 },
			{ 1429, 11163 },
			{ 1430, 11165 },
			{ 1431, 10724 },
			{ 1432, 11165 },
			{ 1433, 11163 },
			{ 1434, 11162 },
			{ 1435, 11163 },
			{ 1436, 11164 },
			{ 1437, 11163 },
			{ 1438, 11068 },
			{ 1439, 10724 },
			{ 1440, 11163 },
			{ 1441, 11165 },
			{ 1442, 10724 },
			{ 1443, 11068 },
			{ 1452, 11068 },
			{ 1462, 11057 },
			{ 1463, 11057 },
			{ 1464, 11057 },
			{ 1465, 11057 },
			{ 1466, 11057 },
			{ 1467, 11057 },
			{ 1468, 11169 },
			{ 1469, 11057 },
			{ 1476, 11124 },
			{ 1477, 11165 },
			{ 1479, 10724 },
			{ 1480, 10724 },
			{ 1484, 11165 },
			{ 1486, 11165 },
			{ 1502, 11162 },
			{ 1503, 11163 },
			{ 1504, 11162 },
			{ 1505, 11123 },
			{ 1506, 11163 },
			{ 1535, 11165 },
			{ 1536, 11166 },
			{ 1940, 11164 },
			{ 1941, 11164 },
			{ 1942, 11164 },
			{ 1943, 11164 },
			{ 1944, 11164 },
			{ 1945, 11164 },
			{ 1946, 11124 },
			{ 1947, 11124 },
			{ 1948, 11124 },
			{ 1949, 11124 },
			{ 1950, 11124 },
			{ 1951, 11124 },
			{ 1952, 11166 },
			{ 1953, 11166 },
			{ 1954, 11166 },
			{ 1955, 11166 },
			{ 1956, 11166 },
			{ 1957, 11166 },
			{ 1958, 11163 },
			{ 1959, 11163 },
			{ 1960, 11163 },
			{ 1961, 11163 },
			{ 1962, 11163 },
			{ 1963, 11163 },
			{ 1964, 11165 },
			{ 1965, 11165 },
			{ 1966, 11165 },
			{ 1967, 11165 },
			{ 1968, 11165 },
			{ 1969, 11165 },
			{ 1970, 11165 },
			{ 1971, 11165 },
			{ 1972, 11165 },
			{ 1973, 11165 },
			{ 1974, 11165 },
			{ 1975, 11165 },
			{ 1976, 11162 },
			{ 1977, 11162 },
			{ 1978, 11162 },
			{ 1979, 11162 },
			{ 1980, 11162 },
			{ 1981, 11162 },
			{ 1982, 11168 },
			{ 1983, 11168 },
			{ 1984, 11168 },
			{ 1985, 11168 },
			{ 1986, 11168 },
			{ 1987, 11168 },
			{ 1988, 11167 },
			{ 1989, 11167 },
			{ 1990, 11167 },
			{ 1991, 11167 },
			{ 1992, 11167 },
			{ 1993, 11167 },
			{ 1994, 11164 },
			{ 1995, 11124 },
			{ 1996, 11166 },
			{ 1997, 11163 },
			{ 1998, 11165 },
			{ 1999, 11165 },
			{ 2000, 11162 },
			{ 2001, 11169 },
			{ 2002, 11168 },
			{ 2085, 11123 },
			{ 2086, 11124 },
			{ 2087, 11167 },
			{ 2088, 11165 },
			{ 2089, 11166 },
			{ 2090, 11162 },
			{ 2091, 11163 },
			{ 2092, 11165 },
			{ 2093, 11124 },
			{ 2094, 11166 },
			{ 2095, 11168 },
			{ 2096, 11167 },
			{ 2097, 11166 },
			{ 2098, 11167 },
			{ 2099, 11163 },
			{ 2100, 11163 },
			{ 2101, 11162 },
			{ 2102, 11168 },
			{ 2103, 11166 },
			{ 2104, 11169 },
			{ 2190, 11123 },
			{ 2191, 11124 },
			{ 2233, 11154 },
			{ 2234, 11155 },
			{ 2235, 11156 },
			{ 2236, 11157 },
			{ 2237, 11158 },
			{ 2238, 11159 },
			{ 2239, 11160 },
			{ 2240, 11161 },
			{ 2241, 11162 },
			{ 2242, 11163 },
			{ 2243, 11164 },
			{ 2244, 11165 },
			{ 2245, 11166 },
			{ 2246, 11167 },
			{ 2247, 11168 },
			{ 2248, 11169 },

			// Instruments

			{  551, 10603 },
			{  593,    15 },
			{  594, 10600 },
			{  751, 10601 },
			{ 1152, 10602 },
			{ 1317, 10601 },
			{ 1318, 11501 },
			{ 1319, 11502 },

			// Parts
			{  552,    64 },
			{  553,    64 },
			{  554,    64 },
			{  555,    64 },
			{  556,    64 },
			{  680,    64 },
			{  743,    71 },
			{  744,    64 },
			{  774, 11118 },
			{  775, 11118 },
			{  786, 11118 },
			{  787, 11118 },
			{  794,    71 },
			{  797, 11069 },
			{  799, 11118 },
			{  800,    15 },
			{  801,    15 },
			{  804, 11058 },
			{  807,    15 },
			{  814,    64 },
			{  817,    64 },
			{  818,    64 },
			{  819, 11069 },
			{  820,    64 },
			{  823,   224 },
			{  833,    64 },
			{  834,    64 },
			{  835,    64 },
			{  853,    71 },
			{  859, 11069 },
			{  871, 10611 },
			{  885,   402 },
			{  905, 10695 },
			{  906, 10695 },
			{  907, 10695 },
			{  917, 11070 },
			{  918,    64 },
			{  919,    64 },
			{  920,    64 },
			{  927,   220 },
			{  943,   224 },
			{  972, 10611 },
			{  980,    64 },
			{  981,    64 },
			{  982,    64 },
			{  983,    64 },
			{  984,    64 },
			{  985,    64 },
			{  986,    64 },
			{  987,    64 },
			{  988,    64 },
			{  989,    64 },
			{  990,    64 },
			{  991,    64 },
			{  992,    64 },
			{  993,    64 },
			{  994,    64 },
			{  995,    64 },
			{  996,    64 },
			{  997,    64 },
			{ 1001,    64 },
			{ 1003, 11069 },
			{ 1026, 10724 },
			{ 1067,    71 },
			{ 1069,    64 },
			{ 1070,    64 },
			{ 1088, 11069 },
			{ 1089,    71 },
			{ 1094, 11058 },
			{ 1125,    94 },
			{ 1130, 11069 },
			{ 1131, 11069 },
			{ 1136, 10611 },
			{ 1137, 10695 },
			{ 1139,   220 },
			{ 1203,    15 },
			{ 1204,    64 },
			{ 1205,    64 },
			{ 1207, 11118 },
			{ 1208,    64 },
			{ 1209,    64 },
			{ 1219,    15 },
			{ 1220,   217 },
			{ 1221,    64 },
			{ 1222,    64 },
			{ 1223,   220 },
			{ 1224,   220 },
			{ 1225,   224 },
			{ 1229,    15 },
			{ 1231,    64 },
			{ 1232, 11069 },
			{ 1233,   217 },
			{ 1234,   224 },
			{ 1236, 11069 },
			{ 1241,    15 },
			{ 1242,   220 },
			{ 1243,   217 },
			{ 1251,    64 },
			{ 1252,    64 },
			{ 1257,    64 },
			{ 1258,    64 },
			{ 1259,    64 },
			{ 1260,    64 },
			{ 1261,    64 },
			{ 1445,    64 },
			{ 1470,    64 },
			{ 1471,    64 },
			{ 1472,    64 },
			{ 1473,    64 },
			{ 1474,    64 },
			{ 1475,    64 },
			{ 1487,    64 },
			{ 1488,    64 },
			{ 1489,    64 },
			{ 1490,    64 },
			{ 1491,    64 },
			{ 1492,    64 },
			{ 1493,    64 },
			{ 1494,    64 },
			{ 1495,    64 },
			{ 1498, 10695 },
			{ 1499, 10724 },
			{ 1500, 10695 },
			{ 1501, 10695 },
			{ 1633,    64 },
			{ 1635,    71 },
			{ 1637, 10724 },
			{ 1723,    64 },
			{ 1904,    64 },
			{ 1905,    64 },
			{ 1906,    64 },
			{ 1907,    64 },
			{ 1908,    64 },
			{ 1909,    64 },
			{ 1910,    64 },
			{ 1911,    64 },
			{ 1912,    64 },
			{ 1913,    64 },
			{ 2016, 11058 },
			{ 2137, 11058 },
			{ 2138, 10695 },
			{ 2139, 10695 },
			{ 2150,    64 },
			{ 2151,    64 },
			{ 2185, 11118 },

			// Pierce
			{  574, 10633 },
			{  591, 10650 },
			{  592, 10006 },
			{  736,    23 },
			{  740, 10100 },
			{  742,    30 },
			{  762, 10680 },
			{  763, 10680 },
			{  768, 10686 },
			{  776, 10100 },
			{  888, 10686 },
			{ 1163,    79 },
			{ 1179,    94 },
			{ 1182,    98 },
			{ 1183,    99 },
			{ 1214,   118 },
			{ 1266,   176 },
			{ 1267,   167 },
			{ 1270, 10028 },
			{ 1298, 10527 },
			{ 1299, 10528 },
			{ 1328, 10612 },
			{ 1329, 10613 },
			{ 1334, 10618 },
			{ 1337, 10621 },
			{ 1340, 10627 },
			{ 1343, 10630 },
			{ 1346, 10633 },
			{ 1356, 10644 },
			{ 1362, 10650 },
			{ 1365, 10650 },
			{ 1368, 10656 },
			{ 1371, 10659 },
			{ 1374, 10662 },
			{ 1382, 10624 },
			{ 1397, 10679 },
			{ 1398, 10680 },
			{ 1403, 10686 },
			{ 1404, 10687 },
			{ 1406, 10689 },
			{ 1407, 10690 },
			{ 1409, 10692 },
			{ 1410, 10693 },
			{ 1416, 10701 },
			{ 1418, 10703 },
			{ 1424, 10709 },
			{ 1426, 10711 },
			{ 1427, 10712 },
			{ 1451, 10722 },
			{ 1461, 10680 },
			{ 1507, 10735 }, // 10751 - Without particle effect
			{ 1518, 10746 }, // 10762 - Without particle effect
			{ 1525, 10769 },
			{ 1529, 10773 },
			{ 1539, 10782 },
			{ 1549, 10793 },
			{ 1554, 10798 },
			{ 1555, 10799 },
			{ 1668, 10824 },
			{ 1672, 10828 },
			{ 1675, 10831 },
			{ 1686, 10842 },
			{ 1702, 10844 },
			{ 1724, 10866 },
			{ 1726, 10868 },
			{ 1728, 10870 },
			{ 1740, 10882 },
			{ 1761, 10903 },
			{ 1768, 10910 },
			{ 1769, 10911 },
			{ 1770, 10912 },
			{ 1771, 10913 },
			{ 1772, 10914 },
			{ 1773, 10915 },
			{ 1774, 10916 },
			{ 1775, 10917 },
			{ 1776, 10918 },
			{ 1777, 10919 },
			{ 1808, 10950 },
			{ 1809, 10951 },
			{ 1810, 10952 },
			{ 1811, 10953 },
			{ 1812, 10954 },
			{ 1813, 10955 },
			{ 1814, 10956 },
			{ 1815, 10957 },
			{ 1816, 10958 },
			{ 1868, 11041 },
			{ 2155, 11088 },
			{ 2156, 11089 },
			{ 2158, 11091 },
			{ 2159, 11092 },
			{ 2160, 11093 },
			{ 2166, 11099 },
			{ 2168, 11101 }, // PIERCE STARTS
			{ 2179, 11112 },
			{ 2181, 11114 },
			{ 2183, 11116 },
			{ 2230, 11131 },

			// Plants
			{  721, 11060 },
			{  795, 11075 },
			{  796,   403 },
			{  854, 11081 },
			{  911, 10000 },
			{ 1073, 11060 },
			{ 1133,   403 },
			{ 1196,   403 },
			{ 1197, 11060 },
			{ 1198, 11060 },
			{ 1199, 11079 },
			{ 1200, 11075 },
			{ 1201, 11056 },
			{ 1202,   403 },
			{ 1206, 11081 },
			{ 1636, 11060 },
			{ 2014, 11056 },
			{ 2018, 11060 },
			{ 2019, 11061 },
			{ 2024, 11066 },
			{ 2025, 11067 },
			{ 2032, 11075 },
			{ 2033, 11075 },
			{ 2034, 11076 },
			{ 2035, 11077 },
			{ 2036, 11078 },
			{ 2037, 11079 },
			{ 2038, 11080 },
			{ 2039, 11081 },
			{ 2040, 11082 },
			{ 2041, 11083 },
			{ 2042, 11084 },
			{ 2192, 11060 },
			{ 2193, 11060 },
			{ 2194, 11079 },
			{ 2195, 11079 },
			{ 2196, 11060 },
			{ 2197, 11060 },
			{ 2198, 11079 },
			{ 2199, 11079 },
			{ 2200, 11060 },
			{ 2201, 11060 },
			{ 2202, 11079 },
			{ 2203, 11079 },
			{ 2204, 11060 },
			{ 2205, 11060 },
			{ 2206, 11080 },
			{ 2207, 11079 },
			{ 2208, 11060 },
			{ 2209, 11060 },
			{ 2210, 11080 },
			{ 2211, 11079 },
			{ 2212, 11060 },
			{ 2213, 11060 },
			{ 2214, 11079 },
			{ 2215, 11079 },
			{ 2216, 11060 },
			{ 2217, 11060 },
			{ 2218, 11079 },
			{ 2219, 11079 },
			{ 2220, 11060 },
			{ 2221, 11060 },
			{ 2222, 11080 },
			{ 2223, 11080 },
			{ 2224, 11074 },
			{ 2225, 11074 },
			{ 2226, 11077 },
			{ 2227, 11077 },

			// Rings
			{  505, 10512 },
			{  508, 10512 },
			{  509, 10510 },
			{  512, 10512 },
			{  513, 10511 },
			{  515, 10512 },
			{  607, 10511 },
			{  612, 10509 },
			{  613, 10512 },
			{  614, 10511 },
			{  615, 10512 },
			{  616, 10512 },
			{  617,    68 },
			{  674, 10512 },
			{  675, 10512 },
			{  748,    68 },
			{  765,    68 },
			{  872, 10512 },
			{  873, 10512 },
			{  874, 10512 },
			{  875, 10512 },
			{  876, 10512 },
			{  877, 10512 },
			{  878,    68 },
			{  879, 10512 },
			{  880, 10511 },
			{ 1041, 10510 },
			{ 1045, 10510 },
			{ 1047, 10512 },
			{ 1051,    68 },
			{ 1052, 10509 },
			{ 1059, 10512 },
			{ 1060, 10512 },
			{ 1061, 10512 },
			{ 1064, 10512 },
			{ 1071, 10512 },
			{ 1072, 10510 },
			{ 1148, 10512 },
			{ 1255, 10511 },
			{ 1616, 10511 },
			{ 1617,    68 },
			{ 1618, 10512 },
			{ 1619, 10512 },
			{ 1620, 10512 },
			{ 1621,    68 },
			{ 1622, 10509 },
			{ 1623, 10511 },
			{ 1624, 10510 },
			{ 1645, 10512 },
			{ 1721,    68 },
			{ 1722, 10509 },
			{ 1886, 10509 },
			{ 1887, 10512 },
			{ 1888, 10512 },
			{ 1889, 10512 },
			{ 1890, 10512 },
			{ 1891, 10512 },
			{ 1892, 10512 },
			{ 1893, 10510 },

			// Shields
			{  542,   203 },
			{  606,   211 },
			{  676, 11144 },
			{  758,   204 },
			{  759,   209 },
			{  760,   208 },
			{  805,   202 },
			{  970,   214 },
			{  974,   212 },
			{  976,   213 },
			{ 1244,   218 },
			{ 1245,   217 },
			{ 1246,   220 },
			{ 1300, 10530 },
			{ 1301, 10531 },
			{ 1302, 10532 },
			{ 1303, 10533 },
			{ 1304, 10534 },
			{ 1305, 10535 },
			{ 1306, 10536 },
			{ 1307, 10537 },
			{ 1308, 10538 },
			{ 1309, 10539 },
			{ 1310, 10540 },
			{ 1311, 10541 },
			{ 1312, 10542 },
			{ 1313, 10543 },
			{ 1314, 10534 },
			{ 1327,   210 },
			{ 1375, 11002 },
			{ 1383, 10664 },
			{ 1384, 10665 },
			{ 1387, 10668 },
			{ 1388, 10669 },
			{ 1389, 10670 },
			{ 1390, 10671 },
			{ 1408, 10691 },
			{ 1457, 10729 },
			{ 1458, 10730 },
			{ 1496, 67367 },
			{ 1510, 10738 },
			{ 1528, 10772 },
			{ 1531, 10775 },
			{ 1538, 10781 },
			{ 1546, 10790 },
			{ 1670, 10826 },
			{ 1671, 10827 },
			{ 1676, 10832 },
			{ 1677, 10833 },
			{ 1707, 10849 },
			{ 1708, 10850 },
			{ 1715, 10857 },
			{ 1716, 10858 },
			{ 1818, 10960 },
			{ 1819, 10961 },
			{ 1820, 10962 },
			{ 1821, 10963 },
			{ 1822, 10964 },
			{ 1823, 10965 },
			{ 1824, 10966 },
			{ 1825, 10967 },
			{ 1826, 10968 },
			{ 1827, 10969 },
			{ 1828, 10970 },
			{ 1829, 10971 },
			{ 1830, 10972 },
			{ 1831, 10973 },
			{ 1832, 10974 },
			{ 1833, 10975 },
			{ 1834, 10976 },
			{ 1835, 10977 },
			{ 1836, 10978 },
			{ 1837, 10979 },
			{ 1838, 10980 },
			{ 1839, 10981 },
			{ 1840, 10982 },
			{ 1841, 10983 },
			{ 1842, 10984 },
			{ 1843, 10985 },
			{ 1844, 10986 },
			{ 1845, 10987 },
			{ 1846, 10988 },
			{ 1847, 10989 },
			{ 1848, 10990 },
			{ 1849, 10991 },
			{ 1850, 10992 },
			{ 1851, 10993 },
			{ 1852, 10994 },
			{ 1853, 10995 },
			{ 1854, 10996 },
			{ 1875, 11048 },
			{ 1876, 11049 },
			{ 2152, 11085 },
			{ 2153, 11086 },
			{ 2169, 11103 },
			{ 2170, 11102 },
			{ 2177, 11110 },
			{ 2178, 11111 },

			// Slashing
			{  519,   161 },
			{  568,    25 },
			{  569, 10005 },
			{  573,    60 },
			{  575,    53 },
			{  576,    11 },
			{  577,    58 },
			{  579,    39 },
			{  580, 10026 },
			{  588, 10023 },
			{  589,   180 },
			{  590,   168 },
			{  596,    61 },
			{  603, 10004 },
			{  604,    41 },
			{  605,    42 },
			{  781,    40 },
			{  847,    62 }, // Red blade?? Currently Fiery Avenger 2HS (ooooold)
			{  882,   190 },
			{  890,   175 },
			{  902, 10010 }, // Use better if can.
			{  975,    71 },
			{ 1164,    80 },
			{ 1165,    81 },
			{ 1166,    83 },
			{ 1167,    84 },
			{ 1168,    85 },
			{ 1169,    86 },
			{ 1170,    87 },
			{ 1171,    95 },
			{ 1173,    97 },
			{ 1174,    82 },
			{ 1176,    88 },
			{ 1177,    76 },
			{ 1178,    90 },
			{ 1180,    91 },
			{ 1181,    92 },
			{ 1184,   104 },
			{ 1185,   105 },
			{ 1186,   100 },
			{ 1190,   106 },
			{ 1191,   108 },
			{ 1192,   113 },
			{ 1193,   107 },
			{ 1195,   109 },
			{ 1211,   110 },
			{ 1212,    93 },
			{ 1215,   131 },
			{ 1247,   135 },
			{ 1264,   182 },
			{ 1269,    80 },
			{ 1271,   181 },
			{ 1272,   160 }, // Or 62.
			{ 1273,    60 },
			{ 1284, 10513 },
			{ 1285, 10514 },
			{ 1286, 10515 },
			{ 1287, 10516 },
			{ 1297, 10526 },
			{ 1315, 10545 }, // 184 is greenish elvish.
			{ 1326, 10610 },
			{ 1331, 10615 },
			{ 1332, 10616 },
			{ 1333, 10617 },
			{ 1335, 10619 },
			{ 1336, 10620 },
			{ 1338, 10625 }, 
			{ 1339, 10626 },
			{ 1341, 10628 },
			{ 1342, 10629 },
			{ 1344, 10631 },
			{ 1345, 10632 },
			{ 1353, 10640 },
			{ 1360, 10648 },
			{ 1361, 10649 },
			{ 1363, 10651 },
			{ 1364, 10652 },
			{ 1366, 10654 },
			{ 1367, 10655 },
			{ 1369, 10657 },
			{ 1370, 10658 },
			{ 1372, 10660 },
			{ 1373, 10661 },
			{ 1380, 10622 },
			{ 1381, 10623 },
			{ 1392, 10674 },
			{ 1394, 10676 },
			{ 1396, 10678 },
			{ 1400, 10682 },
			{ 1402, 10685 },
			{ 1411, 10694 },
			{ 1413, 10696 },
			{ 1415, 10700 },
			{ 1417, 10702 },
			{ 1419, 10704 },
			{ 1420, 10705 },
			{ 1423, 10708 },
			{ 1425, 10710 },
			{ 1446, 10715 },
			{ 1450, 10719 },
			{ 1454, 10726 },
			{ 1455, 10727 },
			{ 1456, 10728 },
			{ 1509, 10737 }, // 10753 w/o particles
			{ 1516, 10744 }, // 10760 w/o particles
			{ 1517, 10745 }, // 10761 w/o particles
			{ 1519, 10747 }, // 10763 w/o particles
			{ 1521, 10749 }, // 10765 w/o particles
			{ 1526, 10770 },
			{ 1527, 10771 },
			{ 1533,    61 }, // Whip. May have been swapped to 10778 rapier w/ no icon.
			{ 1534, 10776 },
			{ 1537, 10780 },
			{ 1541, 10784 },
			{ 1542, 10785 },
			{ 1543, 10786 },
			{ 1547, 10791 },
			{ 1548, 10792 },
			{ 1553, 10797 },
			{ 1631, 10904 }, // 1H Axe. Must have been scrapped.
			{ 1632, 10905 }, // 2H Axe. Must have been scrapped.
			{ 1654, 10810 },
			{ 1655, 10811 },
			{ 1656, 10812 },
			{ 1657, 10813 },
			{ 1658, 10814 },
			{ 1659, 10815 },
			{ 1660, 10816 },
			{ 1665, 10821 },
			{ 1669, 10825 },
			{ 1684, 10840 },
			{ 1685, 10841 },
			{ 1706, 10848 },
			{ 1713, 10855 },
			{ 1714, 10856 },
			{ 1718, 10860 },
			{ 1720, 10862 },
			{ 1725, 10867 },
			{ 1727, 10869 },
			{ 1729, 10871 },
			{ 1730, 10872 },
			{ 1731, 10873 },
			{ 1732, 10874 },
			{ 1733, 10875 },
			{ 1734, 10876 },
			{ 1735, 10877 },
			{ 1736, 10878 },
			{ 1737, 10879 },
			{ 1738, 10880 },
			{ 1739, 10881 },
			{ 1741, 10883 },
			{ 1742, 10884 },
			{ 1743, 10885 },
			{ 1744, 10886 },
			{ 1745, 10887 },
			{ 1746, 10888 },
			{ 1747, 10889 },
			{ 1748, 10890 },
			{ 1749, 10891 },
			{ 1750, 10892 },
			{ 1751, 10893 },
			{ 1752, 10894 },
			{ 1753, 10895 },
			{ 1754, 10896 },
			{ 1755, 10897 },
			{ 1756, 10898 },
			{ 1757, 10899 },
			{ 1758, 10900 },
			{ 1759, 10901 },
			{ 1760, 10902 },
			{ 1762, 10904 },
			{ 1763, 10905 },
			{ 1764, 10906 },
			{ 1765, 10907 },
			{ 1766, 10908 },
			{ 1767, 10909 },
			{ 1778, 10920 },
			{ 1779, 10921 },
			{ 1780, 10922 },
			{ 1781, 10923 },
			{ 1817, 10959 },
			{ 1869, 11042 },
			{ 1870, 11043 },
			{ 1873, 11046 },
			{ 1874, 11047 },
			{ 1880, 11053 },
			{ 2157, 11090 },
			{ 2161, 11094 },
			{ 2162, 11095 },
			{ 2167, 11100 },
			{ 2175, 11108 },
			{ 2176, 11109 },
			{ 2180, 11113 },
			{ 2182, 11115 },
			{ 2228, 11129 },
			{ 2231, 11132 },

			// Tradeskills
			{  716,    65 },
			{  732,   123 },
			{  733, 11057 },
			{  734, 10724 },
			{  749,    38 },
			{  790,    65 },
			{  812,    65 },
			{  857,   133 },
			{  858,   133 },
			{  978,    65 },
			{ 1008,    64 },
			{ 1012,    65 },
			{ 1013,    64 },
			{ 1031, 11072 },
			{ 1032, 11072 },
			{ 1033, 11072 },
			{ 1034, 11072 },
			{ 1035, 11072 },
			{ 1036,    64 },
			{ 1074, 11065 },
			{ 1075, 11064 },
			{ 1076, 11065 },
			{ 1082,    78 },
			{ 1090, 11068 },
			{ 1095, 10724 },
			{ 1096,    64 },
			{ 1103,    78 },
			{ 1110,    65 },
			{ 1135,    64 },
			{ 1138,    64 },
			{ 1140,    65 },
			{ 1143,    38 },
			{ 1151,    64 },
			{ 1213, 10944 },
			{ 1238,    64 },
			{ 1278,   122 },
			{ 1914, 11064 },
			{ 1915, 11064 },
			{ 1916, 11064 },
			{ 1917, 11065 },
			{ 2003, 11068 },
			{ 2013, 11065 },
			{ 2015, 11057 },
			{ 2022, 11064 },
			{ 2023, 11065 },
			{ 2026, 11068 },
			{ 2082,    64 },
			{ 2163, 11096 },

			// Trinkets
			{  547,    36 },
			{  559,    36 },
			{  572,    61 },
			{  644, 11122 },
			{  645, 11121 },
			{  646, 11122 },
			{  647, 11121 },
			{  648,    65 },
			{  649,    65 },
			{  650,    65 },
			{  651,    65 },
			{  652,    65 },
			{  653,    65 },
			{  654,    65 },
			{  684,    48 },
			{  685,    48 },
			{  720, 10697 },
			{  728,    48 },
			{  729,    48 },
			{  735,   199 },
			{  893,    72 },
			{  894,    72 },
			{  895,    72 },
			{  896,    65 },
			{  897,    65 },
			{  898,    65 },
			{  899,    65 },
			{  900,    65 },
			{  901,    65 },
			{  904,    55 },
			{ 1007,    67 },
			{ 1027,   133 },
			{ 1028,   199 },
			{ 1127,   133 },
			{ 1128,    36 },
			{ 1129, 11050 },
			{ 1248,   133 },
			{ 1276, 10409 },
			{ 1316, 10600 },
			{ 1385, 10666 },
			{ 1386, 10667 },
			{ 1414, 10697 },
			{ 1481, 11071 },
			{ 1514, 10742 }, // 10758 w/o particles
			{ 1857, 10999 },
			{ 1863, 11036 },
			{ 1864, 11037 },
			{ 1865, 11038 },
			{ 1866, 11039 },
			{ 1867, 11040 },
			{ 1896, 11122 },
			{ 1897, 11122 },
			{ 1898, 11122 },
			{ 2030, 11072 },
			{ 2079, 11121 },
			{ 2084, 11122 },
			{ 2187, 11120 },
			{ 2188, 11121 },
			{ 2189, 11122 },
			{ 2249, 11139 },
			{ 2252, 11120 },
			{ 2253,    65 }

		}; // 10750 - Lightning halo, rising stars
		// 1877's is 11050 (iceball w/ particles. Hand held item?

        #region Races - 732 Entries

        public string[] Races = new string[] {
            "Human",
            "Barbarian",
            "Erudite",
            "Wood Elf",
            "High Elf",
            "Dark Elf",
            "Half Elf",
            "Dwarf",
            "Troll",
            "Ogre",
            "Halfling",
            "Gnome",
            "Aviak",
            "Werewolf",
            "Brownie",
            "Centaur",
            "Golem",
            "Giant",
            "Trakanon",
            "Venril Sathir",
            "Evil Eye",
            "Beetle",
            "Kerran",
            "Fish",
            "Fairy",
            "Froglok",
            "Froglok",
            "Fungusman",
            "Gargoyle",
            "Gasbag",
            "Gelatinous Cube",
            "Ghost",
            "Ghoul",
            "Bat",
            "Unknown (35)",
            "Rat",
            "Snake",
            "Spider",
            "Gnoll",
            "Goblin",
            "Gorilla",
            "Wolf",
            "Bear",
            "Guard",
            "Demi Lich",
            "Imp",
            "Griffin",
            "Kobold",
            "Dragon",
            "Lion",
            "Lizard Man",
            "Mimic",
            "Minotaur",
            "Orc",
            "Beggar",
            "Pixie",
            "Drachnid",
            "Solusek Ro",
            "Goblin",
            "Skeleton",
            "Shark",
            "Tunare",
            "Tiger",
            "Treant",
            "Vampire",
            "Rallos Zek",
            "Human",
            "Tentacle Terror",
            "Will-O-Wisp",
            "Zombie",
            "Human",
            "Ship",
            "Launch",
            "Piranha",
            "Elemental",
            "Puma",
            "Dark Elf",
            "Erudite",
            "Bixie",
            "Reanimated Hand",
            "Halfling",
            "Scarecrow",
            "Skunk",
            "Snake Elemental",
            "Spectre",
            "Sphinx",
            "Armadillo",
            "Clockwork Gnome",
            "Drake",
            "Barbarian",
            "Alligator",
            "Troll",
            "Ogre",
            "Dwarf",
            "Cazic Thule",
            "Cockatrice",
            "Daisy Man",
            "Vampire",
            "Amygdalan",
            "Dervish",
            "Efreeti",
            "Tadpole",
            "Kedge",
            "Leech",
            "Swordfish",
            "Guard",
            "Mammoth",
            "Eye",
            "Wasp",
            "Mermaid",
            "Harpy",
            "Guard",
            "Drixie",
            "Ghost Ship",
            "Clam",
            "Seahorse",
            "Ghost",
            "Ghost",
            "Saber-toothed Cat",
            "Wolf",
            "Gorgon",
            "Dragon",
            "Innoruuk",
            "Unicorn",
            "Pegasus",
            "Djinn",
            "Invisible Man",
            "Iksar",
            "Scorpion",
            "Vah Shir",
            "Sarnak",
            "Draglock",
            "Drolvarg",
            "Mosquito",
            "Rhinoceros",
            "Xalgoz",
            "Goblin",
            "Yeti",
            "Iksar",
            "Giant",
            "Boat",
            "Unknown (142)",
            "Unknown (143)",
            "Burynai",
            "Goo",
            "Sarnak Spirit",
            "Iksar Spirit",
            "Fish",
            "Scorpion",
            "Erollisi",
            "Tribunal",
            "Bertoxxulous",
            "Bristlebane",
            "Fay Drake",
            "Undead Sarnak",
            "Ratman",
            "Wyvern",
            "Wurm",
            "Devourer",
            "Iksar Golem",
            "Undead Iksar",
            "Man-Eating Plant",
            "Raptor",
            "Sarnak Golem",
            "Dragon",
            "Animated Hand",
            "Succulent",
            "Holgresh",
            "Brontotherium",
            "Snow Dervish",
            "Dire Wolf",
            "Manticore",
            "Totem",
            "Ice Spectre",
            "Enchanted Armor",
            "Snow Rabbit",
            "Walrus",
            "Geonid",
            "Unknown (179)",
            "Unknown (180)",
            "Yakkar",
            "Faun",
            "Coldain",
            "Dragon",
            "Hag",
            "Hippogriff",
            "Siren",
            "Giant",
            "Giant",
            "Othmir",
            "Ulthork",
            "Dragon",
            "Abhorrent",
            "Sea Turtle",
            "Dragon",
            "Dragon",
            "Ronnie Test",
            "Dragon",
            "Shik'Nar",
            "Rockhopper",
            "Underbulk",
            "Grimling",
            "Worm",
            "Evan Test",
            "Shadel",
            "Owlbear",
            "Rhino Beetle",
            "Vampire",
            "Earth Elemental",
            "Air Elemental",
            "Water Elemental",
            "Fire Elemental",
            "Wetfang Minnow",
            "Thought Horror",
            "Tegi",
            "Horse",
            "Shissar",
            "Fungal Fiend",
            "Vampire",
            "Stonegrabber",
            "Scarlet Cheetah",
            "Zelniak",
            "Lightcrawler",
            "Shade",
            "Sunflower",
            "Sun Revenant",
            "Shrieker",
            "Galorian",
            "Netherbian",
            "Akheva",
            "Grieg Veneficus",
            "Sonic Wolf",
            "Ground Shaker",
            "Vah Shir Skeleton",
            "Wretch",
            "Seru",
            "Recuso",
            "Vah Shir",
            "Guard",
            "Teleport Man",
            "Werewolf",
            "Nymph",
            "Dryad",
            "Treant",
            "Fly",
            "Tarew Marr",
            "Solusek Ro",
            "Clockwork Golem",
            "Clockwork Brain",
            "Banshee",
            "Guard of Justice",
            "Mini POM",
            "Diseased Fiend",
            "Solusek Ro Guard",
            "Bertoxxulous",
            "The Tribunal",
            "Terris Thule",
            "Vegerog",
            "Crocodile",
            "Bat",
            "Hraquis",
            "Tranquilion",
            "Tin Soldier",
            "Nightmare Wraith",
            "Malarian",
            "Knight of Pestilence",
            "Lepertoloth",
            "Bubonian",
            "Bubonian Underling",
            "Pusling",
            "Water Mephit",
            "Stormrider",
            "Junk Beast",
            "Broken Clockwork",
            "Giant Clockwork",
            "Clockwork Beetle",
            "Nightmare Goblin",
            "Karana",
            "Blood Raven",
            "Nightmare Gargoyle",
            "Mouth of Insanity",
            "Skeletal Horse",
            "Saryrn",
            "Fennin Ro",
            "Tormentor",
            "Soul Devourer",
            "Nightmare",
            "Rallos Zek",
            "Vallon Zek",
            "Tallon Zek",
            "Air Mephit",
            "Earth Mephit",
            "Fire Mephit",
            "Nightmare Mephit",
            "Zebuxoruk",
            "Mithaniel Marr",
            "Undead Knight",
            "The Rathe",
            "Xegony",
            "Fiend",
            "Test Object",
            "Crab",
            "Phoenix",
            "Dragon",
            "Bear",
            "Giant",
            "Giant",
            "Giant",
            "Giant",
            "Giant",
            "Giant",
            "Giant",
            "War Wraith",
            "Wrulon",
            "Kraken",
            "Poison Frog",
            "Nilborien",
            "Valorian",
            "War Boar",
            "Efreeti",
            "War Boar",
            "Valorian",
            "Animated Armor",
            "Undead Footman",
            "Rallos Zek Minion",
            "Arachnid",
            "Crystal Spider",
            "Zebuxoruk's Cage",
            "BoT Portal",
            "Froglok",
            "Troll",
            "Troll",
            "Troll",
            "Ghost",
            "Pirate",
            "Pirate",
            "Pirate",
            "Pirate",
            "Pirate",
            "Pirate",
            "Pirate",
            "Pirate",
            "Frog",
            "Troll Zombie",
            "Luggald",
            "Luggald",
            "Luggalds",
            "Drogmore",
            "Froglok Skeleton",
            "Undead Froglok",
            "Knight of Hate",
            "Arcanist of Hate",
            "Veksar",
            "Veksar",
            "Veksar",
            "Chokidai",
            "Undead Chokidai",
            "Undead Veksar",
            "Vampire",
            "Vampire",
            "Rujarkian Orc",
            "Bone Golem",
            "Synarcana",
            "Sand Elf",
            "Vampire",
            "Rujarkian Orc",
            "Skeleton",
            "Mummy",
            "Goblin",
            "Insect",
            "Froglok Ghost",
            "Dervish",
            "Shade",
            "Golem",
            "Evil Eye",
            "Box",
            "Barrel",
            "Chest",
            "Vase",
            "Table",
            "Weapon Rack",
            "Coffin",
            "Bones",
            "Jokester",
            "Nihil",
            "Trusik",
            "Stone Worker",
            "Hynid",
            "Turepta",
            "Cragbeast",
            "Stonemite",
            "Ukun",
            "Ixt",
            "Ikaav",
            "Aneuk",
            "Kyv",
            "Noc",
            "Ra`tuk",
            "Taneth",
            "Huvul",
            "Mutna",
            "Mastruq",
            "Taelosian",
            "Discord Ship",
            "Stone Worker",
            "Mata Muram",
            "Lightning Warrior",
            "Succubus",
            "Bazu",
            "Feran",
            "Pyrilen",
            "Chimera",
            "Dragorn",
            "Murkglider",
            "Rat",
            "Bat",
            "Gelidran",
            "Discordling",
            "Girplan",
            "Minotaur",
            "Dragorn Box",
            "Runed Orb",
            "Dragon Bones",
            "Muramite Armor Pile",
            "Crystal Shard",
            "Portal",
            "Coin Purse",
            "Rock Pile",
            "Murkglider Egg Sac",
            "Drake",
            "Dervish",
            "Drake",
            "Goblin",
            "Kirin",
            "Dragon",
            "Basilisk",
            "Dragon",
            "Dragon",
            "Puma",
            "Spider",
            "Spider Queen",
            "Animated Statue",
            "Unknown (443)",
            "Unknown (444)",
            "Dragon Egg",
            "Dragon Statue",
            "Lava Rock",
            "Animated Statue",
            "Spider Egg Sack",
            "Lava Spider",
            "Lava Spider Queen",
            "Dragon",
            "Giant",
            "Werewolf",
            "Kobold",
            "Sporali",
            "Gnomework",
            "Orc",
            "Corathus",
            "Coral",
            "Drachnid",
            "Drachnid Cocoon",
            "Fungus Patch",
            "Gargoyle",
            "Witheran",
            "Dark Lord",
            "Shiliskin",
            "Snake",
            "Evil Eye",
            "Minotaur",
            "Zombie",
            "Clockwork Boar",
            "Fairy",
            "Witheran",
            "Air Elemental",
            "Earth Elemental",
            "Fire Elemental",
            "Water Elemental",
            "Alligator",
            "Bear",
            "Scaled Wolf",
            "Wolf",
            "Spirit Wolf",
            "Skeleton",
            "Spectre",
            "Bolvirk",
            "Banshee",
            "Banshee",
            "Elddar",
            "Forest Giant",
            "Bone Golem",
            "Horse",
            "Pegasus",
            "Shambling Mound",
            "Scrykin",
            "Treant",
            "Vampire",
            "Ayonae Ro",
            "Sullon Zek",
            "Banner",
            "Flag",
            "Rowboat",
            "Bear Trap",
            "Clockwork Bomb",
            "Dynamite Keg",
            "Pressure Plate",
            "Puffer Spore",
            "Stone Ring",
            "Root Tentacle",
            "Runic Symbol",
            "Saltpetter Bomb",
            "Floating Skull",
            "Spike Trap",
            "Totem",
            "Web",
            "Wicker Basket",
            "Nightmare/Unicorn",
            "Horse",
            "Nightmare/Unicorn",
            "Bixie",
            "Centaur",
            "Drakkin",
            "Giant",
            "Gnoll",
            "Griffin",
            "Giant Shade",
            "Harpy",
            "Mammoth",
            "Satyr",
            "Dragon",
            "Dragon",
            "Dyn'Leth",
            "Boat",
            "Weapon Rack",
            "Armor Rack",
            "Honey Pot",
            "Jum Jum Bucket",
            "Toolbox",
            "Stone Jug",
            "Small Plant",
            "Medium Plant",
            "Tall Plant",
            "Wine Cask",
            "Elven Boat",
            "Gnomish Boat",
            "Barrel Barge Ship",
            "Goo",
            "Goo",
            "Goo",
            "Merchant Ship",
            "Pirate Ship",
            "Ghost Ship",
            "Banner",
            "Banner",
            "Banner",
            "Banner",
            "Banner",
            "Aviak",
            "Beetle",
            "Gorilla",
            "Kedge",
            "Kerran",
            "Shissar",
            "Siren",
            "Sphinx",
            "Human",
            "Campfire",
            "Brownie",
            "Dragon",
            "Exoskeleton",
            "Ghoul",
            "Clockwork Guardian",
            "Mantrap",
            "Minotaur",
            "Scarecrow",
            "Shade",
            "Rotocopter",
            "Tentacle Terror",
            "Wereorc",
            "Worg",
            "Wyvern",
            "Chimera",
            "Kirin",
            "Puma",
            "Boulder",
            "Banner",
            "Elven Ghost",
            "Human Ghost",
            "Chest",
            "Chest",
            "Crystal",
            "Coffin",
            "Guardian CPU",
            "Worg",
            "Mansion",
            "Floating Island",
            "Cragslither",
            "Wrulon",
            "Spell Particle 1",
            "Invisible Man of Zomm",
            "Robocopter of Zomm",
            "Burynai",
            "Frog",
            "Dracolich",
            "Iksar Ghost",
            "Iksar Skeleton",
            "Mephit",
            "Muddite",
            "Raptor",
            "Sarnak",
            "Scorpion",
            "Tsetsian",
            "Wurm",
            "Nekhon",
            "Hydra Crystal",
            "Crystal Sphere",
            "Gnoll",
            "Sokokar",
            "Stone Pylon",
            "Demon Vulture",
            "Wagon",
            "God of Discord",
            "Feran Mount",
            "Ogre NPC - Male",
            "Sokokar Mount",
            "Giant (Rallosian mats)",
            "Sokokar (w saddle)",
            "10th Anniversary Banner",
            "10th Anniversary Cake",
            "Wine Cask",
            "Hydra Mount",
            "Hydra NPC",
            "Wedding Flowers",
            "Wedding Arbor",
            "Wedding Altar",
            "Powder Keg",
            "Apexus",
            "Bellikos",
            "Brell's First Creation",
            "Brell",
            "Crystalskin Ambuloid",
            "Cliknar Queen",
            "Cliknar Soldier",
            "Cliknar Worker",
            "Coldain",
            "Unknown (646)",
            "Crystalskin Sessiloid",
            "Genari",
            "Gigyn",
            "Greken - Young Adult",
            "Greken - Young",
            "Cliknar Mount",
            "Telmira",
            "Spider Mount",
            "Bear Mount",
            "Rat Mount",
            "Sessiloid Mount",
            "Morell Thule",
            "Marionette",
            "Book Dervish",
            "Topiary Lion",
            "Rotdog",
            "Amygdalan",
            "Sandman",
            "Grandfather Clock",
            "Gingerbread Man",
            "Royal Guard",
            "Rabbit",
            "Blind Dreamer",
            "Cazic Thule",
            "Topiary Lion Mount",
            "Rot Dog Mount",
            "Goral Mount",
            "Selyrah Mount",
            "Sclera Mount",
            "Braxi Mount",
            "Kangon Mount",
            "Erudite",
            "Wurm Mount",
            "Raptor Mount",
            "Invisible Man",
            "Whirligig",
            "Gnomish Balloon",
            "Gnomish Rocket Pack",
            "Gnomish Hovering Transport",
            "Selyrah",
            "Goral",
            "Braxi",
            "Kangon",
            "Invisible Man",
            "Floating Tower",
            "Explosive Cart",
            "Blimp Ship",
            "Tumbleweed",
            "Alaran",
            "Swinetor",
            "Triumvirate",
            "Hadal",
            "Hovering Platform",
            "Parasitic Scavenger",
            "Grendlaen",
            "Ship in a Bottle",
            "Alaran Sentry Stone",
            "Dervish",
            "Regeneration Pool",
            "Teleportation Stand",
            "Relic Case",
            "Alaran Ghost",
            "Skystrider",
            "Water Spout",
            "Aviak Pull Along",
            "Gelatinous Cube",
            "Cat",
            "Elk Head",
            "Holgresh",
            "Beetle",
            "Vine Maw",
            "Ratman",
            "Fallen Knight",
            "Flying Carpet",
            "Carrier Hand",
            "Akheva",
            "Servant of Shadow",
            "Luclin",
            "Xaric the Unspoken",
            "Dervish (Ver. 5)",
            "Dervish (Ver. 6)",
            "God - Luclin (Ver. 2)",
            "God - Luclin (Ver. 3)",
            "Orb",
            "God - Luclin (Ver. 4)",
            "Pegasus"
        };

        #endregion

        #endregion

        public const bool IgnoreFieldErrors = false;
        
        public enum ItemClasses
        {
            Common,
            Container,
            Readable,
            Unknown = 255
        };

        public enum ItemGroups
        {
            Unknown = 0,
            Weapon,
            Armor,
            Augmentation,
            Container,
            Instrument,
            FoodDrink,
            Benefit,
            Potion
        }

        public DataRow PreviewItem;

        public int HighestNourishment;
        public int HighestFactionID;

        public int OldItemID = 0;
        public bool ItemLoading = false;
        public DataRow Item;
        public ItemGroups ItemGroup;
        public StringBuilder ItemLine = new StringBuilder();
        public List<string> IconCategories = new List<string>();
        public List<int[]> IconsInCategory = new List<int[]>();
        public Dictionary<int, Image> IconImages = new Dictionary<int, Image>();
        public Dictionary<int, int> CategoryOfIcon = new Dictionary<int, int>();
        
        public formIconPicker IconPickerWindow = null;
        public Point IconPickerWindowCoords = new Point(0,0);
        public bool IconPickerWindowCoordsValid = false;

		public string DBError = "";
		public bool DBLoaded = false;
		public bool DBConnected = false;
		public bool DBTested = false;
        public bool EditableState = false;

        public Dictionary<string, bool> ChangedColumn = new Dictionary<string, bool>();
        public Dictionary<string, bool> UsedColumn = new Dictionary<string, bool>();
        public bool AllUsedColumnsFinished = false;
        
        public Dictionary<int, string> SpellNames = new Dictionary<int,string>();
        public Dictionary<int, string> SpellDescs = new Dictionary<int,string>();
        public Dictionary<int, string> FactionNames = new Dictionary<int, string>();
        public Dictionary<int, string> ItemNames = new Dictionary<int, string>();

		#region Form Event Handlers

        public formMain()
        {
            InitializeComponent();

            this.Text += " v" + float.Parse(Application.ProductVersion.Substring(0, 2).Replace(".", "") + "." + Application.ProductVersion.Substring(2).Replace(".", "")).ToString("#0.0###");

            ItemLoading = true;

            listEditScrollTypeShow.SelectedIndex = 0;
            listEditProcTypeShow.SelectedIndex = 0;
            listEditWornTypeShow.SelectedIndex = 0;
            listEditFocusTypeShow.SelectedIndex = 0;
            listEditBardEffectTypeShow.SelectedIndex = 0;

            ItemLoading = false;

            SetHandler(this);
        }
        
        private void buttonEditIcon_Click(object sender, EventArgs e)
        {
            if (IconPickerWindow == null)
            {
                IconPickerWindow = new formIconPicker();

                IconPickerWindow.ShowDialog(this);
            }
            else
            {
                IconPickerWindow.Visible = true;
            }
        }

        private void buttonIconFolder_Click(object sender, EventArgs e)
        {
            switch (dialogIconFolder.ShowDialog(this))
            {
                case DialogResult.OK:
                    labelIconFolder.Text = dialogIconFolder.SelectedPath;

                    if (labelIconFolder.Text.StartsWith(Environment.GetEnvironmentVariable("userprofile"), StringComparison.CurrentCultureIgnoreCase))
                    {
                        labelIconFolder.Text = @"%UserProfile%" + labelIconFolder.Text.Substring(Environment.GetEnvironmentVariable("userprofile").Length);
                    }
                    break;
            }
        }

        private void buttonItemClone_Click(object sender, EventArgs e)
        {
            if (Item == null)
            {
                CheckEditableState();

                return;
            }

            DataRow _curItem = Item;
            DataRow _curPreview = PreviewItem;

            if (panelEdit.Visible)
            {
                // Cloning the one item we're viewing.

                if (!Item_ConfirmChange())
                {
                    return;
                }

                PreviewItem = Item;

                if (Item_Clone() == DialogResult.OK)
                {
                    ChangedColumn.Clear();
                    PreviewItem = Item;
                    Item = null;
                    Item_EditMode();
                    UpdatePreviewBox(-1);
                    buttonSearchName_Click(buttonSearchName, null);
                }
                else
                {
                    Item = _curItem;
                    PreviewItem = _curPreview;
                }
            }
            else
            {
                // Cloning one or more items from the search results

                foreach (ListViewItem _item in listSearchItems.SelectedItems)
                {
                    Item = Item_Load((int)_item.Tag);
                    PreviewItem = Item;
                    Item_Clone();
                }

                Item = _curItem;
                PreviewItem = _curPreview;

                buttonSearchName_Click(buttonSearchName, null);
            }
        }

        private void buttonItemDelete_Click(object sender, EventArgs e)
        {
            if (panelEdit.Visible || (listSearchItems.SelectedItems.Count == 1))
            {
                if (MessageBox.Show(this, "WARNING: You are about to DELETE\r\n\r\n" + TextField("name") + " (" + OldItemID.ToString() + ").\r\n\r\nAre you SURE you wish to do this?", "Delete Item Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                {
                    return;
                }
            }
            else
            {
                if (MessageBox.Show(this, "WARNING: You are about to DELETE " + listSearchItems.SelectedItems.Count.ToString() + " items.\r\n\r\nAre you SURE you wish to do this?", "Delete Item Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                {
                    return;
                }
            }

            if (panelEdit.Visible)
            {
                // Deleting the item we're editing

                Item_Delete();
                ChangedColumn.Clear();
                searchToolStripMenuItem_Click(searchToolStripMenuItem, null);

                Item = null;
                PreviewItem = null;
                Item_Preview(-1);
            }
            else
            {
                int _curItemID = OldItemID;

                // Deleting one or more items from the search results
                foreach (ListViewItem _item in listSearchItems.SelectedItems)
                {
                    OldItemID = (int)_item.Tag;
                    Item_Delete();

                    if (OldItemID == _curItemID)
                    {
                        // We deleted the one we're editing, too.

                        ChangedColumn.Clear();
                        Item = null;
                        PreviewItem = null;
                        _curItemID = -1;
                        buttonSearchName_Click(buttonSearchName, null);

                        if (listSearchItems.SelectedItems.Count > 0)
                        {
                            Item_Preview((int)listSearchItems.SelectedItems[listSearchItems.SelectedItems.Count - 1].Tag);
                        }
                        else
                        {
                            Item_Preview(-1);
                        }
                    }
                }

                OldItemID = _curItemID;
            }

            buttonSearchName_Click(buttonSearchName, null);
            CheckEditableState();
        }

        private void buttonSearchName_Click(object sender, EventArgs e)
        {
            if (textSearchName.Text.Trim().Length > 0)
            {
                Settings.SearchName = textSearchName.Text.Trim();
                Settings.Save();

                listSearchItems.Items.Clear();

                DataTable _items = GetData("SELECT * FROM `items` WHERE `name` LIKE '%" + textSearchName.Text.Replace("'", "''") + "%' ORDER BY `name`;");

                if ((_items.Rows.Count > 0) && (_items.Rows[0]["id"] != DBNull.Value))
                {
                    foreach (DataRow _row in _items.Rows)
                    {
                        ListViewItem _item = new ListViewItem();
                        _item.Name = "item" + _row["id"].ToString();
                        _item.Text = (string)_row["name"];
                        _item.ImageKey = _row["icon"].ToString();
                        _item.Tag = (int)_row["id"];

                        listSearchItems.Items.Add(_item);
                    }
				}

				buttonSearchName.Enabled = false;
            }
        }

		private void buttonTestDatabase_Click(object sender, EventArgs e)
		{
			if ((Settings.Database.Conn != textDBConn.Text) ||
				(Settings.Database.Host != textDBHost.Text) ||
				(Settings.Database.Name != textDBName.Text) ||
				(Settings.Database.User != textDBUser.Text) ||
				(Settings.Database.Pass != textDBPass.Text) ||
				(Settings.Database.Port != textDBPort.Text) ||
				(Settings.AutoClasses != checkAutoSetClasses.Checked) ||
				(Settings.Auto3DModel != checkAutoSet3DModel.Checked))
			{
				Settings.Database.Conn = textDBConn.Text;
				Settings.Database.Host = textDBHost.Text;
				Settings.Database.Name = textDBName.Text;
				Settings.Database.User = textDBUser.Text;
				Settings.Database.Pass = textDBPass.Text;
				Settings.Database.Port = textDBPort.Text;
				Settings.AutoClasses = checkAutoSetClasses.Checked;
				Settings.Auto3DModel = checkAutoSet3DModel.Checked;

				Settings.Save();

				DBLoaded = false;
				DBConnected = false;
			}

			if (CheckDBConnection())
			{
				DBTested = true;

				string _message = "Connection is good!\r\n\r\nThe search screen beckons";
				
				if (labelIconFolder.Text == "")
				{
					_message += ", but be sure to set the Icons folder first!";
				}
				else
				{
					_message += "...";
				}
				
				MessageBox.Show(this, _message, "Database Connection Good", MessageBoxButtons.OK, MessageBoxIcon.Information);

				if (labelIconFolder.Text != "")
				{
					searchToolStripMenuItem_Click(null, null);
				}
			}
			else
			{
				MessageBox.Show(this, "Connection was unsuccessful. The error message received was:\r\n\r\n" + DBError, "Database Connection Good", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ItemLoading)
            {
                return;
            }

            if (!EditableState)
            {
                return;
            }

            if ((Item == null) || (textEditID.Text.Equals("")))
            {
                if (PreviewItem == null)
                {
                    EditableState = false;

                    return;
                }
                
                // Editing our first item of the session.                
                Item_EditMode();
            }
            else
            {
                if ((PreviewItem != null) &&
                    (!textEditID.Text.Equals(Item["id"].ToString()) ||
                     !OldItemID.Equals(PreviewItem["id"])))
                {
                    // We've changed items.
                    Item_EditMode();
                }
            }

            if (Item != null)
            {
                panelSearch.Visible = false;
                panelEdit.Visible = true;
                panelOptions.Visible = false;

                if (Item != PreviewItem)
                {
                    PreviewItem = Item;

                    UpdatePreviewBox(-1);
                }
            }
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            CheckEditableState();
            
            ResetOptionFields();

            if (CheckDBConnection())
            {
				DBTested = true;

				searchToolStripMenuItem.ForeColor = SystemColors.ControlText;
				labelLoading.BringToFront();
				labelLoading.Visible = true;

				timerLoading.Enabled = true;
			}
            else
            {
				// Error connecting to database

				searchToolStripMenuItem.ForeColor = SystemColors.ScrollBar;
				optionsToolStripMenuItem_Click(null, null);
            }
        }

        private void formCheckbox_CheckChanged(object sender, EventArgs e)
        {
            if (ItemLoading)
            {
                return;
            }

            ItemLoading = true;

            CheckBox _box = (CheckBox)sender;
            string _field = _box.Name.Substring(8).ToLower();
            int _boxnum = ((_box.Tag == null) || (_box.Tag.Equals(""))) ? 98 : int.Parse(_box.Tag.ToString());
            int _value = (_box.Checked ? 1 : 0);

            switch (_box.Name.ToLower())
            {
                case "checkautosetclasses":
					Settings.AutoClasses = _box.Checked;
                    ItemLoading = false;
                    return;
                case "checkautoset3dmodel":
					Settings.Auto3DModel = _box.Checked;
                    ItemLoading = false;
					break;
				default:
                    SetField(_field, _value);
                    break;
            }

            UpdatePreviewBox(_boxnum);

            ItemLoading = false;
        }

        private void formCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ItemLoading)
            {
                return;
            }

            ItemLoading = true;

            CheckedListBox _box = (CheckedListBox)sender;
            string _field = _box.Name.Substring(8).ToLower();
            int _boxnum = ((_box.Tag == null) || (_box.Tag.Equals(""))) ? 98 : int.Parse(_box.Tag.ToString());

            int _index = _box.SelectedIndex;
            string _name = (_index < 0) ? "" : _box.SelectedItems[0].ToString();
            bool _checked = !_box.CheckedIndices.Contains(_index);
            int _ifchecked = _checked ? 1 : 0;

            if (_index > -1)
            {
                _box.SelectedIndex = -1;

                editToolStripMenuItem_Click(null, null);
            }

            switch (_field)
            {
                case "flags":
                    string _flag = ItemFlagFields[_index];

                    switch (_flag[0])
                    {
                        case 'l': // loregroup
                            SetField(_flag, _ifchecked);
                            textEditLoreGroup.Text = (-_ifchecked).ToString();
                            UpdatePreviewBox(8, true);
                            break;
                        case '-': // nodrop/norent, but not nopet
                            SetField(_flag.Substring(1), 1 - _ifchecked);
                            break;
                        case 't': // tradeskills
                            _boxnum = 10;
                            SetField(_flag, _ifchecked);
                            break;
                        case 'c': // charmfileid
                            _boxnum = 98; // No preview change
                            SetField(_flag, _ifchecked);
                            break;
                        default:
                            SetField(_flag, _ifchecked);
                            break;
                    }
                    break;
                case "flags2":
                    SetField(ItemFlagFields2[_index], _ifchecked);
                    break;
                case "races":
                    switch (_index)
                    {
                        case 0: // All
                            if (_checked)
                            {
                                SetField(_field, IntField(_field) | 0xFFFF);
                            }
                            else
                            {
                                SetField(_field, IntField(_field) & 0x10000);
                            }

                            for (_index = 1; _index < (_box.Items.Count - 1); _index++)
                            {
                                _box.SetItemChecked(_index, _checked);
                            }
                            break;
                        default:
                            SetField(_field, IntField(_field) ^ (1 << (_index - 1)));
                            break;
                    }
                    break;
                case "classes":
                    switch (_index)
                    {
                        case 0: // All
                            if (_checked)
                            {
                                SetField(_field, 0xFFFF);
                            }
                            else
                            {
                                SetField(_field, 0);
                            }

                            for (_index = 1; _index < _box.Items.Count; _index++)
                            {
                                _box.SetItemChecked(_index, _checked);
                            }
                            break;
                        default:
                            SetField(_field, IntField(_field) ^ (1 << (_index - 1)));
                            break;
                    }
                    break;
                case "deity":
                    switch (_index)
                    {
                        case 0: // All
                            if (_checked)
                            {
                                SetField(_field, 0);
                            }
                            else
                            {
                                SetField(_field, 0x1FFFF);
                            }

                            for (_index = 1; _index < _box.Items.Count; _index++)
                            {
                                _box.SetItemChecked(_index, !_checked);
                            }
                            break;
                        default:
                            _box.SetItemChecked(0, false);

                            SetField(_field, IntField(_field) ^ (1 << (_index - 1)));
                            break;
                    }
                    break;
                case "slots":
                    switch (_index)
                    {
                        case 0: // All
                            if (_checked)
                            {
                                SetField(_field, 0x3FFFFF);
                            }
                            else
                            {
                                SetField(_field, 0);
                            }

                            for (_index = 1; _index < _box.Items.Count; _index++)
                            {
                                _box.SetItemChecked(_index, _checked);
                            }
                            break;
                        default: // Individual Slot
							_box.SetItemChecked(0, false);

							if (_checked)
							{
								SetField(_field, IntField(_field) | SlotFlags[_index - 1]);
							}
							else
							{
								SetField(_field, IntField(_field) & ~SlotFlags[_index - 1]);
							}
                            break;
                    }
                    break;
                case "augtype":
                    switch (_index)
                    {
                        case 0: // All
                            if (_checked)
                            {
                                SetField(_field, 0x3FFFFFFF);
                            }
                            else
                            {
                                SetField(_field, 0);
                            }

                            for (_index = 1; _index < _box.Items.Count; _index++)
                            {
                                _box.SetItemChecked(_index, _checked);
                            }
                            break;
                        default:
                            SetField(_field, IntField(_field) ^ (1 << (_index - 1)));
                            break;
                    }
                    break;
                case "ldontheme":
                    _ifchecked = 0;

                    for (_index = 0; _index < _box.Items.Count; _index++)
                    {
                        if (_box.CheckedIndices.Contains(_index))
                        {
                            _ifchecked |= (1 << _index);
                        }
                    }

                    SetField(_field, _ifchecked);
                    break;
            }

            UpdatePreviewBox(_boxnum);

            ItemLoading = false;
        }

        private void formDropDown_Changed(object sender, EventArgs e)
        {
            if (ItemLoading)
            {
                return;
            }

            ItemLoading = true;
            
            ComboBox _box = (ComboBox)sender;
            string _field = _box.Name.Substring(8).ToLower();
            int _boxnum = ((_box.Tag == null) || (_box.Tag.Equals(""))) ? 98 : int.Parse(_box.Tag.ToString());
            string _value = _box.Text;
            int _index = _box.SelectedIndex;
            int _number = 0;
            bool _isnumber = int.TryParse(_value, out _number);

            if (_value == "")
            {
                _value = "0";
            }

            switch (_field)
            {
                case "faction1":
                case "faction2":
                case "faction3":
                case "faction4":
                    if (_index > 0)
                    {
                        char _factionnumber = _field[_field.Length - 1];
                        tabEditEffects.Controls["textEditFactionMod" + _factionnumber].Text = _box.SelectedValue.ToString();
                        SetField("factionmod" + _factionnumber, _box.SelectedValue.ToString());
                    }
                    break;
                case "itemtype":
                    SetField(_field, _box.SelectedIndex);
					
					if (Settings.AutoClasses)
					{
						SetField("classes", ItemTypeClasses[_box.SelectedIndex]);
						Item_FillField(listEditClasses, "classes", ItemTypeClasses[_box.SelectedIndex].ToString());
					}
					break;
				default:
                    if (!_isnumber && IsNumberField(_field))
                    {
                        SetField(_field, _box.SelectedIndex);
                    }
                    else
                    {
                        SetField(_field, _value);
                    }
                    break;
            }

            UpdatePreviewBox(_boxnum);

            ItemLoading = false;
        }

        private void formRadioButton_CheckChanged(object sender, EventArgs e)
        {
            if (ItemLoading)
            {
                return;
            }

            ItemLoading = true;

            RadioButton _box = (RadioButton)sender;
            string _field = _box.Name.Substring(8).ToLower();
            int _boxnum = ((_box.Tag == null) || (_box.Tag.Equals(""))) ? 98 : int.Parse(_box.Tag.ToString());
            int _value = int.Parse(_field.Substring(_field.Length - 1));
            _field = _field.Substring(0, _field.Length - 1);

            switch (_field)
            {
                case "pointtype":
                    bool _isldon = listEditPointType1.Checked;

                    SetField(_field, _value);
                    SetField("ldonsold", _isldon ? 1 : 0);
                    listEditLDoNTheme.Enabled = _isldon;
                    break;
                default:
                    SetField(_field, _value);
                    break;
            }

            UpdatePreviewBox(_boxnum);

            ItemLoading = false;
        }

        private void formTextbox_Changed(object sender, EventArgs e)
        {
            if (ItemLoading)
            {
                return;
            }

            ItemLoading = true;

            TextBox _box = (TextBox)sender;
            int _boxnum = 0;
            string _value = _box.Text;
            int _number = 0;
            int.TryParse(_value, out _number);
            float _float = 0.0f;
            float.TryParse(_value, out _float);

            switch (_box.Name.ToLower())
            {
                case "textsearchname":
                    buttonSearchName.Enabled = _value.Length > 0;
                    ItemLoading = false;
                    return;
                case "textdbconn":
                case "textdbname":
                case "textdbport":
                case "textdbuser":
                case "textdbpass":
					DBConnected = false;
					DBLoaded = false;
					DBTested = false;
					searchToolStripMenuItem.ForeColor = SystemColors.ScrollBar;
					ItemLoading = false;
                    return;
                case "textdbhost":
                    if (_value.Trim().Equals("localhost", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _box.Text = "127.0.0.1";
                        labelDBHostNote.Visible = true;
                    }
					DBConnected = false;
					DBLoaded = false;
					DBTested = false;
					searchToolStripMenuItem.ForeColor = SystemColors.ScrollBar;
                    ItemLoading = false;
                    return;
                default: // Item Field!
                    string _field = _box.Name.Substring(8);
                    _boxnum = ((_box.Tag == null) || (_box.Tag.ToString() == "")) ? 98 : int.Parse(_box.Tag.ToString());

                    switch (_field.ToLower())
                    {
                        case "bagslots":
                            if (_number > 0)
                            {
                                SetField("itemclass", (int)ItemClasses.Container);
                            }
                            else if (TextField("filename") != "")
                            {
                                SetField("itemclass", (int)ItemClasses.Readable);
                            }
                            else
                            {
                                SetField("itemclass", (int)ItemClasses.Common);
                            }
                            SetField(_field, _value);
                            break;
                        case "filename":
                            if (_value != "")
                            {
                                SetField("itemclass", (int)ItemClasses.Readable);
                                listEditFlags.SetItemChecked(12, true); // Book = 1
                            }
                            else if (IntField("bagslots") > 0)
                            {
                                SetField("itemclass", (int)ItemClasses.Container);
                            }
                            else
                            {
                                SetField("itemclass", (int)ItemClasses.Common);
                            }
                            SetField(_field, _value);
                            break;
                        case "casttime":
                        case "casttime_":
                            SetField("casttime", _number);
                            SetField("casttime_", _number);
                            break;
                        case "loregroup":
                            SetField(_field, _number);

                            listEditFlags.SetItemChecked(1, _number != 0);
                            break;
                        case "color":
                            long _tint = long.Parse(textEditColor.Text, System.Globalization.NumberStyles.HexNumber) | 0xFF000000;
                            boxColorTint.BackColor = Color.FromArgb(unchecked((int)_tint));
                            SetField(_field, _tint);
                            break;
                        case "factionmod1":
                            listEditFaction1.SelectedValue = _number;
                            SetField(_field, _number);
                            break;
                        case "factionmod2":
                            listEditFaction2.SelectedValue = _number;
                            SetField(_field, _number);
                            break;
                        case "factionmod3":
                            listEditFaction3.SelectedValue = _number;
                            SetField(_field, _number);
                            break;
                        case "factionmod4":
                            listEditFaction4.SelectedValue = _number;
                            SetField(_field, _number);
                            break;
                        case "augdistiller":
                            ItemNames.TryGetValue(_number, out _value);
                            labelAugDistiller.Text = _value;
                            SetField(_field, _number);
                            break;
                        case "texteditscrolleffect":
                            SetField(_field, _number);
                            SetField("scrolltype", (_number <= 0) ? 0 : 7);
                            break;
                        case "texteditproceffect":
                            SetField(_field, _number);
                            SetField("proctype", 0);
                            break;
                        case "texteditworneffect":
                            SetField(_field, _number);
                            SetField("worntype", (_number <= 0) ? 0 : 2);
                            break;
                        case "texteditfocuseffect":
                            SetField(_field, _number);
                            SetField("scrolltype", (_number <= 0) ? 0 : 6);
                            break;
                        case "texteditbardeffect":
                            SetField(_field, _number);
                            SetField("bardeffecttype", (_number <= 0) ? 0 : 8);
                            break;
                        case "price":
                        case "ldonpoints":
                        case "ldonsellbackrate":
                            SetField(_field, _number);
                            UpdateCoinValues();
                            break;
                        case "sellrate":
                            SetField(_field, _float);
                            UpdateCoinValues();
                            break;
                        default:
                            try
                            {
                                SetField(_field, _value); // DataRow[] boxes strings into ints automatically to match the field type. Nice!
                            }
                            catch
                            {
                                // User has started typing a negative or floating point number, but hasn't gotten to digits yet
                                if ((_value != "-") && !_value.StartsWith("."))
                                {
                                    // Nope. I don't know why it threw up.

                                    if (!IgnoreFieldErrors)
                                    {
                                        MessageBox.Show(this, "Error setting field `" + _field + "` to value '" + _value + "'", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }
                                }
                            }
                            break;
                    }

                    if (Item[_field].Equals(0) && _value.Equals(""))
                    {
                        _box.Text = Item[_field].ToString();
                    }

                    break;
            }

            UpdatePreviewBox(_boxnum);

            ItemLoading = false;
        }

        private void formTextboxFloat_Keypress(object sender, KeyPressEventArgs e)
        {
            float _dummy;

            TextBox _tbox = sender as TextBox;
            ComboBox _cbox = sender as ComboBox;

            int _selstart = (_tbox != null) ? _tbox.SelectionStart : (_cbox != null) ? _cbox.SelectionStart : 0;
            int _sellength = (_tbox != null) ? _tbox.SelectionLength : (_cbox != null) ? _cbox.SelectionLength : 0;
            string _text = ((Control)sender).Text.Remove(_selstart, _sellength);

            if (e.KeyChar == '.')
            {
                e.Handled = _text.Contains(".");
            }
            else if (e.KeyChar == '-')
            {
                e.Handled = _text != "";
            }
            else if (e.KeyChar > 31)
            {
                e.Handled = !float.TryParse(_text.Insert(_selstart, "" + e.KeyChar), out _dummy);
            }
        }

        private void formTextboxNumber_Keypress(object sender, KeyPressEventArgs e)
        {
            int _dummy;
            
            TextBox _tbox = sender as TextBox;
            ComboBox _cbox = sender as ComboBox;

            int _selstart = (_tbox != null) ? _tbox.SelectionStart : (_cbox != null) ? _cbox.SelectionStart : 0;
            int _sellength = (_tbox != null) ? _tbox.SelectionLength : (_cbox != null) ? _cbox.SelectionLength : 0;
            string _text = ((Control)sender).Text.Remove(_selstart, _sellength);

            if (e.KeyChar == '.')
            {
                e.Handled = true;
            }
            else if (e.KeyChar == '-')
            {
                e.Handled = _text != "";
            }
            else if (e.KeyChar > 31)
            {
                e.Handled = !int.TryParse(_text.Insert(_selstart, "" + e.KeyChar), out _dummy);
            }
        }

        private void formTextboxHex_Keypress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar > 31) && ("0123456789ABCDEFabcdef".IndexOf(e.KeyChar) < 0))
            {
                e.Handled = true;
            }
        }

        private void listSearchItems_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listSearchItems.SelectedItems.Count == 1)
            {
                Item_EditMode();
            }
            else
            {
                MessageBox.Show(this, "Please select one item to edit.", "Cannot Edit Multiple Items", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void listSearchItems_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (ItemLoading)
            {
                return;
            }
            
            CheckEditableState();

            if (e.IsSelected)
            {
                Item_Preview((int)e.Item.Tag);
            }
            else if (listSearchItems.SelectedItems.Count > 0)
            {
                Item_Preview((int)listSearchItems.SelectedItems[listSearchItems.SelectedItems.Count - 1].Tag);
            }
            else
            {
                Item_Preview(-1);
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ItemLoading)
            {
                return;
            }

            ItemLoading = true;

            panelSearch.Visible = false;
            panelEdit.Visible = false;
            panelOptions.Visible = true;

            ItemLoading = false;
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (Settings.IconFolder != labelIconFolder.Text)
			{
				Settings.IconFolder = labelIconFolder.Text;

				Settings.Save();

				itemIcons.Images.Clear();
				itemIcons.Tag = null;
			}

			if (DBTested)
			{
				if (DBLoaded && itemIcons.Tag != null)
				{
					panelSearch.Visible = true;
					panelEdit.Visible = false;
					panelOptions.Visible = false;
				}
				else if (CheckDBConnection())
				{
					labelLoading.BringToFront();
					labelLoading.Visible = true;

					timerLoading.Enabled = true;
				}
			}
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (panelEdit.Visible == true)
            {
                if (PreviewItem != Item)
                {
                    PreviewItem = Item;

                    UpdatePreviewBox(-1);
                }
            }
        }

        private void tabControlEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlEdit.SelectedTab.Text.Equals("Augs", StringComparison.CurrentCultureIgnoreCase))
            {
                boxAugSpecs.Enabled = IntField("itemtype") == 54;

                boxAugSlots.Enabled = !boxAugSpecs.Enabled;
            }
        }

        private void textSearchName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSearchName_Click(null, null);
            }
        }

        private void timerLoading_Tick(object sender, EventArgs e)
        {
			int _i, _j;

			timerLoading.Enabled = false;

            ItemLoading = true;

            if (!DBLoaded)
            {
                Item = null;
                PreviewItem = null;
                OldItemID = -1;
                
                CheckEditableState();
                
                panelSearch.Visible = true;
                panelEdit.Visible = false;
                panelOptions.Visible = false;

                buttonSearchName.Enabled = textSearchName.Text.Length > 0;
                textSearchName.Text = Settings.SearchName;

                HighestNourishment = GetInt("SELECT MAX(`casttime`) FROM `items` WHERE `itemtype` IN (14, 15);");

                ItemNames.Clear();
                DataTable _itemNames = GetData("SELECT `id`, `name` FROM `items`;");
                if (_itemNames.Rows[0][0].ToString() != "") // Empty Recordset
                {
                    for (_i = 0; _i < _itemNames.Rows.Count; _i++)
                    {
                        _j = (int)_itemNames.Rows[_i][0];
                        ItemNames.Add(_j, (string)_itemNames.Rows[_i][1]);
                    }
                }
                _itemNames.Dispose();

                SpellNames.Clear();
                DataTable _spellNames = GetData("SELECT `id`, `name` FROM `spells_new`;");
                if (_spellNames.Rows[0][0].ToString() != "") // Empty Recordset
                {
                    for (_i = 0; _i < _spellNames.Rows.Count; _i++)
                    {
                        _j = (int)_spellNames.Rows[_i][0];
                        SpellNames.Add(_j, (string)_spellNames.Rows[_i][1]);
                    }
                }
                _spellNames.Dispose();

                SpellDescs.Clear();
                DataTable _spellDescs = GetData("SELECT `id`, `string` FROM `dbstr` WHERE `type`=6;");
                if (_spellDescs.Rows[0][0].ToString() != "") // Empty Recordset
                {
                    for (_i = 0; _i < _spellDescs.Rows.Count; _i++)
                    {
                        _j = (int)_spellDescs.Rows[_i][0];
                        SpellDescs.Add(_j, (string)_spellDescs.Rows[_i][1]);
                    }
                }
                _spellDescs.Dispose();

                FactionNames.Clear();
                DataTable _factionNames = GetData("SELECT `id`,`name` FROM `faction_list`;");
                FactionNames.Add(0, "");
                if (_factionNames.Rows[0][0].ToString() != "") // Empty Recordset
                {
                    for (_i = 0; _i < _factionNames.Rows.Count; _i++)
                    {
                        _j = (int)_factionNames.Rows[_i][0];
                        FactionNames.Add(_j, (string)_factionNames.Rows[_i][1]);

                        if (_j > HighestFactionID)
                        {
                            HighestFactionID = _j;
                        }
                    }

                    ItemLoading = true;

                    listEditFaction1.DataSource = new BindingSource(FactionNames, null); // Each one needs a separate BindingSource,
                    listEditFaction2.DataSource = new BindingSource(FactionNames, null); // otherwise selecting from one drop-down
                    listEditFaction3.DataSource = new BindingSource(FactionNames, null); // will make all the others change to match.
                    listEditFaction4.DataSource = new BindingSource(FactionNames, null);

                    ItemLoading = false;
                }
                _factionNames.Dispose();

				buttonSearchName.Enabled = true;
				
				DBLoaded = true;
            }

            if ((itemIcons.Tag == null) && (itemIcons.Images.Count == 0))
            {
                LoadItemIcons();

                itemIcons.Tag = "Loaded";
            }

            buttonItemNew.Enabled = DBLoaded;
            ItemLoading = false;

            labelLoading.Visible = false;
            labelLoading.SendToBack();

			searchToolStripMenuItem_Click(searchToolStripMenuItem, null);
		}

        #endregion

        #region Helper Functions

        private void AppendIf(string FieldName, string Text)
        {
            AppendIf(IntField(FieldName) != 0, Text);
        }
        private void AppendIf(int Value, string Text)
        {
            AppendIf(Value != 0, Text);
        }
        private void AppendIf(bool Condition, string Text)
        {
            if (Condition)
            {
                ItemLine.Append(Text);
            }
        }

        private bool CheckDBConnection()
		{
			if (GetInt("SELECT 1;") == 1)
			{
				DBConnected = true;

				searchToolStripMenuItem.ForeColor = SystemColors.ControlText;
			}
			else
			{
				DBConnected = false;

				searchToolStripMenuItem.ForeColor = SystemColors.ScrollBar;
			}

			return DBConnected;
		}
		
		private void CheckEditableState()
        {
            if (listSearchItems.SelectedItems.Count == 1)
            {
                EditableState = true;
            }
            else if ((listSearchItems.SelectedItems.Count == 0) &&
                     (Item != null))
            {
                EditableState = true;
            }
            else
            {
                EditableState = false;
            }

            if (EditableState)
            {
                editToolStripMenuItem.ForeColor = SystemColors.ControlText;
            }
            else
            {
                editToolStripMenuItem.ForeColor = SystemColors.ScrollBar;
            }

            buttonItemNew.Enabled = DBLoaded;
            
            buttonItemClone.Enabled = (Item != null) ||
                                      (listSearchItems.SelectedItems.Count > 0);

            buttonItemSave.Enabled = (Item != null) &&
                                     (ChangedColumn.Count > 0);

            buttonItemDelete.Enabled = buttonItemClone.Enabled;
        }
        
        private int Execute(string Query)
        {
            string _connStr = Settings.Database.Conn.
                Replace("{Database}", Settings.Database.Name).
                Replace("{Server}", Settings.Database.Host).
                Replace("{Port}", Settings.Database.Port).
                Replace("{Username}", Settings.Database.User).
                Replace("{Password}", Settings.Database.Pass);

            using (OdbcConnection _dbconn = new OdbcConnection(_connStr))
            {
                try
                {
                    _dbconn.Open();

                    using (OdbcCommand _command = new OdbcCommand())
                    {
                        _command.Connection = _dbconn;
                        _command.CommandType = CommandType.Text;
                        _command.CommandText = Query;

                        return _command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    return -1;
                }
            }
        }
        
        private DataTable GetData(string Query)
        {
            string _connStr = Settings.Database.Conn.
                Replace("{Database}", Settings.Database.Name).
                Replace("{Server}", Settings.Database.Host).
                Replace("{Port}", Settings.Database.Port).
                Replace("{Username}", Settings.Database.User).
                Replace("{Password}", Settings.Database.Pass);

            DataTable _dt = new DataTable();

			DBError = "";

            using (OdbcConnection _dbconn = new OdbcConnection(_connStr))
            {
                try
                {
                    _dbconn.Open();

                    using (OdbcCommand _command = new OdbcCommand())
                    {
                        _command.Connection = _dbconn;
                        _command.CommandType = CommandType.Text;
                        _command.CommandText = Query;

                        OdbcDataAdapter _adap = new OdbcDataAdapter(_command);

                        _adap.Fill(_dt);
                        _adap.Dispose();
                    }
                }
                catch (Exception _ex)
                {
					DBError = _ex.Message;

					if (DBError.Substring(0,(DBError.Length >> 1) - 1).Equals(DBError.Substring((DBError.Length >> 1) + 1)))
					{
						// ODBC is doubling up error messages for some reason.

						DBError = DBError.Substring(0, (DBError.Length >> 1) - 1);
					}

					_dt.Columns.Add("Empty", typeof(string));
                }

                if (_dt.Rows.Count < 1)
                {
                    _dt.Rows.Add();
                }
            }

            return _dt;
        }

        private int GetInt(string Query)
        {
            DataTable _dt = GetData(Query);
            int _value;

            if (_dt.Columns[0].ColumnName != "Empty")
            {
                if (int.TryParse(_dt.Rows[0][0].ToString(), out _value))
                {
                    return _value;
                }
            }

            return 0;
        }

        private string GetText(string Query) { return GetText(Query, ""); }
        private string GetText(string Query, string IfEmpty)
        {
            DataTable _dt = GetData(Query);
            object _field;
            string _text;

            if (_dt.Columns[0].ColumnName != "Empty")
            {
                _field = _dt.Rows[0][0];

                if (_field != null)
                {
                    _text = _field.ToString();

                    if (_text != "")
                    {
                        return _text;
                    }
                }
            }

            return IfEmpty;
        }

        private bool IsNumberField(string FieldName)
        {
            return Item[FieldName].GetType().IsPrimitive;
        }
        
        public float FloatField(string FieldName)
        {
            try
            {
                if (!AllUsedColumnsFinished && (Item == PreviewItem))
                {
                    UsedColumn[FieldName.ToLower()] = true; // Edit mode, mark this field used.
                }

                return float.Parse(PreviewItem[FieldName].ToString());
            }
            catch
            {
                if (!IgnoreFieldErrors)
                {
                    MessageBox.Show(this, "Error reading float field `" + FieldName + "`", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            return 0.0f;
        }

        public int IntField(string FieldName)
        {
            try
            {
                if (!AllUsedColumnsFinished && (Item == PreviewItem))
                {
                    UsedColumn[FieldName.ToLower()] = true; // Edit mode, mark this field used.
                }

                return int.Parse(PreviewItem[FieldName].ToString());
            }
            catch
            {
                if (!IgnoreFieldErrors)
                {
                    MessageBox.Show(this, "Error reading numeric field `" + FieldName + "`", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            return 0;
        }

        public string TextField(string FieldName)
        {
            try
            {
                if (!AllUsedColumnsFinished && (Item == PreviewItem))
                {
                    UsedColumn[FieldName.ToLower()] = true; // Edit mode, mark this field used.
                }

                return PreviewItem[FieldName].ToString();
            }
            catch
            {
                if (!IgnoreFieldErrors)
                {
                    MessageBox.Show(this, "Error reading text field `" + FieldName + "`", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            return "";
        }

        public void SetField(string FieldName, object Value)
        {
            if (Item.Table.Columns[FieldName].DataType.IsPrimitive && Value.Equals(""))
            {
                Value = 0; // Empty string in a numeric field -> zero
            }
            
            try
            {
                Item[FieldName] = Value;
                ChangedColumn[FieldName.ToLower()] = true;

				switch (FieldName.ToUpper())
				{
					case "ICON":
						if (Settings.Auto3DModel && !ItemLoading)
						{
							int _modelnum = 0;
							
							if (IconModels.TryGetValue((int)Item[FieldName], out _modelnum))
							{
								textEditIDFile.Text = "IT" + _modelnum.ToString();
							}
							else
							{
								textEditIDFile.Text = "IT63";
							}
						}
						break;
				}

                return;
            }
            catch
            { }

            Item[FieldName] = Value.ToString();
            ChangedColumn[FieldName.ToLower()] = true;

            return;
        }

        public object SQLDefault(DataColumn Column)
        {
            switch (Column.DataType.ToString())
            {
                case "System.Byte":
                case "System.Int8":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                    return 0;
                case "System.Single":
                case "System.Double":
                    return 0.0f;
                case "System.DateTime":
                    return DateTime.UtcNow;
                case "System.String":
                    return "";
                default:
                    System.Diagnostics.Debugger.Break();
                    return null;
            }
        }
        
        public string SQLEncode(DataColumn Column, object Value)
        {
            if (Value == DBNull.Value)
            {
                return "null";
            }
            else
            {
                switch (Column.DataType.ToString())
                {
                    case "System.Byte":
                    case "System.Int8":
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                        return Value.ToString();
                    case "System.Single":
                        return ((Single)Value).ToString("#.0###");
                    case "System.Double":
                        return ((Double)Value).ToString("#.0#######");
                    case "System.DateTime":
                        return "'" + ((DateTime)Value).ToString("yyyy-MM-dd HH:mm:ss.ffff") + "'";
                    case "System.String":
                        return "'" + Value.ToString().Replace("'", "''") + "'";
                    default:
                        return "'" + Value.ToString().Replace("'", "''") + "'";
                }
            }
        }
        
        private void AddValue(int Box, string Value) { AddField(Box, "", "", Value, false); }
        private void AddValue(int Box, string Value, bool LineAbove) { AddField(Box, "", "", Value, LineAbove); }

        private void AddField(int Box, string Field) { AddField(Box, Field, Field, TextField(Field.EndsWith(":") ? Field.Substring(0, Field.Length - 1) : Field), false); }
        private void AddField(int Box, string Name, string Field) { AddField(Box, Name, Field, TextField(Field), false); }
        private void AddField(int Box, string Name, string Field, string Value) { AddField(Box, Name, Field, Value, false); }
        private void AddField(int Box, string Name, string Field, string Value, bool LineAbove)
        {
            if ((Value != "0") && (Value != ""))
            {
                Label _label = (Label)itemPreview.Controls["labelPreviewBox" + Box.ToString()];
                Label _textbox = (Label)itemPreview.Controls["textPreviewBox" + Box.ToString()];

                if (_label != null)
                {
                    if (_label.Text.Length == 0)
                    {
                        _label.Text = Name;
                    }
                    else if (LineAbove)
                    {
                        _label.Text += "\r\n\r\n" + Name;
                    }
                    else
                    {
                        _label.Text += "\r\n" + Name;
                    }
                }

                if (_textbox != null)
                {
                    if (_textbox.Text.Length == 0)
                    {
                        _textbox.Text = Value;
                    }
                    else if (LineAbove)
                    {
                        _textbox.Text += "\r\n\r\n" + Value;
                    }
                    else
                    {
                        _textbox.Text += "\r\n" + Value;
                    }
                }
            }
        }

        private bool Item_ConfirmChange()
        {
            if (ChangedColumn.Count > 0)
            {
                if (MessageBox.Show(this, "You have made unsaved changes to " + Item["name"] + " (" + OldItemID.ToString() + ((OldItemID != IntField("id") ? (", now " + TextField("id")) : "")) + ").\n\nAre you sure you wish to discard them?", "Discard Unsaved Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                {
                    return false;
                }
            }

            return true;
        }

        private void Item_EditMode()
        {
            if (listSearchItems.SelectedItems.Count < 1)
            {
                if (Item == null)
                {
                    if (PreviewItem == null)
                    {
                        EditableState = false;
                        CheckEditableState();

                        return;
                    }
                }
                else if (PreviewItem == null)
                {
                    PreviewItem = Item;
                }

                OldItemID = IntField("id");
            }
            else if (listSearchItems.SelectedItems.Count > 1)
            {
                EditableState = false;
                CheckEditableState();

                return;
            }
            else
            {
                OldItemID = (int)listSearchItems.SelectedItems[0].Tag;
            }

            Item_Preview(OldItemID);

            int _columns = Item.Table.Columns.Count;
            ItemLoading = true;

            string _field = "";
            string _value = "";
            
            List<Control> _controls = new List<Control>();

            // Marking these fields as used so they don't end up in Unrecognized:
            // They are handled in a custom way.
            TextField("icon");
            TextField("itemclass");
            TextField("scrolltype");
            TextField("proctype");
            TextField("worntype");
            TextField("focustype");
            TextField("bardeffecttype");
            TextField("casttime_");
            TextField("ldonsold");
            TextField("pointtype");

            TextField("loregroupmembercount"); // A read-only aggregate result

            foreach (TabPage _page in tabControlEdit.TabPages)
            {
                foreach (Control _pageControl in _page.Controls)
                {
                    _controls.Clear();

                    if (_pageControl.HasChildren)
                    {
                        foreach (Control _childControl in _pageControl.Controls)
                        {
                            if (_childControl.HasChildren)
                            {
                                foreach (Control _grandchildControl in _childControl.Controls)
                                {
                                    _controls.Add(_grandchildControl);
                                }
                            }
                            else
                            {
                                _controls.Add(_childControl);
                            }
                        }
                    }
                    else
                    {
                        _controls.Add(_pageControl);
                    }

                    foreach (Control _checkControl in _controls)
                    {
                        if (_checkControl.Name.Length > 8)
                        {
                            _field = _checkControl.Name.Substring(8).ToLower();

                            if (Item.Table.Columns.Contains(_field))
                            {
                                _value = Item[_field].ToString();
                            }
                            else
                            {
                                switch (_field)
                                {
                                    case "flags": // These controls handled separately
                                    case "flags2":
                                    case "faction1":
                                    case "faction2":
                                    case "faction3":
                                    case "faction4":
                                    case "augtype":
                                    case "pointtype0":
                                    case "pointtype1":
                                    case "pointtype2":
                                    case "pointtype3":
                                    case "pointtype4":
                                    case "pointtype5":
                                        break;
                                    default:
                                        _field = ""; // This control is not tied to an item field
                                        break;
                                }
                            }

                            if (_field != "")
                            {
                                if (Item_FillField(_checkControl, _field, _value) != DialogResult.OK)
                                {
                                    MessageBox.Show(this, "Unable to fill in " + _checkControl.Name + " with field " + _field + " value '" + _value + "'!", "Field Fill Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
            }

            AllUsedColumnsFinished = true;

            List<string> _unusedColumns = new List<string>();

            ItemLine.Length = 0;
            foreach (DataColumn _col in Item.Table.Columns)
            {
                bool _dummy;
                string _colName = _col.ColumnName.ToLower();

                if (!UsedColumn.TryGetValue(_colName, out _dummy))
                {
                    _unusedColumns.Add(_colName);
                }
            }

            _unusedColumns.Sort();

            // Forgot this part in 1.0. My bad...
            tabEditUnrecognized.Controls.Clear();
            
            int _i = 0;
            foreach (string _unusedColumn in _unusedColumns)
            {
                Label _unusedLabel = new Label();
                _unusedLabel.Name = "labelEdit" + _unusedColumn;
                _unusedLabel.AutoSize = false;
                _unusedLabel.TextAlign = ContentAlignment.TopRight;
                _unusedLabel.Text = _unusedColumn;
                _unusedLabel.Size = new Size(101, 16);
                _unusedLabel.Location = new Point(10 + 213 * (_i % 2), 18 + 25 * (_i / 2));
                
                tabEditUnrecognized.Controls.Add(_unusedLabel);

                TextBox _unusedField = new TextBox();
                _unusedField.Name = "textEdit" + _unusedColumn;
                _unusedField.Text = TextField(_unusedColumn);
                _unusedField.Size = new Size(100, 20);
                _unusedField.Location = new Point(114 + 213 * (_i % 2), 16 + 25 * (_i / 2));
                _unusedLabel.TextChanged += this.formTextbox_Changed;

                tabEditUnrecognized.Controls.Add(_unusedField);
                
                _i++;
            }

            OldItemID = IntField("id");
            ItemLoading = false;

            editToolStripMenuItem_Click(editToolStripMenuItem, null);
        }
        
        private DialogResult Item_FillField(Control Field, string Column, string Value)
        {
            ComboBox _combobox = Field as ComboBox;
            TextBox _textbox = Field as TextBox;
            CheckedListBox _checklist = Field as CheckedListBox;
            CheckBox _checkbox = Field as CheckBox;
            RadioButton _radio = Field as RadioButton;

            int _slot = 0;
            int _number = 0;
            bool _isNumber = int.TryParse(Value, out _number);
            string _value = "";

            if (!AllUsedColumnsFinished)
            {
                UsedColumn[Column.ToLower()] = true;
            }

            switch (Column.ToLower())
            {
                case "races":
                    _checklist.SetItemChecked(0, ((_number & 0xFFFF) == 0xFFFF));
                    
                    for (_slot = 0; _slot < RaceNicks.Length; _slot++)
                    {
                        _checklist.SetItemChecked(_slot + 1, (_number & (1 << _slot)) != 0);
                    }
                    break;
                case "classes":
                    _checklist.SetItemChecked(0, ((_number & 0xFFFF) == 0xFFFF));
                    
                    for (_slot = 0; _slot < ClassNicks.Length; _slot++)
                    {
                        _checklist.SetItemChecked(_slot + 1, (_number & (1 << _slot)) != 0);
                    }
                    break;
                case "deity":
                    if (_number == 0x1FFFF)
                    {
                        _number = 0;
                    }
                    
                    _checklist.SetItemChecked(0, _number == 0);

                    for (_slot = 0; _slot < Deities.Length; _slot++)
                    {
                        _checklist.SetItemChecked(_slot + 1, (_number & (1 << _slot)) != 0);
                    }
                    break;
                case "weight":
                    _textbox.Text = Value.Replace(".", "");
                    break;
                case "flags":
                    foreach (string _flag in ItemFlagFields)
                    {
                        switch (_flag[0])
                        {
                            case '-': // nodrop/norent, but not nopet
                                _checklist.SetItemChecked(_slot++, IntField(_flag.Substring(1)) == 0);
                                break;
                            default:
                                _checklist.SetItemChecked(_slot++, IntField(_flag) != 0);
                                break;
                        }
                    }
                    break;
                case "slots":
                    // "All"
					_checklist.SetItemChecked(0, _number == 0x3FFFFF);

					for (_slot = 1; _slot < _checklist.Items.Count; _slot++)
					{
						_checklist.SetItemChecked(_slot,
							(_number & SlotFlags[_slot - 1]) != 0);
					}
					break;
                case "color":
                    long _tint = long.Parse(Value); // Would have gone with a uint, myself, but it's a long in the database
                    boxColorTint.BackColor = Color.FromArgb((int)_tint);
                    _tint &= 0x00FFFFFF;
                    _textbox.Text = _tint.ToString("X6");
                    break;
                case "factionmod1":
                    listEditFaction1.SelectedValue = _number;
                    textEditFactionMod1.Text = Value;
                    break;
                case "factionmod2":
                    listEditFaction2.SelectedValue = _number;
                    textEditFactionMod2.Text = Value;
                    break;
                case "factionmod3":
                    listEditFaction3.SelectedValue = _number;
                    textEditFactionMod3.Text = Value;
                    break;
                case "factionmod4":
                    listEditFaction4.SelectedValue = _number;
                    textEditFactionMod4.Text = Value;
                    break;
                case "faction1":
                case "faction2":
                case "faction3":
                case "faction4":
                    break;
                case "flags2":
                    _checklist.SetItemChecked(0, IntField("potionbeltslots") > 0);
                    _checklist.SetItemChecked(1, IntField("expendablearrow") > 0);
                    break;
                case "augtype":
                    _checklist.SetItemChecked(0, ((_number & 0x3FFFFFFF) == 0x3FFFFFFF));

                    for (_slot = 1; _slot < AugTypes.Length; _slot++)
                    {
                        _checklist.SetItemChecked(_slot, (_number & (1 << (_slot - 1))) != 0);
                    }
                    break;
                case "augdistiller":
                    ItemNames.TryGetValue(_number, out _value);
                    labelAugDistiller.Text = _value;
                    _textbox.Text = Value;
                    break;
                case "pointtype0":
                case "pointtype1":
                case "pointtype2":
                case "pointtype3":
                case "pointtype4":
                case "pointtype5":
                    _radio.Checked = IntField("pointtype") == int.Parse(Column.Substring(Column.Length - 1));
                    break;
                case "ldontheme":
                    for (_slot = 0; _slot < LDoNThemes.Length; _slot++)
                    {
                        _checklist.SetItemChecked(_slot, (_number & (1 << _slot)) != 0);
                    }
                    _checklist.Enabled = IntField("pointtype") == 1;
                    break;
                case "price":
                case "sellrate":
                case "ldonpoints":
                case "ldonsellbackrate":
                    _textbox.Text = Value;
                    UpdateCoinValues();
                    break;
                default:
                    if (_combobox != null)
                    {
                        if (_combobox.DropDownStyle == ComboBoxStyle.DropDown)
                        {
                            _combobox.Text = Value;
                        }
                        else if (_isNumber)
                        {
                            if (_combobox.ValueMember == "")
                            {
                                _combobox.SelectedIndex = _number;
                            }
                            else
                            {
                                _combobox.SelectedValue = _number;
                            }
                        }
                        else
                        {
                            try
                            {
                                _combobox.SelectedValue = Value;
                            }
                            catch
                            {
                                return DialogResult.Abort;
                            }
                        }
                    }
                    else if (_textbox != null)
                    {
                        _textbox.Text = Value;
                    }
                    else if (_checkbox != null)
                    {
                        _checkbox.Checked = !(_number == 0);
                    }
                    else
                    {
                        return DialogResult.Abort;
                    }
                    break;
            }

            return DialogResult.OK;
        }
        
        private DataRow Item_Load(int ItemID)
        {
            return GetData("SELECT *,0 AS `loregroupmembercount` FROM `items` WHERE `id`=" + ItemID.ToString() + ";").Rows[0];
        }

        private void Item_Preview(int ItemID)
        {
            if (!Item_ConfirmChange())
            {
                return;
            }

            if ((PreviewItem == null) || (OldItemID != ItemID))
            {
                if (ItemID > -1)
                {
                    PreviewItem = Item_Load(ItemID);

                    if (IntField("loregroup") > 0)
                    {
                        PreviewItem["loregroupmembercount"] = GetInt("SELECT COUNT(*) FROM `items` WHERE `loregroup`=" + TextField("loregroup") + ");");
                    }
                }
                else
                {
                    PreviewItem = null;
                }
            }

            Item = PreviewItem;
            OldItemID = ItemID;
            ChangedColumn.Clear();

            UpdatePreviewBox(-1);
        }

        private void Item_New(object sender, EventArgs e)
        {
			if (!Item_ConfirmChange())
            {
                return;
            }

            int _i = GetInt("SELECT MAX(`id`) FROM `items`;");
            if (_i < 1001)
            {
                _i = 1001;
            }
            else
            {
                _i++;
            }

            Item = GetData("SELECT *, 0 AS loregroupmembercount FROM `items` LIMIT 1;").Rows[0];

            foreach (DataColumn _col in Item.Table.Columns)
            {
                Item[_col.ColumnName] = SQLDefault(_col);
            }

            Item["id"] = _i;
            Item["name"] = "New Item #" + _i.ToString();
            Item["nodrop"] = 1; // Disable "No Trade" flag for new items
            Item["norent"] = 1; // Disable "Temporary" flag for new items
            Item["classes"] = 0xFFFF; // Class: All by default (mostly for non-armor)
            Item["races"] = 0xFFFF; // Race: All except Shroud by default (mostly for non-armor)
            Item["ldonsellbackrate"] = 70; // This appears to be the EQLive default
            Item["sellrate"] = 1.0f;
            Item["idfile"] = "IT63"; // In 3D space appears as a tiny bag
            Item["created"] = DateTime.UtcNow;

            // No spell effects (-1) by default
            Item["scrolleffect"] = -1;
            Item["clickeffect"] = -1;
            Item["proceffect"] = -1;
            Item["worneffect"] = -1;
            Item["focuseffect"] = -1;
            Item["bardeffect"] = -1;

            // VarChars in DB for some reason, but int fields.
            Item["charmfileid"] = "0";
            Item["combateffects"] = "0";

            switch (Item_Create())
            {
                case -1:
                    MessageBox.Show(this, "Error: There was a problem creating blank item # " + _i.ToString() + " in the Items table.", "Error Saving Item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Item = PreviewItem;
                    CheckEditableState();
                    break;
                case 0:
                    MessageBox.Show(this, "Weird: There was a glitch creating blank item # " + _i.ToString() + " in the Items table.\r\n\r\nThe database didn't report an error, but it did not insert the new item.", "Glitch Cloning Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Item = PreviewItem;
                    CheckEditableState();
                    break;
                default:
                    ChangedColumn.Clear();
                    ItemLoading = true;
                    listSearchItems.SelectedItems.Clear();
                    ItemLoading = false;
                    PreviewItem = Item;
                    Item = null;
                    Item_EditMode();
                    break;
            }
        }
        
        private DialogResult Item_Clone()
        {
			int _i, _j;
			string _text;

			_i = GetInt("SELECT MAX(`id`) FROM `items`;");
            if (_i < 1001)
            {
                _i = 1001;
            }
            else
            {
                _i++;
            }

            int _oldItemID = IntField("id"); // Might be different from OldItemID
            Item["id"] = _i;

            string _name = Item["name"].ToString();
            if (_name.EndsWith(")"))
            {
                _i = _name.LastIndexOf('(');
                if (_i >= 0)
                {
                    _text = _name.Substring(_i + 1, _name.Length - _i - 2);
                    if (int.TryParse(_text, out _j))
                    {
                        _j++;
                        Item["name"] = _name.Substring(0, _i + 1) + _j.ToString() + ")";
                    }
                }

            }
            else
            {
                Item["name"] = _name + " (2)";
            }

            switch (Item_Create())
            {
                case -1:
                    MessageBox.Show(this, "Error: There was a problem cloning item # " + _oldItemID.ToString() + " as item # " + Item["id"].ToString() + " in the Items table.", "Error Saving Item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Item["id"] = _oldItemID;
                    return DialogResult.Abort;
                case 0:
                    MessageBox.Show(this, "Weird: There was a glitch cloning item # " + _oldItemID.ToString() + " as item # " + Item["id"].ToString() + " in the Items table.\r\n\r\nThe database didn't report an error, but it did not insert the cloned item.", "Glitch Cloning Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Item["id"] = _oldItemID;
                    return DialogResult.Abort;
                default:
                    return DialogResult.OK;
            }
        }

        private int Item_Create()
        {
            StringBuilder _columns = new StringBuilder();
            StringBuilder _values = new StringBuilder();
            bool _first = true;
            
            _columns.Length = 0;
            _columns.Append("INSERT INTO `items` (");

            _values.Append(" VALUES (");

            foreach (DataColumn _col in Item.Table.Columns)
            {
                // Skip our member count function returned with the SELECT statement.
                if (!_col.ColumnName.Equals("loregroupmembercount", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (_first)
                    {
                        _first = false;
                    }
                    else
                    {
                        _columns.Append(", ");
                        _values.Append(", ");
                    }

                    _columns.Append('`');
                    _columns.Append(_col.ColumnName);
                    _columns.Append('`');

                    _values.Append(SQLEncode(_col, Item[_col.ColumnName]));
                }
            }

            _columns.Append(')');
            _values.Append(");");

            _columns.Append(_values);

            return Execute(_columns.ToString());
        }
        
        private void Item_Save(object sender, EventArgs e)
        {
            if (ChangedColumn.Count < 1)
            {
                CheckEditableState();

                return;
            }

            if (ChangedColumn.ContainsKey("id"))
            {
                DataRow _olditem = GetData("SELECT `id`,`name` FROM `items` WHERE `id`=" + Item["id"].ToString()).Rows[0];

                if (_olditem["id"] != DBNull.Value)
                {
                    if (MessageBox.Show(this, "NOTE: You have changed the item's ID to " + Item["id"].ToString() + ", and there is already an item with that ID:\r\n\r\n" + _olditem["name"].ToString() + "\r\n\r\nDo you wish to overwrite that item with this one?", "Overwrite Item Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                    {
                        return;
                    }
                    else
                    {
                        if (Execute("DELETE FROM `items` WHERE `id`=" + Item["id"].ToString()) == -1)
                        {
                            MessageBox.Show("Error: There was a problem removing the old item # " + Item["id"].ToString() + " to replace it with your new one.");
                        }
                    }
                }
            }

            ItemLine.Length = 0;
            ItemLine.Append("UPDATE `items` SET ");

            foreach (string _col in ChangedColumn.Keys)
            {
                if (ItemLine.Length > 19)
                {
                    ItemLine.Append(", ");
                }
                ItemLine.Append('`');
                ItemLine.Append(_col);
                ItemLine.Append("`=");
                ItemLine.Append(SQLEncode(Item.Table.Columns[_col], Item[_col]));
            }

            ItemLine.Append(" WHERE `id`=");
            ItemLine.Append(OldItemID.ToString());
            ItemLine.Append(';');

            switch (Execute(ItemLine.ToString()))
            {
                case -1:
                    MessageBox.Show(this, "Error: There was a problem saving this as item # " + Item["id"].ToString() + " to the Items table.", "Error Saving Item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case 0:
                    MessageBox.Show(this, "Weird: There was a glitch saving this as item # " + Item["id"].ToString() + " to the Items table.\r\n\r\nThe database said it couldn't find that item, so it did nothing.", "Glitch Saving Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                default:
                    OldItemID = IntField("id");
                    ChangedColumn.Clear();
                    CheckEditableState();
                    buttonSearchName_Click(buttonSearchName, null);
					listSearchItems.Items["item" + TextField("id")].EnsureVisible();
					listSearchItems.Items["item" + TextField("id")].Selected = true;
                    break;
            }
        }

        private DialogResult Item_Delete()
        {
            switch (Execute("DELETE FROM `items` WHERE `id`=" + OldItemID.ToString()))
            {
                case -1:
                    MessageBox.Show(this, "Error: There was a problem deleting item # " + OldItemID.ToString() + " from the Items table.", "Error Deleting Item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return DialogResult.Abort;
                case 0:
                    MessageBox.Show(this, "Weird: There was a glitch deleting item # " + OldItemID.ToString() + " from the Items table.\r\n\r\nThe database said it couldn't find that item, so it did nothing.", "Glitch Saving Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return DialogResult.Abort;
                default:
                    return DialogResult.OK;
            }
        }

        private void LoadItemIcons()
        {
            try
            {
                string[] _categories = Directory.GetDirectories(Environment.ExpandEnvironmentVariables(Settings.IconFolder));

                IconCategories.Clear();
                CategoryOfIcon.Clear();
                IconsInCategory.Clear();
                itemIcons.Images.Clear();

                IconCategories.Add("");

                // TODO: TGA support
                List<string> _iconExtensions = new List<string>(new string[] {".png", ".gif", ".bmp", ".jpg", ".jpeg" });

                string _category;
                string[] _icons;
                string _iconPath;
                int _iconNum;

                for (int _catindex = 0; _catindex <= _categories.Length; _catindex++)
                {
                    if (_catindex == 0)
                    {
                        _iconPath = Environment.ExpandEnvironmentVariables(Settings.IconFolder);
                    }
                    else
                    {
                        _iconPath = _categories[_catindex - 1];
                    }

                    _icons = Directory.GetFiles(_iconPath);

                    _category = _iconPath.Substring(_iconPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                    if (_catindex > 0)
                    {
                        IconCategories.Add(_category);
                    }
                    
                    List<int> _iconsInCat = new List<int>();

                    foreach (string _icon in _icons)
                    {
                        if (_iconExtensions.Contains(Path.GetExtension(_icon).ToLower()))
                        {
                            string _justNum = Path.GetFileNameWithoutExtension(_icon);
                            if (int.TryParse(_justNum, out _iconNum))
                            {
                                // Last Icon: Titanium = 1700, SoF = 2253, RoF2 = 6898
                                if ((_iconNum >= 500))// && (_iconNum <= 2253))
                                {
                                    CategoryOfIcon[_iconNum] = _catindex;
                                    _iconsInCat.Add(_iconNum);

                                    Image _iconImage = new Bitmap(_icon);
                                    IconImages[_iconNum] = _iconImage;

                                    itemIcons.Images.Add(_justNum, _iconImage);
                                }
                            }
                        }
                    }

                    IconsInCategory.Add(_iconsInCat.ToArray());
                }

                if (itemIcons.Images.Count > 0)
                {
                    iconPreview.Image = itemIcons.Images[itemIcons.Images.Count - 1];
                }
            }
            catch
            {
                MessageBox.Show("Icon folder not found. Item Icons will not be available.");
            }
        }

        private void ResetOptionFields()
        {
            Settings.Load();

            labelIconFolder.Text = Settings.IconFolder;
			checkAutoSetClasses.Checked = Settings.AutoClasses;
			checkAutoSet3DModel.Checked = Settings.Auto3DModel;

            textDBConn.Text = Settings.Database.Conn;
            textDBHost.Text = Settings.Database.Host;
            textDBName.Text = Settings.Database.Name;
            textDBUser.Text = Settings.Database.User;
            textDBPass.Text = Settings.Database.Pass;
            textDBPort.Text = Settings.Database.Port;
        }

        private void SetHandler(Control Field)
        {
            if (Field.HasChildren)
            {
                foreach (Control _child in Field.Controls)
                {
                    SetHandler(_child);
                }
            }
            else
            {
                TextBox _textbox = Field as TextBox;
                ComboBox _combobox = Field as ComboBox;
                CheckedListBox _checklist = Field as CheckedListBox;
                CheckBox _checkbox = Field as CheckBox;

                if (_textbox != null)
                {
                    _textbox.TextChanged += this.formTextbox_Changed;
                }
                else if (_combobox != null)
                {
                    _combobox.SelectedIndexChanged += this.formDropDown_Changed;
                    if (_combobox.DropDownStyle != ComboBoxStyle.DropDownList)
                    {
                        _combobox.TextChanged += this.formDropDown_Changed;
                    }
                }
                else if (_checklist != null)
                {
                    _checklist.ItemCheck += this.formCheckedListBox_ItemCheck;
                }
                else if (_checkbox != null)
                {
                    _checkbox.CheckedChanged += this.formCheckbox_CheckChanged;
                }
            }
        }
        
        private string TrimComma(string Text)
        {
            if (Text.EndsWith(", "))
            {
                return Text.Substring(0, Text.Length - 2);
            }

            return Text;
        }

        private void UpdateCoinValues()
        {
            int _coins = IntField("price");

            labelPrice1c.Text = (_coins % 10).ToString() + " cp";
            _coins /= 10;
            labelPrice1s.Text = (_coins % 10).ToString() + " sp";
            _coins /= 10;
            labelPrice1g.Text = (_coins % 10).ToString() + " gp";
            _coins /= 10;
            labelPrice1p.Text = _coins.ToString() + " pp";

            _coins = (int)((float)IntField("price") * FloatField("sellrate"));

            labelPrice2c.Text = (_coins % 10).ToString() + " cp";
            _coins /= 10;
            labelPrice2s.Text = (_coins % 10).ToString() + " sp";
            _coins /= 10;
            labelPrice2g.Text = (_coins % 10).ToString() + " gp";
            _coins /= 10;
            labelPrice2p.Text = _coins.ToString() + " pp";

            _coins = (int)((float)IntField("ldonprice") * (FloatField("ldonsellbackrate")/100.0f));

            labelSellPoints.Text = _coins.ToString() + " Point" + ((_coins == 1) ? "" : "s");
        }
        
        public void UpdatePreviewBox(int BoxNumber) { UpdatePreviewBox(BoxNumber, false); }
        public void UpdatePreviewBox(int BoxNumber, bool SkipLayout)
        {
            if (BoxNumber < 0)
            {
                // UpdatePreviewBox(-1) == Update All Preview Boxes
                iconPreview.Image = null;
                UpdatePreviewBox(99);

                for (BoxNumber = 0; BoxNumber < 20; BoxNumber++)
                {
                    UpdatePreviewBox(BoxNumber, true);
                }
            }
            else if (BoxNumber >= 100)
            {
                // UpdatePreviewBox(208) == Update(2) and Update(8)
                
                UpdatePreviewBox(BoxNumber / 100, true);
                UpdatePreviewBox(BoxNumber % 100, true);
            }
            else
            {
                Label _label = (Label)itemPreview.Controls["labelPreviewBox" + BoxNumber.ToString()];
                Label _textbox = (Label)itemPreview.Controls["textPreviewBox" + BoxNumber.ToString()];

                if (_label != null)
                {
                    _label.Text = "";
                }

                if (_textbox != null)
                {
                    _textbox.Text = "";
                }

                if (PreviewItem != null)
                {
                    switch (BoxNumber)
                    {
                        case 0:
                            UpdatePreviewBox0(); // Basics: ID, Name, Icon, ItemType. ItemType change requires ALL updated => UpdatePreviewBox(-1);
                            break;
                        case 1:
                            UpdatePreviewBox1(); // Under name: Flags, Races, Classes, Deities, Slots
                            break;
                        case 2:
                            UpdatePreviewBox2(); // Upper-left. Size, Weight, Container Stats, Rec/Req Levels, Weapon Type
                            break;
                        case 3:
                            UpdatePreviewBox3(); // Upper-middle: AC/HP/Mana/Endurance/Purity
                            break;
                        case 4:
                            UpdatePreviewBox4(); // Upper-right: Weapon Damages and Delay
                            break;
                        case 5:
                            UpdatePreviewBox5(); // Lower-left: Character stats
                            break;
                        case 6:
                            UpdatePreviewBox6(); // Lower-middle: Character Resists
                            break;
                        case 7:
                            UpdatePreviewBox7(); // Lower-right: mod2s
                            break;
                        case 8:
                            UpdatePreviewBox8(); // Augmentation Slots, Lore Group, Container Type
                            break;
                        case 9:
                            UpdatePreviewBox9(); // Spell Effects (Click, Combat, Worn, Focus, Instruments)
                            break;
                        case 10:
                            UpdatePreviewBox10(); // Misc: Slots an augment fits into, Charges, Tradeskills, Food/Drink, Bane DMG, Augment Restriction/Solvent
                            break;
                        case 11:
                            UpdatePreviewBox11();
                            break;
                        case 12:
                            UpdatePreviewBox12();
                            break;
                    }
                }
            }

            if (!SkipLayout)
            {
                labelPreviewBox2.Top = Math.Max(104, textPreviewBox1.Bottom + 15);
                textPreviewBox2.Top = labelPreviewBox2.Top;
                labelPreviewBox3.Top = labelPreviewBox2.Top;
                textPreviewBox3.Top = labelPreviewBox3.Top;
                labelPreviewBox4.Top = labelPreviewBox2.Top;
                textPreviewBox4.Top = labelPreviewBox4.Top;

                labelPreviewBox5.Top = Math.Max(Math.Max(labelPreviewBox2.Bottom, labelPreviewBox3.Bottom), labelPreviewBox4.Bottom) + 15;
                textPreviewBox5.Top = labelPreviewBox5.Top;
                textPreviewBox51.Top = textPreviewBox5.Top;
                labelPreviewBox6.Top = labelPreviewBox5.Top;
                textPreviewBox6.Top = labelPreviewBox6.Top;
                textPreviewBox61.Top = textPreviewBox6.Top;
                labelPreviewBox7.Top = labelPreviewBox5.Top;
                textPreviewBox7.Top = labelPreviewBox7.Top;

                textPreviewBox8.Top = Math.Max(Math.Max(labelPreviewBox5.Bottom, labelPreviewBox6.Bottom), labelPreviewBox7.Bottom) + 15;
                textPreviewBox9.Top = textPreviewBox8.Bottom + 15;
                textPreviewBox10.Top = textPreviewBox9.Bottom  - ((textPreviewBox9.Text.Length  == 0) ? 15 : 0);
                textPreviewBox11.Top = textPreviewBox10.Bottom - ((textPreviewBox10.Text.Length == 0) ? 15 : 0);
                textPreviewBox12.Top = textPreviewBox11.Bottom - ((textPreviewBox11.Text.Length == 0) ? 15 : 0);
                
                textPreviewBox99.Top = textPreviewBox12.Bottom; // -((textPreviewBox12.Text.Length == 0) ? 15 : 0);

                CheckEditableState();
            }
        }

        // Basics: ID, Name, Icon, ItemType
        private void UpdatePreviewBox0()
        {
			int _i;

			try
            {
                iconPreview.Image = itemIcons.Images[TextField("icon")];
            }
            catch
            {
                iconPreview.Image = null;
            }
            
            textPreviewBox0.Text = TextField("name") + " (" + TextField("id") + ")";

            switch (_i = IntField("itemtype"))
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 7:
                case 19:
                case 27:
                case 35:
                case 45:
                    ItemGroup = ItemGroups.Weapon;
                    break;
                case 10:
                case 29:
                case 52:
                    ItemGroup = ItemGroups.Armor;
                    break;
                case 21:
                    ItemGroup = ItemGroups.Potion;
                    break;
                case 23:
                case 24:
                case 25:
                case 26:
                case 50:
                case 51:
                    ItemGroup = ItemGroups.Instrument;
                    break;
                case 14:
                case 15:
                    ItemGroup = ItemGroups.FoodDrink;
                    break;
                case 54:
                    ItemGroup = ItemGroups.Augmentation;
                    break;
                default:
                    if (IntField("bagslots") > 0)
                    {
                        ItemGroup = ItemGroups.Container;
                    }
                    else if (IntField("benefitflag") > 0)
                    {
                        ItemGroup = ItemGroups.Benefit;
                    }
                    else if ((_i == 0) && (IntField("delay") > 0))
                    {
                        ItemGroup = ItemGroups.Weapon;
                    }
                    else
                    {
                        ItemGroup = ItemGroups.Unknown;
                    }
                    break;
            }
        }

        // Under name: Flags, Evolving, Races, Classes, Deities, Slots
        private void UpdatePreviewBox1()
        {
			int _i, _j;
			string _text;
			string _text2;
			string _text3;

			ItemLine.Length = 0;
            AppendIf("magic", "Magic, ");
            AppendIf("loregroup", "Lore, ");
            AppendIf("artifactflag", "Artifact, ");
            AppendIf("fvnodrop", "No Trade +FV, ");
            AppendIf((IntField("fvnodrop") == 0) && (IntField("nodrop") == 0), "No Trade, ");
            AppendIf("notransfer", "No Transfer, ");
            AppendIf(IntField("norent") == 0, "Temporary, ");
            AppendIf("summonedflag", "Summoned, ");
            AppendIf("questitemflag", "Quest, ");
            AppendIf("nopet", "No Pets, ");
            AppendIf("attuneable", "Attuneable, ");
            AppendIf(ItemGroup == ItemGroups.Augmentation, "Augmentation, ");
            AppendIf(IntField("benefitflag") == 1, "Benefit, ");
            AppendIf(IntField("expendablearrow") > 0, "Expendable, ");

            AddValue(1, TrimComma(ItemLine.ToString()));

            if ((_i = IntField("evolvinglevel")) > 0)
            {
                _text = GetData("SELECT `id`, `name` FROM `items` WHERE `evolvinglevel` > 0 AND `loregroup` = " + TextField("loregroup") + " ORDER BY `evolvinglevel` DESC LIMIT 1;").Rows[0][0].ToString();

                if (_text != "")
                {
                    _text2 = " Final Result: #" + _text;
                    if (ItemNames.TryGetValue(int.Parse(_text), out _text3))
                    {
                        _text2 += ", " + _text3;
                    }
                }
                else
                {
                    _text2 = "";
                }

                AddValue(1, "Evolving " + _i.ToString() + "/" + TextField("loregroupmembercount") + _text2);
            }

            // Classes
            if (IntField("itemtype") != 20)
            {
                switch (_i = IntField("classes"))
                {
                    case 0:
                        _text = "Class: None";
                        break;
                    case 65535:
                        _text = ""; //(ItemGroup == ItemGroups.Unknown) ? "" : "Class: ALL";
                        break;
                    default:
                        ItemLine.Length = 0;
                        ItemLine.Append("Class:");
                        for (_j = 0; _j < ClassNicks.Length; _j++)
                        {
                            if ((_i & (1 << _j)) != 0)
                            {
                                ItemLine.Append(" ");
                                ItemLine.Append(ClassNicks[_j]);
                            }
                        }
                        _text = ItemLine.ToString();

                        ItemLine.Length = 0;
                        ItemLine.Append("Class: All except");
                        for (_j = 0; _j < ClassNicks.Length; _j++)
                        {
                            if ((_i & (1 << _j)) == 0)
                            {
                                ItemLine.Append(" ");
                                ItemLine.Append(ClassNicks[_j]);
                            }
                        }
                        _text2 = ItemLine.ToString();

                        if ((_text.Length + 11) > _text2.Length)
                        {
                            _text = _text2;
                        }
                        break;
                }
                AddValue(1, _text);
            }

            // Races
            switch (_i = IntField("races"))
            {
                case 0:
                    _text = "Race: None";
                    break;
                case 65535:
                    _text = ""; //(ItemGroup == ItemGroups.Unknown) ? "" : "Race: ALL";
                    break;
                case 65536:
                    _text = "Race: Shroud";
                    break;
                case 131071:
                    _text = "Race: ALL + Shroud";
                    break;
                default:
                    ItemLine.Length = 0;
                    ItemLine.Append("Race:");
                    for (_j = 0; _j < RaceNicks.Length; _j++)
                    {
                        if ((_i & (1 << _j)) != 0)
                        {
                            ItemLine.Append(" ");
                            ItemLine.Append(RaceNicks[_j]);
                        }
                    }
                    _text = ItemLine.ToString();

                    ItemLine.Length = 0;
                    if ((_i & 0x10000) != 0)
                    {
                        ItemLine.Append("Race: All + Shroud except");
                    }
                    else
                    {
                        ItemLine.Append("Race: All except");
                    }

                    for (_j = 0; _j < (RaceNicks.Length - 1); _j++)
                    {
                        if ((_i & (1 << _j)) == 0)
                        {
                            ItemLine.Append(" ");
                            ItemLine.Append(RaceNicks[_j]);
                        }
                    }
                    _text2 = ItemLine.ToString();

                    if ((_text.Length + 11) > _text2.Length)
                    {
                        _text = _text2;
                    }
                    break;
            }
            AddValue(1, _text);

            // Deities
            switch (_i = IntField("deity"))
            {
                case 0:
                case 131071:
                    _text = "";
                    break;
                default:
                    ItemLine.Length = 0;
                    ItemLine.Append("Deity: ");
                    for (_j = 0; _j < Deities.Length; _j++)
                    {
                        if ((_i & (1 << _j)) != 0)
                        {
                            ItemLine.Append(Deities[_j]);
                            ItemLine.Append(", ");
                        }
                    }
                    _text = ItemLine.ToString();

                    ItemLine.Length = 0;
                    ItemLine.Append("Deity: All except ");
                    for (_j = 0; _j < Deities.Length; _j++)
                    {
                        if ((_i & (1 << _j)) == 0)
                        {
                            ItemLine.Append(Deities[_j]);
                            ItemLine.Append(", ");
                        }
                    }
                    _text2 = ItemLine.ToString();

                    if ((_text.Length + 11) > _text2.Length)
                    {
                        _text = _text2;
                    }

                    _text = TrimComma(_text);
                    break;
            }
            AddValue(1, _text);

            // Slots
            switch (_i = IntField("slots"))
            {
                case 0:
                    _text = "";
                    break;
                default:
                    ItemLine.Length = 0;
                    for (_j = 0; _j < SlotNames.Length; _j++)
                    {
                        switch (_j)
                        {
                            case 4:
                            case 10:
                            case 16:
                                break;
                            default:
                                if ((_i & (1 << _j)) != 0)
                                {
                                    ItemLine.Append(SlotNames[_j]);
                                    ItemLine.Append(", ");
                                }
                                break;
                        }
                    }
                    _text = TrimComma(ItemLine.ToString());
                    break;
            }
            AddValue(1, _text);
        }

        // Upper-left: Size, Weight, Container Stats, Rec/Req Levels, Weapon Type
        private void UpdatePreviewBox2()
        {
			int _i;
			string _text;

			_i = IntField("size");
            AddField(2, "Size:", "", SizeNames[((_i < 0) || (_i > 5)) ? 5 : _i]);

            if ((ItemGroup != ItemGroups.Augmentation) && (ItemGroup != ItemGroups.Benefit))
            {
                _i = IntField("weight");
                if (_i != 0)
                {
                    AddField(2, "Weight:", "", (_i / 10).ToString() + "." + (_i % 10).ToString());
                }
            }

            bool _isContainer = IntField("bagslots") > 0;
            
            if (_isContainer)
            {
                AddField(2, "Weight Red:", "", TextField("bagwr") + " %");
                AddField(2, "Capacity:", "bagslots");
                _i = IntField("bagsize");
                AddField(2, "Size Cap:", "", SizeNames[((_i < 0) || (_i > 5)) ? 5 : _i]);
            }

            AddField(2, "Rec Level:", "RecLevel");
            AddField(2, "Req Level:", "ReqLevel");
            AddField(2, "Rec Skill:", "RecSkill");

            if ((ItemGroup == ItemGroups.Weapon) || (ItemGroup == ItemGroups.Instrument))
            {
                _text = ItemTypes[IntField("itemtype")];
                if ((_i = _text.IndexOf(" - ")) >= 0)
                {
                    _text = _text.Substring(0, _i);
                }

                AddField(2, "Skill:", "", _text);
            }

            listEditBagSize.Visible = 
                labelEditBagWR.Visible = 
                    textEditBagWR.Visible = 
                        listEditBagType.Visible = _isContainer;
        }

        // Upper-middle: AC, HP, Mana, Endurance, Purity, Haste
        private void UpdatePreviewBox3()
        {
			string _text;

			AddField(3, "AC:");
            AddField(3, "HP:");
            AddField(3, "Mana:");
            AddField(3, "End:", "Endur");
            AddField(3, "Purity:");
            if ((_text = TextField("haste")) != "0")
            {
                AddField(3, "Haste:", "", _text + "%");
            }
        }

        // Upper-right: Weapon Damage, Delay, Range
        private void UpdatePreviewBox4()
        {
            AddField(4, "Base Dmg:", "damage");
			int _i;

            _i = IntField("elemdmgtype");
            if ((_i > 0) && (_i < Resists.Length))
            {
                AddField(4, Resists[_i] + " Dmg:", "elemdmgamt");
            }

            AddField(4, "Backstab Dmg:", "backstabdmg");

            AddField(4, "Delay:", "delay");

            AddField(4, "Range:", "range");
        }

        // Lower-left: Character stats
        private void UpdatePreviewBox5()
        {
			string _text;

			AddField(5, "Strength:", "astr");
            AddField(5, "Stamina:", "asta");
            AddField(5, "Intelligence:", "aint");
            AddField(5, "Wisdom:", "awis");
            AddField(5, "Agility:", "aagi");
            AddField(5, "Dexterity:", "adex");
            AddField(5, "Charisma:", "acha");

            textPreviewBox51.Text = "";
            textPreviewBox51.Text += (((_text = TextField("heroic_str")) == "0") ? "" : "+" + _text) + "\r\n";
            textPreviewBox51.Text += (((_text = TextField("heroic_sta")) == "0") ? "" : "+" + _text) + "\r\n";
            textPreviewBox51.Text += (((_text = TextField("heroic_int")) == "0") ? "" : "+" + _text) + "\r\n";
            textPreviewBox51.Text += (((_text = TextField("heroic_wis")) == "0") ? "" : "+" + _text) + "\r\n";
            textPreviewBox51.Text += (((_text = TextField("heroic_agi")) == "0") ? "" : "+" + _text) + "\r\n";
            textPreviewBox51.Text += (((_text = TextField("heroic_dex")) == "0") ? "" : "+" + _text) + "\r\n";
            textPreviewBox51.Text += (((_text = TextField("heroic_cha")) == "0") ? "" : "+" + _text);
        }

        // Lower-middle: Character Resists
        private void UpdatePreviewBox6()
        {
			string _text;

			AddField(6, "Magic:", "MR");
            AddField(6, "Fire:", "FR");
            AddField(6, "Cold:", "CR");
            AddField(6, "Disease:", "DR");
            AddField(6, "Poison:", "PR");
            AddField(6, "Corrupt:", "svcorruption");

            textPreviewBox61.Text = "";
            textPreviewBox61.Text += (((_text = TextField("heroic_mr")) == "0") ? "" : "+" + _text) + "\r\n";
            textPreviewBox61.Text += (((_text = TextField("heroic_fr")) == "0") ? "" : "+" + _text) + "\r\n";
            textPreviewBox61.Text += (((_text = TextField("heroic_cr")) == "0") ? "" : "+" + _text) + "\r\n";
            textPreviewBox61.Text += (((_text = TextField("heroic_dr")) == "0") ? "" : "+" + _text) + "\r\n";
            textPreviewBox61.Text += (((_text = TextField("heroic_pr")) == "0") ? "" : "+" + _text) + "\r\n";
            textPreviewBox61.Text += (((_text = TextField("heroic_svcorrup")) == "0") ? "" : "+" + _text);
        }

        // Lower-right: mod2s
        private void UpdatePreviewBox7()
        {
			int _i, _j;

			AddField(7, "Attack:");
            AddField(7, "HP Regen:", "regen");
            AddField(7, "Mana Regen:", "manaregen");
            AddField(7, "Endur Regen:", "enduranceregen");
            AddField(7, "Spell Shield:", "spellshield");
            AddField(7, "Combat Eff:", "combateffects");
            AddField(7, "Shielding:");
            AddField(7, "Dmg Shield:", "damageshield");
            AddField(7, "DoT Shield:", "dotshielding");
            AddField(7, "Dmg Shld Mit:", "dsmitigation");
            AddField(7, "Avoidance:");
            AddField(7, "Accuracy:");
            AddField(7, "Stun Resist:", "stunresist");
            AddField(7, "Strike Thr:", "strikethrough");
            AddField(7, "Heal Amount:", "healamt");
            AddField(7, "Spell Dmg:", "spelldmg");
            AddField(7, "Clairvoyance:");
            AddField(7, "Potion Slots:", "potionbeltslots");

            if ((_j = IntField("extradmgamt")) > 0)
            {
                _i = IntField("extradmgskill");

                if ((_i < 0) || (_i >= Skills.Length))
                {
                    AddField(7, "UnkSkill:", "", _j.ToString());
                }
                else
                {
                    AddField(7, Skills[_i] + ":", "", _j.ToString());
                }
            }
        }

        // Faction Modifiers, Augmentation Slots, Lore Group, Container Type, Lore Text
        private void UpdatePreviewBox8()
        {
			int _i, _j = 0;
			string _text;
			string _text2;

			for (_i = 1; _i <= 4; _i++)
            {
                if ((_text = TextField("factionmod" + _i.ToString())) != "0")
                {
                    ItemLine.Length = 0;
                    ItemLine.Append("Faction ");
                    _j = int.Parse(_text);
                    
                    ItemLine.Append(FactionNames.TryGetValue(_j, out _text2) ? _text2 : "#" + _text);

                    if ((_j = IntField("factionamt" + _i.ToString())) < 0)
                    {
                        ItemLine.Append(" - ");
                        ItemLine.Append(-_j);
                    }
                    else
                    {
                        ItemLine.Append(" + ");
                        ItemLine.Append(_j);
                    }

                    AddValue(8, ItemLine.ToString());
                }
            }

            if (_j != 0) // We had at least one faction modifier
            {
                AddValue(8, " ");
            }

            _j = 0;
            for (_i = 1; _i < 7; _i++)
            {
                string _slot = _i.ToString();

                int _slotType = IntField("augslot" + _slot + "type");
                int _slotVisible = IntField("augslot" + _slot + "visible");

                if ((_slotVisible != 0) && (_slotType != 0))
                {
                    ItemLine.Length = 0;
                    ItemLine.Append("[   ] Slot ");
                    ItemLine.Append(_slot);
                    ItemLine.Append(", type ");
                    if ((_slotType < 0) || (_slotType >= AugTypes.Length))
                    {
                        ItemLine.Append(_slotType);
                        ItemLine.Append(" (Unknown)");
                    }
                    else
                    {
                        ItemLine.Append(AugTypes[_slotType]);
                    }

                    AddValue(8, ItemLine.ToString());
                    _j = 1;
                }
            }

            if (_j != 0) // We had at least one aug slot
            {
                AddValue(8, " ");
            }

            if ((_i = IntField("loregroup")) > 0)
            {
                ItemLine.Length = 0;
                ItemLine.Append((IntField("evolvinglevel") < 1) ? "Lore" : "Evolving");
                ItemLine.Append(" Group: #");
                ItemLine.Append(_i);

                if ((_text2 = GetText("SELECT `string` FROM `dbstr` WHERE `type`=7 AND `id`=" + _i.ToString())) != "")
                {
                    ItemLine.Append(", ");
                    ItemLine.Append(_text2);
                }

                ItemLine.Append(" (");
                ItemLine.Append(TextField("loregroupmembercount"));
                ItemLine.Append(" Members)");
                
                AddValue(8, ItemLine.ToString(), true);
            }

            if (ItemGroup == ItemGroups.Container)
            {
                _i = IntField("bagtype");
                if ((_i < 0) || (_i >= BagTypes.Length))
                {
                    AddValue(8, "Container Type: Unknown (" + _i.ToString() + ")");
                }
                else
                {
                    AddValue(8, "Container Type: " + BagTypes[_i]);
                }
            }

            if ((_i = IntField("skillmodvalue")) != 0)
            {
                _text = Skills[IntField("skillmodtype")];
                if (_text == "Armor")
                {
                    _text = "Defense";
                }

                AddValue(8, "Skill Mod: " + _text + ((_i < 0) ? " " : " +") + _i.ToString() + " %");
            }

            if ((_text = TextField("lore")) != "")
            {
                if (!_text.Equals(TextField("name"), StringComparison.CurrentCulture))
                {
                    AddValue(8, "Lore: " + _text, true);
                }
            }
        }

        // Spell Effects (Scroll, Click, Combat, Worn, Focus, Instruments)
        private void UpdatePreviewBox9()
        {
			int _i, _j;
			string _text;
			string _text2;
			string _text3;

			// Clear spell effects descriptions
            UpdatePreviewBox(99, true);
            
            if ((_i = IntField("scrolleffect")) > 0)
            {
                DataRow _spelllevels = GetData("SELECT `classes1`,`classes2`,`classes3`,`classes4`,`classes5`,`classes6`,`classes7`,`classes8`,`classes9`,`classes10`,`classes11`,`classes12`,`classes13`,`classes14`,`classes15`,`classes16` FROM `spells_new` WHERE `id`=" + _i.ToString()).Rows[0];

                if ((_spelllevels.Table.Columns.Count == 16) && (_spelllevels[0] != DBNull.Value))
                {
                    SpellDescs.TryGetValue(_i, out _text3);

                    ItemLine.Length = 0;
                    ItemLine.Append("Level Needed:");
                    _i = IntField("classes");
                    for (_j = 0; _j < ClassNicks.Length; _j++)
                    {
                        if ((_i & (1 << _j)) != 0)
                        {
                            ItemLine.Append(' ');
                            ItemLine.Append(ClassNicks[_j]);
                            ItemLine.Append('(');
                            if (_spelllevels.Table.Columns.Count > _j)
                            {
                                if (_spelllevels[_j] == DBNull.Value)
                                {
                                    ItemLine.Append("UNK");
                                }
                                else
                                {
                                    ItemLine.Append(_spelllevels[_j]);
                                }
                            }
                            ItemLine.Append(')');
                        }
                    }

                    if (ItemLine.Length > 13)
                    {
                        AddValue(9, ItemLine.ToString(), true);
                    }

                    if (_text3 != null)
                    {
                        AddValue(9, "Spell Effect: " + _text3);
                    }
                }
            }
            
            if ((_i = IntField("clickeffect")) > 0)
            {
                if ((_text2 = TextField("clickname")) == "")
                {
                    if (!SpellNames.TryGetValue(_i, out _text2))
                    {
                        _text2 = "Spell #" + _i.ToString();
                    }
                }

                switch (IntField("clicktype"))
                {
                    case 1:
                    case 3: // 3 == Expendable (if maxcharges>0)
                        _text = " (Any Slot, Casting Time: ";
                        break;
                    case 4:
                        _text = " (Must Equip. Casting Time: ";
                        break;
                    case 5: // Race/Class/Deity restricted
                        _text = " (Any Slot/Can Equip, Casting Time: ";
                        break;
                    default:
                        _text = " (Casting Time: ";
                        break;
                }
                
                _j = IntField("casttime");
                _text += (_j == 0) ? "Instant)" : (((((float)_j) / 1000.0).ToString("n1")) + ")");

                if ((_j = IntField("clicklevel")) > 1)
                {
                    _text += " As Level " + _j.ToString();
                }

                AddValue(9, "Effect: " + _text2 + _text);

                if ((_j = IntField("recastdelay")) > 0)
                {
                    ItemLine.Length = 0;
                    ItemLine.Append("Recast Delay: ");
                    if ((_j % 3600) == 0)
                    {
                        ItemLine.Append(_j / 3600);
                        ItemLine.Append(" hour");
                        if (_j > 3600)
                        {
                            ItemLine.Append("s");
                        }
                    }
                    else if ((_j % 60) == 0)
                    {
                        ItemLine.Append(_j / 60);
                        ItemLine.Append(" minute");
                        if (_j > 60)
                        {
                            ItemLine.Append("s");
                        }
                    }
                    else if ((_j % 6) == 0)
                    {
                        ItemLine.Append(((float)_j / 6.0).ToString("n1"));
                        ItemLine.Append(" minute");
                        if (_j > 60)
                        {
                            ItemLine.Append("s");
                        }
                    }
                    else
                    {
                        ItemLine.Append(_j);
                        ItemLine.Append(" second");
                        if (_j > 1)
                        {
                            ItemLine.Append("s");
                        }
                    }

                    if ((_j = IntField("recasttype")) > 0)
                    {
                        ItemLine.Append(", Recast Timer: ");
                        ItemLine.Append(_j);
                    }

                    AddValue(9, ItemLine.ToString());
                }

                _j = IntField("clicklevel2");
                SpellDescs.TryGetValue(_i, out _text3);

                if ((_j > 1) || (_text3 != null))
                {
                    AddValue(99, _text2, true);
                    if (_j > 1)
                    {
                        AddValue(99, "Required Level: " + _j.ToString());
                    }
                    if (_text3 != "")
                    {
                        AddValue(99, "Spell Effect: " + _text3);
                    }
                }
            }
            
            if ((_i = IntField("proceffect")) > 0)
            {
                if ((_text2 = TextField("procname")) == "")
                {
                    if (!SpellNames.TryGetValue(_i, out _text2))
                    {
                        _text2 = "Spell #" + _i.ToString();
                    }
                }


                _text = " (Combat)";
                if ((_j = IntField("proclevel")) > 1)
                {
                    _text += " As Level " + _j.ToString();
                }

                if ((_j = IntField("procrate")) != 0)
                {
                    _text += ", Proc Rate: " + _j.ToString();
                }

                AddValue(9, "Effect: " + _text2 + _text);

                _j = IntField("proclevel2");
                SpellDescs.TryGetValue(_i, out _text3);

                if ((_j > 1) || (_text3 != null))
                {
                    AddValue(99, _text2, true);
                    if (_j > 1)
                    {
                        AddValue(99, "Required Level: " + _j.ToString());
                    }
                    if (_text3 != "")
                    {
                        AddValue(99, "Spell Effect: " + _text3);
                    }
                }
            }
            
            if ((_i = IntField("worneffect")) > 0)
            {
                if ((_text2 = TextField("wornname")) == "")
                {
                    if (!SpellNames.TryGetValue(_i, out _text2))
                    {
                        _text2 = "Spell #" + _i.ToString();
                    }
                }

                _text = " (Worn)";
                if ((_j = IntField("wornlevel")) > 1)
                {
                    _text += " As Level " + _j.ToString();
                }
                AddValue(9, "Effect: " + _text2 + _text);

                _j = IntField("wornlevel2");
                SpellDescs.TryGetValue(_i, out _text3);

                if ((_j > 1) || (_text3 != null))
                {
                    AddValue(99, _text2, true);
                    if (_j > 1)
                    {
                        AddValue(99, "Required Level: " + _j.ToString());
                    }
                    if (_text3 != "")
                    {
                        AddValue(99, "Spell Effect: " + _text3);
                    }
                }
            }

            if ((_i = IntField("focuseffect")) > 0)
            {
                if ((_text2 = TextField("focusname")) == "")
                {
                    if (!SpellNames.TryGetValue(_i, out _text2))
                    {
                        _text2 = "Spell #" + _i.ToString();
                    }
                }

                if ((_j = IntField("focuslevel")) > 1)
                {
                    _text2 += " As Level " + _j.ToString();
                }
                AddValue(9, "Focus Effect: " + _text2);

                _j = IntField("focuslevel2");
                SpellDescs.TryGetValue(_i, out _text3);

                if ((_j > 1) || (_text3 != null))
                {
                    AddValue(99, _text2, true);
                    if (_j > 1)
                    {
                        AddValue(99, "Required Level: " + _j.ToString());
                    }
                    if (_text3 != "")
                    {
                        AddValue(99, "Spell Effect: " + _text3);
                    }
                }
            }

            if ((_i = IntField("bardeffect")) > 0)
            {
                if ((_text2 = TextField("bardname")) == "")
                {
                    if (!SpellNames.TryGetValue(_i, out _text2))
                    {
                        _text2 = "Spell #" + _i.ToString();
                    }
                }

                if ((_j = IntField("bardlevel")) > 1)
                {
                    _text2 += " As Level " + _j.ToString();
                }
                AddValue(9, "Focus Effect: " + _text2);

                _j = IntField("bardlevel2");
                SpellDescs.TryGetValue(_i, out _text3);

                if ((_j > 1) || (_text3 != null))
                {
                    AddValue(99, _text2, true);
                    if (_j > 1)
                    {
                        AddValue(99, "Required Level: " + _j.ToString());
                    }
                    if (_text3 != "")
                    {
                        AddValue(99, "Spell Effect: " + _text3);
                    }
                }
            }
            else if (IntField("bardtype") > 0)
            {
                _text2 = (IntField("bardvalue") - 10).ToString();
                _text = ItemTypes[IntField("bardtype")] + " Resonance " + _text2;

                AddValue(9, "Focus Effect: " + _text);
                
                AddValue(99, _text, true);
                AddValue(99, "Spell Effect: Increases the power of " + ItemTypes[IntField("bardtype")].ToLower() + " based bard songs by up to " + _text2 + "0 percent");
            }
        }

        // Misc: Book, Slots an augment fits into, Charges, Potion Belt, Tradeskills, Food/Drink, Bane DMG, Augment Restriction/Solvent
        private void UpdatePreviewBox10()
        {
			int _i, _j;
			string _text;
			string _text2;

			if (ItemGroup == ItemGroups.Augmentation)
            {
                ItemLine.Length = 0;
                ItemLine.Append("This Augmentation fits in slot types: ");
                _i = IntField("augtype");
                for (_j = 0; _j < AugTypes.Length; _j++)
                {
                    if ((_i & (1 << _j)) != 0)
                    {
                        ItemLine.Append(AugTypes[_j + 1]);
                        ItemLine.Append(", ");
                    }
                }

                AddValue(10, TrimComma(ItemLine.ToString()), true);
            }

            // Max Charges and Tradeskills get grouped together, but there is a line break above them if one is set.
            _j = IntField("tradeskills");

            if ((_i = IntField("maxcharges")) > 0)
            {
                _text = "Charges: " + _i.ToString();

                if (IntField("potionbelt") != 0)
                {
                    _text += ", Potion Belt Enabled";
                }
                
                if (((IntField("clicktype") == 3) || (ItemGroup == ItemGroups.Potion)) && (IntField("maxcharges") > 0))
                {
                    _text += ", Expendable";
                }

                if (_j == 0)
                {
                    AddValue(10, " ");
                }
                AddValue(10, _text);
            }

            if (IntField("tradeskills") != 0)
            {
                AddValue(10, "This item can be used in tradeskills", _i == 0);
            }

            if ((IntField("book") != 0) || (IntField("itemclass") == 2))
            {
                _i = IntField("booktype");

                if ((_i < 0) || (_i >= Languages.Length))
                {
                    _text = "Unknown";
                }
                else
                {
                    _text = Languages[_i];
                }

                _text2 = TextField("filename");
                if (_text2 == "")
                {
                    _text2 = "unopenable";
                }
                else
                {
                    _text2 = "[" + _text2 + "]";
                }

                AddValue(10, "This book is " + _text2 + ", Language: " + _text);
            }

            if (ItemGroup == ItemGroups.FoodDrink)
            {
                if (IntField("itemtype") == 14)
                {
                    AddValue(10, "This is a meal (" + TextField("casttime") + " / " + HighestNourishment.ToString() + ")");
                }
                else
                {
                    AddValue(10, "This is a drink (" + TextField("casttime") + " / " + HighestNourishment.ToString() + ")");
                }
            }

            if ((IntField("banedmgrace") > 0) || (IntField("banedmgbody") > 0))
            {
                ItemLine.Length = 0;
                ItemLine.Append("\r\nBane Dmg: ");

                if ((_i = IntField("banedmgrace")) > 0)
                {
                    if (_i >= (Races.Length - 1))
                    {
                        ItemLine.Append("Unknown Race (");
                        ItemLine.Append(_i);
                        ItemLine.Append(")");
                    }
                    else
                    {
                        ItemLine.Append(Races[_i - 1]);
                    }

                    ItemLine.Append(" +");
                    ItemLine.Append(TextField("banedmgraceamt"));
                }

                if ((_i = IntField("banedmgbody")) > 0)
                {
                    if (IntField("banedmgraceamt") > 0)
                    {
                        ItemLine.Append(", ");
                    }

                    if (_i >= BodyTypes.Length)
                    {
                        ItemLine.Append("Unknown (");
                        ItemLine.Append(_i);
                        ItemLine.Append(")");
                    }
                    else
                    {
                        ItemLine.Append(BodyTypes[_i]);
                    }

                    ItemLine.Append(" +");
                    ItemLine.Append(TextField("banedmgamt"));
                }

                AddValue(10, ItemLine.ToString(), true);
            }

            if (ItemGroup == ItemGroups.Augmentation)
            {
                _i = IntField("augrestrict");

                ItemLine.Length = 0;
                ItemLine.Append("Restrictions - ");
                ItemLine.Append(((_i < 0) || (_i >= AugRestrictions.Length)) ? "Unknown (" + _i.ToString() + ")" : AugRestrictions[_i]);

                AddValue(10, ItemLine.ToString(), true);

                ItemLine.Length = 0;
                //ItemLine.Append("You must use the solvent ");

                ItemLine.Append("Solvent: ");
                ItemLine.Append(GetText("SELECT `name` FROM `items` WHERE `id`=" + TextField("augdistiller")));
                //ItemLine.Append(", or a Perfected Augmentation Distiller, to remove this augment safely.");

                AddValue(10, ItemLine.ToString(), true);
            }

            if ((_text = TextField("CharmFile")) != "")
            {
                _text = GetText("SELECT `txtfile` FROM `books` WHERE `name`='" + TextField("CharmFile").Replace("'", "''") + "';");

                if (_text != "")
                {
                    AddValue(10, "Item information:", true);
                    AddValue(10, _text, true);
                }
            }
        }

        private void UpdatePreviewBox11()
        {

        }

        private void UpdatePreviewBox12()
        {

        }

        #endregion
    }
}
