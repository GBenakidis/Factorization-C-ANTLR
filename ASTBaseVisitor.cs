namespace MiniC {
    public abstract class ASTBaseVisitor<T> {
        public virtual T Visit(ASTElement node) {
            /* Going to the node it took as the default
             * and call it on that node Accept, and passes 
             * the current Visitor where we are at.*/
            return node.Accept(this);
        }

        public virtual T VisitChildren(ASTElement node) {
            for (int i = 0; i < node.GetContextNumber(); i++) {
                foreach (ASTElement child in node.GetChildren(i)) {
                    Visit(child);
                }
            }
            return default(T);
        }

        public virtual T VisitContextChildren(ASTElement node, int context) {
            foreach (ASTElement child in node.GetChildren(context)) {
                Visit(child);
            }
            return default(T);
        }
    }

    public class MiniCASTBaseVisitor<T> : ASTBaseVisitor<T> {
        public virtual T VisitCompileUnit(CCompileUnit node) {
            return VisitChildren(node);
        }

        public virtual T VisitCFunctionDefinition(CFunctionDefinition node) {
            return VisitChildren(node);
        }

        // Statements
        public virtual T VisitCStatementExpr(CStatementExpr node) {
            return VisitChildren(node);
        }

        public virtual T VisitCStatementCompound(CStatementCompound node) {
            return VisitChildren(node);
        }

        public virtual T VisitCStatementIf(CStatementIf node) {
            return VisitChildren(node);
        }

        public virtual T VisitCStatementFor(CStatementFor node) {
            return VisitChildren(node);
        }

        public virtual T VisitCStatementWhile(CStatementWhile node) {
            return VisitChildren(node);
        }

        public virtual T VisitCStatementDoWhile(CStatementDoWhile node) {
            return VisitChildren(node);
        }

        public virtual T VisitCStatementSwitch(CStatementSwitch node) {
            return VisitChildren(node);
        }

        public virtual T VisitCStatementRETURN(CStatementRETURN node) {
            return VisitChildren(node);
        }

        public virtual T VisitCStatementBreak(CStatementBreak node) {
            return VisitChildren(node);
        }

        public virtual T VisitCStatementNull(CStatementNull node) {
            return VisitChildren(node);
        }

        // Expressions
        public virtual T VisitCExprAssignment(CExprAssignment node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprPlusOne(CExprPlusOne node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprMinusOne(CExprMinusOne node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprUnaryPlus(CExprUnaryPlus node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprUnaryMinus(CExprUnaryMinus node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprNot(CExprNot node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprMultiplication(CExprMultiplication node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprDivision(CExprDivision node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprAddition(CExprAddition node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprSubtraction(CExprSubtraction node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprGreaterThan(CExprGreaterThan node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprGreaterThanEqual(CExprGreaterThanEqual node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprLesserThan(CExprLesserThan node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprLesserThanEqual(CExprLesserThanEqual node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprEqual(CExprEqual node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprNotEqual(CExprNotEqual node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprAnd(CExprAnd node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprOr(CExprOr node) {
            return VisitChildren(node);
        }

        public virtual T VisitCExprFCall(CExprFCall node) {
            return VisitChildren(node);
        }

        // Terminal
        public virtual T VisitCExprCOMMENT(CExprCOMMENT node) {
            return default(T);
        }

        public virtual T VisitCExprSTRING(CExprSTRING node) {
            return default(T);
        }

        public virtual T VisitCExprINTEGER(CExprINTEGER node) {
            return default(T);
        }

        public virtual T VisitCExprVARIABLE(CExprVARIABLE node) {
            return default(T);
        }
    }
}
