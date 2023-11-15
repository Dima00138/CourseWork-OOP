using CourseWork.View;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseWork.Model
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? Get(string id);
        void Create(T item);
        void Update(T item, string columnName, string newVal);
        void Delete(T item, string id);

    }
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly OracleContext _context;
        protected readonly ObservableCollection<T> _entities;
        protected const string Admin = "MANAGER";
        public Repository(OracleContext context)
        {
            _context = context;
            _entities = new ObservableCollection<T>();
        }
        public virtual void Create(T item)
        {
            
        }
        public void Update(T item, string columnName, string newVal)
        {

        }

        public virtual void Delete(T item, string id)
        {

        }

        public virtual T? Get(string id)
        {
            return null;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _entities;
        }
    }

    public class ScheduleRepository : Repository<Schedule>
    {
        public ScheduleRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.conn.Open();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.SCHEDULE";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                Schedule temp = new Schedule();
                while (reader.Read())
                {
                    temp.Id = (long)reader["ID"];
                    temp.IdTrain = (long)reader["Id_Train"];
                    temp.Date = (DateTime)reader["Date"];
                    temp.Route = (long)reader["Route"];
                    temp.Frequency = (short)reader["Frequency"];
                    _entities.Add(temp);
                }
                reader.Close();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Нет соединения с базой данных");
            }
        }

        public virtual void Create(Schedule item)
        {
                int result = 0;
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"EXEC {Admin}.INSERT_SCHEDULE(" +
                    $":idTrain, :date, :route, :frequency, :result)";
                insert.Parameters.Add(":idTrain", item.IdTrain);
                insert.Parameters.Add(":date", item.Date);
                insert.Parameters.Add(":route", item.Route);
                insert.Parameters.Add(":frequency", item.Frequency);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show($"Schedule.Create Exception," +
                    $"Insert {Convert.ToBoolean(result)}");
            }
        }

        public void Update(Schedule item, string columnName, string newVal)
        {
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.SCHEDULE SET {columnName} = {newVal}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Schedule.Update Exception");
            }
        }

        public virtual void Delete(Schedule item, string id)
        {
            try
            {
                _entities.Remove(item);
                _context.conn.Open();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.SCHEDULE WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Schedule.Delete Exception");
            }
        }

        public virtual Schedule? Get(long id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public virtual IEnumerable<Schedule> GetAll()
        {
            return _entities;
        }

        public virtual DataView TakeSchedule()
        {
            try
            {
                DataSet ds = new DataSet();
                _context.conn.Open();
                OracleCommand take = _context.conn.CreateCommand();
                take.CommandText = $"SELECT * FROM {Admin}.TAKE_SCHEDULE";
                OracleDataAdapter adapter = new OracleDataAdapter(take);
                adapter.Fill(ds, "ScheduleList");

                return ds.Tables["ScheduleList"].DefaultView;
            }
            catch
            {
                MessageBox.Show("Schedule.TakeSchedule Exception");
                return null;
            }
        }
    }
}
