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

        public static string[] Slots = new string[] {
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

        public bool DBLoaded = false;

        public Dictionary<string, bool> ChangedColumn = new Dictionary<string, bool>();
        public Dictionary<string, bool> UsedColumn = new Dictionary<string, bool>();
        public bool AllUsedColumnsFinished = false;
        
        public Dictionary<int, string> SpellNames = new Dictionary<int,string>();
        public Dictionary<int, string> SpellDescs = new Dictionary<int,string>();
        public Dictionary<int, string> FactionNames = new Dictionary<int, string>();
        public Dictionary<int, string> ItemNames = new Dictionary<int, string>();

        // Moved local general use variables to class properties just so they don't need to be defined
        // separately in each UpdatePreviewBox() method.  It's nice having all of the rendering logic
        // split up into separate methods like it is.
        private int _i, _j;
        private string _text;
        private string _text2;
        private string _text3;

        #region Form Event Handlers

        public formMain()
        {
            InitializeComponent();

            ItemLoading = true;

            Dictionary<int, string> _clickTypes = new Dictionary<int, string>();
            _clickTypes.Add(0, "Standard Item Effect Activation");
            _clickTypes.Add(1, "Any Slot, No Race/Class Check");
            _clickTypes.Add(3, "Any Slot, No Check, Expendable");
            _clickTypes.Add(4, "Must Equip in order to Activate");
            _clickTypes.Add(5, "Any Slot, Must be Able to Equip");
            listEditClickType.DataSource = new BindingSource(_clickTypes, null);
            listEditClickType.ValueMember = "Key";
            listEditClickType.DisplayMember = "Value";
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
                        _item.Name = (string)_row["name"];
                        _item.Text = (string)_row["name"];
                        _item.ImageKey = _row["icon"].ToString();
                        _item.Tag = (int)_row["id"];

                        listSearchItems.Items.Add(_item);
                    }
            }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ItemLoading)
            {
                return;
            }

            ItemLoading = true;

            if (Item == null)
            {
                if (PreviewItem == null)
                {
                    return;
                }
                
                Item_EditMode();
            }
            else if (Item["id"].ToString() != textEditID.Text)
            {
                Item_EditMode();
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

            ItemLoading = false;
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            if (!DBLoaded)
            {
                ResetOptionFields();

                if (GetData("SELECT 1;").Rows[0][0].ToString() == "")
                {
                    // Error connecting to database

                    optionsToolStripMenuItem_Click(null, null);
                }
                else
                {
                    labelLoading.BringToFront();
                    labelLoading.Visible = true;

                    timerLoading.Enabled = true;
                }
            }

            if ((itemIcons.Tag == null) && (itemIcons.Images.Count == 0))
            {
                timerLoading.Enabled = true;
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
            int _boxnum = ((_box.Tag == null) || (_box.Tag == "")) ? 98 : int.Parse(_box.Tag.ToString());
            int _value = (_box.Checked ? 1 : 0);

            switch (_field)
            {
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
            int _boxnum = ((_box.Tag == null) || (_box.Tag == "")) ? 98 : int.Parse(_box.Tag.ToString());

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
                            _boxnum = 99; // No preview change
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
                        case 2: // Ear
                            _box.SetItemChecked(0, false);

                            SetField(_field, IntField(_field) ^ 0x000012);
                            break;
                        case 9: // Wrist
                            _box.SetItemChecked(0, false);

                            SetField(_field, IntField(_field) ^ 0x000600);
                            break;
                        case 14: // Fingers
                            _box.SetItemChecked(0, false);

                            SetField(_field, IntField(_field) ^ 0x018000);
                            break;
                        default:
                            _box.SetItemChecked(0, false);

                            _index += (_index > 4) ? 1 : 0;
                            _index += (_index > 10) ? 1 : 0;
                            _index += (_index > 16) ? 1 : 0;

                            SetField(_field, IntField(_field) ^ (1 << (_index - 1)));
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
            int _boxnum = ((_box.Tag == null) || (_box.Tag == "")) ? 98 : int.Parse(_box.Tag.ToString());
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
            int _boxnum = ((_box.Tag == null) || (_box.Tag == "")) ? 98 : int.Parse(_box.Tag.ToString());
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
                    ItemLoading = false;
                    return;
                case "textdbhost":
                    if (_value.Trim().Equals("localhost", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _box.Text = "127.0.0.1";
                        labelDBHostNote.Visible = true;
                    }
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
                                if (_value == "")
                                {
                                    SetField(_field, 0); // Empty box for a number value. That means zero.
                                }
                                else if (_value != "-") // User has started typing a negative number, but hasn't gotten to digits yet
                                {
                                    // I don't know why it threw up.

                                    System.Diagnostics.Debugger.Break();
                                }
                            }
                            break;
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

        private void listSearchItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonItemClone.Enabled = (Item != null); // listSearchItems.SelectedItems.Count > 0;

            if ((PreviewItem != null) || (listSearchItems.SelectedItems.Count == 1))
            {
                editToolStripMenuItem.ForeColor = SystemColors.ControlText;
            }
            else
            {
                editToolStripMenuItem.ForeColor = SystemColors.ScrollBar;
            }

            if (listSearchItems.SelectedItems.Count == 1)
            {
                Item_Preview((int)listSearchItems.SelectedItems[0].Tag);
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
            if (ItemLoading)
            {
                return;
            }

            ItemLoading = true;

            if (panelOptions.Visible)
            {
                // Options Panel Open. Did we make changes?

                if (Settings.IconFolder != labelIconFolder.Text)
                {
                    Settings.IconFolder = labelIconFolder.Text;
                    
                    itemIcons.Images.Clear();
                    itemIcons.Tag = null;
                }

                if ((Settings.Database.Conn != textDBConn.Text) ||
                    (Settings.Database.Host != textDBHost.Text) ||
                    (Settings.Database.Name != textDBName.Text) ||
                    (Settings.Database.User != textDBUser.Text) ||
                    (Settings.Database.Pass != textDBPass.Text) ||
                    (Settings.Database.Port != textDBPort.Text))
                {
                    Settings.Database.Conn = textDBConn.Text;
                    Settings.Database.Host = textDBHost.Text;
                    Settings.Database.Name = textDBName.Text;
                    Settings.Database.User = textDBUser.Text;
                    Settings.Database.Pass = textDBPass.Text;
                    Settings.Database.Port = textDBPort.Text;

                    if (DBLoaded) // Reconfirm database connection in case something changed
                    {
                        DBLoaded = (GetData("SELECT 1;").Rows[0][0].ToString() != "");
                    }
                }

                Settings.Save();
            }

            // Confirm we've got a database connection
            formMain_Load(null, null);
            
            if (DBLoaded)
            {
                panelSearch.Visible = true;
                panelEdit.Visible = false;
                panelOptions.Visible = false;
            }

            ItemLoading = false;
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
            timerLoading.Enabled = false;

            ItemLoading = true;

            if (!DBLoaded)
            {
                editToolStripMenuItem.ForeColor = SystemColors.ScrollBar;

                panelSearch.Visible = true;
                panelEdit.Visible = false;
                panelOptions.Visible = false;

                buttonItemClone.Enabled = false;
                buttonItemSave.Enabled = false;
                buttonItemDelete.Enabled = false;
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
                catch
                {
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
            try
            {
                Item[FieldName] = Value;
                ChangedColumn[FieldName.ToLower()] = true;

                return;
            }
            catch
            { }

            try
            {
                Item[FieldName] = Value.ToString();
                ChangedColumn[FieldName.ToLower()] = true;

                return;
            }
            catch
            {
                if (!IgnoreFieldErrors)
                {
                    MessageBox.Show(this, "Error setting field `" + FieldName + "` to value '" + Value + "'", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                if (MessageBox.Show(this, "You have made unsaved changes to " + Item["name"] + " (" + Item["id"] + ").\n\nAre you sure you wish to discard them?", "Discard Unsaved Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                {
                    return false;
                }
            }

            return true;
        }

        private void Item_EditMode()
        {
            int _i = (listSearchItems.SelectedItems.Count < 1) ? IntField("id") : (int)listSearchItems.SelectedItems[0].Tag;
            Item_Preview(_i);
            Item = PreviewItem;
            ChangedColumn.Clear();

            int _columns = Item.Table.Columns.Count;
            ItemLoading = true;

            string _field = "";
            string _value = "";
            
            List<Control> _controls = new List<Control>();

            // Marking these fields as used so they don't end up in Unrecognized:

            // Handled in a custom way.
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
            TextField("loregroupmembercount");

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

                                    System.Diagnostics.Debugger.Break();
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

            _i = 0;
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

            editToolStripMenuItem_Click(null, null);
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
                    int _slotCount = 1;

                    _checklist.SetItemChecked(0, _number == 0x3FFFFF);

                    for (_slot = 1; _slot < Slots.Length - 1; _slot++)
                    {
                        switch (_slot)
                        {
                            case 4:
                            case 10:
                            case 16:
                                break;
                            default:
                                _checklist.SetItemChecked(_slotCount++, (_number & (1 << (_slot - 1))) != 0);
                                break;
                        }
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
        
        private void Item_Preview(int ItemID)
        {
            if (!Item_ConfirmChange())
            {
                return;
            }

            if ((PreviewItem == null) || (IntField("id") != ItemID))
            {
                PreviewItem = GetData("SELECT *,0 AS `loregroupmembercount` FROM `items` WHERE `id`=" + ItemID.ToString() + ";").Rows[0];

                if (IntField("loregroup") > 0)
                {
                    PreviewItem["loregroupmembercount"] = GetData("SELECT COUNT(*) FROM `items` WHERE `loregroup`=" + TextField("loregroup") + ");").Rows[0][0];
                }

                Item = PreviewItem;
                OldItemID = ItemID;
                ChangedColumn.Clear();
                UpdatePreviewBox(-1);
            }
        }

        private void Item_New(object sender, EventArgs e)
        {
            if (!Item_ConfirmChange())
            {
                return;
            }

            _i = GetInt("SELECT MAX(`id`) FROM `items`;");
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
                if (Item[_col].GetType().IsPrimitive)
                {
                    Item[_col] = 0;
                }
                else if (Item[_col].GetType().ToString() == "System.DateTime")
                {
                    Item[_col] = DateTime.UtcNow;
                }
                else if (Item[_col].GetType().ToString() == "System.DBNull")
                {
                    Item[_col] = DBNull.Value;
                }
                else
                {
                    Item[_col] = "";
                }
            }

            Item["id"] = _i;
            Item["name"] = "New Item #" + _i.ToString();
            Item["charmfileid"] = 0; // Null in integer field instead of 0 makes a TextBox throw up.

            switch (Item_Create())
            {
                case -1:
                    MessageBox.Show(this, "Error: There was a problem creating blank item # " + _i.ToString() + " in the Items table.", "Error Saving Item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case 0:
                    MessageBox.Show(this, "Weird: There was a glitch creating blank item # " + _i.ToString() + " in the Items table.\r\n\r\nThe database didn't report an error, but it did not insert the new item.", "Glitch Cloning Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                default:
                    ChangedColumn.Clear();
                    buttonItemSave.Enabled = false;
                    editToolStripMenuItem.ForeColor = SystemColors.ControlText;
                    listSearchItems.SelectedItems.Clear();
                    PreviewItem = Item;
                    UpdatePreviewBox(-1);
                    Item = null;
                    Item_EditMode();
                    break;
            }
        }
        
        private void Item_Clone(object sender, EventArgs e)
        {
            if (Item == null)
            {
                buttonItemClone.Enabled = false;
                return;
            }

            _i = GetInt("SELECT MAX(`id`) FROM `items`;");
            if (_i < 1001)
            {
                _i = 1001;
            }
            else
            {
                _i++;
            }

            int _oldItemID = IntField("id");
            Item["id"] = _i;

            switch (Item_Create())
            {
                case -1:
                    MessageBox.Show(this, "Error: There was a problem cloning this as item # " + Item["id"].ToString() + " to the Items table.", "Error Saving Item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Item["id"] = _oldItemID;
                    break;
                case 0:
                    MessageBox.Show(this, "Weird: There was a glitch cloning this as item # " + Item["id"].ToString() + " to the Items table.\r\n\r\nThe database didn't report an error, but it did not insert the cloned item.", "Glitch Cloning Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Item["id"] = _oldItemID;
                    break;
                default:
                    ChangedColumn.Clear();
                    buttonItemSave.Enabled = false;
                    ItemLoading = true;
                    textEditID.Text = Item["id"].ToString();
                    ItemLoading = false;
                    PreviewItem = Item;
                    UpdatePreviewBox(-1);
                    buttonSearchName_Click(buttonSearchName, null);
                    break;
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
                    _columns.Append(_col);
                    _columns.Append('`');

                    if (Item[_col].GetType().ToString() == "System.DateTime")
                    {
                        _values.Append('\'');
                        _values.Append(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                        _values.Append('\'');
                    }
                    else if (Item[_col] == DBNull.Value)
                    {
                        _values.Append("null");
                    }
                    else
                    {
                        _values.Append('\'');
                        _values.Append(Item[_col].ToString().Replace("'", "''"));
                        _values.Append('\'');
                    }
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
                buttonItemSave.Enabled = false;
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
                ItemLine.Append("`=\'");
                ItemLine.Append(Item[_col].ToString().Replace("'", "''"));
                ItemLine.Append('\'');
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
                    buttonItemSave.Enabled = false;
                    buttonSearchName_Click(buttonSearchName, null);
                    break;
            }
        }

        private void Item_Delete(object sender, EventArgs e)
        {
            if (Item == null)
            {
                buttonItemDelete.Enabled = false;
                return;
            }

            if (MessageBox.Show(this, "WARNING: You are about to DELETE\r\n\r\n" + TextField("name") + " (" + OldItemID.ToString() + ").\r\n\r\nAre you SURE you wish to do this?", "Delete Item Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
            {
                return;
            }

            switch (Execute("DELETE FROM `items` WHERE `id`=" + OldItemID.ToString()))
            {
                case -1:
                    MessageBox.Show(this, "Error: There was a problem deleting item # " + OldItemID.ToString() + " from the Items table.", "Error Deleting Item", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case 0:
                    MessageBox.Show(this, "Weird: There was a glitch deleting item # " + OldItemID.ToString() + " from the Items table.\r\n\r\nThe database said it couldn't find that item, so it did nothing.", "Glitch Saving Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                default:
                    ChangedColumn.Clear();
                    buttonItemSave.Enabled = false;
                    searchToolStripMenuItem_Click(searchToolStripMenuItem, null);
                    buttonSearchName_Click(buttonSearchName, null);
                    OldItemID = -1;
                    Item = null;
                    PreviewItem = null;
                    editToolStripMenuItem.ForeColor = SystemColors.ScrollBar;
                    break;
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

                List<string> _iconExtensions = new List<string>(new string[] {".png", ".gif", ".bmp", ".tga" });

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
                                if ((_iconNum >= 500) && (_iconNum <= 2253)) // Last Icon: Titanium = 1700, SoF = 2253, RoF2 = 6898
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
                    if (_textbox.Name != "textEditDebug")
                    {
                        _textbox.TextChanged += this.formTextbox_Changed;
                    }
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
                for (BoxNumber = 0; BoxNumber < 20; BoxNumber++)
                {
                    UpdatePreviewBox(BoxNumber, true);
                }
            }
            else if (BoxNumber >= 100)
            {
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

                buttonItemSave.Enabled = ChangedColumn.Count > 0;
                buttonItemDelete.Enabled = (Item != null);
                buttonItemClone.Enabled = (Item != null);
            }
        }

        // Basics: ID, Name, Icon, ItemType
        private void UpdatePreviewBox0()
        {
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
                    for (_j = 0; _j < Slots.Length; _j++)
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
                                    ItemLine.Append(Slots[_j]);
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
