using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNumber : XButton
{
#pragma warning disable 0649

	[SerializeField] int number;

#pragma warning restore 0649

	public static event Action<int> OnButtonNumberClicked;

	// Events
	private void ButtonClicked()
	{
		OnButtonNumberClicked?.Invoke(number);
	}

	// Unity
	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(ButtonClicked);
	}
}