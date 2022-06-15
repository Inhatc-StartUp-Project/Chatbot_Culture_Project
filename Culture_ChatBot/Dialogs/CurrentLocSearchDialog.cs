using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;           // Add to process Async Task
using Microsoft.Bot.Connector;          // Add for Activity Class
using Microsoft.Bot.Builder.Dialogs;    // Add for Dialog 
using System.Net.Http;                  // Add for internet
using Culture_ChatBot.Helpers;               // Add for CardHelper
using System.Data;                      // Add for DB Connection
using System.Data.SqlClient;            // Add for DB Connection
using Culture_ChatBot.Model;                  // Add for Model
using System.Diagnostics;

namespace Culture_ChatBot.Dialogs  // 현재 위치에서 검색 다이얼로그
{
    [Serializable]
    public class CurrentLocSearchDialog : IDialog<string>
    {
        string strSelected;
        string dummy = "";

        //MyLocation user = new MyLocation();
        //private string strWelcomeMessage = "[검색을 시도합니다.]";

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("[현재 위치에서 검색]");

            var message = context.MakeMessage();            //Create message
            var actions = new List<CardAction>();           //Create List

            actions.Add(new CardAction() { Title = "위치정보 제공 동의", Value = "1", Type = ActionTypes.ImBack });

            //Create Hero Card & attachment
            message.Attachments.Add(new HeroCard { Title = "검색을 시도합니다.", Buttons = actions }.ToAttachment());

            //return our reply to the user
            await context.PostAsync(message);    // return our reply to the us

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            MyLocation user = new MyLocation();
            user.GetLocationProperty();
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if (activity.Text.Trim().ToUpper() == "EXIT" || activity.Text.Trim().ToUpper() == "Q" || activity.Text.Trim().Contains("뒤로"))
            {
                context.Done("Culture ChatBot Finish");
            }
            else if (activity.Text.Trim().Contains("010"))
            {
                var card = new List<CardAction>();
                card.Add(new CardAction() { Title = "뒤로가기", Value = "exit", Type = ActionTypes.ImBack });
                var messageCard = context.MakeMessage();
                messageCard.Attachments.Add(new HeroCard
                {
                    Title = dummy,
                    Text = "즐겨찾기에 추가 되었습니다.",
                    Buttons = card
                }.ToAttachment());
                await context.PostAsync(messageCard);
            }
            else if (activity.Text.Trim().Contains("Bookmark"))
            {
                dummy = activity.Text.Trim().Replace("Bookmark ", "");
                string messagePhone = "휴대전화 번호를 입력해 주십시오.";
                await context.PostAsync(messagePhone);
            }
            else if (strSelected == "1")
            {
                //context.Wait(SendWelcomeMessageAsync);


                                                                //var map = new List<CardAction>();           //Create List

                string apiKey = "AIzaSyDZONqGZJWjsnLOruZcXbFAR5j8h71z7DM";

                user.GetLocationProperty();

                // 37.4490713
                // 126.6571986

                double lat = 37.4490713;
                double lng = 126.6571986;
                Debug.WriteLine("Latitude: {0}, Longitude {1}", lat, lng);

                string hor = "37.48863335"; // 목적지
                string ver = "126.7706577"; // 목적지

                string serch_marker = "size:mid%7Ccolor:blue%7C";
                string user_marker = "size:mid%7Ccolor:red%7C";

                string path = "color:0x0000ff%7Cweight:5%7C" +
                    lat + ',' + lng + "%7C" + hor + "," + ver; // 사용자 위치 -> 목적지 위치

                // 
                string temp_url = "https://maps.googleapis.com/maps/api/staticmap?" +
                    "center=63.259591,-144.667969&zoom=6" +
                    "&size=400x400&markers=color:blue" +
                    "%7Clabel:S%7C62.107733,-145.541936" +
                    "&key=" + apiKey;

                double avg_lat = (lat + double.Parse(hor)) / 2; // 정적맵 이미지 시점을 중앙으로 두기 위함임
                double avg_lng = (lng + double.Parse(ver)) / 2;

                // 사용자위치만
                string userLocationImageURL = "https://maps.googleapis.com/maps/api/staticmap?center=" +
                                               lat + "," + lng +
                                               "&zoom=16&size=400x400&" +
                                               "&markers=" + user_marker + lat + "," + lng +
                                               "&key=" +
                                               apiKey;
                // 목적지 위치
                string destinationURL = "https://maps.googleapis.com/maps/api/staticmap?center=" +
                                        hor + "," + ver +
                                        "&zoom=16&size=400x400&" +
                                        "&markers=" + serch_marker + hor + "," + ver +
                                        "&key=" +
                                        apiKey;


                // 사용자 -> 목적지 경로 포함 맵 출력
                string RouteimageURL = "https://maps.googleapis.com/maps/api/staticmap" +
                                   "?center=" + avg_lat + "," + avg_lng +
                                   "&zoom=13" + "&size=400x400" +
                                   "&markers=" + user_marker + lat + "," + lng +
                                   "&markers=" + serch_marker + hor + "," + ver +
                                   "&path=" + path +
                                   "&key=" + apiKey;


                var message = context.MakeMessage();            //Create message

                String directionURL = "https://www.google.co.kr/maps/place/" + hor + "," + ver;


                var mapimage = new List<CardImage>
                {
                    //new CardImage (url:userLocationImageURL),
                    //new CardImage (url:destinationURL),
                    new CardImage (url:RouteimageURL)
                };           //Create List
                var actions = new List<CardAction>();           //Create List
                actions.Add(new CardAction() { Title = "구글맵 열기", Value = directionURL, Type = ActionTypes.OpenUrl });
                actions.Add(new CardAction() { Title = "즐겨찾기", Value = "Bookmark 청소년을 위한 스쿨클래식", Type = ActionTypes.ImBack });
                message.Attachments.Add(new HeroCard {
                    Title = "청소년을 위한 스쿨클래식",
                    Images = mapimage,
                    Subtitle = "가장 가까운 곳",
                    Text = "공연 내용: 부천필 연주회 \r\r" +
                           "시작 날짜: 2022-08-18 \r\r" +
                           "종료 날짜: 2022-08-18 \r\r" +
                           "행사 장소: 부천시민회관 대공연장" +
                           "홈페이지 주소: http://www.bucheonphil.or.kr/",
                    Buttons =actions
                }.ToAttachment());

                message.AttachmentLayout = "carousel";
                await context.PostAsync(message);

            }
        }

        public async Task SendWelcomeMessageAsync(IDialogContext context, IAwaitable<object> result)
        {
            


        }



    }
}