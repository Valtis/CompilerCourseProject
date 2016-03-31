using System;
using CompilersCourseWork.AST;
using System.Collections.Generic;
using CompilersCourseWork.Parsing;
using CompilersCourseWork.ErrorHandling;

namespace CompilersCourseWork.SemanticChecking
{
    /*
    Semantic checks using visitor pattern. Also builds symboltable
    */
    public class SemanticChecker : NodeVisitor
    {
        private readonly IDictionary<string, VariableData> symbolTable;
        private int variableId;
        private ErrorReporter reporter;

        public IDictionary<string, VariableData> SymbolTable
        {
            get
            {
                return symbolTable;
            }
        }

        public int Variables
        {
            get
            {
                return variableId;
            }
        }

        public SemanticChecker(ErrorReporter reporter)
        {
            symbolTable = new Dictionary<string, VariableData>();
            variableId = 0;
            this.reporter = reporter;
        }


        public void Visit(AssertNode node)
        {
            if (node.Children.Count != 1)
            {
                throw new InternalCompilerError("Invalid child count for assert node");
            }

            HandleExpression(node.Children[0], new List<VariableType> { VariableType.BOOLEAN });
        }

        public void Visit(DivideNode node)
        {
            BinaryOperator(
                node,
                "/",
                () => VariableType.INTEGER,
                new List<VariableType> { VariableType.INTEGER });
        }

        public void Visit(ForNode node)
        {
            if (node.Children.Count != 4)
            {
                throw new InternalCompilerError("Invalid child count for ForNode");
            }

            if (!(node.Children[0] is IdentifierNode))
            {
                throw new InternalCompilerError("Invalid node type for loop variable");
            }

            var loopVariable = (IdentifierNode)node.Children[0];
            loopVariable.Accept(this);

            if (loopVariable.NodeType() != VariableType.INTEGER &&
                loopVariable.NodeType() != VariableType.ERROR_TYPE)
            {
                reporter.ReportError(
                    Error.SEMANTIC_ERROR,
                    "Loop control variable must have type '" + VariableType.INTEGER.Name() + "'" +
                    " but has type '" + loopVariable.NodeType().Name() + "'",
                    loopVariable.Line,
                    loopVariable.Column);
                NoteVariableWasDeclaredHere(loopVariable.Name);
            }

            var acceptableTypes = new List<VariableType> { VariableType.INTEGER };

            var startExpression = node.Children[1];
            HandleExpression(startExpression, acceptableTypes);

            var endExpression = node.Children[2];
            HandleExpression(endExpression, acceptableTypes);

            var loopBody = node.Children[3];
            loopBody.Accept(this);


            CheckLoopControlVariableUsageInStartEndExpression(node, loopVariable, startExpression, endExpression);
            CheckVariableAssignmentUsedInStartEndExpression(loopVariable, startExpression, endExpression, loopBody);
            CheckLoopControlVariableAssignmentInBody(node, loopVariable, loopBody);
        }

        private void CheckLoopControlVariableUsageInStartEndExpression(ForNode node, IdentifierNode loopVariable, Node startExpression, Node endExpression)
        {
            var stack = new Stack<Node>();
            stack.Push(endExpression);
            stack.Push(startExpression);

            while (stack.Count != 0)
            {
                var current = stack.Pop();

                if (current is IdentifierNode && ((IdentifierNode)current).Name == loopVariable.Name)
                {
                    reporter.ReportError(
                        Error.WARNING,
                        "Possibly incorrect or confusing usage of loop control variable '" + loopVariable.Name + "' in loop expression",
                        current.Line,
                        current.Column);

                    reporter.ReportError(
                        Error.NOTE,
                        "Loop is here",
                        node.Line,
                        node.Column);

                    reporter.ReportError(
                        Error.NOTE_GENERIC,
                        "Loop start and end expressions are evaluated only once before entering the loop",
                        0,
                        0);
                    break;
                }

                foreach (var child in current.Children)
                {
                    stack.Push(child);
                }
            }
        }

