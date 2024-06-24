using UnityEngine;
using UnityEngine.UI;

public class V1_SettingsPanelController : MonoBehaviour
{
	public Slider volumeSlider;
	//public Toggle autosaveToggle;

	private void OnEnable()
	{
		// Set UI elements to reflect data
		LoadSettings();
	}

	void Start()
	{
		// Subscribe UI elements to value changes
		volumeSlider.onValueChanged.AddListener((v) => {
			V1_SettingsManager.instance.data.volume = v;
		});
	}

	// Update UI elements to reflect settings data
	public void LoadSettings()
	{
		volumeSlider.value = V1_SettingsManager.instance.data.volume;
	}

	// Commit settings to file
	public void SaveSettings()
	{
		// Save to file
		V1_SettingsManager.instance.SaveSettingsData();
	}

	// Reload settings data from file
	public void DiscardSettings()
	{
		// Override the modified settings with the data in the file
		V1_SettingsManager.instance.LoadSettingsData();
	}

	public void ButtonAccept()
	{
		// Save to file
		SaveSettings();
		// Return to title screen
		V1_MainMenuController.instance.SwitchPanel(0);
	}

	public void ButtonBack()
	{
		// Reload data from settings file
		DiscardSettings();
		// Return to title screen
		V1_MainMenuController.instance.SwitchPanel(0);
	}
}