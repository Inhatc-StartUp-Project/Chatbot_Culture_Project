using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using Culture_ChatBot.Dialogs;
using System.Collections.Generic;
using Culture_ChatBot.Dialog;

namespace Culture_ChatBot  // 시작 다이얼로그
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        protected int count = 1;
        string strMessage;
        private string strWelcomeMessage = "[Culture ChatBot]";

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync(strWelcomeMessage); // return our reply to the user

            var message = context.MakeMessage();    // Create message
            var actions = new List<CardAction>();   // Create List

            actions.Add(new CardAction() { Title = "1. 지도에서 검색", Value = "1", Type = ActionTypes.ImBack });
            actions.Add(new CardAction() { Title = "2. 즐겨찾기", Value = "2", Type = ActionTypes.ImBack });

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
                context.Call(new MapSearchDialog(), DialogResumeAfter);
            }
            else if (strSelected == "2")
            {
                strMessage = "[즐겨찾기로 검색] 전화번호를 입력해주세요. >";
                await context.PostAsync(strMessage);

                context.Call(new BookmarkSearchDialog(), DialogResumeAfter);
            }
            else
            {
                context.Wait(MessageReceivedAsync);
            }
        }

        public async Task DialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                strMessage = await result;

                await context.PostAsync(strWelcomeMessage);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Error occurred....");
            }
        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                this.count = 1;
                await context.PostAsync("Reset count.");
            }
            else
            {
                await context.PostAsync("Did not reset count.");
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}