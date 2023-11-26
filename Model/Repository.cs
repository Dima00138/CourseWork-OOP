using CourseWork.View;
using MaterialDesignThemes.Wpf;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
                while (reader.Read())
                {
                    Schedule temp = new Schedule();
                    temp.Id = reader.GetInt64(0);
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

        public virtual void Create(Schedule item)
        {
                int result = 0;
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"EXEC {Admin}.INSERT_SCHEDULE(" +
                    $":idTrain, :date, :route, :frequency)";
                insert.Parameters.Add(":idTrain", item.IdTrain);
                insert.Parameters.Add(":date", item.Date);
                insert.Parameters.Add(":route", item.Route);
                insert.Parameters.Add(":frequency", item.GetFrequency());
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
            catch
            {
                MessageBox.Show("Schedule.TakeSchedule_User Exception");
            }
        }
    }

    public class PassengerRepository : Repository<Passenger>
    {
        public PassengerRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.conn.Open();
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

        public virtual void Create(Passenger item)
        {
            int result = 0;
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"EXEC {Admin}.INSERT_PASSENGERS(" +
                    $":fullName, :passport, :benefits)";
                insert.Parameters.Add(":fullName", item.FullName);
                insert.Parameters.Add(":passport", item.Passport);
                insert.Parameters.Add(":benefits", item.Benefits);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show($"Passenger.Create Exception," +
                    $"Insert {Convert.ToBoolean(result)}");
            }
        }

        public void Update(Passenger item, string columnName, string newVal)
        {
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.PASSENGERS SET {columnName} = {newVal}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Passenger.Update Exception");
            }
        }

        public virtual void Delete(Passenger item, string id)
        {
            try
            {
                _entities.Remove(item);
                _context.conn.Open();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.PASSENGERS WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Passenger.Delete Exception");
            }
        }

        public virtual Passenger? Get(long id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public virtual IEnumerable<Passenger> GetAll()
        {
            return _entities;
        }
    }

    public class PaymentRepository : Repository<Payment>
    {
        public PaymentRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.conn.Open();
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

        public virtual void Create(Payment item)
        {
            int result = 0;
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"EXEC {Admin}.INSERT_PAYMENTS(" +
                    $":idTick, :datePay, :stat)";
                insert.Parameters.Add(":idTick", item.IdTicket);
                insert.Parameters.Add(":datePay", item.DatePay);
                insert.Parameters.Add(":stat", item.Status);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show($"Payment.Create Exception," +
                    $"Insert {Convert.ToBoolean(result)}");
            }
        }

        public void Update(Payment item, string columnName, string newVal)
        {
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.PAYMENTS SET {columnName} = {newVal}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Payment.Update Exception");
            }
        }

        public virtual void Delete(Payment item, string id)
        {
            try
            {
                _entities.Remove(item);
                _context.conn.Open();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.PAYMENTS WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Payment.Delete Exception");
            }
        }

        public virtual Payment? Get(long id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public virtual IEnumerable<Payment> GetAll()
        {
            return _entities;
        }
    }

    public class RouteRepository : Repository<Route>
    {
        public RouteRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.conn.Open();
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

        public virtual void Create(Route item)
        {
            int result = 0;
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"EXEC {Admin}.INSERT_ROUTES(" +
                    $":depId, :arrId, :dist, :dur)";
                insert.Parameters.Add(":depId", item.DeparturePoint);
                insert.Parameters.Add(":arrId", item.ArrivalPoint);
                insert.Parameters.Add(":dist", item.Distance);
                insert.Parameters.Add(":dur", item.Duration);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show($"Route.Create Exception," +
                    $"Insert {Convert.ToBoolean(result)}");
            }
        }

        public void Update(Route item, string columnName, string newVal)
        {
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.ROUTES SET {columnName} = {newVal}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Route.Update Exception");
            }
        }

        public virtual void Delete(Route item, string id)
        {
            try
            {
                _entities.Remove(item);
                _context.conn.Open();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.ROUTES WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Route.Delete Exception");
            }
        }

        public virtual Route? Get(long id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public virtual IEnumerable<Route> GetAll()
        {
            return _entities;
        }
    }

    public class StationRepository : Repository<Station>
    {
        public StationRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.conn.Open();
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

        public virtual void Create(Station item)
        {
            int result = 0;
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"EXEC {Admin}.INSERT_STATIONS(" +
                    $":stName, :city, :state, :country)";
                insert.Parameters.Add(":stName", item.StationName);
                insert.Parameters.Add(":city", item.City);
                insert.Parameters.Add(":state", item.State);
                insert.Parameters.Add(":country", item.Country);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show($"Station.Create Exception," +
                    $"Insert {Convert.ToBoolean(result)}");
            }
        }

        public void Update(Station item, string columnName, string newVal)
        {
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.STATIONS SET {columnName} = {newVal}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Station.Update Exception");
            }
        }

        public virtual void Delete(Station item, string id)
        {
            try
            {
                _entities.Remove(item);
                _context.conn.Open();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.STATIONS WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Station.Delete Exception");
            }
        }

        public virtual Station? Get(long id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public virtual IEnumerable<Station> GetAll()
        {
            return _entities;
        }
    }

    public class StationsRouteRepository : Repository<StationsRoute>
    {
        public StationsRouteRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.conn.Open();
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

        public virtual void Create(StationsRoute item)
        {
            int result = 0;
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"EXEC {Admin}.INSERT_STATIONS_ROUTES(" +
                    $":route, :stId, :stOrder)";
                insert.Parameters.Add(":route", item.RouteId);
                insert.Parameters.Add(":stId", item.StationId);
                insert.Parameters.Add(":stOrder", item.StationOrder);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show($"StationsRoute.Create Exception," +
                    $"Insert {Convert.ToBoolean(result)}");
            }
        }

        public void Update(StationsRoute item, string columnName, string newVal)
        {
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.STATIONS_ROUTES SET {columnName} = {newVal}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("StationsRoute.Update Exception");
            }
        }

        public virtual void Delete(StationsRoute item, string id)
        {
            try
            {
                _entities.Remove(item);
                _context.conn.Open();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.STATIONS_ROUTES WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("StationsRoute.Delete Exception");
            }
        }

        public virtual StationsRoute? Get(long id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public virtual IEnumerable<StationsRoute> GetAll()
        {
            return _entities;
        }
    }

    public class TicketRepository : Repository<Ticket>
    {
        public TicketRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.conn.Open();
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

        public virtual void Create(Ticket item)
        {
            int result = 0;
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"EXEC {Admin}.INSERT_TICKETS(" +
                    $":passenger, :train, :van, :seatN, :from, :to. :date, :cost)";
                insert.Parameters.Add(":passenger", item.IdPassenger);
                insert.Parameters.Add(":train", item.IdTrain);
                insert.Parameters.Add(":van", item.IdVan);
                insert.Parameters.Add(":seatN", item.SeatNumber);
                insert.Parameters.Add(":from", item.FromWhere);
                insert.Parameters.Add(":to", item.ToWhere);
                insert.Parameters.Add(":date", item.Date);
                insert.Parameters.Add(":cost", item.Cost);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show($"Ticket.Create Exception," +
                    $"Insert {Convert.ToBoolean(result)}");
            }
        }

        public void Update(Ticket item, string columnName, string newVal)
        {
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.TICKETS SET {columnName} = {newVal}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Ticket.Update Exception");
            }
        }

        public virtual void Delete(Ticket item, string id)
        {
            try
            {
                _entities.Remove(item);
                _context.conn.Open();
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

        public virtual Ticket? Get(long id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public virtual IEnumerable<Ticket> GetAll()
        {
            return _entities;
        }
    }

    public class TrainRepository : Repository<Train>
    {
        public TrainRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.conn.Open();
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

        public virtual void Create(Train item)
        {
            int result = 0;
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"EXEC {Admin}.INSERT_TRAINS(" +
                    $":category, :forPas, :vans, :countVans, :parkTime)";
                insert.Parameters.Add(":category", item.CategoryOfTrain);
                insert.Parameters.Add(":forPas", item.IsForPassengers);
                insert.Parameters.Add(":vans", item.Vans);
                insert.Parameters.Add(":countVans", item.CountOfVans);
                insert.Parameters.Add(":parkTime", item.ParkingTime);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show($"Train.Create Exception," +
                    $"Insert {Convert.ToBoolean(result)}");
            }
        }

        public void Update(Train item, string columnName, string newVal)
        {
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.TRAINS SET {columnName} = {newVal}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Train.Update Exception");
            }
        }

        public virtual void Delete(Train item, string id)
        {
            try
            {
                _entities.Remove(item);
                _context.conn.Open();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.TRAINS WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Train.Delete Exception");
            }
        }

        public virtual Train? Get(long id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public virtual IEnumerable<Train> GetAll()
        {
            return _entities;
        }
    }

    public class VanRepository : Repository<Van>
    {
        public VanRepository(OracleContext con) : base(con)
        {
            try
            {
                _context.conn.Open();
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

        public virtual void Create(Van item)
        {
            int result = 0;
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand insert = _context.conn.CreateCommand();
                insert.CommandText = $"EXEC {Admin}.INSERT_VANS(" +
                    $":type, :capacity, :free)";
                insert.Parameters.Add(":type", item.Type);
                insert.Parameters.Add(":capacity", item.Capacity);
                insert.Parameters.Add(":free", item.IsFree);
                insert.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show($"Van.Create Exception," +
                    $"Insert {Convert.ToBoolean(result)}");
            }
        }

        public void Update(Van item, string columnName, string newVal)
        {
            try
            {
                _entities.Add(item);
                _context.conn.Open();
                OracleCommand update = _context.conn.CreateCommand();
                update.CommandText = $"UPDATE {Admin}.VANS SET {columnName} = {newVal}";
                update.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Van.Update Exception");
            }
        }

        public virtual void Delete(Van item, string id)
        {
            try
            {
                _entities.Remove(item);
                _context.conn.Open();
                OracleCommand delete = _context.conn.CreateCommand();
                delete.CommandText = $"DELETE FROM {Admin}.VANS WHERE \"ID\" = {item.Id}";
                delete.ExecuteNonQuery();
                _context.conn.Close();
            }
            catch
            {
                MessageBox.Show("Van.Delete Exception");
            }
        }

        public virtual Van? Get(long id)
        {
            return _entities.ToList().Find(item => item.Id == id);
        }

        public virtual IEnumerable<Van> GetAll()
        {
            return _entities;
        }
    }
}
