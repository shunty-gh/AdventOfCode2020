<Query Kind="Program" />

void Main()
{
	/*
	0: 4 1 5
	1: 2 3 | 3 2
	2: 4 4 | 5 5
	3: 4 5 | 5 4
	4: "a"
	5: "b"
	
	ababbb
	bababa
	abbbab
	aaabbb
	aaaabbb
	 */

	//var fn = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day19-input-test.txt");
	var fn = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "day19-input.txt");
	var input = File.ReadAllLines(fn);
	
	var getrules = true;
	var rules = new Dictionary<string,string>();
	var msgs = new List<string>();
	foreach (var line in input)
	{
		if (string.IsNullOrWhiteSpace(line))
		{
			getrules = false;
			continue;
		}
		if (getrules)
		{
			var parts = line.Split(':');
			rules[parts[0]] = parts[1].Trim();
		}
		else
		{
			msgs.Add(line.Trim());
		}
	}
	//rules.Dump();
	//msgs.Dump();
	foreach (var msg in msgs)
	{
		if (!Matches(msg, rules))
		{
			//msg.Dump("not match");
		}
	}
	var exprule = GetRuleZero(rules);
	//exprule.Dump();
	
	var re = new Regex("^" + exprule.Replace(" ", "") + "$");
	var badcount = 0;
	foreach (var msg in msgs)
	{
		if (!re.IsMatch(msg))
		{
			//msg.Dump("Not match");
			badcount++;
		}
	}
	var result = msgs.Count - badcount;
	result.Dump("Part 1");
}

private bool Matches(string msg, Dictionary<string,string> rules, string ruleIndex = "")
{
	var rule = string.IsNullOrWhiteSpace(ruleIndex) ? rules["0"] : rules[ruleIndex];
	return false;
}

private string GetRuleZero(Dictionary<string,string> rules)
{
	var r0 = rules["0"];
	return ExpandRule(r0, rules);
}

private string ExpandRule(string rule, Dictionary<string,string> rules)
{
	var result = "";
	if (rule.Contains("|"))
	{
		var morerules = rule.Split("|");
		foreach (var r in morerules)
		{
			if (string.IsNullOrWhiteSpace(result))
				result += " ( (" + ExpandRule(r, rules) + ") ";
			else
				result += " | (" + ExpandRule(r, rules) + ") ";
		}
		result += " ) ";
	}
	else 
	{
		var morerules = rule.Split(' ');
		if (morerules.Count() == 1)
		{
			var r = morerules[0].Trim();
			if (int.TryParse(r, out var num))
			{	
				// 0: 8 11
				// Part 2 changes:
				//   8: 42    =>  8: 42 | 42 8        => 42 42 8...42 42 42 8 => (42)+ 8
				//  11: 42 31 => 11: 42 31 | 42 11 31 => 42 42 11 31 31... 42 42 42 11 31 31 31
				// 42: 29 104 | 115 95
				// 31: 95 7 | 104 130
				var next = rules[r];
				if (r == "8")
				{
					var tmp = "(" + ExpandRule(next, rules) + ")+ ";
					//tmp.Dump("8 expands to");
					return tmp;
				}
				else if (r == "11")
				{
					var r42 = ExpandRule("42", rules);
					//r42.Dump("R42");
					//var r31 = ExpandRule("31", rules);
					//return "((" + r42 + r31 + ")"
					//	+ "|(" + r42 + r42 + r31 + r31 + ")"
					//	+ "|(" + r42 + r42 + r42 + r31 + r31 + r31 + ")"
					//	+ "|(" + r42 + r42 + r42 + r42 + r31 + r31 + r31 + r31 + ")"
					//	+ "|(" + r42 + r42 + r42 + r42 + r42 + r31 + r31 + r31 + r31 + r31 + ")"
					//	+ "|(" + r42 + r42 + r42 + r42 + r42 + r42 + r31 + r31 + r31 + r31 + r31 + r31 + "))";
					var replaced = "42 31 "
						+ "| 42 42 31 31 "
						+ "| 42 42 42 31 31 31 "
						+ "| 42 42 42 42 31 31 31 31 "
						+ "| 42 42 42 42 42 31 31 31 31 31"
						+ "| 42 42 42 42 42 42 31 31 31 31 31 31"
						+ "| 42 42 42 42 42 42 42 31 31 31 31 31 31 31";
					return ExpandRule(replaced, rules);
				}
				return ExpandRule(next, rules);
			}
			// otherwise it's a letter. EOT.
			return morerules[0].Trim('"');
		}
		else
		{
			foreach (var r in morerules)
			{
				result += ExpandRule(r, rules);
			}
		}
	}
	return result;
}
