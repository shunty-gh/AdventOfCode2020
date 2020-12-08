package aocutils

import (
	"io/ioutil"
	"strconv"
	"strings"
)

func GetInputStrings(inputFile string) ([]string, error) {
	txt, err := ioutil.ReadFile(inputFile)
	if err != nil {
		return nil, err
	}
	src := strings.Replace(string(txt), "\r\n", "\n", -1)
	return strings.Split(src, "\n"), nil
}

func GetInputIntegers(inputFile string) ([]int, error) {
	input, err := GetInputStrings(inputFile)
	if err != nil {
		return nil, err
	}

	result := make([]int, len(input))
	for i, s := range input {
		v, err := strconv.Atoi(strings.Trim(s, "\r\n"))
		if err != nil {
			return nil, err
		}
		result[i] = v
	}

	return result, nil
}
