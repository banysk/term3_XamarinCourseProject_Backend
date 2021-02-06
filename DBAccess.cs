using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using System.Threading.Tasks;

namespace BackEnd
{
    // Основной класс для общения с базой данных
    class DBAccess
    {
        // переменные 
        private readonly string cs;
        private readonly NpgsqlConnection con;
        private NpgsqlCommand cmd;
        private NpgsqlDataReader rdr;

        // конструктор
        public DBAccess(string SettingsFile)
        {
            string line;
            List<string> args = new List<string>();
            StreamReader file = new StreamReader(SettingsFile);
            while ((line = file.ReadLine()) != null)
            {
                args.Add(line);
            }
            cs = $"Host={args[0]};" +
                $"Username={args[1]};" +
                $"Password={args[2]};" +
                $"Database={args[3]}";
            con = new NpgsqlConnection(cs);
            con.Open();
            Console.WriteLine("OK");
        }

        // деструктор
        ~DBAccess()
        {
            con.CloseAsync();
        }

        // логин
        async public Task<string> Login(string login, string password)
        {
            try
            {
                string sql = $"SELECT * FROM LOGIN('{login}','{password}')";
                cmd = new NpgsqlCommand(sql, con);
                var result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
            catch(Exception ex)
            {
                WriteException(ex.Message);
                return ex.Message;
            }
        }

        // получаем данные пользователя
        async public Task<User> GetUserData(string login)
        {
            try
            {
                string sql = $"SELECT * FROM GET_USER_DATA('{login}')";
                cmd = new NpgsqlCommand(sql, con);
                rdr = await cmd.ExecuteReaderAsync();
                await rdr.ReadAsync();
                User user =  new User
                {
                    id = rdr.GetInt32(0),
                    firstname = rdr.GetString(1),
                    surnamme = rdr.GetString(2),
                    patronymic = rdr.GetString(3),
                    dob = rdr.GetString(4),
                    phone = rdr.GetString(5),
                    pseries = rdr.GetString(6),
                    pnumber = rdr.GetString(7),
                };
                await rdr.CloseAsync();
                return user;
            }
            catch(Exception ex)
            {
                WriteException(ex.Message);
                return new User();
            }
        }

        // добавляем посещение
        async public Task<string> AddVisit(string login)
        {
            try
            {
                string sql = $"SELECT * FROM ADD_VISIT('{login}')";
                cmd = new NpgsqlCommand(sql, con);
                var result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
            catch(Exception ex)
            {
                WriteException(ex.Message);
                return ex.Message;
            }
        }

        async public Task<List<Card>> GetCards(string login)
        {
            try
            {
                string sql = $"SELECT * FROM GET_CARDS('{login}')";
                cmd = new NpgsqlCommand(sql, con);
                List<Card> cards = new List<Card>();
                rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    cards.Add(new Card
                    {
                        card_name = rdr.GetString(0),
                        card_number = rdr.GetString(1),
                        card_currency = rdr.GetString(2),
                        card_type = rdr.GetString(3),
                        balance = rdr.GetDouble(4),
                        card_blocked = rdr.GetBoolean(5)
                    });
                }
                await rdr.CloseAsync();
                return cards;
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return new List<Card>();
            }
        }

        // получаем счета
        async public Task<List<Bill>> GetBills(string login)
        {
            try
            {
                string sql = $"SELECT * FROM GET_BILLS('{login}')";
                cmd = new NpgsqlCommand(sql, con);
                List<Bill> bills = new List<Bill>();
                rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    bills.Add(new Bill
                    {
                        bill_number = rdr.GetString(0),
                        bill_currency = rdr.GetString(1),
                        balance = rdr.GetDouble(2)
                    });
                }
                await rdr.CloseAsync();
                return bills;
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return new List<Bill>();
            }
        }

        // получаем кредиты
        async public Task<List<Credit>> GetCredits(string login)
        {
            try
            {
                string sql = $"SELECT * FROM GET_CREDITS('{login}')";
                cmd = new NpgsqlCommand(sql, con);
                List<Credit> credits = new List<Credit>();
                rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    credits.Add(new Credit
                    {
                        credit_number = rdr.GetString(0),
                        credit_currency = rdr.GetString(1),
                        balance = rdr.GetDouble(2),
                        pay_time = rdr.GetTimeStamp(3).ToString()
                    });
                }
                await rdr.CloseAsync();
                return credits;
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return new List<Credit>();
            }
        }

        // меняем логин
        async public Task<string> ChangeLogin(string login, string new_login)
        {
            try
            {
                string sql = $"SELECT * FROM CHANGE_LOGIN('{login}', '{new_login}')";
                cmd = new NpgsqlCommand(sql, con);
                var result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return ex.Message;
            }
        }

        // меняем пароль
        async public Task<string> ChangePassword(string login, string new_password)
        {
            try
            {
                string sql = $"SELECT * FROM CHANGE_PASSWORD('{login}', '{new_password}')";
                cmd = new NpgsqlCommand(sql, con);
                var result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return ex.Message;
            }
        }

