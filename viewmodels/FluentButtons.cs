using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix.viewmodels
{
    public static class FluentButtons
    {
        public static WixButtonViewModel Show( this WixButtonViewModel vm )
        {
            vm.Visibility = Visibility.Visible;
            return vm;
        }

        public static WixButtonViewModel Hide( this WixButtonViewModel vm)
        {
            vm.Visibility=Visibility.Collapsed;
            return vm;
        }

        public static WixButtonViewModel SetText( this WixButtonViewModel vm, string text )
        {
            vm.Text = text;
            return vm;
        }

        public static WixButtonViewModel SetID(this WixButtonViewModel vm, string id)
        {
            vm.ButtonID = id;
            return vm;
        }
    }
}
