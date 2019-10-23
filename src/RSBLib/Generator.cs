using System;
using System.Collections.Generic;

namespace SMC.Utilities.RSG
{
    /// <summary>
    /// Takes tokenized data created from the <see cref="Tokenizer"/> to create the random string.
    /// </summary>
    public class Generator : IDisposable
    {
        #region Variables
        private Tokenizer _tokenizer;
        private object _tokenizedPattern;
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
            Pattern = pattern;
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
            if (null == _tokenizedPattern)
                throw new InvalidPatternException("A valid pattern must be set before attempting to generate a random string.");

            return string.Empty;
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