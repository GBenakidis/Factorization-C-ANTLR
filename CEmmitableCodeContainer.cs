using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MiniC {
    public enum CodeContainerType {
        CT_NA,
        CT_CODEREPOSITORY,
        CT_FILE,
        CT_FUNCTION_DEFINITION,
        CT_COMPOUNDCONTAINER,
        CT_IFCONTAINER,
        CT_FORCONTAINER,
        CT_WHILECONTAINER,
        CT_DOWHILECONTAINER
    }

    public abstract class CEmmitableCodeContainer {
        private CodeContainerType m_containerType;
        protected CEmmitableCodeContainer m_parent;
        private int m_serialNumber;
        private static int m_serialNumberCounter = 0;
        private string m_label;
        protected int m_nestingLevel = 0;

        // Properties
        public CodeContainerType MContainerType => m_containerType;
        public CEmmitableCodeContainer MParent => m_parent;
        public int MSerialNumber => m_serialNumber;
        public static int MSerialNumberCounter => m_serialNumberCounter;
        public string MLabel => m_label;

        protected int MNestingLevel {
            get => m_nestingLevel;
            set => m_nestingLevel = value;
        }

        protected CEmmitableCodeContainer(CodeContainerType mContainerType, CEmmitableCodeContainer mParent) {
            m_containerType = mContainerType;
            m_parent = mParent;
            m_serialNumber = m_serialNumberCounter++;
            m_label = m_containerType + "_" + m_serialNumber;
            m_nestingLevel = m_parent ?. MNestingLevel ?? 0;
        }

        public void SetParent(CEmmitableCodeContainer parent) {
            m_parent = parent;
        }

        public abstract CodeContainer AssemblyCodeContainer();
        public abstract void AddCode(string code, int context = 0);
        public abstract void AddCode(CEmmitableCodeContainer code, int context = 0);
        public abstract void PrintStructure(StreamWriter m_ostream);
        public abstract string EmmitStdout();
        public abstract void EmmitToFile(StreamWriter f);
        public virtual void EnterScope() {
            m_nestingLevel++;
        }

        public virtual void LeaveScope() {
            if (m_nestingLevel > 0) {
                m_nestingLevel--;
            }
            else {
                throw new Exception("Non-matched nesting");
            }
        }
        public abstract void AddNewLine(int context = 0);
    }

    public abstract class CComboContainer : CEmmitableCodeContainer {
        protected List<CEmmitableCodeContainer>[] m_repository;
        private static int m_clusterSerial = 0;

        protected CComboContainer(CodeContainerType nodeType, CEmmitableCodeContainer parent, int numcontexts) : base(nodeType, parent) {
            m_repository = new List<CEmmitableCodeContainer>[numcontexts];
            for (int i = 0; i < numcontexts; i++) {
                m_repository[i] = new List<CEmmitableCodeContainer>();
            }
        }

        protected virtual CodeContainer AssemblyContext(int ct) {
            CodeContainer rep = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, this);
            for (int i = 0; i < m_repository[ct].Count; i++) {
                rep.AddCode(m_repository[ct][i].AssemblyCodeContainer());
            }
            return rep;
        }

        public override void AddCode(string code, int context) {
            CodeContainer container = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, this);
            container.AddCode(code, 0);
            m_repository[context].Add(container);
        }

        public override void AddCode(CEmmitableCodeContainer code, int context) {
            m_repository[context].Add(code);
            code.SetParent(this);
        }

        public override void AddNewLine(int context) {
            CodeContainer container = new CodeContainer(CodeContainerType.CT_CODEREPOSITORY, this);
            container.AddNewLine();
            m_repository[context].Add(container);
        }

        public override string EmmitStdout() {
            Console.WriteLine(AssemblyCodeContainer().ToString());
            return AssemblyCodeContainer().ToString();
        }

        public override string ToString() {
            return AssemblyCodeContainer().ToString();
        }

        public override void EmmitToFile(StreamWriter f) {
            f.WriteLine(AssemblyCodeContainer().ToString());
        }

        internal CEmmitableCodeContainer GetChild(int ct, int index = 0) {
            return m_repository[ct][index];
        }

        internal CEmmitableCodeContainer[] GetContextChildren(int ct) {
            return m_repository[ct].ToArray();
        }

        protected void ExtractSubgraphs(StreamWriter m_ostream, int context,string contextname) {
            if (m_repository[context].Count != 0) {
                m_ostream.WriteLine("\tsubgraph cluster" + m_clusterSerial++ + "{");
                m_ostream.WriteLine("\t\tnode [style=filled,color=white];");
                m_ostream.WriteLine("\t\tstyle=filled;");
                m_ostream.WriteLine("\t\tcolor=lightgrey;");
                m_ostream.Write("\t\t");
                for (int i = 0; i < m_repository[context].Count; i++) {
                    m_ostream.Write(m_repository[context][i].MLabel + ";");
                }

                m_ostream.WriteLine("\n\t\tlabel=" + contextname + ";");
                m_ostream.WriteLine("\t}");
            }
        }
    }
    
    public class CodeContainer : CEmmitableCodeContainer {
        StringBuilder m_repository = new StringBuilder();
        public CodeContainer(CodeContainerType mContainerType, CEmmitableCodeContainer mParent) :
            base(mContainerType, mParent)
        { }

        public override void AddCode(string code, int context = 0) {
            string[] lines = code.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines) {
                m_repository.Append(line);
                if (code.Contains('\n')) {
                    m_repository.Append("\r\n");
                    m_repository.Append(new string('\t', m_nestingLevel));
                }
            }
        }

        public override void AddCode(CEmmitableCodeContainer code, int context = 0) {
            AddCode(code.ToString(), context);
        }

        public override void AddNewLine(int context = 0) {
            m_repository.Append("\r\n");
            m_repository.Append(new string('\t', m_nestingLevel));
        }

        public override void EnterScope() {
            base.EnterScope();
            AddNewLine();
        }

        public override void LeaveScope() {
            base.LeaveScope();
            AddNewLine();
        }

        public override string EmmitStdout() {
            System.Console.WriteLine(m_repository.ToString());
            return m_repository.ToString();
        }

        public override void EmmitToFile(StreamWriter f) {
            f.WriteLine(m_repository.ToString());
        }

        public override string ToString() {
            return m_repository.ToString();
        }

        public override CodeContainer AssemblyCodeContainer() {
            return this;
        }

        public override void PrintStructure(StreamWriter m_ostream) {
            m_ostream.WriteLine("\"{0}\"->\"{1}\"", MParent.MLabel, MLabel);
        }
    }
}