using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMC.Utilities.RSG
{
    internal class Tokenizer
    {
        private static readonly List<char> MODIFIERS = "^!~".ToList();

        internal List<Token> TokenizedPattern { get; set; }

        private List<Token> tokenizedList;
        private int pos;

        internal bool Tokenize(string pattern)
        {
            tokenizedList = new List<Token>();
            for (pos = 0; pos < pattern.Length; pos++)
            {
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
                    default:
                        throw new InvalidPatternException($"Unknown token '{pattern[pos]}' found in Position {pos}.");
                }

                if (token.Type != TokenType.CONTROL_BLOCK || (token.Type == TokenType.CONTROL_BLOCK && token.ControlBlock != null && token.ControlBlock.Global == false))
                {
                    if (token.Type != TokenType.OPTIONAL && token.Type != TokenType.LITERAL && token.Type != TokenType.RANGE)
                    {
                        if (pos != pattern.Length - 1)
                        {
                            if (MODIFIERS.Contains(pattern[pos + 1])) HandleModifier(ref token, pattern);
                        }
                    }
                    //}

                    //if(token.Type != TokenType.CONTROL_BLOCK || (token.Type == TokenType.CONTROL_BLOCK && token.ControlBlock.Global == false))
                    //{ 
                    if (pos < pattern.Length - 1)
                    {
                        if (char.Equals(pattern[pos + 1], '(')) HandleCount(ref token, pattern);
                    }

                    if (pos < pattern.Length - 1)
                    {
                        if (char.Equals(pattern[pos + 1], '>')) HandleFormatControlBlock(ref token, pattern);
                    }


                    tokenizedList.Add(token);
                }
                //switch (pattern[pos])
                //{
                //    case 'a':
                //    case '0':
                //    case '9':
                //    case '@':
                //    case '.':
                //    case '+':
                //    case '%':
                //    case '*':
                //        HandleToken(pattern);
                //        break;             
                //    //case '^':
                //    //case '!':
                //    //case '~':
                //    //    if (pos == 0)
                //    //        throw new InvalidPatternException($"Modifier '{pattern[0]}' found in Position 0.  Modifiers must follow tokens.");
                //    //    HandleModifier(pattern);
                //    //    break;
                //    //case '[':
                //    //    HandleLiteral(pattern);
                //    //    break;
                //    default:
                //        throw new InvalidPatternException($"Unknown token '{pattern[pos]}' found in Position {pos}.");
                //}

            }

            TokenizedPattern = tokenizedList;
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
                tempToken = tokenizedList.Last();
                if (tempToken.ControlBlock != null && tempToken.ControlBlock.Global == true)
                    throw new DuplicateGlobalControlBlockException($"A second global control block was found starting at position {originalPosition}.");

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
                tokenizedList.Add(token);
            }
            else
            {
                tempToken.ControlBlock = cb;
                tokenizedList.RemoveAt(tokenizedList.Count - 1);
                tokenizedList.Add(tempToken);
            }
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
        }

        private void HandleModifier(ref Token token, string pattern)
        {
            pos++;
            var modifier = pattern[pos];
            var EOS = false;

            if (token.Type == TokenType.SYMBOL)
                throw new InvalidModifierException($"The token modifier '{modifier}' at position {pos} is not valid for the preceeding token.  Symbol tokens cannot have any modifiers.");

            do
            {
                switch (modifier)
                {
                    case '^':
                        if (token.Type == TokenType.NUMBER || token.Type == TokenType.NUMBER_EXCEPT_ZERO || token.Type == TokenType.NUMBER_SYMBOL)
                            throw new InvalidModifierException($"The token modifier '{modifier}' at position {pos} is not valid for the preceeding token.");

                        if (token.Modifier.HasFlag(ModifierType.UPPERCASE))
                            throw new DuplicateModifierException($"A duplicate modifier '{modifier}' is present at position {pos}.");

                        token.Modifier = token.Modifier | ModifierType.UPPERCASE;
                        break;
                    case '!':
                        if (token.Type == TokenType.NUMBER || token.Type == TokenType.NUMBER_EXCEPT_ZERO || token.Type == TokenType.SYMBOL || token.Type == TokenType.NUMBER_SYMBOL)
                            throw new InvalidModifierException($"The token modifier '{modifier}' at position {pos} is not valid for the preceeding token.");

                        if (token.Modifier.HasFlag(ModifierType.LOWERCASE))
                            throw new DuplicateModifierException($"A duplicate modifier '{modifier}' is present at position {pos}.");

                        token.Modifier = token.Modifier | ModifierType.LOWERCASE;
                        break;
                    case '~':
                        if (token.Type == TokenType.LETTER || token.Type == TokenType.SYMBOL || token.Type == TokenType.LETTER_SYMBOL)
                            throw new InvalidModifierException($"The token modifier '{modifier}' at position {pos} is not valid for the preceeding token.");

                        if (token.Modifier.HasFlag(ModifierType.EXCLUDE_ZERO))
                            throw new DuplicateModifierException($"A duplicate modifier '{modifier}' is present at position {pos}.");

                        token.Modifier = token.Modifier | ModifierType.EXCLUDE_ZERO;
                        break;
                }

                if (pos != pattern.Length - 1)
                {
                    if (!MODIFIERS.Contains(pattern[pos + 1]))
                        EOS = true;
                    else
                        pos++;
                }
                else
                {
                    EOS = true;
                }

            } while (!EOS);
        }

        private void HandleCount(ref Token token, string pattern)
        {
            var EOS = false;
            var originalPosition = pos;
            var commaCount = 0;
            var sb = new StringBuilder();

            pos++; //Consume the opening parenthesis
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

            var countString = sb.ToString();
            if (countString.Contains(","))
            {
                var div = countString.Split(',');
                if (string.IsNullOrWhiteSpace(div[0]))
                {
                    token.MinimumCount = 0;
                }
                else
                {
                    token.MinimumCount = int.Parse(div[0]);
                }

                token.MaximumCount = int.Parse(div[1]);

                if (token.MinimumCount > token.MaximumCount)
                    throw new InvalidPatternException($"The count token starting at position {originalPosition}, ending at {pos}, is not valid.  Maximum repeat count must be greater than minimum repeat count.");
            }
            else
            {
                var val = int.Parse(countString);
                if (val == 0)
                    throw new InvalidPatternException($"The count token starting at position {originalPosition}, ending at {pos}, is not valid.  Repeat count cannot be exactly 0.");

                token.MinimumCount = int.Parse(countString);
                token.MaximumCount = int.Parse(countString);
            }
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

        private void HandleFormatControlBlock(ref Token token, string pattern)
        {
            var EOS = false;
            var originalPosition = pos;
            var format = new StringBuilder();

            pos++; //Consume the open angle bracket
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

                        token.ControlBlock = new ControlBlock()
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