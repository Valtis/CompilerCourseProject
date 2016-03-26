using System;
using CompilersCourseWork.AST;
using System.Collections.Generic;
using CompilersCourseWork.SemanticChecking;
using CompilersCourseWork.Parsing;
using CompilersCourseWork.Interpreting;

namespace CompilersCourseWork.CodeGeneration
{
    public class CodeGenerator : NodeVisitor
    {

        private IDictionary<string, VariableData> symbolTable;
        private readonly IList<string> strings;
        private readonly List<byte> bytecode;
        private int variableId;

        public byte[] Bytecodes
        {
            get
            {
                return bytecode.ToArray();
            }
        }

        public IList<string> Strings
        {
            get
            {
                return strings;
            }
        }

        public int Variables
        {
            get
            {
                return variableId;
            }
        }

        public CodeGenerator(IDictionary<string, VariableData> symbolTable, int variables)
        {
            variableId = variables;
            this.symbolTable = symbolTable;
            strings = new List<string>();
            strings.Add(""); // for default initialization
            bytecode = new List<byte>();
        }

        public void Visit(AssertNode node)
        {
            node.Children[0].Accept(this);
            Emit(Bytecode.ASSERT);
            Emit(node.Children[0].Line);
        }

        public void Visit(DivideNode node)
        {
            BinaryNode(node, Bytecode.DIV);
        }

        public void Visit(ForNode node)
        {
            // Extremely naive strategy: 
            // Create new variable end condition, and never reuse it elsewhere
            var loop_counter = symbolTable[((IdentifierNode)node.Children[0]).Name].id;
            var endVariable = variableId++;

            // Initialize loop counter
            node.Children[1].Accept(this);
            Emit(Bytecode.STORE_VARIABLE);
            Emit(loop_counter);
            
            // initialize end varible
            node.Children[2].Accept(this);
            Emit(Bytecode.STORE_VARIABLE);
            Emit(endVariable);

            var jumpTarget = bytecode.Count;

            node.Children[3].Accept(this);

            // increase loop counter
            Emit(Bytecode.PUSH_INT_VAR);
            Emit(loop_counter);
            Emit(Bytecode.PUSH_INT);
            Emit(1L);
            Emit(Bytecode.ADD);
            Emit(Bytecode.STORE_VARIABLE);
            Emit(loop_counter);          

            // do start\end comparison
            Emit(Bytecode.PUSH_INT_VAR);
            Emit(loop_counter);
            Emit(Bytecode.PUSH_INT_VAR);
            Emit(endVariable);
            Emit(Bytecode.IS_LESS_OR_EQUAL_INT);

            // and jump, based on results
            Emit(Bytecode.JUMP_IF_TRUE);
            Emit(jumpTarget);
        }

        public void Visit(IntegerNode node)
        {
            Emit(Bytecode.PUSH_INT);
            Emit(node.Value);
        }

        public void Visit(MultiplyNode node)
        {
            BinaryNode(node, Bytecode.MUL);
        }

        public void Visit(PrintNode node)
        {
            node.Children[0].Accept(this);
            switch (node.Children[0].NodeType())
            {
                case VariableType.INTEGER:
                    Emit(Bytecode.PRINT_INT);
                    break;
                case VariableType.STRING:
                    Emit(Bytecode.PRINT_STRING);
                    break;
                default:
                    throw new InternalCompilerError("Invalid type for print statement");
            }
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
            BinaryNode(node, Bytecode.SUB);
        }

        public void Visit(VariableDeclarationNode node)
        {
            if (node.Children.Count == 0)
            {
                // default initialize
                if (node.Type == VariableType.INTEGER)
                {
                    Emit(Bytecode.PUSH_INT);
                    Emit(0L);
                }
                else if (node.Type == VariableType.STRING)
                {
                    Emit(Bytecode.PUSH_STRING);
                    Emit(0);
                }
                else if (node.Type == VariableType.BOOLEAN)
                {
                    Emit(Bytecode.PUSH_FALSE);
                }
                else
                {
                    throw new InternalCompilerError("Invalid variable type for variable declaration");
                }
            }
            else
            {
                node.Children[0].Accept(this);
            }

            Emit(Bytecode.STORE_VARIABLE);
            Emit(symbolTable[node.Name].id);
        }

