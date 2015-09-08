EQEmu Item Editor
-----------

Current Version: 1.1.3

Last Updated: 8/19/2015

Github Link: https://github.com/Shendare/EQEmuItemEditor

To Download: https://github.com/Shendare/EQEmuItemEditor/releases

#Features:

* Search, create, edit, save, clone, and delete items from the EQEmu items table.

#Screenshots:

>https://raw.githubusercontent.com/Shendare/EQEmuItemEditor/master/screenshots/screenshot1.png

>https://raw.githubusercontent.com/Shendare/EQEmuItemEditor/master/screenshots/screenshot2.png

>https://raw.githubusercontent.com/Shendare/EQEmuItemEditor/master/screenshots/screenshot3.png

>https://raw.githubusercontent.com/Shendare/EQEmuItemEditor/master/screenshots/screenshot4.png

>https://raw.githubusercontent.com/Shendare/EQEmuItemEditor/master/screenshots/screenshot5.png

#Disclaimer:

>EQEmu Item Editor is not affiliated with, endorsed by, approved by, or in any way associated with Daybreak Games or the EverQuest franchise, who reserve all copyrights and trademarks to their properties.

#License:

>Portions of this software's code not covered by another author's or entity's copyright are released under the Creative Commons Zero (CC0) public domain license.

>To the extent possible under law, Shendare (Jon D. Jackson) has waived all copyright and related or neighboring rights to this EQEmuItemEditor application. This work is published from: The United States.

>You may copy, modify, and distribute the work, even for commercial purposes, without asking permission.

>For more information, read the CC0 summary and full legal text here:

>https://creativecommons.org/publicdomain/zero/1.0/

#Release Notes:

8/23/2015 - Version 1.1.3

* Fixed: Post-SoF item icons in the icon folder will now load and display properly.


8/14/2015 - Version 1.1.2

* Fixed: The Slots checklist logic has been completely redone. It should no longer be flakey.

* Improved: Clarified the database connection process, which was somewhat obtuse before. There is now a Test Connection button below the database setting fields, and clicking it will result in a confirmation or error message, depending on the results of the connection test. You must test a new or modified connection before performing a search.

* Improved: After performing a search, the Search button will disable itself until you make a change to the search terms or database connection. This will prevent accidental multi-clicks and will let you know if a search was successfully performed and there were simply no matches.


8/11/2015 - Version 1.1.1

* Fixed: Put maximum lengths onto text fields based on the columns in the database, to prevent errors saving when text is too long.
         The maximum lengths are not being sent over by the database (they show -1), so I have to hard-code them.

* Fixed: Waist and Ammo armor slot checkboxes weren't displaying correctly.

* Fixed: The New item button was confusing the search list and preview window. Straightened it out.


8/11/2015 - Version 1.1

* Fixed: Error setting '[NumericField]' to ''.

* Fixed: "Lore Group: #1 (Oops!)" showing on new items

* Fixed: Unrecognized tab could be populated with the same bunch of controls multiple times

* Fixed: Smartened SQL encoding of various field types

* Fixed: Added jpeg and removed targa from the Icon loader to match actual .Net support

* Fixed: Vastly improved handling of app's edit-state and what item is being previewed/edited

* Fixed: Handling of broken database connection at first startup

* Fixed: Mismatched Preview of "Must Equip" and "Any Slot/Can Equip" click effect types

* Added: Implemented cloning and deleting (with confirmation) multiple items at once

* Added: Version number to window title

* Improved: Better default values for some fields when creating New item from scratch

* Improved: Unchangeable non-click spell effects on items now display as obviously unchangeable

* Improved: Cloned items will now be named with a number after to denote that it's a duplicate

* Improved: Clarified discard changes prompt for items where the ID has been changed


8/10/2015 - Version 1.0.1

* Fixed glitch with saving or deleting new items, or changing an item's id


8/9/2015 - Version 1.0

* Initial Release
