using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Utilities.RSG
{
    public class PatternBuilder
    {
        #region Variables
        private List<Token> _patternList;
        #endregion

        #region Properties
        internal List<Token> TokenizedPattern => _patternList;
        #endregion

        #region Constructors
        public PatternBuilder()
        {
            _patternList = new List<Token>();
        }
        #endregion

        #region Methods
        public void Clear()
        {
            _patternList.Clear();
        }
        #endregion

        #region Tokens
        public PatternBuilder Letter()
        {
            _patternList.Add(new Token() { Type = TokenType.LETTER });
            return this;
        }

        public PatternBuilder Number()
        {
            _patternList.Add(new Token() { Type = TokenType.NUMBER });
            return this;
        }

        public PatternBuilder NumberExceptZero()
        {
            _patternList.Add(new Token() { Type = TokenType.NUMBER_EXCEPT_ZERO });
            return this;
        }

        public PatternBuilder Symbol()
        {
            _patternList.Add(new Token() { Type = TokenType.SYMBOL });
            return this;
        }

        public PatternBuilder LetterOrNumber()
        {
            _patternList.Add(new Token() { Type = TokenType.LETTER_NUMBER });
            return this;
        }

        public PatternBuilder LetterOrSymbol()
        {
            _patternList.Add(new Token() { Type = TokenType.LETTER_SYMBOL });
            return this;
        }

        public PatternBuilder NumberOrSymbol()
        {
            _patternList.Add(new Token() { Type = TokenType.NUMBER_SYMBOL });
            return this;
        }

        public PatternBuilder LetterNumberOrSymbol()
        {
            _patternList.Add(new Token() { Type = TokenType.LETTER_NUMBER_SYMBOL });
            return this;
        }

        public PatternBuilder Literal(string value)
        {
            _patternList.Add(new Token() { Type = TokenType.LITERAL, Value = value });
            return this;
        }
        #endregion

        #region Repeats
        public PatternBuilder Repeat(int repeats)
        {
            return Repeat(repeats, repeats);
        }

        public PatternBuilder Repeat(int minRepeats, int maxRepeats)
        {
            if (0 == _patternList.Count)
                throw new PatternBuilderException("No tokens have been added to the builder yet.  Add a least one valid token before attempting to specify repeats.");

            var lastToken = _patternList[_patternList.Count - 1];

            if (lastToken.Type == TokenType.CONTROL_BLOCK)
                throw new PatternBuilderException("Cannot add a repeat count to a global exclusion block.");

            lastToken.MinimumCount = minRepeats;
            lastToken.MaximumCount = maxRepeats;
            return this;
        }
        #endregion

        #region Modifiers
        public PatternBuilder UppercaseOnly()
        {
            if (0 == _patternList.Count)
                throw new PatternBuilderException("No tokens have been added to the builder yet.  Add a least one valid token before attempting to specify repeats.");

            var lastToken = _patternList[_patternList.Count - 1];

            if (lastToken.Type == TokenType.CONTROL_BLOCK)
                throw new PatternBuilderException("Cannot add a modifier to a global exclusion block.");

            if (lastToken.Type == TokenType.NUMBER || lastToken.Type == TokenType.NUMBER_EXCEPT_ZERO || lastToken.Type == TokenType.NUMBER_SYMBOL)
                throw new PatternBuilderException("The uppercase token modifier can only be used on tokens that generate letters.");

            if (lastToken.Modifier.HasFlag(ModifierType.UPPERCASE))
                throw new DuplicateModifierException("The token already has an uppercase modifier specified.");

            lastToken.Modifier = lastToken.Modifier | ModifierType.UPPERCASE;
            return this;
        }

        public PatternBuilder LowercaseOnly()
        {
            if (0 == _patternList.Count)
                throw new PatternBuilderException("No tokens have been added to the builder yet.  Add a least one valid token before attempting to specify repeats.");

            var lastToken = _patternList[_patternList.Count - 1];

            if (lastToken.Type == TokenType.CONTROL_BLOCK)
                throw new PatternBuilderException("Cannot add a modifier to a global exclusion block.");

            if (lastToken.Type == TokenType.NUMBER || lastToken.Type == TokenType.NUMBER_EXCEPT_ZERO || lastToken.Type == TokenType.NUMBER_SYMBOL)
                throw new PatternBuilderException("The lowercase token modifier can only be used on tokens that generate letters.");

            if (lastToken.Modifier.HasFlag(ModifierType.LOWERCASE))
                throw new DuplicateModifierException("The token already has a lowercase modifier specified.");

            lastToken.Modifier = lastToken.Modifier | ModifierType.LOWERCASE;
            return this;
        }

        public PatternBuilder ExcludeZero()
        {
            if (0 == _patternList.Count)
                throw new PatternBuilderException("No tokens have been added to the builder yet.  Add a least one valid token before attempting to specify repeats.");

            var lastToken = _patternList[_patternList.Count - 1];

            if (lastToken.Type == TokenType.CONTROL_BLOCK)
                throw new PatternBuilderException("Cannot add a modifier to a global exclusion block.");

            if (lastToken.Type == TokenType.LETTER || lastToken.Type == TokenType.SYMBOL || lastToken.Type == TokenType.LETTER_SYMBOL)
                throw new InvalidModifierException($"The exclude zero token modifier can only be used on tokens that generate numbers.");

            if (lastToken.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                throw new DuplicateModifierException($"The token already has an exclude zero modifier specified.");

            lastToken.Modifier = lastToken.Modifier | ModifierType.EXCLUDE_ZERO;
            return this;
        }
        #endregion

        #region Advanced
        public PatternBuilder Optional(IEnumerable<string> values)
        {
            return this.Optional(values.ToList());
        }

        public PatternBuilder Optional(params string[] values)
        {
            return this.Optional(values.ToList());
        }

        public PatternBuilder Optional(List<string> values)
        {
            if (values.Count == 0)
                throw new ArgumentNullException(nameof(values), "At least one value must be provided for an optional token.");

            _patternList.Add(new Token() { Type = TokenType.OPTIONAL, Values = values });
            return this;
        }

        public PatternBuilder Range(char start, char end)
        {
            if (end > start)
                throw new ArgumentOutOfRangeException("The end value must be greater than, or equal to, the start value for a range token.");

            _patternList.Add(new Token() { Type = TokenType.RANGE, Ranges = new List<Range>() { new Range() { Start = start, End = end } } });
            return this;
        }

        public PatternBuilder Format(string format)
        {
            if (!format.Contains("{0"))
                throw new PatternBuilderException("No argument placeholder '{0}' found in the previous token.");

            if (0 == _patternList.Count)
                throw new PatternBuilderException("No tokens have been added to the builder yet.  Add a least one valid token before attempting to specify repeats.");

            var lastToken = _patternList[_patternList.Count - 1];

            if (lastToken.Type == TokenType.CONTROL_BLOCK)
                throw new PatternBuilderException("Cannot add a repeat count to a global exclusion block.");


            lastToken.ControlBlock = new ControlBlock()
            {
                Type = ControlBlockType.FMT,
                Value = format
            };

            return this;
        }
        #endregion

        #region Functions
        public PatternBuilder AddDateTime()
        {
            return this.AddDateTime(string.Empty, false);
        }

        public PatternBuilder AddDateTime(string format)
        {
            return this.AddDateTime(format, false);
        }

        public PatternBuilder AddDateTime(bool force)
        {
            return this.AddDateTime(string.Empty, force);
        }

        public PatternBuilder AddDateTime(string format, bool force)
        {
            var token = new Token();
            ControlBlock cb = null;

            if (!string.IsNullOrWhiteSpace(format))
            {
                if (force)
                {
                    token.Type = TokenType.LITERAL;
                    token.Value = DateTime.Now.ToString(format);
                }
                else
                {
                    cb = new ControlBlock()
                    {
                        FunctionName = "DATETIME",
                        Function = () => DateTime.Now.ToString(format)
                    };
                }
            }
            else
            {
                if (force)
                {
                    token.Type = TokenType.LITERAL;
                    token.Value = DateTime.Now.ToString();
                }
                else
                {
                    cb = new ControlBlock()
                    {
                        FunctionName = "DATETIME",
                        Function = () => DateTime.Now.ToString()
                    };
                }
            }

            token.ControlBlock = cb;
            _patternList.Add(token);
            return this;
        }

        public PatternBuilder AddGuid()
        {
            return this.AddGuid(string.Empty, false);
        }

        public PatternBuilder AddGuid(string format)
        {
            return this.AddGuid(format, false);
        }

        public PatternBuilder AddGuid(bool force)
        {
            return this.AddGuid(string.Empty, force);
        }

        public PatternBuilder AddGuid(string format, bool force)
        {
            var token = new Token();
            ControlBlock cb = null;

            if (!string.IsNullOrWhiteSpace(format))
            {
                if (!string.Equals("N", format, StringComparison.CurrentCulture) && !string.Equals("D", format, StringComparison.CurrentCulture)
                        && !string.Equals("B", format, StringComparison.CurrentCulture) && !string.Equals("P", format, StringComparison.CurrentCulture)
                        && !string.Equals("X", format, StringComparison.CurrentCulture))
                {
                    throw new PatternBuilderException($"An unrecognized GUID format string was found.  Format string found: '{format}'.");
                }

                if (force)
                {
                    token.Type = TokenType.LITERAL;
                    token.Value = Guid.NewGuid().ToString(format);
                }
                else
                {
                    cb = new ControlBlock()
                    {
                        FunctionName = "GUID",
                        Function = () => Guid.NewGuid().ToString(format)
                    };
                }
            }
            else
            {
                if (force)
                {
                    token.Type = TokenType.LITERAL;
                    token.Value = Guid.NewGuid().ToString();
                }
                else
                {
                    cb = new ControlBlock()
                    {
                        FunctionName = "GUID",
                        Function = () => Guid.NewGuid().ToString()
                    };
                }
            }

            token.ControlBlock = cb;
            _patternList.Add(token);
            return this;
        }

        public PatternBuilder AddUDF(string name)
        {
            return this.AddUDF(name, null);
        }

        public PatternBuilder AddUDF(string name, Func<string> function)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new PatternBuilderException("User-defined function name cannot be null, empty, or whitespace.");

            _patternList.Add(new Token()
            {
                Type = TokenType.CONTROL_BLOCK,
                ControlBlock = new ControlBlock()
                {
                    Type = ControlBlockType.FCB,
                    FunctionName = name,
                    Function = function
                }
            });

            return this;
        }
        #endregion

        #region Exclusions
        public PatternBuilder AddGlobalExclusions(IEnumerable<char> values)
        {
            return this.AddGlobalExclusions(values.ToList());
        }

        public PatternBuilder AddGlobalExclusions(params char[] values)
        {
            return this.AddGlobalExclusions(values.ToList());
        }

        public PatternBuilder AddGlobalExclusions(List<char> values)
        {
            var firstToken = _patternList[0];

            if (firstToken.Type == TokenType.CONTROL_BLOCK)
                throw new PatternBuilderException("A global exclusion block already exists in the pattern.");

            _patternList.Add(new Token()
            {
                Type = TokenType.CONTROL_BLOCK,
                ControlBlock = new ControlBlock()
                {
                    Type = ControlBlockType.ECB,
                    Global = true,
                    ExceptValues = values.ToArray()
                }
            });

            return this;
        }

        public PatternBuilder Exclude(IEnumerable<char> values)
        {
            return this.Exclude(values.ToList());
        }

        public PatternBuilder Exclude(params char[] values)
        {
            return this.Exclude(values.ToList());
        }

        public PatternBuilder Exclude(List<char> values)
        {
            var lastToken = _patternList[_patternList.Count - 1];

            if (lastToken.ControlBlock != null)
                throw new PatternBuilderException("Only one exclusion control block can be specified for a token.");


            lastToken.ControlBlock = new ControlBlock()
            {
                Type = ControlBlockType.ECB,
                Global = false,
                ExceptValues = values.ToArray()
            };

            return this;
        }
        #endregion

        public override string ToString()
        {
            //TODO: Implement ToString()
            return base.ToString();
        }
    }
}
