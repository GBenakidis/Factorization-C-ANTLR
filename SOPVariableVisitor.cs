using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniC {
    class SOPVariableVisitor : MiniCASTBaseVisitor<Stack<CExprVARIABLE>> {
        private Stack<CExprVARIABLE> varNodes = new Stack<CExprVARIABLE>();

        public override Stack<CExprVARIABLE> VisitCExprAddition(CExprAddition node) {
            base.VisitCExprAddition(node);
            return varNodes;
        }

        public override Stack<CExprVARIABLE> VisitCExprMultiplication(CExprMultiplication node) {
            base.VisitCExprMultiplication(node);
            return varNodes;
        }

        public override Stack<CExprVARIABLE> VisitCExprINTEGER(CExprINTEGER node) {
            return varNodes;
        }

        public override Stack<CExprVARIABLE> VisitCExprVARIABLE(CExprVARIABLE node) {
            varNodes.Push(node);
            return varNodes;
        }
    }
}
