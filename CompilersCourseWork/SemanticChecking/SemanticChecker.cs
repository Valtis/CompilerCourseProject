using System;
using CompilersCourseWork.AST;
using System.Collections.Generic;
using CompilersCourseWork.Parsing;
using CompilersCourseWork.ErrorHandling;

namespace CompilersCourseWork.SemanticChecking
{
    public class SemanticChecker : NodeVisitor
    {
        class VariableData
        {
            public readonly int id;
            public readonly VariableDeclarationNode node;

            public VariableData(int id, VariableDeclarationNode node)
            {
                this.id = id;
                this.node = node;
            }

        }

        private IDictionary<string, VariableData> symbolTable;
        private int variableId;
        private ErrorReporter reporter;

        public SemanticChecker(ErrorReporter reporter)
        {
            symbolTable = new Dictionary<string, VariableData>();
            variableId = 0;
            this.reporter = reporter;
        }


        public void Visit(AssertNode node)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            if (symbolTable.ContainsKey(node.Name))
            {
                reporter.ReportError(
                    Error.SEMANTIC_ERROR,
                    "Redeclaration of variable '" + node.Name + "'",
                    node.Line,
                    node.Column);

                reporter.ReportError(
                    Error.NOTE,
                    "Previous declaration here",
                    symbolTable[node.Name].node.Line,
                    symbolTable[node.Name].node.Column);
            }
            else
            {
                symbolTable[node.Name] = new VariableData(variableId++, node);
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

            if (!symbolTable.ContainsKey(node.Name))
            {
                reporter.ReportError(
                    Error.SEMANTIC_ERROR,
                    "Variable '" + node.Name + "' has not been declared at this point",
                    node.Line,
                    node.Column);
                return;
            }

            node.Children[0].Accept(this);

            var variableNode = symbolTable[node.Name].node;
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
            throw new NotImplementedException();
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
            if (symbolTable.ContainsKey(node.Name))
            {
                node.SetType(symbolTable[node.Name].node.NodeType());
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
            string types = "'" + acceptableOperandTypes[0].Name()  + "'";
            for (int i = 1; i < acceptableOperandTypes.Count; ++i)
            {
                types += ", '" + acceptableOperandTypes[i].Name() + "'"; 
            }

            string typeStr = "type";
            if (acceptableOperandTypes.Count > 1)
            {
                typeStr = "one of types";
            }

            reporter.ReportError(
                            Error.SEMANTIC_ERROR,
                            "Operator '" + op + "' expects operands to have " + typeStr + " " + types,
                            node.Line,
                            node.Column);
            LeftRightTypeNote(node);
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
    }
}
