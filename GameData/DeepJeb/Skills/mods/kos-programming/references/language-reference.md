# KerboScript Language Reference

## Core Language Features

**Case Insensitivity:** Everything in KerboScript is case-insensitive, including variable names, filenames, and string comparisons.
```
PRINT "Hello" = "HELLO".   // Returns TRUE
```

**Dynamic Typing (Late Typing):** Variables are dynamically typed. Reassigning a variable to a different data type does not cause an error; the variable's type changes.
```
SET x TO 42.        // x is now a number
SET x TO "hello".   // x is now a string (no error)
```

**Short-Circuiting Booleans:** Evaluation of `AND`/`OR` expressions stops early once the outcome is determined.
```
// If A is TRUE, B is never evaluated
IF A OR B { ... }
```

## Basic Syntax
```
// Comments use double slashes
PRINT "Hello World!".   // Print statement
SET x TO 10.            // Variable assignment
IF x > 5 { PRINT "Big!". }  // Conditional
FOR i IN RANGE(10) { PRINT i. }  // Loop
```

## Variables and Types
```
SET name TO "Kerbin".           // String
SET radius TO 600000.           // Number (float)
SET hasAtmo TO TRUE.            // Boolean
SET myVec TO V(1, 2, 3).       // Vector
```

| Type | Description |
|------|-------------|
| `STRING` | Text (e.g. `"Kerbin"`) |
| `NUMBER` | Floating point (e.g. `3.14`) |
| `BOOLEAN` | True/false |
| `VECTOR` | 3D vector (e.g. `V(1,0,0)`) |
| `LIST` | Ordered collection (e.g. `LIST(1,2,3)`) |
| `LEXICON` | Key-value pairs (e.g. `LEX("key", "value")`) |
| `QUEUE` | FIFO collection |
| `STACK` | LIFO collection |

## Variable Declaration and Scoping

**Explicit Declaration:**
```
LOCAL x IS 1.          // Local variable
GLOBAL y TO 1.         // Global variable
DECLARE PARAMETER p.   // Function parameter
```

**Implicit Declaration (Lazy Global):**
```
SET x TO 1.  // Creates implicit global if not in scope
```

**@LAZYGLOBAL Directive:**
```
@LAZYGLOBAL OFF.   // Must be first line; requires explicit LOCAL/GLOBAL
```
- Prevents accidental implicit globals
- Does not affect `LOCK` statements

**Scoping Rules:**
- `DECLARE` → `LOCAL` (default)
- `FUNCTION` at file top → `GLOBAL`
- `FUNCTION` in braces → `LOCAL`
- `PARAMETER` → Always `LOCAL`
- `LOCK` → Always `GLOBAL`

## Operators
- **Arithmetic:** `+`, `-`, `*`, `/`, `^` (power), `MOD` (modulo)
- **Comparison:** `=`, `<>`, `>`, `<`, `>=`, `<=`
- **Logical:** `AND`, `OR`, `NOT`
- **String:** `+` (concatenation)

## Control Structures
```
// IF-ELSE
IF altitude > 70000 {
    PRINT "In space!".
} ELSE {
    PRINT "Still in atmosphere.".
}

// UNTIL loop (like while)
SET i TO 0.
UNTIL i >= 10 {
    PRINT i.
    SET i TO i + 1.
}

// FOR loop
FOR stageNum IN RANGE(0, 5) {
    PRINT "Stage " + stageNum.
}

// WHEN trigger (event-driven)
WHEN ALTITUDE > 100000 THEN {
    PRINT "Passed 100km!".
    PRESERVE.  // Keep trigger active
}.
```

## User Functions
```
// Declare a function
DECLARE FUNCTION degrees_to_radians {
    DECLARE PARAMETER deg.
    RETURN CONSTANT():PI * deg / 180.
}

// Call the function
SET alpha TO 45.
PRINT alpha + " degrees is " + degrees_to_radians(alpha) + " radians."
```

**Function features:**
- Parameters use `DECLARE PARAMETER` or `PARAMETER`
- Optional parameters with defaults: `DECLARE PARAMETER p1, p2 is 0.`
- Return values with `RETURN`
- Local variables within function scope

## Built-in Name Protection
- kOS **forbids** overwriting built-in names (e.g., `ALTITUDE`, `V()`, `TARGET`)
- Use `@CLOBBERBUILTINS ON.` at file top to override (for legacy compatibility)
