using MobilePlatformsProject.Models.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePlatformsProject.Models
{
    public class Currency : NotifyPropertyChangedBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                SetField(ref _name, value);
            }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                SetField(ref _code, value);
            }
        }

        private double _exchangeRate;
        public double ExchangeRate
        {
            get { return _exchangeRate; }
            set
            {
                SetField(ref _exchangeRate, value);
            }
        }

        public ObservableCollection<Rate> Rates { get; set; }
    }
}
