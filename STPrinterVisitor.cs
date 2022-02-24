using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MiniC {
    public class STPrinterVisitor : MiniCBaseVisitor<int> {
        /* StreamWriter object manages the file where, the desciption of the syntax tree
         * to graphviz, will be exported. */
        StreamWriter m_STSpecFile = new StreamWriter("tree.dot");
        Stack<string> m_parentsLabel = new Stack<string>();
        private static int ms_serialCounter = 0;

        public override int VisitCompileUnit([NotNull] MiniCParser.CompileUnitContext context) {
            string label = "CompileUnit" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("digraph G{");
            m_parentsLabel.Push(label);
            base.VisitCompileUnit(context); // Visiting children
            m_parentsLabel.Pop();
            m_STSpecFile.WriteLine("}");
            m_STSpecFile.Close();

            /* Prepare the process dot to run */
            ProcessStartInfo start = new ProcessStartInfo();
            /* Enter in the command line arguments, everything you would enter after
             * the executable name itself */
            start.Arguments = "-Tgif " + 
                              Path.GetFileName("tree.dot") + " -o " +
                              Path.GetFileNameWithoutExtension("tree") + ".gif";
            /* Enter the executable to run, including the complete path */
            start.FileName = "dot";
            /* Showing a console window */
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;

            /* Run the external process & wait for it to finish */
            using (Process proc = Process.Start(start)) {
                proc.WaitForExit();
                /* Retrieve the app's exit process */
                exitCode = proc.ExitCode;
            }
            return 0;
        }

        public override int VisitFunctionDefinition([NotNull] MiniCParser.FunctionDefinitionContext context) {
            string label = "FunctionDef" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label); // Printing links
            m_parentsLabel.Push(label);
            base.VisitFunction_definition(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitFargs([NotNull] MiniCParser.FargsContext context) {
            string label = "FArgs" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitFargs(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitArgs([NotNull] MiniCParser.ArgsContext context) {
            string label = "Args" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitArgs(context);
            m_parentsLabel.Pop();
            return 0;
        }

        // Statements
        public override int VisitStatementExpr([NotNull] MiniCParser.StatementExprContext context) {
            string label = "ExpressionStatement" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitStatementExpr(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitStatementIf([NotNull] MiniCParser.StatementIfContext context) {
            string label = "IfStatement" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitStatementIf(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitStatementFor([NotNull] MiniCParser.StatementForContext context) {
            string label = "ForStatement" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitStatementFor(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitStatementWhile([NotNull] MiniCParser.StatementWhileContext context) {
            string label = "WhileStatement" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitStatementWhile(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitStatementDoWhile([NotNull] MiniCParser.StatementDoWhileContext context) {
            string label = "DoWhileStatement" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitStatementDoWhile(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitStatementSwitch(MiniCParser.StatementSwitchContext context) {
            string label = "SwitchStatement" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitStatementSwitch(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitStatementRETURN([NotNull] MiniCParser.StatementRETURNContext context) {
            string label = "RETURN" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitStatementRETURN(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitStatementBREAK([NotNull] MiniCParser.StatementBREAKContext context) {
            string label = "BREAK" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitStatementBREAK(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitStatementNULL([NotNull] MiniCParser.StatementNULLContext context) {
            string label = "NULL" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitStatementNULL(context);
            m_parentsLabel.Pop();
            return 0;
        }

        // Expressions
        public override int VisitExprAssignment([NotNull] MiniCParser.ExprAssignmentContext context) {
            string label = "Assignment" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitExprAssignment(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitExprPlusOne([NotNull] MiniCParser.ExprPlusOneContext context) {
            string label = "PlusOne" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitExprPlusOne(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitExprMinusOne([NotNull] MiniCParser.ExprMinusOneContext context) {
            string label = "MinusOne" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitExprMinusOne(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitExprUnaryPlus([NotNull] MiniCParser.ExprUnaryPlusContext context) {
            string label = "PlusOne" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitExprUnaryPlus(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitExprUnaryMinus([NotNull] MiniCParser.ExprUnaryMinusContext context) {
            string label = "MinusOne" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitExprUnaryMinus(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitExprNot([NotNull] MiniCParser.ExprNotContext context) {
            string label = "MinusOne" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitExprNot(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitExprMulDiv([NotNull] MiniCParser.ExprMulDivContext context) {
            string label = "";
            switch (context.op.Type) {
                case MiniCLexer.MULTI:
                    label = "Multiplication" + "_" + ms_serialCounter++ + "|  " + context.GetText();
                    break;
                case MiniCLexer.DIV:
                    label = "Division" + "_" + ms_serialCounter++ + "|  " + context.GetText();
                    break;
            }
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitExprMulDiv(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitExprAddSub([NotNull] MiniCParser.ExprAddSubContext context) {
            string label = "";
            switch (context.op.Type) {
                case MiniCLexer.PLUS:
                    label = "Addition" + "_" + ms_serialCounter++ + "|  " + context.GetText();
                    break;
                case MiniCLexer.MINUS:
                    label = "Subtraction" + "_" + ms_serialCounter++ + "|  " + context.GetText();
                    break;
            }
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitExprAddSub(context);
            m_parentsLabel.Pop();
            return 0;
        }
        
        public override int VisitExprParenthesis([NotNull] MiniCParser.ExprParenthesisContext context) {
            return base.VisitExprParenthesis(context);
        }

        public override int VisitExprRelationalOperators([NotNull] MiniCParser.ExprRelationalOperatorsContext context) {
            string label = "";
            switch (context.op.Type) {
                case MiniCLexer.GT:
                    label = "GT" + "_" + ms_serialCounter++;
                    break;
                case MiniCLexer.GTE:
                    label = "GTE" + "_" + ms_serialCounter++;
                    break;
                case MiniCLexer.LT:
                    label = "LT" + "_" + ms_serialCounter++;
                    break;
                case MiniCLexer.LTE:
                    label = "LTE" + "_" + ms_serialCounter++;
                    break;
            }
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitExprRelationalOperators(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitExprComparison([NotNull] MiniCParser.ExprComparisonContext context) {
            string label = "";
            switch (context.op.Type) {
                case MiniCLexer.EQUAL:
                    label = "EQUAL" + "_" + ms_serialCounter++;
                    break;
                case MiniCLexer.NEQUAL:
                    label = "NEQUAL" + "_" + ms_serialCounter++;
                    break;
            }
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitExprComparison(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitExprAnd([NotNull] MiniCParser.ExprAndContext context) {
            string label = "And" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitExprAnd(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitExprOr([NotNull] MiniCParser.ExprOrContext context) {
            string label = "Or" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitExprOr(context);
            m_parentsLabel.Pop();
            return 0;
        }

        public override int VisitExprFCall([NotNull] MiniCParser.ExprFCallContext context) {
            string label = "FCall" + "_" + ms_serialCounter++;
            m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
            m_parentsLabel.Push(label);
            base.VisitExprFCall(context);
            m_parentsLabel.Pop();
            return 0;
        }

        // Terminal
        public override int VisitTerminal([NotNull] ITerminalNode node) {
            string label = "";
            switch (node.Symbol.Type) {
                case MiniCLexer.COMMENT:
                    label = "COMMENT" + "_" + ms_serialCounter++ + "|  " + node.GetText();
                    m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
                    break;
                case MiniCLexer.STRING:
                    label = "STRING" + "_" + ms_serialCounter++ + "|  " + node.GetText();
                    m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
                    break;
                case MiniCLexer.INTEGER:
                    label = "INTEGER" + "_" + ms_serialCounter++ + "|  " + node.GetText();
                    m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
                    return 0;
                case MiniCLexer.VARIABLE:
                    label = "VARIABLE" + "_" + ms_serialCounter++ + "|  " + node.GetText();
                    m_STSpecFile.WriteLine("\"{0}\"->\"{1}\";", m_parentsLabel.Peek(), label);
                    return 0;
            }
            return base.VisitTerminal(node);
        }
    }
}
