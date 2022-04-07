using UnityEngine;
using UnityEngine.UI;

public class XButton : MonoBehaviour
{
	Button button;

	public void Disable(bool value) => button.interactable = !value;

	// Unity
	private void OnEnable()
	{
		button = GetComponent<Button>();
	}
}