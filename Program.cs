using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.IO;

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

            // MiniCASTBaseVisitor<int> dummyVisitor = new MiniCASTBaseVisitor<int>();
            // dummyVisitor.Visit(astGen.MRoot);

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
