using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMC.Utilities.RSG
{
    internal class Tokenizer
    {
        private static readonly List<char> MODIFIERS = "^!~".ToList();

        internal List<Token> TokenizedPattern { get; set; }

        private int pos;

        internal (bool valid, string parseError) Tokenize(string pattern)
        {
            var tokenizedList = new List<Token>();
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
                    case '[':
                        token.Type = TokenType.LITERAL;
                        HandleLiteral(ref token, pattern);
                        break;
                    default:
                        throw new InvalidPatternException($"Unknown token '{pattern[pos]}' found in Position {pos}.");
                }

                if (pos != pattern.Length - 1)
                {
                    if (MODIFIERS.Contains(pattern[pos + 1])) HandleModifier(ref token, pattern);
                    if (char.Equals(pattern[pos + 1], '(')) HandleCount(ref token, pattern);
                }
                tokenizedList.Add(token);
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
            return (true, string.Empty);
        }


        private void HandleModifier(ref Token token, string pattern)
        {

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
                        if (char.Equals(pattern[pos], ',')){
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
                if (div.Length != 2)
                    throw new InvalidPatternException($"The count token starting at position {originalPosition}, ending at {pos}, is not valid.  Too many commas detected.");

                if (string.IsNullOrWhiteSpace(div[0]))
                {
                    token.MinimumCount = 0;
                }
                else
                {
                    token.MinimumCount = int.Parse(div[0]);
                }

                token.MaximumCount = int.Parse(div[1]);

                if(token.MinimumCount > token.MaximumCount)
                    throw new InvalidPatternException($"The count token starting at position {originalPosition}, ending at {pos}, is not valid.  Maximum repeat count must be greater than minimum repeat count.");
            }
            else
            {
                var val = int.Parse(countString);
                if(val == 0)
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
    }
}