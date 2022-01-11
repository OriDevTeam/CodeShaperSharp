
# Expressions
### Be aware that this doc is in development and might not be accurate
___

There are several expressions that are used when using actions, such as:
 - PCRE Regex Expressions
 - Builder Expressions
 - Resolver Expressions

## Expression Groups

Due to lack of better quality implementation currently, expressions are grouped into
these categories

 - Actions Expressions
   - Builder Expressions 
   
     - Builder Build Expressions
       - Regex Replace Expressions
       - Resolver Expressions
   
   - Resolver Expressions
   

 - Regex Expressions
   - PCRE Regex Expressions

When one group is in use, the other cannot be used, for example when its required
to use Actions Expressions you cannot use Regex Expressions, and vice-versa

### Regex Expressions
___

In all the places where expressions are used, regex expressions supports
is always present, the regex support is Perl Compatible Regex Expressions (PCRE)

You can test regex expressions at [Regex101 with the Flavor PCRE (PHP<7.3)](https://regex101.com/)


### Builder Expressions
___

This expression is used in actions to get a specific builder built string
and its format it's the following:

`#{builder_name}`

For example, calling the following builder:

```
"builder_name": {
    "location": method
    "reference_location": method.body
    "reference": Oh no, you found (.*?)!
    "build": I found $1!
}
```

When trying to find the location specified:
```
void hello_there()
{
    printf("Oh no, you found cookies!")
}
```

Will return the built string:
```
I found cookies!
```

### Resolver Expressions

This expression is used in actions to call a resolver to return a constant string
and its format is the following:

```
#!{resolver_name}
#!{resolver_name}(arg1)
$!{resolver_name}(arg1, arg2, arg_with_expression1, ...)
```

and you can access local variables inside the resolving with:

```
#!@{local_variable}
```

**Note**: Arguments should be a simple string, any type of expression are not supported

**Automated local variables**

When called the resolver will automatically provide these variables locally:
 - args: representation of all the given arguments separated by `, `(comma and space)
 - args_count: count amount of given arguments


#### List Mode

List mode is to make a switch case like operation which takes any type of expression
as its keys and values, and it's defined following:

```
"resolver_name": {
    "mode": list
    "index": #!@{args_count}
    "list": {
        Casey
        Anne
    }
}
```

For example, when resolving the expression using the resolver above:
```
Albert should be prepared to solve the problem with John
```


Will return the resolved string:
```
Albert and John solved the problem!
```

#### Switch Mode

Switch mode is to make a switch case like operation which takes any type of expression
as its keys and values, and it's defined following:

```
"resolver_name": {
    "mode": switch,
    "switch": {
        "person_one": Casey
        "person_two": Anne
    }
}
```


For example, when resolving the expression using the resolver above:
```
Albert should be prepared to solve the problem with John
```


Will return the resolved string:
```
Albert and John solved the problem!
```


___


<!--
For example, calling the following resolver:

```
"resolver_name": {
    "mode": simple
    "prepare": (?<first_name>.*?) should be prepared to solve the problem with (?<second_name>.*?)
    "solve": ${first_name} and ${second_name} solved the problem!
}   
```

When resolving the expression:
```
Albert should be prepared to solve the problem with John
```


Will return the resolved string:
```
Albert and John solved the problem!
```
-->