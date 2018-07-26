using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using Beers.Models;
using Beers.Controllers;

namespace Beers.Views
{
    public partial class MenuPage : ContentPage
    {
        public List<BeersMenu> MenuList { get; set; }

        public MenuPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await ExcuteDisplayAlert();

            CreateMenu();
        }

        private void CreateMenu()
        {
            trMenu.Clear();
            SetMenuList();
            int targetRole = 0;
            if (UserController.LoginUser != null) targetRole = UserController.LoginUser.Role;
            //targetRole = 3;
            for (int i = 0 ; i <= targetRole ; i++)
                CreateMenuForTargetAuthLevel(i);

            if(trMenu.Count==0)
            {
                TableSection ts = new TableSection();
                ts.Title = "表示できるメニューがありません。";
                trMenu.Add(ts);
            }
        }

        private void SetMenuList()
        {
            MenuList = new List<BeersMenu>
            {
				new BeersMenu { Id="001", Title="テスト", Detail="テスト", Order=1, UserAuthLevel = 0, PageName = "SearchPubsOnMapPage",Icon="Step_icon.png",Section="001"},
				new BeersMenu { Id="002", Title="イベント検索", Detail="イベントを検索!", Order=2, UserAuthLevel = 0,PageName = "EventsPage,1",Icon="Step_icon.png",Section="001"},
				new BeersMenu { Id="003", Title="付近検索", Detail="付近のイベント開催店を検索!", Order=3, UserAuthLevel = 0,PageName = "SearchPubsOnMapPage",Icon="Step_icon.png",Section="001"},
				new BeersMenu { Id="101", Title="歩数計", Detail="歩いてポイントをゲット！", Order=1, UserAuthLevel = 1,PageName = "StepCounterPage",Icon="Step_icon.png",Section="101"},
				new BeersMenu { Id="102", Title="イベント状況", Detail="参加イベントの状況をチェック!", Order=1, UserAuthLevel = 1,PageName = "EventSituationViewPage",Icon="Step_icon.png",Section="102"},
				new BeersMenu { Id="103", Title="QRコード読取", Detail="QRコード読取ります", Order=1, UserAuthLevel = 1,PageName = "QrReaderPage",Icon="Qr_icon.png",Section="109"},
				new BeersMenu { Id="201", Title="店舗商品照会", Detail="店舗の商品照会および取引を行います", Order=1, UserAuthLevel = 2,PageName = "PubsPage,0",Icon="List_icon.png",Section="201"},
				new BeersMenu { Id="202", Title="参加イベント", Detail="参加しているイベントの確認", Order=2, UserAuthLevel = 2,PageName = "PubsPage,1",Icon="List_icon.png",Section="201"},
            };
        }

        private void CreateMenuForTargetAuthLevel(int authLevel)
        {
			string tscode = "";
			string beforeTscode = "";
			List<BeersMenu> sameGroupList = new List<BeersMenu>();
            
			foreach(var menu in MenuList.Where(a => a.UserAuthLevel == authLevel).OrderBy(a => a.Section).OrderBy(a => a.Order))
            {
				tscode = menu.Section;
				if (tscode != beforeTscode){   
					if (!string.IsNullOrEmpty(beforeTscode)) {
						CreateMenuSection(sameGroupList);
						sameGroupList = new List<BeersMenu>();
					}
				}   
                sameGroupList.Add(menu);
				beforeTscode = menu.Section;
            }    

			if(sameGroupList.Count>0) CreateMenuSection(sameGroupList);
        }

		private void CreateMenuSection(List<BeersMenu> menus)
		{
			TableSection ts = new TableSection();
			ts.Title = GetTableSectionTitle(menus[0].Section);
            foreach(BeersMenu menu in menus)
			{
				ViewCell viewCell = CreateMenuViewCell(menu);
                ts.Add(viewCell);
			}
			trMenu.Add(ts);
		}

