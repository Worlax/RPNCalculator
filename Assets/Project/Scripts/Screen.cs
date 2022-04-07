using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class Screen : MonoBehaviour
{
	public enum ScreenLastInput
	{
		None,
		FirstNumber,
		Space,
		SecondNumber,
		Operation
	}

#pragma warning disable 0649

	[SerializeField] TextMeshProUGUI text;

#pragma warning restore 0649

	public ScreenLastInput LastInput { get; private set; }
	public event Action<ScreenLastInput> OnStatusChanged;

	public void AddToScreen(string str)
	{
		if (str == "") return;

		if (Element.IsNumber(str))
		{
			double number = Double.Parse(str);
			number = Math.Round(number, 2);
			str = number.ToString();

			if (Element.IsOperation(GetLastElement()))
			{
				text.text += " ";
			}
		}
		else if (Element.IsOperation(str))
		{
			RemoveNumberSpace();
			text.text += " ";
		}
		text.text += str;

		PrintLogDebug();
		UpdateStatus();
	}

	private void PrintLogDebug()
	{
		string fullString = "\"" + text.text + "\"";
		string numbers = "";
		string unaryOperations = "";
		string binaryOperations = "";

		foreach (double str in GetAllNumbers())
		{
			if (numbers.Length > 0) numbers += " ";
			numbers += "\"" + str + "\"";
		}
		foreach (string str in GetAllUnaryOperations())
		{
			if (unaryOperations.Length > 0) unaryOperations += " ";
			unaryOperations += "\"" + str + "\"";
		}
		foreach (string str in GetAllBinaryOperations())
		{
			if (binaryOperations.Length > 0) binaryOperations += " ";
			binaryOperations += "\"" + str + "\"";
		}

		print("Full string: " + fullString);
		print("Numbers: " + numbers);
		print("Unary operations: " + unaryOperations);
		print("Binary operations: " + binaryOperations);
		print("----------------------------------------------");
	}

	public void ClearScreen()
	{
		text.text = "";

		UpdateStatus();
	}

	public IEnumerable<string> GetAllElements()
	{
		return text.text.Split(' ', '\t');
	}

	public IEnumerable<double> GetAllNumbers()
	{
		List<string> numbersInString = GetAllElements().Where(x => Element.IsNumber(x)).ToList();
		return numbersInString.Select(x => Double.Parse(x));
	}

	public IEnumerable<string> GetAllOperations()
	{
		return GetAllElements().Where(x => Element.IsOperation(x));
	}

	public IEnumerable<string> GetAllUnaryOperations()
	{
		return GetAllOperations().Where(x => Element.IsUnaryOperation(x));
	}

	public IEnumerable<string> GetAllBinaryOperations()
	{
		return GetAllOperations().Where(x => Element.IsBinaryOperation(x));
	}

	private string GetLastElement()
	{
		IEnumerable<string> elements = GetAllElements();
		return elements.Count() != 0 ? elements.Last() : "";
	}

	private void RemoveNumberSpace()
	{
		int textLength = text.text.Length;
		if (textLength > 0 && text.text[textLength - 1].ToString() == Operation.NextNumber)
		{
			text.text = text.text.Remove(textLength - 1);
		}
	}

	private void UpdateStatus()
	{
		if (text.text == "")
		{
			LastInput = ScreenLastInput.None;
		}
		else
		{
			char lastChar = text.text[text.text.Length - 1];

			if (Char.IsNumber(lastChar))
			{
				if (LastInput == ScreenLastInput.None)
				{
					LastInput = ScreenLastInput.FirstNumber;
				}
				else if (LastInput == ScreenLastInput.Space || LastInput == ScreenLastInput.Operation)
				{
					LastInput = ScreenLastInput.SecondNumber;
				}
			}
			else if (lastChar == '\t')
			{
				LastInput = ScreenLastInput.Space;
			}
			else
			{
				LastInput = ScreenLastInput.Operation;
			}
		}

		OnStatusChanged?.Invoke(LastInput);
	}

	// Events
	private void ButtonNumberClicked(int number)
	{
		AddToScreen(number.ToString());
	}

	private void ButtonOperationClicked(string operation)
	{
		AddToScreen(operation);
	}

	// Unity
	private void Start()
	{
		ButtonNumber.OnButtonNumberClicked += ButtonNumberClicked;
		ButtonOperation.OnButtonOperationClicked += ButtonOperationClicked;
	}
}