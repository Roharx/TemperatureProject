ì
'C:\tempProjNew\exceptions\Exceptions.cs
	namespace 	

exceptions
 
; 
public 
class 

Exceptions 
{ 
public 

class '
DatabaseConnectionException ,
:- .
	Exception/ 8
{ 
public '
DatabaseConnectionException *
(* +
string+ 1
message2 9
,9 :
	Exception; D
innerExceptionE S
)S T
: 
base 
( 
message 
, 
innerException *
)* +
{		 	
}

 	
} 
public 

class #
QueryExecutionException (
:) *
	Exception+ 4
{ 
public #
QueryExecutionException &
(& '
string' -
message. 5
,5 6
	Exception7 @
innerExceptionA O
)O P
: 
base 
( 
message 
, 
innerException *
)* +
{ 	
} 	
} 
public 

class *
LoggingQueryExecutionException /
:0 1
	Exception2 ;
{ 
public *
LoggingQueryExecutionException -
(- .
string. 4
message5 <
,< =
	Exception> G
innerExceptionH V
)V W
: 
base 
( 
message 
, 
innerException *
)* +
{ 	
} 	
} 
public 

class '
InvalidAccountDataException ,
:- .
	Exception/ 8
{ 
public '
InvalidAccountDataException *
(* +
string+ 1
message2 9
)9 :
:; <
base= A
(A B
messageB I
)I J
{K L
}M N
} 
public 

class $
PasswordHashingException )
:* +
	Exception, 5
{   
public!! $
PasswordHashingException!! '
(!!' (
string!!( .
message!!/ 6
,!!6 7
	Exception!!8 A
innerException!!B P
)!!P Q
:!!R S
base!!T X
(!!X Y
message!!Y `
,!!` a
innerException!!b p
)!!p q
{!!r s
}!!t u
}"" 
public$$ 

class$$ !
InvalidTokenException$$ &
:$$' (
	Exception$$) 2
{%% 
public&& !
InvalidTokenException&& $
(&&$ %
)&&% &
:&&' (
base&&) -
(&&- .
$str&&. I
)&&I J
{'' 	
}(( 	
})) 
},, 