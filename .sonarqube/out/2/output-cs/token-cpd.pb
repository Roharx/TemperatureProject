ø
2C:\tempProjNew\service\Interfaces\IActionLogger.cs
	namespace 	
service
 
. 

Interfaces 
; 
public 
	interface 
IActionLogger 
{ 
public 

void 
	LogAction 
( 
int 
? 
	accountId (
,( )
int* -
roomId. 4
,4 5
int6 9
officeId: B
,B C
stringD J
actionDescriptionK \
)\ ]
;] ^
} ò
1C:\tempProjNew\service\Interfaces\ICrudService.cs
	namespace 	
service
 
. 

Interfaces 
; 
public 
	interface 
ICrudService 
{ 
IEnumerable 
< 
T 
> 
GetAllItems 
< 
T  
>  !
(! "
string" (
	tableName) 2
)2 3
;3 4
IEnumerable 
< 
T 
>  
GetItemsByParameters '
<' (
T( )
>) *
(* +
string+ 1
	tableName2 ;
,; <

Dictionary= G
<G H
stringH N
,N O
objectP V
>V W

parametersX b
)b c
;c d
T 
? %
GetSingleItemByParameters  
<  !
T! "
>" #
(# $
string$ *
	tableName+ 4
,4 5

Dictionary6 @
<@ A
stringA G
,G H
objectI O
>O P

parametersQ [
)[ \
;\ ]
IEnumerable 
< 
T 
> )
GetSelectedParametersForItems 0
<0 1
T1 2
>2 3
(3 4
string4 :
	tableName; D
,D E
stringF L
columnsM T
,T U

DictionaryV `
<` a
stringa g
,g h
objecti o
>o p

parametersq {
){ |
;| }
int

 

CreateItem

 
<

 
T

 
>

 
(

 
string

 
	tableName

 &
,

& '

Dictionary

( 2
<

2 3
string

3 9
,

9 :
object

; A
>

A B

parameters

C M
)

M N
;

N O
bool #
CreateItemWithoutReturn	  
(  !
string! '
	tableName( 1
,1 2

Dictionary3 =
<= >
string> D
,D E
objectF L
>L M

parametersN X
)X Y
;Y Z
bool 

UpdateItem	 
( 
string 
	tableName $
,$ %

Dictionary& 0
<0 1
string1 7
,7 8
object9 ?
>? @
conditionColumnsA Q
,Q R

DictionaryS ]
<] ^
string^ d
,d e
objectf l
>l m
modificationsn {
){ |
;| }
bool 

DeleteItem	 
( 
string 
	tableName $
,$ %
int& )
itemId* 0
)0 1
;1 2
bool (
DeleteItemWithMultipleParams	 %
(% &
string& ,
	tableName- 6
,6 7

Dictionary8 B
<B C
stringC I
,I J
objectK Q
>Q R
conditionColumnsS c
)c d
;d e
string 

?
 
VerifyLogin 
( 
string 
username '
,' (
string) /
password0 8
)8 9
;9 :
int 
CreateAccount 
( 

Dictionary  
<  !
string! '
,' (
object) /
>/ 0
accountData1 <
)< =
;= >
bool 
UpdateAccount	 
( 

Dictionary !
<! "
string" (
,( )
object* 0
>0 1
conditionColumns2 B
,B C

DictionaryD N
<N O
stringO U
,U V
objectW ]
>] ^
modifications_ l
)l m
;m n
} º
1C:\tempProjNew\service\Interfaces\IHashService.cs
	namespace 	
service
 
. 

Interfaces 
; 
public 
	interface 
IHashService 
{ 
string

 

HashPassword

 
(

 
string

 
password

 '
)

' (
;

( )
bool 
VerifyPassword	 
( 
string 
hashedPassword -
,- .
string/ 5
rawPassword6 A
)A B
;B C
} á
2C:\tempProjNew\service\Interfaces\ITokenService.cs
	namespace 	
service
 
. 

Interfaces 
; 
public 
	interface 
ITokenService 
{ 
string 

?
 
GenerateToken 
( 
Account !
account" )
)) *
;* +
} ö
/C:\tempProjNew\service\Services\ActionLogger.cs
	namespace 	
service
 
