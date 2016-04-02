using CompilersCourseWork.AST;
using CompilersCourseWork.ErrorHandling;
using CompilersCourseWork.Lexing;
using CompilersCourseWork.Tokens;
using System;
using System.Collections.Generic;

namespace CompilersCourseWork.Parsing
{
    /*
    Recursive descent parser
    */
    public class Parser
    {
        private Lexer lexer;
        private ErrorReporter reporter;

        // Operators that have two operands
        private IDictionary<Type, Type> binaryOperators;

        public Parser(Lexer lexer, ErrorReporter reporter)
        {
            this.lexer = lexer;
            this.reporter = reporter;

            binaryOperators = new Dictionary<Type, Type>
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

            if (token is EOFToken)
            {
                reporter.ReportError(Error.SYNTAX_ERROR,
                   "Program must contain at least a single statement",
                    0,
                    0);
                statements.AddChild(new ErrorNode());
            }

            while (!(lexer.PeekToken() is EOFToken))
            {
                statements.AddChild(ParseStatement());
            }

            return statements;
        }

        private Node ParseStatement()
        {
            var token = lexer.PeekToken();
            Node node = null;
            if (token is VarToken)
            {
                node = ParseVariableDeclaration();
            }
            else if (token is IdentifierToken)
            {
                node = ParseVariableAssignment();
            }
            else if (token is ForToken)
            {
                node = ParseForStatement();
            }
            else if (token is ReadToken)
            {
                node = ParseReadStatement();
            }
            else if (token is PrintToken)
            {
                node = ParsePrintStatement();
            }
            else if (token is AssertToken)
            {
                node = ParseAssertStatement();
            }
            else
            {
                ReportUnexpectedToken(
                    new List<Token>
                    {
                        new VarToken(),
                        new IdentifierToken(),
                        new ForToken(),
                        new ReadToken(),
                        new PrintToken(),
                        new AssertToken(),
                    },
                    lexer.PeekToken());
                SkipToToken<SemicolonToken>();

                node = new ErrorNode();
            }

            try
            {
                Expect<SemicolonToken>();
            }
            catch (InvalidParseException e)
            {
                SkipToToken<SemicolonToken>();
                lexer.NextToken();

                reporter.ReportError(Error.NOTE,
                    "This statement is missing its semicolon",
                    token.Line,
                    token.Column);

                return new ErrorNode();
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
                var varNode = new VariableDeclarationNode(
                    idToken.Line,
                    idToken.Column,
                    idToken.Identifier,
                    GetTypeFrom(typeToken)
                    );

                if (lexer.PeekToken() is AssignmentToken)
                {
                    lexer.NextToken();
                    varNode.AddChild(ParseExpression());
                }
                else if (!(lexer.PeekToken() is SemicolonToken) && !(lexer.PeekToken() is EOFToken))
                {
                    var token = lexer.PeekToken();
                    // better error reporting for this case
                    ReportUnexpectedToken(
                        new List<Token> { new SemicolonToken(), new AssignmentToken() },
                        token);

                    // if it looks like next token start an expression, note that 
                    // maybe assignment is missing

                    NoteMissingExpressionStartOrAssignment(token);

                    throw new InvalidParseException();
                }

                return varNode;
            }
            catch (InvalidParseException e)
            {
                reporter.ReportError(Error.NOTE,
                    "Error occured while parsing variable declaration",
                    varToken.Line,
                    varToken.Column
                );

                SkipToToken<SemicolonToken>();
                return new ErrorNode();
            }
        }

        Node ParseVariableAssignment()
        {
            var identifier = Expect<IdentifierToken>();
            try
            {

                try
                {
                    Expect<AssignmentToken>();
                }
                catch (InvalidParseException e)
                {
                    var token = lexer.PeekToken();

                    NoteMissingExpressionStartOrAssignment(token);

                    throw;
                }

                var expressionNode = ParseExpression();


                var assignNode = new VariableAssignmentNode(
                    identifier.Line,
                    identifier.Column,
                    identifier.Identifier);

                assignNode.AddChild(expressionNode);
                return assignNode;
            }
            catch (InvalidParseException e)
            {
                reporter.ReportError(Error.NOTE,
                    "Error occured while parsing variable assignment",
                    identifier.Line,
                    identifier.Column
                );

                SkipToToken<SemicolonToken>();
                return new ErrorNode();
            }
        }

