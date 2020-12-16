package main

import (
	"fmt"

	aocutils "../common"
)

func main() {
	input, _ := aocutils.GetInputIntegers(aocutils.FindInputFile(1))

	var p1, p2 = 0, 0

	for _, i := range input {
		for _, j := range input {

			if p1 == 0 && i+j == 2020 {
				p1 = i * j
			}

			if p2 == 0 && i+j <= 2020 {
				for _, k := range input {
					if i+j+k == 2020 {
						p2 = i * j * k
						break
					}
				}
			}

			if p1 > 0 && p2 > 0 {
				break
			}
		}
		if p1 > 0 && p2 > 0 {
			break
		}
	}

	fmt.Println("Part 1: ", p1)
	fmt.Println("Part 2: ", p2)
}
