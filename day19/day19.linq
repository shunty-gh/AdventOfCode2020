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
				var next = rules[r];
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