        private ViewCell CreateMenuViewCell(BeersMenu menu)
        {
            var icon = new Image();
            icon.Source = menu.Icon;

            var title = new Label() { FontSize = 15,TextColor=Color.Black };
            title.Text = menu.Title;
            var detail = new Label() { FontSize = 12 };
            detail.Text = menu.Detail;
            detail.TextColor = Color.Gray;
            
            //No Messageを縦に並べたテキストレイアウトを作成する
            var textLayout = new StackLayout
            {
				Padding = new Thickness(0, 0, 0, 1),
                Children = { title, detail }
            };

            ViewCell viewCell = new ViewCell
            {
                View = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,//Iconとテキストレイアウトを横に並べる
                    Padding = new Thickness(10,2,0,0),//パディング
                    //Spacing = 10,//スペース
                    Children = { icon,textLayout },
                },
            };

            viewCell.Tapped += OnTapped;
            viewCell.AutomationId = menu.PageName;

            return viewCell;

            /*
            Grid grid = new Grid();

            RowDefinitionCollection rowDefinitions = new RowDefinitionCollection();
            RowDefinition rowDefinition1 = new RowDefinition()
            {Height = new GridLength(15)};
            RowDefinition rowDefinition2 = new RowDefinition()
            {Height = new GridLength(15)};
            rowDefinitions.Add(rowDefinition1);
            rowDefinitions.Add(rowDefinition2);

            ColumnDefinitionCollection columnDefinitions = new ColumnDefinitionCollection();
            ColumnDefinition columnDefinition1 = new ColumnDefinition()
            { Width = new GridLength(30)};
            ColumnDefinition columnDefinition2 = new ColumnDefinition()
            { Width = new GridLength(1, GridUnitType.Auto) };
            columnDefinitions.Add(columnDefinition1);
            columnDefinitions.Add(columnDefinition2);

            Image image = new Image()
            {
                Source = menu.Icon,   
                Aspect = Aspect.AspectFill,

            };
            Label label1 = new Label()
            {
                Text = menu.Title, 
                LineBreakMode = LineBreakMode.TailTruncation
            };
            Label label2 = new Label()
            {
                Text = menu.Detail,
                TextColor = Color.Gray,
                FontSize = 10,
                LineBreakMode = LineBreakMode.TailTruncation
            };

            grid.Children.Add(image, 0, 0);
            Grid.SetRowSpan(image, 2);
            grid.Children.Add(label1,1,0);
            grid.Children.Add(label2, 1, 1);

            viewCell.View = grid;
            viewCell.Tapped += OnTapped;
            viewCell.AutomationId = menu.PageName;

            return viewCell;
            */

        }

        private string GetTableSectionTitle(string tscode)
        {
			switch (tscode)
            {
				case "001":
                    return "一般利用者";
                case "101":
                    return "歩数計";
                case "102":
                    return "ブラット";
                case "109":
                    return "共通";
                case "201":
                    return "店舗";
                default:
                    return "";
            }
        }

        private async void OnTapped(Object sender, EventArgs e)
        {
            ViewCell viewCell = (ViewCell)sender;
            List<string> bufs = viewCell.AutomationId.ToString().Split(',').ToList();

            string pageName = bufs[0];
            Type target = Type.GetType("Beers.Views." + pageName);
            Page page = new Page();

            if (bufs.Count() > 1)
            {
                string[] viewparams = bufs.GetRange(1, bufs.Count() - 1).ToArray();
                page = (Page)Activator.CreateInstance(target, new object[] { viewparams });
            }
            else page = (Page)Activator.CreateInstance(target);

            await Navigation.PushAsync(page);
            
        }

        private async Task ExcuteDisplayAlert()
        {
            if(DisplayAlertManager.IsAlert)
            {
                await DisplayAlert(DisplayAlertManager.Title, DisplayAlertManager.Message, DisplayAlertManager.Button);
                DisplayAlertManager.IsAlert = false;
            }
        }


    }
}