        Node ParseForStatement()
        {
            var forToken = Expect<ForToken>();

            try
            {
                Node identifierNode, startExpression, stopExpression;
                try
                {
                    var identifierToken = Expect<IdentifierToken>();
                    identifierNode = new IdentifierNode(
                        identifierToken.Line,
                        identifierToken.Column,
                        identifierToken.Identifier);
                    Expect<InToken>();
                    startExpression = ParseExpression();
                    Expect<RangeToken>();
                    stopExpression = ParseExpression();
                    Expect<DoToken>();
                }
                catch (InvalidParseException e)
                {
                    // skip the for statement body
                    SkipToToken<EndToken>();
                    throw new InvalidParseException();
                }
                
                StatementsNode statements;
                if (lexer.PeekToken() is EndToken)
                {
                    reporter.ReportError(
                        Error.SYNTAX_ERROR,
                        "For loop must contain at least a single statement",
                        forToken.Line,
                        forToken.Column
                    );

                    SkipToToken<SemicolonToken>();
                    statements = new StatementsNode(0, 0);
                    statements.AddChild(new ErrorNode());
                }
                else
                {
                    statements = ParseForStatementBody();
                }


                return new ForNode(
                    forToken.Line,
                    forToken.Column,
                    identifierNode,
                    startExpression,
                    stopExpression,
                    statements
                    );
            }
            catch (InvalidParseException e)
            {
                reporter.ReportError(Error.NOTE,
                    "Error occured while parsing for loop",
                    forToken.Line,
                    forToken.Column
                );

                SkipToToken<SemicolonToken>();
                return new ErrorNode();
            }
        }

        StatementsNode ParseForStatementBody()
        {
            var statements = new StatementsNode(0, 0);
            do
            {
                /* If we see "for;", it is likely a case of missing 'end'
                   rather than completely malformed for loop 

                    e.g.

                        for foo in 1..2 do

                        // statements
                        for; 

                    In this case, break

                   */
                if (lexer.PeekToken() is ForToken)
                {
                    lexer.NextToken();
                    var peek = lexer.PeekToken();
                    lexer.Backtrack();
                    if (peek is SemicolonToken)
                    {
                        break;
                    }
                }


                var node = ParseStatement();

                statements.AddChild(node);

                if (node is ErrorNode)
                {
                    /* if the last statement is missing a semicolon, e.g. we have something like this:

                    for i in 0 .. 1 do
                       var a := 5
                    end for;

                    var b := 5;

                    the parser fails during parsing the declaration of 'a', skips to the semicolon after 'for', 
                    and keeps parsing statements. This would lead to scenario, where the var b := 5
                    declaration would be considered to be part of the loop body.

                    To prevent this, check if the previous tokens were "end for;", and if so, 
                    exit the loop

                    */
                    lexer.Backtrack();
                    lexer.Backtrack();
                    lexer.Backtrack();

                    var first = lexer.NextToken();
                    var second = lexer.NextToken();
                    if (first is EndToken &&
                        second is ForToken)
                    {
                        lexer.Backtrack();
                        lexer.Backtrack();
                        break;
                    }
                    else
                    {
                        // skip the semicolon
                        lexer.NextToken();
                    }

                }

            } while (
                    lexer.PeekToken() is VarToken ||
                    lexer.PeekToken() is IdentifierToken ||
                    lexer.PeekToken() is ForToken ||
                    lexer.PeekToken() is ReadToken ||
                    lexer.PeekToken() is PrintToken ||
                    lexer.PeekToken() is AssertToken);

            Expect<EndToken>();
            Expect<ForToken>();

            return statements;
        }

        Node ParseReadStatement()
        {
            var readToken = Expect<ReadToken>();

            try
            {
                var identifier = Expect<IdentifierToken>();
                return new ReadNode(
                    readToken.Line,
                    readToken.Column,
                    new IdentifierNode(
                        identifier.Line,
                        identifier.Column,
                        identifier.Identifier));
            }
            catch (InvalidParseException e)
            {

                reporter.ReportError(
                    Error.NOTE,
                    "Error occured while parsing read statement",
                    readToken.Line,
                    readToken.Column);

                lexer.Backtrack(); // in case identifier is missing and we consumed semicolon
                SkipToToken<SemicolonToken>();
                return new ErrorNode();
            }
        }

