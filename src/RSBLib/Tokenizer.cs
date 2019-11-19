using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMC.Utilities.RSG
{
    internal class Tokenizer : CultureHelper
    {
        private static readonly List<char> MODIFIERS = new List<char> { '^', '!', '~' };
        private static readonly List<char> TOKENS = new List<char> { 'a', '0', '9', '@', '.', '+', '%', '*', '[', '#', '<', '&' };

        internal List<TokenGroup> TokenizedPattern { get; set; }

        private List<TokenGroup> tokenizedGroup;
        private TokenGroup currentGroup;
        private int pos;
        private bool handled = false;
        private bool isInGroup = false;

        internal bool Tokenize(string pattern)
        {
            handled = false;
            tokenizedGroup = new List<TokenGroup>();
            currentGroup = new TokenGroup();
            
            for (pos = 0; pos < pattern.Length; pos++)
            {
                if (handled) handled = false;
                var token = new Token();
                switch (pattern[pos])
                {
                    case 'a':
                        token.Type = TokenType.LETTER;
                        break;
                    case '0':
                        token.Type = TokenType.NUMBER;
                        break;
                    case '9':
                        token.Type = TokenType.NUMBER_EXCEPT_ZERO;
                        break;
                    case '@':
                        token.Type = TokenType.SYMBOL;
                        break;
                    case '.':
                        token.Type = TokenType.LETTER_NUMBER;
                        break;
                    case '+':
                        token.Type = TokenType.LETTER_SYMBOL;
                        break;
                    case '%':
                        token.Type = TokenType.NUMBER_SYMBOL;
                        break;
                    case '*':
                        token.Type = TokenType.LETTER_NUMBER_SYMBOL;
                        break;
                    case '{':
                        token.Type = TokenType.CONTROL_BLOCK;
                        HandleControlBlock(ref token, pattern);
                        break;
                    case '[':
                        token.Type = TokenType.LITERAL;
                        HandleLiteral(ref token, pattern);
                        break;
                    case '#':
                        token.Type = TokenType.OPTIONAL;
                        HandleOptional(ref token, pattern);
                        break;
                    case '<':
                        token.Type = TokenType.RANGE;
                        HandleRange(ref token, pattern);
                        break;
                    case '/':
                        if (!isInGroup)
                        {
                            // This is the start of a new group
                            isInGroup = true;
                            if (currentGroup.Tokens.Count > 0)
                            {
                                tokenizedGroup.Add(currentGroup);
                                currentGroup = new TokenGroup();
                            }
                        }
                        else
                        {
                            // This is the end of a group
                            // Need to check for modifiers, repeat count, and ECB
                            if (pos != pattern.Length - 1)
                            {
                                if (pattern[pos + 1].Equals('(') || MODIFIERS.Contains(pattern[pos + 1]) || pattern[pos + 1].Equals('{') || pattern[pos +1].Equals('&'))
                                {
                                    do
                                    {
                                        if (pos == pattern.Length - 1) break;
                                        if (pattern[pos + 1].Equals('(')) { pos++; HandleCount(pattern); }
                                        if (pos == pattern.Length - 1) break;
                                        if (MODIFIERS.Contains(pattern[pos + 1])) { pos++; HandleModifier(pattern, true); }
                                        if (pos == pattern.Length - 1) break;
                                        if(pattern[pos +1].Equals('&')) { pos++; HandleCulture(pattern); }
                                        if (pos == pattern.Length - 1) break;
                                        if (pattern[pos + 1].Equals('{')) {var dummy = new Token(); pos++; HandleControlBlock(ref dummy, pattern);}
                                        if (pos == pattern.Length - 1) break;
                                        if (TOKENS.Contains(pattern[pos + 1])) break;

                                    } while ((pattern[pos + 1].Equals('(') || MODIFIERS.Contains(pattern[pos + 1])) && !pattern[pos + 1].Equals('{') && !pattern[pos + 1].Equals('&'));
                                }
                            }

                            tokenizedGroup.Add(currentGroup);
                            currentGroup = new TokenGroup();
                            isInGroup = false;
                        }
                        handled = true;
                        break;
                    case '&':
                        HandleCulture(pattern);
                        handled = true;
                        break;
                    case '\\':
                        token.Type = TokenType.LITERAL;
                        pos++;
                        if (pattern[pos].Equals('n'))
                        {
                            token.Value = Environment.NewLine;
                        }
                        else if (pattern[pos].Equals('t'))
                        {
                            token.Value = "\t";
                        }
                        else
                        {
                            throw new InvalidPatternException($"Unknown escape sequence \\{pattern[pos]} at position {pos - 1}.");
                        }
                        break;
                    case '^':
                    case '!':
                    case '~':
                        HandleModifier(pattern);
                        handled = true;
                        break;
                    case '(':
                        HandleCount(pattern);
                        handled = true;
                        break;
                    case '>':
                        HandleFormatControlBlock(pattern);
                        handled = true;
                        break;
                    default:
                        throw new InvalidPatternException($"Unknown token '{pattern[pos]}' found in Position {pos}.");
                }

                if (!handled)
                {
                    //tokenizedList.Add(token);
                    currentGroup.Tokens.Add(token);
                }
            }

            if (currentGroup.Tokens.Count > 0)
                tokenizedGroup.Add(currentGroup);
            //TokenizedPattern = tokenizedList;
            TokenizedPattern = tokenizedGroup;
            return true;
        }

        private void HandleControlBlock(ref Token token, string pattern)
        {
            var originalPosition = pos;

            if (char.Equals(pattern[pos + 1], '-'))
            {
                pos++;
                HandleExclusionBlock(ref token, pattern, originalPosition);
            }
            else
            {
                HandleFunctionBlock(ref token, pattern, originalPosition);
            }
        }

        private void HandleExclusionBlock(ref Token token, string pattern, int originalPosition)
        {
            Token tempToken = null;
            var cb = new ControlBlock();
            var sb = new StringBuilder();

            cb.Type = ControlBlockType.ECB;
            if (pos == 1)
            {
                //This is global ECB
                cb.Global = true;
            }
            else
            {
                //Get the previous token in the the list.  If it is not a regular token (i.e. it is a control box) throw exception
                //tempToken = tokenizedList.Last();
                tempToken = currentGroup.Tokens.Last();
                if (tempToken.ControlBlock != null) //&& tempToken.ControlBlock.Global == true)
                    throw new DuplicateGlobalControlBlockException($"A second exclusion control block was found starting at position {originalPosition}.");

                cb.Global = false;
            }

            if (pos + 1 >= pattern.Length)
                throw new InvalidPatternException($"No closing control block token found for the control block starting at position {originalPosition}.");

            var process = true;
            while (process)
            {
                pos++;
                if (pos >= pattern.Length)
                    throw new InvalidPatternException($"No closing control block token found for the control block starting at position {originalPosition}.");

                switch (pattern[pos])
                {
                    case '{':
                        sb.Append(pattern[pos]);
                        break;
                    case '\\':
                        pos++;
                        if (pattern[pos].Equals('}'))
                        {
                            sb.Append('}');
                        }
                        else if (pattern[pos].Equals('\\'))
                        {
                            sb.Append('\\');
                        }
                        else if (pattern[pos].Equals('-'))
                        {
                            sb.Append('-');
                        }
                        else
                        {
                            throw new InvalidPatternException($"Unknown escape sequence \\{pattern[pos]} at position {pos - 1}.");
                        }
                        break;
                    case '}':
                        process = false;
                        break;
                    default:
                        sb.Append(pattern[pos]);
                        break;
                }
            }

            cb.ExceptValues = sb.ToString().ToCharArray();
            if (cb.Global)
            {
                token.ControlBlock = cb;
                //tokenizedList.Add(token);
                currentGroup.Tokens.Add(token);
            }
            else
            {
                if (TokenType.DUMMY == token.Type)
                {
                    currentGroup.ControlBlock = cb;
                }
                else
                {
                    tempToken.ControlBlock = cb;
                    //tokenizedList.RemoveAt(tokenizedList.Count - 1);
                    //tokenizedList.Add(tempToken);
                    currentGroup.Tokens.RemoveAt(currentGroup.Tokens.Count - 1);
                    currentGroup.Tokens.Add(tempToken);
                }
            }

            handled = true;
        }

        private void HandleFunctionBlock(ref Token token, string pattern, int originalPosition)
        {
            var EOS = false;
            var force = false;
            var cb = new ControlBlock() { Type = ControlBlockType.FCB };
            var functionName = new StringBuilder();
            var formatter = new StringBuilder(string.Empty);
            char functionCode = 'x';

            while (!EOS)
            {
                pos++;
                if (pos >= pattern.Length)
                    throw new InvalidPatternException($"No closing control block token found for the control block starting at position {originalPosition}.");

                switch (pattern[pos])
                {
                    case 'T':
                    case 'G':
                        functionCode = pattern[pos];
                        pos++;
                        if (pos >= pattern.Length)
                            throw new InvalidPatternException($"No closing control block token found for the control block starting at position {originalPosition}.");
                        if (char.Equals(pattern[pos], '}'))
                        {
                            EOS = true;
                            //cb.FunctionName = functionCode.ToString();
                            //if (char.Equals(functionCode, 'T'))
                            //    cb.Function = () => DateTime.Now.ToString();
                            //else
                            //    cb.Function = () => Guid.NewGuid().ToString();

                            //token.ControlBlock = cb;
                            //EOS = true;
                        }
                        else if (char.Equals(pattern[pos], '?'))
                        {
                            if (!char.Equals(pattern[pos + 1], '}'))
                                throw new InvalidPatternException($"Expecting a control block closing brace at positions {pos + 1}, which was not found.");

                            force = true;
                            EOS = true;
                            pos++;
                        }
                        else if (char.Equals(pattern[pos], ':'))
                        {
                            cb.FunctionName = functionCode.ToString();
                            while (!EOS)
                            {
                                pos++;
                                if (pos >= pattern.Length)
                                    throw new InvalidPatternException($"No closing control block token found for the control block starting at position {originalPosition}.");
                                if (char.Equals(pattern[pos], '}'))
                                {
                                    if (formatter.Length < 1)
                                        throw new InvalidPatternException($"Expected function formatting string at position {pos}.");

                                    EOS = true;
                                }
                                else if (char.Equals(pattern[pos], '?'))
                                {
                                    if (!char.Equals(pattern[pos + 1], '}'))
                                        throw new InvalidPatternException($"Expecting a control block closing brace at positions {pos + 1}, which was not found.");

                                    force = true;
                                }
                                else
                                {
                                    formatter.Append(pattern[pos]);
                                }
                            }
                        }
                        else
                        {
                            throw new InvalidPatternException($"An unexpected token '{pattern[pos]}' was found at position {pos}.");
                        }
                        break;
                    default:
                        functionName.Append(pattern[pos]);
                        while (true)
                        {
                            pos++;
                            if (pos >= pattern.Length)
                                throw new InvalidPatternException($"No closing control block token found for the control block starting at position {originalPosition}.");

                            if (pattern[pos].Equals('}')) break;
                            functionName.Append(pattern[pos]);
                        }
                        cb.FunctionName = functionName.ToString();

                        if (string.IsNullOrWhiteSpace(cb.FunctionName))
                            throw new InvalidPatternException($"User-defined function name cannot be null, empty, or whitespace (UDF starting at position {originalPosition}).");

                        EOS = true;
                        break;
                }
            }

            var format = formatter.ToString();

            if (char.Equals(functionCode, 'G'))
            {
                cb.FunctionName = "GUID";
                if (!string.IsNullOrWhiteSpace(format))
                {
                    if (!string.Equals("N", format, StringComparison.CurrentCulture) && !string.Equals("D", format, StringComparison.CurrentCulture)
                        && !string.Equals("B", format, StringComparison.CurrentCulture) && !string.Equals("P", format, StringComparison.CurrentCulture)
                        && !string.Equals("X", format, StringComparison.CurrentCulture))
                    {
                        throw new InvalidPatternException($"An unrecognized GUID format string was found.  Format string found: '{format}'.");
                    }


                    if (force)
                    {
                        token.Type = TokenType.LITERAL;
                        token.Value = Guid.NewGuid().ToString(format);
                        cb = null;
                    }
                    else
                    {
                        cb.Function = () => Guid.NewGuid().ToString(format);
                    }
                }
                else
                {
                    if (force)
                    {
                        token.Type = TokenType.LITERAL;
                        token.Value = Guid.NewGuid().ToString();
                        cb = null;
                    }
                    else
                    {
                        cb.Function = () => Guid.NewGuid().ToString();
                    }
                }


            }
            else if (char.Equals(functionCode, 'T'))
            {
                cb.FunctionName = "DATETIME";
                if (!string.IsNullOrWhiteSpace(format))
                {
                    if (force)
                    {
                        token.Type = TokenType.LITERAL;
                        token.Value = DateTime.Now.ToString(format);
                        cb = null;
                    }
                    else
                    {
                        cb.Function = () => DateTime.Now.ToString(format);
                    }
                }
                else
                {
                    if (force)
                    {
                        token.Type = TokenType.LITERAL;
                        token.Value = DateTime.Now.ToString();
                        cb = null;
                    }
                    else
                    {
                        cb.Function = () => DateTime.Now.ToString();
                    }
                }
            }

            token.ControlBlock = cb;

            if (TokenType.DUMMY == token.Type)
            {
                token.Type = TokenType.CONTROL_BLOCK;

                if (isInGroup)
                {
                    tokenizedGroup.Add(currentGroup);
                    currentGroup = new TokenGroup();
                    currentGroup.Tokens.Add(token);
                    handled = true;
                }
            }
        }

        private void HandleModifier(string pattern, bool applyToGroup = false)
        {
            //var lastToken = tokenizedList[tokenizedList.Count - 1];

            Token lastToken = null;

            var modifier = pattern[pos];

            if (!applyToGroup)
            {
                lastToken = currentGroup.Tokens[currentGroup.Tokens.Count - 1];
                if (lastToken.Type == TokenType.SYMBOL || lastToken.Type == TokenType.CONTROL_BLOCK)
                    throw new InvalidModifierException($"The token modifier '{modifier}' at position {pos} is not valid for the preceeding token.  Symbol and Control Block tokens cannot have any modifiers.");
            }

            switch (modifier)
            {
                case '^':

                    if (!applyToGroup)
                    {
                        if (lastToken.Type == TokenType.NUMBER || lastToken.Type == TokenType.NUMBER_EXCEPT_ZERO || lastToken.Type == TokenType.NUMBER_SYMBOL)
                            throw new InvalidModifierException($"The token modifier '{modifier}' at position {pos} is not valid for the preceeding token.");

                        if (lastToken.Modifier.HasFlag(ModifierType.UPPERCASE))
                            throw new DuplicateModifierException($"A duplicate modifier '{modifier}' is present at position {pos}.");

                        lastToken.Modifier = lastToken.Modifier | ModifierType.UPPERCASE;
                    }
                    else
                    {
                        if (currentGroup.Modifier.HasFlag(ModifierType.UPPERCASE))
                            throw new DuplicateModifierException($"A duplicate modifier '{modifier}' is present at position {pos}.");

                        currentGroup.Modifier = currentGroup.Modifier | ModifierType.UPPERCASE;
                    }
                    break;
                case '!':
                    if (!applyToGroup)
                    {
                        if (lastToken.Type == TokenType.NUMBER || lastToken.Type == TokenType.NUMBER_EXCEPT_ZERO || lastToken.Type == TokenType.SYMBOL || lastToken.Type == TokenType.NUMBER_SYMBOL)
                            throw new InvalidModifierException($"The token modifier '{modifier}' at position {pos} is not valid for the preceeding token.");

                        if (lastToken.Modifier.HasFlag(ModifierType.LOWERCASE))
                            throw new DuplicateModifierException($"A duplicate modifier '{modifier}' is present at position {pos}.");

                        lastToken.Modifier = lastToken.Modifier | ModifierType.LOWERCASE;
                    }
                    else
                    {
                        if (currentGroup.Modifier.HasFlag(ModifierType.LOWERCASE))
                            throw new DuplicateModifierException($"A duplicate modifier '{modifier}' is present at position {pos}.");

                        currentGroup.Modifier = currentGroup.Modifier | ModifierType.LOWERCASE;
                    }
                    break;
                case '~':
                    if (!applyToGroup)
                    {
                        if (lastToken.Type == TokenType.LETTER || lastToken.Type == TokenType.SYMBOL || lastToken.Type == TokenType.LETTER_SYMBOL)
                            throw new InvalidModifierException($"The token modifier '{modifier}' at position {pos} is not valid for the preceeding token.");

                        if (lastToken.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                            throw new DuplicateModifierException($"A duplicate modifier '{modifier}' is present at position {pos}.");

                        lastToken.Modifier = lastToken.Modifier | ModifierType.EXCLUDE_ZERO;
                    }
                    else
                    {
                        if (currentGroup.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                            throw new DuplicateModifierException($"A duplicate modifier '{modifier}' is present at position {pos}.");

                        currentGroup.Modifier = currentGroup.Modifier | ModifierType.EXCLUDE_ZERO;
                    }
                    break;
            }

            //pos++;
            //var modifier = pattern[pos];
            //var EOS = false;

            //if (token.Type == TokenType.SYMBOL)
            //    throw new InvalidModifierException($"The token modifier '{modifier}' at position {pos} is not valid for the preceeding token.  Symbol tokens cannot have any modifiers.");

            //do
            //{
            //    switch (modifier)
            //    {
            //        case '^':
            //            if (token.Type == TokenType.NUMBER || token.Type == TokenType.NUMBER_EXCEPT_ZERO || token.Type == TokenType.NUMBER_SYMBOL)
            //                throw new InvalidModifierException($"The token modifier '{modifier}' at position {pos} is not valid for the preceeding token.");

            //            if (token.Modifier.HasFlag(ModifierType.UPPERCASE))
            //                throw new DuplicateModifierException($"A duplicate modifier '{modifier}' is present at position {pos}.");

            //            token.Modifier = token.Modifier | ModifierType.UPPERCASE;
            //            break;
            //        case '!':
            //            if (token.Type == TokenType.NUMBER || token.Type == TokenType.NUMBER_EXCEPT_ZERO || token.Type == TokenType.SYMBOL || token.Type == TokenType.NUMBER_SYMBOL)
            //                throw new InvalidModifierException($"The token modifier '{modifier}' at position {pos} is not valid for the preceeding token.");

            //            if (token.Modifier.HasFlag(ModifierType.LOWERCASE))
            //                throw new DuplicateModifierException($"A duplicate modifier '{modifier}' is present at position {pos}.");

            //            token.Modifier = token.Modifier | ModifierType.LOWERCASE;
            //            break;
            //        case '~':
            //            if (token.Type == TokenType.LETTER || token.Type == TokenType.SYMBOL || token.Type == TokenType.LETTER_SYMBOL)
            //                throw new InvalidModifierException($"The token modifier '{modifier}' at position {pos} is not valid for the preceeding token.");

            //            if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
            //                throw new DuplicateModifierException($"A duplicate modifier '{modifier}' is present at position {pos}.");

            //            token.Modifier = token.Modifier | ModifierType.EXCLUDE_ZERO;
            //            break;
            //    }

            //    if (pos != pattern.Length - 1)
            //    {
            //        if (!MODIFIERS.Contains(pattern[pos + 1]))
            //            EOS = true;
            //        else
            //            pos++;
            //    }
            //    else
            //    {
            //        EOS = true;
            //    }

            //} while (!EOS);
        }

        private void HandleCount(string pattern)
        {
            var EOS = false;
            var originalPosition = pos;
            var commaCount = 0;
            var sb = new StringBuilder();
            Token lastToken = null;

            if (!isInGroup)
            {
                //var lastToken = tokenizedList[tokenizedList.Count - 1];
                lastToken = currentGroup.Tokens[currentGroup.Tokens.Count - 1];
                if (lastToken.Type == TokenType.CONTROL_BLOCK && lastToken.ControlBlock != null && lastToken.ControlBlock.Global)
                    throw new InvalidModifierException("Count blocks are not valid on global control blocks.");
            }

            while (!EOS)
            {
                pos++;
                if (pos >= pattern.Length)
                    throw new InvalidPatternException($"No closing count token found for the opening count token at position {originalPosition}.");

                switch (pattern[pos])
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case ',':
                        if (char.Equals(pattern[pos], ','))
                        {
                            commaCount++;
                            if (commaCount > 1)
                                throw new InvalidPatternException($"An extra comma was detected at position {pos}.");
                        }
                        sb.Append(pattern[pos]);
                        break;
                    case ')':
                        EOS = true;
                        break;
                    default:
                        throw new InvalidPatternException($"Invalid value '{pattern[pos]}' found at position {pos}.");

                }
            }

            var min = 0;
            var max = 0;
            var countString = sb.ToString();

            if (countString.Contains(","))
            {
                var div = countString.Split(',');
                if (string.IsNullOrWhiteSpace(div[0]))
                {
                    min = 0;
                }
                else
                {
                    min = int.Parse(div[0]);
                }

                max = int.Parse(div[1]);

                if (min > max)
                    throw new InvalidPatternException($"The count token starting at position {originalPosition}, ending at {pos}, is not valid.  Maximum repeat count must be greater than minimum repeat count.");
            }
            else
            {
                var val = int.Parse(countString);
                if (val == 0)
                    throw new InvalidPatternException($"The count token starting at position {originalPosition}, ending at {pos}, is not valid.  Repeat count cannot be exactly 0.");

                min = val;
                max = val;
            }

            if (isInGroup)
            {
                currentGroup.MinimumCount = min;
                currentGroup.MaximumCount = max;
            }
            else
            {
                lastToken.MinimumCount = min;
                lastToken.MaximumCount = max;
            }


            /*
            if (countString.Contains(","))
            {
                var div = countString.Split(',');
                if (string.IsNullOrWhiteSpace(div[0]))
                {
                        lastToken.MinimumCount = 0;
                }
                else
                {
                    lastToken.MinimumCount = int.Parse(div[0]);
                }

                lastToken.MaximumCount = int.Parse(div[1]);

                if (lastToken.MinimumCount > lastToken.MaximumCount)
                    throw new InvalidPatternException($"The count token starting at position {originalPosition}, ending at {pos}, is not valid.  Maximum repeat count must be greater than minimum repeat count.");
            }
            else
            {
                var val = int.Parse(countString);
                if (val == 0)
                    throw new InvalidPatternException($"The count token starting at position {originalPosition}, ending at {pos}, is not valid.  Repeat count cannot be exactly 0.");

                lastToken.MinimumCount = int.Parse(countString);
                lastToken.MaximumCount = int.Parse(countString);
            }
            */
        }

        private void HandleLiteral(ref Token token, string pattern)
        {
            var EOS = false;
            var openings = 1;
            var originalPosition = pos;
            var literal = new StringBuilder();

            while (!EOS)
            {
                pos++;
                if (pos >= pattern.Length)
                    throw new InvalidPatternException($"No closing literal token found for the opening literal token at position {originalPosition}.");

                switch (pattern[pos])
                {
                    case '[':
                        openings += 1;
                        literal.Append(pattern[pos]);
                        break;
                    case '\\':
                        pos++;
                        if (pattern[pos].Equals('n'))
                        {
                            literal.Append(Environment.NewLine);
                        }
                        else if (pattern[pos].Equals('t'))
                        {
                            literal.Append("\t");
                        }
                        else if (pattern[pos].Equals(']'))
                        {
                            literal.Append("]");
                        }
                        else if (pattern[pos].Equals('\\'))
                        {
                            literal.Append('\\');
                        }
                        else
                        {
                            throw new InvalidPatternException($"Unknown escape sequence \\{pattern[pos]} at position {originalPosition}.");
                        }
                        break;
                    case ']':
                        openings -= 1;
                        if (openings == 0)
                        {
                            token.Value = literal.ToString();
                            EOS = true;
                        }
                        else
                        {
                            literal.Append(pattern[pos]);
                        }
                        break;
                    default:
                        literal.Append(pattern[pos]);
                        break;
                }
            }
        }

        private void HandleOptional(ref Token token, string pattern)
        {
            var EOS = false;
            var originalPosition = pos;
            var optional = new StringBuilder();
            if (null == token.Values) token.Values = new List<string>();

            while (!EOS)
            {
                pos++;
                if (pos >= pattern.Length)
                    throw new InvalidPatternException($"No closing optional token found for the opening optional token at position {originalPosition}.");

                switch (pattern[pos])
                {
                    case '\\':
                        pos++;
                        if (pattern[pos].Equals('n'))
                        {
                            optional.Append(Environment.NewLine);
                        }
                        else if (pattern[pos].Equals('t'))
                        {
                            optional.Append("\t");
                        }
                        else if (pattern[pos].Equals('#'))
                        {
                            optional.Append('#');
                        }
                        else if (pattern[pos].Equals('\\'))
                        {
                            optional.Append('\\');
                        }
                        else if (pattern[pos].Equals(','))
                        {
                            optional.Append(',');
                        }
                        else
                        {
                            throw new InvalidPatternException($"Unknown escape sequence \\{pattern[pos]} at position {originalPosition}.");
                        }
                        break;
                    case ',':
                        token.Values.Add(optional.ToString());
                        optional.Clear();
                        break;
                    case '#':
                        token.Values.Add(optional.ToString());
                        EOS = true;
                        break;
                    default:
                        optional.Append(pattern[pos]);
                        break;
                }
            }
        }

        private void HandleRange(ref Token token, string pattern)
        {
            var EOS = false;
            var originalPosition = pos;
            Range range;
            if (null == token.Ranges) token.Ranges = new List<Range>();

            while (!EOS)
            {
                pos++;
                if (pos >= pattern.Length)
                    throw new InvalidPatternException($"No closing range token found for the opening range token at position {originalPosition}.");


                switch (pattern[pos])
                {
                    case '\\':
                        pos++;
                        char start;
                        if (pattern[pos].Equals('\\'))
                        {
                            start = '\\';
                        }
                        else if (pattern[pos].Equals('>'))
                        {
                            start = '>';
                        }
                        else
                        {
                            throw new InvalidPatternException($"Unknown escape sequence \\{pattern[pos]} at position {originalPosition}.");
                        }

                        if (pattern[pos + 1] == '-')
                        {
                            range = new Range() { Start = pattern[pos] };
                            pos++;  // Move to the hypen
                            pos++;   // Move to character after hypen
                            range.End = pattern[pos];
                            token.Ranges.Add(range);
                        }
                        else
                        {
                            token.Ranges.Add(new Range { Start = start, End = start });
                        }
                        break;
                    case '>':
                        EOS = true;
                        break;
                    default:
                        if (pattern[pos + 1] == '-')
                        {
                            range = new Range() { Start = pattern[pos] };
                            pos++;  // Move to the hypen
                            pos++;   // Move to character after hypen
                            range.End = pattern[pos];
                            token.Ranges.Add(range);
                        }
                        else
                        {
                            token.Ranges.Add(new Range { Start = pattern[pos], End = pattern[pos] });
                        }
                        break;
                }
            }
        }

        private void HandleCulture(string pattern)
        {
            var EOS = false;
            var originalPosition = pos;
            var sb = new StringBuilder();
            Token lastToken = null;

            if (!isInGroup)
            {
                lastToken = currentGroup.Tokens[currentGroup.Tokens.Count - 1];
                if (lastToken.Type == TokenType.CONTROL_BLOCK && lastToken.ControlBlock.Global)
                    throw new InvalidCultureException("Language/Culture tokens are not valid on global control blocks.");
            }

            while (!EOS)
            {
                pos++;
                if (pos >= pattern.Length)
                    throw new InvalidPatternException($"No closing language/culture token found for the opening language/culture token at position {originalPosition}.");

                if (pattern[pos].Equals('&'))
                {
                    EOS = true;
                }
                else
                {
                    sb.Append(pattern[pos]);
                }
            }

            var cultureName = sb.ToString().Trim();
            if (!IsCultureValid(cultureName))
                throw new InvalidCultureException($"The culture name '{cultureName}', found at position {originalPosition} is not a valid culture name.");

            if (isInGroup)
                currentGroup.CultureName = cultureName;
            else
                lastToken.CultureName = cultureName;
        }

        private void HandleFormatControlBlock(string pattern)
        {
            var EOS = false;
            var originalPosition = pos;
            var format = new StringBuilder();

            //var lastToken = tokenizedList[tokenizedList.Count - 1];
            var lastToken = currentGroup.Tokens[currentGroup.MaximumCount - 1];
            if (lastToken.Type == TokenType.CONTROL_BLOCK && lastToken.ControlBlock != null && lastToken.ControlBlock.Global)
                throw new InvalidModifierException($"Format control blocks are not valid on global control blocks");

            while (!EOS)
            {
                pos++;
                if (pos >= pattern.Length)
                    throw new InvalidPatternException($"No closing format block token found for the format control block starting at position {originalPosition}.");

                switch (pattern[pos])
                {
                    case '\\':
                        pos++;
                        if (pattern[pos].Equals('n'))
                        {
                            format.Append(Environment.NewLine);
                        }
                        else if (pattern[pos].Equals('t'))
                        {
                            format.Append("\t");
                        }
                        else if (pattern[pos].Equals('<'))
                        {
                            format.Append("<");
                        }
                        else if (pattern[pos].Equals('\\'))
                        {
                            format.Append('\\');
                        }
                        else
                        {
                            throw new InvalidPatternException($"Unknown escape sequence \\{pattern[pos]} at position {originalPosition}.");
                        }
                        break;
                    case '<':
                        var formatString = format.ToString();
                        if (!formatString.Contains("{0"))
                            throw new InvalidPatternException($"No argument placeholder '{{0}}' found in the format block starting at position {originalPosition}.");

                        lastToken.ControlBlock = new ControlBlock()
                        {
                            Type = ControlBlockType.FMT,
                            Value = formatString
                        };
                        EOS = true;
                        break;
                    default:
                        format.Append(pattern[pos]);
                        break;
                }
            }
        }
    }
}