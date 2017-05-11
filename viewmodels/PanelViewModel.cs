using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Olbert.Wix.messages;

namespace Olbert.Wix.ViewModels
{
    public abstract class PanelViewModel : ViewModelBase
    {
        public abstract ButtonsViewModel GetButtonsViewModel();
    }
}
