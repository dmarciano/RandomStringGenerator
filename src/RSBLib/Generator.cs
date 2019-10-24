﻿using System;
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
        private List<Token> _tokenizedPattern;
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
        /// Creates a new <see cref="Tokenizer"/> class.
        /// </summary>
        public Generator() : this(string.Empty) { }

        /// <summary>
        /// Creates a new <see cref="Tokenizer"/> class.
        /// </summary>
        /// <param name="pattern">The pattern string to use for the random string generation.</param>
        /// <exception cref="NoPatternException">Pattern is null, empty, or only whitespace.</exception>
        /// <exception cref="InvalidPatternException">Pattern is not valid.</exception>
        public Generator(string pattern)
        {
            _tokenizer = new Tokenizer();
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
        public void SetPattern(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                throw new NoPatternException();

            var (valid, parseError) = _tokenizer.Tokenize(pattern);
            if (!valid)
                throw new InvalidPatternException(parseError);

            _tokenizedPattern = _tokenizer.TokenizedPattern;

            //TODO: Allow users to specify random number generator.
            _rng = new RandomGenerator();
            Pattern = pattern;
        }

        /// <summary>
        /// Creates a random string based on the specified pattern.
        /// </summary>
        /// <returns>The randomly generated string.</returns>
        /// <exception cref="InvalidPatternException">No valid pattern set.</exception>
        public override string ToString()
        {
            return GetString();
        }

        /// <summary>
        /// Creates a random string based on the specified pattern.
        /// </summary>
        /// <returns>The randomly generated string.</returns>
        /// <exception cref="InvalidPatternException">No valid pattern set.</exception>
        public string GetString()
        {
            return CreateRandomString();
        }

        /// <summary>
        /// Creates the specified number of random string based on the specified pattern.
        /// </summary>
        /// <param name="count">The number of random strings to create</param>
        /// <returns>a list of <paramref name="count"/> random strings.</returns>
        /// <exception cref="InvalidPatternException">No valid pattern set.</exception>
        public IEnumerable<string> GetStrings(int count)
        {
            for (var i = 0; i < count; i++)
                yield return CreateRandomString();
        }

        private string CreateRandomString()
        {
            if (null == _tokenizedPattern && _tokenizedPattern.Count> 0)
                throw new InvalidPatternException("A valid pattern must be set before attempting to generate a random string.");

            var sb = new StringBuilder();
            var repeat = 1;
            char[] characters = new char[] { };
            foreach (var token in _tokenizedPattern)
            {
                if (token.MinimumCount == token.MaximumCount)
                {
                    repeat = token.MinimumCount;
                }
                else
                {
                    repeat = _rng.Next(token.MinimumCount, token.MaximumCount + 1);
                }

                switch (token.Type)
                {
                    case TokenType.LETTER:
                        if (token.Modifier.HasFlag(ModifierType.UPPERCASE))
                        {
                            characters = UPPERCASE;
                        }
                        else if (token.Modifier.HasFlag(ModifierType.LOWERCASE))
                        {
                            characters = LOWERCASE;
                        }
                        else
                        {
                            characters = ALL_LETTERS;
                        }
                        break;
                    case TokenType.NUMBER:
                        characters = NUMBERS;
                        break;
                    case TokenType.NUMBER_EXCEPT_ZERO:
                        characters = NUMBERS_EXCEPT_0;
                        break;
                    case TokenType.SYMBOL:
                        characters = SYMBOLS;
                        break;
                    case TokenType.LETTER_NUMBER:
                        characters = GetLettersNumbers(token);
                        break;
                    case TokenType.LETTER_SYMBOL:
                         characters = GetLettersSymbols(token);
                        break;
                    case TokenType.NUMBER_SYMBOL:
                        characters = GetNumbersSymbols(token);
                        break;
                    case TokenType.LETTER_NUMBER_SYMBOL:
                        characters = GetLettersNumbersSymbols(token);
                        break;
                    case TokenType.LITERAL:
                        for (var count = 0; count < repeat; count++)
                        {
                            sb.Append(token.Value);
                        }
                        break;
                }

                if(token.Type != TokenType.LITERAL)
                {
                    for (var count = 0; count < repeat; count++)
                    {
                        sb.Append(characters[_rng.Next(characters.Length)]);
                    }
                }
            }

            return sb.ToString();
        }

        private char[] GetLettersNumbers(Token token)
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

        private char[] GetLettersSymbols(Token token)
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

        private char[] GetNumbersSymbols(Token token)
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

        private char[] GetLettersNumbersSymbols(Token token)
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
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Generator() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}