        Node ParsePrintStatement()
        {
            var printToken = Expect<PrintToken>();

            try
            {
                var expression = ParseExpression();
                return new PrintNode(
                 printToken.Line,
                 printToken.Column,
                 expression);
            }
            catch (InvalidParseException)
            {
                reporter.ReportError(
                    Error.NOTE,
                    "Error occured while parsing print statement",
                    printToken.Line,
                    printToken.Column);

                lexer.Backtrack(); // in case  we already consumed the semicolon
                SkipToToken<SemicolonToken>();
                return new ErrorNode();
            }
        }

        Node ParseAssertStatement()
        {
            var assertToken = Expect<AssertToken>();

            try
            {
                Expect<LParenToken>();
                var expression = ParseExpression();
                Expect<RParenToken>();

                return new AssertNode(
                    assertToken.Line,
                    assertToken.Column,
                    expression);
            }
            catch (InvalidParseException)
            {
                reporter.ReportError(
                    Error.NOTE,
                    "Error occured while parsing assert statement",
                    assertToken.Line,
                    assertToken.Column);

                lexer.Backtrack(); // in case  we already consumed the semicolon
                SkipToToken<SemicolonToken>();
                return new ErrorNode();
            }
        }
            
        Node ParseExpression()
        {

            if (lexer.PeekToken() is NotToken)
            {
                return ParseNotExpression();
            }


            var lhs = ParseOperand();

            var peek_token = lexer.PeekToken();

            if (binaryOperators.ContainsKey(peek_token.GetType()))
            {
                var op = lexer.NextToken();

                return (Node)Activator.CreateInstance(
                binaryOperators[op.GetType()],
                new object[] {
                    op.Line,
                    op.Column,
                    lhs,
                    ParseOperand() });
            }
            else if (
                !(peek_token is SemicolonToken) && 
                !(peek_token is RangeToken) && 
                !(peek_token is DoToken) && 
                !(peek_token is RParenToken))
            {
                reporter.ReportError(
                    Error.SYNTAX_ERROR,
                    "Unexpected token " + peek_token +
                    " when binary operator, " + new SemicolonToken() +
                    ", " + new DoToken() +
                    " or " + new RangeToken() +
                    "  was expected",
                    peek_token.Line,
                    peek_token.Column
                    );

                throw new InvalidParseException();
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
            var token = lexer.NextToken();

            if (token is NumberToken)
            {

                return new IntegerNode(
                    token.Line,
                    token.Column,
                    (token as NumberToken).Value);
            }
            else if (token is TextToken)
            {
                return new StringNode(
                    token.Line,
                    token.Column,
                    (token as TextToken).Text);
            }
            else if (token is IdentifierToken)
            {
                return new IdentifierNode(
                    token.Line,
                    token.Column,
                    (token as IdentifierToken).Identifier);
            }
            else if (token is LParenToken)
            {
                var exp = ParseExpression();
                Expect<RParenToken>();
                return exp;
            }
            else
            {
                reporter.ReportError(
                    Error.SYNTAX_ERROR,
                    "Unexpected token " + token + " when operand was expected",
                    token.Line,
                    token.Column);
                lexer.Backtrack();
                throw new InvalidParseException();
            }
        }



        private VariableType GetTypeFrom(Token token)
        {
            if (token is IntToken)
            {
                return VariableType.INTEGER;
            }
            else if (token is StringToken)
            {
                return VariableType.STRING;
            }
            else if (token is BoolToken)
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
            if (!(token is T))
            {

                // backtrack in case this token is a token that we will
                // skip to in the calling method
                lexer.Backtrack();

                // workaround for double semicolon reporting 
                ReportUnexpectedToken(new T(), token);

                throw new InvalidParseException();
            }

            return (T)token;
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
            while (!(lexer.PeekToken() is T ||
                lexer.PeekToken() is EOFToken))
            {
                lexer.NextToken();
            }

        }

        private void NoteMissingExpressionStartOrAssignment(Token token)
        {
            if (token is NumberToken ||
                token is TextToken ||
                token is IdentifierToken ||
                token is LParenToken ||
                token is MinusToken)
            {
                reporter.ReportError(
                    Error.NOTE,
                    "Maybe you are missing ':='",
                    token.Line,
                    token.Column);
            }
            else if (token is ComparisonToken)
            {
                reporter.ReportError(
                    Error.NOTE,
                    "Maybe you meant to use assignment ':=' instead of comparison '='",
                    token.Line,
                    token.Column);
            }
        }
    }
}
