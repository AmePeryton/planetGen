Edit Menu V1.0.0
6/7/2024

An in-game UI for the user to manipulate and view variables within instances of custom classes, with minimal edits to the original script file.

Includes all of LeanTween (https://assetstore.unity.com/packages/tools/animation/leantween-3595) for animations
Also includes 2 icons from UX Flat Icons [Free] (https://assetstore.unity.com/packages/2d/gui/icons/ux-flat-icons-free-202525)
Does NOT include the required dependency TextMeshPro

To implement:
1. Import package
	1a. If LeanTween is already in your project, you can delete the LeanTween folder that comes with this package
2. Place "Edit Menu" prefab into canvas with UI scale mode set to "Scale With Screen Size", reference res 1920x1080
3. Implement the "IMenuEditable" interface into any classes you want the menu to be able to edit or monitor
4. Add "MenuEditable" attributes to any PUBLIC fields in the class you want to be editable, with parameters
	4a. Currently supported data types are bool, color, int, float, string, vector2, and vector3
	4b. Parameters are display name (mandatory), editable (defaults to true), and slider min and max (default to 0 and 1)

To use:
1. The edit menus should automatically populate with scripts in the scene that implement IMenuEditable, and regularly refresh
2. Press the button on the top left to reveal the selection menu and select which instance you want to view or edit
3. The right "stat" menu should pop out, showing the variables with the MenuEditable attribute
4. Input data or use sliders and toggles to change values in the focused script
5. Press the "Reset Focus" button to deselect the current instance
6. Press toggle buttons on the top left and top right to show / hide the menus

To change style of the menu, go to the focusLine or statLine prefabs and change images, colors, font, etc.

Changelog:
V1.0.0 - Initial version

Future TODO:
- Implement support for object click selection rather than menu selection
- Implement a list priority system to order stat/focus lines by user preference
- Add text and UI scaling options
- Add display update frequency options (currently 10 updates per second)
- Implement more data types to display
- Add "Headers" to the diplay
- Implement default variable names if parameters are blank
- Add color wheel UI
- Make text input boxes show values better (long values are often hidden)
- Implement dynamically editable UI elements (changing line names with other line variables, etc.)