. 
Services 
{ 
public 

class 
ActionLogger 
: 
IActionLogger  -
{ 
private 
readonly 
ICrudService %
_service& .
;. /
public		 
ActionLogger		 
(		 
ICrudService		 (
service		) 0
)		0 1
{

 	
_service 
= 
service 
; 
} 	
public 
void 
	LogAction 
( 
int !
?! "
	accountId# ,
,, -
int. 1
roomId2 8
,8 9
int: =
officeId> F
,F G
stringH N
actionDescriptionO `
)` a
{ 	
try 
{ 
_service 
. 

CreateItem #
<# $
bool$ (
>( )
() *
$str* 5
,5 6
new 

Dictionary "
<" #
string# )
,) *
object+ 1
>1 2
{ 
{ 
$str %
,% &
	accountId' 0
}0 1
,1 2
{ 
$str $
,$ %
officeId& .
}. /
,/ 0
{ 
$str "
," #
roomId$ *
}* +
,+ ,
{ 
$str !
,! "
actionDescription# 4
}4 5
} 
) 
; 
} 
catch 
( 
	Exception 
e 
) 
{ 
Console 
. 
	WriteLine !
(! "
e" #
)# $
;$ %
throw 
new 

Exceptions $
.$ %*
LoggingQueryExecutionException% C
(C D
$strD [
,[ \
e\ ]
)] ^
;^ _
} 
} 	
}   
}!! ∑k
.C:\tempProjNew\service\Services\CrudService.cs
	namespace 	
service
 
