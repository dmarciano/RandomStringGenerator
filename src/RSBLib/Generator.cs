using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
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
        public void SetPattern(string pattern)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(Generator));
            if (string.IsNullOrWhiteSpace(pattern))
                throw new NoPatternException();

            var valid = _tokenizer.Tokenize(pattern);
            if (valid)
                _tokenizedPattern = _tokenizer.TokenizedPattern;

            Pattern = pattern;
        }

        /// <summary>
        /// Saves a tokenized pattern to a file.
        /// </summary>
        /// <param name="filePath">The folder to save the file to.  If null or empty, the application data folder is used.</param>
        /// <param name="fileName">The name of the file.  If null or empty, a auto-generated name is used.</param>
        /// <param name="overwrite"><c>true</c> is the file should be overwritten if it exist, otherwise <c>false</c>.</param>
        /// <returns>The full file path of the saved *.tok file.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SerializationException "></exception>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        public string Save(string filePath, string fileName, bool overwrite = false)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(Generator));

            if (null == _tokenizedPattern || _tokenizedPattern.Count < 1)
                throw new InvalidPatternException("A valid pattern must be set before attempting to generate a random string.");

            if (string.IsNullOrWhiteSpace(filePath))
                filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (string.IsNullOrWhiteSpace(fileName))
                fileName = Guid.NewGuid().ToString();

            fileName += ".tok";
            var fullFilePath = Path.Combine(filePath, fileName);
            if (File.Exists(fullFilePath) && !overwrite)
                throw new IOException($"The file '{fileName}' already exist in the specified folder.");

            var formatter = new BinaryFormatter();
            byte[] data;
            using(var ms = new MemoryStream())
            {
                formatter.Serialize(ms, new TokenFile() { Tokens = _tokenizedPattern, Pattern = Pattern });
                data = ms.ToArray();
            }

            using(var fs = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }

            return fullFilePath;
        }

        /// <summary>
        /// Saves a tokenized pattern to a stream
        /// </summary>
        /// <param name="stream">The stream to save the tokenized pattern to.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="SerializationException "></exception>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        public void Save(Stream stream)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(Generator));

            if (null == _tokenizedPattern || _tokenizedPattern.Count < 1)
                throw new InvalidPatternException("A valid pattern must be set before attempting to generate a random string.");
            if (null == stream)
                throw new ArgumentNullException(nameof(stream), "A stream must be provided.");

            var formatter = new BinaryFormatter();
            byte[] data;
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, new TokenFile() { Tokens = _tokenizedPattern, Pattern = Pattern });
                data = ms.ToArray();
            }

            stream.Write(data, 0, data.Length);

        }

        /// <summary>
        /// Load a string generation pattern from file.
        /// </summary>
        /// <param name="file">The full path of the file to load.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        public void Load(string file)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(Generator));
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentNullException(nameof(file), "A file must be specified.");

            using(var stream = new FileStream(file,FileMode.Open, FileAccess.Read, FileShare.None))
            {
                Load(stream);
            }
        }

        /// <summary>
        /// Load a string generation pattern from file.
        /// </summary>
        /// <param name="stream">A stream for the file to load.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
        public void Load(Stream stream)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(Generator));
            if(null == stream)
                throw new ArgumentNullException(nameof(stream), "A stream must be provided.");

            var formatter = new BinaryFormatter();
            using (stream)
            {
                var data = (TokenFile)formatter.Deserialize(stream);
                _tokenizedPattern = data.Tokens;
                Pattern = data.Pattern;
            }
        }


        /// <summary>
        /// Creates a random string based on the specified pattern.
        /// </summary>
        /// <returns>The randomly generated string.</returns>
        /// <exception cref="InvalidPatternException">No valid pattern set.</exception>
        /// <exception cref="InvalidModifierException">An unknown modifier was found in the pattern.</exception>
        /// <exception cref="DuplicateModifierException">A modifier was found twice in a row.</exception>
        /// <exception cref="ObjectDisposedException">Object has been disposed.</exception>
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
        public IEnumerable<string> GetStrings(int count)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(Generator));
            for (var i = 0; i < count; i++)
                yield return CreateRandomString();
        }

        private string CreateRandomString()
        {
            if (null == _tokenizedPattern || _tokenizedPattern.Count < 1)
                throw new InvalidPatternException("A valid pattern must be set before attempting to generate a random string.");

            var sb = new StringBuilder();
            var repeat = 1;
            char[] characters = new char[] { };
            char[] globalExcept = new char[] { };
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
                        if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                            characters = NUMBERS_EXCEPT_0;
                        else
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
                    case TokenType.CONTROL_BLOCK:
                        if (token.ControlBlock.Global)
                        {
                            globalExcept = token.ControlBlock.ExceptValues;
                        }
                        break;
                }

                if (token.Type != TokenType.LITERAL && token.Type!=TokenType.CONTROL_BLOCK)
                {
                    characters = characters.Except(globalExcept).ToArray();
                    if(token.ControlBlock != null && !token.ControlBlock.Global && token.ControlBlock.Type == ControlBlockType.ECB && token.ControlBlock.ExceptValues.Length> 0)
                    {
                        characters = characters.Except(token.ControlBlock.ExceptValues).ToArray();
                    }
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

        private void HandleControlBlock(Token token)
        {

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