package aocutils

import (
	"fmt"
	"io/ioutil"
	"os"
	"path"
	"runtime"
	"strconv"
	"strings"
)

// Try and locate the daily input file file
// Look in the current working directory, then try in a subdirectory named dayXX,
// then try the directory of the calling package, then try the parent directory
// of the calling package, then try a dayXX subdirectory of the calling package
// parent
func FindInputFile(day int) string {
	fnamebase := fmt.Sprintf("day%2.2d-input.txt", day)
	// Current working directory
	fname := fnamebase
	_, err := os.Stat(fname)
	if os.IsNotExist(err) {
		// dayXX subdirectory of cwd
		fname = path.Join(fmt.Sprintf("day%2.2d", day), fnamebase)
		if _, err = os.Stat(fname); os.IsNotExist(err) {
			_, filename, _, ok := runtime.Caller(0)
			if !ok {
				panic(fmt.Sprintf("Can't find caller information while looking for day %d input", day))
			}
			// Directory of calling package
			fname = path.Join(path.Dir(filename), fnamebase)
			if _, err = os.Stat(fname); os.IsNotExist(err) {
				// Parent directory of calling package
				fname = path.Join(path.Dir(filename), "..", fnamebase)
				if _, err = os.Stat(fname); os.IsNotExist(err) {
					// dayXX subdirectory of parent dir of calling package
					fname = path.Join(path.Dir(filename), "..", fmt.Sprintf("day%2.2d", day), fnamebase)
					if _, err = os.Stat(fname); os.IsNotExist(err) {
						panic(fmt.Sprintf("Unable to find input file for day %d (%s)", day, fname))
					}
				}
			}
		}
	}
	return fname
}

func GetInputStrings(inputFile string) ([]string, error) {
	txt, err := ioutil.ReadFile(inputFile)
	if err != nil {
		return nil, err
	}
	src := strings.Replace(string(txt), "\r\n", "\n", -1)
	// Strip the last line if it is empty
	src = strings.TrimSuffix(src, "\n")
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

func GetInputInt64(inputFile string) ([]int64, error) {
	input, err := GetInputStrings(inputFile)
	if err != nil {
		return nil, err
	}

	result := make([]int64, len(input))
	for i, s := range input {
		v, err := strconv.ParseInt(s, 10, 64)
		if err != nil {
			return nil, err
		}
		result[i] = v
	}

	return result, nil
}

func MaxInt64(values ...int64) int64 {
	if len(values) == 0 {
		panic("No values passed to Max func")
	}

	result := values[0]
	for _, v := range values {
		if result < v {
			result = v
		}
	}
	return result
}

func MinInt64(values ...int64) int64 {
	if len(values) == 0 {
		panic("No values passed to Min func")
	}

	result := values[0]
	for _, v := range values {
		if result > v {
			result = v
		}
	}
	return result
}

func MinMaxInt64(values ...int64) (int64, int64) {
	if len(values) == 0 {
		panic("No values passed to MinMax func")
	}

	min := values[0]
	max := values[0]
	for _, v := range values {
		if v < min {
			min = v
		}
		if v > max {
			max = v
		}
	}
	return min, max
}
