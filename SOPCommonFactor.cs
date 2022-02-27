using System;
using System.Collections.Generic;

namespace MiniC {
    class SOPCommonFactor : MiniCASTBaseVisitor<int> {
        private Stack<CExprAddition> sumNodes = new Stack<CExprAddition>();
        private String nameOfLeftChild = null;
        private String nameOfRightChild = null;
        private Boolean firstInsertion = false;

        public override int VisitCExprAddition(CExprAddition node) {
            base.VisitCExprAddition(node);
            int intnameOfLeftChild = Int32.Parse(nameOfLeftChild);
            int intnameOfRightChild = Int32.Parse(nameOfRightChild);

            if (intnameOfLeftChild == intnameOfRightChild) {
                return intnameOfRightChild;
            } else {
                return 0;
            }
        }

        public override int VisitCExprMultiplication(CExprMultiplication node) {
            base.VisitCExprMultiplication(node);
            return 0;
        }

        public override int VisitCExprINTEGER(CExprINTEGER node) {
            if (firstInsertion == false) {
                nameOfLeftChild = node.MName;
                firstInsertion = true;
            } else {
                nameOfRightChild = node.MName;
            }
            return 0;
        }

        public override int VisitCExprVARIABLE(CExprVARIABLE node) {
            return 0;
        }
    }


}