. 
Services 
{ 
public 

class 
CrudService 
: 
ICrudService +
{		 
private

 
readonly

 
ICrudHandler

 %
_crudHandler

& 2
;

2 3
private 
readonly 
IHashService %
_hashService& 2
;2 3
private 
readonly 
ITokenService &
_tokenService' 4
;4 5
public 
CrudService 
( 
ICrudHandler '
crudHandler( 3
,3 4
IHashService5 A
hashServiceB M
,M N
ITokenServiceO \
tokenService] i
)i j
{ 	
_crudHandler 
= 
crudHandler &
;& '
_hashService 
= 
hashService &
;& '
_tokenService 
= 
tokenService (
;( )
} 	
public 
IEnumerable 
< 
T 
> 
GetAllItems )
<) *
T* +
>+ ,
(, -
string- 3
	tableName4 =
)= >
{ 	
return 
_crudHandler 
.  
GetAllItems  +
<+ ,
T, -
>- .
(. /
	tableName/ 8
)8 9
;9 :
} 	
public 
IEnumerable 
< 
T 
>  
GetItemsByParameters 2
<2 3
T3 4
>4 5
(5 6
string6 <
	tableName= F
,F G

DictionaryH R
<R S
stringS Y
,Y Z
object[ a
>a b

parametersc m
)m n
{ 	
return 
_crudHandler 
.   
GetItemsByParameters  4
<4 5
T5 6
>6 7
(7 8
	tableName8 A
,A B

parametersC M
)M N
;N O
} 	
public 
T 
? %
GetSingleItemByParameters +
<+ ,
T, -
>- .
(. /
string/ 5
	tableName6 ?
,? @

DictionaryA K
<K L
stringL R
,R S
objectT Z
>Z [

parameters\ f
)f g
{   	
return!! 
_crudHandler!! 
.!!  %
GetSingleItemByParameters!!  9
<!!9 :
T!!: ;
>!!; <
(!!< =
	tableName!!= F
,!!F G

parameters!!H R
)!!R S
;!!S T
}"" 	
public$$ 
IEnumerable$$ 
<$$ 
T$$ 
>$$ )
GetSelectedParametersForItems$$ ;
<$$; <
T$$< =
>$$= >
($$> ?
string$$? E
	tableName$$F O
,$$O P
string$$Q W
columns$$X _
,$$_ `

Dictionary%% 
<%% 
string%% 
,%% 
object%% %
>%%% &

parameters%%' 1
)%%1 2
{&& 	
return'' 
_crudHandler'' 
.''  )
GetSelectedParametersForItems''  =
<''= >
T''> ?
>''? @
(''@ A
	tableName''A J
,''J K
columns''L S
,''S T

parameters''U _
)''_ `
;''` a
}(( 	
public** 
int** 

CreateItem** 
<** 
T** 
>**  
(**  !
string**! '
	tableName**( 1
,**1 2

Dictionary**3 =
<**= >
string**> D
,**D E
object**F L
>**L M

parameters**N X
)**X Y
{++ 	
return,, 
_crudHandler,, 
.,,  

CreateItem,,  *
(,,* +
	tableName,,+ 4
,,,4 5

parameters,,6 @
),,@ A
;,,A B
}-- 	
public// 
bool// #
CreateItemWithoutReturn// +
(//+ ,
string//, 2
	tableName//3 <
,//< =

Dictionary//> H
<//H I
string//I O
,//O P
object//Q W
>//W X

parameters//Y c
)//c d
{00 	
return11 
_crudHandler11 
.11  #
CreateItemWithoutReturn11  7
(117 8
	tableName118 A
,11A B

parameters11C M
)11M N
;11N O
}22 	
public44 
bool44 

UpdateItem44 
(44 
string44 %
	tableName44& /
,44/ 0

Dictionary441 ;
<44; <
string44< B
,44B C
object44D J
>44J K
conditionColumns44L \
,44\ ]

Dictionary55 
<55 
string55 
,55 
object55 %
>55% &
modifications55' 4
)554 5
{66 	
return77 
_crudHandler77 
.77  

UpdateItem77  *
(77* +
	tableName77+ 4
,774 5
conditionColumns776 F
,77F G
modifications77H U
)77U V
;77V W
}88 	
public:: 
bool:: 

DeleteItem:: 
(:: 
string:: %
	tableName::& /
,::/ 0
int::1 4
itemId::5 ;
)::; <
{;; 	
return<< 
_crudHandler<< 
.<<  

DeleteItem<<  *
(<<* +
	tableName<<+ 4
,<<4 5
itemId<<6 <
)<<< =
;<<= >
}== 	
public?? 
bool?? (
DeleteItemWithMultipleParams?? 0
(??0 1
string??1 7
	tableName??8 A
,??A B

Dictionary??C M
<??M N
string??N T
,??T U
object??V \
>??\ ]
conditionColumns??^ n
)??n o
{@@ 	
returnAA 
_crudHandlerAA 
.AA  (
DeleteItemWithMultipleParamsAA  <
(AA< =
	tableNameAA= F
,AAF G
conditionColumnsAAH X
)AAX Y
;AAY Z
}BB 	
publicDD 
stringDD 
?DD 
VerifyLoginDD "
(DD" #
stringDD# )
usernameDD* 2
,DD2 3
stringDD4 :
passwordDD; C
)DDC D
{EE 	
tryFF 
{GG 
varHH 

parametersHH 
=HH  
newHH! $

DictionaryHH% /
<HH/ 0
stringHH0 6
,HH6 7
objectHH8 >
>HH> ?
{HH@ A
{HHB C
$strHHD J
,HHJ K
usernameHHL T
}HHU V
}HHW X
;HHX Y
varII 
accountII 
=II 
_crudHandlerII *
.II* +%
GetSingleItemByParametersII+ D
<IID E
AccountVerificationIIE X
>IIX Y
(IIY Z
$strIIZ c
,IIc d

parametersIIe o
)IIo p
;IIp q
ifJJ 
(JJ 
accountJJ 
!=JJ 
nullJJ #
&&JJ$ &
_hashServiceJJ' 3
.JJ3 4
VerifyPasswordJJ4 B
(JJB C
accountJJC J
.JJJ K
PasswordJJK S
,JJS T
passwordJJU ]
)JJ] ^
)JJ^ _
{KK 
returnLL 
_tokenServiceLL (
.LL( )
GenerateTokenLL) 6
(LL6 7
newLL7 :
AccountLL; B
(LLB C
accountLLC J
.LLJ K
IdLLK M
,LLM N
accountLLO V
.LLV W
NameLLW [
,LL[ \
accountLL] d
.LLd e
EmailLLe j
)LLj k
)LLk l
;LLl m
}MM 
returnOO 
nullOO 
;OO 
}PP 
catchQQ 
(QQ 
	ExceptionQQ 
exQQ 
)QQ  
{RR 
ConsoleTT 
.TT 
	WriteLineTT !
(TT! "
$strTT" Q
+TTR S
exTTT V
.TTV W
MessageTTW ^
)TT^ _
;TT_ `
returnUU 
nullUU 
;UU 
}VV 
}WW 	
publicYY 
intYY 
CreateAccountYY  
(YY  !

DictionaryYY! +
<YY+ ,
stringYY, 2
,YY2 3
objectYY4 :
>YY: ;
accountDataYY< G
)YYG H
{ZZ 	
try[[ 
{\\ 
if]] 
(]] 
!]] 
accountData]]  
.]]  !
ContainsKey]]! ,
(]], -
$str]]- 3
)]]3 4
||]]5 7
!^^ 
accountData^^  
.^^  !
ContainsKey^^! ,
(^^, -
$str^^- 7
)^^7 8
||^^9 ;
!__ 
accountData__  
.__  !
ContainsKey__! ,
(__, -
$str__- 4
)__4 5
)__5 6
{`` 
throwaa 
newaa 

Exceptionsaa (
.aa( )'
InvalidAccountDataExceptionaa) D
(aaD E
$strbb E
)bbE F
;bbF G
}cc 
stringee 
passwordee 
=ee  !
accountDataee" -
[ee- .
$stree. 8
]ee8 9
?ee9 :
.ee: ;
ToStringee; C
(eeC D
)eeD E
??eeF H
throwff" '
newff( +

Exceptionsff, 6
.ff6 7'
InvalidAccountDataExceptionff7 R
(ffR S
$strffS l
)ffl m
;ffm n
stringhh 
hashedPasswordhh %
;hh% &
tryii 
{jj 
hashedPasswordkk "
=kk# $
_hashServicekk% 1
.kk1 2
HashPasswordkk2 >
(kk> ?
passwordkk? G
)kkG H
;kkH I
}ll 
catchmm 
(mm 
	Exceptionmm  
exmm! #
)mm# $
{nn 
throwoo 
newoo 

Exceptionsoo (
.oo( )$
PasswordHashingExceptionoo) A
(ooA B
$strpp G
,ppG H
exppI K
)ppK L
;ppL M
}qq 
accountDatass 
[ss 
$strss &
]ss& '
=ss( )
hashedPasswordss* 8
;ss8 9
returnuu 
_crudHandleruu #
.uu# $

CreateItemuu$ .
(uu. /
$struu/ 8
,uu8 9
accountDatauu: E
)uuE F
;uuF G
}vv 
catchww 
(ww 
	Exceptionww 
exww 
)ww  
{xx 
Consolezz 
.zz 
	WriteLinezz !
(zz! "
$strzz" O
+zzP Q
exzzR T
.zzT U
MessagezzU \
)zz\ ]
;zz] ^
throw{{ 
;{{ 
}|| 
}}} 	
public 
bool 
UpdateAccount !
(! "

Dictionary" ,
<, -
string- 3
,3 4
object5 ;
>; <
conditionColumns= M
,M N

DictionaryO Y
<Y Z
stringZ `
,` a
objectb h
>h i
modificationsj w
)w x
{
ÄÄ 	
try
ÅÅ 
{
ÇÇ 
if
ÉÉ 
(
ÉÉ 
modifications
ÉÉ !
.
ÉÉ! "
ContainsKey
ÉÉ" -
(
ÉÉ- .
$str
ÉÉ. 8
)
ÉÉ8 9
)
ÉÉ9 :
{
ÑÑ 
string
ÖÖ 
password
ÖÖ #
=
ÖÖ$ %
modifications
ÖÖ& 3
[
ÖÖ3 4
$str
ÖÖ4 >
]
ÖÖ> ?
?
ÖÖ? @
.
ÖÖ@ A
ToString
ÖÖA I
(
ÖÖI J
)
ÖÖJ K
??
ÖÖL N
throw
ÜÜ& +
new
ÜÜ, /

Exceptions
ÜÜ0 :
.
ÜÜ: ;)
InvalidAccountDataException
ÜÜ; V
(
ÜÜV W
$str
ÜÜW p
)
ÜÜp q
;
ÜÜq r
string
àà 
hashedPassword
àà )
;
àà) *
try
ââ 
{
ää 
hashedPassword
ãã &
=
ãã' (
_hashService
ãã) 5
.
ãã5 6
HashPassword
ãã6 B
(
ããB C
password
ããC K
)
ããK L
;
ããL M
}
åå 
catch
çç 
(
çç 
	Exception
çç $
ex
çç% '
)
çç' (
{
éé 
throw
èè 
new
èè !

Exceptions
èè" ,
.
èè, -&
PasswordHashingException
èè- E
(
èèE F
$str
èèF u
,
èèu v
ex
êê 
)
êê 
;
êê  
}
ëë 
modifications
ìì !
[
ìì! "
$str
ìì" ,
]
ìì, -
=
ìì. /
hashedPassword
ìì0 >
;
ìì> ?
}
îî 
return
ññ 
_crudHandler
ññ #
.
ññ# $

