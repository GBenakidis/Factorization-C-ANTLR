using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MiniC {
    class Program {
        static void Main(string[] args) {
            /* Passing an input from a file, as the first argument 
             * in the command line, where we should open for reading:
             * (command lines are given via string[] args) */
            StreamReader aStreamReader = new StreamReader(args[0]);

            /* Stream of characters should be specific for ANTLR */
            AntlrInputStream antlrInputStream = new AntlrInputStream(aStreamReader);

            /* Creating class for lexer */
            MiniCLexer lexer = new MiniCLexer(antlrInputStream);

            /* Lexer converts string of characters into a string of tokens */
            CommonTokenStream tokens = new CommonTokenStream(lexer);

            /* Creating parser by giving tokens as an input */
            MiniCParser parser = new MiniCParser(tokens);

            /* Starting syntax analysis from given input */
            IParseTree tree = parser.compileUnit();

            /* Printing syntax tree */
            Console.WriteLine(tree.ToStringTree());

            /* Creating visitor object for tree traversing
             * [ This prints the syntax tree (object) ] */
            STPrinterVisitor stPrinter = new STPrinterVisitor();
            stPrinter.Visit(tree);  // Traverse from the root node of the syntax tree.

            /* Printing the abstract syntax tree */
            ASTGenerator astGen = new ASTGenerator();
            astGen.Visit(tree); // Starting from the root node of the abstract syntax tree.

            // ==== Factoring part ===

            SOPFVisitor sopRunner = new SOPFVisitor();
            // Returning sum of product
            Stack<CExprAddition> sopNodes = sopRunner.Visit(tree);

            // Sum of products nodes in a file
            StreamWriter m_dotFile;
            m_dotFile = new StreamWriter("SOPRunnerPrints.dot");
            m_dotFile.WriteLine("Sum of products nodes:");
            foreach (CExprAddition sumNode in sopNodes) {
                m_dotFile.WriteLine(sumNode.M_Name);
            }
            m_dotFile.Close();
            // = = = =

            // For ONE sum of product
            // SOPCommonFactor <- Sum of Product Common Factor
            // Returning common factor of previous sum of product if it exists
            SOPCommonFactor sopCF = new SOPCommonFactor();
            CExprINTEGER cf = null;
            foreach(CExprAddition sumNode in sopNodes) {
                cf = sopCF.Visit(sumNode);
            }
            
            // Common factor in a file
            StreamWriter m_dotFile1 = new StreamWriter("SOPCommonFactorPrints.dot");
            if (cf == null) { // if no common factor found
                m_dotFile1.WriteLine("No common factor found!");
            } else { // if common factor found
                m_dotFile1.WriteLine("Our common factor is " + cf.MName);
            }
            m_dotFile1.Close();

            // = = = =

            // SOPVariableVisitor
            // Traversing sop nodes to save variables
            // Returns stack of variables
            SOPVariableVisitor svv = new SOPVariableVisitor();
            Stack<CExprVARIABLE> variableStack = null;
            foreach (CExprAddition sumNode in sopNodes) {
                variableStack = svv.Visit(sumNode);
            }

            // Variables in a file
            StreamWriter m_dotFile2 = new StreamWriter("SOPVariablePrints.dot");
            m_dotFile2.WriteLine("Variables:");
            foreach (CExprVARIABLE variableNode in variableStack) {
                m_dotFile2.WriteLine(variableNode.MName);
            }
            m_dotFile2.Close();
            // = = = =

            // FGenerator
            // Creating factored tree
            if (cf != null) { 
                FGenerator fg = new FGenerator(sopNodes, cf, variableStack);
                fg.run();
            }
            // ====

            ASTPrinterVisitor astPrinter = new ASTPrinterVisitor("ast.dot");
            astPrinter.Visit(astGen.MRoot);

            MiniC2CGeneration cGeneration = new MiniC2CGeneration();
            cGeneration.Visit(astGen.MRoot);

            String cFileName = Path.GetFileNameWithoutExtension(args[0]);
            // StreamWriter mir = new StreamWriter("mir.dot");
            // cGeneration.MTranslatedFile.PrintStructure(mir);
            StreamWriter outCFile = new StreamWriter(@"C:\Users\Giannis\source\repos\Compilers 2\Factorization\Testbench" + cFileName + ".c");
            cGeneration.MTranslatedFile.EmmitToFile(outCFile);
            outCFile.Close();
        }
    }
}
