grammar MiniC;

/*
 * Parser Rules
 */

compileUnit
	: (function_definition | statement)+ EOF
	;

function_definition
	: FUNCTION VARIABLE LP fargs? RP compound_statement						#FunctionDefinition
	;

fargs
	: VARIABLE ( COMMA VARIABLE )*
	;

statement 
	: expr_statement
	| compound_statement
	| if_statement
	| for_statement
	| while_statement
	| do_while_statement
	| switch_statement
	| return_statement
	| break_statement
	| null_statement
	;

expr_statement
	: expr SEMICOLON														#StatementExpr
	;

if_statement
	: IF LP expr RP statement ( ELSE statement )?							#StatementIf
	;

for_statement
	: FOR LP expr SEMICOLON expr SEMICOLON expr RP compound_statement		#StatementFor
	;

while_statement
	: WHILE LP expr RP compound_statement									#StatementWhile
	;

do_while_statement
	: DO compound_statement WHILE LP expr RP SEMICOLON						#StatementDoWhile
	;

switch_statement
	: SWITCH LP expr RP LB (switch_case)+ (switch_default)? RB				#StatementSwitch
	;

switch_case
	: CASE expr COLON statement
	;

switch_default
	: DEFAULT COLON statement
	;

return_statement
	: RETURN expr SEMICOLON													#StatementRETURN
	;

break_statement
	: BREAK SEMICOLON														#StatementBREAK
	;

null_statement
	: SEMICOLON																#StatementNULL
	;

compound_statement
	: LB statements? RB														#StatementCompound
	;

statements
	: statement+
	;

expr
	: COMMENT																#ExprCOMMENT
	| STRING																#ExprSTRING
	| INTEGER																#ExprINTEGER
	| VARIABLE																#ExprVARIABLE
	| VARIABLE LP args? RP													#ExprFCall
	| expr PLUSONE															#ExprPlusOne
	| expr MINUSONE															#ExprMinusOne
	| PLUS expr																#ExprUnaryPlus
	| MINUS expr															#ExprUnaryMinus
	| NOT expr																#ExprNot
	| expr op=(MULTI|DIV) expr												#ExprMulDiv
	| expr op=(PLUS|MINUS) expr												#ExprAddSub
	| LP expr RP															#ExprParenthesis
	| expr op=(GT|GTE|LT|LTE) expr											#ExprRelationalOperators
	| expr op=(EQUAL|NEQUAL) expr											#ExprComparison
	| expr AND expr															#ExprAnd
	| expr OR expr															#ExprOr
	| VARIABLE ASSIGN<assoc=right> expr										#ExprAssignment
	;

args
	: expr ( COMMA expr )*
	;

/*
 * Lexer Rules
 */

FUNCTION : 'function';
IF : 'if';
ELSE : 'else';
FOR : 'for';
WHILE : 'while';
DO : 'do';
SWITCH : 'switch';
CASE : 'case';
DEFAULT : 'default';
RETURN : 'return';
BREAK : 'break';

LP : '(';
RP : ')';
LB : '{';
RB : '}';
SEMICOLON : ';';
COLON : ':';
COMMA : ',';
PLUS : '+';
MINUS : '-';
MULTI : '*';
DIV : '/';
ASSIGN : '=';
PLUSONE : '++';
MINUSONE : '--';
AND : '&&';
OR : '||';
NOT : '!';
GT : '>';
GTE : '>=';
LT : '<';
LTE : '<=';
EQUAL : '==';
NEQUAL : '!=';

INTEGER : '0'|[1-9][0-9]*;
VARIABLE : [a-zA-Z][a-zA-Z0-9_]*;
STRING : ('"'(~[\n\"]|('\\\n')|('\\'.))*'"' | '\''(~[\n\']|('\\\n')|('\\'.))*'\'');
COMMENT : '/*'(.|'\n')*?'*\\' | '\\'.*;
WS
	:	[\n\r\t ] -> skip
	;
