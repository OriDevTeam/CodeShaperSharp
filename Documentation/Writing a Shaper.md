# Writing a Shaper
___

Currently and probably the only supported format for a shaper
is [Human Json (HJSON)](https://hjson.github.io/)

The expected schema for a shaper file cam be seen below, but first comes an explanation:

```
{
    "project" (string): VCX Project Alias (the alias provided for a project name)
    "file" (string/array): Name or Names of the files to shape
    "actions" (dict): A collection of actions that can be taken to shape text
    {
        "builders" (dict): A collection of builders that build text based on captures
        "resolvers (dict): A collection of resolvers that provide constant pieces of text
        "replacements" (dict): A collection of replacers that replace text captured
        "additions" (dict): A collection of additions that add text regarding captured text
        "subtractions" (dict): A collection of subtracters that remove captured text
    }
}
```

And the expected schema format

```
{
    "project": _project_alias_
    "file": _file_name_
    "actions": {
        "replacements": {...}
        "additions": {...}
        "subtractions: {...}
        "resolvers: {...}
    }
}
```

### Builder
___

The builder schema is has following:

```
{
    *"location" (enum string): Location where to start building from
    ?"reference_location (enum string): A reference location that should match simulatenously 
    ?"reference" (string): Regex Expression to match for the reference location
    *"build (string)": Builder Expression to builder to build a string
}
```

**Notes:**

*: Required

?: Optional

**References:**

[Locations Enum](https://github.com/OriDevTeam/CodeShaper/blob/9d6d056e88d06157897208277c0dba8991c36478/Lib/Shapers/CPP/CPPPatch.cs#L111)


