È
3C:\tempProjNew\infrastructure\DataModels\Account.cs
	namespace 	
infrastructure
 
. 

DataModels #
;# $
public 
class 
Account 
{ 
public 

Account 
( 
int 
id 
, 
string !
name" &
,& '
string( .
email/ 4
)4 5
{ 
Id 

= 
id 
; 
Name 
= 
name 
; 
Email		 
=		 
email		 
;		 
}

 
public 

int 
Id 
{ 
get 
; 
set 
; 
} 
public 

string 
Name 
{ 
get 
; 
set !
;! "
}# $
public 

string 
Email 
{ 
get 
; 
set "
;" #
}$ %
} ‰
?C:\tempProjNew\infrastructure\DataModels\AccountVerification.cs
	namespace 	
infrastructure
 
. 

DataModels #
;# $
public 
class 
AccountVerification  
{ 
public 

int 
Id 
{ 
get 
; 
set 
; 
} 
public 

string 
Name 
{ 
get 
; 
set !
;! "
}# $
=% &
string' -
.- .
Empty. 3
;3 4
public 

string 
Email 
{ 
get 
; 
set "
;" #
}$ %
=& '
string( .
.. /
Empty/ 4
;4 5
public 

string 
Password 
{ 
get  
;  !
set" %
;% &
}' (
=) *
string+ 1
.1 2
Empty2 7
;7 8
}		 “
8C:\tempProjNew\infrastructure\Interfaces\ICrudHandler.cs
	namespace 	
infrastructure
 
. 

Interfaces #
;# $
public 
	interface 
ICrudHandler 
{ 
IEnumerable 
< 
T 
> 
GetAllItems 
< 
T  
>  !
(! "
string" (
	tableName) 2
)2 3
;3 4
IEnumerable 
< 
T 
>  
GetItemsByParameters '
<' (
T( )
>) *
(* +
string+ 1
	tableName2 ;
,; <

Dictionary= G
<G H
stringH N
,N O
objectP V
>V W

parametersX b
)b c
;c d
T 
? %
GetSingleItemByParameters  
<  !
T! "
>" #
(# $
string$ *
	tableName+ 4
,4 5

Dictionary6 @
<@ A
stringA G
,G H
objectI O
>O P

parametersQ [
)[ \
;\ ]
IEnumerable)) 
<)) 
T)) 
>)) )
GetSelectedParametersForItems)) 0
<))0 1
T))1 2
>))2 3
())3 4
string))4 :
	tableName)); D
,))D E
string))F L
columns))M T
,))T U

Dictionary** 
<** 
string** 
,** 
object** !
>**! "

parameters**# -
)**- .
;**. /
int88 

CreateItem88 
(88 
string88 
	tableName88 #
,88# $

Dictionary88% /
<88/ 0
string880 6
,886 7
object888 >
>88> ?

parameters88@ J
)88J K
;88K L
boolAA #
CreateItemWithoutReturnAA	  
(AA  !
stringAA! '
	tableNameAA( 1
,AA1 2

DictionaryAA3 =
<AA= >
stringAA> D
,AAD E
objectAAF L
>AAL M

parametersAAN X
)AAX Y
;AAY Z
boolXX 

UpdateItemXX	 
(XX 
stringXX 
	tableNameXX $
,XX$ %

DictionaryXX& 0
<XX0 1
stringXX1 7
,XX7 8
objectXX9 ?
>XX? @
conditionColumnsXXA Q
,XXQ R

DictionaryXXS ]
<XX] ^
stringXX^ d
,XXd e
objectXXf l
>XXl m
modificationsXXn {
)XX{ |
;XX| }
booldd 

DeleteItemdd	 
(dd 
stringdd 
	tableNamedd $
,dd$ %
intdd& )
itemIddd* 0
)dd0 1
;dd1 2
boolll (
DeleteItemWithMultipleParamsll	 %
(ll% &
stringll& ,
	tableNamell- 6
,ll6 7

Dictionaryll8 B
<llB C
stringllC I
,llI J
objectllK Q
>llQ R
conditionColumnsllS c
)llc d
;lld e
}oo ≈
5C:\tempProjNew\infrastructure\Interfaces\IDatabase.cs
	namespace 	
