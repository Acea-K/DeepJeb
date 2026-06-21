# kOS Example Scripts

## Launch Script

Standard launch-to-orbit script:

```
CLEARSCREEN.
PRINT "Launching...".
LOCK THROTTLE TO 1.
LOCK STEERING TO HEADING(90, 90).
STAGE.
WAIT UNTIL APOAPSIS > 100000.
LOCK THROTTLE TO 0.
PRINT "Coasting to apoapsis...".
WAIT UNTIL ETA:APOAPSIS < 10.
PRINT "Circularizing...".
```

## Gravity Turn

Adjusts pitch from 90° (vertical) to 45° by 10km altitude:

```
LOCK STEERING TO HEADING(90, 90 - (ALTITUDE / 10000) * 45).
// Adjusts pitch from 90° (vertical) to 45° by 10km
```

## Landing Script

Basic retrograde suicide burn:

```
LOCK STEERING TO RETROGRADE.
LOCK THROTTLE TO 1.
WAIT UNTIL VERTICALSPEED > -10.
LOCK THROTTLE TO 0.
STAGE.
```
