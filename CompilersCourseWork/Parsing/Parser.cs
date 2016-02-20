using CompilersCourseWork.AST;
using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Lexing;
using CompilersCourseWork.Tokens;
using System;

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
            var type = lexer.PeekToken().GetType();
            Node node = null;
            if (type == typeof(VarToken))
            {
                node = ParseVariableDeclaration();
            }

            lexer.NextToken();
            if (node == null)
            {
                throw new NotImplementedException("Not implemented");
            }
            return node;
        }


        private Node ParseVariableDeclaration()
        {
            lexer.NextToken();
            var idToken = (IdentifierToken)lexer.NextToken();
            lexer.NextToken();

            var typeToken = lexer.NextToken();
            var var_node = new VariableNode(
                idToken.Line, 
                idToken.Column,
                idToken.Identifier,
                GetTypeFrom(typeToken)
                );


            return var_node;
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

            throw new NotImplementedException("Error handling not implemented");
        }
        
    }
}