infrastructure
 
. 

Interfaces #
{ 
public 

	interface 
	IDatabase 
{ 
IEnumerable 
< 
T 
> 
Query 
< 
T 
> 
(  
string  &
sql' *
,* +
object, 2
?2 3
param4 9
=: ;
null< @
,@ A
IDbTransactionB P
?P Q
transactionR ]
=^ _
null` d
,d e
boolf j
bufferedk s
=t u
truev z
,z {
int| 
?	 Ä
commandTimeout
Å è
=
ê ë
null
í ñ
,
ñ ó
CommandType
ò £
?
£ §
commandType
• ∞
=
± ≤
null
≥ ∑
)
∑ ∏
;
∏ π
T 	
?	 

QueryFirstOrDefault 
< 
T  
>  !
(! "
string" (
sql) ,
,, -
object. 4
?4 5
param6 ;
=< =
null> B
,B C
IDbTransactionD R
?R S
transactionT _
=` a
nullb f
,f g
inth k
?k l
commandTimeoutm {
=| }
null	~ Ç
,
Ç É
CommandType
Ñ è
?
è ê
commandType
ë ú
=
ù û
null
ü £
)
£ §
;
§ •
int		 
Execute		 
(		 
string		 
sql		 
,		 
object		  &
?		& '
param		( -
=		. /
null		0 4
,		4 5
IDbTransaction		6 D
?		D E
transaction		F Q
=		R S
null		T X
,		X Y
int		Z ]
?		] ^
commandTimeout		_ m
=		n o
null		p t
,		t u
CommandType			v Å
?
		Å Ç
commandType
		É é
=
		è ê
null
		ë ï
)
		ï ñ
;
		ñ ó
T

 	
?

	 

ExecuteScalar

 
<

 
T

 
>

 
(

 
string

 "
sql

# &
,

& '
object

( .
?

. /
param

0 5
=

6 7
null

8 <
,

< =
IDbTransaction

> L
?

L M
transaction

N Y
=

Z [
null

\ `
,

` a
int

b e
?

e f
commandTimeout

g u
=

v w
null

x |
,

| }
CommandType	

~ â
?


â ä
commandType


ã ñ
=


ó ò
null


ô ù
)


ù û
;


û ü
} 
} ˇ
DC:\tempProjNew\infrastructure\Interfaces\INpgsqlConnectionFactory.cs
	namespace 	
infrastructure
 
. 

Interfaces #
;# $
public 
	interface $
INpgsqlConnectionFactory )
{ 
NpgsqlConnection 
CreateConnection %
(% &
)& '
;' (
} r
9C:\tempProjNew\infrastructure\Repositories\CrudHandler.cs
	namespace 	
infrastructure
 
