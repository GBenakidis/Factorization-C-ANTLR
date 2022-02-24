using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniC {
    public class MiniC2CGeneration : MiniCASTBaseVisitor<CodeContainer> {
        private CCFile m_translatedFile;
        private Stack<(CEmmitableCodeContainer, CEmmitableCodeContainer, int)> m_parentsInfo = 
            new Stack<(CEmmitableCodeContainer, CEmmitableCodeContainer, int)>();
         /* ^ First argument: parent, 
          *   Second argument: function that local variables are declared, 
          *   Third Argument: parent context */

        public CCFile MTranslatedFile => m_translatedFile;

        public override CodeContainer VisitCompileUnit(CCompileUnit node) {
            m_translatedFile = new CCFile(true);

            m_translatedFile.AddCode("#include <stdio.h>\n", CCFile.mc_PREPROCESSOR);
            m_translatedFile.AddCode("#include <stdlib.h>\n", CCFile.mc_PREPROCESSOR);

            // Function definition
            //(CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = (m_translatedFile, null, CCFile.mc_FUNCTION_DEFINITION);
            //m_parentsInfo.Push(infoTuple);
            //VisitContextChildren(node, CCompileUnit.CT_FUNCTION_BODY);
            //m_parentsInfo.Pop();

            // Main
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = (m_translatedFile.MainContainer.GetChild(CMainFunctionContainer.mc_BODY), m_translatedFile.MainContainer, 
                    CCompoundContainer.mc_COMPOUNDSTATEMENT_BODY);
            m_parentsInfo.Push(infoTuple);
            VisitContextChildren(node, CCompileUnit.CT_BODY);
            m_parentsInfo.Pop();

            return null;
        }

        public override CodeContainer VisitCFunctionDefinition(CFunctionDefinition node) {
            String funName = "", funArgs = "";
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CFunctionContainer newFunction = new CFunctionContainer(infoTuple.Item1);
            infoTuple.Item1.AddCode(newFunction,infoTuple.Item3);

            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) childrenInfo =
                (newFunction, newFunction, CFunctionContainer.mc_FNAME);

            m_parentsInfo.Push(childrenInfo);
            funName += Visit(node.GetChild(CFunctionDefinition.CT_FNAME));
            m_parentsInfo.Pop();

            childrenInfo = (newFunction, newFunction, CFunctionContainer.mc_ARGS);
            m_parentsInfo.Push(childrenInfo);
            int i = 0;
            foreach (var child in node.GetContextChildren(CFunctionDefinition.CT_ARGS)) {
                if (i > 0) {
                    funArgs += ",";
                }
                funArgs += "float " + Visit(child);
                i++;
            }
            m_parentsInfo.Pop();

            m_translatedFile.DeclareFunction(funName, funArgs);

            childrenInfo = (newFunction.GetChild(CFunctionContainer.mc_BODY), newFunction, CFunctionContainer.mc_BODY);
            m_parentsInfo.Push(childrenInfo);
            Visit(node.GetChild(CFunctionDefinition.CT_BODY));
            m_parentsInfo.Pop();

            return null;
        }

        // Statements
        public override CodeContainer VisitCStatementExpr(CStatementExpr node) {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);
            infoTuple.Item1.AddCode(rep, infoTuple.Item3);

            // Translation.
            ASTElement child = node.GetChild(CStatementExpr.CT_BODY, 0);
            rep.AddCode(Visit(child));
            rep.AddCode(";");
            rep.AddNewLine();
            if (child is CExprAssignment assignment) {
                CExprVARIABLE var = child.GetChild(CExprAssignment.CT_LEFT) as CExprVARIABLE;
                rep.AddCode($"printf(\"{var.MName} = %f\\n\", {var.MName});");
            }
            return rep;
        }

        public override CodeContainer VisitCStatementCompound(CStatementCompound node) {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();
            CCompoundContainer rep;
            if (!(infoTuple.Item1 is CCompoundContainer rep1)) {
                rep = new CCompoundContainer(infoTuple.Item1);
                infoTuple.Item1.AddCode(rep, infoTuple.Item3);
            }
            else {
                rep = rep1;
            }

            // Translation.
            infoTuple = (rep, m_translatedFile.MainContainer, CCompoundContainer.mc_COMPOUNDSTATEMENT_BODY);
            m_parentsInfo.Push(infoTuple);
            foreach (var varChild in node.GetContextChildren(CStatementCompound.CT_BODY)) {
                Visit(varChild);
            }
            m_parentsInfo.Pop();
            return null;
        }

        public override CodeContainer VisitCStatementIf(CStatementIf node) {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CIfContainer rep = new CIfContainer(infoTuple.Item1);
            infoTuple.Item1.AddCode(rep, infoTuple.Item3);

            // Translation
            infoTuple = (rep, m_translatedFile.MainContainer, CStatementIf.CT_CONDITION);
            m_parentsInfo.Push(infoTuple);
            rep.AddCode(Visit(node.GetChild(CStatementIf.CT_CONDITION)), CIfContainer.mc_IF_CONDITION);
            m_parentsInfo.Pop();

            infoTuple = (rep.GetChild(CStatementIf.CT_TRUE_BODY), m_translatedFile.MainContainer, CCompoundContainer.mc_COMPOUNDSTATEMENT_BODY);
            m_parentsInfo.Push(infoTuple);
            Visit(node.GetChild(CStatementIf.CT_TRUE_BODY));
            m_parentsInfo.Pop();

            if (node.GetChildren(CStatementIf.CT_FALSE_BODY).Count() > 0) {
                infoTuple = (rep.GetChild(CStatementIf.CT_FALSE_BODY), m_translatedFile.MainContainer, CCompoundContainer.mc_COMPOUNDSTATEMENT_BODY);
                m_parentsInfo.Push(infoTuple);
                Visit(node.GetChild(CStatementIf.CT_FALSE_BODY));
                m_parentsInfo.Pop();
            }

            return null;
        }

        public override CodeContainer VisitCStatementFor(CStatementFor node) {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CForContainer rep = new CForContainer(infoTuple.Item1);
            infoTuple.Item1.AddCode(rep, infoTuple.Item3);

            // Translation
            infoTuple = (rep, m_translatedFile.MainContainer, CForContainer.mc_START);
            m_parentsInfo.Push(infoTuple);
            CodeContainer start = Visit(node.GetChild(CStatementFor.CT_START));
            rep.AddCode(start, CStatementFor.CT_START);
            m_parentsInfo.Pop();

            infoTuple = (rep, m_translatedFile.MainContainer, CForContainer.mc_FINALIZATION);
            m_parentsInfo.Push(infoTuple);
            CodeContainer fin = Visit(node.GetChild(CStatementFor.CT_CONDITION));
            rep.AddCode(fin, CStatementFor.CT_CONDITION);
            m_parentsInfo.Pop();

            infoTuple = (rep, m_translatedFile.MainContainer, CForContainer.mc_STEP);
            m_parentsInfo.Push(infoTuple);
            CodeContainer step = Visit(node.GetChild(CStatementFor.CT_LOOP));
            rep.AddCode(step, CStatementFor.CT_LOOP);
            m_parentsInfo.Pop();

            infoTuple = (rep.GetChild(CStatementFor.CT_BODY), m_translatedFile.MainContainer, CCompoundContainer.mc_COMPOUNDSTATEMENT_BODY);
            m_parentsInfo.Push(infoTuple);
            Visit(node.GetChild(CStatementFor.CT_BODY));
            m_parentsInfo.Pop();
            return null;
        }

        public override CodeContainer VisitCStatementWhile(CStatementWhile node) {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CWhileContainer rep = new CWhileContainer(infoTuple.Item1);
            infoTuple.Item1.AddCode(rep, infoTuple.Item3);

            // Translation
            infoTuple = (rep, m_translatedFile.MainContainer, CWhileContainer.mc_CONDITION);
            m_parentsInfo.Push(infoTuple);
            CodeContainer condition = Visit(node.GetChild(CStatementWhile.CT_CONDITION));
            rep.AddCode(condition, CStatementWhile.CT_CONDITION);
            m_parentsInfo.Pop();

            infoTuple = (rep.GetChild(CStatementWhile.CT_BODY), m_translatedFile.MainContainer, CCompoundContainer.mc_COMPOUNDSTATEMENT_BODY);
            m_parentsInfo.Push(infoTuple);
            Visit(node.GetChild(CStatementWhile.CT_BODY));
            m_parentsInfo.Pop();
            return null;
        }

        public override CodeContainer VisitCStatementDoWhile(CStatementDoWhile node) {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CDoWhileContainer rep = new CDoWhileContainer(infoTuple.Item1);
            infoTuple.Item1.AddCode(rep, infoTuple.Item3);

            // Translation
            infoTuple = (rep.GetChild(CStatementDoWhile.CT_BODY), m_translatedFile.MainContainer, CCompoundContainer.mc_COMPOUNDSTATEMENT_BODY);
            m_parentsInfo.Push(infoTuple);
            Visit(node.GetChild(CStatementDoWhile.CT_BODY));
            m_parentsInfo.Pop();

            infoTuple = (rep, m_translatedFile.MainContainer, CDoWhileContainer.mc_CONDITION);
            m_parentsInfo.Push(infoTuple);
            CodeContainer condition = Visit(node.GetChild(CStatementDoWhile.CT_CONDITION));
            rep.AddCode(condition, CStatementDoWhile.CT_CONDITION);
            m_parentsInfo.Pop();
            return null;
        }

        public override CodeContainer VisitCStatementRETURN(CStatementRETURN node) {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);
            infoTuple.Item1.AddCode(rep, infoTuple.Item3);

            // Translation.
            rep.AddCode("return ");
            rep.AddCode(Visit(node.GetChild(CStatementRETURN.CT_BODY)));
            rep.AddCode(";");
            rep.AddNewLine();
            return rep;
        }

        // Expressions
        public override CodeContainer VisitCExprPlusOne(CExprPlusOne node) {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode(Visit(node.GetChild(CExprPlusOne.CT_BODY, 0))); // Adds variable
            rep.AddCode("++");
            return rep;
        }

        public override CodeContainer VisitCExprMinusOne(CExprMinusOne node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode(Visit(node.GetChild(CExprMinusOne.CT_BODY, 0)));
            rep.AddCode("--");
            return rep;
        }

        public override CodeContainer VisitCExprUnaryPlus(CExprUnaryPlus node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode("+");
            rep.AddCode(Visit(node.GetChild(CExprUnaryPlus.CT_BODY, 0)));
            return rep;
        }

        public override CodeContainer VisitCExprUnaryMinus(CExprUnaryMinus node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode("-");
            rep.AddCode(Visit(node.GetChild(CExprUnaryMinus.CT_BODY, 0)));
            return rep;
        }

        public override CodeContainer VisitCExprNot(CExprNot node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode("!");
            rep.AddCode(Visit(node.GetChild(CExprNot.CT_BODY, 0)));
            return rep;
        }

        public override CodeContainer VisitCExprAddition(CExprAddition node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            // Translation.
            rep.AddCode(Visit(node.GetChild(CExprAddition.CT_LEFT, 0)));
            rep.AddCode(" + ");
            rep.AddCode(Visit(node.GetChild(CExprAddition.CT_RIGHT, 0)));
            return rep;
        }

        public override CodeContainer VisitCExprSubtraction(CExprSubtraction node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            // Translation.
            rep.AddCode(Visit(node.GetChild(CExprSubtraction.CT_LEFT, 0)));
            rep.AddCode(" - ");
            rep.AddCode(Visit(node.GetChild(CExprSubtraction.CT_RIGHT, 0)));
            return rep;
        }

        public override CodeContainer VisitCExprMultiplication(CExprMultiplication node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            // Translation.
            rep.AddCode(Visit(node.GetChild(CExprMultiplication.CT_LEFT, 0)));
            rep.AddCode(" * ");
            rep.AddCode(Visit(node.GetChild(CExprMultiplication.CT_RIGHT, 0)));
            return rep;
        }

        public override CodeContainer VisitCExprDivision(CExprDivision node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            // Translation.
            rep.AddCode(Visit(node.GetChild(CExprDivision.CT_LEFT, 0)));
            rep.AddCode(" / ");
            rep.AddCode(Visit(node.GetChild(CExprDivision.CT_RIGHT, 0)));
            return rep;
        }

        public override CodeContainer VisitCExprGreaterThan(CExprGreaterThan node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode(Visit(node.GetChild(CExprGreaterThan.CT_LEFT)));
            rep.AddCode(" > ");
            rep.AddCode(Visit(node.GetChild(CExprGreaterThan.CT_RIGHT)));

            return rep;
        }

        public override CodeContainer VisitCExprGreaterThanEqual(CExprGreaterThanEqual node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode(Visit(node.GetChild(CExprGreaterThanEqual.CT_LEFT)));
            rep.AddCode(" >= ");
            rep.AddCode(Visit(node.GetChild(CExprGreaterThanEqual.CT_RIGHT)));

            return rep;
        }

        public override CodeContainer VisitCExprLesserThan(CExprLesserThan node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode(Visit(node.GetChild(CExprLesserThan.CT_LEFT)));
            rep.AddCode(" < ");
            rep.AddCode(Visit(node.GetChild(CExprLesserThan.CT_RIGHT)));

            return rep;
        }

        public override CodeContainer VisitCExprLesserThanEqual(CExprLesserThanEqual node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode(Visit(node.GetChild(CExprLesserThanEqual.CT_LEFT)));
            rep.AddCode(" <= ");
            rep.AddCode(Visit(node.GetChild(CExprLesserThanEqual.CT_RIGHT)));

            return rep;
        }

        public override CodeContainer VisitCExprEqual(CExprEqual node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode(Visit(node.GetChild(CExprEqual.CT_LEFT)));
            rep.AddCode(" == ");
            rep.AddCode(Visit(node.GetChild(CExprEqual.CT_RIGHT)));

            return rep;
        }

        public override CodeContainer VisitCExprNotEqual(CExprNotEqual node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode(Visit(node.GetChild(CExprNotEqual.CT_LEFT)));
            rep.AddCode(" != ");
            rep.AddCode(Visit(node.GetChild(CExprNotEqual.CT_RIGHT)));

            return rep;
        }

        public override CodeContainer VisitCExprAnd(CExprAnd node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode(Visit(node.GetChild(CExprAnd.CT_LEFT)));
            rep.AddCode(" && ");
            rep.AddCode(Visit(node.GetChild(CExprAnd.CT_RIGHT)));

            return rep;
        }

        public override CodeContainer VisitCExprOr(CExprOr node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode(Visit(node.GetChild(CExprOr.CT_LEFT)));
            rep.AddCode(" || ");
            rep.AddCode(Visit(node.GetChild(CExprOr.CT_RIGHT)));

            return rep;
        }

        public override CodeContainer VisitCExprFCall(CExprFCall node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, infoTuple.Item1);

            rep.AddCode(Visit(node.GetChild(CExprFCall.CT_FNAME)));
            rep.AddCode("(");
            int i = 0;
            foreach (var child in node.GetContextChildren(CExprFCall.CT_ARGS))
            {
                if (i > 0)
                {
                    rep.AddCode(",");
                }
                rep.AddCode(Visit(child));
                i++;
            }
            rep.AddCode(")");

            return rep;
        }

        public override CodeContainer VisitCExprAssignment(CExprAssignment node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CFunctionContainer fun = infoTuple.Item2 as CFunctionContainer;

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, null);

            // Translation.
            CExprVARIABLE id = node.GetChild(CExprAssignment.CT_LEFT, 0) as CExprVARIABLE;
            fun.DeclareVariable(id.MName, false);
            rep.AddCode(id.MName);
            rep.AddCode(" = ");
            rep.AddCode(Visit(node.GetChild(CExprAssignment.CT_RIGHT, 0)));
            return rep;
        }

        // Terminal
        public override CodeContainer VisitCExprCOMMENT(CExprCOMMENT node)
        {
            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, null);

            // Translation.
            rep.AddCode(node.MName);
            return rep;
        }

        public override CodeContainer VisitCExprSTRING(CExprSTRING node)
        {
            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, null);

            // Translation.
            rep.AddCode(node.MName);
            return rep;
        }

        public override CodeContainer VisitCExprINTEGER(CExprINTEGER node)
        {
            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, null);

            // Translation.
            rep.AddCode(node.MName);
            return rep;
        }

        public override CodeContainer VisitCExprVARIABLE(CExprVARIABLE node)
        {
            (CEmmitableCodeContainer, CEmmitableCodeContainer, int) infoTuple = m_parentsInfo.Peek();

            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, null);

            if (infoTuple.Item1 is CFunctionContainer && 
                (infoTuple.Item3 == CFunctionContainer.mc_ARGS || infoTuple.Item3 == CFunctionContainer.mc_FNAME))
            {
                infoTuple.Item1.AddCode(rep, infoTuple.Item3);
            }

            // Translation.
            rep.AddCode(node.MName);
            return rep;
        }

    }
}
