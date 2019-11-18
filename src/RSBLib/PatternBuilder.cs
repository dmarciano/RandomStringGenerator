using System;
using System.Collections.Generic;
using System.Linq;

namespace SMC.Utilities.RSG
{
    public class PatternBuilder
    {
        #region Variables
        private List<TokenGroup> _patternList;
        private bool inInitialGroup = true;
        private bool inCreatedGroup = false;
        #endregion

        #region Properties
        internal List<TokenGroup> TokenizedPattern => _patternList;
        #endregion

        #region Constructors
        public PatternBuilder()
        {
            _patternList = new List<TokenGroup>
            {
                new TokenGroup()
            };
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
            _patternList.Last().Tokens.Add(new Token() { Type = TokenType.LETTER });
            return this;
        }

        public PatternBuilder Number()
        {
            _patternList.Last().Tokens.Add(new Token() { Type = TokenType.NUMBER });
            return this;
        }

        public PatternBuilder NumberExceptZero()
        {
            _patternList.Last().Tokens.Add(new Token() { Type = TokenType.NUMBER_EXCEPT_ZERO });
            return this;
        }

        public PatternBuilder Symbol()
        {
            _patternList.Last().Tokens.Add(new Token() { Type = TokenType.SYMBOL });
            return this;
        }

        public PatternBuilder LetterOrNumber()
        {
            _patternList.Last().Tokens.Add(new Token() { Type = TokenType.LETTER_NUMBER });
            return this;
        }

        public PatternBuilder LetterOrSymbol()
        {
            _patternList.Last().Tokens.Add(new Token() { Type = TokenType.LETTER_SYMBOL });
            return this;
        }

        public PatternBuilder NumberOrSymbol()
        {
            _patternList.Last().Tokens.Add(new Token() { Type = TokenType.NUMBER_SYMBOL });
            return this;
        }

        public PatternBuilder LetterNumberOrSymbol()
        {
            _patternList.Last().Tokens.Add(new Token() { Type = TokenType.LETTER_NUMBER_SYMBOL });
            return this;
        }

        public PatternBuilder Literal(string value)
        {
            _patternList.Last().Tokens.Add(new Token() { Type = TokenType.LITERAL, Value = value });
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
            if (inInitialGroup || inCreatedGroup)
            {
                if (0 == _patternList.Count)
                    throw new PatternBuilderException("No tokens have been added to the builder yet.  Add a least one valid token before attempting to specify repeats.");

                var lastToken = _patternList[_patternList.Count - 1].Tokens.Last();

                if (lastToken.Type == TokenType.CONTROL_BLOCK && null != lastToken.ControlBlock && lastToken.ControlBlock.Global)
                    throw new PatternBuilderException("Cannot add a repeat count to a global exclusion block.");

                lastToken.MinimumCount = minRepeats;
                lastToken.MaximumCount = maxRepeats;
            }
            else
            {

            }
            return this;
        }
        #endregion

        #region Modifiers
        public PatternBuilder UppercaseOnly()
        {
            if (0 == _patternList.Count)
                throw new PatternBuilderException("No tokens have been added to the builder yet.  Add a least one valid token before attempting to specify repeats.");

            var lastToken = _patternList[_patternList.Count - 1].Tokens.Last();

            if (lastToken.Type == TokenType.CONTROL_BLOCK)
                throw new PatternBuilderException("Cannot add a modifier to a global exclusion block.");

            if (lastToken.Type == TokenType.NUMBER || lastToken.Type == TokenType.NUMBER_EXCEPT_ZERO || lastToken.Type == TokenType.NUMBER_SYMBOL)
                throw new PatternBuilderException("The uppercase token modifier can only be used on tokens that generate letters.");

            if (lastToken.Modifier.HasFlag(ModifierType.UPPERCASE))
                throw new PatternBuilderException("The token already has an uppercase modifier specified.");

            if (lastToken.Modifier.HasFlag(ModifierType.LOWERCASE))
                throw new PatternBuilderException("The token already has a lowercase modifier specified.");

            lastToken.Modifier = lastToken.Modifier | ModifierType.UPPERCASE;
            return this;
        }

