namespace MiniC {
    public abstract class CNodeType<T> { // T = all the node types of a grammar.
        private T m_nodeType;

        public T NodeType => m_nodeType;

        protected CNodeType(T nodeType) { // Constructor
            m_nodeType = nodeType;
        }

        public abstract T Default();    // Returns the default type value.

        public abstract T NA(); // Returns Non-Applicable type value.

        public abstract T Map(int type);    // Maps an integer to a type value.

        public abstract int Map(T type);    // Maps a type to an integer.
    }
}
