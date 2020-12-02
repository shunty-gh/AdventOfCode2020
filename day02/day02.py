import re
import os

pattern = r'^(\d+)-(\d+) (\w): (\w+)$'

with open(os.path.dirname(os.path.realpath(__file__)) + "/day02-input.txt", "r") as f:
    matches = [re.match(pattern, line) for line in f.readlines()]

p1 = 0; p2 = 0
for match in matches:
    lb, ub = [int(i) for i in match.group(1, 2)]
    ch, pwd = match.group(3, 4)

    # Part 1
    p1 += int(pwd.count(ch) in range(lb, ub + 1))

    # Part 2 - the password rules are not 0-indexed
    p2 += int((pwd[lb - 1] == ch) ^ (pwd[ub - 1] == ch))


print("Part 1: ", p1)
print("Part 2: ", p2)
