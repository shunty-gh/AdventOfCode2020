package main

import (
	"fmt"
	"log"
	"strconv"
	"strings"

	aocutils "../common"
)

func main() {
	input, err := aocutils.GetInputStrings("./day02-input.txt")
	if err != nil {
		log.Fatal(err)
	}

	var p1, p2 = 0, 0

	for _, s := range input {
		var splits = strings.Split(s, " ")
		var rule, ch, pwd = splits[0], strings.Trim(splits[1], ":"), splits[2]
		var rules = strings.Split(rule, "-")
		ruleA, _ := strconv.Atoi(rules[0])
		ruleB, _ := strconv.Atoi(rules[1])

		// Part 1
		chcount := strings.Count(pwd, ch)
		if chcount >= ruleA && chcount <= ruleB {
			p1++
		}

		// Part 2
		c1, c2 := pwd[ruleA-1], pwd[ruleB-1]
		if c1 != c2 && (c1 == ch[0] || c2 == ch[0]) {
			p2++
		}
	}

	fmt.Println("Part 1: ", p1)
	fmt.Println("Part 2: ", p2)
}
