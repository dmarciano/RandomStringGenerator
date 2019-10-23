# Random String Generator (RSG)
A library from generating random strings based on a simple pattern language.

## Uses
This library is used for generating random string based on a pattern or specific requirements, such as:
- User ID's
- Passwords
- Order/Product Numbers

The RSG language used to the specify the pattern is relatively simply.  Although Regular Expressions can express *very* complicated patterns (e.g. using look-aheads), it makes them much more complicated.  In addition,
regular expressions were designed more for matching strings to a pattern rather than generating a string from a pattern.  The latter is exactly what this library is designed to do and is in many ways a complement to
regular expressions.

## RSG Language
Generating a string using the RSG library is done by specifying a pattern for the string.  This pattern is made up of [Tokens](#tokens), [Modifiers](#modifiers), [Literals](#literals), and [Others](#others).

### Tokens
Tokens are used to specify the type of character that should be generated.  Currently, there are eight (8) different tokens that can be specified in a pattern and each is listed below with what they represent:
- **a** - Any letter
- **0** - Any number 0 - 9
- **9** - Any number 1 - 9
- **@** - Any of the following symbols ```!"#$%&'()*+,-./:;<=>?@[\]^_`{|}~```
  > These were chosen as they are the special characters that are present on a standard US keyboard and commonly used in passwords as specified by [OWASP](https://www.owasp.org/index.php/Password_special_characters)
- **.** - Any letter or number
- **+** - Any letter or symbol
- **%** - Any number or symbol
- **\*** - Any letter, number, or symbol

### Modifiers
Modifiers are special characters which provide more granularity for specific tokens.  Currently, there are three (3) modifers.  Each is listed below along with which tokens they can be used with:
- **^** - Uppercase letters only
- **!** - Lowercase letters only
- **~** - Exlude the number zero (0)

The first two modifiers can only appear once for each valid token and they *cannot both be specified on the same token*.  The tokens the first two modifiers are valid for are:
- **a**
- **.**
- **+**
- **\***

The last modifier can only appear once per valid token, listed below:
- **.**
- **%**
- **\***

**Note** The **.** and **\*** tokens are the only tokens which support multiple modifiers.  That is, these two tokens can contain the last modifier **~** AND the **^** *OR* **!** modifier.
You can see the [Examples](#examples) section for samples of valid/invalid patterns and examples of what their output would look like for further clarity.

### Literals
Literals are characters that can appear anywhere in the string, as many times as is necessary, and are printed exactly how they appear:
- **-** - Outputs a single hypen
- **_** - Outputs a single underscore
- **\\** - Outputs a single backspace
- **/** - Outputs a single forward slash
- **" "** - Outputs a single blank space.  The double quotes should **not** be used and are only used here to show the blank space.  **Cannot** be the first or last item in a pattern (i.e. to leading or trailing whitespace).
- **[ ]** - Outputs the text within the brackets without any processing.  For example [45] would output "45"

Modifiers are **not** valid on any literals.

### Others
There are a couple of additional characters that are used to help define the pattern:
- **()** - Parentheses are used next to tokens (but after modifiers), or after literals, to specify a number of repeats.  For example:
  - **a(2)** - Would output any two letters.  Some possible examples are:
    - aa
	- tV
	- Px
	- ZY
  - **a(1,3)** - Would output one to three letters.  Some possible examples are;
    - abc
	- tRy
	- RvP
	- TUV
	- tUP
  - **a(0,1)** or **a(,2)** - Would output zero (0) to 2 characters.  The possible outputs would be the same as the first example, however it is also possible nothing would be outputted (i.e. the character would just be skipped).
  - **a!(2)** - Would output any two LOWERCASE letters.
  - **-(3)** - Would output three hypens sequentially.
  - **\[34](3)** - Would output "34" three times "343434"

#### NOTE Although **a(0)** seems like it would be valid based on the information above, this pattern is not valid as this would not generate any output and would simply add to the processing time.

## Examples

### Valid Patterns
Below are valid patterns with a description and an example of a possible output:



### Invalid Patterns
Below are invalid patterns with a description of *why* it is invalid


## Contributing
Contributing to this project is welcome.  However, we ask that you please follow our [contributing guidelines](./CONTRIBUTING.md) to help ensure consistency.

## Versions
**0.1.0** - Initial Release
  - *Please note that this is a **PREVIEW** version and there is no guarantee that any method signatures or functionality will remaing the same*

This project uses [SemVer](http://semver.org) for versioning.

## Authors
- Dominick Marciano Jr.

## License
Copyright (c) 2019 Dominick Marciano Jr., Sci-Med Coding.  All rights reserved

See [LICENSE](./LICENSE) for full licesning terms.