. 
Repositories %
{ 
public 

class 
CrudHandler 
: 
ICrudHandler +
{		 
private

 
readonly

 
	IDatabase

 "
	_database

# ,
;

, -
public 
CrudHandler 
( 
	IDatabase $
database% -
)- .
{ 	
	_database 
= 
database  
;  !
} 	
private 
T 
ExecuteDbOperation $
<$ %
T% &
>& '
(' (
Func( ,
<, -
	IDatabase- 6
,6 7
T8 9
>9 :
	operation; D
)D E
{ 	
try 
{ 
return 
	operation  
(  !
	_database! *
)* +
;+ ,
} 
catch 
( 
NpgsqlException "
ex# %
)% &
{ 
throw 
new 

Exceptions $
.$ %'
DatabaseConnectionException% @
(@ A
$strA i
,i j
exk m
)m n
;n o
} 
catch 
( 
	Exception 
ex 
)  
{ 
throw 
new 

Exceptions $
.$ %#
QueryExecutionException% <
(< =
$str= g
,g h
exi k
)k l
;l m
} 
} 	
private!! 
static!! 
string!! 
BuildWhereClause!! .
(!!. /

Dictionary!!/ 9
<!!9 :
string!!: @
,!!@ A
object!!B H
>!!H I

parameters!!J T
)!!T U
{"" 	
return## 
string## 
.## 
Join## 
(## 
$str## &
,##& '

parameters##( 2
.##2 3
Keys##3 7
.##7 8
Select##8 >
(##> ?
key##? B
=>##C E
$"##F H
{##H I
key##I L
}##L M
$str##M Q
{##Q R
key##R U
}##U V
"##V W
)##W X
)##X Y
;##Y Z
}$$ 	
public&& 
IEnumerable&& 
<&& 
T&& 
>&& 
GetAllItems&& )
<&&) *
T&&* +
>&&+ ,
(&&, -
string&&- 3
	tableName&&4 =
)&&= >
{'' 	
var(( 
sql(( 
=(( 
$"(( 
$str(( &
{((& '
	tableName((' 0
}((0 1
"((1 2
;((2 3
return)) 
ExecuteDbOperation)) %
())% &
db))& (
=>))) +
db)), .
.)). /
Query))/ 4
<))4 5
T))5 6
>))6 7
())7 8
sql))8 ;
))); <
)))< =
;))= >
}** 	
public,, 
IEnumerable,, 
<,, 
T,, 
>,,  
GetItemsByParameters,, 2
<,,2 3
T,,3 4
>,,4 5
(,,5 6
string,,6 <
	tableName,,= F
,,,F G

Dictionary,,H R
<,,R S
string,,S Y
,,,Y Z
object,,[ a
>,,a b

parameters,,c m
),,m n
{-- 	
var.. 
whereClause.. 
=.. 
BuildWhereClause.. .
(... /

parameters../ 9
)..9 :
;..: ;
var// 
sql// 
=// 
$"// 
$str// &
{//& '
	tableName//' 0
}//0 1
$str//1 8
{//8 9
whereClause//9 D
}//D E
"//E F
;//F G
return00 
ExecuteDbOperation00 %
(00% &
db00& (
=>00) +
db00, .
.00. /
Query00/ 4
<004 5
T005 6
>006 7
(007 8
sql008 ;
,00; <

parameters00= G
)00G H
)00H I
;00I J
}11 	
public33 
T33 
?33 %
GetSingleItemByParameters33 +
<33+ ,
T33, -
>33- .
(33. /
string33/ 5
	tableName336 ?
,33? @

Dictionary33A K
<33K L
string33L R
,33R S
object33T Z
>33Z [

parameters33\ f
)33f g
{44 	
var55 
whereClause55 
=55 
BuildWhereClause55 .
(55. /

parameters55/ 9
)559 :
;55: ;
var66 
sql66 
=66 
$"66 
$str66 &
{66& '
	tableName66' 0
}660 1
$str661 8
{668 9
whereClause669 D
}66D E
"66E F
;66F G
return77 
ExecuteDbOperation77 %
(77% &
db77& (
=>77) +
db77, .
.77. /
QueryFirstOrDefault77/ B
<77B C
T77C D
>77D E
(77E F
sql77F I
,77I J

parameters77K U
)77U V
)77V W
;77W X
}88 	
public:: 
IEnumerable:: 
<:: 
T:: 
>:: )
GetSelectedParametersForItems:: ;
<::; <
T::< =
>::= >
(::> ?
string::? E
	tableName::F O
,::O P
string::Q W
columns::X _
,::_ `

Dictionary::a k
<::k l
string::l r
,::r s
object::t z
>::z {

parameters	::| Ü
)
::Ü á
{;; 	
var<< 
whereClause<< 
=<< 
BuildWhereClause<< .
(<<. /

parameters<</ 9
)<<9 :
;<<: ;
var== 
sql== 
=== 
$"== 
$str== 
{==  
columns==  '
}==' (
$str==( .
{==. /
	tableName==/ 8
}==8 9
$str==9 @
{==@ A
whereClause==A L
}==L M
"==M N
;==N O
return>> 
ExecuteDbOperation>> %
(>>% &
db>>& (
=>>>) +
db>>, .
.>>. /
Query>>/ 4
<>>4 5
T>>5 6
>>>6 7
(>>7 8
sql>>8 ;
,>>; <

parameters>>= G
)>>G H
)>>H I
;>>I J
}?? 	
publicAA 
intAA 

CreateItemAA 
(AA 
stringAA $
	tableNameAA% .
,AA. /

DictionaryAA0 :
<AA: ;
stringAA; A
,AAA B
objectAAC I
>AAI J

parametersAAK U
)AAU V
{BB 	
varCC 
columnsCC 
=CC 
stringCC  
.CC  !
JoinCC! %
(CC% &
$strCC& *
,CC* +

parametersCC, 6
.CC6 7
KeysCC7 ;
)CC; <
;CC< =
varDD 
valuesDD 
=DD 
stringDD 
.DD  
JoinDD  $
(DD$ %
$strDD% )
,DD) *

parametersDD+ 5
.DD5 6
KeysDD6 :
.DD: ;
SelectDD; A
(DDA B
keyDDB E
=>DDF H
$"DDI K
$strDDK L
{DDL M
keyDDM P
}DDP Q
"DDQ R
)DDR S
)DDS T
;DDT U
varEE 
sqlEE 
=EE 
$"EE 
$strEE $
{EE$ %
	tableNameEE% .
}EE. /
$strEE/ 1
{EE1 2
columnsEE2 9
}EE9 :
$strEE: D
{EED E
valuesEEE K
}EEK L
$strEEL Z
"EEZ [
;EE[ \
returnGG 
ExecuteDbOperationGG %
(GG% &
dbGG& (
=>GG) +
dbGG, .
.GG. /
ExecuteScalarGG/ <
<GG< =
intGG= @
>GG@ A
(GGA B
sqlGGB E
,GGE F

parametersGGG Q
)GGQ R
)GGR S
;GGS T
}HH 	
publicJJ 
boolJJ #
CreateItemWithoutReturnJJ +
(JJ+ ,
stringJJ, 2
	tableNameJJ3 <
,JJ< =

DictionaryJJ> H
<JJH I
stringJJI O
,JJO P
objectJJQ W
>JJW X

parametersJJY c
)JJc d
{KK 	
varLL 
columnsLL 
=LL 
stringLL  
.LL  !
JoinLL! %
(LL% &
$strLL& *
,LL* +

parametersLL, 6
.LL6 7
KeysLL7 ;
)LL; <
;LL< =
varMM 
valuesMM 
=MM 
stringMM 
.MM  
JoinMM  $
(MM$ %
$strMM% )
,MM) *

parametersMM+ 5
.MM5 6
KeysMM6 :
.MM: ;
SelectMM; A
(MMA B
keyMMB E
=>MMF H
$"MMI K
$strMMK L
{MML M
keyMMM P
}MMP Q
"MMQ R
)MMR S
)MMS T
;MMT U
varNN 
sqlNN 
=NN 
$"NN 
$strNN $
{NN$ %
	tableNameNN% .
}NN. /
$strNN/ 1
{NN1 2
columnsNN2 9
}NN9 :
$strNN: D
{NND E
valuesNNE K
}NNK L
$strNNL M
"NNM N
;NNN O
ExecuteDbOperationPP 
(PP 
dbPP !
=>PP" $
dbPP% '
.PP' (
ExecutePP( /
(PP/ 0
sqlPP0 3
,PP3 4

parametersPP5 ?
)PP? @
)PP@ A
;PPA B
returnQQ 
trueQQ 
;QQ 
}RR 	
publicTT 
boolTT 

UpdateItemTT 
(TT 
stringTT %
	tableNameTT& /
,TT/ 0

DictionaryTT1 ;
<TT; <
stringTT< B
,TTB C
objectTTD J
>TTJ K
conditionColumnsTTL \
,TT\ ]

DictionaryTT^ h
<TTh i
stringTTi o
,TTo p
objectTTq w
>TTw x
modifications	TTy Ü
)
TTÜ á
{UU 	
varVV 
conditionClausesVV  
=VV! "
stringVV# )
.VV) *
JoinVV* .
(VV. /
$strVV/ 6
,VV6 7
conditionColumnsVV8 H
.VVH I
SelectVVI O
(VVO P
condVVP T
=>VVU W
$"VVX Z
{VVZ [
condVV[ _
.VV_ `
KeyVV` c
}VVc d
$strVVd h
{VVh i
condVVi m
.VVm n
KeyVVn q
}VVq r
"VVr s
)VVs t
)VVt u
;VVu v
varWW 
	updateSetWW 
=WW 
stringWW "
.WW" #
JoinWW# '
(WW' (
$strWW( ,
,WW, -
modificationsWW. ;
.WW; <
SelectWW< B
(WWB C
modWWC F
=>WWG I
$"WWJ L
{WWL M
modWWM P
.WWP Q
KeyWWQ T
}WWT U
$strWWU Y
{WWY Z
modWWZ ]
.WW] ^
KeyWW^ a
}WWa b
"WWb c
)WWc d
)WWd e
;WWe f
varYY 
sqlYY 
=YY 
$"YY 
$strYY 
{YY  
	tableNameYY  )
}YY) *
$strYY* /
{YY/ 0
	updateSetYY0 9
}YY9 :
$strYY: A
{YYA B
conditionClausesYYB R
}YYR S
"YYS T
;YYT U
var[[ 

parameters[[ 
=[[ 
conditionColumns[[ -
.[[- .
Union[[. 3
([[3 4
modifications[[4 A
)[[A B
.[[B C
ToDictionary[[C O
([[O P
pair[[P T
=>[[U W
pair[[X \
.[[\ ]
Key[[] `
,[[` a
pair[[b f
=>[[g i
pair[[j n
.[[n o
Value[[o t
)[[t u
;[[u v
ExecuteDbOperation\\ 
(\\ 
db\\ !
=>\\" $
db\\% '
.\\' (
Execute\\( /
(\\/ 0
sql\\0 3
,\\3 4

parameters\\5 ?
)\\? @
)\\@ A
;\\A B
return]] 
true]] 
;]] 
}^^ 	
public`` 
bool`` 

DeleteItem`` 
(`` 
string`` %
	tableName``& /
,``/ 0
int``1 4
itemId``5 ;
)``; <
{aa 	
varbb 
sqlbb 
=bb 
$"bb 
$strbb $
{bb$ %
	tableNamebb% .
}bb. /
$strbb/ <
"bb< =
;bb= >
ExecuteDbOperationcc 
(cc 
dbcc !
=>cc" $
dbcc% '
.cc' (
Executecc( /
(cc/ 0
sqlcc0 3
,cc3 4
newcc5 8
{cc9 :
idcc; =
=cc> ?
itemIdcc@ F
}ccG H
)ccH I
)ccI J
;ccJ K
returndd 
truedd 
;dd 
}ee 	
publicgg 
boolgg (
DeleteItemWithMultipleParamsgg 0
(gg0 1
stringgg1 7
	tableNamegg8 A
,ggA B

DictionaryggC M
<ggM N
stringggN T
,ggT U
objectggV \
>gg\ ]
conditionColumnsgg^ n
)ggn o
{hh 	
varii 
conditionClausesii  
=ii! "
stringii# )
.ii) *
Joinii* .
(ii. /
$strii/ 6
,ii6 7
conditionColumnsii8 H
.iiH I
SelectiiI O
(iiO P
condiiP T
=>iiU W
$"iiX Z
{iiZ [
condii[ _
.ii_ `
Keyii` c
}iic d
$striid h
{iih i
condiii m
.iim n
Keyiin q
}iiq r
"iir s
)iis t
)iit u
;iiu v
varjj 
sqljj 
=jj 
$"jj 
$strjj $
{jj$ %
	tableNamejj% .
}jj. /
$strjj/ 6
{jj6 7
conditionClausesjj7 G
}jjG H
"jjH I
;jjI J
ExecuteDbOperationll 
(ll 
dbll !
=>ll" $
dbll% '
.ll' (
Executell( /
(ll/ 0
sqlll0 3
,ll3 4
conditionColumnsll5 E
)llE F
)llF G
;llG H
returnmm 
truemm 
;mm 
}nn 	
}oo 
}pp ı,
<C:\tempProjNew\infrastructure\Repositories\DapperDatabase.cs
	namespace 	
infrastructure
 
. 
Repositories %
{ 
public 

class 
DapperDatabase 
:  !
	IDatabase" +
{		 
private

 
readonly

 $
INpgsqlConnectionFactory

 1
_connectionFactory

2 D
;

D E
public 
DapperDatabase 
( $
INpgsqlConnectionFactory 6
connectionFactory7 H
)H I
{ 	
_connectionFactory 
=  
connectionFactory! 2
;2 3
} 	
private 
NpgsqlConnection  
GetOpenConnection! 2
(2 3
)3 4
{ 	
return 
_connectionFactory %
.% &
CreateConnection& 6
(6 7
)7 8
;8 9
} 	
public 
IEnumerable 
< 
T 
> 
Query #
<# $
T$ %
>% &
(& '
string' -
sql. 1
,1 2
object3 9
?9 :
param; @
=A B
nullC G
,G H
IDbTransactionI W
?W X
transactionY d
=e f
nullg k
,k l
boolm q
bufferedr z
={ |
true	} Å
,
Å Ç
int
É Ü
?
Ü á
commandTimeout
à ñ
=
ó ò
null
ô ù
,
ù û
CommandType
ü ™
?
™ ´
commandType
¨ ∑
=
∏ π
null
∫ æ
)
æ ø
{ 	
using 
var 

connection  
=! "
GetOpenConnection# 4
(4 5
)5 6
;6 7
return 

connection 
. 
Query #
<# $
T$ %
>% &
(& '
sql' *
,* +
param, 1
,1 2
transaction3 >
,> ?
buffered@ H
,H I
commandTimeoutJ X
,X Y
commandTypeZ e
)e f
;f g
} 	
public 
T 
? 
QueryFirstOrDefault %
<% &
T& '
>' (
(( )
string) /
sql0 3
,3 4
object5 ;
?; <
param= B
=C D
nullE I
,I J
IDbTransactionK Y
?Y Z
transaction[ f
=g h
nulli m
,m n
into r
?r s
commandTimeout	t Ç
=
É Ñ
null
Ö â
,
â ä
CommandType
ã ñ
?
ñ ó
commandType
ò £
=
§ •
null
¶ ™
)
™ ´
{ 	
using 
var 

connection  
=! "
GetOpenConnection# 4
(4 5
)5 6
;6 7
return 

connection 
. 
QueryFirstOrDefault 1
<1 2
T2 3
>3 4
(4 5
sql5 8
,8 9
param: ?
,? @
transactionA L
,L M
commandTimeoutN \
,\ ]
commandType^ i
)i j
;j k
}   	
public"" 
int"" 
Execute"" 
("" 
string"" !
sql""" %
,""% &
object""' -
?""- .
param""/ 4
=""5 6
null""7 ;
,""; <
IDbTransaction""= K
?""K L
transaction""M X
=""Y Z
null""[ _
,""_ `
int""a d
?""d e
commandTimeout""f t
=""u v
null""w {
,""{ |
CommandType	""} à
?
""à â
commandType
""ä ï
=
""ñ ó
null
""ò ú
)
""ú ù
{## 	
using$$ 
var$$ 

connection$$  
=$$! "
GetOpenConnection$$# 4
($$4 5
)$$5 6
;$$6 7
return%% 

connection%% 
.%% 
Execute%% %
(%%% &
sql%%& )
,%%) *
param%%+ 0
,%%0 1
transaction%%2 =
,%%= >
commandTimeout%%? M
,%%M N
commandType%%O Z
)%%Z [
;%%[ \
}&& 	
public(( 
T(( 
?(( 
ExecuteScalar(( 
<((  
T((  !
>((! "
(((" #
string((# )
sql((* -
,((- .
object((/ 5
?((5 6
param((7 <
=((= >
null((? C
,((C D
IDbTransaction((E S
?((S T
transaction((U `
=((a b
null((c g
,((g h
int((i l
?((l m
commandTimeout((n |
=((} ~
null	(( É
,
((É Ñ
CommandType
((Ö ê
?
((ê ë
commandType
((í ù
=
((û ü
null
((† §
)
((§ •
{)) 	
using** 
var** 

connection**  
=**! "
GetOpenConnection**# 4
(**4 5
)**5 6
;**6 7
return++ 

connection++ 
.++ 
ExecuteScalar++ +
<+++ ,
T++, -
>++- .
(++. /
sql++/ 2
,++2 3
param++4 9
,++9 :
transaction++; F
,++F G
commandTimeout++H V
,++V W
commandType++X c
)++c d
;++d e
},, 	
}-- 
}.. ”
EC:\tempProjNew\infrastructure\Repositories\NpgsqlConnectionFactory.cs
	namespace 	
infrastructure
 
. 
Repositories %
;% &
public 
class #
NpgsqlConnectionFactory $
:% &$
INpgsqlConnectionFactory' ?
{ 
private 
readonly 
string 
_connectionString -
;- .
public

 
#
NpgsqlConnectionFactory

 "
(

" #
string

# )
connectionString

* :
)

: ;
{ 
_connectionString 
= 
connectionString ,
;, -
} 
public 

NpgsqlConnection 
CreateConnection ,
(, -
)- .
{ 
return 
new 
NpgsqlConnection #
(# $
_connectionString$ 5
)5 6
;6 7
} 
} Ú
*C:\tempProjNew\infrastructure\Utilities.cs
	namespace 	
infrastructure
 
{ 
public 

abstract 
class 
	Utilities #
{ 
private 
static 
readonly 
Uri  #
Uri$ '
=( )
new* -
Uri. 1
(1 2
Environment2 =
.= >"
GetEnvironmentVariable> T
(T U
$strU g
)g h
??2 4
throw5 :
new; >%
InvalidOperationException? X
(X Y
$str6 o
)o p
)p q
;q r
public		 
static		 
readonly		 
string		 %-
!ProperlyFormattedConnectionString		& G
=		H I
$"

 
$str

 
{

 
Uri

 
.

 
Host

 
}

 
$str

  
"

  !
+

" #
$" 
$str 
{ 
Uri 
. 
AbsolutePath (
.( )
Trim) -
(- .
$char. 1
)1 2
}2 3
$str3 4
"4 5
+6 7
$" 
$str 
{ 
Uri 
. 
UserInfo #
.# $
Split$ )
() *
$char* -
)- .
[. /
$num/ 0
]0 1
}1 2
$str2 3
"3 4
+5 6
$" 
$str 
{ 
Uri 
. 
UserInfo $
.$ %
Split% *
(* +
$char+ .
). /
[/ 0
$num0 1
]1 2
}2 3
$str3 4
"4 5
+6 7
$" 
$str 
{ 
( 
Uri 
. 
Port 
> 
$num  !
?" #
Uri$ '
.' (
Port( ,
:- .
$num/ 3
)3 4
}4 5
$str5 6
"6 7
+8 9
$" 
$str 
" 
+ 
$" 
$str 
" 
; 
} 
} 