        private void CheckVariableAssignmentUsedInStartEndExpression(IdentifierNode loopVariable, Node startExpression, Node endExpression, Node loopBody)
        {
            var stack = new Stack<Node>();
            stack.Push(endExpression);
            stack.Push(startExpression);

            var loopExpressionVariables = new Dictionary<IdentifierNode, IdentifierNode>();

            while (stack.Count != 0)
            {
                var current = stack.Pop();

                if (current is IdentifierNode && ((IdentifierNode)current).Name != loopVariable.Name)
                {
                    var id = (IdentifierNode)current;
                    if (!loopExpressionVariables.ContainsKey(id))
                    {
                        loopExpressionVariables.Add(id, id);
                    }
                }

                foreach (var child in current.Children)
                {
                    stack.Push(child);
                }
            }

            stack.Push(loopBody);
            while (stack.Count != 0)
            {
                var current = stack.Pop();

                if (current is VariableAssignmentNode &&
                    loopExpressionVariables.ContainsKey(new IdentifierNode(0, 0, ((VariableAssignmentNode)current).Name)))
                {
                    reporter.ReportError(
                        Error.WARNING,
                        "Possibly incorrect or confusing assignment to a variable '" +
                        ((VariableAssignmentNode)current).Name + "', used in loop expression,",
                        current.Line,
                        current.Column);

                    var id = loopExpressionVariables[new IdentifierNode(0, 0, ((VariableAssignmentNode)current).Name)];

                    reporter.ReportError(
                        Error.NOTE,
                        "Previous usage here",
                        id.Line,
                        id.Column);

                    reporter.ReportError(
                        Error.NOTE_GENERIC,
                        "Loop start and end expressions are evaluated only once before entering the loop",
                        0,
                        0);
                    break;
                }

                foreach (var child in current.Children)
                {
                    stack.Push(child);
                }
            }
        }

        private void CheckLoopControlVariableAssignmentInBody(ForNode node, IdentifierNode loopVariable, Node loopBody)
        {
            var stack = new Stack<Node>();
            stack.Push(loopBody);

            while (stack.Count != 0)
            {
                var current = stack.Pop();

                if (current is VariableAssignmentNode && ((VariableAssignmentNode)current).Name == loopVariable.Name)
                {
                    reporter.ReportError(
                        Error.SEMANTIC_ERROR,
                        "Cannot reassign control variable '" + loopVariable.Name + "'",
                        current.Line,
                        current.Column);

                    reporter.ReportError(
                        Error.NOTE,
                        "Loop is here",
                        node.Line,
                        node.Column);
                    break;
                }

                foreach (var child in current.Children)
                {
                    stack.Push(child);
                }
            }
        }

        private void HandleExpression(Node expression, List<VariableType> acceptableTypes)
        {
            expression.Accept(this);

            if (!acceptableTypes.Contains(expression.NodeType()) &&
                expression.NodeType() != VariableType.ERROR_TYPE)
            {

                var typeStr = BuildTypeErrorString(acceptableTypes);
                reporter.ReportError(
                    Error.SEMANTIC_ERROR,
                    "Expression must have " + typeStr +
                    " but has type '" + expression.NodeType().Name() + "'",
                    expression.Line,
                    expression.Column);

                if (expression is IdentifierNode)
                {
                    NoteVariableWasDeclaredHere(((IdentifierNode)expression).Name);
                }
            }
        }

        public void Visit(IntegerNode node)
        {
            // do nothing
        }

        public void Visit(MultiplyNode node)
        {
            BinaryOperator(
                node,
                "*",
                () => VariableType.INTEGER,
                new List<VariableType> { VariableType.INTEGER });
        }

        public void Visit(PrintNode node)
        {
            if (node.Children.Count != 1)
            {
                throw new InternalCompilerError("Invalid child count for print node");
            }

            HandleExpression(node.Children[0], new List<VariableType> { VariableType.INTEGER, VariableType.STRING });
        }

        public void Visit(StatementsNode node)
        {
            foreach (var child in node.Children)
            {
                child.Accept(this);
            }
        }

        public void Visit(SubtractNode node)
        {
            BinaryOperator(
                node,
                "-",
                () => VariableType.INTEGER,
                new List<VariableType> { VariableType.INTEGER });
        }

        public void Visit(VariableDeclarationNode node)
        {
            if (SymbolTable.ContainsKey(node.Name))
            {
                reporter.ReportError(
                    Error.SEMANTIC_ERROR,
                    "Redeclaration of variable '" + node.Name + "'",
                    node.Line,
                    node.Column);

                var name = node.Name;
                NoteVariableWasDeclaredHere(name);
            }
            else
            {
                SymbolTable[node.Name] = new VariableData(variableId++, node);
            }

            if (node.Children.Count == 0)
            {
                return;
            }

            if (node.Children.Count > 1)
            {
                throw new InternalCompilerError("Variable declaration node has more children than 1");
            }


            node.Children[0].Accept(this);
            TypeCheckVariableAssignment(node, node);

            Node n = null;
            if ((n = HasSelfAssignment(node)) != null)
            {
                reporter.ReportError(
                    Error.SEMANTIC_ERROR,
                    "Cannot initialize variable with self",
                    n.Line,
                    n.Column);
            }
        }



