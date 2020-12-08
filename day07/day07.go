package main

import (
	"fmt"
	"log"
	"strconv"
	"strings"

	aocutils "../common"
)

type bag struct {
	Name        string
	ContainedBy []string       // aka Parents
	Contains    map[string]int // aka Children
}

func main() {
	input, err := aocutils.GetInputStrings("./day07-input.txt")
	if err != nil {
		log.Fatal(err)
	}

	bags := buildBaggage(input)
	fmt.Println("Part 1 (BFS):       ", len(getContainersBFS(bags, "shiny gold")))
	fmt.Println("Part 1 (Recursion): ", len(getContainersRecurse(bags, "shiny gold")))
	fmt.Println("Part 2 (Recursion): ", getContainedRecurse(bags, "shiny gold"))
}

func buildBaggage(input []string) map[string]*bag {
	result := make(map[string]*bag, len(input))
	for _, s := range input {
		if len(s) == 0 {
			continue
		}

		splits := strings.Split(strings.Trim(s, "."), " bags contain ")
		bagname := splits[0]

		if _, ok := result[bagname]; !ok {
			result[bagname] = &bag{bagname, []string{}, map[string]int{}}
		}
		if strings.Contains(splits[1], "no other") {
			continue
		}
		ss := strings.Replace(strings.Replace(splits[1], " bags", "", -1), " bag", "", -1)
		contains := strings.Split(ss, ", ")
		for _, child := range contains {
			count, _ := strconv.Atoi(strings.Split(child, " ")[0])
			childname := child[strings.Index(child, " ")+1:]

			result[bagname].Contains[childname] = count
			if _, ok := result[childname]; !ok {
				result[childname] = &bag{childname, []string{}, map[string]int{}}
			}
			result[childname].ContainedBy = append(result[childname].ContainedBy, bagname)
		}
	}
	return result
}

func getContainersBFS(bags map[string]*bag, bagName string) map[string]bool {
	result := make(map[string]bool)
	tovisit := []string{bagName}

	for {
		thisbag := bags[tovisit[0]]
		for _, parent := range thisbag.ContainedBy {
			if _, ok := result[parent]; ok {
				continue
			}

			result[parent] = true
			tovisit = append(tovisit, parent)
		}

		if len(tovisit) == 1 {
			break
		}
		tovisit = tovisit[1:]
	}
	return result
}

func appendDistinct(first map[string]bool, second map[string]bool) map[string]bool {
	for s, v := range second {
		if _, ok := first[s]; ok {
			continue
		}
		first[s] = v
	}
	return first
}

func getContainersRecurse(bags map[string]*bag, bagName string) map[string]bool {
	result := map[string]bool{}
	bag := bags[bagName]
	for _, parent := range bag.ContainedBy {
		if _, ok := result[parent]; ok {
			continue
		}
		result[parent] = true
		result = appendDistinct(result, getContainersRecurse(bags, parent))
	}
	return result
}

func getContainedRecurse(bags map[string]*bag, bagName string) int {
	bag := bags[bagName]
	result := 0
	for childName, count := range bag.Contains {
		result += count + (count * getContainedRecurse(bags, childName))
	}
	return result
}
