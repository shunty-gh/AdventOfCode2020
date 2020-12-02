import re

pattern = r'^(\d+)-(\d+) (\w): (\w+)$'

with open("./day02-input.txt", "r") as f:
    matches = [re.match(pattern, line) for line in f.readlines()]

p1 = 0; p2 = 0
for match in matches:
    lb = int(match.group(1))
    ub = int(match.group(2))
    ch = match.group(3)
    pwd = match.group(4)

    # Part 1
    chcount = pwd.count(ch)
    if chcount >= lb and chcount <= ub:
        p1 += 1

    # Part 2
    c1 = pwd[lb - 1]; c2 = pwd[ub - 1]
    if c1 != c2 and (c1 == ch or c2 == ch):
        p2 += 1

print("Part 1: ", p1)
print("Part 2: ", p2)
