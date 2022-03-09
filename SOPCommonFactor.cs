using System;
using System.Collections.Generic;

namespace MiniC {
    class SOPCommonFactor : MiniCASTBaseVisitor<CExprINTEGER> {
        private CExprINTEGER cfNode;
        private String nameOfLeftChild = null;
        private String nameOfRightChild = null;
        private Boolean firstInsertion = false;

        public override CExprINTEGER VisitCExprAddition(CExprAddition node) {
            base.VisitCExprAddition(node);
            int intnameOfLeftChild = Int32.Parse(nameOfLeftChild);
            int intnameOfRightChild = Int32.Parse(nameOfRightChild);

            if (intnameOfLeftChild == intnameOfRightChild) {
                return cfNode;
            } else {
                return null;
            }
        }

        public override CExprINTEGER VisitCExprMultiplication(CExprMultiplication node) {
            base.VisitCExprMultiplication(node);
            return null;
        }

        public override CExprINTEGER VisitCExprINTEGER(CExprINTEGER node) {
            if (firstInsertion == false) {
                nameOfLeftChild = node.MName;
                cfNode=node;
                firstInsertion = true;
            } else {
                nameOfRightChild = node.MName;
            }
            return null;
        }

        public override CExprINTEGER VisitCExprVARIABLE(CExprVARIABLE node) {
            return null;
        }
    }


}
