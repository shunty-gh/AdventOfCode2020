# Advent Of Code 2020

## Notes for each day
So that I can find specific types of approach if I ever need them again.

### Helpful Links And Notes
* T.B.A

### Other Solutions
* T.B.A

| URL                                      | Language(s) | Notes     |
|------------------------------------------|-------------|-----------|
| https://github.com/Bogdanp/awesome-advent-of-code#project-templates | Many, many | Links and tips |
| https://github.com/kodsnack/advent_of_code_2020 | So many  | Links to many AoC repos |
| https://github.com/arknave/advent-of-code-2020 | Python mainly |  |
| https://github.com/mcpower/adventofcode | Python | No 2020 (yet?) |

### Day 1 - Report Repair
Compare/add/multiply items in list of numbers

### Day 2 - Password Philosophy
Splitting strings, comparing and counting characters

### Day 3 - Toboggan Trajectory
Simple map navigation

### Day 4 - Passport Processing
Substring processing

### Day 5 - Binary Boarding
Convert string to binary

### Day 6 - Custom Customs
Character processing in strings, Distinct/Intersection

### Day 7 - Handy Haversacks
Parent/child relationships. Find parents for child, count children. Recursion (not obligatory). BFS in Go.

### Day 8 - Handheld Halting
3 operation "computer".

### Day 9 - Encoding Error
Find sum, in a list, of previous numbers. Finding sums of contiguous items in a large list. Used a "sliding window" kind of approach.

### Day 10 - Adapter Array
Combinations of integer "adapters" linking together. Calculate total number of potential paths/combinations.

### Day 11 - Seating System
Game of life based around a plane seating system where people move or stay put based on the number of nearby passengers.

### Day 12 - Rain Risk
Navigate a ship around a grid. Follow a "waypoint". Rotate co-ords about a given point; Manhattan distance.

### Day 13 - Shuttle Search
Intersecting schedules of buses.
 Apparently this is based on the [Chinese Remainder Theorem](https://en.wikipedia.org/wiki/Chinese_remainder_theorem). Massive numbers involved so required to do some "proper" algorithms rather than brute force. Turns out my solution is along the lines of [search by sieving](https://en.wikipedia.org/wiki/Chinese_remainder_theorem#Search_by_sieving).
...

### Day 14 - Docking Data
Apply a variable bitmask to 36 bit numbers. Many combinations.

### Day 15 - Rambunctious Recitation
Counting game. Remember previous numbers. Many loops.

### Day 16 - Ticket Translation
List of tickets with fields that could match 1 or more options. Determine, by elimination, which part fits where.

### Day 17 - Conway Cubes
Game of life in 3D and then in 4D.

### Day 18 - Operation Order
Parse numeric expressions and apply non-standard numerical precedence to calculate the results.
Today I learned that this can be solved by using a ["shunting yard algorithm"](https://en.wikipedia.org/wiki/Shunting-yard_algorithm). My original solution was convoluted string processing along with stacks and recursion. Almost on the right lines. Sort of.

### Day 19 - Monster Messages
Recusively derive a regex expression from input rules and apply it to strings. Then modify the regex to allow for *infinite* loops.

### Day 20 - Jurassic Jigsaw
Read blocks of data and determine how to arrange them (by flipping and rotating) into a square based on matching their edges. Then locate matching patterns within the resulting "image".

This took an age. Trying to get my head round the logic to rotate and flip blocks of data so that it matched up with adjacent blocks.

### Day 21 - Allergen Assessment
Determine by elimination the "ingredient" (term/id) associated with a particular "allergen" term.

### Day 22 - Crab Combat
Card game with integer queues and recursion.

### Day 23 - Crab Cups
Circular list, shuffling around. Initial part 1 using arrays but part 2 required 1,000,000 items and 10,000,000 "shuffles" so refactored to a proper circular linked list.

### Day 24 - Lobby Layout
Parse string directions to navigate a hexagonal grid. Flip tiles. Game of life on the hex grid. Implemented (in C#) with a little bit of Parallel processing - improves speed by about 20%.

### Day 25 - Combo Breaker
Trial and error find multiplier. Apply it.
