mkdir day%1
copy day-template.cs day%1\day%1.cs
sed -i 's/Day00/Day%1/g' day%1/day%1.cs
