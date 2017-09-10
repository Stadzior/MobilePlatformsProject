using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePlatformsProject.Interfaces
{
    public interface INavigatableViewModel
    {
        void OnNavigateTo(object parameter = null);    
    }
}
