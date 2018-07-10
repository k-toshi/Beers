using System;
using System.Threading;

namespace Beers.ViewModels
{
    public class MyViewModel : BaseViewModel
    {
        
        public MyViewModel()
        {

        }

        public void doAction()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Thread.Sleep(10000);
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