        public void Visit(VariableAssignmentNode node)
        {
            if (node.Children.Count != 1)
            {
                throw new InternalCompilerError("Variable assignment node has no children");
            }

            if (!SymbolTable.ContainsKey(node.Name))
            {
                ReportUndeclaredVariable(node);
                return;
            }

            node.Children[0].Accept(this);

            var variableNode = SymbolTable[node.Name].node;
            TypeCheckVariableAssignment(node, variableNode);

            // warn on self assignment. This could be improved by also warning, if there are operations
            // that do not change the outcome (like c := c + 0;) but for now we just warn on the specific 
            // case of var c := c;

            if (node.Children[0] is IdentifierNode && ((IdentifierNode)node.Children[0]).Name == node.Name)
            {
                reporter.ReportError(
                    Error.WARNING,
                    "Self assignment has no effect",
                    node.Children[0].Line,
                    node.Children[0].Column);
            }
        }



        public void Visit(StringNode node)
        {
            // do nothing
        }

        public void Visit(ReadNode node)
        {
            if (node.Children.Count != 1)
            {
                throw new InternalCompilerError("Invalid node count for read node");
            }

            if (!(node.Children[0] is IdentifierNode))
            {
                throw new InternalCompilerError("Invalid node type for read node");
            }

            var identifier = (IdentifierNode)node.Children[0];
            if (!SymbolTable.ContainsKey(identifier.Name))
            {
                ReportUndeclaredVariable(identifier);
                return;
            }

            identifier.Accept(this);

            if (identifier.NodeType() == VariableType.BOOLEAN)
            {
                reporter.ReportError(
                    Error.SEMANTIC_ERROR,
                    "Variable has invalid type '" + VariableType.BOOLEAN.Name() + "'" +
                    " when '" + VariableType.INTEGER.Name() + "' or '" +
                    VariableType.STRING.Name() + "' were expected",
                    identifier.Line,
                    identifier.Column
                    );

                NoteVariableWasDeclaredHere(identifier.Name);
            }
        }

        public void Visit(NotNode node)
        {
            if (node.Children.Count != 1)
            {
                throw new InternalCompilerError("Invalid child count for not-node");
            }

            node.Children[0].Accept(this);

            if (node.Children[0].NodeType() != VariableType.BOOLEAN)
            {
                reporter.ReportError(
                    Error.SEMANTIC_ERROR,
                    "Incompatible expression for operator '!'",
                    node.Line,
                    node.Column);

                reporter.ReportError(
                    Error.NOTE,
                    "Expression has type '" + node.Children[0].NodeType().Name() + "'",
                    node.Children[0].Line,
                    node.Children[0].Column);

                reporter.ReportError(
                    Error.NOTE,
                    "Operator '!' expects the expression to have type '" + VariableType.BOOLEAN.Name() + "'",
                    node.Line,
                    node.Column);

                node.SetType(VariableType.ERROR_TYPE);
            }
            else
            {
                node.SetType(VariableType.BOOLEAN);
            }
        }

        public void Visit(LessThanNode node)
        {
            BinaryOperator(
                node, "<",
                () => VariableType.BOOLEAN,
                new List<VariableType> { VariableType.INTEGER, VariableType.STRING, VariableType.BOOLEAN });
        }

        public void Visit(IdentifierNode node)
        {
            if (SymbolTable.ContainsKey(node.Name))
            {
                node.SetType(SymbolTable[node.Name].node.NodeType());
            }
            else
            {
                node.SetType(VariableType.ERROR_TYPE);
                ReportUndeclaredVariable(node);
            }
        }

        public void Visit(ErrorNode node)
        {
            // do nothing
        }

        public void Visit(ComparisonNode node)
        {
            BinaryOperator(
                node, "=",
                () => VariableType.BOOLEAN,
                new List<VariableType> { VariableType.INTEGER, VariableType.STRING, VariableType.BOOLEAN });
        }

        public void Visit(AndNode node)
        {
            BinaryOperator(
                node,
                "&",
                () => node.Children[0].NodeType(),
                new List<VariableType> { VariableType.BOOLEAN });
        }

        public void Visit(AddNode node)
        {
            BinaryOperator(
                node,
                "+",
                () => node.Children[0].NodeType(),
                new List<VariableType> { VariableType.INTEGER, VariableType.STRING });
        }
        
        private void BinaryOperator(Node node, string name, Func<VariableType> nodeType, IList<VariableType> acceptableOperandTypes)
        {
            if (node.Children.Count != 2)
            {
                throw new InternalCompilerError("Invalid child count for less-than-node");
            }

            foreach (var child in node.Children)
            {
                child.Accept(this);
            }

            BinaryOperatorTypeChecks(node, name, nodeType, acceptableOperandTypes);
        }

