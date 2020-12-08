package main

import (
	"fmt"
	"log"
	"strconv"
	"strings"

	aocutils "../common"
)

func main() {
	input, err := aocutils.GetInputStrings("./day08-input.txt")
	if err != nil {
		log.Fatal(err)
	}

	p1, _ := runProgram(input)
	fmt.Println("Part 1: ", p1)

	p2, faultIndex := 0, -1
	var patchedSource []string
	for {
		patchedSource, faultIndex = patchInput(input, faultIndex)
		p2, err = runProgram(patchedSource)
		if err == nil {
			break
		}
	}
	fmt.Println("Part 2: ", p2)
}

func patchInput(input []string, lastPatchIndex int) ([]string, int) {
	result := make([]string, len(input))
	patchIndex := -1
	for i, s := range input {
		if i > lastPatchIndex && patchIndex < 0 {
			switch s[0:3] {
			case "nop":
				result[i] = strings.Replace(s, "nop", "jmp", 1)
				patchIndex = i
			case "jmp":
				result[i] = strings.Replace(s, "jmp", "nop", 1)
				patchIndex = i
			default:
				result[i] = s
			}
			continue
		}
		//...else
		result[i] = s
	}
	return result, patchIndex
}

func runProgram(input []string) (int, error) {
	ip, acc := 0, 0
	iplog := map[int]int{}

	for {
		parts := strings.Split(strings.TrimSpace(input[ip]), " ")
		instruction := parts[0]
		count, _ := strconv.Atoi(parts[1])

		if _, ok := iplog[ip]; ok {
			return acc, fmt.Errorf("Infinite loop detected at line %d", ip)
		}
		iplog[ip] = 1
		switch instruction {
		case "nop":
			ip++
		case "jmp":
			ip += count
		case "acc":
			acc += count
			ip++
		default:
			return 0, fmt.Errorf("Unhandled program instruction \"%s\" encountered at line %d", instruction, ip)
		}

		if ip >= len(input) || len(input[ip]) == 0 { // In case there's a rogue CR/LF at the end of the input file
			return acc, nil
		}
	}
}
