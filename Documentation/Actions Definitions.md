# Actions Definitions

Here you can find the structure fields for all the existing options
and the information of what their keys and values correspond to


## Builder

```json lines
"example_builder": {
  "location": Visitor Location (?)
  "reference_location": Visitor Location (?*+location)
  "reference": Regex Expressions (?*+reference_location)
  "prepare": Regex Expressions (?*+build)
  "build": Regex Expressions (*)
}
```

#### Example
```json
"example_builder": {
  "location": method
  "reference_location: method.def
  "reference":
  '''
  void (?P<method_name>) \((.*?)\)
  '''
  "build": "The method name is ${method_name}!"
}
```


## Resolver

```json
"example_resolver": {
  "mode": Resolver Modes (*)
  "index": Resolver Local Expression (required if mode is set to list | last)
  "last": Resolver Local Expression (required if mode is set to list | index)
  "list": Resolver Local Expression Array (required if mode is set to list)
  "cases": Resolver Local Expression Dictionary (required if mode is set to switch)
}

```

#### Example

```json
"example_resolver": {
  "mode": list
  "index": #!@{index}
  "list": {
    "I'm value 1!"
  }
}

```

## Replacement
```json
"example_replacer": {
  "location": Visitor Location (*)
  "reference_location": Visitor Location (?*+location)
  "reference": Regex Expressions (?*+reference_location)
  "from": Regex Expressions (*)
  "to": Regex Expressions (*)
}
```

## Addition
```json
"example_addition": {
  "location": Visitor Location (*)
  "reference_location": Visitor location (?*+location)
  "reference": Regex Expressions (*)
}
```

## Subtraction
```json
"example_subtraction": {
  "location": Visitor Location (*)
  "reference_location": Visitor location (?*+location)
  "reference": Regex Expressions (*)
}
```


#### Usage


## References:
 - Visitor:
   - [Visitor Location](https://github.com/OriDevTeam/CodeShaper/blob/f0b8b2b8dcad7b87a414e3aecfc6d8267d8d45ca/CodeShaper/Lib/Shapers/CPP/CPPPatch.cs#L175)
 
 - Builder:
   - Builder Expression

 - Resolver:
   - [Resolver Modes](https://github.com/OriDevTeam/CodeShaper/blob/f0b8b2b8dcad7b87a414e3aecfc6d8267d8d45ca/CodeShaper/Lib/Shapers/CPP/CPPPatch.cs#L169)
   - Resolver Expression
   - Resolver Local Expression


## Legend
The symbols that appear before the key name of the property mean:
 - `*` = Required 
 - `*+` = Required with another field
 - `?` = Optional
 - `|` = Either the previous condition or the next condition is required
