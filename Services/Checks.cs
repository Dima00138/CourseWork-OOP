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
                //"Ошибка: ID должен быть положительным числом."
                return false;
            }

            // Проверка льгот
            if (p.Benefits < 0 || p.Benefits > 100)
            {
                //"Ошибка: Льготы не могут быть отрицательными и превышать 100." 
                return false;
            }

            // Проверка полного имени
            Regex fullNameRegex = new Regex(@"^[A-Za-zА-Яа-я]+\s[A-Za-zА-Яа-я]+\s[A-Za-zА-Яа-я]+$"); // Пример формата: Имя Отчество Фамилия
            if (string.IsNullOrEmpty(p.FullName) || !fullNameRegex.IsMatch(p.FullName))
            {
                //Ошибка: Полное имя должно состоять из трех слов, разделенных пробелами."
                return false;
            }
            return true;
        }
        public static bool CheckPayment(Payment p)
        {
            if (p.IdTicket <= 0)
            {
                //"Ошибка: ID билета должен быть положительным числом."
                return false;
            }

            if (p.DatePay > DateTime.Now)
            {
                //"Ошибка: Дата оплаты не может быть в будущем."
                return false;
            }

            // Проверка статуса
            if (p.Status != 'R' && p.Status != 'S' && p.Status != 'W')
            {
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
                //"Ошибка: Точка отправления должна быть положительным числом."
                return false;
            }

            // Проверка точки прибытия
            if (p.ArrivalPoint <= 0 || p.ArrivalPoint == p.DeparturePoint)
            {
                //"Ошибка: Точка прибытия должна быть положительным числом."
                return false;
            }

            // Проверка расстояния
            if (p.Distance <= 0)
            {
                //"Ошибка: Расстояние должно быть положительным числом."
                return false;
            }

            // Проверка продолжительности
            if (p.Duration <= 0)
            {
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
                //"Ошибка: ID поезда должен быть положительным числом."
                return false;
            }

            // Проверка даты
            if (p.Date < DateTime.Now)
            {
                //"Ошибка: Дата не может быть в будущем."
                return false;
            }

            // Проверка маршрута
            if (p.Route <= 0)
            {
                //"Ошибка: Маршрут должен быть положительным числом."
                return false;
            }

            // Проверка частоты
            if (p.GetFrequency() == -1)
            {
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
                //"Ошибка: Название станции не может быть пустым."
                return false;
            }

            // Проверка города
            if (string.IsNullOrEmpty(p.City))
            {
                //"Ошибка: Город не может быть пустым."
                return false;
            }

            // Проверка штата
            if (string.IsNullOrEmpty(p.State))
            {
                //"Ошибка: Штат не может быть пустым."
                return false;
            }

            // Проверка страны
            if (string.IsNullOrEmpty(p.Country))
            {
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
                //"Ошибка: ID маршрута должен быть положительным числом.");
                return false;
            }

            // Проверка ID станции
            if (p.StationId <= 0)
            {
                //"Ошибка: ID станции должен быть положительным числом.");
                return false;
            }

            // Проверка порядка станции
            if (p.StationOrder <= 0)
            {
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
                //"Ошибка: ID пассажира должен быть положительным числом.");
                return false;
            }

            // Проверка ID поезда
            if (p.IdTrain <= 0)
            {
                //"Ошибка: ID поезда должен быть положительным числом.");
                return false;
            }

            // Проверка ID вагона
            if (p.IdVan <= 0)
            {
                //"Ошибка: ID вагона должен быть положительным числом.");
                return false;
            }

            // Проверка номера места
            if (p.SeatNumber <= 0)
            {
                //"Ошибка: Номер места должен быть положительным числом.");
                return false;
            }

            // Проверка места отправления
            if (p.FromWhere <= 0)
            {
                //"Ошибка: Место отправления должно быть положительным числом.");
                return false;
            }

            // Проверка места прибытия
            if (p.ToWhere <= 0 || p.ToWhere == p.FromWhere)
            {
                //"Ошибка: Место прибытия должно быть положительным числом.");
                return false;
            }

            // Проверка даты
            if (p.Date < DateTime.Now)
            {
                //"Ошибка: Дата не может быть в будущем.");
                return false;
            }

            // Проверка стоимости
            if (p.Cost <= 0)
            {
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
                //"Ошибка: ID пассажира должен быть положительным числом.");
                return false;
            }

            // Проверка ID поезда
            if (p.IdTrain <= 0)
            {
                //"Ошибка: ID поезда должен быть положительным числом.");
                return false;
            }

            // Проверка ID вагона
            if (p.IdVan <= 0)
            {
                //"Ошибка: ID вагона должен быть положительным числом.");
                return false;
            }

            // Проверка номера места
            if (p.SeatNumber <= 0)
            {
                //"Ошибка: Номер места должен быть положительным числом.");
                return false;
            }

            // Проверка места отправления
            if (p.FromWhere <= 0)
            {
                //"Ошибка: Место отправления должно быть положительным числом.");
                return false;
            }

            // Проверка места прибытия
            if (p.ToWhere <= 0 || p.ToWhere == p.FromWhere)
            {
                //"Ошибка: Место прибытия должно быть положительным числом.");
                return false;
            }

            // Проверка даты
            if (p.Date < DateTime.Now)
            {
                //"Ошибка: Дата не может быть в будущем.");
                return false;
            }

            // Проверка стоимости
            if (p.Cost <= 0)
            {
                //"Ошибка: Стоимость должна быть положительным числом.");
                return false;
            }
            if (p.DatePay > DateTime.Now)
            {
                //"Ошибка: Дата оплаты не может быть в будущем."
                return false;
            }

            // Проверка статуса
            if (p.Status != 'R' && p.Status != 'S' && p.Status != 'W')
            {
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
                //"Ошибка: Категория поезда не может быть пустой.");
                return false;
            }

            // Проверка вагонов
            if (string.IsNullOrEmpty(p.Vans))
            {
                //"Ошибка: Вагоны не могут быть пустыми.");
                return false;
            }

            // Проверка времени стоянки
            if (p.ParkingTime < 0)
            {
                //"Ошибка: Время стоянки не может быть отрицательным.");
                return false;
            }

            // Проверка количества вагонов
            if (p.CountOfVans <= 0)
            {
                //"Ошибка: Количество вагонов должно быть положительным числом.");
                return false;
            }
            if ((p.Vans.Split(',')).Length != p.CountOfVans)
            {
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
                //"Ошибка: Тип не может быть пустым.");
                return false;
            }

            // Проверка вместимости
            if (p.Capacity < 0)
            {
                //"Ошибка: Вместимость должна быть положительным числом.");
                return false;
            }
            return true;
        }
    }


}
