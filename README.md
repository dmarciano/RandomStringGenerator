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

The first two modifiers can only appear once for each valid token and they *cannot both be specified on the same token*.  The first two modifiers are valid for the following tokens:
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
Literal are written by surrounding them with brackets (**[]**).  Some examples include:
- **[-]** - Outputs a single hypen
- **[_]** - Outputs a single underscore
- **[\\]** - Outputs a single backspace
- **[/]** - Outputs a single forward slash
- **[ ]** - Outputs a single blank space.  The double quotes should **not** be used and are only used here to show the blank space.  **Cannot** be the first or last item in a pattern (i.e. no leading or trailing whitespace).
- **[a]** - Outputs the letter 'a'.

**Note** Modifiers are *not* valid on any literals.

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
  - **\[-](3)** - Would output three hypens sequentially.
  - **\[a4](3)** - Would output "a4" three times (i.e. "a4a4a4").

#### NOTE Although **a(0)** seems like it would be valid based on the information above, this pattern is not valid as this would not generate any output and would simply add to the processing time.

## Examples

### Valid Patterns
Below are valid patterns with a description and an example of a possible output:
- **a** - Generate a single letter (either uppercase or lowercase)
- **a(2)** - Generate two letters (either uppercase or lowercase)
- **.(1,2)** - Generate one to two letters or numbers.  For example: *a1*, *2B*, *uV*, *45*, etc.
- **a^(2,3)** - Generate two to three *uppercase* letters.
- **%~(2)** - Generate two numbers or symbols *excluding* zero.
- **[Order-]0(3,5)[-P]a(2)[-]9(4)** - Generate a string which starts with "Order-", followed by three to five numbers, followed by "-P", followed by two letters, then a hypen, and then four numbers not including 0.  Some examples include:
  - Order-012-PAb-1234
  - Order-3210-PAA-2589
  - Order-4567-Pbb-9999
- **0(3)[-]0(2)[-]0(4)** - Generate a social security number
- **[(]90(2)[) ]90(2)[-]0(4)** - Generate a phone number in the format of (xxx) xxx-xxxx where the first number of the area code and exchange code can only be 1 - 9.

In the **RSGLib.Tests** project, there is a category of unit tests called *Real-World Patterns* that those how to generate various real-world pattern (e.g. social security number, various types of credit card numbers, etc.)
along with a regular expression to confirm that the generated string is valid.
   
### Invalid Patterns
Below are invalid patterns with a description of *why* it is invalid:
- **a(0)** - Repeat count cannot be exactly zero
- **a~** - Cannot apply exclude zero modifier to letter token
- **0^** - Cannot apply uppercase modifier to number token
- **a(2,** or **a(2,3%** - No closing parenthesis
- **a(2,3,4)** - Invalid format/too many commas


## Future Plans
- Currently there is plan for the next release to include a token, or modifier, to indicate that certain letters and/or numbers should be *not* be generated.
This would make it possible for users to specify, for example, that they don't want the letter 'l' (lowercase L) or '1' (one) output for specific tokens.
- The ```Generator``` class currently uses a built-in random number generator for creating the output string.  The RNG is built upon the .NET Framework ```RNGCryptoServiceProvider``` class.  There are plans
to allow users to specify their own class to generate random numbers as long as the custom class implements the ```IRandom``` interface (specified in the RSG library).

## Contributing
Contributing to this project is welcome.  However, we ask that you please follow our [contributing guidelines](./CONTRIBUTING.md) to help ensure consistency.

## Versions
**0.1.0** - Initial Release
  - *Please note that this is a **PREVIEW** version and there is no guarantee that any method signatures or functionality will remaing the same*

This project uses [SemVer](http://semver.org) for versioning.

## Authors
- Dominick Marciano Jr.

## Special Thanks
- Markus Olsson for his cryptographically secure random number generator for generating numbers within a specific range [GitHub Gist](https://gist.github.com/niik/1017834).

## License
Copyright (c) 2019 Dominick Marciano Jr., Sci-Med Coding.  All rights reserved

See [LICENSE](./LICENSE) for full licesning terms.