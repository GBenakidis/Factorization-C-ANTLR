using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniC {
    public class SOPFVisitor : MiniCBaseVisitor<Stack<CExprAddition>> {
        private CCompileUnit m_root;
        private StreamWriter m_dotFile;
        private int multCouter = 0;

        public ASTElement MRoot => m_root;
        private Stack<(ASTElement, int)> m_contextData =
            new Stack<(ASTElement, int)>();
        private Stack<CExprAddition> sumNodes = new Stack<CExprAddition>();

        public override Stack<CExprAddition> VisitCompileUnit(MiniCParser.CompileUnitContext context) {
            CCompileUnit newNode = new CCompileUnit();
            m_root = newNode;

            m_contextData.Push((newNode, CCompileUnit.CT_BODY));
            foreach (var child in context.statement()) {
                Visit(child);
            }
            m_contextData.Pop();

            return sumNodes;
        }

        public override Stack<CExprAddition> VisitExprAddSub(MiniCParser.ExprAddSubContext context) {
            (ASTElement, int) parent_data;

            switch (context.op.Type) {
                case MiniCLexer.PLUS:
                    CExprAddition newNodePlus = new CExprAddition();
                    // We figure who is the parent of this node.
                    parent_data = m_contextData.Peek();
                    // We are adding to the parent the children of this node. (Item1 = parent, Item2 = children)
                    parent_data.Item1.AddChild(newNodePlus, parent_data.Item2);
                    newNodePlus.MParent = parent_data.Item1;

                    m_contextData.Push((newNodePlus, CExprAddition.CT_LEFT));

                    // We visit the children on the left.
                    Visit(context.expr(0));
                    m_contextData.Pop();

                    m_contextData.Push((newNodePlus, CExprAddition.CT_RIGHT));

                    // We visit the children on the right.
                    Visit(context.expr(1));
                    m_contextData.Pop();

                    if (multCouter == 2) {
                        sumNodes.Push(newNodePlus);
                    }
                    multCouter = 0;

                    break;
            }
            return sumNodes;
        }

        public override Stack<CExprAddition> VisitExprMulDiv(MiniCParser.ExprMulDivContext context) {
            (ASTElement, int) parent_data;

            switch (context.op.Type) {
                case MiniCLexer.MULTI:
                    CExprMultiplication newNodeMulti = new CExprMultiplication();
                    // We figure who is the parent of this node.
                    parent_data = m_contextData.Peek();
                    // We are adding to the parent the children of this node. (Item1 = parent, Item2 = children)
                    parent_data.Item1.AddChild(newNodeMulti, parent_data.Item2);
                    newNodeMulti.MParent = parent_data.Item1;
                    multCouter++;
                    // We give the parents before adding the child to the stack and determining the place of the child to be on CT_LEFT.
                    m_contextData.Push((newNodeMulti, CExprMultiplication.CT_LEFT));
                    // We visit the children on the left.
                    Visit(context.expr(0));
                    m_contextData.Pop();

                    // We give the parents before adding the child to the stack and determining the place of the child to be on CT_RIGHT.
                    m_contextData.Push((newNodeMulti, CExprMultiplication.CT_RIGHT));
                    // We visit the children on the right.
                    Visit(context.expr(1));
                    m_contextData.Pop();

                    break;
            }
            return sumNodes;
        }

        public override Stack<CExprAddition> VisitTerminal(ITerminalNode node) {
            (ASTElement, int) parent_data;

            switch (node.Symbol.Type) {
                case MiniCLexer.VARIABLE:
                    CExprVARIABLE newVARIABLE = new CExprVARIABLE(node.Symbol.Text);
                    // We figure who is the parent of this node.
                    parent_data = m_contextData.Peek();
                    // We are adding to the parent the children of this node. (Item1 = parent, Item2 = children)
                    parent_data.Item1.AddChild(newVARIABLE, parent_data.Item2);
                    newVARIABLE.MParent = parent_data.Item1;

                    break;
                case MiniCLexer.INTEGER:
                    CExprINTEGER newINTEGER = new CExprINTEGER(node.Symbol.Text);
                    // We figure who is the parent of this node.
                    parent_data = m_contextData.Peek();
                    // We are adding to the parent the children of this node. (Item1 = parent, Item2 = children)
                    parent_data.Item1.AddChild(newINTEGER, parent_data.Item2);
                    newINTEGER.MParent = parent_data.Item1;

                    break;
            }
            return sumNodes;
        }

    }
}
