using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Olbert.Wix.ViewModels;

namespace Olbert.Wix
{
    public class WixAutofacModule<TWixViewModel> : Module
        where TWixViewModel: IWixViewModel
    {
        protected WixAutofacModule()
        {
        }

        protected override void Load( ContainerBuilder builder )
        {
            builder.RegisterType<TWixViewModel>().As<IWixViewModel>().SingleInstance();
        }
    }
}
