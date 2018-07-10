﻿using System;
using System.Threading.Tasks;
using Beers.Services;
using Beers.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Beers.Controllers
{
	/// <summary>
    /// Pub controller.
    /// </summary>
    public static class PubController
    {
        public static async Task<ObservableCollection<Category>> GetCategories(int pubId)
        {
            HttpServiceResult<ObservableCollection<Category>> hsr =
                await HttpService.GetDataFromServiceWithToken<ObservableCollection<Category>>
                                 ("api/categories/getcateogry?pubid=" + pubId);

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }

        public static async Task<ObservableCollection<Item>> GetCategoryItems(int pubId,int categoryId)
        {
            HttpServiceResult<ObservableCollection<Item>> hsr =
                await HttpService.GetDataFromServiceWithToken<ObservableCollection<Item>>
                                 ("api/items/getitem?pubid=" + pubId + "&categoryid=" + categoryId);

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }
    }
}
