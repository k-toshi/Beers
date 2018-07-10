using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beers.Models;
using Beers.Controllers;

namespace Beers.ViewModels
{
    public class PubsViewModel : BaseViewModel
    {
        public ObservableCollection<PubView> Pubs { get; set; }
        public Command LoadPubsCommand { get; set; }
        public CommonDef.PubsViewMode pubsViewMode { get; private set; }

        public PubsViewModel(CommonDef.PubsViewMode pvm )
        {
            pubsViewMode = pvm;

            Pubs = new ObservableCollection<PubView>();
            LoadPubsCommand = new Command(async () => await ExecuteLoadPubsCommand());
            
        }

        async Task ExecuteLoadPubsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Pubs.Clear();
                await SetPubs();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SetPubs()
        {
            var pubs = new ObservableCollection<PubView>();

            switch (pubsViewMode)
            {
                case CommonDef.PubsViewMode.UserPub:
                    Title = "店舗一覧";
                    pubs = await UserController.GetUserPubs();
                    break;
                default:
                    break;
            }

            foreach (var pub in pubs) Pubs.Add(pub);

        }

    }
}
