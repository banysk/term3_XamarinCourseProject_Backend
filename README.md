# XamarinCourseProject_Backend
## Server
=/=/=/=/=/=/=/=/=/=
## DataBase
### :star2:Main Tables
#### users:white_check_mark:    
<details>
 
  [register_user (function)](#register_userwhite_check_mark)    
  [find_user_with_login (function)](#find_user_with_loginwhite_check_mark)    
  [find_user_with_personal_data (function)](#find_user_with_personal_datawhite_check_mark)  
  
</details>    
  
#### auth:white_check_mark:    
<details>
 
  [register_user (function)](#register_userwhite_check_mark)    
  [change_login (function)](#change_loginwhite_check_mark)    
  [change_password (function)](#change_passwordwhite_check_mark)    
  [get_password (function)](#get_passwordwhite_check_mark)    
  [trigger_auth_changed (trigger)](#trigger_auth_changedwhite_check_mark)    
  
</details> 

#### bills:white_check_mark: 
<details>
 
  [create_bill (function)](#create_billwhite_check_mark)    
  [get_bills (function)](#get_billswhite_check_mark)    
  [do_operation (function)](#do_operationwhite_check_mark)    
  
</details> 
   
#### patterns:white_check_mark:
<details>
 
  [create_pattern (function)](#create_patternwhite_check_mark)   
  [get_patterns (function)](#get_patternswhite_check_mark)
  
</details> 

### :star2:Change&History Tables
#### auth_changes:white_check_mark:  
#### auth_history:white_check_mark:
<details>
 
  [add_visit (function)](#add_visitwhite_check_mark)    
  [get_auth_history (function)](#get_auth_historywhite_check_mark)
  
</details> 

#### bills_history:white_check_mark:
<details>

  [get_bills_history (function)](#get_bills_historywhite_check_mark)

</details> 

### :star2:Type Tables
#### change_types:white_check_mark:    
#### currency_types:white_check_mark:    
#### bill_types:white_check_mark:    
#### operation_types:white_check_mark:    
 
### :star2:Functions
#### register_user:white_check_mark:
<details>
  <summary>args</summary>
  
  new_first_name VARCHAR(30),    
  new_surname VARCHAR(30),    
  new_date_of_birth VARCHAR(10),    
  new_phone VARCHAR(10),    
  new_pass_series VARCHAR(6),    
  new_pass_number VARCHAR(8),    
  new_login VARCHAR(16),    
  new_user_password VARCHAR(16),    
  new_patronymic VARCHAR(30) default NULL  
  
</details>    
<details>
  <summary>return</summary>
  
  0 - success    
  1 - already registered    
  2 - login is already taken   
  
</details>

***
#### find_user_with_login:white_check_mark:  
<details>
  <summary>args</summary>
  
  input_login VARCHAR(16)   
  
</details>    
<details>
  <summary>return</summary>
  
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
  
</details>

***
#### find_user_with_personal_data:white_check_mark:  
<details>
  <summary>args</summary>
  
  input_first_name VARCHAR(30),    
  input_surname VARCHAR(30),    
  input_pass_series VARCHAR(6),    
  input_pass_number VARCHAR(8)  
  
</details>  
<details>
  <summary>return</summary>
  
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
  
</details>

***
#### change_login:white_check_mark:    
<details>
  <summary>args</summary>
  
  input_login VARCHAR(16),    
  new_login VARCHAR(16)    
  
</details>  
<details>
  <summary>return</summary>
  
  0 - success    
  1 - logins are equal    
  2 - login is already taken    
  3 - wrong login  
  
</details>

***
#### change_password:white_check_mark:    
<details>
  <summary>args</summary>
  
  input_login VARCHAR(16),    
  new_password VARCHAR(16)  
  
</details>
<details>
  <summary>return</summary>
  
  0 - success    
  1 - passwords are equal    
  2 - wrong login 
  
</details>

***
#### get_password:white_check_mark:    
<details>
  <summary>args</summary>
  
  input_login VARCHAR(16)  
  
</details>
<details>
  <summary>return</summary>
  
  user_password VARCHAR(16) - success    
  ERR - wrong login   
  
</details>

***
#### create_bill:white_check_mark:
<details>
  <summary>args</summary>
  
  input_bill_number VARCHAR(20),    
  input_user_id INT,    
  input_currency VARCHAR(3),    
  input_balance INT,    
  input_bill_type VARCHAR(10)    
  
</details>
<details>
  <summary>return</summary>
  
  0 - success    
  1 - bill exists
  
</details>

***
#### get_bills:white_check_mark:
<details>
  <summary>args</summary>
  
  input_user_id INT
  
</details>
<details>
  <summary>return</summary>
  
  {    
  bill_number VARCHAR(20),    
	 currency VARCHAR(3),    
	 bill_type VARCHAR(10),    
	 balance INT    
  }    
  
</details>

***
#### do_operation:white_check_mark:  
<details>
  <summary>args</summary>
	
  input_bill_from VARCHAR(20),    
  input_bill_to VARCHAR(20),    
  input_amount INT    
  
</details>
<details>
  <summary>return</summary>
  
  0 - success    
  1 - wrong bills    
  2 - wrong currency    
  
</details>

***
#### create_pattern:white_check_mark:    
<details>
  <summary>args</summary>
  
  input_user_id INT,    
  input_pattern_name VARCHAR(20),   
  input_bill_number VARCHAR(20),    
  input_amount INT
  
</details>
<details>
  <summary>return</summary>
  
  0 - success     
  1 - exists     
  2 - wrong bill number
  
</details>

***
#### get_patterns:white_check_mark:  
<details>
  <summary>args</summary>
  
  input_user_id INT
  
</details>
<details>
  <summary>return</summary>
  
  {    
  pattern_name VARCHAR(20),    
  bill_number VARCHAR(20),    
  amount INT    
  }
  
</details>

***
#### add_visit:white_check_mark:
<details>
  <summary>args</summary>
  
  input_user_id INT
  
</details>
<details>
  <summary>return</summary>
  
  0 - success
  
</details>

***
#### get_auth_history:white_check_mark:
<details>
  <summary>args</summary>
  
  input_user_id INT
  
</details>
<details>
  <summary>return</summary>
  
  {      
  visit_time TIMESTAMP    
  }    
  
</details>

***
#### get_bills_history:white_check_mark:
<details>
  <summary>args</summary>
  
  input_user_id INT
  
</details>
<details>
  <summary>return</summary>
  
  {      
  move_from INT, -- Откуда    
  move_to INT, -- Куда    
  bill_type INT -- Тип операции    
  }    
  
</details>  

### :star2:Triggers
#### trigger_auth_changed:white_check_mark:     
inserts info about changing login or password into AUTH_CHANGES    
