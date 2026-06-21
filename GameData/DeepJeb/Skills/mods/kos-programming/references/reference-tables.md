# kOS Reference Tables

## Built-in Variables (Bound Variables)

### Ship Information
| Variable | Description |
|----------|-------------|
| `SHIP:ALTITUDE` | Current altitude (meters) |
| `SHIP:VELOCITY:ORBIT` | Orbital velocity vector |
| `SHIP:VELOCITY:SURFACE` | Surface velocity vector |
| `SHIP:MASS` | Current mass (kg) |
| `SHIP:DRYMASS` | Dry mass (kg) |
| `SHIP:TWR` | Current thrust-to-weight ratio |
| `SHIP:MAXTHRUST` | Maximum thrust (N) |
| `SHIP:AVAILABLETHRUST` | Available thrust (N) |
| `SHIP:SPECIFICIMPULSE` | Current Isp (seconds) |
| `SHIP:ORBIT` | Current orbit object |
| `SHIP:BODY` | Current celestial body |

### Orbit Information
| Variable | Description |
|----------|-------------|
| `SHIP:ORBIT:PERIAPSIS` | Periapsis altitude |
| `SHIP:ORBIT:APOAPSIS` | Apoapsis altitude |
| `SHIP:ORBIT:INCLINATION` | Orbital inclination |
| `SHIP:ORBIT:ECCENTRICITY` | Orbital eccentricity |
| `SHIP:ORBIT:PERIOD` | Orbital period (seconds) |
| `SHIP:ORBIT:TIMEAPOAPSIS` | Time to apoapsis |
| `SHIP:ORBIT:TIMEPERIAPSIS` | Time to periapsis |
| `SHIP:ORBIT:SEMIMAJORAXIS` | Semi-major axis |

### Flight Information
| Variable | Description |
|----------|-------------|
| `SHIP:Q` | Dynamic pressure |
| `SHIP:MACH` | Mach number |
| `SHIP:AIRSPEED` | Airspeed (m/s) |
| `SHIP:VERTICALSPEED` | Vertical speed (m/s) |
| `SHIP:GROUNDSPEED` | Ground speed (m/s) |
| `SHIP:HEADING` | Current heading (degrees) |

### Resource Information
| Variable | Description |
|----------|-------------|
| `SHIP:LIQUIDFUEL` | Current liquid fuel |
| `SHIP:OXIDIZER` | Current oxidizer |
| `SHIP:ELECTRICCHARGE` | Current electric charge |
| `SHIP:MONOPROPELLANT` | Current monopropellant |
| `SHIP:SOLIDFUEL` | Current solid fuel |

### Control Inputs
| Variable | Description |
|----------|-------------|
| `SHIP:CONTROL:PILOT` | Pilot pitch input |
| `SHIP:CONTROL:PILOTYAW` | Pilot yaw input |
| `SHIP:CONTROL:PILOTROLL` | Pilot roll input |
| `SHIP:CONTROL:PILOTTHROTTLE` | Pilot throttle input |

### Structure Suffixes
- Structures can be chained: `SHIP:VELOCITY:ORBIT:MAG`
- Access sub-elements with colon (`:`) operator
- Some suffixes are settable: `SET SHIP:CONTROL:PILOT TO 0.5.`
- Structure methods callable: `SHIP:PARTS():LENGTH`

---

## Math Library

### Vector Operations
```
SET a TO V(1, 0, 0).
SET b TO V(0, 1, 0).

a + b           // Vector addition
a - b           // Vector subtraction
a * 2           // Scalar multiplication
V(1,2,3)        // Create vector
a:MAG           // Magnitude (length)
a:NORMALIZED    // Unit vector
a:DOT(b)        // Dot product
a:CROSS(b)      // Cross product
ANGLEOF(a, b)   // Angle between vectors
```

### Trigonometric Functions
| Function | Description |
|----------|-------------|
| `SIN(degrees)` | Sine |
| `COS(degrees)` | Cosine |
| `TAN(degrees)` | Tangent |
| `ARCSIN(value)` | Arcsine (degrees) |
| `ARCCOS(value)` | Arccosine (degrees) |
| `ARCTAN(value)` | Arctangent (degrees) |
| `ARCTAN2(y, x)` | Arctangent with two arguments |

### Other Math Functions
| Function | Description |
|----------|-------------|
| `SQRT(x)` | Square root |
| `ABS(x)` | Absolute value |
| `ROUND(x)` | Round to nearest integer |
| `ROUND(x, places)` | Round to decimal places |
| `MIN(a, b)` | Minimum |
| `MAX(a, b)` | Maximum |
| `CLAMP(x, min, max)` | Clamp value |
| `LN(x)` | Natural logarithm |
| `LOG10(x)` | Base-10 logarithm |
| `RANDOM()` | Random number 0-1 |

### Constants
```
CONSTANT():PI    // Pi (3.14159...)
CONSTANT():E     // Euler's number
CONSTANT():G     // Gravitational constant
```

---

## List Operations

### Creating Lists
```
SET myList TO LIST().
myList:ADD("first").
myList:ADD("second").
myList:ADD("third").

// Or initialize with values
SET myList TO LIST("a", "b", "c").
```

### List Methods
| Method | Description |
|--------|-------------|
| `myList:LENGTH` | Number of elements |
| `myList:ADD(item)` | Add to end |
| `myList:INSERT(i, item)` | Insert at index |
| `myList:REMOVE(i)` | Remove at index |
| `myList:CLEAR()` | Remove all |
| `myList:INDEX(i)` | Get element at index |
| `myList:CONTAINS(item)` | Check if contains |
| `myList:POSITION(item)` | Get index of item |
| `myList:COPY()` | Create a copy |
| `myList:SORT()` | Sort in place |
| `myList:REVERSE()` | Reverse in place |
| `myList:SUBLIST(start, count)` | Get sublist |

---

## File System

### Volumes
```
SET vol TO PROCESSOR:VOLUME.  // Get processor's volume
SET vol TO "0".               // By volume number
SET vol TO PATH("myfolder").  // By path

// List files
LIST FILES IN fileList.
FOR f IN fileList { PRINT f. }
```

### Reading and Writing
```
// Write to file
SET file TO vol:CREATE("myScript.ks").
file:WRITE("PRINT \"Hello from file!\".").

// Read from file
SET file TO vol:OPEN("myScript.ks").
PRINT file:READALL().

// Append to file
file:WRITE("PRINT \"More content!\"."):APPEND.
```

### File Methods
| Method | Description |
|--------|-------------|
| `file:EXISTS` | Boolean: file exists |
| `file:NAME` | String: file name |
| `file:SIZE` | Number: file size in bytes |
| `file:MODTIME` | Number: last modification time |
| `file:READ()` | Read entire file |
| `file:READ(n)` | Read n bytes |
| `file:WRITE(content)` | Write content |
| `file:WRITE(content):APPEND` | Append content |
| `file:CLOSE()` | Close file handle |

### JSON Support
```
// Write JSON
SET config TO LEX("fuel", 100, "name", "test").
file:WRITEJSON(config).

// Read JSON
SET data TO file:READJSON().
PRINT data:fuel.
```
