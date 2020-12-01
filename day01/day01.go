package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
)

func main() {
	file, err := os.Open("./day01-input.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	// Get a list of integer values
	var input []int
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		val, _ := strconv.Atoi(scanner.Text())
		input = append(input, val)
	}

	var p1, p2 = 0, 0

	for _, i := range input {
		for _, j := range input {

			if p1 == 0 && i+j == 2020 {
				p1 = i * j
			}

			if p2 == 0 {
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
