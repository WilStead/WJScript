using System.Linq;

namespace WScriptParser
{
    public static class Constants
    {
        public const char START_ARG = '(';
        public const char END_ARG = ')';
        public const char ARG_SEPARATOR = ',';
        public static char[] ARG_SEPARATORS = new char[]
        {
            ARG_SEPARATOR, ' '
        };

        public const char START_INDEX = '[';
        public const char END_INDEX = ']';

        public const char START_BLOCK = '{';
        public const char END_BLOCK = '}';

        public const char STRING_DELIMITER = '"';
        public static char[] STRING_DELIMITERS = new char[]
        {
            STRING_DELIMITER, '“', '”'
        };

        public static char[] TOKEN_DELIMITERS = new char[]
        {
            STRING_DELIMITER,
            START_ARG, END_ARG,
            START_BLOCK, END_BLOCK,
            START_INDEX, END_INDEX,
            EMPTY
        };

        public const char STRING_ESCAPE = '\\';

        public const char END_STATEMENT = ';';

        public static char[] END_SECTION = new char[]
        {
            END_ARG,
            END_BLOCK,
            END_STATEMENT
        };

        public static char[] NEXT_OR_END_SECTION = new char[]
        {
            ARG_SEPARATOR, END_ARG,
            END_INDEX,
            END_BLOCK,
            END_STATEMENT,
            ' '
        };

        public const string ASSIGNMENT = "=";
        public const string ADD_ASSIGN = "+=";
        public const string SUB_ASSIGN = "-=";
        public const string MULT_ASSIGN = "*=";
        public const string DIV_ASSIGN = "/=";
        public const string MOD_ASSIGN = "%=";
        public const string AND_ASSIGN = "&=";
        public const string OR_ASSIGN = "|=";
        public const string XOR_ASSIGN = "^=";

        public const string CHILD_SEPARATOR = ".";

        public static string[] OPERATION_ACTIONS = new string[]
        {
            ADD_ASSIGN,
            SUB_ASSIGN,
            MULT_ASSIGN,
            DIV_ASSIGN,
            MOD_ASSIGN,
            AND_ASSIGN,
            OR_ASSIGN,
            XOR_ASSIGN,
            CHILD_SEPARATOR
        };

        public const string NOT = "!";
        public const string INCREMENT = "++";
        public const string DECREMENT = "--";
        public const string EQUAL = "==";
        public const string NOT_EQUAL = "!=";
        public const string LESS_THAN = "<";
        public const string LESS_THAN_EQUAL = "<=";
        public const string GREATER_THAN = ">";
        public const string GREATER_THAN_EQUAL = ">=";
        public const string AND = "&&";
        public const string OR = "||";

        public const string ADD = "+";
        public const string SUB = "-";
        public const string MULT = "*";
        public const string DIV = "/";
        public const string MOD = "%";
        public const string POW = "^";

        public static string[] MATH_ACTIONS = new string[]
        {
            ASSIGNMENT,
            INCREMENT, DECREMENT,
            EQUAL, NOT_EQUAL,
            LESS_THAN, LESS_THAN_EQUAL,
            GREATER_THAN, GREATER_THAN_EQUAL,
            AND, OR,
            ADD, SUB, MULT, DIV, MOD, POW
        };

        public static string[] ACTIONS = OPERATION_ACTIONS.Concat(MATH_ACTIONS).ToArray();

        public const string IF = "if";
        public const string ELSE = "else";
        public const string ELSE_IF = "elseif";
        public const string WHILE = "while";
        public const string BREAK = "break";
        public const string CONTINUE = "continue";
        public const string FUNCTION = "function";
        public const string RETURN = "return";

        public const char EMPTY = '\0';
        public const char COMMENT = '/';

        public static char[] DELIMITERS =
            string.Concat(
                LESS_THAN, GREATER_THAN, ASSIGNMENT,
                NOT, ADD, SUB, MULT, DIV, MOD, POW,
                START_ARG, END_ARG, ARG_SEPARATOR,
                START_INDEX, END_INDEX,
                START_BLOCK, END_BLOCK,
                STRING_DELIMITER,
                END_STATEMENT,
                ' ')
            .ToCharArray();

        public static string[] CONTROL_STATEMENTS = new string[]
        {
            IF, ELSE, ELSE_IF,
            WHILE, BREAK, CONTINUE,
            FUNCTION, RETURN
        };
    }
}
