using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Collections.ObjectModel;

namespace Ice_Arena_ASU
{
    public enum Operation
    {
        Expense,
        Income
    }
    public class db
    {
        private string DbFile { get; } = "sqlite.db";
        private const string SqlForCreateon = @"SQLDBCreate.sql";
        SQLiteConnection _conn;

        /// <summary>
        /// При создании класса, сразу подключаем.(если базы нет, он ее создаст)
        /// </summary>
        public db()
        {
            if (!File.Exists(DbFile)) Initialization();
            else Connect();
        }
        public void Connect()
        {
            try
            {
                _conn = new SQLiteConnection($"Data Source={DbFile}; Version=3;");
                _conn.Open();

                var cmd = _conn.CreateCommand();
                cmd.CommandText = "PRAGMA foreign_keys = 1";
                cmd.ExecuteNonQuery();

            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void Disconnect()
        {
            _conn.Close();
            _conn.Dispose();
        }
        public void CreateDefaultDb()
        {
            var sql = File.ReadAllText(SqlForCreateon);

            Connect();
            var cmd = _conn.CreateCommand();
            cmd.CommandText = sql;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void Initialization()
        {   
            Console.WriteLine("Start Initialization");

            Console.WriteLine("CreateDB");
            CreateDefaultDb();

            AddOperation(Operation.Expense.ToString());
            AddOperation(Operation.Income.ToString());

            Console.WriteLine("Finish Initialization");
        }
        private void AddOperation(string name)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = $"INSERT INTO operation (name) VALUES('{name}')";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void AddTransaction(Operation operation, string name, decimal amount, DateTime date)
        {
            var operation_id = GetOperationIdByName(operation.ToString());
            var cmd = _conn.CreateCommand();
            cmd.CommandText = $"INSERT INTO _transaction (transaction_date, operation_id, name, amount) VALUES('{date:yyyy-MM-dd}',{operation_id},'{name}',{amount})";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public List<string> GetTransactionsByPeriod(DateTime from, DateTime to)
        {
            var transactions = new List<string>();
            var cmd = _conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM _transaction WHERE transaction_date >= '{@from:yyyy-MM-dd HH:mm:ss.fff}' and transaction_date <= '{to:yyyy-MM-dd HH:mm:ss.fff}'";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read() && reader.HasRows)
                {
                    var transaction = $"{reader["id"]},{reader["transaction_date"]},{reader["operation_id"]},{reader["name"]},{reader["amount"]}";
                    transactions.Add(transaction);
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return transactions;
        }

        internal void DeleteById(string text)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = $"DELETE FROM _transaction WHERE id = {text}";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        internal void DeleteAll()
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = $"DELETE FROM _transaction";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public ObservableCollection<Transaction> GetIncomes(DateTime from , DateTime to)
        {
            var transactions = new ObservableCollection<Transaction>();
            var cmd = _conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM _transaction WHERE operation_id = {GetOperationIdByName(Operation.Income.ToString())} and " +
                              $"transaction_date >= '{@from:yyyy-MM-dd}' and transaction_date <= '{to:yyyy-MM-dd}'";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read() && reader.HasRows)
                {
                    var transaction = new Transaction(
                        reader["id"].ToString(),
                        reader["name"].ToString(),
                        reader["amount"].ToString(),
                        reader["transaction_date"].ToString());

                    transactions.Add(transaction);
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return transactions;
        }

        public ObservableCollection<Transaction> GetExpenses(DateTime from, DateTime to)
        {
            var transactions = new ObservableCollection<Transaction>();
            var cmd = _conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM _transaction WHERE operation_id = {GetOperationIdByName(Operation.Expense.ToString())} and " +
                              $"transaction_date >= '{@from:yyyy-MM-dd}' and transaction_date <= '{to:yyyy-MM-dd}'";
            try
            {
                var reader = cmd.ExecuteReader();
                while (reader.Read() && reader.HasRows)
                {
                    var transaction = new Transaction(
                        reader["id"].ToString(),
                        reader["name"].ToString(),
                        reader["amount"].ToString(),
                        reader["transaction_date"].ToString());

                    transactions.Add(transaction);
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return transactions;
        }

        public int GetOperationIdByName(string name)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = $"SELECT id FROM operation WHERE name = '{name}'";
            try
            {
                var reader = cmd.ExecuteReader();

                while (reader.Read() && reader.HasRows)
                {
                    return Convert.ToInt32(reader["id"].ToString());
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }
    }
}