        // получаем историю входов
        async public Task<List<string>> GetAuthHistory(string login)
        {
            try
            {
                string sql = $"SELECT * FROM GET_AUTH_HISTORY('{login}')";
                cmd = new NpgsqlCommand(sql, con);
                List<string> info = new List<string>();
                rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    info.Add(rdr.GetTimeStamp(0).ToString());
                }
                await rdr.CloseAsync();
                return info;
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return new List<string>();
            }
        }

        // можно перевести?
        async public Task<string> CanTransferTo(string number)
        {
            try
            {
                string sql = $"SELECT * FROM CAN_TRANSFER_TO('{number}')";
                cmd = new NpgsqlCommand(sql, con);
                var result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return ex.Message;
            }
        }

        // получаем всю историю
        async public Task<List<Operation>> GetHistory(string login)
        {
            try
            {
                string sql = $"SELECT * FROM GET_HISTORY('{login}')";
                cmd = new NpgsqlCommand(sql, con);
                List<Operation> info = new List<Operation>();
                rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    info.Add(new Operation()
                    {
                        user_number = rdr.GetString(0),
                        other_number = rdr.GetString(1),
                        amount = rdr.GetDouble(2),
                        operation_currency = rdr.GetString(3),
                        operation_type = rdr.GetString(4),
                        operation_time = rdr.GetTimeStamp(5).ToString(),
                    });
                }
                await rdr.CloseAsync();
                return info;
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return new List<Operation>();
            }
        }

        // получаем историю по номеру
        async public Task<List<Operation>> GetPartHistory(string number)
        {
            try
            {
                string sql = $"SELECT * FROM GET_PART_HISTORY('{number}')";
                cmd = new NpgsqlCommand(sql, con);
                List<Operation> info = new List<Operation>();
                rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    info.Add(new Operation()
                    {
                        user_number = rdr.GetString(0),
                        other_number = rdr.GetString(1),
                        amount = rdr.GetDouble(2),
                        operation_currency = rdr.GetString(3),
                        operation_type = rdr.GetString(4),
                        operation_time = rdr.GetTimeStamp(5).ToString(),
                    });
                }
                await rdr.CloseAsync();
                return info;
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return new List<Operation>();
            }
        }

        // выполняем перевод
        async public Task<string> DoTransfer(string start_number, string target_number, string amount)
        {
            try
            {
                string sql = $"SELECT * FROM DO_TRANSFER('{start_number}', '{target_number}', {amount})";
                cmd = new NpgsqlCommand(sql, con);
                var result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return ex.Message;
            }
        }

        // блокируем карту
        async public Task<string> BlockCard(string number)
        {
            try
            {
                string sql = $"SELECT * FROM BLOCK_CARD('{number}')";
                cmd = new NpgsqlCommand(sql, con);
                var result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return ex.Message;
            }
        }

        // переименовываем карту
        async public Task<string> RenameCard(string number, string name)
        {
            try
            {
                string sql = $"SELECT * FROM RENAME_CARD('{number}', '{name}')";
                cmd = new NpgsqlCommand(sql, con);
                var result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return ex.Message;
            }
        }

        // получаем шаблоны
        async public Task<List<Pattern>> GetPatterns(string login)
        {
            try
            {
                string sql = $"SELECT * FROM GET_PATTERNS('{login}')";
                cmd = new NpgsqlCommand(sql, con);
                List<Pattern> patterns = new List<Pattern>();
                rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    patterns.Add(new Pattern
                    {
                        pattern_name = rdr.GetString(0),
                        from_ = rdr.GetString(1),
                        to_ = rdr.GetString(2),
                        amount = rdr.GetDouble(3)
                    });
                }
                await rdr.CloseAsync();
                return patterns;
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return new List<Pattern>();
            }
        }

        // создаём шаблон
        async public Task<string> CreatePattern(string login, string name, string from, string to, string amount)
        {
            try
            {
                string sql = $"SELECT * FROM CREATE_PATTERN('{login}', '{name}', '{from}', '{to}', {amount})";
                cmd = new NpgsqlCommand(sql, con);
                var result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return ex.Message;
            }
        }

        // удаляем шаблон
        async public Task<string> RemovePattern(string login, string name)
        {
            try
            {
                string sql = $"SELECT * FROM REMOVE_PATTERN('{login}', '{name}')";
                cmd = new NpgsqlCommand(sql, con);
                var result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return ex.Message;
            }
        }

        // отправляем сообщение
        async public Task<string> SendMessage(string from_, string to_, string msg)
        {
            try
            {
                string sql = $"SELECT * FROM SEND_MESSAGE('{from_}', '{to_}', '{msg}')";
                cmd = new NpgsqlCommand(sql, con);
                var result = await cmd.ExecuteScalarAsync();
                return result.ToString();
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return ex.Message;
            }
        }

        // получаем сообщения
        async public Task<List<Message>> GetMessages(string login)
        {
            try
            {
                string sql = $"SELECT * FROM GET_MESSAGES('{login}')";
                cmd = new NpgsqlCommand(sql, con);
                List<Message> messages = new List<Message>();
                rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    messages.Add(new Message
                    {
                        from_ = rdr.GetString(0),
                        to_ = rdr.GetString(1),
                        msg = rdr.GetString(2),
                        msg_time = rdr.GetTimeStamp(3).ToString()
                    });
                }
                await rdr.CloseAsync();
                return messages;
            }
            catch (Exception ex)
            {
                WriteException(ex.Message);
                return new List<Message>();
            }
        }

        // вывод ошибок
        public void WriteException(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}