using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MiniC {
    class ASTPrinterVisitor : MiniCASTBaseVisitor<int> {
        private Stack<CExprMultiplication> numMulti = new Stack<CExprMultiplication>();
        private String m_dotFilename;
        private StreamWriter m_dotFile;
        private ASTElement m_root;
        private static int ms_clusterSerial = 0;

        public ASTPrinterVisitor(string dotFilename) {
            m_dotFilename = dotFilename;
            m_root = null;
            m_dotFile = null;
        }

        public void ExtractSubgraphs(ASTElement element, int context, string[] contextNames) {
            m_dotFile.WriteLine($"subgraph cluster{ms_clusterSerial++} {{");
            m_dotFile.WriteLine("node [style=filled,color=white];");
            m_dotFile.WriteLine("style=filled;");
            m_dotFile.WriteLine("color=lightgrey;");

            foreach (ASTElement c in element.GetChildren(context)) {
                m_dotFile.Write($"{c.M_Name};");
            }
            m_dotFile.WriteLine("");
            m_dotFile.WriteLine($"label = \"{contextNames[context]}\";");
            m_dotFile.WriteLine("}");
        }

        public override int VisitCompileUnit(CCompileUnit node) {
            // Open dotFile
            m_dotFile = new StreamWriter(m_dotFilename);

            m_dotFile.WriteLine("digraph G{");

            // Generate contexts
            ExtractSubgraphs(node, CCompileUnit.CT_BODY, CCompileUnit.msc_contextNames);
            ExtractSubgraphs(node, CCompileUnit.CT_FUNCTION_BODY, CCompileUnit.msc_contextNames);

            // Visit contexts
            base.VisitCompileUnit(node);

            // Close dotFile
            m_dotFile.WriteLine("}");
            m_dotFile.Close();

            // Call graphviz to print tree
            // Prepare the process dot to run
            ProcessStartInfo start = new ProcessStartInfo();
            //Enter, in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = "-Tgif " +
                              Path.GetFileName("ast.dot") + " -o " +
                              Path.GetFileNameWithoutExtension("ast") + ".gif";
            // Enter the executable to run , including the complete path
            start.FileName = "dot";
            // Do you want to show the console window?
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;

            // Run the external process and wait for it to finish
            using (Process proc = Process.Start(start)) {
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;
            }
            return 0;
        }

        public override int VisitCFunctionDefinition(CFunctionDefinition node) {
            // Generate edge with parent.
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            // Generate contexts
            ExtractSubgraphs(node, CFunctionDefinition.CT_FNAME, CFunctionDefinition.msc_contextNames);
            ExtractSubgraphs(node, CFunctionDefinition.CT_ARGS, CFunctionDefinition.msc_contextNames);
            ExtractSubgraphs(node, CFunctionDefinition.CT_BODY, CFunctionDefinition.msc_contextNames);

            // Visit Assignment.
            base.VisitCFunctionDefinition(node);
            return 0;
        }

        // Statements.
        public override int VisitCStatementExpr(CStatementExpr node) {
            // Generate edge with parent.
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            // Generate contexts
            ExtractSubgraphs(node, CStatementExpr.CT_BODY, CStatementExpr.msc_contextNames);

            // Visit Assignment.
            base.VisitCStatementExpr(node);
            return 0;
        }

        public override int VisitCStatementIf(CStatementIf node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CStatementIf.CT_CONDITION, CStatementIf.msc_contextNames);
            ExtractSubgraphs(node, CStatementIf.CT_TRUE_BODY, CStatementIf.msc_contextNames);
            ExtractSubgraphs(node, CStatementIf.CT_FALSE_BODY, CStatementIf.msc_contextNames);

            //Visit Assignment
            base.VisitCStatementIf(node);
            return 0;
        }

        public override int VisitCStatementCompound(CStatementCompound node) {
            // Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            // Generate contexts
            ExtractSubgraphs(node, CStatementCompound.CT_BODY, CStatementCompound.msc_contextNames);

            // Visit Assignment
            base.VisitCStatementCompound(node);
            return 0;
        }

        public override int VisitCStatementFor(CStatementFor node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CStatementFor.CT_START, CStatementFor.msc_contextNames);
            ExtractSubgraphs(node, CStatementFor.CT_CONDITION, CStatementFor.msc_contextNames);
            ExtractSubgraphs(node, CStatementFor.CT_LOOP, CStatementFor.msc_contextNames);
            ExtractSubgraphs(node, CStatementFor.CT_BODY, CStatementFor.msc_contextNames);

            //Visit Assignment
            base.VisitCStatementFor(node);
            return 0;
        }

        public override int VisitCStatementWhile(CStatementWhile node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CStatementWhile.CT_CONDITION, CStatementWhile.msc_contextNames);
            ExtractSubgraphs(node, CStatementWhile.CT_BODY, CStatementWhile.msc_contextNames);
            
            //Visit Assignment
            base.VisitCStatementWhile(node);
            return 0;
        }

        public override int VisitCStatementDoWhile(CStatementDoWhile node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CStatementDoWhile.CT_BODY, CStatementDoWhile.msc_contextNames);
            ExtractSubgraphs(node, CStatementDoWhile.CT_CONDITION, CStatementDoWhile.msc_contextNames);
            
            //Visit Assignment
            base.VisitCStatementDoWhile(node);
            return 0;
        }

        public override int VisitCStatementRETURN(CStatementRETURN node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CStatementRETURN.CT_BODY, CStatementRETURN.msc_contextNames);

            //Visit Assignment
            base.VisitCStatementRETURN(node);
            return 0;
        }

        public override int VisitCStatementBreak(CStatementBreak node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CStatementBreak.CT_BODY, CStatementBreak.msc_contextNames);

            //Visit Assignment
            base.VisitCStatementBreak(node);
            return 0;
        }

        public override int VisitCStatementNull(CStatementNull node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");
            return 0;
        }

        // Expressions
        public override int VisitCExprAssignment(CExprAssignment node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprAssignment.CT_LEFT, CExprAssignment.msc_contextNames);
            ExtractSubgraphs(node, CExprAssignment.CT_RIGHT, CExprAssignment.msc_contextNames);

            //Visit Assignment
            base.VisitCExprAssignment(node);
            return 0;
        }

        public override int VisitCExprPlusOne(CExprPlusOne node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprPlusOne.CT_BODY, CExprPlusOne.msc_contextNames);

            //Visit Assignment
            base.VisitCExprPlusOne(node);
            return 0;
        }

        public override int VisitCExprMinusOne(CExprMinusOne node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprMinusOne.CT_BODY, CExprMinusOne.msc_contextNames);

            //Visit Assignment
            base.VisitCExprMinusOne(node);
            return 0;
        }

        public override int VisitCExprUnaryPlus(CExprUnaryPlus node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprUnaryPlus.CT_BODY, CExprUnaryPlus.msc_contextNames);

            //Visit Assignment
            base.VisitCExprUnaryPlus(node);
            return 0;
        }

        public override int VisitCExprUnaryMinus(CExprUnaryMinus node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprUnaryMinus.CT_BODY, CExprUnaryMinus.msc_contextNames);

            //Visit Assignment
            base.VisitCExprUnaryMinus(node);
            return 0;
        }

        public override int VisitCExprNot(CExprNot node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprNot.CT_BODY, CExprNot.msc_contextNames);

            //Visit Assignment
            base.VisitCExprNot(node);
            return 0;
        }

        public override int VisitCExprMultiplication(CExprMultiplication node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");
            //Generate contexts
            ExtractSubgraphs(node, CExprMultiplication.CT_LEFT, CExprMultiplication.msc_contextNames);
            ExtractSubgraphs(node, CExprMultiplication.CT_RIGHT, CExprMultiplication.msc_contextNames);

            //Visit Assignment
            base.VisitCExprMultiplication(node);
            return 0;
        }

        public override int VisitCExprDivision(CExprDivision node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprDivision.CT_LEFT, CExprDivision.msc_contextNames);
            ExtractSubgraphs(node, CExprDivision.CT_RIGHT, CExprDivision.msc_contextNames);

            //Visit Assignment
            base.VisitCExprDivision(node);
            return 0;
        }

        public override int VisitCExprAddition(CExprAddition node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");
            //Generate contexts
            ExtractSubgraphs(node, CExprAddition.CT_LEFT, CExprAddition.msc_contextNames);
            ExtractSubgraphs(node, CExprAddition.CT_RIGHT, CExprAddition.msc_contextNames);

            //Visit Assignment
            base.VisitCExprAddition(node);
            return 0;
        }

        public override int VisitCExprSubtraction(CExprSubtraction node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprSubtraction.CT_LEFT, CExprSubtraction.msc_contextNames);
            ExtractSubgraphs(node, CExprSubtraction.CT_RIGHT, CExprSubtraction.msc_contextNames);

            //Visit Assignment
            base.VisitCExprSubtraction(node);
            return 0;
        }

        public override int VisitCExprGreaterThan(CExprGreaterThan node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprGreaterThan.CT_LEFT, CExprGreaterThan.msc_contextNames);
            ExtractSubgraphs(node, CExprGreaterThan.CT_RIGHT, CExprGreaterThan.msc_contextNames);

            //Visit Assignment
            base.VisitCExprGreaterThan(node);
            return 0;
        }

        public override int VisitCExprGreaterThanEqual(CExprGreaterThanEqual node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprGreaterThanEqual.CT_LEFT, CExprGreaterThanEqual.msc_contextNames);
            ExtractSubgraphs(node, CExprGreaterThanEqual.CT_RIGHT, CExprGreaterThanEqual.msc_contextNames);

            //Visit Assignment
            base.VisitCExprGreaterThanEqual(node);
            return 0;
        }

        public override int VisitCExprLesserThan(CExprLesserThan node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprLesserThan.CT_LEFT, CExprLesserThan.msc_contextNames);
            ExtractSubgraphs(node, CExprLesserThan.CT_RIGHT, CExprLesserThan.msc_contextNames);

            //Visit Assignment
            base.VisitCExprLesserThan(node);
            return 0;
        }

        public override int VisitCExprLesserThanEqual(CExprLesserThanEqual node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprLesserThanEqual.CT_LEFT, CExprLesserThanEqual.msc_contextNames);
            ExtractSubgraphs(node, CExprLesserThanEqual.CT_RIGHT, CExprLesserThanEqual.msc_contextNames);

            //Visit Assignment
            base.VisitCExprLesserThanEqual(node);
            return 0;
        }

        public override int VisitCExprEqual(CExprEqual node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprEqual.CT_LEFT, CExprEqual.msc_contextNames);
            ExtractSubgraphs(node, CExprEqual.CT_RIGHT, CExprEqual.msc_contextNames);

            //Visit Assignment
            base.VisitCExprEqual(node);
            return 0;
        }

        public override int VisitCExprNotEqual(CExprNotEqual node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprNotEqual.CT_LEFT, CExprNotEqual.msc_contextNames);
            ExtractSubgraphs(node, CExprNotEqual.CT_RIGHT, CExprNotEqual.msc_contextNames);

            //Visit Assignment
            base.VisitCExprNotEqual(node);
            return 0;
        }

        public override int VisitCExprAnd(CExprAnd node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprAnd.CT_LEFT, CExprAnd.msc_contextNames);
            ExtractSubgraphs(node, CExprAnd.CT_RIGHT, CExprAnd.msc_contextNames);

            //Visit Assignment
            base.VisitCExprAnd(node);
            return 0;
        }

        public override int VisitCExprOr(CExprOr node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprOr.CT_LEFT, CExprOr.msc_contextNames);
            ExtractSubgraphs(node, CExprOr.CT_RIGHT, CExprOr.msc_contextNames);

            //Visit Assignment
            base.VisitCExprOr(node);
            return 0;
        }

        public override int VisitCExprFCall(CExprFCall node) {
            //Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");

            //Generate contexts
            ExtractSubgraphs(node, CExprFCall.CT_FNAME, CExprFCall.msc_contextNames);
            ExtractSubgraphs(node, CExprFCall.CT_ARGS, CExprFCall.msc_contextNames);

            //Visit Assignment
            base.VisitCExprFCall(node);
            return 0;
        }

        public override int VisitCExprCOMMENT(CExprCOMMENT node) {
            // Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");
            return 0;
        }

        public override int VisitCExprSTRING(CExprSTRING node) {
            // Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");
            return 0;
        }

        public override int VisitCExprINTEGER(CExprINTEGER node) {
            // Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name};");
            return 0;
        }

        public override int VisitCExprVARIABLE(CExprVARIABLE node) {
            // Generate edge with parent
            m_dotFile.WriteLine($"{node.MParent.M_Name}->{node.M_Name}");
            return 0;
        }
    }
}
