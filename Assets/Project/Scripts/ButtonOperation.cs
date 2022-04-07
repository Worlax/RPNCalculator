using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOperation : XButton
{
	public enum OperationType
	{
		None,
		Add,
		Subtract,
		Multiply,
		Divide,
		Percentage,
		Power,
		Root,
		NextNumber
	}

#pragma warning disable 0649

	[field: SerializeField] public OperationType Type { get; private set; }

#pragma warning restore 0649

	public static event Action<string> OnButtonOperationClicked;
	public string GetOperationCode()
	{
		string operationCode = "";

		switch (Type)
		{
			case OperationType.Add:
				operationCode = Operation.Add;
				break;

			case OperationType.Subtract:
				operationCode = Operation.Subtract;
				break;

			case OperationType.Multiply:
				operationCode = Operation.Multiply;
				break;

			case OperationType.Divide:
				operationCode = Operation.Divide;
				break;

			case OperationType.Percentage:
				operationCode = Operation.Percentage;
				break;

			case OperationType.Power:
				operationCode = Operation.Power;
				break;

			case OperationType.Root:
				operationCode = Operation.Root;
				break;

			case OperationType.NextNumber:
				operationCode = Operation.NextNumber;
				break;
		}

		return operationCode;
	}

	// Events
	private void ButtonClicked()
	{
		OnButtonOperationClicked?.Invoke(GetOperationCode());
	}

	// Unity
	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(ButtonClicked);
	}

}