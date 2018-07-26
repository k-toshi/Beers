using System;
using System.Threading.Tasks;
using Beers.Services;
using Beers.Models;
using Beers.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Beers.Controllers
{
    public class EventController
    {
        public EventController()
        {
        }

		public static async Task<ObservableCollection<EventView>> GetNotParticipatedEvents(int pubId)
        {
            HttpServiceResult<ObservableCollection<EventView>> hsr =
                await HttpService.GetDataFromServiceWithToken<ObservableCollection<EventView>>
                                 ("api/events/GetEventsWithType?type=0&apptype=1&pubid=" + pubId);

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }

		public static async Task<ObservableCollection<EventView>> GetParticipatedEvents(int pubId)
        {
			HttpServiceResult<ObservableCollection<EventView>> hsr =
				await HttpService.GetDataFromServiceWithToken<ObservableCollection<EventView>>
				                 ("api/events/GetEventsWithType?type=1&apptype=1&pubid=" + pubId);

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }

		public static async Task<ObservableCollection<EventView>> GetAllEvents()
        {
            HttpServiceResult<ObservableCollection<EventView>> hsr =
                await HttpService.GetDataFromServiceWithToken<ObservableCollection<EventView>>
                                 ("api/events/GetEventsWithType?type=2&apptype=1");

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }

		public static async Task<ObservableCollection<EventView>> GetParticipatedEventsOfEnd(int pubId)
        {
            HttpServiceResult<ObservableCollection<EventView>> hsr =
                await HttpService.GetDataFromServiceWithToken<ObservableCollection<EventView>>
                                 ("api/events/GetEventsWithType?type=3&apptype=1&pubid=" + pubId);

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }

		public static async Task<ObservableCollection<EventView>> GetAllNotStartedEvents()
        {
            HttpServiceResult<ObservableCollection<EventView>> hsr =
                await HttpService.GetDataFromServiceWithToken<ObservableCollection<EventView>>
                                 ("api/events/GetEventsWithType?type=4&apptype=1");

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }

		public static async Task<EventUserView> ApplyEvent(int eventId,int pubId,string appType)
        {
			Dictionary<string, string> contents = new Dictionary<string, string>
            {
                {"EventId",eventId.ToString()},
                {"PubId",pubId.ToString()},
                {"UserId",UserController.LoginUser.Id},
				{"AppType",appType},
				{"CreateDateTime",DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}
            };

			HttpServiceResult<EventUserView> hsr = await HttpService.PostDataFromServiceWithToken<EventUserView>("api/EventUsers", contents);

			if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }

		public static async Task<ObservableCollection<EventSituationView>> GetEventSituation(int eventId)
        {         
			HttpServiceResult<ObservableCollection<EventSituationView>> hsr = await HttpService.GetDataFromService<ObservableCollection<EventSituationView>>("api/EventSituation/" + eventId);
            
            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }

		public static async Task<ObservableCollection<EventUserCountView>> GetEventUserCount(int eventId)
        {
			HttpServiceResult<ObservableCollection<EventUserCountView>> hsr = await HttpService.GetDataFromService<ObservableCollection<EventUserCountView>>("api/EventUserCount/" + eventId);

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }

		public static async Task<ObservableCollection<WinPubView>> GetWinPubs()
		{
			HttpServiceResult<ObservableCollection<WinPubView>> hsr = await HttpService.GetDataFromServiceWithToken<ObservableCollection<WinPubView>>("api/winpubs/getwinpubs?eventid=" + UserController.LoginUser.EventId);

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }
        
		public static async Task<ObservableCollection<EventHeaderView>> GetRecommendedEventHeader()
        {
            HttpServiceResult<ObservableCollection<EventHeaderView>> hsr = await HttpService.GetDataFromService<ObservableCollection<EventHeaderView>>("api/eventheaderview?apptype=1");

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }

		public static async Task<ObservableCollection<EventHeaderView>> GetEventHeader(int eventId)
        {
			HttpServiceResult<ObservableCollection<EventHeaderView>> hsr = await HttpService.GetDataFromService<ObservableCollection<EventHeaderView>>("api/eventheaderview/geteventpubs?id=" + eventId +"&apptype=1");

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }

		public static async Task<ObservableCollection<EventDetailView>> GetRecommendedEventDetail()
        {
			HttpServiceResult<ObservableCollection<EventDetailView>> hsr = await HttpService.GetDataFromService<ObservableCollection<EventDetailView>>("api/eventdetailview?apptype=1");

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }
        
		public static async Task<ObservableCollection<EventDetailView>> GetEventDetail(int eventId)
        {
			HttpServiceResult<ObservableCollection<EventDetailView>> hsr = await HttpService.GetDataFromService<ObservableCollection<EventDetailView>>("api/eventdetailview/geteventpubs?id=" + eventId + "&apptype=1");

            if (!hsr.IsError) return hsr.ResultData;
            else return null;
        }
    }
}
