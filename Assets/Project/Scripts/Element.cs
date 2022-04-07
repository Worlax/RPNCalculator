using System;

public class Element
{
	public static bool IsNumber(string str) => str.Length != 0 && Char.IsDigit(str[str.Length - 1]);
	public static bool IsNewNumber(string str) => str.Length != 0 && (str[0] == ' ' || str[0] == '\t');
	public static bool IsOperation(string str)
	{
		if (str == Operation.Add || str == Operation.Subtract || str == Operation.Multiply
			|| str == Operation.Divide || str == Operation.Percentage || str == Operation.Power
			|| str == Operation.Root)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public static bool IsBinaryOperation(string str) => IsOperation(str) && !IsUnaryOperation(str);
	public static bool IsUnaryOperation(string str)
	{
		if (str == Operation.Root)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
}