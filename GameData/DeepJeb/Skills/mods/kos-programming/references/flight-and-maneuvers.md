# kOS Flight Control, Triggers & Maneuvers

## Flight Control

### Steering
```
// Point at a target direction
LOCK STEERING TO PROGRADE.         // Point prograde
LOCK STEERING TO RETROGRADE.       // Point retrograde
LOCK STEERING TO NORTH.            // Point north
LOCK STEERING TO HEADING(90, 45).  // 90° east, 45° up
LOCK STEERING TO UP.               // Point straight up
LOCK STEERING TO SRFPROGRADE.      // Surface prograde

// Point at a specific vector
LOCK STEERING TO V(0, 1, 0).

// Point at a target
LOCK STEERING TO TARGET:DIRECTION.
```

### Throttle
```
LOCK THROTTLE TO 1.0.    // Full throttle
LOCK THROTTLE TO 0.5.    // Half throttle
LOCK THROTTLE TO 0.0.    // Cut engines

// Dynamic throttle control
LOCK THROTTLE TO MIN(1, 2 * SHIP:MASS * 9.81 / SHIP:MAXTHRUST).
```

### Staging
```
STAGE.                    // Activate next stage
WAIT UNTIL STAGE:READY.   // Wait for staging to complete

// Check current stage resources
PRINT STAGE:LIQUIDFUEL.
PRINT STAGE:SOLIDFUEL.
```

### RCS and SAS
```
SET RCS TO TRUE.          // Enable RCS
SET RCS TO FALSE.         // Disable RCS
SET SAS TO TRUE.          // Enable SAS
SET SASMODE TO "MANEUVER". // Set SAS mode
```

### Action Groups
```
TOGGLE AG1.    // Toggle action group 1
AG1 ON.        // Turn on action group 1
AG1 OFF.       // Turn off action group 1
```

---

## Triggers and Events

### WHEN Triggers
```
WHEN ALTITUDE > 100000 THEN {
    PRINT "Passed 100km!".
    PRESERVE.  // Keep trigger active after firing
}.
```
- Triggers interrupt normal flow to run code when a condition is met
- Then resume execution
- Can use local variables (since v1.1.0)

### ON Triggers
```
ON THROTTLE {
    PRINT "Throttle changed to " + THROTTLE.
}.
```
- Fires when the watched variable changes

### WAIT Commands
```
WAIT 5.                    // Wait 5 seconds
WAIT UNTIL ALTITUDE > 10000.  // Wait until condition
WAIT UNTIL STAGE:READY.    // Wait for staging
```

---

## Maneuver Nodes

### Create a Maneuver Node
```
// Create node at apoapsis, 100 m/s prograde
SET node TO NODE(
    TIME:SECONDS + SHIP:ORBIT:TIMEAPOAPSIS,  // Time
    0,                                        // Prograde
    0,                                        // Normal
    100                                       // Radial
).
ADD node.
```

### Execute a Maneuver Node
```
SET node TO NEXTNODE.
SET startTime TO TIME:SECONDS + node:ETA - 60.

// Point at node
LOCK STEERING TO node:BURNVECTOR.
WAIT UNTIL TIME:SECONDS >= startTime.

// Execute burn
LOCK THROTTLE TO 1.
WAIT UNTIL node:DELTAV:MAG < 1.
LOCK THROTTLE TO 0.
REMOVE node.
```

### Common Maneuvers
```
// Circularize at apoapsis
SET circBurn TO NODE(
    TIME:SECONDS + SHIP:ORBIT:TIMEAPOAPSIS,
    0, 0,
    SQRT(SHIP:BODY:MU / (SHIP:ORBIT:APOAPSIS + SHIP:BODY:RADIUS)) - SHIP:VELOCITY:ORBIT:MAG
).
ADD circBurn.
```

---

## Encounters and Transfers

### Check for Encounter
```
SET encounter TO SHIP:ORBIT:ENCOUNTER.
IF encounter:NAME = "Mun" {
    PRINT "Mun encounter in " + encounter:ETA + " seconds".
}
```

### Transfer Window Calculator
```
FUNCTION transferAngle {
    PARAMETER target.
    SET a1 TO SHIP:ORBIT:SEMIMAJORAXIS.
    SET a2 TO target:ORBIT:SEMIMAJORAXIS.
    SET mu TO SHIP:BODY:MU.
    SET transferTime TO PI * SQRT((a1+a2)^3 / (8*mu)).
    SET angle TO 180 - (transferTime / target:ORBIT:PERIOD * 360).
    RETURN angle.
}
```
