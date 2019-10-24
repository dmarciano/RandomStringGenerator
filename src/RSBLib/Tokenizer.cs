using System.Collections.Generic;
using System.Linq;

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
                        break;
                    default:
                        throw new InvalidPatternException($"Unknown token '{pattern[pos]}' found in Position {pos}.");
                }

                if (pos != pattern.Length - 1)
                {
                    if (MODIFIERS.Contains(pattern[pos + 1])) HandleModifier(ref token);
                    if (char.Equals(pattern[pos + 1], '(')) HandleCount(ref token);
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


        private void HandleModifier(ref Token token)
        {

        }

        private void HandleCount(ref Token token)
        {

        }

        private void HandleLiteral(ref Token token)
        {

        }
    }
}