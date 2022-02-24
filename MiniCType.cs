using System;

namespace MiniC {
    public class CCompileUnit : ASTElement {
        /* Contains in order all expressions
         * that the user gives to the input file */
        public const int CT_BODY = 0, CT_FUNCTION_BODY = 1;
        public static readonly string[] msc_contextNames = { "statementContext", "functionDefinitionContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor) {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCompileUnit(this);
        }

        public CCompileUnit() : base(2, NodeType.NT_COMPILEUNIT) { }
    }

    public class CFunctionDefinition : ASTElement {
        public const int CT_FNAME = 0, CT_ARGS = 1, CT_BODY = 2;
        public static readonly string[] msc_contextNames = { "FUNCTIONNAME", "ARGS", "BODY" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor) {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCFunctionDefinition(this);
        }

        public CFunctionDefinition() : base(3, NodeType.NT_FUNCTIONDEFINITION) { }
    }

    // Statements
    public class CStatementExpr : ASTElement {
        public const int CT_BODY = 0;
        public static readonly string[] msc_contextNames = { "bodyExpr" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor) {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCStatementExpr(this);
        }

        public CStatementExpr() : base(1, NodeType.NT_STATEMENTEXPR) { }
    }

    public class CStatementCompound : ASTElement {
        public const int CT_BODY = 0;
        public static readonly string[] msc_contextNames = { "bodyCompound" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor) {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCStatementCompound(this);
        }

        public CStatementCompound() : base(1, NodeType.NT_STATEMENTCOMPOUND) { }
    }

    public class CStatementIf : ASTElement {
        public const int CT_CONDITION = 0, CT_TRUE_BODY = 1, CT_FALSE_BODY = 2;
        public static readonly string[] msc_contextNames = { "conditionIf", "trueIf", "falseIf" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor) {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCStatementIf(this);
        }

        public CStatementIf() : base(3, NodeType.NT_STATEMENTIF) { }
    }

    public class CStatementFor : ASTElement {
        public const int CT_START = 0, CT_CONDITION = 1, CT_LOOP = 2, CT_BODY = 3;
        public static readonly string[] msc_contextNames = { "startFor", "conditionFor", "loopFor", "bodyFor" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor) {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCStatementFor(this);
        }

        public CStatementFor() : base(4, NodeType.NT_STATEMENTFOR) { }
    }

    public class CStatementWhile : ASTElement {
        public const int CT_CONDITION = 0, CT_BODY = 1;
        public static readonly string[] msc_contextNames = { "conditionWhile", "bodyWhile" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor) {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCStatementWhile(this);
        }

        public CStatementWhile() : base(2, NodeType.NT_STATEMENTWHILE) { }
    }

    public class CStatementDoWhile : ASTElement {
        public const int CT_BODY = 0, CT_CONDITION = 1;
        public static readonly string[] msc_contextNames = { "bodyDoWhile", "conditionDoWhile" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor) {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCStatementDoWhile(this);
        }

        public CStatementDoWhile() : base(2, NodeType.NT_STATEMENTDOWHILE) { }
    }
    
    public class CStatementSwitch : ASTElement {
        public const int CT_IDENTIFIER = 0, CT_CASE = 1, CT_DEFAULT = 2;
        public static readonly string[] msc_contextNames = { "identifierSwitch", "caseSwitch", "defaultSwitch" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor) {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCStatementSwitch(this);
        }

        public CStatementSwitch() : base(3, NodeType.NT_STATEMENTSWITCH) { }
    }

    public class CStatementRETURN : ASTElement {
        public const int CT_BODY = 0;
        public static readonly string[] msc_contextNames = { "bodyContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor) {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCStatementRETURN(this);
        }

        public CStatementRETURN() : base(1, NodeType.NT_STATEMENTRETURN) { }
    }

    public class CStatementBreak : ASTElement {
        public const int CT_BODY = 0;
        public static readonly string[] msc_contextNames = { "bodyContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor) {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCStatementBreak(this);
        }