UpdateItem
ññ$ .
(
ññ. /
$str
ññ/ 8
,
ññ8 9
conditionColumns
ññ: J
,
ññJ K
modifications
ññL Y
)
ññY Z
;
ññZ [
}
óó 
catch
òò 
(
òò 
	Exception
òò 
ex
òò 
)
òò  
{
ôô 
Console
õõ 
.
õõ 
	WriteLine
õõ !
(
õõ! "
$str
õõ" M
+
õõN O
ex
õõP R
.
õõR S
Message
õõS Z
)
õõZ [
;
õõ[ \
throw
úú 
;
úú 
}
ùù 
}
ûû 	
}
üü 
}†† “

.C:\tempProjNew\service\Services\HashService.cs
	namespace 	
service
 
. 
Services 
; 
public 
class 
HashService 
: 
IHashService '
{ 
public 

string 
HashPassword 
( 
string %
password& .
). /
{		 
var

 
salt

 
=

 
Crypter

 
.

 
Blowfish

 #
.

# $
GenerateSalt

$ 0
(

0 1
)

1 2
;

2 3
var 
hashed 
= 
Crypter 
. 
Blowfish %
.% &
Crypt& +
(+ ,
password, 4
,4 5
salt6 :
): ;
;; <
return 
hashed 
; 
} 
public 

bool 
VerifyPassword 
( 
string %
hashedPassword& 4
,4 5
string6 <
rawPassword= H
)H I
{ 
return 
Crypter 
. 
CheckPassword $
($ %
rawPassword% 0
,0 1
hashedPassword2 @
)@ A
;A B
} 
} ø

