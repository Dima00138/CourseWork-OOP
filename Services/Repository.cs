using CourseWork.View;
using MaterialDesignThemes.Wpf;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CourseWork.Model
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        void GetAll(int RowMin, int RowMax, string Order, ObservableCollection<T> Items);
        T? Get(int id);
        void Create(T item);
        void Update(T item);
        void Update(T item, string columnName, string newVal);
        void Delete(T item, int Id);

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
        public virtual void Update(T item)
        {

        }

        public virtual void Update(T item, string columnName, string newVal)
        {

        }

        public virtual void Delete(T item, int id)
        {

        }

        public virtual T? Get(int id)
        {
            return null;
        }

        public virtual IEnumerable<T> GetAll()
        {
            
            return _entities;
        }

        public virtual void GetAll(int RowMin, int RowMax, string Order, ObservableCollection<T> Items)
        {
            
        }
    }

    public class ScheduleRepository : Repository<Schedule>
    {
        public ScheduleRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.SCHEDULE";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Schedule temp = new Schedule();
                    temp.Id = reader.GetInt32(0);
                    temp.IdTrain = reader.GetInt64(1);
                    temp.Route = reader.GetInt64(3);
                    temp.Date = reader.GetDateTime(2);
                    temp.SetFrequency(reader.GetInt16(4));
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

        public override void Create(Schedule item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"CALL {Admin}.INSERT_SCHEDULE(" +
                    $":idTrain, :dat, :route, :frequency)";
                insert.Parameters.Add(":idTrain", item.IdTrain);
                insert.Parameters.Add(":dat", OracleDbType.Date, item.Date, ParameterDirection.Input);
                insert.Parameters.Add(":route", item.Route);
                insert.Parameters.Add(":frequency", item.GetFrequency());
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Schedule.Create Exception," +
                    $"{ex.Message}");
            }
        }

        public override void Update(Schedule item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"CALL {Admin}.UPDATE_SCHEDULE(" +
                    $":id, :idTrain, :dat, :route, :frequency)";
                update.Parameters.Add(":id", item.Id);
                update.Parameters.Add(":idTrain", item.IdTrain);
                update.Parameters.Add(":dat", OracleDbType.Date, item.Date, ParameterDirection.Input);
                update.Parameters.Add(":route", item.Route);
                update.Parameters.Add(":frequency", item.GetFrequency());
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Schedule.Update Exception {ex.Message}");
            }
        }

        public override void Update(Schedule item, string columnName, string newVal)
        {
            try
            {
                string convertedString = string.Concat(columnName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                if (convertedString.Contains("DATE"))
                {
                    DateTime dateValue;
                    DateTime.TryParseExact(newVal, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                    if (dateValue == DateTime.MinValue)
                    {
                        MessageBox.Show("Неверно введена дата,\n Правильный формат: 'dd/MM/yyyy hh:mm tt'", "Ошибка в дате", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                        return;
                    }
                    update.CommandText = $"UPDATE {Admin}.SCHEDULE SET \"{convertedString}\" = TO_DATE('{dateValue.ToString("dd/MM/yyyy HH:mm")}', 'DD/MM/YYYY HH24:MI') WHERE ID = {item.Id}";
                }
                else if (convertedString.Contains("FREQUENCY"))
                {
                    update.CommandText = $"UPDATE {Admin}.SCHEDULE SET \"{convertedString}\" = {item.GetFrequency()} WHERE ID = {item.Id}";
                }
                else update.CommandText = $"UPDATE {Admin}.SCHEDULE SET {convertedString} = '{newVal}' WHERE ID = {item.Id}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Schedule.Update Exception {ex.Message}");
            }
        }


        public override void Delete(Schedule item, int Id)
        {
            try
            {
                _entities.Remove(item);
                _context.SaveOpen();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.SCHEDULE WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Schedule.Delete Exception {ex.Message}");
            }
        }

        public override Schedule? Get(int id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public override IEnumerable<Schedule> GetAll()
        {
            return _entities;
        }

        public override void GetAll(int RowMin, int RowMax, string Order, ObservableCollection<Schedule> Items)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.SCHEDULE WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Schedule temp = new Schedule();
                    temp.Id = reader.GetInt32(0);
                    temp.IdTrain = reader.GetInt64(1);
                    temp.Route = reader.GetInt64(3);
                    temp.Date = reader.GetDateTime(2);
                    temp.SetFrequency(reader.GetInt16(4));
                    Items.Add(temp);
                }
                reader.Close();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Нет соединения с базой данных");
            }
        }

        public virtual DataView TakeSchedule()
        {
            try
            {
                DataSet ds = new DataSet();
                _context.SaveOpen();
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

        public void TakeSchedule_User(int RowMin, int RowMax, string Order, ObservableCollection<UserSchedule> Items)
        {
            try
            {
                string sql = $"SELECT * FROM MANAGER.TAKE_SCHEDULE_USER WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";
                
                using (OracleDataReader reader = _context.SelectQuery(new OracleCommand(sql)))
                {
                    while (reader.Read())
                    {
                        if (reader.GetBoolean(11) == false) continue;

                        UserSchedule item = new UserSchedule();

                        item.Id = reader.GetInt64(0);
                        item.IdTrain = reader.GetInt64(1);
                        item.CategoryOfTrain = reader.GetString(2);
                        item.DeparturePoint = reader.GetString(3);
                        item.DepartureCity = reader.GetString(4);
                        item.ArrivalPoint = reader.GetString(5);
                        item.ArrivalCity = reader.GetString(6);
                        item.Distance = reader.GetInt64(7);
                        item.Duration = reader.GetInt64(8);
                        item.Date = reader.GetDateTime(9);
                        item.SetFrequency(reader.GetInt16(10));


                        Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Schedule.TakeSchedule_User Exception\n{ex.Message}");
            }
        }

        public void TakeSchedule_User(int RowMin, int RowMax, string Where, string Order, ObservableCollection<UserSchedule> Items)
        {
            try
            {
                string sql = $"SELECT * FROM MANAGER.TAKE_SCHEDULE_USER WHERE {Where} AND ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";

                using (OracleDataReader reader = _context.SelectQuery(new OracleCommand(sql)))
                {
                    while (reader.Read())
                    {
                        if (reader.GetBoolean(11) == false) continue;

                        UserSchedule item = new UserSchedule();

                        item.Id = reader.GetInt64(0);
                        item.IdTrain = reader.GetInt64(1);
                        item.CategoryOfTrain = reader.GetString(2);
                        item.DeparturePoint = reader.GetString(3);
                        item.DepartureCity = reader.GetString(4);
                        item.ArrivalPoint = reader.GetString(5);
                        item.ArrivalCity = reader.GetString(6);
                        item.Distance = reader.GetInt64(7);
                        item.Duration = reader.GetInt64(8);
                        item.Date = reader.GetDateTime(9);
                        item.SetFrequency(reader.GetInt16(10));


                        Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Schedule.TakeSchedule_User Exception\n{ex.Message}");
            }
        }
    }

    public class PassengerRepository : Repository<Passenger>
    {
        public PassengerRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.PASSENGERS";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Passenger temp = new Passenger();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.FullName = reader["FULL_NAME"].ToString();
                    temp.Passport = reader["PASSPORT"].ToString();
                    temp.Benefits = Convert.ToInt16(reader["BENEFITS"]);
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

        public override void Create(Passenger item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"CALL {Admin}.INSERT_PASSENGERS(" +
                    $":fullName, :passport, :benefits)";
                insert.Parameters.Add(":fullName", item.FullName);
                insert.Parameters.Add(":passport", item.Passport);
                insert.Parameters.Add(":benefits", item.Benefits);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Passenger.Create Exception," +
                    $"{ex.Message}");
            }
        }

        public override void Update(Passenger item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"CALL {Admin}.UPDATE_PASSENGERS(" +
                    $":id, :fullName, :passport, :benefits)";
                update.Parameters.Add(":id", item.Id);
                update.Parameters.Add(":fullName", item.FullName);
                update.Parameters.Add(":passport", item.Passport);
                update.Parameters.Add(":benefits", item.Benefits);
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Passenger.Update Exception {ex.Message}");
            }
        }

        public override void Update(Passenger item, string columnName, string newVal)
        {
            try
            {
                string convertedString = string.Concat(columnName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.PASSENGERS SET {convertedString} = '{newVal}' WHERE ID = {item.Id}";

                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Passenger.Update Exception {ex.Message}");
            }
        }

        public override void Delete(Passenger item, int Id)
        {
            try
            {
                _entities.Remove(item);
                _context.SaveOpen();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.PASSENGERS WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Passenger.Delete Exception {ex.Message}");
            }
        }

        public override Passenger? Get(int id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public override IEnumerable<Passenger> GetAll()
        {
            return _entities;
        }

        public override void GetAll(int RowMin, int RowMax, string Order, ObservableCollection<Passenger> Items)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.PASSENGERS WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Passenger temp = new Passenger();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.FullName = reader["FULL_NAME"].ToString();
                    temp.Passport = reader["PASSPORT"].ToString();
                    temp.Benefits = Convert.ToInt16(reader["BENEFITS"]);
                    Items.Add(temp);
                }
                reader.Close();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Нет соединения с базой данных");
            }
        }
    }

    public class PaymentRepository : Repository<Payment>
    {
        public PaymentRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.PAYMENTS";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Payment temp = new Payment();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.IdTicket = Convert.ToInt32(reader["ID_TICKET"]);
                    temp.DatePay = Convert.ToDateTime(reader["DATE_PAY"]);
                    temp.Status = Convert.ToChar(reader["STATUS"]);
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

        public override void Create(Payment item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"CALL {Admin}.INSERT_PAYMENTS(" +
                    $":idTick, :datePay, :stat)";
                insert.Parameters.Add(":idTick", item.IdTicket);
                insert.Parameters.Add(":datePay", OracleDbType.Date, item.DatePay, ParameterDirection.Input);
                insert.Parameters.Add(":stat", item.Status);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Payment.Create Exception," +
                    $"{ex.Message}");
            }
        }

        public override void Update(Payment item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"CALL {Admin}.UPDATE_PAYMENTS(" +
                    $":id, :idTick, :datePay, :stat)";
                update.Parameters.Add(":id", item.Id);
                update.Parameters.Add(":idTick", item.IdTicket);
                update.Parameters.Add(":datePay", OracleDbType.Date, item.DatePay, ParameterDirection.Input);
                update.Parameters.Add(":stat", item.Status);
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Payment.Update Exception {ex.Message}");
            }
        }

        public override void Update(Payment item, string columnName, string newVal)
        {
            try
            {
                string convertedString = string.Concat(columnName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                if (convertedString.Contains("DATE"))
                {
                    DateTime dateValue;
                    DateTime.TryParseExact(newVal, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                    if (dateValue == DateTime.MinValue)
                    {
                        MessageBox.Show("Неверно введена дата,\n Правильный формат: 'dd/MM/yyyy hh:mm tt'", "Ошибка в дате", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                        return;
                    }
                    update.CommandText = $"UPDATE {Admin}.PAYMENTS SET {convertedString} = TO_DATE('{dateValue.ToString("dd/MM/yyyy HH:mm")}', 'DD/MM/YYYY HH24:MI') WHERE ID = {item.Id}";
                }
                else update.CommandText = $"UPDATE {Admin}.PAYMENTS SET {convertedString} = '{newVal}' WHERE ID = {item.Id}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Payment.Update Exception {ex.Message}");
            }
        }

        public override void Delete(Payment item, int Id)
        {
            try
            {
                _entities.Remove(item);
                _context.SaveOpen();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.PAYMENTS WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Payment.Delete Exception {ex.Message}");
            }
        }

        public override Payment? Get(int id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public override IEnumerable<Payment> GetAll()
        {
            return _entities;
        }

        public override void GetAll(int RowMin, int RowMax, string Order, ObservableCollection<Payment> Items)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.PAYMENTS WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Payment temp = new Payment();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.IdTicket = Convert.ToInt32(reader["ID_TICKET"]);
                    temp.DatePay = Convert.ToDateTime(reader["DATE_PAY"]);
                    temp.Status = Convert.ToChar(reader["STATUS"]);
                    Items.Add(temp);
                }
                reader.Close();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Нет соединения с базой данных");
            }
        }
    }

    public class RouteRepository : Repository<Route>
    {
        public RouteRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.ROUTES";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Route temp = new Route();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.DeparturePoint = Convert.ToInt32(reader["DEPARTURE_POINT"]);
                    temp.ArrivalPoint = Convert.ToInt32(reader["ARRIVAL_POINT"]);
                    temp.Distance = Convert.ToInt32(reader["DISTANCE"]);
                    temp.Duration = Convert.ToInt32(reader["DURATION"]);
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

        public override void Create(Route item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"CALL {Admin}.INSERT_ROUTES(" +
                    $":depId, :arrId, :dist, :dur)";
                insert.Parameters.Add(":depId", item.DeparturePoint);
                insert.Parameters.Add(":arrId", item.ArrivalPoint);
                insert.Parameters.Add(":dist", item.Distance);
                insert.Parameters.Add(":dur", item.Duration);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Route.Create Exception," +
                    $"{ex.Message}");
            }
        }

        public override void Update(Route item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"CALL {Admin}.UPDATE_ROUTES(" +
                    $":id, :depId, :arrId, :dist, :dur)";
                update.Parameters.Add(":id", item.Id);
                update.Parameters.Add(":depId", item.DeparturePoint);
                update.Parameters.Add(":arrId", item.ArrivalPoint);
                update.Parameters.Add(":dist", item.Distance);
                update.Parameters.Add(":dur", item.Duration);
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Route.Update Exception {ex.Message}");
            }
        }

        public override void Update(Route item, string columnName, string newVal)
        {
            try
            {
                string convertedString = string.Concat(columnName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.ROUTES SET {convertedString} = '{newVal}' WHERE ID = {item.Id}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Route.Update Exception {ex.Message}");
            }
        }

        public override void Delete(Route item, int Id)
        {
            try
            {
                _entities.Remove(item);
                _context.SaveOpen();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.ROUTES WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Route.Delete Exception {ex.Message}");
            }
        }

        public override Route? Get(int id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public override IEnumerable<Route> GetAll()
        {
            return _entities;
        }

        public override void GetAll(int RowMin, int RowMax, string Order, ObservableCollection<Route> Items)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.ROUTES WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Route temp = new Route();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.DeparturePoint = Convert.ToInt32(reader["DEPARTURE_POINT"]);
                    temp.ArrivalPoint = Convert.ToInt32(reader["ARRIVAL_POINT"]);
                    temp.Distance = Convert.ToInt32(reader["DISTANCE"]);
                    temp.Duration = Convert.ToInt32(reader["DURATION"]);
                    Items.Add(temp);
                }
                reader.Close();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Нет соединения с базой данных");
            }
        }
    }

    public class StationRepository : Repository<Station>
    {
        public StationRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.STATIONS";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Station temp = new Station();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.StationName = reader["STATION_NAME"].ToString();
                    temp.City = reader["CITY"].ToString();
                    temp.State = reader["STATE"].ToString();
                    temp.Country = reader["COUNTRY"].ToString();
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

        public override void Create(Station item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"CALL {Admin}.INSERT_STATIONS(" +
                    $":stName, :city, :state, :country)";
                insert.Parameters.Add(":stName", item.StationName);
                insert.Parameters.Add(":city", item.City);
                insert.Parameters.Add(":state", item.State);
                insert.Parameters.Add(":country", item.Country);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Station.Create Exception," +
                    $"{ex.Message}");
            }
        }

        public override void Update(Station item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"CALL {Admin}.UPDATE_STATIONS(" +
                    $":id, :stName, :city, :state, :country)";
                update.Parameters.Add(":id", item.Id);
                update.Parameters.Add(":stName", item.StationName);
                update.Parameters.Add(":city", item.City);
                update.Parameters.Add(":state", item.State);
                update.Parameters.Add(":country", item.Country);
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Station.Update Exception {ex.Message}");
            }
        }

        public override void Update(Station item, string columnName, string newVal)
        {
            try
            {
                string convertedString = string.Concat(columnName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.STATIONS SET {convertedString} = '{newVal}' WHERE ID = {item.Id}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Station.Update Exception {ex.Message}");
            }
        }

        public override void Delete(Station item, int Id)
        {
            try
            {
                _entities.Remove(item);
                _context.SaveOpen();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.STATIONS WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Station.Delete Exception {ex.Message}");
            }
        }

        public override Station? Get(int id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public override IEnumerable<Station> GetAll()
        {
            return _entities;
        }

        public override void GetAll(int RowMin, int RowMax, string Order, ObservableCollection<Station> Items)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.STATIONS WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Station temp = new Station();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.StationName = reader["STATION_NAME"].ToString();
                    temp.City = reader["CITY"].ToString();
                    temp.State = reader["STATE"].ToString();
                    temp.Country = reader["COUNTRY"].ToString();
                    Items.Add(temp);
                }
                reader.Close();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Нет соединения с базой данных");
            }
        }
    }

    public class StationsRouteRepository : Repository<StationsRoute>
    {
        public StationsRouteRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.STATIONS_ROUTES";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    StationsRoute temp = new StationsRoute();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.RouteId = Convert.ToInt32(reader["ROUTE_ID"]);
                    temp.StationId = Convert.ToInt32(reader["STATION_ID"]);
                    temp.StationOrder = Convert.ToInt32(reader["STATION_ORDER"]);
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

        public override void Create(StationsRoute item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"CALL {Admin}.INSERT_STATIONS_ROUTES(" +
                    $":route, :stId, :stOrder)";
                insert.Parameters.Add(":route", item.RouteId);
                insert.Parameters.Add(":stId", item.StationId);
                insert.Parameters.Add(":stOrder", item.StationOrder);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"StationsRoute.Create Exception," +
                    $"{ex.Message}");
            }
        }

        public override void Update(StationsRoute item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"CALL {Admin}.UPDATE_STATIONS_ROUTES(" +
                    $":id, :route, :stId, :stOrder)";
                update.Parameters.Add(":id", item.Id);
                update.Parameters.Add(":route", item.RouteId);
                update.Parameters.Add(":stId", item.StationId);
                update.Parameters.Add(":stOrder", item.StationOrder);
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"StationsRoute.Update Exception {ex.Message}");
            }
        }

        public override void Update(StationsRoute item, string columnName, string newVal)
        {
            try
            {
                string convertedString = string.Concat(columnName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.STATIONS_ROUTES SET {convertedString} = '{newVal}' WHERE ID = {item.Id}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"StationsRoute.Update Exception {ex.Message}");
            }
        }

        public override void Delete(StationsRoute item, int Id)
        {
            try
            {
                _entities.Remove(item);
                _context.SaveOpen();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.STATIONS_ROUTES WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"StationsRoute.Delete Exception {ex.Message}");
            }
        }

        public override StationsRoute? Get(int id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public override IEnumerable<StationsRoute> GetAll()
        {
            return _entities;
        }

        public override void GetAll(int RowMin, int RowMax, string Order, ObservableCollection<StationsRoute> Items)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.STATIONS_ROUTES WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    StationsRoute temp = new StationsRoute();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.RouteId = Convert.ToInt32(reader["ROUTE_ID"]);
                    temp.StationId = Convert.ToInt32(reader["STATION_ID"]);
                    temp.StationOrder = Convert.ToInt32(reader["STATION_ORDER"]);
                    Items.Add(temp);
                }
                reader.Close();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Нет соединения с базой данных");
            }
        }
    }

    public class TicketRepository : Repository<Ticket>
    {
        public TicketRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.TICKETS";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Ticket temp = new Ticket();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.IdPassenger = Convert.ToInt32(reader["ID_PASSENGER"]);
                    temp.IdTrain = Convert.ToInt32(reader["ID_TRAIN"]);
                    temp.IdVan = Convert.ToInt32(reader["ID_VAN"]);
                    temp.SeatNumber = Convert.ToInt32(reader["SEAT_NUMBER"]);
                    temp.FromWhere = Convert.ToInt32(reader["FROM_WHERE"]);
                    temp.ToWhere = Convert.ToInt32(reader["TO_WHERE"]);
                    temp.Date = Convert.ToDateTime(reader["DATE"]);
                    temp.Cost = Convert.ToInt32(reader["COST"]);
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

        public override void Create(Ticket item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"CALL {Admin}.INSERT_TICKETS(" +
                    $":passenger, :train, :van, :seatN, :from, :to. :dat, :cost)";
                insert.Parameters.Add(":passenger", item.IdPassenger);
                insert.Parameters.Add(":train", item.IdTrain);
                insert.Parameters.Add(":van", item.IdVan);
                insert.Parameters.Add(":seatN", item.SeatNumber);
                insert.Parameters.Add(":from", item.FromWhere);
                insert.Parameters.Add(":to", item.ToWhere);
                insert.Parameters.Add(":dat", OracleDbType.Date, item.Date, ParameterDirection.Input);
                insert.Parameters.Add(":cost", item.Cost);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ticket.Create Exception," +
                    $"{ex.Message}");
            }
        }

        public override void Update(Ticket item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"CALL {Admin}.UPDATE_TICKETS(" +
                    $":id, :passenger, :train, :van, :seatN, :from, :to. :dat, :cost)";
                update.Parameters.Add(":id", item.Id);
                update.Parameters.Add(":passenger", item.IdPassenger);
                update.Parameters.Add(":train", item.IdTrain);
                update.Parameters.Add(":van", item.IdVan);
                update.Parameters.Add(":seatN", item.SeatNumber);
                update.Parameters.Add(":from", item.FromWhere);
                update.Parameters.Add(":to", item.ToWhere);
                update.Parameters.Add(":dat", OracleDbType.Date, item.Date, ParameterDirection.Input);
                update.Parameters.Add(":cost", item.Cost);
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ticket.Update Exception {ex.Message}");
            }
        }

        public override void Update(Ticket item, string columnName, string newVal)
        {
            try
            {
                string convertedString = string.Concat(columnName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                if (convertedString.Contains("DATE"))
                {
                    DateTime dateValue;
                    DateTime.TryParseExact(newVal, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                    if (dateValue == DateTime.MinValue)
                    {
                        MessageBox.Show("Неверно введена дата,\n Правильный формат: 'dd/MM/yyyy hh:mm tt'", "Ошибка в дате", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                        return;
                    }
                    update.CommandText = $"UPDATE {Admin}.TICKETS SET \"{convertedString}\" = TO_DATE('{dateValue.ToString("dd/MM/yyyy HH:mm")}', 'DD/MM/YYYY HH24:MI') WHERE ID = {item.Id}";
                }
                else update.CommandText = $"UPDATE {Admin}.TICKETS SET {convertedString} = '{newVal}' WHERE ID = {item.Id}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ticket.Update Exception {ex.Message}");
            }
        }

        public override void Delete(Ticket item, int Id)
        {
            try
            {
                _entities.Remove(item);
                _context.SaveOpen();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.TICKETS WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Ticket.Delete Exception");
            }
        }

        public override Ticket? Get(int id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public override IEnumerable<Ticket> GetAll()
        {
            return _entities;
        }

        public override void GetAll(int RowMin, int RowMax, string Order, ObservableCollection<Ticket> Items)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.TICKETS WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Ticket temp = new Ticket();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.IdPassenger = Convert.ToInt32(reader["ID_PASSENGER"]);
                    temp.IdTrain = Convert.ToInt32(reader["ID_TRAIN"]);
                    temp.IdVan = Convert.ToInt32(reader["ID_VAN"]);
                    temp.SeatNumber = Convert.ToInt32(reader["SEAT_NUMBER"]);
                    temp.FromWhere = Convert.ToInt32(reader["FROM_WHERE"]);
                    temp.ToWhere = Convert.ToInt32(reader["TO_WHERE"]);
                    temp.Date = Convert.ToDateTime(reader["DATE"]);
                    temp.Cost = Convert.ToInt32(reader["COST"]);
                    Items.Add(temp);
                }
                reader.Close();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Нет соединения с базой данных");
            }
        }
    }

    public class OrdersRepository : Repository<TakeTicket>
    {
        public OrdersRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.TAKE_TICKET";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    TakeTicket temp = new TakeTicket();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.IdPassenger = Convert.ToInt32(reader["ID_PASSENGER"]);
                    temp.IdTrain = Convert.ToInt32(reader["ID_TRAIN"]);
                    temp.IdVan = Convert.ToInt32(reader["ID_VAN"]);
                    temp.SeatNumber = Convert.ToInt32(reader["SEAT_NUMBER"]);
                    temp.FromWhere = Convert.ToInt32(reader["FROM_WHERE"]);
                    temp.ToWhere = Convert.ToInt32(reader["TO_WHERE"]);
                    temp.Date = Convert.ToDateTime(reader["DATE"]);
                    temp.Cost = Convert.ToInt32(reader["COST"]);
                    temp.DatePay = Convert.ToDateTime(reader["DATE_PAY"]);
                    temp.Status = Convert.ToChar(reader["STATUS"]);
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

        public override void Create(TakeTicket item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"CALL {Admin}.INSERT_TAKE_TICKET(:passenger, :train, :van, :seatN, :fromW, :toW, :dat, :cos, :dateP, :stat)";
                insert.Parameters.Add(":passenger", item.IdPassenger);
                insert.Parameters.Add(":train", item.IdTrain);
                insert.Parameters.Add(":van", item.IdVan);
                insert.Parameters.Add(":seatN", item.SeatNumber);
                insert.Parameters.Add(":fromW", item.FromWhere);
                insert.Parameters.Add(":toW", item.ToWhere);
                insert.Parameters.Add(":dat", OracleDbType.Date, item.Date, ParameterDirection.Input);
                insert.Parameters.Add(":cos", item.Cost);
                insert.Parameters.Add(":dateP", OracleDbType.Date, item.DatePay, ParameterDirection.Input);
                insert.Parameters.Add(":stat", item.Status);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TAKE_TICKET.Create Exception," +
                    $"{ex.Message}");
            }
        }

        public override void Update(TakeTicket item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"CALL {Admin}.TAKE_TICKET(" +
                    $":id, :passenger, :train, :van, :seatN, :fromW, :toW, :dat, :cost, :dateP, :status)";
                update.Parameters.Add(":id", item.Id);
                update.Parameters.Add(":passenger", item.IdPassenger);
                update.Parameters.Add(":train", item.IdTrain);
                update.Parameters.Add(":van", item.IdVan);
                update.Parameters.Add(":seatN", item.SeatNumber);
                update.Parameters.Add(":fromW", item.FromWhere);
                update.Parameters.Add(":toW", item.ToWhere);
                update.Parameters.Add(":dat", OracleDbType.Date, item.Date, ParameterDirection.Input);
                update.Parameters.Add(":cost", item.Cost);
                update.Parameters.Add(":dateP", OracleDbType.Date, item.DatePay, ParameterDirection.Input);
                update.Parameters.Add(":status", item.Status);
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Take_Ticket.Update Exception {ex.Message}");
            }
        }

        public override void Update(TakeTicket item, string columnName, string newVal)
        {
            try
            {
                string convertedString = string.Concat(columnName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                if (convertedString.Contains("DATE"))
                {
                    DateTime dateValue;
                    DateTime.TryParseExact(newVal, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                    if (dateValue == DateTime.MinValue)
                    {
                        MessageBox.Show("Неверно введена дата,\n Правильный формат: 'dd/MM/yyyy hh:mm tt'", "Ошибка в дате", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                        return;
                    }
                    update.CommandText = $"UPDATE {Admin}.TAKE_TICKET SET \"{convertedString}\" = TO_DATE('{dateValue.ToString("dd/MM/yyyy HH:mm")}', 'DD/MM/YYYY HH24:MI') WHERE ID = {item.Id}";
                }
                else update.CommandText = $"UPDATE {Admin}.TAKE_TICKET SET {convertedString} = '{newVal}' WHERE ID = {item.Id}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TAKE_TICKET.Update Exception {ex.Message}");
            }
        }

        public override void Delete(TakeTicket item, int Id)
        {
            try
            {
                _entities.Remove(item);
                _context.SaveOpen();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.TAKE_TICKET WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TAKE_TICKET.Delete Exception {ex.Message}");
            }
        }

        public override TakeTicket? Get(int id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public override IEnumerable<TakeTicket> GetAll()
        {
            return _entities;
        }

        public void GetAll(int RowMin, int RowMax, string IdPas, string Order, ObservableCollection<TakeTicket> Items)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                if (IdPas.Trim() == "")
                    getAll.CommandText = $"SELECT * FROM {Admin}.TAKE_TICKET WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";
                else
                {
                    getAll.CommandText = $"SELECT * FROM {Admin}.TAKE_TICKET WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} " +
                    $"AND ID_PASSENGER = {IdPas} ORDER BY {Order}";
                }
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    TakeTicket temp = new TakeTicket();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.IdPassenger = Convert.ToInt32(reader["ID_PASSENGER"]);
                    temp.IdTrain = Convert.ToInt32(reader["ID_TRAIN"]);
                    temp.IdVan = Convert.ToInt32(reader["ID_VAN"]);
                    temp.SeatNumber = Convert.ToInt32(reader["SEAT_NUMBER"]);
                    temp.FromWhere = Convert.ToInt32(reader["FROM_WHERE"]);
                    temp.ToWhere = Convert.ToInt32(reader["TO_WHERE"]);
                    temp.Date = Convert.ToDateTime(reader["DATE"]);
                    temp.Cost = Convert.ToInt32(reader["COST"]);
                    temp.DatePay = Convert.ToDateTime(reader["DATE_PAY"]);
                    temp.Status = Convert.ToChar(reader["STATUS"]);
                    Items.Add(temp);
                }
                reader.Close();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Нет соединения с базой данных");
            }
        }
    }

    public class TrainRepository : Repository<Train>
    {
        public TrainRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.TRAINS";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Train temp = new Train();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.CategoryOfTrain = reader["CATEGORY_OF_TRAIN"].ToString();
                    temp.IsForPassengers = Convert.ToBoolean(reader["IS_FOR_PASSENGERS"]);
                    temp.Vans = reader["VANS"].ToString();
                    temp.CountOfVans = Convert.ToInt32(reader["COUNT_OF_VANS"]);
                    temp.ParkingTime = Convert.ToInt32(reader["PARKING_TIME"]);
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

        public override void Create(Train item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"CALL {Admin}.INSERT_TRAINS(" +
                    $":category, :forPas, :vans, :countVans, :parkTime)";
                insert.Parameters.Add(":category", item.CategoryOfTrain);
                insert.Parameters.Add(":forPas", Convert.ToInt16(item.IsForPassengers));
                insert.Parameters.Add(":vans", item.Vans);
                insert.Parameters.Add(":countVans", item.CountOfVans);
                insert.Parameters.Add(":parkTime", item.ParkingTime);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Train.Create Exception," +
                    $"Insert {ex.Message}");
            }
        }

        public override void Update(Train item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"CALL {Admin}.UPDATE_TRAINS(" +
                    $":id, :category, :forPas, :vans, :countVans, :parkTime)";
                update.Parameters.Add(":id", item.Id);
                update.Parameters.Add(":category", item.CategoryOfTrain);
                update.Parameters.Add(":forPas", item.IsForPassengers);
                update.Parameters.Add(":vans", item.Vans);
                update.Parameters.Add(":countVans", item.CountOfVans);
                update.Parameters.Add(":parkTime", item.ParkingTime);
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Train.Update Exception {ex.Message}");
            }
        }

        public override void Update(Train item, string columnName, string newVal)
        {
            try
            {
                string convertedString = string.Concat(columnName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.TRAINS SET {convertedString} = '{newVal}' WHERE ID = {item.Id}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Train.Update Exception {ex.Message}");
            }
        }

        public override void Delete(Train item, int Id)
        {
            try
            {
                _entities.Remove(item);
                _context.SaveOpen();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.TRAINS WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Train.Delete Exception {ex.Message}");
            }
        }

        public override Train? Get(int id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public override IEnumerable<Train> GetAll()
        {
            return _entities;
        }

        public override void GetAll(int RowMin, int RowMax, string Order, ObservableCollection<Train> Items)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.TRAINS WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Train temp = new Train();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.CategoryOfTrain = reader["CATEGORY_OF_TRAIN"].ToString();
                    temp.IsForPassengers = Convert.ToBoolean(reader["IS_FOR_PASSENGERS"]);
                    temp.Vans = reader["VANS"].ToString();
                    temp.CountOfVans = Convert.ToInt32(reader["COUNT_OF_VANS"]);
                    temp.ParkingTime = Convert.ToInt32(reader["PARKING_TIME"]);
                    Items.Add(temp);
                }
                reader.Close();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Нет соединения с базой данных");
            }
        }
    }

    public class VanRepository : Repository<Van>
    {
        public VanRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.VANS";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Van temp = new Van();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.Type = reader["TYPE"].ToString();
                    temp.Capacity = Convert.ToInt32(reader["CAPACITY"]);
                    temp.IsFree = Convert.ToBoolean(reader["IS_FREE"]);
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

        public override void Create(Van item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"CALL {Admin}.INSERT_VANS(" +
                    $":type, :capacity, :free)";
                insert.Parameters.Add(":type", item.Type);
                insert.Parameters.Add(":capacity", item.Capacity);
                insert.Parameters.Add(":free", item.IsFree ? 1 : 0);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Van.Create Exception," +
                    $"{ex.Message}");
            }
        }

        public override void Update(Van item)
        {
            try
            {
                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"CALL {Admin}.UPDATE_VANS(" +
                    $":id, :type, :capacity, :free)";
                update.Parameters.Add(":id", item.Id);
                update.Parameters.Add(":type", item.Type);
                update.Parameters.Add(":capacity", item.Capacity);
                update.Parameters.Add(":free", item.IsFree);
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Van.Update Exception {ex.Message}");
            }
        }

        public override void Update(Van item, string columnName, string newVal)
        {
            try
            {
                string convertedString = string.Concat(columnName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();

                _entities.Add(item);
                _context.SaveOpen();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.VANS SET {convertedString} = '{newVal}' WHERE ID = {item.Id}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Van.Update Exception {ex.Message}");
            }
        }

        public override void Delete(Van item, int Id)
        {
            try
            {
                _entities.Remove(item);
                _context.SaveOpen();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.VANS WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Van.Delete Exception {ex.Message}");
            }
        }

        public override Van? Get(int id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public override IEnumerable<Van> GetAll()
        {
            return _entities;
        }

        public override void GetAll(int RowMin, int RowMax, string Order, ObservableCollection<Van> Items)
        {
            try
            {
                _context.SaveOpen();
                OracleCommand getAll = _context.conn.CreateCommand();
                getAll.CommandText = $"SELECT * FROM {Admin}.VANS WHERE ROWNUM > {RowMin} AND ROWNUM <= {RowMax} ORDER BY {Order}";
                getAll.CommandType = System.Data.CommandType.Text;
                OracleDataReader reader = getAll.ExecuteReader();
                while (reader.Read())
                {
                    Van temp = new Van();
                    temp.Id = Convert.ToInt32(reader["ID"]);
                    temp.Type = reader["TYPE"].ToString();
                    temp.Capacity = Convert.ToInt32(reader["CAPACITY"]);
                    temp.IsFree = Convert.ToBoolean(reader["IS_FREE"]);
                    Items.Add(temp);
                }
                reader.Close();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Нет соединения с базой данных");
            }
        }
    }
}
