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

        public Parser(Lexer lexer, ErrorReporter reporter)
        {
            this.lexer = lexer;
            this.reporter = reporter;
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
            var token =  lexer.PeekToken();
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
            }

            try
            {
                Expect<SemicolonToken>();
            } catch (InvalidParseException e)
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
                }
                , token);

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
            string str =  "";
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