        public CStatementBreak() : base(1, NodeType.NT_STATEMENTBREAK) { }
    }

    public class CStatementNull : ASTElement {
        public override T Accept<T>(ASTBaseVisitor<T> visitor) {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCStatementNull(this);
        }

        public CStatementNull() : base(0, NodeType.NT_STATEMENTNULL) { }
    }

    // Expressions
    public class CExprAssignment : ASTElement {
        public const int CT_LEFT = 0, CT_RIGHT = 1;
        public static readonly string[] msc_contextNames = { "leftContext", "rightContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor) {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprAssignment(this);
        }

        public CExprAssignment() : base(2, NodeType.NT_ASSIGNMENT) { }
    }

    public class CExprPlusOne : ASTElement
    {
        public const int CT_BODY = 0;
        public static readonly string[] msc_contextNames = { "bodyContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprPlusOne(this);
        }

        public CExprPlusOne() : base(1, NodeType.NT_PLUSONE)
        {
        }
    }

    public class CExprMinusOne : ASTElement
    {
        public const int CT_BODY = 0;
        public static readonly string[] msc_contextNames = { "bodyContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprMinusOne(this);
        }

        public CExprMinusOne() : base(1, NodeType.NT_MINUSONE)
        {
        }
    }

    public class CExprUnaryPlus : ASTElement
    {
        public const int CT_BODY = 0;
        public static readonly string[] msc_contextNames = { "bodyContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprUnaryPlus(this);
        }

        public CExprUnaryPlus() : base(1, NodeType.NT_UNARYPLUS)
        {
        }
    }

    public class CExprUnaryMinus : ASTElement
    {
        public const int CT_BODY = 0;
        public static readonly string[] msc_contextNames = { "bodyContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprUnaryMinus(this);
        }

        public CExprUnaryMinus() : base(1, NodeType.NT_UNARYMINUS)
        {
        }
    }

    public class CExprNot : ASTElement
    {
        public const int CT_BODY = 0;
        public static readonly string[] msc_contextNames = { "bodyContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprNot(this);
        }

        public CExprNot() : base(1, NodeType.NT_NOT)
        {
        }
    }

    public class CExprMultiplication : ASTElement
    {
        public const int CT_LEFT = 0, CT_RIGHT = 1;
        public static readonly string[] msc_contextNames = { "leftContext", "rightContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprMultiplication(this);
        }

        public CExprMultiplication() : base(2, NodeType.NT_MULTIPLICATION)
        {
        }
    }

    public class CExprDivision : ASTElement
    {
        public const int CT_LEFT = 0, CT_RIGHT = 1;
        public static readonly string[] msc_contextNames = { "leftContext", "rightContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprDivision(this);
        }

        public CExprDivision() : base(2, NodeType.NT_DIVISION)
        {
        }
    }

    public class CExprAddition : ASTElement
    {
        public const int CT_LEFT = 0, CT_RIGHT = 1;
        public static readonly string[] msc_contextNames = { "leftContext", "rightContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprAddition(this);
        }

        public CExprAddition() : base(2, NodeType.NT_ADDITION)
        {
        }
    }

    public class CExprSubtraction : ASTElement
    {
        public const int CT_LEFT = 0, CT_RIGHT = 1;
        public static readonly string[] msc_contextNames = { "leftContext", "rightContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprSubtraction(this);
        }

        public CExprSubtraction() : base(2, NodeType.NT_SUBTRACTION)
        {
        }
    }

    public class CExprGreaterThan : ASTElement
    {
        public const int CT_LEFT = 0, CT_RIGHT = 1;
        public static readonly string[] msc_contextNames = { "leftContext", "rightContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprGreaterThan(this);
        }

        public CExprGreaterThan() : base(2, NodeType.NT_GREATERTHAN)
        {
        }
    }

