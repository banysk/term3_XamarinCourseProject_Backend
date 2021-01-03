# XamarinCourseProject_Backend
## Server
=/=/=/=/=/=/=/=/=/=
## DataBase
### :star2:Main Tables
#### users:white_check_mark:    
[register_user (function)](#register_userwhite_check_mark)    
[find_user_with_login (function)](#find_user_with_loginwhite_check_mark)    
[find_user_with_personal_data (function)](#find_user_with_personal_datawhite_check_mark)    
#### auth:white_check_mark:    
[register_user (function)](#register_userwhite_check_mark)    
[change_login (function)](#change_loginwhite_check_mark)    
[change_password (function)](#change_passwordwhite_check_mark)    
[get_password (function)](#get_passwordwhite_check_mark)    
[trigger_auth_changed (trigger)](#trigger_auth_changedwhite_check_mark)    
#### bills
create_bill (function)    
get_bills (function)    
do_operation (function)    
trigger_bills_changed (trigger)    
#### patterns | created | ...   

### :star2:Change&History Tables
#### auth_changes:white_check_mark:  
#### auth_history | created | ...    
#### bills_history | created | ...   

### :star2:Type Tables
#### change_types:white_check_mark:    
#### currency_types:white_check_mark:    
#### bill_types:white_check_mark:    
#### operation_types:white_check_mark:    
 
### :star2:Functions
#### register_user:white_check_mark:
args:    
new_first_name VARCHAR(30),    
new_surname VARCHAR(30),    
new_date_of_birth VARCHAR(10),    
new_phone VARCHAR(10),    
new_pass_series VARCHAR(6),    
new_pass_number VARCHAR(8),    
new_login VARCHAR(16),    
new_user_password VARCHAR(16),    
new_patronymic VARCHAR(30) default NULL    
return:    
0 - success    
1 - already registered    
2 - login is already taken    
____
#### find_user_with_login:white_check_mark:    
args:    
input_login VARCHAR(16)    
return:    
{    
login VARCHAR(16),    
first_name VARCHAR(30),    
surname VARCHAR(30),    
patronymic VARCHAR(30),    
date_of_birth VARCHAR(10),    
phone VARCHAR(10),    
pass_series VARCHAR(6),    
pass_number VARCHAR(8)    
}    
____
#### find_user_with_personal_data:white_check_mark:    
args:    
input_first_name VARCHAR(30),    
input_surname VARCHAR(30),    
input_pass_series VARCHAR(6),    
input_pass_number VARCHAR(8)    
return:    
{    
login VARCHAR(16),    
first_name VARCHAR(30),    
surname VARCHAR(30),    
patronymic VARCHAR(30),    
date_of_birth VARCHAR(10),    
phone VARCHAR(10),    
pass_series VARCHAR(6),    
pass_number VARCHAR(8)    
}    
____
#### change_login:white_check_mark:    
args:    
input_login VARCHAR(16),    
new_login VARCHAR(16)    
return:    
0 - success    
1 - logins are equal    
2 - login is already taken    
3 - wrong login   
____
#### change_password:white_check_mark:    
args:    
input_login VARCHAR(16),    
new_password VARCHAR(16)    
return:    
0 - success    
1 - passwords are equal    
2 - wrong login   
____
#### get_password:white_check_mark:    
args:    
input_login VARCHAR(16)    
return:    
user_password VARCHAR(16) - success    
ERR - wrong login    
____
#### create_bill
args:   
return:
____
#### get_bills
args:   
return:
____
#### do_operation
args:   
return:
____

### :star2:Triggers
#### trigger_auth_changed:white_check_mark:     
inserts info about changing login or password into AUTH_CHANGES    
____
#### trigger_bills_changed
