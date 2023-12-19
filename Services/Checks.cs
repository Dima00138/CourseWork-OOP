using CourseWork.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CourseWork.Services
{
    public static class Checks
    {
        public static bool CheckPassenger(Passenger p)
        {
            // Проверка паспорта
            Regex passportRegex = new Regex(@"^[A-ZА-Я]{2}\d{7}$"); // Пример формата: AB1234567
            if (string.IsNullOrEmpty(p.Passport) || !passportRegex.IsMatch(p.Passport))
            {
                MessageBox.Show("Ошибка: паспорт должен быть формата \"AB1234567\".", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Проверка льгот
            if (p.Benefits < 0 || p.Benefits > 100)
            {
                MessageBox.Show("Ошибка: Льготы не могут быть отрицательными и превышать 100.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Льготы не могут быть отрицательными и превышать 100." 
                return false;
            }

            // Проверка полного имени
            Regex fullNameRegex = new Regex(@"^[A-Za-zА-Яа-я]+\s[A-Za-zА-Яа-я]+\s[A-Za-zА-Яа-я]+$"); // Пример формата: Имя Отчество Фамилия
            if (string.IsNullOrEmpty(p.FullName) || !fullNameRegex.IsMatch(p.FullName))
            {
                MessageBox.Show("Ошибка: Полное имя должно состоять из трех слов, разделенных пробелами.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //Ошибка: Полное имя должно состоять из трех слов, разделенных пробелами."
                return false;
            }
            return true;
        }
        public static bool CheckPayment(Payment p)
        {
            if (p.IdTicket <= 0)
            {
                MessageBox.Show("Ошибка: ID билета должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: ID билета должен быть положительным числом."
                return false;
            }

            if (p.DatePay > DateTime.Now)
            {
                MessageBox.Show("Ошибка: Дата оплаты не может быть в будущем.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Дата оплаты не может быть в будущем."
                return false;
            }

            // Проверка статуса
            if (p.Status != 'R' && p.Status != 'S' && p.Status != 'W')
            {
                MessageBox.Show("Ошибка: Статус может быть только 'R', 'S' или 'W'.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Статус может быть только 'R', 'S' или 'W'."
                return false;
            }
            return true;
        }
        public static bool CheckRoute(Route p)
        {
            // Проверка точки отправления
            if (p.DeparturePoint <= 0)
            {
                MessageBox.Show("Ошибка: Точка отправления должна быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Точка отправления должна быть положительным числом."
                return false;
            }

            // Проверка точки прибытия
            if (p.ArrivalPoint <= 0 || p.ArrivalPoint == p.DeparturePoint)
            {
                MessageBox.Show("Ошибка: Точка прибытия должна быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Точка прибытия должна быть положительным числом."
                return false;
            }

            // Проверка расстояния
            if (p.Distance <= 0)
            {
                MessageBox.Show("Ошибка: Расстояние должно быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Расстояние должно быть положительным числом."
                return false;
            }

            // Проверка продолжительности
            if (p.Duration <= 0)
            {
                MessageBox.Show("Ошибка: Продолжительность должна быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Продолжительность должна быть положительным числом."
                return false;
            }
            return true;
        }
        public static bool CheckSchedule(Schedule p)
        {
            // Проверка ID поезда
            if (p.IdTrain <= 0)
            {
                MessageBox.Show("Ошибка: ID поезда должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: ID поезда должен быть положительным числом."
                return false;
            }

            // Проверка даты
            if (p.Date < DateTime.Now)
            {
                MessageBox.Show("Ошибка: Дата не может быть в прошлом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Дата не может быть в будущем."
                return false;
            }

            // Проверка маршрута
            if (p.Route <= 0)
            {
                MessageBox.Show("Ошибка: Маршрут должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Маршрут должен быть положительным числом."
                return false;
            }

            // Проверка частоты
            if (p.GetFrequency() == -1)
            {
                MessageBox.Show("Ошибка: Частота не может быть пустой.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Частота не может быть пустой."
                return false;
            }
            return true;
        }
        public static bool CheckStation(Station p)
        {
            // Проверка названия станции
            if (string.IsNullOrEmpty(p.StationName))
            {
                MessageBox.Show("Ошибка: Название станции не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Название станции не может быть пустым."
                return false;
            }

            // Проверка города
            if (string.IsNullOrEmpty(p.City))
            {
                MessageBox.Show("Ошибка: Город не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Город не может быть пустым."
                return false;
            }

            // Проверка штата
            if (string.IsNullOrEmpty(p.State))
            {
                MessageBox.Show("Ошибка: Штат(Регион) не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Штат не может быть пустым."
                return false;
            }

            // Проверка страны
            if (string.IsNullOrEmpty(p.Country))
            {
                MessageBox.Show("Ошибка: Страна не может быть пустой.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Страна не может быть пустой."
                return false;
            }
            return true;
        }
        public static bool CheckStationRoute(StationsRoute p)
        {
            // Проверка ID маршрута
            if (p.RouteId <= 0)
            {
                MessageBox.Show("Ошибка: ID маршрута должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: ID маршрута должен быть положительным числом.");
                return false;
            }

            // Проверка ID станции
            if (p.StationId <= 0)
            {
                MessageBox.Show("Ошибка: ID станции должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: ID станции должен быть положительным числом.");
                return false;
            }

            // Проверка порядка станции
            if (p.StationOrder <= 0)
            {
                MessageBox.Show("Ошибка: Порядок станции должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Порядок станции должен быть положительным числом.");
                return false;
            }
            return true;
        }
        public static bool CheckTicket(Ticket p)
        {
            // Проверка ID пассажира
            if (p.IdPassenger <= 0)
            {
                MessageBox.Show("Ошибка: ID пассажира должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: ID пассажира должен быть положительным числом.");
                return false;
            }

            // Проверка ID поезда
            if (p.IdTrain <= 0)
            {
                MessageBox.Show("Ошибка: ID поезда должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: ID поезда должен быть положительным числом.");
                return false;
            }

            // Проверка ID вагона
            if (p.IdVan <= 0)
            {
                MessageBox.Show("Ошибка: ID вагона должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: ID вагона должен быть положительным числом.");
                return false;
            }

            // Проверка номера места
            if (p.SeatNumber <= 0)
            {
                MessageBox.Show("Ошибка: Номер места должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Номер места должен быть положительным числом.");
                return false;
            }

            // Проверка места отправления
            if (p.FromWhere <= 0)
            {
                MessageBox.Show("Ошибка: Место отправления должно быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Место отправления должно быть положительным числом.");
                return false;
            }

            // Проверка места прибытия
            if (p.ToWhere <= 0 || p.ToWhere == p.FromWhere)
            {
                MessageBox.Show("Ошибка: Место прибытия должно быть положительным числом и не должно равнятся точке отправления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Место прибытия должно быть положительным числом.");
                return false;
            }

            // Проверка даты
            if (p.Date < DateTime.Now)
            {
                MessageBox.Show("Ошибка: Дата не может быть в будущем.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Дата не может быть в будущем.");
                return false;
            }

            // Проверка стоимости
            if (p.Cost <= 0)
            {
                MessageBox.Show("Ошибка: Стоимость должна быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Стоимость должна быть положительным числом.");
                return false;
            }
            return true;
        }

        public static bool CheckOrder(TakeTicket p)
        {
            // Проверка ID пассажира
            if (p.IdPassenger <= 0)
            {
                MessageBox.Show("Ошибка: ID пассажира должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: ID пассажира должен быть положительным числом.");
                return false;
            }

            // Проверка ID поезда
            if (p.IdTrain <= 0)
            {
                MessageBox.Show("Ошибка: ID поезда должен быть положительным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: ID поезда должен быть положительным числом.");
                return false;
            }

            // Проверка ID вагона
            if (p.IdVan <= 0)
            {
                MessageBox.Show("Ошибка: ID вагона должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: ID вагона должен быть положительным числом.");
                return false;
            }

            // Проверка номера места
            if (p.SeatNumber <= 0)
            {
                MessageBox.Show("Ошибка: Номер места должен быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Номер места должен быть положительным числом.");
                return false;
            }

            // Проверка места отправления
            if (p.FromWhere <= 0)
            {
                MessageBox.Show("Ошибка: Место отправления должно быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Место отправления должно быть положительным числом.");
                return false;
            }

            // Проверка места прибытия
            if (p.ToWhere <= 0 || p.ToWhere == p.FromWhere)
            {
                MessageBox.Show("Ошибка: Место прибытия должно быть положительным числом и не должно равнятся месту отправления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Место прибытия должно быть положительным числом.");
                return false;
            }

            // Проверка даты
            if (p.Date < DateTime.Now)
            {
                MessageBox.Show("Ошибка: Дата не может быть в прошлом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Дата не может быть в будущем.");
                return false;
            }

            // Проверка стоимости
            if (p.Cost <= 0)
            {
                MessageBox.Show("Ошибка: Стоимость должна быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Стоимость должна быть положительным числом.");
                return false;
            }
            if (p.DatePay > DateTime.Now)
            {
                MessageBox.Show("Ошибка: Дата оплаты не может быть в будущем.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Дата оплаты не может быть в будущем."
                return false;
            }

            // Проверка статуса
            if (p.Status != 'R' && p.Status != 'S' && p.Status != 'W')
            {
                MessageBox.Show("Ошибка: Статус может быть только 'R', 'S' или 'W'.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Статус может быть только 'R', 'S' или 'W'."
                return false;
            }
            return true;
        }
        public static bool CheckTrain(Train p)
        {
            // Проверка категории поезда
            if (string.IsNullOrEmpty(p.CategoryOfTrain))
            {
                MessageBox.Show("Ошибка: Категория поезда не может быть пустой.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Категория поезда не может быть пустой.");
                return false;
            }

            // Проверка вагонов
            if (string.IsNullOrEmpty(p.Vans))
            {
                MessageBox.Show("Ошибка: Вагоны не могут быть пустыми.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Вагоны не могут быть пустыми.");
                return false;
            }

            // Проверка времени стоянки
            if (p.ParkingTime < 0)
            {
                MessageBox.Show("Ошибка: Время стоянки не может быть отрицательным.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Время стоянки не может быть отрицательным.");
                return false;
            }

            // Проверка количества вагонов
            if (p.CountOfVans <= 0)
            {
                MessageBox.Show("Ошибка: Количество вагонов должно быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Количество вагонов должно быть положительным числом.");
                return false;
            }
            if ((p.Vans.Split(',')).Length != p.CountOfVans)
            {
                MessageBox.Show("Должно быть правильное число вагонов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Должно быть правильное число вагонов");
                return false;
            }
            return true;
        }
        public static bool CheckVan(Van p)
        {
            // Проверка типа
            if (string.IsNullOrEmpty(p.Type))
            {
                MessageBox.Show("Ошибка: Тип вагона не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Тип не может быть пустым.");
                return false;
            }

            // Проверка вместимости
            if (p.Capacity < 0)
            {
                MessageBox.Show("Ошибка: Вместимость должна быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //"Ошибка: Вместимость должна быть положительным числом.");
                return false;
            }
            return true;
        }
    }


}
