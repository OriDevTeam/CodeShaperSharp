# How It Works


When shaping starts, the following is occurs:
 - Parsing of Shape Project data (settings, shapers...)
 - Parsing of Visual Studio Solution and its Projects (sln and vcxproj)
 - Parsing of Project's modules (cpp and header) with Antlr
 - Shaping of Module () with a CPP Visitor


## Parsing Visual Studio Solution

Starts by loading the solution file (.sln) and parses its projects (.vcxproj)
location and names using regex, for example:

``` json
Project("{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}") = "ExampleCLI", "vs_files\CWebBrowser\CWebBrowser.vcxproj", "{2E953487-E73A-4C43-A9B6-174AB7B9A7E2}"
EndProject
Project("{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}") = "ExampleLib", "vs_files\EffectLib\EffectLib.vcxproj", "{7F1EC9EC-35DA-4332-A339-B68E3C95976F}"
EndProject
...
```

will result in `ExampleCLI` and `ExampleLib` data and then proceeds to parse its project files


## Parsing Visual Studio Project

Starts by loading each given project file (vcxproj) and parses its modules (.cpp)
and headers (.h) using regex, for example: 

``` json
  <ItemGroup>
    <ClInclude Include="..\source\ExampleCLI\CLI.h"/>
    <ClInclude Include="..\source\ExampleCLI\Menu.h"/>
  </ItemGroup>
  <ItemGroup>
    <ClCompile Include="..\source\ExampleCLI\CLI.cpp"/>
    <ClCompile Include="..\source\ExampleCLI\Menu.cpp"/>
  </ItemGroup>
  ...
```

will result in the files array and then proceeds to parse each module:
```
..\source\ExampleCLI\CLI.cpp
..\source\ExampleCLI\Menu.cpp
..\source\ExampleCLI\CLI.h
..\source\ExampleCLI\Menu.h
```


## Visiting a CPP Module

Starts by first creating an Antrl Input stream, a Visitor and
then uses a CPP Visitor, loads the module and starts visiting
the module, such as its methods, declarations, expressions, etc...

*You can check all the locations it visits in the [Visitor Class](https://github.com/OriDevTeam/CodeShaper/blob/main/CodeShaper/Lib/AST/ANTLR/CPPModuleVisitor.cs)*


## Shaping the module

When any visit occurs, for example a method visit:

```cpp
void Hello ()
{
    // This is a function yes!
}
```

its text its stored in the Module Dictionary giving its location, for example:

```cpp
Module.Dictionary[Location.Method] = context
```

then, processing of Shaping Actions occurs, such as Builders, Resolvers, Replacements, Additions and Subtractions (we will call them BRRAS)


### Building the builders

Think of a Builder like a Tree, it has its Root and Branches (children actions), and the leafs
are its BRASS (in this case only keep Builders in mind)

When processing of builders starts, all Root level builders are called
and its active builder its processed, the active builder is always the Root builder,

A builder can have its own Actions(BRRAS) and these will be triggered when
a builder is set as active (except Builders which will only build when the last builder on its branch is reach)


##### Marking a builder as active
To set a builder as active, some conditions have to occur:
 - The location of the builder has to be equal to the current visitor location
 - (optional) The location reference had to have been previously visited
 - (optional) When location reference is set, the reference expression has to match the reference location context text



##### Building the result

When the last builder (leaf) of a Branch is visited, all the builders in the branch
will build its results, using **Actions Expressions** (check [Expressions Documentation](https://github.com/OriDevTeam/CodeShaper/blob/main/Documentation/Expressions.md))