        public PatternBuilder LowercaseOnly()
        {
            if (0 == _patternList.Count)
                throw new PatternBuilderException("No tokens have been added to the builder yet.  Add a least one valid token before attempting to specify repeats.");

            var lastToken = _patternList[_patternList.Count - 1].Tokens.Last();

            if (lastToken.Type == TokenType.CONTROL_BLOCK)
                throw new PatternBuilderException("Cannot add a modifier to a global exclusion block.");

            if (lastToken.Type == TokenType.NUMBER || lastToken.Type == TokenType.NUMBER_EXCEPT_ZERO || lastToken.Type == TokenType.NUMBER_SYMBOL)
                throw new PatternBuilderException("The lowercase token modifier can only be used on tokens that generate letters.");

            if (lastToken.Modifier.HasFlag(ModifierType.LOWERCASE))
                throw new DuplicateModifierException("The token already has a lowercase modifier specified.");

            if (lastToken.Modifier.HasFlag(ModifierType.UPPERCASE))
                throw new PatternBuilderException("The token already has an uppercase modifier specified.");

            lastToken.Modifier = lastToken.Modifier | ModifierType.LOWERCASE;
            return this;
        }

        public PatternBuilder ExcludeZero()
        {
            if (0 == _patternList.Count)
                throw new PatternBuilderException("No tokens have been added to the builder yet.  Add a least one valid token before attempting to specify repeats.");

            var lastToken = _patternList[_patternList.Count - 1].Tokens.Last();

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

        #region Token Group
        public PatternBuilder BeginGroup()
        {
            return BeginGroup(1, 1, ModifierType.NONE, null);
        }
        public PatternBuilder BeginGroup(int repeats)
        {
            return BeginGroup(repeats, repeats, ModifierType.NONE, null);
        }

        public PatternBuilder BeginGroup(int minRepeats, int maxRepeats)
        {
            return BeginGroup(minRepeats, maxRepeats, ModifierType.NONE, null);
        }

        public PatternBuilder BeginGroup(ModifierType modifiers)
        {
            return BeginGroup(1, 1, modifiers, null);
        }

        public PatternBuilder BeginGroup(IEnumerable<char> exclude)
        {
            return BeginGroup(1, 1, ModifierType.NONE, exclude);
        }

        public PatternBuilder BeginGroup(int repeats, ModifierType modifiers)
        {
            return BeginGroup(repeats, repeats, modifiers, null);
        }

        public PatternBuilder BeginGroup(int minRepeats, int maxRepeats, ModifierType modifiers)
        {
            return BeginGroup(minRepeats, maxRepeats, modifiers, null);
        }

        public PatternBuilder BeginGroup(int repeats, IEnumerable<char> exclude)
        {
            return BeginGroup(repeats, repeats, ModifierType.NONE, exclude);
        }

        public PatternBuilder BeginGroup(int minRepeats, int maxRepeats, IEnumerable<char> exclude)
        {
            return BeginGroup(minRepeats, maxRepeats, ModifierType.NONE, exclude);
        }

        public PatternBuilder BeginGroup(ModifierType modifiers, IEnumerable<char> exclude)
        {
            return BeginGroup(1, 1, modifiers, exclude);
        }

        public PatternBuilder BeginGroup(int minRepeats, int maxRepeats, ModifierType modifiers, IEnumerable<char> exclude)
        {
            if (_patternList.Last().Tokens.Count > 0)
            {
                var tg = new TokenGroup() { MinimumCount = minRepeats, MaximumCount = maxRepeats, Modifier = modifiers };
                if (null != exclude && exclude.Count() > 0)
                    tg.ControlBlock = new ControlBlock() { Type = ControlBlockType.ECB, ExceptValues = exclude.ToArray() };
                _patternList.Add(tg);
            }
            else
            {
                var tg = _patternList.Last();
                tg.MinimumCount = minRepeats;
                tg.MaximumCount = maxRepeats;
                tg.Modifier = modifiers;
                if (null != exclude && exclude.Count() > 0)
                    tg.ControlBlock = new ControlBlock() { Type = ControlBlockType.ECB, ExceptValues = exclude.ToArray() };
            }

            return this;
        }

        public PatternBuilder EndGroup()
        {
            _patternList.Add(new TokenGroup() { });
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

            _patternList.Last().Tokens.Add(new Token() { Type = TokenType.OPTIONAL, Values = values });
            return this;
        }

        public PatternBuilder Range(char start, char end)
        {
            if (start > end)
                throw new ArgumentOutOfRangeException("The end value must be greater than, or equal to, the start value for a range token.");

            _patternList.Last().Tokens.Add(new Token() { Type = TokenType.RANGE, Ranges = new List<Range>() { new Range() { Start = start, End = end } } });
            return this;
        }

        public PatternBuilder Format(string format)
        {
            if (!format.Contains("{0"))
                throw new PatternBuilderException("No argument placeholder '{0}' found in the previous token.");

            if (0 == _patternList.Count)
                throw new PatternBuilderException("No tokens have been added to the builder yet.  Add a least one valid token before attempting to specify repeats.");

            var lastToken = _patternList[_patternList.Count - 1].Tokens.Last();

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
                    token.Type = TokenType.CONTROL_BLOCK;
                    cb = new ControlBlock()
                    {
                        Type = ControlBlockType.FCB,
                        FunctionName = "DATETIME",
                        Function = () => DateTime.Now.ToString(format),
                        Format = format
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
                    token.Type = TokenType.CONTROL_BLOCK;
                    cb = new ControlBlock()
                    {
                        Type = ControlBlockType.FCB,
                        FunctionName = "DATETIME",
                        Function = () => DateTime.Now.ToString()
                    };
                }
            }

            token.ControlBlock = cb;
            //_patternList.Add(token);
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
                    token.Type = TokenType.CONTROL_BLOCK;
                    cb = new ControlBlock()
                    {
                        Type = ControlBlockType.FCB,
                        FunctionName = "GUID",
                        Function = () => Guid.NewGuid().ToString(format),
                        Format = format,
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
                    token.Type = TokenType.CONTROL_BLOCK;
                    cb = new ControlBlock()
                    {
                        Type = ControlBlockType.FCB,
                        FunctionName = "GUID",
                        Function = () => Guid.NewGuid().ToString()
                    };
                }
            }

