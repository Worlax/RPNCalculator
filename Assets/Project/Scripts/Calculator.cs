using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calculator : MonoBehaviour
{
#pragma warning disable 0649

	[SerializeField] Screen screen;
	[SerializeField] Button clear;
	[SerializeField] Button calculate;

#pragma warning restore 0649

	List<ButtonNumber> numberButtons = new List<ButtonNumber>();
	List<ButtonOperation> binaryOperationButtons = new List<ButtonOperation>();
	List<ButtonOperation> unaryOperationButtons = new List<ButtonOperation>();
	List<ButtonOperation> newNumberButtons = new List<ButtonOperation>();

	private bool AllBinaryOperationsAdded => screen.GetAllBinaryOperations().Count() + 1 == screen.GetAllNumbers().Count();
	private void DisableNumbers(bool value) => numberButtons.ForEach(x => x.Disable(value));
	private void DisableUnaryOperations(bool value) => unaryOperationButtons.ForEach(x => x.Disable(value));
	private void DisableBinaryOperations(bool value) => binaryOperationButtons.ForEach(x => x.Disable(value));
	private void DisableNewNumberButtons(bool value) => newNumberButtons.ForEach(x => x.Disable(value));
	private void DisableCalculateButton(bool value) => calculate.interactable = !value;

	// Events
	private void ScreenStatusChanged(Screen.ScreenLastInput status)
	{
		DisableCalculateButton(true);

		switch (status)
		{
			case Screen.ScreenLastInput.None:
				SetAvaibleButton(true, false, false, false, false);
				break;

			case Screen.ScreenLastInput.FirstNumber:
				SetAvaibleButton(true, true, true, false, false);
				break;

			case Screen.ScreenLastInput.Space:
				SetAvaibleButton(true, false, false, false, false);
				break;

			case Screen.ScreenLastInput.SecondNumber:
				SetAvaibleButton(true, true, true, true, false);
				break;

			case Screen.ScreenLastInput.Operation:
				if (AllBinaryOperationsAdded)
				{
					SetAvaibleButton(true, false, true, false, true);
				}
				else
				{
					SetAvaibleButton(false, false, true, true, false);
				}
				break;
		}
	}

	public void SetAvaibleButton(bool numbers, bool newNumber, bool unaryOperations, bool binaryOperations, bool calculate)
	{
		DisableNumbers(!numbers);
		DisableNewNumberButtons(!newNumber);
		DisableUnaryOperations(!unaryOperations);
		DisableBinaryOperations(!binaryOperations);
		DisableCalculateButton(!calculate);
	}

	private void Calculate()
	{
		Stack<string> allElements = new Stack<string>(screen.GetAllElements().Reverse());
		Stack<double> numbers = new Stack<double>();

		while (allElements.Count > 0)
		{
			string element = allElements.Pop();
			if (Element.IsNumber(element))
			{
				numbers.Push(Double.Parse(element));
			}
			else if (Element.IsOperation(element))
			{
				double n1 = numbers.Pop();
				string operation = element;
				double result = 0;

				if (Element.IsUnaryOperation(element))
				{
					result = Calculate(n1, operation);
				}
				else if (Element.IsBinaryOperation(element))
				{
					double n2 = numbers.Pop();
					result = Calculate(n2, n1, operation);
				}

				numbers.Push(result);
			}
		}

		string finResult = numbers.Pop().ToString();
		screen.ClearScreen();
		screen.AddToScreen(finResult);

	}

	private double Calculate(double num1, double num2, string operation)
	{
		if (operation == Operation.Add) return num1 + num2;
		else if (operation == Operation.Subtract) return num1 - num2;
		else if (operation == Operation.Multiply) return num1 * num2;
		else if (operation == Operation.Divide) return num1 / num2;
		else if (operation == Operation.Percentage) return num1 * (num2 / 100);
		else if (operation == Operation.Power) return Math.Pow(num1, num2);
		else return 0;
	}

	private double Calculate(double num1, string operation)
	{
		if (operation == Operation.Root) return Math.Sqrt(num1);
		else return 0;
	}

	private void Clear() => screen.ClearScreen();

	// Unity
	private void Start()
	{
		foreach (ButtonNumber buttonNumber in FindObjectsOfType<ButtonNumber>())
		{
			numberButtons.Add(buttonNumber);
		}

		foreach (ButtonOperation buttonOperation in FindObjectsOfType<ButtonOperation>())
		{
			string operationCode = buttonOperation.GetOperationCode();

			if (Element.IsNewNumber(operationCode))
			{
				newNumberButtons.Add(buttonOperation);
			}
			else if (Element.IsUnaryOperation(operationCode))
			{
				unaryOperationButtons.Add(buttonOperation);
			}
			else if (Element.IsBinaryOperation(operationCode))
			{
				binaryOperationButtons.Add(buttonOperation);
			}
		}

		calculate.onClick.AddListener(Calculate);
		clear.onClick.AddListener(Clear);
		screen.OnStatusChanged += ScreenStatusChanged;
		ScreenStatusChanged(screen.LastInput);
	}
}