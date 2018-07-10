using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beers.Models;
using Beers.Controllers;

namespace Beers.ViewModels
{
    public class TransactionHistoryViewModel : BaseViewModel
    {
        public ObservableCollection<Transaction> TransactionHistory { get; set; }
        public Command LoadTransactionHistoryCommand { get; set; }

        public TransactionHistoryViewModel()
        {
            Title = "取引履歴";
            TransactionHistory = new ObservableCollection<Transaction>();
            LoadTransactionHistoryCommand = new Command(async () => await ExecuteLoadTransactionHistoryCommand());
        }

        async Task ExecuteLoadTransactionHistoryCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                TransactionHistory.Clear();
                var transactions = await UserController.GetTransactionHistory();
                foreach (var trans in transactions) TransactionHistory.Add(trans);
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
    }
}
