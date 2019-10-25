# Random String Generator (RSG)
A library from generating random strings based on a simple pattern language.

## Features
- Quick string generation (see [Benchmarks](#benchmarks))
- Ability to specify a random number generator to use
  - Library comes with two built-in classes: ```RandomGenerator``` and ```CryptoRandomGenerator```
- Able to specify anything from single character strings to very complex patterns including letters, numbers, and symbols/punctuation
- Able to specify
- Control Blocks to have granular control over tokens or to use built-in/user-defined, functions (e.g. date/time, GUID, etc.) to generate part of the string

## Uses
This library is used for generating random string based on a pattern or specific requirements, such as:
- User ID's
- Passwords
- Order/Product Numbers
- File Names

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
-- **{}** - Braces are used to specify a control block and can have different means depending on where it is placed and its exact format.  See [Advanced Features](#advanced-features) for more information on this token.

#### NOTE Although **a(0)** seems like it would be valid based on the information above, this pattern is not valid as this would not generate any output and would simply add to the processing time.

## Advanced Features

### Control Block
A control block, **{}** is a special token which is used for more granular control of tokens (called an "exclusion control block" or "ECB") or for specifying either built-in or user-defined functions 
(called a "function control block" or "FCB".  Each is described below:

#### Exclusion Control Block (ECB)
A **{}** block can be specified at the very beginning of a pattern string to specify any letters, numbers, or symbols to leave out of any token selection.  For example, if the pattern string is *a9a*, but the letters
'l' (lowercase L) or 'O' (uppercase letter "Oh") should not be generated for either **a** token in the pattern, a control block can be specified at the beginning of the pattern string as **{-lO}a9a**.  Such a global control 
block can only be specified once at the beginning of the pattern.  That is writing **{-l}{-O}a9a** is invalid.

However, if 'l' should only be excluded from the first letter token and 'O' should be excluded from the second letter token, a  ECB can be specified for each token individually: **a{-l}9a{-O}**.
> **NOTE** ECBs must *always* have a hypen ('-') immediately after the opening brace ('{') to indicate that it is a ECB and not a Function Control Block (described below).

#### Function Control Block (FCB)
A control block can also be used to specify functions to either generate a string from a .NET Framework method (e.g. date/time, GUID, etc.) or by a specified user function.  Currently there are two built-in functions that
can be used:
- **T** - Date and time string
- **G** - GUID

Both of these accept a standard .NET formatting string by separating it with a colon (':') like this: **{T:<format_string>}**.  Some examples include:
- **{T:d}** - This would generate a standard .NET DateTime short date string (e.g. 6/15/2009)
- **{T:T}** - This would generate a standard .NET DateTime long time string (e.g. 1:45:30 PM)
- **{T: MMMM dd, yyyy}** - This would generate a custom DateTime string (e.g June 10, 2011).
- **{G:N}** - This would generate a GUID with only numbers (e.g. 00000000000000000000000000000000)
- **{G:X}** - This would generate a GUID formatted as hexadecimals (e.g. {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}})
- **{T:d}[ ]{T:t}** - This would gnerated a standard .NET DateTime short data string followed by a long time string (e.g. 6/15/2009 1:45:30 PM)

It is also possible to call a custom Func\<TResult> method where *TResult* is a *string* type.  This also inserting data to the randomly generated string that is taken from other places.  For example, the Func\<TResult>
may return data from a database or different data depending on the state of the application.  In order to use this kind of FCB, several things must be done:
- Create a unique name that will be used to associate the function.  It is recommended that this is a short string and can only be letters.
- Include the FCB in the pattern using this name
- Call the **AssignFunction** method of the generator instance and pass in this unique name as well as the delegate for the function.

For example, if the function being used was getting data from a database, it can be called **DB**.  This name is included in the FCB in the same way that the date/time and GUID functions were used, **{DB}**.  Finally,
it is necessary to call the **AssignFunction** method of the generator instance with this name and the function **AssignFunction("DB", GetDataFromDatabase)**.

The FCBs are evaluated using a lazy methodology.  That is, the **GetDataFromDatabase** method in the example above is not called during tokenization and is only called when the random string is actually generated.
Because of this, using a function, like the date/time function, will use a data/time when the string is actually generated instead of when the pattern was originally loaded and parsed.  It is possible though to force
an immediate evaluation of the FCB, *which use built-in functions* by including the **?** switch before the closing brace.

For example, if every generated string should have the *same* date/time at the beginning of the string, the pattern can be written as
**{T:d?}**.  This will ensure that every string has the same date. 
> **NOTE** The force switch applies *per block*.  So if a pattern contains **{T:d?}a{T:d?}**, the date/time will be gotten twice; once for the first block and once for the second block.  Therefore, all the generated
string will have the same date/time in the first and second blocks, but the first and second blocks will be different from each other.
> **NOTE** The force switch *cannot* be applied to user-defined functions.  This is because the user-defined function is specified in the generator and may not even be defined in the generator when the pattern is processed

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
- A method of specifying a range, or set, of numbers/character to use for generating the string.

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