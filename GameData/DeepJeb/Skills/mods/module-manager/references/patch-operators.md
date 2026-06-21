# Module Manager Patch Operators Reference

## Value Patch Syntax
```
<Op><Name-With-Wildcards>(,<index>)?
```

**Wildcards:**
- `?` — Any single character (including none)
- `*` — Any number of any characters

**Index:** 0-based, negative indexes supported (e.g., `-1` = last)

## Node Patch Syntax
```
<Op><NodeType>([<NodeNameWithWildcards>])?(,<index-or-*>)?
```

Optional qualifiers:
- `:HAS[<has block>]` — Conditional match on sub-nodes/values
- Index or `*` for all matches

## Insert (No Operator)

Creates new nodes/values unconditionally.

```
@PART[myPart]
{
    // Insert new nodes
    node_stack_top = 0.0, 1.0, 0.0, 0.0, 1.0, 0.0, 0
    MODULE,0 { name = ModuleSomeFeature }
}
```

## Edit (`@`)

Modifies existing values. Supports arithmetic and regex.

```
@PART[somePart]
{
    @name = newName              // Replace value
    @numeric += 5                // Add
    @numeric *= 2                // Multiply
    @numeric != 2                // Square
    @text ^= :old:new:           // Regex replace
}
```

**Useful regex patterns (using `:` as separator):**
- `:$: Some Extra Stuff:` — Append text
- `:^: Preamble :` — Prepend text
- `:^.*$: Preamble $0 Extra:` — Wrap entire value

## Copy (`+` or `$`)

Duplicates a node and allows modifications on the copy.

```
+PART[originalPart]
{
    @name = clonedPart           // Rename the copy
    @MODULE[ModuleEngines] { @maxThrust *= 2 }
}
```

## Delete (`-` or `!`)

Removes nodes or values.

```
@PART[targetPart]
{
    -MODULE[moduleToRemove] {}   // Delete matching node
    -MODULE,2 {}                 // Delete by index
    -MODULE,-1 {}                // Delete last match
    -someValue = dummy           // Delete matching value
}
```

**Note:** Nodes require `{}` after the delete target; values require a dummy value.

## Edit-or-Create (`%`)

If the value exists, edit it. If not, create it. No wildcards or indexes.

```
@PART[somePart]
{
    %rescaleFactor = 1.25        // Set if missing, update if present
}
```

## Create (`&`)

Only creates if the target does not already exist. Safe for setting defaults.

```
@PART[somePart]
{
    &someDefault = value         // Won't overwrite existing values
}
```

## Rename Node (`|`)

Renames an entire node.

```
@EXAMPLE
{
    |_ = RENAMED                 // Renames EXAMPLE to RENAMED
}
```

## Copy Node (`#`)

Copies a node from a different location in the config tree.

```
EXAMPLE { NESTED { TARGET { foo = bar } } }
@EXAMPLE
{
    #/NESTED/TARGET {}           // Copies TARGET node into current context
}
```
