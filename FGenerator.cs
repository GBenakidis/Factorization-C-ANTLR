using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniC {
    class FGenerator : MiniCASTBaseVisitor<int> {
        private String m_dotFilename;
        private StreamWriter m_dotFile;
        private ASTElement m_root;
        private static int ms_clusterSerial = 0;

        private Stack<CExprAddition> fsopNodes;
        private CExprINTEGER fcf;
        private Stack<CExprVARIABLE> fvariableStack;

        public FGenerator(Stack<CExprAddition> sopNodes, CExprINTEGER cf, Stack<CExprVARIABLE> variableStack) {
            fsopNodes = sopNodes;
            fcf = cf;
            fvariableStack = variableStack;
            m_dotFilename = "sop.dot";
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

        public void run() {
            // Open dotFile
            m_dotFile = new StreamWriter(m_dotFilename);

            m_dotFile.WriteLine("digraph G{");
            CExprMultiplication multNode = new CExprMultiplication();
            //Generate contexts
            //ExtractSubgraphs(multNode, CExprMultiplication.CT_LEFT, CExprMultiplication.msc_contextNames);
            //ExtractSubgraphs(multNode, CExprMultiplication.CT_RIGHT, CExprMultiplication.msc_contextNames);

            m_dotFile.WriteLine($"{multNode.M_Name}->{fcf.MName};");

            CExprAddition sn = null;
            foreach (CExprAddition sopNode in fsopNodes) {
                m_dotFile.WriteLine($"{multNode.M_Name}->{sopNode.M_Name};");
                sn = sopNode;
                //Generate contexts
                //ExtractSubgraphs(sopNode, CExprAddition.CT_LEFT, CExprAddition.msc_contextNames);
                //ExtractSubgraphs(sopNode, CExprAddition.CT_RIGHT, CExprAddition.msc_contextNames);
            }

            foreach (CExprVARIABLE varNode in fvariableStack) {
                m_dotFile.WriteLine($"{sn.M_Name}->{varNode.MName};");
            }

            // Close dotFile
            m_dotFile.WriteLine("}");
            m_dotFile.Close();

            // Call graphviz to print tree
            // Prepare the process dot to run
            ProcessStartInfo start = new ProcessStartInfo();
            //Enter, in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = "-Tgif " +
                              Path.GetFileName("sop.dot") + " -o " +
                              Path.GetFileNameWithoutExtension("sop") + ".gif";
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
        }
    }
}
