using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class V1_SettingsPanelController : MonoBehaviour, ISettingsSavable
{
	public float volume;
	public Slider volumeSlider;

	private void Awake()
	{
		volumeSlider.onValueChanged.AddListener((v) => {
			volume = v;
		});
	}

	public void LoadSettingsData(V1_SettingsData data)
	{
		volume = data.volume;
		volumeSlider.value = volume;
	}

	public void SaveSettingsData(ref V1_SettingsData data)
	{
		data.volume = volume;
	}

	public void ButtonApply()
	{
		V1_SettingsManager.instance.SaveSettings();
	}

	private void OnEnable()
	{
		V1_SettingsManager.instance.LoadSettings();
	}
}