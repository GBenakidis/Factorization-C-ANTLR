using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniC {
    public class ASTElementChildrenEmulator : IEnumerator<ASTElement> {
        private int m_currentContext;
        private int m_currentChildIndex;
        private ASTElement m_currentChild;
        private ASTElement m_currentNode;

        public ASTElement Current => m_currentChild;

        public ASTElementChildrenEmulator(ASTElement mCurrentNode) { // Take a node.
            m_currentNode = mCurrentNode;
        }

        public void Dispose() { }

        public bool MoveNext() {
            m_currentChildIndex++;
            if (m_currentChildIndex == m_currentNode.GetContextChildrenNumber(m_currentContext)) { // Checking if we are at the end of a child.
                if (m_currentContext == m_currentNode.GetContextNumber()) { // Checking if we are at a leaf.
                    return false; // We reached the end.
                }
                else {
                    m_currentContext++;
                    while (m_currentNode.GetContextChildrenNumber(m_currentContext) != 0 && m_currentContext < m_currentNode.GetContextNumber()) {
                        m_currentContext++;
                    }

                    if (m_currentContext == m_currentNode.GetContextNumber()) { // Checking if we are at a leaf.
                        return false; // We reached the end.
                    }
                    else {
                        m_currentChildIndex = 0;
                        m_currentChild = m_currentNode.GetChild(m_currentContext, m_currentChildIndex);
                        return true;
                    }
                }
            }
            else {
                m_currentChild = m_currentNode.GetChild(m_currentContext, m_currentChildIndex);
                return true;
            }
        }

        public void Reset() { // Initiating the variables.
            m_currentContext = -1;
            m_currentChildIndex = -1;
            m_currentChild = null;
        }

        object IEnumerator.Current => Current;
    }

    public enum NodeType {
        NT_NA,
        NT_COMPILEUNIT, NT_FUNCTIONDEFINITION,
        NT_STATEMENTEXPR, NT_STATEMENTCOMPOUND,
        NT_STATEMENTRETURN, NT_STATEMENTBREAK, NT_STATEMENTNULL,
        NT_STATEMENTIF, NT_STATEMENTFOR, NT_STATEMENTWHILE, NT_STATEMENTDOWHILE,
        NT_STATEMENTSWITCH, NT_STATEMENTSWITCHCASE, NT_STATEMENTSWITCHDEFAULT,
        NT_COMMENT, NT_STRING, NT_INTEGER, NT_VARIABLE,
        NT_ASSIGNMENT,
        NT_PLUSONE,
        NT_MINUSONE,
        NT_UNARYPLUS,
        NT_UNARYMINUS,
        NT_NOT,
        NT_MULTIPLICATION, NT_DIVISION,
        NT_ADDITION, NT_SUBTRACTION,
        NT_GREATERTHAN, NT_GREATERTHANEQUAL, NT_LESSERTHAN, NT_LESSERTHANEQUAL,
        NT_EQUAL, NT_NOTEQUAL,
        NT_AND,
        NT_OR,
        NT_FCALL
    }

    public abstract class ASTElement : IEnumerable<ASTElement> {
        private List<ASTElement>[] m_children;  // References to the children of the node.
        private ASTElement m_parent;            // References to the parent of the node.
        private int m_serial;                   // Node's ID.
        private string m_name;                  // Node's name.
        private static int m_serialCounter = 0; // Node's ID
        private NodeType m_type;

        public NodeType Type {
            get => m_type;
        }

        public ASTElement MParent {
            get => m_parent;
            set => m_parent = value;
        }

        // So that we can access the parent and name of the node.
        public string M_Name => m_name;

        public IEnumerator<ASTElement> GetEnumerator() {
            return new ASTElementChildrenEmulator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /* "ASTBaseVisitor<T> visitor" corresponds to Visitor where we want
         * to run Visit class for the edit of the current node. */
        public abstract T Accept<T>(ASTBaseVisitor<T> visitor);

        /* Protected because our class is already abstract, so we can't create
         * objects of it */
        protected ASTElement(int context, NodeType type) {  // Constructor
            m_serial = m_serialCounter++;
            m_type = type;
            m_name = GenerateNodeName();
            if (context != 0) { // 0 = the node has no children so don't initialize the m_children.
                m_children = new List<ASTElement>[context]; // If the node has children, we make room for them in the list .
                for (int i = 0; i < context; i++) {
                    m_children[i] = new List<ASTElement>();
                }
            }
        }

        public void AddChild(ASTElement child, int contextIndex) {
            m_children[contextIndex].Add(child);    // Adding child to that specific position in the list.
            child.MParent = this;
        }

        public ASTElement GetChild(int context, int index = 0) {
            return m_children[context][index];  // Getting a child from the list.
        }

        public IEnumerable<ASTElement> GetChildren(int context) {
            return m_children[context];
        }

        public IEnumerable<ASTElement> GetContextChildren(int context) {
            foreach (ASTElement c in m_children[context]) {
                yield return c;
            }
        }

        public virtual string GenerateNodeName() {
            return Enum.GetName(typeof(NodeType), Type) + "_" + m_serial;  // The default name of a node.
        }

        public int GetContextChildrenNumber(int context) {
            if (m_children.Length > context) {
                return m_children[context].Count;
            }
            else {
                throw new IndexOutOfRangeException("Index out of context's array range for the current node!");
            }
        }

        public int GetContextNumber() {
            return m_children.Length;
        }
    }
}
