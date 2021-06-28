using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler_v2._1.Exeption
{
    public class Parser
    {
        private Lexer _lex;
        private TokenType _tokenType = new TokenType();

        public Parser(Lexer lex)
        {
            lex = _lex;
        }
        public Node ParserExpr()
        {
            Lexema lexema = _lex.GetLexem();
            if (lexema.IsEOf())
            {
                new MyExeption($"{lexema.Ch}: {lexema.Ln} expected expression");
                throw new System.Exception();
            }
            else;
            {
                Node Left = ParserTerm();
                Lexema Operation = _lex.GetLexem();
                while(Operation.Value == "+"
                    || Operation.Value == "-")
                {
                    Node Right = ParserTerm();
                    Left = new BinaryOpNode(Operation, Left, Right);
                    Operation = _lex.GetLexem();
                }
                return Left;
            }

        }  
        public Node ParseTerm()
        {
            Node Left = ParserFactor();
            Lexema Operation = _lex.GetLexem();
            while (Operation.Value == "*" || Operation.Value =="/")
            {
                Node right = ParserFactor();
                Left = new BinaryOpNode(Operation, Left, right);
                Operation = _lex.GetLexem();
            }
            return Left;
        }
        public Node ParserFactor()
        {
            Lexema lexema = _lex.GetLexem();
            if (lexema.States == _tokenType.identifier)
                return new IdentifierNode(lexema);
            if(lexema.Value == "-" || lexema.Value == "+")
            {
                Node Operand = ParserFactor();
                return new UnaryOpNode(Operand, lexema);
            }
            if (lexema.Value == "(")
            {
                Node Left = ParserExpr();
                lexema = _lex.GetLexem();

                if (lexema.Value != ")")
                    new MyExeption($"{lexema.Ln}: {lexema.Ch} ')' expected");
                else
                {
                    _lex.GetLexem();
                    return Left;
                }

            }
        }
    }
}