    public class CExprGreaterThanEqual : ASTElement
    {
        public const int CT_LEFT = 0, CT_RIGHT = 1;
        public static readonly string[] msc_contextNames = { "leftContext", "rightContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprGreaterThanEqual(this);
        }

        public CExprGreaterThanEqual() : base(2, NodeType.NT_GREATERTHANEQUAL)
        {
        }
    }

    public class CExprLesserThan : ASTElement
    {
        public const int CT_LEFT = 0, CT_RIGHT = 1;
        public static readonly string[] msc_contextNames = { "leftContext", "rightContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprLesserThan(this);
        }

        public CExprLesserThan() : base(2, NodeType.NT_LESSERTHAN)
        {
        }
    }

    public class CExprLesserThanEqual : ASTElement
    {
        public const int CT_LEFT = 0, CT_RIGHT = 1;
        public static readonly string[] msc_contextNames = { "leftContext", "rightContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprLesserThanEqual(this);
        }

        public CExprLesserThanEqual() : base(2, NodeType.NT_LESSERTHANEQUAL)
        {
        }
    }

    public class CExprEqual : ASTElement
    {
        public const int CT_LEFT = 0, CT_RIGHT = 1;
        public static readonly string[] msc_contextNames = { "leftContext", "rightContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprEqual(this);
        }

        public CExprEqual() : base(2, NodeType.NT_EQUAL)
        {
        }
    }

    public class CExprNotEqual : ASTElement
    {
        public const int CT_LEFT = 0, CT_RIGHT = 1;
        public static readonly string[] msc_contextNames = { "leftContext", "rightContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprNotEqual(this);
        }

        public CExprNotEqual() : base(2, NodeType.NT_NOTEQUAL)
        {
        }
    }

    public class CExprAnd : ASTElement
    {
        public const int CT_LEFT = 0, CT_RIGHT = 1;
        public static readonly string[] msc_contextNames = { "leftContext", "rightContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprAnd(this);
        }

        public CExprAnd() : base(2, NodeType.NT_AND)
        {
        }
    }

    public class CExprOr : ASTElement
    {
        public const int CT_LEFT = 0, CT_RIGHT = 1;
        public static readonly string[] msc_contextNames = { "leftContext", "rightContext" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprOr(this);
        }

        public CExprOr() : base(2, NodeType.NT_OR)
        {
        }
    }

    public class CExprFCall : ASTElement
    {
        public const int CT_FNAME = 0, CT_ARGS = 1;
        public static readonly string[] msc_contextNames = { "fnameFCall", "argsFCall" };

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprFCall(this);
        }

        public CExprFCall() : base(2, NodeType.NT_FCALL)
        {
        }
    }

    // Terminal
    public class CExprCOMMENT : ASTElement
    {
        private string m_name;
        public string MName => m_name;

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprCOMMENT(this);
        }

        public CExprCOMMENT(String name) : base(0, NodeType.NT_COMMENT)
        {
            m_name = name;
        }
    }

    public class CExprSTRING : ASTElement
    {
        private string m_name;
        public string MName => m_name;

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprSTRING(this);
        }

        public CExprSTRING(String name) : base(0, NodeType.NT_STRING)
        {
            m_name = name;
        }
    }

    public class CExprINTEGER : ASTElement
    {
        private string m_text;
        private int m_value;

        public string MName => m_text;

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprINTEGER(this);
        }

        public CExprINTEGER(String name) : base(0, NodeType.NT_INTEGER)
        {
            m_text = name;
            m_value = int.Parse(name);
        }
    }

    public class CExprVARIABLE : ASTElement
    {
        private string m_name;
        public string MName => m_name;

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            MiniCASTBaseVisitor<T> miniCVisitor = visitor as MiniCASTBaseVisitor<T>;    // Casting in C#
            return miniCVisitor.VisitCExprVARIABLE(this);
        }

        public CExprVARIABLE(String name) : base(0, NodeType.NT_VARIABLE)
        {
            m_name = name;
        }
    }
}
