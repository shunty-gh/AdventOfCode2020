<Query Kind="Program" />

void Main()
{
	/// Implementation of a [shunting yard algorithm](https://en.wikipedia.org/wiki/Shunting-yard_algorithm)
	/// using user defined operator precedence.
	/// As seen in [AoC 2020 day 18](https://adventofcode.com/2020/day/18)

	//var s1 = "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"; // P1 => 13632; P2 => 23340
	var s1 = "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"; // P1 => 12240; P2 => 669060

	var outq = new Queue<(Int64, char?)>();
	var op = new Stack<char>();
	var expr = s1;
	var i = 0;
	while (i < expr.Length)
	{
		var ch = expr[i++];
		if (ch == ' ') continue;

		if (ch >= '0' && ch <= '9')
		{
			outq.Enqueue(((int)Char.GetNumericValue(ch), null));
		}
		else if (ch == '(')
		{
			op.Push(ch);
		}
		else if (ch == ')')
		{
			var top = op.Peek();
			while (top != '(')
			{
				outq.Enqueue((0, op.Pop()));
				top = op.Peek();
			}
			if (top == '(')
			{
				op.Pop();
			}
		}
		else if (ch == '+' || ch == '*')
		{
			if (op.Count > 0)
			{
				var top = op.Peek();
				while ((top == '+' || top == '*')
					&& (HasHigherOperatorPrecedence(top, ch)
						|| (HasEqualOperatorPrecedence(top, ch) && IsOperatorLeftAssociative(top)))
					&& (top != ')'))
				{
					outq.Enqueue((0, op.Pop()));
					top = op.Count > 0 ? op.Peek() : ' ';
				}
			}
			op.Push(ch);
		}
	}
	while (op.Count > 0)
	{
		outq.Enqueue((0, op.Pop()));
	}

	outq.Dump();
	System.Diagnostics.Debug.Assert(op.Count == 0);

	// Unroll the queue
	var st = new Stack<Int64>();
	while (outq.Any())
	{
		var (val, ch) = outq.Dequeue();
		if (ch == null)
		{
			st.Push(val);
		}
		else
		{
			if (ch == '+')
			{
				st.Push(st.Pop() + st.Pop());
			}
			else if (ch == '*')
			{
				st.Push(st.Pop() * st.Pop());
			}
		}
	}

	st.Pop().Dump("Result");
}

// In part 1 of the AoC puzzle all operators have the same precedence
private bool HasEqualOperatorPrecedence(char a, char b) => true;
private bool HasHigherOperatorPrecedence(char a, char b) => false;
// In part 2 of the AoC puzzle '+' takes higher precedence than '*'
//private bool HasEqualOperatorPrecedence(char a, char b) => a == b;
//private bool HasHigherOperatorPrecedence(char a, char b) => (a == '+' && b == '*');

// The AoC puzzle only deals with '+' and '*' operators
private bool IsOperatorLeftAssociative(char op) => op == '*' || op == '+';
