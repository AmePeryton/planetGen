using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditMenu : MonoBehaviour
{
	public static EditMenu instance { get; private set; }

	[Header("Selection Menu")]
	public GameObject selectionMenu;			// Selection panel GameObject
	public RectTransform selectionContentRect;	// RectTransform where new focusLines will go
	public Button resetFocusButton;				// Button the reset the focus (to null)
	public GameObject focusLinePrefab;			// Prefab for the focus lines
	public List<FocusLine> focusLines;			// List of focus line instances
	public bool selectionPanelEnabled;			// If the selection panel is on screen or hidden off screen
	public List<IMenuEditable> focusCandidateList;	// List of potential focuses in the scene (all have IMenuEditable implemented)

	[Header("Stat Menu")]
	public GameObject statMenu;					// Stat panel GameObject
	public RectTransform statContentRect;		// RectTransform where new statLines will go
	public GameObject arrowSprite;				// The image for the panel toggle arrow (so it can be flipped)
	public TextMeshProUGUI StatNameLabel;		// The text label for the focus' name
	public TextMeshProUGUI StatInfoLabel;		// The text label for the focus' information
	public GameObject[] statLinePrefabs = new GameObject[8];	// Default, bool, Color, float, int, string, Vector2, Vector3 stat line prefabs
	public List<StatLine> statLines;			// List of stat line instances
	public bool statPanelEnabled;				// If the stat panel is on screen or hidden off screen
	public IMenuEditable focus;					// The current focus

	private void Awake()
	{
		// Singleton line
		if (instance != null) { Debug.LogWarning(GetType().Name + " already present in scene!"); } instance = this;
	}

	private void Start()
	{
		FindFocusCandidates();
		NewFocus(null);
		//InvokeRepeating(nameof(FindFocusCandidates), 2f, 2f);
	}

	public void FindFocusCandidates()
	{
		//Debug.Log("Ping!");
		// If the list of new candidates is the same as the old one, dont update the display
		List<IMenuEditable> newFocusCandidateList = FindObjectsOfType<MonoBehaviour>().OfType<IMenuEditable>().ToList();
		if (newFocusCandidateList.Equals(focusCandidateList))
		{
			//Debug.Log("Pong!");
			return;
		}
		//Debug.Log("Pop!");

		// If the list is different, assign the new list and update the display
		focusCandidateList = newFocusCandidateList;
		// Destroy all old focus lines
		foreach (FocusLine line in focusLines)
		{
			Destroy(line.gameObject);
		}
		focusLines.Clear();

		// Instantiate new focus lines
		foreach (var candidate in focusCandidateList)
		{
			FocusLine newFocusLine = Instantiate(focusLinePrefab, selectionContentRect).GetComponent<FocusLine>();
			newFocusLine.focus = candidate;
			newFocusLine.InitDisplay();
			focusLines.Add(newFocusLine);
		}

		// To properly set the interactability of the buttons
		//NewFocus(focus);
	}

	public void ToggleStatPanel()
	{
		// Toggles current stat panel state (good for buttons)
		EnableStatPanel(!statPanelEnabled);
	}

	public void ToggleSelectionPanel()
	{
		// Toggles current selection panel state (good for buttons)
		EnableSelectionPanel(!selectionPanelEnabled);
	}

	public void EnableStatPanel(bool enable)
	{
		if (enable)
		{
			if (focus == null)
			{
				// Keep panel hidden if there is no current focus
				statPanelEnabled = false;
				Debug.LogWarning("[EditMenu]: No focus selected.");
				return;
			}

			statPanelEnabled = true;

			// Move panel into frame
			LeanTween.moveLocalX(statMenu, 960, 1).setEaseOutCubic();

			// Reverse arrow direction
			LeanTween.rotateY(arrowSprite, 0, 0.5f);
		}
		else
		{
			statPanelEnabled = false;

			// Move panel out of frame
			LeanTween.moveLocalX(statMenu, 1680, 1).setEaseOutCubic();

			// Reverse arrow direction
			LeanTween.rotateY(arrowSprite, 180, 0.5f);
		}
	}

	public void EnableSelectionPanel(bool enable)
	{
		if (enable)
		{
			selectionPanelEnabled = true;

			// Move panel into frame
			LeanTween.moveLocalX(selectionMenu, -960, 1).setEaseOutCubic();
		}
		else
		{
			selectionPanelEnabled = false;

			// Move panel out of frame
			LeanTween.moveLocalX(selectionMenu, -1440, 1).setEaseOutCubic();
		}
	}

	public void NewFocus(IMenuEditable newFocus)
	{
		// Destroy old stat lines
		foreach (StatLine line in statLines)
		{
			Destroy(line.gameObject);
		}
		statLines.Clear();

		// Re-enable all focus line buttons (the currently selected focus will then disable its own button)
		foreach (FocusLine line in focusLines)
		{
			line.GetComponent<Button>().interactable = true;
		}

		focus = newFocus;

		// If the focus is null, only disable the reset button, clear the stat panel's text, and disable it
		if (focus == null)
		{
			resetFocusButton.interactable = false;
			StatNameLabel.text = "";
			StatInfoLabel.text = "";
			EnableStatPanel(false);
			return;
		}

		// If the focus is not null, enable the reset button and set the label texts
		resetFocusButton.interactable = true;
		StatNameLabel.text = focus.GetName();
		StatInfoLabel.text = focus.GetInfo();

		// Instantiate a new stat line for all valid fields in the focus class
		foreach (var field in focus.GetType().GetFields())
		{
			MenuEditableAttribute mea = field.GetCustomAttribute<MenuEditableAttribute>();
			if (mea != null)
			{
				// Instantiate new stat line based on field type
				GameObject newObj;
				switch (field.FieldType.ToString())
				{
					case "System.Boolean":
						newObj = Instantiate(statLinePrefabs[1], statContentRect);
						break;
					case "UnityEngine.Color":
						newObj = Instantiate(statLinePrefabs[2], statContentRect);
						break;
					case "System.Single":
						newObj = Instantiate(statLinePrefabs[3], statContentRect);
						break;
					case "System.Int32":
						newObj = Instantiate(statLinePrefabs[4], statContentRect);
						break;
					case "System.String":
						newObj = Instantiate(statLinePrefabs[5], statContentRect);
						break;
					case "UnityEngine.Vector2":
						newObj = Instantiate(statLinePrefabs[6], statContentRect);
						break;
					case "UnityEngine.Vector3":
						newObj = Instantiate(statLinePrefabs[7], statContentRect);
						break;
					default:
						Debug.LogWarning("UNRECOGNIZED TYPE: " + field.FieldType.ToString());
						newObj = Instantiate(statLinePrefabs[0], statContentRect);
						break;
				}

				// Set up individual stat line variables and add to list
				StatLine newStatLine = newObj.GetComponent<StatLine>();
				newStatLine.focus = focus;
				newStatLine.field = field;

				// Prompt new statline to initialize its display with the new data
				newStatLine.InitDisplay();
				statLines.Add(newStatLine);
			}
		}

		EnableStatPanel(true);

		// If no fields with headers were found in the focus class, print warning
		if (focus.GetType().GetFields().Length == 0)
		{
			Debug.LogWarning("[EditMenu] No suitable fields found in class " + focus.GetType().ToString());
		}
	}

	public void ResetFocus()
	{
		NewFocus(null);
	}
}