.C:\tempProjNew\service\Services\JwtSettings.cs
	namespace 	
service
 
. 
Services 
; 
public 
class 
JwtSettings 
{ 
public 

string 
Key 
{ 
get 
; 
set  
;  !
}" #
=$ %
string& ,
., -
Empty- 2
;2 3
public 

string 
Issuer 
{ 
get 
; 
set  #
;# $
}% &
=' (
string) /
./ 0
Empty0 5
;5 6
public 

string 
Audience 
{ 
get  
;  !
set" %
;% &
}' (
=) *
string+ 1
.1 2
Empty2 7
;7 8
public 

int 
ExpirationMinutes  
{! "
get# &
;& '
set( +
;+ ,
}- .
public		 

string		 
Pepper		 
{		 
get		 
;		 
set		  #
;		# $
}		% &
=		' (
string		) /
.		/ 0
Empty		0 5
;		5 6
}

 ´
/C:\tempProjNew\service\Services\TokenService.cs
	namespace 	
service
 
. 
Services 
{		 
public

 

class

 
TokenService

 
:

 
ITokenService

  -
{ 
private 
readonly 
JwtSettings $
_jwtSettings% 1
;1 2
public 
TokenService 
( 
JwtSettings '
jwtSettings( 3
)3 4
{ 	
_jwtSettings 
= 
jwtSettings &
;& '
} 	
public 
string 
? 
GenerateToken $
($ %
Account% ,
account- 4
)4 5
{ 	
var 
securityKey 
= 
new ! 
SymmetricSecurityKey" 6
(6 7
Encoding7 ?
.? @
UTF8@ D
.D E
GetBytesE M
(M N
_jwtSettingsN Z
.Z [
Key[ ^
)^ _
)_ `
;` a
var 
credentials 
= 
new !
SigningCredentials" 4
(4 5
securityKey5 @
,@ A
SecurityAlgorithmsB T
.T U

HmacSha256U _
)_ `
;` a
var 
claims 
= 
new 
[ 
] 
{ 
new 
Claim 
( #
JwtRegisteredClaimNames 1
.1 2
Sub2 5
,5 6
account7 >
.> ?
Name? C
)C D
,D E
new 
Claim 
( #
JwtRegisteredClaimNames 1
.1 2
Email2 7
,7 8
account9 @
.@ A
EmailA F
)F G
,G H
new 
Claim 
( #
JwtRegisteredClaimNames 1
.1 2
Jti2 5
,5 6
Guid7 ;
.; <
NewGuid< C
(C D
)D E
.E F
ToStringF N
(N O
)O P
)P Q
,Q R
new 
Claim 
( 
$str 
, 
account  '
.' (
Id( *
.* +
ToString+ 3
(3 4
)4 5
)5 6
} 
; 
var   
token   
=   
new   
JwtSecurityToken   ,
(  , -
issuer!! 
:!! 
_jwtSettings!! $
.!!$ %
Issuer!!% +
,!!+ ,
audience"" 
:"" 
_jwtSettings"" &
.""& '
Audience""' /
,""/ 0
claims## 
:## 
claims## 
,## 
expires$$ 
:$$ 
DateTime$$ !
.$$! "
Now$$" %
.$$% &

AddMinutes$$& 0
($$0 1
_jwtSettings$$1 =
.$$= >
ExpirationMinutes$$> O
)$$O P
,$$P Q
signingCredentials%% "
:%%" #
credentials%%$ /
)&& 
;&& 
return(( 
new(( #
JwtSecurityTokenHandler(( .
(((. /
)((/ 0
.((0 1

WriteToken((1 ;
(((; <
token((< A
)((A B
;((B C
})) 	
}** 
}++ 