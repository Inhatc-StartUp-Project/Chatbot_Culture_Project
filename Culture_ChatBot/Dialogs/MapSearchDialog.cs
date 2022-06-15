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


namespace Culture_ChatBot.Dialogs  // 지도에서 검색 다이얼로그
{
    [Serializable]
    public class MapSearchDialog : IDialog<string>
    {
        string strMessage;

        public async Task StartAsync(IDialogContext context)
        {
            strMessage = null;

            await this.MessageReceivedAsync(context, null);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            strMessage = "[지도에서 검색]";
            await context.PostAsync(strMessage);

            var message = context.MakeMessage();
            var actions = new List<CardAction>();

            actions.Add(new CardAction() { Title = "1. 현재 위치에서 검색", Value = "1", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. 공연 행사 제목으로 검색", Value = "2", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "3. 전체 목록 검색", Value = "3", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "4. 돌아가기", Value = "4", Type = ActionTypes.ImBack });

            message.Attachments.Add(
                new HeroCard { Title = "검색 방식을 선택해주세요. > ", Buttons = actions }.ToAttachment()
            );

            await context.PostAsync(message);

            context.Wait(SendWelcomeMessageAsync);

            
        }

        public async Task SendWelcomeMessageAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;
            string strSelected = activity.Text.Trim();

            if (strSelected == "1")
            {
                strMessage = "[현재 위치에서 검색]";
                context.PostAsync(strMessage);

                context.Call(new CurrentLocSearchDialog(), DialogResumeAfter);
            }
            else if (strSelected == "2")
            {
                strMessage = "[공연 행사 제목으로 검색] 공연 행사 제목(ex. 가을 콘서트)을 입력해주세요. (종료:q or exit) >";
                await context.PostAsync(strMessage);

                context.Call(new NameSelectDialog(), DialogResumeAfter);
            }
            else if (strSelected == "3")
            {
                var card = new List<CardAction>();
                card.Add(new CardAction() { Title = "전체보기", Value = "전체보기", Type = ActionTypes.ImBack });
                var messageCard = context.MakeMessage();
                messageCard.Attachments.Add(new HeroCard
                {
                    Title = "전체 공연/행사 정보",
                    Buttons = card
                }.ToAttachment());
                await context.PostAsync(messageCard);

                context.Call(new DataListDialog(), DialogResumeAfter);
            }
            else if (strSelected == "4")
            {
                strMessage = "[돌아가기] 이전 선택지로 돌아갑니다.";
                await context.PostAsync(strMessage);

                context.Done("");
                return;
            }
            else
            {
                strMessage = "보기에서 선택해 주십시오.";
                await context.PostAsync(strMessage);

                context.Wait(MessageReceivedAsync);
            }
        }


        public async Task DialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                strMessage = await result;
                await this.MessageReceivedAsync(context, result);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Error occurred....");
            }
        }
    }
}