        private void BinaryOperatorTypeChecks(Node node, string name, Func<VariableType> nodeType, IList<VariableType> acceptableOperandTypes)
        {

            // if one (or both) children are errors, the error has already been reported.
            // Just set the node type as error and return
            if (node.Children[0].NodeType() == VariableType.ERROR_TYPE ||
                node.Children[1].NodeType() == VariableType.ERROR_TYPE)
            {
                node.SetType(VariableType.ERROR_TYPE);
                return;
            }

            if (node.Children[0].NodeType() != node.Children[1].NodeType())
            {
                ReportInvalidTypes(node, name);
                node.SetType(VariableType.ERROR_TYPE);
            }
            else if (!acceptableOperandTypes.Contains(node.Children[0].NodeType()) ||
                !acceptableOperandTypes.Contains(node.Children[1].NodeType()))
            {
                ReportUnacceptableTypes(node, name, acceptableOperandTypes);
                node.SetType(VariableType.ERROR_TYPE);
            }
            else
            {
                node.SetType(nodeType());
            }
        }

        private void ReportUnacceptableTypes(Node node, string op, IList<VariableType> acceptableOperandTypes)
        {
            var typeStr = BuildTypeErrorString(acceptableOperandTypes);

            reporter.ReportError(
                            Error.SEMANTIC_ERROR,
                            "Operator '" + op + "' expects operands to have " + typeStr,
                            node.Line,
                            node.Column);
            LeftRightTypeNote(node);
        }

        private static string BuildTypeErrorString(IList<VariableType> acceptableOperandTypes)
        {
            var types = "'" + acceptableOperandTypes[0].Name() + "'";
            for (int i = 1; i < acceptableOperandTypes.Count; ++i)
            {
                types += ", '" + acceptableOperandTypes[i].Name() + "'";
            }

            var typeStr = "type";
            if (acceptableOperandTypes.Count > 1)
            {
                typeStr = "one of types";
            }

            typeStr += " " + types;
            return typeStr;
        }

        private void ReportInvalidTypes(Node node, string op)
        {
            reporter.ReportError(
                Error.SEMANTIC_ERROR,
                "Incompatible types for operator '" + op + "'",
                node.Line,
                node.Column);
            LeftRightTypeNote(node);
        }

        private void LeftRightTypeNote(Node node)
        {
            reporter.ReportError(
                            Error.NOTE,
                            "Left side expression has type '" + node.Children[0].NodeType().Name() + "'",
                            node.Children[0].Line,
                            node.Children[0].Column);

            reporter.ReportError(
                Error.NOTE,
                "Right side expression has type '" + node.Children[1].NodeType().Name() + "'",
                node.Children[1].Line,
                node.Children[1].Column);
        }

        private static Node HasSelfAssignment(VariableDeclarationNode node)
        {
            var stack = new Stack<Node>();
            foreach (var child in node.Children)
            {
                stack.Push(child);
            }

            while (stack.Count != 0)
            {
                var current = stack.Pop();

                if (current is IdentifierNode && ((IdentifierNode)current).Name == node.Name)
                {
                    return current;
                }

                foreach (var child in current.Children)
                {
                    stack.Push(child);
                }
            }
            return null;
        }

        private void TypeCheckVariableAssignment(Node assignmentNode, VariableDeclarationNode declarationNode)
        {
            var childType = assignmentNode.Children[0].NodeType();
            // if type is error type, the error has already been reported
            if (declarationNode.Type != childType && childType != VariableType.ERROR_TYPE)
            {
                reporter.ReportError(
                    Error.SEMANTIC_ERROR,
                    "Expression has invalid type '" +
                        childType.Name() + "' while variable has type '" + declarationNode.Type.Name() + "'",
                    assignmentNode.Children[0].Line,
                    assignmentNode.Children[0].Column);


                reporter.ReportError(
                    Error.NOTE,
                    "Variable was declared here",
                    declarationNode.Line,
                    declarationNode.Column);
            }
        }

        private void ReportUndeclaredVariable(VariableAssignmentNode node)
        {
            var name = node.Name;
            ReportUndeclaredVariable(node, node.Name);
        }

        private void ReportUndeclaredVariable(IdentifierNode node)
        {
            ReportUndeclaredVariable(node, node.Name);
        }


        private void ReportUndeclaredVariable(Node node, string name)
        {
            reporter.ReportError(
                            Error.SEMANTIC_ERROR,
                            "Variable '" + name + "' has not been declared at this point",
                            node.Line,
                            node.Column);
        }

        private void NoteVariableWasDeclaredHere(string name)
        {
            reporter.ReportError(
                                Error.NOTE,
                                "Variable was declared here",
                                SymbolTable[name].node.Line,
                                SymbolTable[name].node.Column);
        }
    }
}
