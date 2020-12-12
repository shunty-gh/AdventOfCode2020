package main

import (
	"fmt"
	"strconv"

	aocutils "../common"
)

func main() {
	input, _ := aocutils.GetInputStrings(aocutils.FindInputFile(12))

	fmt.Println("Part 1: ", part1(input))
	fmt.Println("Part 2: ", part2(input))
}

func part1(input []string) int {
	x, y, face := 0, 0, 90
	for _, direction := range input {
		d := direction[0]
		count, _ := strconv.Atoi(direction[1:])

		switch d {
		case 'N':
			y += count
		case 'S':
			y -= count
		case 'W':
			x -= count
		case 'E':
			x += count
		case 'L':
			face = (360 + face - count) % 360
		case 'R':
			face = (face + count) % 360
		case 'F':
			if face == 0 {
				y += count
			} else if face == 90 {
				x += count
			} else if face == 180 {
				y -= count
			} else if face == 270 {
				x -= count
			}
		}
	}
	return aocutils.AbsInt(x) + aocutils.AbsInt(y)
}

func part2(input []string) int {
	wX, wY, sX, sY := 10, 1, 0, 0
	for _, direction := range input {
		d := direction[0]
		count, _ := strconv.Atoi(direction[1:])

		switch d {
		case 'N':
			wY += count
		case 'S':
			wY -= count
		case 'W':
			wX -= count
		case 'E':
			wX += count
		case 'L':
			wX, wY = rotate(wX, wY, sX, sY, count)
		case 'R':
			wX, wY = rotate(wX, wY, sX, sY, 360-count)
		case 'F':
			// Get waypoint distance from ship
			dx := wX - sX
			dy := wY - sY
			// Move ship 'count' times in direction of waypoint
			sX += (dx * count)
			sY += (dy * count)
			// Maintain waypoint distance from ship
			wX = sX + dx
			wY = sY + dy
		}
	}
	return aocutils.AbsInt(sX) + aocutils.AbsInt(sY)
}

// rotate clockwise through 0, 90, 180 or 270 around a non-zero origin
func rotate(pX, pY, originX, originY, angle int) (int, int) {
	offX, offY := pX-originX, pY-originY
	switch angle {
	case 0:
		return pX, pY
	case 90:
		return -offY + originX, offX + originY
	case 180:
		return -offX + originX, -offY + originY
	case 270:
		return offY + originX, -offX + originY
	default:
		panic("Unexpected angle to rotate")
	}
}
