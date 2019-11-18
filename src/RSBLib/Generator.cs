using SMC.Utilities.RSG.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMC.Utilities.RSG
{
    /// <summary>
    /// Takes tokenized data created from the <see cref="Tokenizer"/> to create the random string.
    /// </summary>
    public class Generator : IDisposable
    {
        #region Statics
        private static readonly char[] UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private static readonly char[] LOWERCASE = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private static readonly char[] ALL_LETTERS = UPPERCASE.Concat(LOWERCASE).ToArray();
        private static readonly char[] NUMBERS = "0123456789".ToCharArray();
        private static readonly char[] NUMBERS_EXCEPT_0 = "123456789".ToCharArray();
        private static readonly char[] SYMBOLS = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~".ToCharArray();

        private static readonly char[] LETTERS_NUMBERS = ALL_LETTERS.Concat(NUMBERS).ToArray();
        private static readonly char[] LETTERS_NUMBERS_EXCEPT_0 = ALL_LETTERS.Concat(NUMBERS_EXCEPT_0).ToArray();
        private static readonly char[] UPPER_LETTERS_NUMBERS = UPPERCASE.Concat(NUMBERS).ToArray();
        private static readonly char[] LOWER_LETTERS_NUMBERS = LOWERCASE.Concat(NUMBERS).ToArray();
        private static readonly char[] UPPER_LETTERS_NUMBERS_EXCEPT_0 = UPPERCASE.Concat(NUMBERS_EXCEPT_0).ToArray();
        private static readonly char[] LOWER_LETTERS_NUMBERS_EXCEPT_0 = LOWERCASE.Concat(NUMBERS_EXCEPT_0).ToArray();

        private static readonly char[] LETTERS_SYMBOLS = ALL_LETTERS.Concat(SYMBOLS).ToArray();
        private static readonly char[] UPPER_LETTERS_SYMBOLS = UPPERCASE.Concat(SYMBOLS).ToArray();
        private static readonly char[] LOWER_LETTERS_SYMBOLS = LOWERCASE.Concat(SYMBOLS).ToArray();

        private static readonly char[] NUMBERS_SYMBOLS = NUMBERS.Concat(SYMBOLS).ToArray();
        private static readonly char[] NUMBERS_EXCEPT_0_SYMBOLS = NUMBERS_EXCEPT_0.Concat(SYMBOLS).ToArray();

        private static readonly char[] ALL_LETTERS_NUMBERS_SYMBOLS = ALL_LETTERS.Concat(NUMBERS).Concat(SYMBOLS).ToArray();
        private static readonly char[] ALL_LETTERS_NUMBERS_EXCEPT_0_SYMBOLS = ALL_LETTERS.Concat(NUMBERS_EXCEPT_0).Concat(SYMBOLS).ToArray();
        private static readonly char[] UPPER_NUMBERS_SYMBOLS = UPPERCASE.Concat(NUMBERS).Concat(SYMBOLS).ToArray();
        private static readonly char[] LOWER_NUMBERS_SYMBOLS = LOWERCASE.Concat(NUMBERS).Concat(SYMBOLS).ToArray();
        private static readonly char[] UPPER_NUMBERS_EXCEPT_0_SYMBOLS = UPPERCASE.Concat(NUMBERS_EXCEPT_0).Concat(SYMBOLS).ToArray();
        private static readonly char[] LOWER_NUMBERS_EXCEPT_0_SYMBOLS = LOWERCASE.Concat(NUMBERS_EXCEPT_0).Concat(SYMBOLS).ToArray();

        #endregion

        #region Variables
        private Tokenizer _tokenizer;
        //private List<Token> _tokenizedPattern;
        private List<TokenizedGroup> _tokenizedPattern;
        private Dictionary<string, Func<string>> _functions = new Dictionary<string, Func<string>>();
        private IRandom _rng;
        #endregion

        #region Properties
        /// <summary>
        /// The current set pattern used for random string generation.
        /// </summary>
        public string Pattern { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new <see cref="Generator"/> class.
        /// </summary>
        /// <exception cref="NoPatternException">Pattern is null, empty, or only whitespace.</exception>
        /// <exception cref="InvalidPatternException">Pattern is not valid.</exception>
        public Generator() : this(string.Empty, new RandomGenerator()) { }

        /// <summary>
        /// Creates a new <see cref="Generator"/> class.
        /// </summary>
        /// <param name="random">The random number generator used for the string generation.</param>
        /// <exception cref="NoPatternException">Pattern is null, empty, or only whitespace.</exception>
        /// <exception cref="InvalidPatternException">Pattern is not valid.</exception>
        public Generator(IRandom random) : this(string.Empty, random) { }

        /// <summary>
        /// Creates a new <see cref="Generator"/> class.
        /// </summary>
        /// <param name="pattern">The pattern string to use for the random string generation.</param>
        /// <exception cref="NoPatternException">Pattern is null, empty, or only whitespace.</exception>
        /// <exception cref="InvalidPatternException">Pattern is not valid.</exception>
        public Generator(string pattern) : this(pattern, new RandomGenerator()) { }

        /// <summary>
        /// Creates a new <see cref="Generator"/> class.
        /// </summary>
        /// <param name="pattern">The pattern string to use for the random string generation.</param>
        /// <param name="random">The random number generator used for the string generation.</param>
        /// <exception cref="NoPatternException">Pattern is null, empty, or only whitespace.</exception>
        /// <exception cref="InvalidPatternException">Pattern is not valid.</exception>
        public Generator(string pattern, IRandom random)
        {
            _tokenizer = new Tokenizer();
            _rng = random;
            if (!string.IsNullOrEmpty(pattern))
                SetPattern(pattern);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the pattern to use for the random string generation.
        /// </summary>
        /// <param name="pattern">The pattern to set.</param>
        /// <exception cref="NoPatternException">Pattern is null, empty, or only whitespace.</exception>
        /// <exception cref="InvalidPatternException">Pattern is not valid.</exception>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        public Generator SetPattern(string pattern)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(Generator));
            if (string.IsNullOrWhiteSpace(pattern))
                throw new NoPatternException();

            var valid = _tokenizer.Tokenize(pattern);
            if (valid)
                _tokenizedPattern = _tokenizer.TokenizedPattern;

            Pattern = pattern;

            return this;
        }

        /// <summary>
        /// Uses a <see cref="PatternBuilder"/> for the random string generation.
        /// </summary>
        /// <param name="builder">The <see cref="PatternBuilder"/> to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is null.</exception>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        public Generator UseBuilder(PatternBuilder builder)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(Generator));
            if (null == builder)
                throw new ArgumentNullException(nameof(builder), "Pattern builder cannot be null.");

            //TODO: Implement token groups for the PatternBuilder
            //_tokenizedPattern = builder.TokenizedPattern;
            Pattern = builder.ToString();

            return this;
        }

        /// <summary>
        /// Add a <see cref="Func{TResult}"/> with a specific name.
        /// </summary>
        /// <param name="name">The name of the function as it appears in a function control block in the pattern.</param>
        /// <param name="function">The function that will be executed to get the data for the pattern.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void AddFunction(string name, Func<string> function)
        {
            _functions.Add(name, function);
        }

        /// <summary>
        /// Creates a random string based on the specified pattern.
        /// </summary>
        /// <returns>The randomly generated string.</returns>
        /// <exception cref="InvalidPatternException">No valid pattern set.</exception>
        /// <exception cref="InvalidModifierException">An unknown modifier was found in the pattern.</exception>
        /// <exception cref="DuplicateModifierException">A modifier was found twice in a row.</exception>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="KeyNotFoundException">A user-defined function FCB was not associated with a function in the generator.</exception>
        public override string ToString()
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(Generator));
            return GetString();
        }

        /// <summary>
        /// Creates a random string based on the specified pattern.
        /// </summary>
        /// <returns>The randomly generated string.</returns>
        /// <exception cref="InvalidPatternException">No valid pattern set.</exception>
        /// <exception cref="InvalidModifierException">An unknown modifier was found in the pattern.</exception>
        /// <exception cref="DuplicateModifierException">A modifier was found twice in a row.</exception>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        /// <exception cref="KeyNotFoundException">A user-defined function FCB was not associated with a function in the generator.</exception>
        public string GetString()
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(Generator));
            return CreateRandomString();
        }

        /// <summary>
        /// Creates the specified number of random string based on the specified pattern.
        /// </summary>
        /// <param name="count">The number of random strings to create</param>
        /// <returns>a list of <paramref name="count"/> random strings.</returns>
        /// <exception cref="InvalidPatternException">No valid pattern set.</exception>
        /// <exception cref="InvalidModifierException">An unknown modifier was found in the pattern.</exception>
        /// <exception cref="DuplicateModifierException">A modifier was found twice in a row.</exception>
        /// <exception cref="KeyNotFoundException">A user-defined function FCB was not associated with a function in the generator.</exception>
        public IEnumerable<string> GetStrings(int count)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(Generator));
            for (var i = 0; i < count; i++)
                yield return CreateRandomString();
        }


        //TODO: Handle token/group modifier precedence
        private string CreateRandomString()
        {
            if (null == _tokenizedPattern || _tokenizedPattern.Count < 1)
                throw new InvalidPatternException("A valid pattern must be set before attempting to generate a random string.");

            var sb = new StringBuilder();
            var tokenRepeat = 1;
            var groupRepeat = 1;
            char[] characters = new char[] { };
            char[] globalExcept = new char[] { };
            //foreach (var token in _tokenizedPattern)

            foreach (var group in _tokenizedPattern)
            {
                if (group.MinimumCount == group.MaximumCount)
                    groupRepeat = group.MinimumCount;
                else
                    groupRepeat = _rng.Next(group.MinimumCount, group.MaximumCount);

                for (var gRepeat = 0; gRepeat < groupRepeat; gRepeat++)
                {
                    foreach (var token in group.Tokens)
                    {
                        if (token.MinimumCount == token.MaximumCount)
                            tokenRepeat = token.MinimumCount;
                        else
                            tokenRepeat = _rng.Next(token.MinimumCount, token.MaximumCount + 1);

                        switch (token.Type)
                        {
                            case TokenType.LETTER:
                                if (ModifierType.NONE == token.Modifier)
                                {
                                    // Token has no modifier
                                    // Check group modifier
                                    if (group.Modifier.HasFlag(ModifierType.UPPERCASE))
                                    {
                                        // Group modifier is uppercase
                                        characters = UPPERCASE;
                                    }
                                    else if (group.Modifier.HasFlag(ModifierType.LOWERCASE))
                                    {
                                        // Group modifier is lowercase
                                        characters = LOWERCASE;
                                    }
                                    else
                                    {
                                        // No token or group modifier
                                        characters = ALL_LETTERS;
                                    }
                                }
                                else if (token.Modifier.HasFlag(ModifierType.UPPERCASE))
                                {
                                    characters = UPPERCASE;
                                }
                                else
                                {
                                    characters = LOWERCASE;
                                }
                                break;
                            case TokenType.NUMBER:
                                if (ModifierType.NONE == token.Modifier)
                                {
                                    // Token has no  modifier
                                    // Check group modifier
                                    if (group.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                                    {
                                        characters = NUMBERS_EXCEPT_0;
                                    }
                                    else
                                    {
                                        characters = NUMBERS;
                                    }
                                }
                                else if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                                {
                                    characters = NUMBERS_EXCEPT_0;
                                }
                                else
                                {
                                    characters = NUMBERS;
                                }
                                break;
                            case TokenType.NUMBER_EXCEPT_ZERO:
                                characters = NUMBERS_EXCEPT_0;
                                break;
                            case TokenType.SYMBOL:
                                characters = SYMBOLS;
                                break;
                            case TokenType.LETTER_NUMBER:
                                characters = GetLettersNumbers(group, token);
                                break;
                            case TokenType.LETTER_SYMBOL:
                                characters = GetLettersSymbols(group, token);
                                break;
                            case TokenType.NUMBER_SYMBOL:
                                characters = GetNumbersSymbols(group, token);
                                break;
                            case TokenType.LETTER_NUMBER_SYMBOL:
                                characters = GetLettersNumbersSymbols(group, token);
                                break;
                            case TokenType.LITERAL:
                                break;
                            case TokenType.OPTIONAL:
                                HandleOptionalBlock(token, tokenRepeat, ref sb);
                                break;
                            case TokenType.RANGE:
                                HandleRangeBlock(token, tokenRepeat, ref sb);
                                break;
                            case TokenType.CONTROL_BLOCK:
                                if (token.ControlBlock.Global)
                                {
                                    globalExcept = token.ControlBlock.ExceptValues;
                                }
                                else
                                {
                                    HandleControlBlock(token, tokenRepeat, ref sb);
                                }
                                break;
                        }

                        if (token.Type != TokenType.LITERAL && token.Type != TokenType.CONTROL_BLOCK && token.Type != TokenType.OPTIONAL && token.Type != TokenType.RANGE)
                        {
                            characters = characters.Except(globalExcept).ToArray();

                            if(group.ControlBlock != null && !group.ControlBlock.Global && group.ControlBlock.Type == ControlBlockType.ECB && group.ControlBlock.ExceptValues.Count() > 0)
                            {
                                characters = characters.Except(group.ControlBlock.ExceptValues).ToArray();
                            }

                            if (token.ControlBlock != null && !token.ControlBlock.Global && token.ControlBlock.Type == ControlBlockType.ECB && token.ControlBlock.ExceptValues.Count()> 0)
                            {
                                characters = characters.Except(token.ControlBlock.ExceptValues).ToArray();
                            }

                            if (null == token.ControlBlock || token.ControlBlock.Type != ControlBlockType.FMT)
                            {
                                for (var count = 0; count < tokenRepeat; count++)
                                {
                                    sb.Append(characters[_rng.Next(characters.Length)]);
                                }
                            }
                            else
                            {
                                var temp = new StringBuilder(tokenRepeat);
                                for (var count = 0; count < tokenRepeat; count++)
                                {
                                    temp.Append(characters[_rng.Next(characters.Length)]);
                                }

                                var str = temp.ToString();
                                if (int.TryParse(str, out var number))
                                {
                                    sb.Append(string.Format(token.ControlBlock.Value, number));
                                }
                                else
                                {
                                    sb.Append(string.Format(token.ControlBlock.Value, str));
                                }
                            }
                        }
                        else if (token.Type == TokenType.LITERAL)
                        {
                            for (var count = 0; count < tokenRepeat; count++)
                            {
                                if (null != token.ControlBlock && token.ControlBlock.Type == ControlBlockType.FMT)
                                {
                                    var formatted = string.Empty;
                                    if (int.TryParse(token.Value, out var number))
                                    {
                                        formatted = string.Format(token.ControlBlock.Value, number);
                                    }
                                    else
                                    {
                                        formatted = string.Format(token.ControlBlock.Value, token.Value);
                                    }
                                    sb.Append(formatted);
                                }
                                else
                                {
                                    sb.Append(token.Value);
                                }
                            }
                        }
                    }
                }
            }

            return sb.ToString();
        }

        private char[] GetLettersNumbers(TokenizedGroup group, Token token)
        {
            if (ModifierType.NONE == token.Modifier)
            {
                if (group.Modifier.HasFlag(ModifierType.UPPERCASE))
                {
                    if (group.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                    {
                        return UPPER_LETTERS_NUMBERS_EXCEPT_0;
                    }
                    else
                    {
                        return UPPER_LETTERS_NUMBERS;
                    }
                }

                else if (group.Modifier.HasFlag(ModifierType.LOWERCASE))
                {
                    if (group.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                    {
                        return LOWER_LETTERS_NUMBERS_EXCEPT_0;
                    }
                    else
                    {
                        return LOWER_LETTERS_NUMBERS;
                    }
                }

                else
                {
                    if (group.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                    {
                        return LETTERS_NUMBERS_EXCEPT_0;
                    }
                    else
                    {
                        return LETTERS_NUMBERS;
                    }
                }
            }
            else
            {
                if (token.Modifier.HasFlag(ModifierType.UPPERCASE))
                {
                    if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                    {
                        return UPPER_LETTERS_NUMBERS_EXCEPT_0;
                    }
                    else
                    {
                        return UPPER_LETTERS_NUMBERS;
                    }
                }

                else if (token.Modifier.HasFlag(ModifierType.LOWERCASE))
                {
                    if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                    {
                        return LOWER_LETTERS_NUMBERS_EXCEPT_0;
                    }
                    else
                    {
                        return LOWER_LETTERS_NUMBERS;
                    }
                }

                else
                {
                    if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                    {
                        return LETTERS_NUMBERS_EXCEPT_0;
                    }
                    else
                    {
                        return LETTERS_NUMBERS;
                    }
                }
            }
        }

        private char[] GetLettersSymbols(TokenizedGroup group, Token token)
        {
            if (ModifierType.NONE == token.Modifier)
            {
                if (group.Modifier.HasFlag(ModifierType.UPPERCASE))
                {
                    return UPPER_LETTERS_SYMBOLS;
                }
                else if (group.Modifier.HasFlag(ModifierType.LOWERCASE))
                {
                    return LOWER_LETTERS_SYMBOLS;
                }
                else
                {
                    return LETTERS_SYMBOLS;
                }
            }
            else
            {
                if (token.Modifier.HasFlag(ModifierType.UPPERCASE))
                {
                    return UPPER_LETTERS_SYMBOLS;
                }
                else if (token.Modifier.HasFlag(ModifierType.LOWERCASE))
                {
                    return LOWER_LETTERS_SYMBOLS;
                }
                else
                {
                    return LETTERS_SYMBOLS;
                }
            }
        }

        private char[] GetNumbersSymbols(TokenizedGroup group, Token token)
        {
            if (ModifierType.NONE == token.Modifier)
            {
                if (group.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                {
                    return NUMBERS_EXCEPT_0_SYMBOLS;
                }
                else
                {
                    return NUMBERS_SYMBOLS;
                }
            }
            else
            {
                if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                {
                    return NUMBERS_EXCEPT_0_SYMBOLS;
                }
                else
                {
                    return NUMBERS_SYMBOLS;
                }
            }
        }

        private char[] GetLettersNumbersSymbols(TokenizedGroup group, Token token)
        {
            if (ModifierType.NONE == token.Modifier)
            {
                if (group.Modifier.HasFlag(ModifierType.UPPERCASE))
                {
                    if (group.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                    {
                        return UPPER_NUMBERS_EXCEPT_0_SYMBOLS;
                    }
                    else
                    {
                        return UPPER_NUMBERS_SYMBOLS;
                    }
                }
                else if (group.Modifier.HasFlag(ModifierType.LOWERCASE))
                {
                    if (group.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                    {
                        return LOWER_NUMBERS_EXCEPT_0_SYMBOLS;
                    }
                    else
                    {
                        return LOWER_NUMBERS_SYMBOLS;
                    }
                }
                else
                {
                    if (group.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                    {
                        return ALL_LETTERS_NUMBERS_EXCEPT_0_SYMBOLS;
                    }
                    else
                    {
                        return ALL_LETTERS_NUMBERS_SYMBOLS;
                    }
                }
            }
            else
            {
                if (token.Modifier.HasFlag(ModifierType.UPPERCASE))
                {
                    if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                    {
                        return UPPER_NUMBERS_EXCEPT_0_SYMBOLS;
                    }
                    else
                    {
                        return UPPER_NUMBERS_SYMBOLS;
                    }
                }
                else if (token.Modifier.HasFlag(ModifierType.LOWERCASE))
                {
                    if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                    {
                        return LOWER_NUMBERS_EXCEPT_0_SYMBOLS;
                    }
                    else
                    {
                        return LOWER_NUMBERS_SYMBOLS;
                    }
                }
                else
                {
                    if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                    {
                        return ALL_LETTERS_NUMBERS_EXCEPT_0_SYMBOLS;
                    }
                    else
                    {
                        return ALL_LETTERS_NUMBERS_SYMBOLS;
                    }
                }
            }
        }

        private void HandleControlBlock(Token token, int repeat, ref StringBuilder sb)
        {

            for (var count = 0; count < repeat; count++)
            {
                if (token.ControlBlock.FunctionName.Equals("DATETIME") || token.ControlBlock.FunctionName.Equals("GUID"))
                {
                    sb.Append(token.ControlBlock.Function());
                }
                else
                {
                    if (null != token.ControlBlock.Function)    //Run functions that were added via a PatternBuilder instance
                    {
                        sb.Append(token.ControlBlock.Function());
                    }
                    else
                    {
                        sb.Append(_functions[token.ControlBlock.FunctionName]());
                    }
                }
            }
        }

        private void HandleOptionalBlock(Token token, int repeat, ref StringBuilder sb)
        {
            for (var count = 0; count < repeat; count++)
            {
                sb.Append(token.Values[_rng.Next(token.Values.Count)]);
            }
        }

        private void HandleRangeBlock(Token token, int repeat, ref StringBuilder sb)
        {
            List<char> characters = new List<char>();

            foreach (var range in token.Ranges)
            {
                if (range.Start.Equals(range.End))
                {
                    characters.Add(range.Start);
                }
                else
                {
                    var r = Enumerable.Range(range.Start, range.End - range.Start + 1).Select(i => (char)i);
                    characters.AddRange(r);
                }
            }


            for (var count = 0; count < repeat; count++)
            {
                sb.Append(characters[_rng.Next(characters.Count)]);
            }
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _tokenizer = null;
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            //GC.SuppressFinalize(this);
        }
        #endregion
    }
}