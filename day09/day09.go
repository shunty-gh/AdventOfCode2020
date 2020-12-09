package main

import (
	"fmt"
	"log"

	aocutils "../common"
)

func main() {
	input, err := aocutils.GetInputInt64(aocutils.FindInputFile(9))
	if err != nil {
		log.Fatal(err)
	}

	rangeLen := 25
	var part1 int64
	for i := rangeLen; i < len(input); i++ {
		if !checkTarget(input[i-rangeLen:i], input[i]) {
			part1 = input[i]
			break
		}
	}
	fmt.Println("Part 1: ", part1)
	fmt.Println("Part 2: ", findContiguous(input, part1))
}

func checkTarget(source []int64, target int64) bool {
	for i := 0; i < len(source); i++ {
		for j := 0; j < len(source); j++ {
			if i != j && source[i]+source[j] == target {
				return true
			}
		}
	}
	return false
}

// Sum over a sliding window. Assumes there will be a solution
// before ub overflows
func findContiguous(source []int64, target int64) int64 {
	sum := int64(0)
	lb, ub := 0, 0
	for ; sum != target; ub++ {
		sum += source[ub]

		for ; sum > target; lb++ {
			sum -= source[lb]
		}
	}
	min, max := aocutils.MinMaxInt64(source[lb:ub]...)
	return min + max
}
