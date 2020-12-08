package main

import (
	"fmt"
	"log"
	"strconv"
	"strings"

	aocutils "../common"
)

func main() {
	input, err := aocutils.GetInputStrings("./day07-input.txt")
	//input, err := aocutils.GetInputStrings("./day07-input-test.txt")
	if err != nil {
		log.Fatal(err)
	}

	var bags = buildMap(input)
	fmt.Println("Part 1: ", len(parents(bags, "shiny gold")))
	fmt.Println("Part 2: ", countContained(bags, "shiny gold"))
}

func buildMap(input []string) map[string][]childbag {
	result := make(map[string][]childbag, len(input))
	for _, s := range input {
		if len(s) == 0 {
			continue
		}
		var splits = strings.Split(strings.Trim(s, "."), " bags contain ")
		ss := strings.Replace(strings.Replace(splits[1], " bags", "", -1), " bag", "", -1)
		if strings.Contains(ss, "no other") {
			result[splits[0]] = make([]childbag, 0)
			continue
		}
		ch := strings.Split(ss, ", ")
		children := make([]childbag, len(ch))
		for i, c := range ch {
			count, _ := strconv.Atoi(strings.Split(c, " ")[0])
			name := c[strings.Index(c, " ")+1:]
			children[i] = childbag{count, name}
		}
		result[splits[0]] = children
	}
	return result
}

func parents(bags map[string][]childbag, rootKey string) map[string]bool {
	result := make(map[string]bool)
	tovisit := []string{rootKey}

	for {
		tofind := tovisit[0]
		for key, childbags := range bags {
			if _, ok := result[key]; ok {
				continue
			}

			for _, child := range childbags {
				if child.Name == tofind {
					result[key] = true
					tovisit = append(tovisit, key)
					break
				}
			}
		}

		if len(tovisit) == 1 {
			break
		}
		tovisit = tovisit[1:]
	}
	return result
}

func countContained(bags map[string][]childbag, rootKey string) int {
	childbags := bags[rootKey]
	result := 0
	for _, cb := range childbags {
		result += cb.Count + (cb.Count * countContained(bags, cb.Name))
	}
	return result
}

type childbag struct {
	Count int
	Name  string
}
