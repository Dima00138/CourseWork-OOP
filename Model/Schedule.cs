﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using CourseWork.Services;

namespace CourseWork.Model
{
    public class Schedule
    {
        public int Id { get; set; }
        public long? IdTrain { get; set; }
        public DateTime Date { get; set; }
        public long Route { get; set; }
        private string _frequency = string.Empty;

        public void SetFrequency(short frequency)
        {
            switch (frequency)
            {
                case 1: _frequency = "Каждый день"; break;
                case 2: _frequency = "Каждый нечетный день"; break;
                case 3: _frequency = "Каждый четный день"; break;
                case 4: _frequency = "Единожды"; break;
            }
        }

        public short GetFrequency()
        {
            switch (_frequency)
            {
                case "Каждый день": return 1;
                case "Каждый нечетный день": return 2;
                case "Каждый четный день": return 3;
                case "Единожды": return 4;
            }
            return -1;
        }

        public static bool Update(object sender, DataGridCellEditEndingEventArgs e, OracleContext Conn)
        {
            var item = e.Row.DataContext as Schedule;
            if (e.Column.Header.ToString()?.ToLower() == "id")
            {
                MessageBox.Show("Редактировать Id нельзя");
                e.Cancel = true;
                return false;
            }
            if (item.Id == 0) return true;
            string? col = e.Column.Header.ToString();
            string newVal = (e.EditingElement as TextBox).Text;
            Repository<Schedule> Rep = new ScheduleRepository(Conn);
            Rep.Update(item, col, newVal);
            return true;
        }

        public static bool Delete(object sender, KeyEventArgs e, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;

            if (dataGrid.SelectedCells[0].Item != null)
            {
                var item = dataGrid.SelectedCells[0].Item as Schedule;
                Repository<Schedule> Rep = new ScheduleRepository(Conn);
                Rep.Delete(item, item.Id);
                return true;
            }            
            return false;
        }

        public static bool Insert(object sender, OracleContext Conn)
        {
            DataGrid dataGrid = (DataGrid)sender;
            var item = dataGrid.Items[dataGrid.Items.Count - 2] as Schedule;

            if (item.Id == 0)
            {
                if (!Checks.CheckSchedule(item))
                {
                    MessageBox.Show("Ошибка валидации");
                }
                Repository<Schedule> Rep = new ScheduleRepository(Conn);
                Rep.Create(item);
                return true;
            }
            return false;
        }
    }
}