            token.ControlBlock = cb;
            //_patternList.Add(token);
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

            _patternList.Last().Tokens.Add(new Token()
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
            if (_patternList.Count > 0)
            {
                var firstToken = _patternList[0].Tokens.First();

                if (firstToken.Type == TokenType.CONTROL_BLOCK)
                    throw new PatternBuilderException("A global exclusion block already exists in the pattern.");

                _patternList[0].Tokens.Insert(0, new Token()
                {
                    Type = TokenType.CONTROL_BLOCK,
                    ControlBlock = new ControlBlock()
                    {
                        Type = ControlBlockType.ECB,
                        Global = true,
                        ExceptValues = values.ToArray()
                    }
                });
            }
            else
            {
                _patternList[0].Tokens.Add(new Token()
                {
                    Type = TokenType.CONTROL_BLOCK,
                    ControlBlock = new ControlBlock()
                    {
                        Type = ControlBlockType.ECB,
                        Global = true,
                        ExceptValues = values.ToArray()
                    }
                });
            }

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

        public static PatternBuilder operator +(PatternBuilder p1, PatternBuilder p2)
        {
            p1._patternList.AddRange(p2._patternList);
            return p1;
        }

        public override string ToString()
        {
            //var sb = new StringBuilder(_patternList.Count * 2);

            //foreach (var token in _patternList)
            //{
            //    switch (token.Type)
            //    {
            //        case TokenType.LETTER:
            //            sb.Append("a");
            //            if (token.Modifier.HasFlag(ModifierType.UPPERCASE))
            //            {
            //                sb.Append("^");
            //            }
            //            else if (token.Modifier.HasFlag(ModifierType.LOWERCASE))
            //            {
            //                sb.Append("!");
            //            }
            //            break;
            //        case TokenType.NUMBER:
            //            sb.Append("0");
            //            if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
            //            {
            //                sb.Append("~");
            //            }
            //            break;
            //        case TokenType.NUMBER_EXCEPT_ZERO:
            //            sb.Append("9");
            //            break;
            //        case TokenType.SYMBOL:
            //            sb.Append("@");
            //            break;
            //        case TokenType.LETTER_NUMBER:
            //            sb.Append(".");
            //            if (token.Modifier.HasFlag(ModifierType.UPPERCASE))
            //            {
            //                sb.Append("^");
            //            }
            //            else if (token.Modifier.HasFlag(ModifierType.LOWERCASE))
            //            {
            //                sb.Append("!");
            //            }

            //            if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
            //            {
            //                sb.Append("~");
            //            }
            //            break;
            //        case TokenType.LETTER_SYMBOL:
            //            sb.Append("+");
            //            if (token.Modifier.HasFlag(ModifierType.UPPERCASE))
            //            {
            //                sb.Append("^");
            //            }
            //            else if (token.Modifier.HasFlag(ModifierType.LOWERCASE))
            //            {
            //                sb.Append("!");
            //            }
            //            break;
            //        case TokenType.NUMBER_SYMBOL:
            //            sb.Append("%");
            //            if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
            //            {
            //                sb.Append("~");
            //            }
            //            break;
            //        case TokenType.LETTER_NUMBER_SYMBOL:
            //            sb.Append("*");

            //            if (token.Modifier.HasFlag(ModifierType.UPPERCASE))
            //            {
            //                sb.Append("^");
            //            }
            //            else if (token.Modifier.HasFlag(ModifierType.LOWERCASE))
            //            {
            //                sb.Append("!");
            //            }
            //            if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
            //            {
            //                sb.Append("~");
            //            }
            //            break;
            //        case TokenType.LITERAL:
            //            sb.Append($"[{token.Value}]");
            //            break;
            //        case TokenType.OPTIONAL:
            //            sb.Append($"#{string.Join(",", token.Values)}#");
            //            break;
            //        case TokenType.RANGE:
            //            var r = token.Ranges[0];
            //            if (r.Start.Equals(r.End))
            //                sb.Append($"<{r.Start}>");
            //            else
            //                sb.Append($"<{r.Start}-{r.End}>");
            //            break;
            //        case TokenType.CONTROL_BLOCK:
            //            if (null != token.ControlBlock)
            //            {
            //                if (token.ControlBlock.Global)
            //                {
            //                    var s = string.Join(string.Empty, token.ControlBlock.ExceptValues).Replace("\\", "\\\\").Replace("}", "\\}");
            //                    sb.Insert(0, s);
            //                }
            //                else if (token.ControlBlock.FunctionName.Equals("DATETIME"))
            //                {
            //                    if (string.IsNullOrWhiteSpace(token.ControlBlock.Format))
            //                    {
            //                        sb.Append($"{{T:{token.ControlBlock.Format}}}");
            //                    }
            //                    else
            //                    {
            //                        sb.Append($"{{T}}");
            //                    }
            //                }
            //                else if (token.ControlBlock.FunctionName.Equals("GUID"))
            //                {
            //                    if (string.IsNullOrWhiteSpace(token.ControlBlock.Format))
            //                    {
            //                        sb.Append($"{{G:{token.ControlBlock.Format}}}");
            //                    }
            //                    else
            //                    {
            //                        sb.Append($"{{G}}");
            //                    }
            //                }
            //                else
            //                {
            //                    sb.Append($"{{{token.ControlBlock.FunctionName}}}");
            //                }
            //            }
            //            break;
            //    }

            //    if (token.Type != TokenType.CONTROL_BLOCK)
            //    {
            //        if (null != token.ControlBlock)
            //        {
            //            if (token.ControlBlock.Type == ControlBlockType.ECB)
            //            {
            //                var s = string.Join(string.Empty, token.ControlBlock.ExceptValues).Replace("\\", "\\\\").Replace("}", "\\}");
            //                sb.Append($"{{-{s}}}");
            //            }
            //            else if (token.ControlBlock.Type == ControlBlockType.FMT)
            //            {
            //                sb.Append($">{token.ControlBlock.Value.Replace("\\", "\\\\")}<");
            //            }
            //        }
            //    }
            //}

            //return sb.ToString();
            return "TO BE IMPLEMENTED";
        }
    }
}
