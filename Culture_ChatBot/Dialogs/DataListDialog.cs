using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;               // Add to process Async Task

using Microsoft.Bot.Connector;              // Add for Activity Class
using Microsoft.Bot.Builder.Dialogs;        // Add for Dialog Class
using System.Net.Http;                      // Add for internet

using Microsoft.Bot.Builder.Luis;           // Microsoft.Cognitive.Luis
using Microsoft.Bot.Builder.Luis.Models;    // Microsoft.Cognitive.Luis
using Culture_ChatBot.Helpers;


namespace Culture_ChatBot
{
    [Serializable]
    public class DataListDialog : IDialog<string>
    {
        string dummy = "";

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;

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
            else if (activity.Text.Trim().Contains("bookmark"))
            {
                dummy = activity.Text.Trim().Replace("bookmark ", "");
                string message = "휴대전화 번호를 입력해 주십시오.";
                await context.PostAsync(message);
            }
            else if (activity.Text.Trim().Contains("content"))
            {
                try
                {
                    int count = 0;
                    List<List<Dictionary<string, string>>> contentList = new CsvHelper().GetCsvData();
                    foreach (List<Dictionary<string, string>> content in contentList)
                    {
                        foreach (Dictionary<string, string> value in content)
                        {
                            bool b;
                            string eventNm = value["eventNm"].Replace(" ", "");
                            string activitymsg = activity.Text.Trim().Replace("content ", "").Replace(" ", "");
                            b = eventNm.Contains(activitymsg);
                            if (b)
                            {
                                count++;

                                #region try_catch
                                try { string strMessage = value["eventNm"]; }
                                catch (Exception e) { value["eventNm"] = "없음"; }
                                try { string strMessage = value["opar"]; }
                                catch (Exception e) { value["opar"] = "없음"; }
                                try { string strMessage = value["eventCo"]; }
                                catch (Exception e) { value["eventCo"] = "없음"; }
                                try { string strMessage = value["eventStartDate"]; }
                                catch (Exception e) { value["eventStartDate"] = "없음"; }
                                try { string strMessage = value["eventEndDate"]; }
                                catch (Exception e) { value["eventEndDate"] = "없음"; }
                                try { string strMessage = value["eventStartTime"]; }
                                catch (Exception e) { value["eventStartTime"] = "없음"; }
                                try { string strMessage = value["eventEndTime"]; }
                                catch (Exception e) { value["eventEndTime"] = "없음"; }
                                try { string strMessage = value["chrgeInfo"]; }
                                catch (Exception e) { value["chrgeInfo"] = "없음"; }
                                try { string strMessage = value["phoneNumber"]; }
                                catch (Exception e) { value["phoneNumber"] = "없음"; }
                                try { string strMessage = value["seatNumber"]; }
                                catch (Exception e) { value["seatNumber"] = "없음"; }
                                try { string strMessage = value["admfee"]; }
                                catch (Exception e) { value["admfee"] = "없음"; }
                                try { string strMessage = value["entncAge"]; }
                                catch (Exception e) { value["entncAge"] = "없음"; }
                                try { string strMessage = value["stpn"]; }
                                catch (Exception e) { value["stpn"] = "없음"; }
                                try { string strMessage = value["advantkInfo"]; }
                                catch (Exception e) { value["advantkInfo"] = "없음"; }
                                #endregion

                                var message = context.MakeMessage();
                                message.Attachments.Add(CardHelper.GetHeroCard(value["eventNm"], value["opar"], value["eventCo"],
                                                                               value["eventStartDate"], value["eventEndDate"],
                                                                               value["eventStartTime"], value["eventEndTime"],
                                                                               value["chrgeInfo"], value["phoneNumber"],
                                                                               value["seatNumber"], value["admfee"],
                                                                               value["entncAge"], value["stpn"], value["advantkInfo"],
                                                                               value["latitude"], value["longitude"]));
                                /*
                                 * eventNm: 행사명
                                 * opar: 장소
                                 * eventCo: 행사 정보
                                 * eventStartDate: 
                                 * eventEndDate:
                                 * eventStartTime:
                                 * eventEndTime:
                                 * chrgeInfo: 요금
                                 * phoneNumber: 전화번호
                                 * seatNumber: 좌석 수
                                 * admfee: 가격
                                 * entncAge: 연령
                                 * atpn: 유의사항
                                 * advantkInfo: 홈페이지 주소
                                 * latitude: 위도
                                 * longitude: 경도
                                 */
                                await context.PostAsync(message);
                            }
                        }
                    }
                    if (count == 0)
                    {
                        string message = "정보가 없습니다. 다시 입력하거나 '뒤로가기'를 입력하십시오.";
                        await context.PostAsync(message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("실패");
                    Console.WriteLine(e);
                    await context.PostAsync("xxx");
                    context.Wait(MessageReceivedAsync);
                }
            }

            else
            {
                try
                {
                    var message = context.MakeMessage();
                    List<List<Dictionary<string, string>>> contentList = new CsvHelper().GetCsvData();
                    foreach (List<Dictionary<string, string>> content in contentList)
                    {
                        foreach (Dictionary<string, string> value in content)
                        {

                            message.Attachments.Add(CardHelper.GetThumbnailCard(value["eventNm"], value["eventCo"],
                                                                            value["eventStartDate"], value["eventEndDate"],
                                                                            value["opar"], value["latitude"], value["longitude"]));
                            /*
                            * eventNm: 행사명
                            * opar: 장소
                            * eventCo: 행사 정보
                            * eventStartDate: 
                            * eventEndDate:
                            * eventStartTime:
                            * eventEndTime:
                            * chrgeInfo: 요금
                            * phoneNumber: 전화번호
                            * seatNumber: 좌석 수
                            * admfee: 가격
                            * entncAge: 연령
                            * atpn: 유의사항
                            * advantkInfo: 홈페이지 주소
                            * latitude: 위도
                            * longitude: 경도
                            */
                        }
                    }
                    message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    await context.PostAsync(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("실패");
                    Console.WriteLine(e);
                    await context.PostAsync("xxx");
                    context.Wait(MessageReceivedAsync);
                }
            }
        }
    }
}