        public void Visit(VariableAssignmentNode node)
        {
            node.Children[0].Accept(this);
            Emit(Bytecode.STORE_VARIABLE);
            Emit(symbolTable[node.Name].id);
        }

        public void Visit(StringNode node)
        {
            Emit(Bytecode.PUSH_STRING);
            if (Strings.Contains(node.Value))
            {
                Emit(Strings.IndexOf(node.Value));
            }
            else
            {
                Emit(Strings.Count);
                Strings.Add(node.Value);
            }

        }

        public void Visit(ReadNode node)
        {
            switch (node.Children[0].NodeType())
            {
                case VariableType.INTEGER:
                    Emit(Bytecode.READ_INT);
                    break;
                case VariableType.STRING:
                    Emit(Bytecode.READ_STRING);
                    break;
                default:
                    throw new InternalCompilerError("Invalid type for readnode");
            }

            var name = ((IdentifierNode)node.Children[0]).Name;
            Emit(symbolTable[name].id);
        }

        public void Visit(NotNode node)
        {
            node.Children[0].Accept(this);
            Emit(Bytecode.NOT);
        }

        public void Visit(LessThanNode node)
        {
            node.Children[0].Accept(this);
            node.Children[1].Accept(this);

            switch (node.Children[0].NodeType())
            {
                case VariableType.INTEGER:
                    Emit(Bytecode.IS_LESS_INT);
                    break;
                case VariableType.STRING:
                    Emit(Bytecode.IS_LESS_STRING);
                    break;
                case VariableType.BOOLEAN:
                    Emit(Bytecode.IS_LESS_BOOLEAN);
                    break;
                default:
                    throw new InternalCompilerError("Invalid node type for LessThan node");

            }
        }

        public void Visit(IdentifierNode node)
        {
            var data = symbolTable[node.Name];
            switch (data.node.Type)
            {
                case VariableType.INTEGER:
                    Emit(Bytecode.PUSH_INT_VAR);
                    break;
                case VariableType.STRING:
                    Emit(Bytecode.PUSH_STRING_VAR);
                    break;
                case VariableType.BOOLEAN:
                    Emit(Bytecode.PUSH_BOOLEAN_VAR);
                    break;
                default:
                    throw new InternalCompilerError("Invalid variable type");
            }

            Emit(data.id);
        }

        public void Visit(ErrorNode node)
        {
            throw new InternalCompilerError("AST contains error nodes when generating code");
        }

        public void Visit(ComparisonNode node)
        {
            node.Children[0].Accept(this);
            node.Children[1].Accept(this);

            switch (node.Children[0].NodeType())
            {
                case VariableType.INTEGER:
                    Emit(Bytecode.IS_EQUAL_INT);
                    break;
                case VariableType.STRING:
                    Emit(Bytecode.IS_EQUAL_STRING);
                    break;
                case VariableType.BOOLEAN:
                    Emit(Bytecode.IS_EQUAL_BOOLEAN);
                    break;
                default:
                    throw new InternalCompilerError("Invalid comparison type");

            }
        }

        public void Visit(AndNode node)
        {
            BinaryNode(node, Bytecode.AND);
        }

        public void Visit(AddNode node)
        {
            if (node.NodeType() == VariableType.INTEGER)
            {
                BinaryNode(node, Bytecode.ADD);
            }
            else
            {
                BinaryNode(node, Bytecode.CONCAT);
            }
        }

        private void BinaryNode(Node node, byte code)
        {
            node.Children[0].Accept(this);
            node.Children[1].Accept(this);
            Emit(code);
        }

        private void Emit(byte code)
        {
            bytecode.Add(code);
        }

        private void Emit(long number)
        {
            var bytes = BitConverter.GetBytes(number);
            bytecode.AddRange(bytes);
        }

        private void Emit(int number)
        {
            var bytes = BitConverter.GetBytes(number);
            bytecode.AddRange(bytes);
        }


    }
}
