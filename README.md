# Advent Of Code 2020

Some code to help find solutions to the [AdventOfCode 2020](https://adventofcode.com/2020).

## C#
The C# code is written using [.Net 5](https://dotnet.microsoft.com/download/dotnet/5.0). As such it is cross-platform so should run anywhere that
.Net 5 or greater is supported. It is massively over-engineered just for the sake of it.

### Pre-requisites
* [.Net 5](https://dotnet.microsoft.com/download/dotnet/5.0) installed (or [newer release](https://dotnet.microsoft.com/download/dotnet), if any)

The simplest way to run is:

```
$> dotnet run
```

from the project root directory, which will run the solution for the current day (or 25th if we've gone beyond the last day of the challenges).

Alternatively, extra options can be added to the command line. These can be seen by running:
```
$> dotnet run -- -h
```

For example, to run all available solutions to date:
```
$> dotnet run -- -a
```

or, to run specific day or days (if available):

```
$> dotnet run -- --days 1 3 16
or
$> dotnet run -- -d 1 3 16
```

or, to run using test data (if supplied) rather than real puzzle data:
```
$> dotnet run -- -d 1 -t
```
Test data should be supplied in a file in the same directory as the real data and in the same content format, with a file name of `dayXX-input-test.txt` (where XX is the day number eg 01, 02, 23 etc)


The ```--``` may be needed on the command line to indicate to the `dotnet run` command that further parameters should be passed to the compiled code rather than processed by the dotnet command.

### Build with older dotnet core
It may be possible to build and run all the code using the older Dotnet Core 3.1 SDK.
Replace
```
<TargetFramework>net5.0</TargetFramework>
```
with
```
<TargetFramework>netcoreapp3.1</TargetFramework>
```
or
```
<TargetFrameworks>net5.0;netcoreapp3.1</TargetFrameworks>
```

When supplying multiple target frameworks you need to specify which framework to use with the `--framework net5.0` or `--framework netcoreapp3.1` on the command line just after the `dotnet run` command.

## Python

### Pre-requisites
* A working installation of Python!
  Python 3 was used but some (all?) of the scripts will work ok with Python 2.

To run the Python scripts (where XX is 01, 02 etc):

```python
$> cd <repo_root>/dayXX
$> python3 dayXX.py
```

or, from the repository/project root folder

```python
$> python3 ./dayXX/dayXX.py
```


## Go

### Pre-requisites
* A working installation of [Go](https://golang.org/)!
  Code was written using Go 1.15.x.

To run the Go programs (where XX is 01, 02 etc):

```go
$> cd <repo_root>/dayXX
$> go run ./dayXX.go
```
