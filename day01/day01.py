import os

with open(os.path.dirname(os.path.realpath(__file__)) + "/day01-input.txt", "r") as f:
    input = [int(line) for line in f.readlines()]

# Should really check if i = j and i = j = k
# but assume it won't happen

p1 = 0; p2 = 0
for i in input:
    for j in input:
        if p1 == 0 and i + j == 2020:
            p1 = i * j

        for k in input:
            if p2 == 0 and i + j + k == 2020:
                p2 = i * j * k
                break

        if p1 > 0 and p2 > 0:
            break
    if p1 > 0 and p2 > 0:
        break

print("Part 1: ", p1)
print("Part 2: ", p2)
