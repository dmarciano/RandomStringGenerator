# Random String Generator (RSG)
A library from generating random strings based on the RSG language or via the provided fluent interface

## Features
- Quick string generation (see [Benchmarks](#benchmarks))
- Ability to specify a random number generator to use (via the ```IRandom``` interface specified in the library)
  - Library comes with two built-in classes: ```RandomGenerator``` and ```CryptoRandomGenerator```
- Able to specify anything from single character strings to very complex patterns including letters, numbers, and symbols/punctuation
- Able to specify list to choose an item from
- Specify repeat blocks to more customization and a more compact pattern
- Built-in DateTime and GUID functions
- Ability to specify user-defined functions for generating a portion of the string (e.g. to pull information from a database and have it directly be incorporated in the output string)
- Global and local exclusion blocks to allow exclusing a character, number, or symbol from anywhere in the output string, or just preventing a single token from generating a specific character, number, or symbol.
- Fluent interface provided via the **PatternBuilder** class - no need to remember the specific tokens.

## Uses
This library is used for generating random string based on a pattern or specific requirements, such as:
- User ID's
- Passwords
- Order/Product Numbers
- File Names
- Generating large data sets for testing/demonstration

The RSG language is used to specify the pattern.  RSG can be thought of as a complement to Regular Expression (RegEx) - whereas RegEx is used to *test* strings against a pattern, RSG is used to *create* strings from a pattern.
Therefore an RSG pattern can be as simple as a single character token or as complex as incorporating optional blocks and user-defined functions.  It depends on the specific use can and the data that needs to be generated.
Although it contains some advanced features (such as the user-defined functions), most users will not need to use them; but they are should the need ever arise.

## RSG Language
Generating a string using the RSG library is done by specifying a pattern for the string.  This pattern is made up of various types of tokens.  The types of tokens include:
- [Basic Tokens](#basic-tokens)
- [Modifiers](#modifiers)
- [Literals](#literals)
- [Optionals](#optionals)
- [Ranges](#ranges)
- [Format Strings](#format-strings)
- [Repeats](#repeats)
- [Token Group](#token-groups)
- [Languages/Cultures](#languagescultures)
- [Control Blocks](#control-blocks)

To jump to a specific token type, click the corresponding link above.  If you are new to using RSG, it is recommend that you read the document in order as the various tokes are listed from the most common 
(i.e. basic) to least commonly used ones (i.e. advanced).

### Basic Tokens
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
There are three diffent literals supported by the RSG language:
- **\\n** - Outputs a new line.  This literal can be used directly within a pattern, in an options box, or within the literal brackets (see below).
- **\\t** - Same as the above literal but outputs a tab instead of a new line
- **[ ]** - Outputs anything within the brackets exactly as it is written without any processing
> **NOTE** To output a closing bracket ] it must be escaped with a backslash like so: [\\]]

The following are some samples of how literals can be used:
- **[-]** - Outputs a single hypen
- **[_]** - Outputs a single underscore
- **[\\]** - Outputs a single backspace
- **[/]** - Outputs a single forward slash
- **[Line1\\nLine2]** - Outputs 'Line1', then a new line, and then 'Line2'.
- **[ ]** - Outputs a single blank space.
- **[a]** - Outputs the letter 'a'.

**Note** Modifiers are *not* valid on any literals.

### Optionals
The RSG language has so called **Optional** blocks.  These allow you to specify a list of characters and/or words that will be used as a random selection list.  
These blocks are surrounded by the pound, or hash, symbol **#**.  For example, if a random list of addresses was being used as test data, and the type of street (Street, Avenue, Blvd, etc.) should be randomly selected
to allow a variety of words, this can be specified by using:

\#Street, Avenue, Blvd\#

The generater would then randomly selected one of the items in the list. 
> **NOTE** To use a hash (\#) or comma (,) in the list, it must be escaped using a backslash (\\).  For example, if the list was to contain suite numbers that had a hash symbole, they would need to be escape like this:
\#Suite \\\#1, Suite \\\#2, Suite \\\#3\#

### Ranges
Ranges in the RSG language works in the same way that ranges work in Regular Expressions and are enclosed in angled brackets **<>**.  You can specify ranges of characters **\<a-b>**, numbers **\<3-5>**,
symbols **\<!-$>**, and multiple ranges **\<a-z3-5>** or **\<a-z345>**.  When the output string is generated, it will pick a valid character from the specified range(s).  
If a range should be repeated (e.g. a number between three and six, two times), the repeat count can be specified in the same manner as for a single token **\<3-6>(2)**.

### Format Strings
Text enclosed within angled brackets **><** represents a format string.  This can be any standard .NET format string but **MUST** include **{0}** somehwere in the string (e.g. **>Number as hex {0}<**) and be after a token.
  - **0(2)>#{0:X}<** - Would output a hexadecimal number (with a leading pound/hash sign) that has a decimal value between 0 and 99

### Repeats
Repeats are specified using parentheses **()**.  If just a single number is inside, the precending token/block will be repeated exactly that many times.  To specify a minmum to maximum number of repeats, the min/max values
should be separated with a comma:
- **a(2)** - Would output any two letters.
- **a(1,2)** - Would output either one or two letters
- **a(1,3)** - Would output either one, two, or three letters
- **a(0,2)** or **(,2)** - Would output either zero, one, or two letters.
> #### NOTE Although **a(0)** seems like it would be valid based on the information above, this pattern is not valid as this would not generate any output and would simply add to the processing time.

> The order of modifiers or counts does not matter.  That means that ```a^(2)>{0,5}<```, ```a(2)^>{0,5}<```, and ```a>{0,5}<(2)^``` are all the same pattern and would generate the same output.

### Token Groups
A token group allows a section of a pattern to be repeated one or more times or to apply an [exclusion control block](#exclusion-control-block-ecb) to just a section of the code.  This feature allows more complex
patterns to be created without having to duplicate sections of code.  A token group is delimited by a forward slash **/** as in the following pattern:

**/a![2]/(1,2)**

The above pattern represents a single lowercase letter, followed by the literal **2**, and both of those items together repeated one to two times.  Some examples of valid output would be:
- b2
- c2d2
- b2b2
- c2

#### Modifier/Exclusion Conflicts
Because modifiers can be applied to individual tokens, as well as groups, in some patterns these may conflict.  For example:

**/aa!a/^**

The above pattern states:
- Begin group
 - Any single character
 - Any single, lowercase, character
 - Any single character
- End group
- Apply uppercase character modifier to this group

This conflicts with the second character token's modifier, which states that the second token should only be lowercase letters.  In situation such as this, the individual token modifier takes precedent over the
group modifier as it is more specific.  So the above pattern would generate a string that would be an uppercase letter, followed by a lowercase letter, followed up an uppercase letter (e.g. ```AbC``` or ```XyZ```).

For exclusions blocks, a group exclusion block with apply to all the tokens, however additional exclusions can be applied to individual tokens.  For example:

**/aa{-N}a/{-ABCDEFGHIJKLM}**

The above pattern will exclude the characters A-M from appearing in any of the token positions, however, the middle character token *also* excludes the character N.

### Languages/Cultures
A new feature (introduced in v0.2.0) is the ability to provide a ```Generator``` instance with a list of characters for various languages/cultures and then use a language token to specify what language a specific token, 
group, or entire pattern should use.  The allows the same pattern to be used in various cultures with the only requirement being that the culture tag is changed.

A language/culture can be specified by surround the language/culture name with **&** tokens.  For example, **aa&sv&a** will generate a single ```en-US``` letter, then a single letter from the Swedish alphabet, 
followed by another ```en-US``` letter.  This can be further refined by using the region identifier like so: **aa&sv-FI&a**.  This second pattern is the same as the first, however it states that for the second character
to use the Finland version of the Swedish alphabet.  If the Finland specific version of the alphabet has not be specified, it will fall back to just the general Swedish alphabet.  If this is not specified either, 
then ```en-US``` will be used as a fallback *or* an exception will be thrown, depedning on the value of ```Generator.ThrowExceptionOnUnknownLanguage```.

### Control Blocks
A control block, **{}** is a special token which is used for more granular control of tokens (called an "exclusion control block" or "ECB") or for specifying either built-in or user-defined functions 
(called a "function control block" or "FCB".  Each is described below:

#### Exclusion Control Block (ECB)
A **{}** block can be specified at the very beginning of a pattern string to specify any letters, numbers, or symbols to leave out of any token selection.  For example, if the pattern string is *a9a*, but the letters
'l' (lowercase L) or 'O' (uppercase letter "Oh") should not be generated for either **a** token in the pattern, a control block can be specified at the beginning of the pattern string as **{-lO}a9a**.  Such a global control 
block can only be specified once at the beginning of the pattern.  That is writing **{-l}{-O}a9a** is invalid.
> **NOTE** To exclude the closing brace }, backslash \, or hypen - , it must be escaped within the ECB with a backlash like so: **{\\}}**, **{\\\\}**, and **{\\-}**

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
- **{T:d}[ ]{T:t}** - This would generate a standard .NET DateTime short data string followed by a long time string (e.g. 6/15/2009 1:45:30 PM)

It is also possible to call a custom Func\<TResult> method where *TResult* is a *string* type.  This also inserting data to the randomly generated string that is taken from other places.  For example, the Func\<TResult>
may return data from a database or different data depending on the state of the application.  In order to use this kind of FCB, several things must be done:
- Create a unique name that will be used to associate the function.  It is recommended that this is a short string and can only be letters.
- Include the FCB in the pattern using this name
- Call the **AddFunction** method of the generator instance and pass in this unique name as well as the delegate for the function.

For example, if the function being used was getting data from a database, it can be called **DB**.  This name is included in the FCB in the same way that the date/time and GUID functions were used, **{DB}**.  Finally,
it is necessary to call the **AddFunction** method of the generator instance with this name and the function **AddFunction("DB", GetDataFromDatabase)**.

The FCBs are evaluated using a lazy methodology.  That is, the **GetDataFromDatabase** method in the example above is not called during tokenization and is only called when the random string is actually generated.
Because of this, using a function, like the date/time function, will use a data/time when the string is actually generated instead of when the pattern was originally loaded and parsed.  It is possible though to force
an immediate evaluation of the FCB, *which use built-in functions* by including the **?** switch before the closing brace.

For example, if every generated string should have the *same* date/time at the beginning of the string, the pattern can be written as
**{T:d?}**.  This will ensure that every string has the same date. 
> **NOTE** The force switch applies *per block*.  So if a pattern contains **{T:d?}a{T:d?}**, the date/time will be gotten twice; once for the first block and once for the second block.  Therefore, all the generated
string will have the same date/time in the first and second blocks, but the first and second blocks will be different from each other.
> **NOTE** The force switch *cannot* be applied to user-defined functions.  This is because the user-defined function is specified in the generator and may not even be defined in the generator when the pattern is processed

## Using the PatternBuilder class
The pattern builder class provides methods to help create a pattern without having to remember all the possible tokens, modifiers, control blocks, etc.  This makes it much
easier to build very complex patterns.

Using the **PatternBuilder** class is very easy.  First, a new instance of the class must be created:

```c#
var builder = new PatternBuilder();
```

The the various methods of the ```builder``` can be used to create the pattern:

```c#
builder = builder().Letter().Repeat(2).WithModifer(UpperCase).Letter().WithModifier(LowerCase).Repeat(2,3).Number.Exclude(1,2,3,4,5).NumberWithZero();
```

The above is the equivalent of ```a^(2)a!(2,3)0{1,2,3,4,5}9```.  The fluent interface make is much easier to understand what is being intended with the pattern and allows
changes to be easily made.  The string equivalent of the fluently generated pattern can be gotten by calling the ```ToString()``` method on the ```PatternBuilder``` instance.

It is also possible to combine multiple ```PatternBuilder``` instances using then ```+``` operater.  For example, using the fluent methods, a builder for a phone number can
be generated like this:

```c#
var builder = new PatternBuilder()
	.Literal("(").NumberExceptZero().Number().Repeat(2).Literal(") ")
	.NumberExceptZero().Number().Repeat(2)
	.Literal("-").Number().Repeat(4);
var generator = new Generator().UseBuilder(builder);
var phoneNumber = generator.GetString();
```

or the different parts of the number can be separated out into their own ```PatternBuilder``` instances and combined with the ```+``` operator like so:

```c#
var areaBuilder = new PatternBuilder().Literal("(").NumberExceptZero().Number().Repeat(2).Literal(") ");
var exchangeBuilder = new PatternBuilder().NumberExceptZero().Number().Repeat(2);
var numberBuilder = new PatternBuilder().Number().Repeat(4);

var generator = new Generator().UseBuilder(areaBuilder + exchangeBuilder + new PatternBuilder().Literal("-") + numberBuilder);
var phoneNumber = generator.GetString();
```

Both of the above examples will generate a phone number string using the same pattern.  However, depending on the complexity of the pattern being used, the second method
may be better.  It also allows reusing, and recombining, multiple ```PatternBuilders```.

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

## Benchmarks
This project uses the [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) library for all benchmark testing.  Below is some benchmarks that were executed to test the speed of the RSG library:

### ***COMING SOON***

## TODO/Future Plans
- Culture tags for multiple languages in a single pattern

## Contributing
Contributing to this project is welcome.  However, we ask that you please follow our [contributing guidelines](./CONTRIBUTING.md) to help ensure consistency.

## Versions
**0.2.0**
- Added [Repeat Blocks](#repeat-blocks)
- Added Mersenne Twister random number generator
- Can specify uppercase, lowercase, number, and symbol array for different language/cultures
> *Please note that this is a **PREVIEW** version and there is no guarantee that any method signature or functionality will remaing the same*

**0.1.0** - Initial Release

This project uses [SemVer](http://semver.org) for versioning.

## Authors
- Dominick Marciano Jr.

## Special Thanks
- Markus Olsson for his cryptographically secure random number generator for generating numbers within a specific range [GitHub Gist](https://gist.github.com/niik/1017834).
- Trevor Barnett for his [Mersenne Twister](https://github.com/ullet/MersenneTwister) random number generator implementation.

## License
Copyright (c) 2019 Dominick Marciano Jr., Sci-Med Coding.  All rights reserved

See [LICENSE](./LICENSE) for full licesning terms.