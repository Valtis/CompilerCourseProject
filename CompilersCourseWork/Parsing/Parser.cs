using CompilersCourseWork.AST;
using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Lexing;
using CompilersCourseWork.Tokens;
using System;
using System.Collections.Generic;

namespace CompilersCourseWork.Parsing
{
    public class Parser
    {
        private Lexer lexer;
        private ErrorReporter reporter;

        IDictionary<Type, Type> binary_operators;

        public Parser(Lexer lexer, ErrorReporter reporter)
        {
            this.lexer = lexer;
            this.reporter = reporter;

            binary_operators = new Dictionary<Type, Type>
            {
                { typeof(PlusToken), typeof(AddNode) },
                { typeof(MinusToken), typeof(SubtractNode) },
                { typeof(MultiplyToken), typeof(MultiplyNode) },
                { typeof(DivideToken), typeof(DivideNode) },
                { typeof(AndToken), typeof(AndNode) },
                { typeof(ComparisonToken), typeof(ComparisonNode) },
                { typeof(LessThanToken), typeof(LessThanNode) },
            };
        }

        public Node Parse()
        {
            return ParseStatements();
        }

        private Node ParseStatements()
        {
            var token = lexer.PeekToken();
            var statements = new StatementsNode(token.Line, token.Column);
            while (lexer.PeekToken().GetType() != typeof(EOFToken))
            {
                statements.AddChildren(ParseStatement());
            }

            return statements;
        }

        private Node ParseStatement()
        {
            var token = lexer.PeekToken();
            var type = token.GetType();
            Node node = null;
            if (type == typeof(VarToken))
            {
                node = ParseVariableDeclaration();
            }
            else
            {
                ReportUnexpectedToken(
                    new List<Token>
                    {
                        new VarToken()
                    },
                    lexer.PeekToken());
                SkipToToken<SemicolonToken>();
            }

            try
            {
                Expect<SemicolonToken>();
            }
            catch (InvalidParseException e)
            {
                reporter.ReportError(Error.NOTE,
                    "This statement is missing its semicolon",
                    token.Line,
                    token.Column
                );
            }

            return node;
        }


        private Node ParseVariableDeclaration()
        {
            var varToken = lexer.NextToken();

            try
            {
                var idToken = Expect<IdentifierToken>();

                Expect<ColonToken>();

                var typeToken = lexer.NextToken();
                var var_node = new VariableNode(
                    idToken.Line,
                    idToken.Column,
                    idToken.Identifier,
                    GetTypeFrom(typeToken)
                    );

                if (lexer.PeekToken().GetType().Equals(typeof(AssignmentToken)))
                {
                    lexer.NextToken();
                    var_node.AddChildren(ParseExpression());
                }

                return var_node;
            }
            catch (InvalidParseException e)
            {
                SkipToToken<SemicolonToken>();
                reporter.ReportError(Error.NOTE,
                    "Error occured while parsing variable declaration",
                    varToken.Line,
                    varToken.Column
                );
                return new ErrorNode();
            }
        }

        Node ParseExpression()
        {
            
            if (lexer.PeekToken().GetType() == typeof(NotToken))
            {
                return ParseNotExpression();
            }


            var lhs = ParseOperand();            

            var peek_token = lexer.PeekToken().GetType();
            
            if (binary_operators.ContainsKey(peek_token))
            {
                var op = lexer.NextToken();

                return (Node)Activator.CreateInstance(
                binary_operators[op.GetType()],
                new object[] {
                    op.Line,
                    op.Column,
                    lhs,
                    ParseOperand() });
            }

            return lhs;
            
        }

        private Node ParseNotExpression()
        {
            var notToken = Expect<NotToken>();
            return new NotNode(
                notToken.Line,
                notToken.Column,
                ParseOperand());
        }


        private Node ParseOperand()
        {
            long sign = 1;
            var token = lexer.NextToken();

            if (token.GetType().Equals(typeof(MinusToken)))
            {
                sign = -1;
                token = lexer.NextToken();
            }

            if (token.GetType().Equals(typeof(NumberToken)))
            {

                return new IntegerNode(
                    token.Line,
                    token.Column,
                    sign * (token as NumberToken).Value);
            }
            else if (token.GetType().Equals(typeof(TextToken)))
            {
                return new StringNode(
                    token.Line,
                    token.Column,
                    (token as TextToken).Text);
            }
            else if (token.GetType().Equals(typeof(IdentifierToken)))
            {
                return new IdentifierNode(
                    token.Line,
                    token.Column,
                    (token as IdentifierToken).Identifier);
            }
            else if (token.GetType().Equals(typeof(LParenToken)))
            {
                var exp = ParseExpression();
                Expect<RParenToken>();
                return exp;
            }


            lexer.Backtrack();
            throw new NotImplementedException("Not implemented");
        }



        private VariableType GetTypeFrom(Token token)
        {
            var type = token.GetType();
            if (type.Equals(typeof(IntToken)))
            {
                return VariableType.INTEGER;
            }
            else if (type.Equals(typeof(StringToken)))
            {
                return VariableType.STRING;
            }
            else if (type.Equals(typeof(BoolToken)))
            {
                return VariableType.BOOLEAN;
            }

            ReportUnexpectedToken(
                new List<Token>
                {
                    new StringToken(),
                    new IntToken(),
                    new BoolToken()
                },
                token);

            throw new InvalidParseException();
        }


        private T Expect<T>() where T : Token, new()
        {
            var token = lexer.NextToken();

            var asT = token as T;
            if (asT == null)
            {

                // backtrack in case this token is a token that we will
                // skip to in the calling method
                lexer.Backtrack();

                ReportUnexpectedToken(new T(), token);
                throw new InvalidParseException();
            }

            return asT;
        }

        private void ReportUnexpectedToken(Token expected, Token actual)
        {
            reporter.ReportError(Error.SYNTAX_ERROR,
                    "Expected token " + expected + " but was " + actual,
                    actual.Line,
                    actual.Column
                    );
        }

        private void ReportUnexpectedToken(IList<Token> expected, Token actual)
        {
            string str = "";
            foreach (var e in expected)
            {
                str += " " + e;
            }

            reporter.ReportError(Error.SYNTAX_ERROR,
                    "Expected one of" + str + " but was " + actual,
                    actual.Line,
                    actual.Column
                    );
        }

        private void SkipToToken<T>() where T : Token
        {
            while (lexer.PeekToken()?.GetType() != typeof(T))
            {
                lexer.NextToken();
            }

        }
    }
}
