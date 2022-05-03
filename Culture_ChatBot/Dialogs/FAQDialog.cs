using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading.Tasks;           // Add to process Async Task
using Microsoft.Bot.Connector;          // Add for Activity Class
using Microsoft.Bot.Builder.Dialogs;    // Add for Dialog Class
using System.Threading;
using QnAMakerDialog.Models;            // Add for reference QnAMakerDialog
using QnAMakerDialog;                   // Add for reference QnAMakerDialog

namespace Culture_ChatBot.Dialog
{
    [Serializable]

    //[QnAMakerService(Host, endpointKey, Knowledgebases, MaxAnswers = 0)]
    [QnAMakerService(" https://Culture_ChatBotqna1.azurewebsites.net/qnamaker",
        "40f3451b-3b9d-4723-9244-f740c6d1c1db", "4623d313-a8ef-465f-ac0d-523a0fac0be9",
        MaxAnswers = 5)]
    public class FAQDialog : QnAMakerDialog<string>
    {
        // This method is called automatically when there are no results for the question.
        public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
        {
            await context.PostAsync($"Sorry, I couldn't find an answer for '{originalQueryText}'.");

            context.Wait(MessageReceived);
        }

        // This method is called automatically when there is a result for the question.
        public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        {
            if (originalQueryText == "Exit")
            {
                context.Done("");
                return;
            }
            await context.PostAsync(result.Answers.First().Answer);

            context.Wait(MessageReceived);
        }

        [QnAMakerResponseHandler(0.5)]  // 1: 100%, 0.5: 50%
        // This method is called when there is a low-order result,
        public async Task LowScoreHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        {
            var messageActivity = ProcessResultAndCreateMessageActivity(context, ref result);

            messageActivity.Text = $"I found an answer that might help..." +
                                   $"{result.Answers.First().Answer}.";

            await context.PostAsync(messageActivity);

            context.Wait(MessageReceived);
        }
    }

    //public class FAQDialog : IDialog<string>
    //{
    //    public async Task StartAsync(IDialogContext context)
    //    {
    //        await context.PostAsync("FAQ Service: ");
    //        context.Wait(MessageReceivedAsync);

    //        //return Task.CompletedTask;
    //    }

    //    public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
    //    {
    //        Activity activity = await result as Activity;

    //        if(activity.Text.Trim() == "Exit")
    //        {
    //            context.Done("Order Completed");
    //        }
    //        else
    //        {
    //            await context.PostAsync("FAQ Dialog."); // return our reply to the user
    //            context.Wait(MessageReceivedAsync);
    //        }
    //    }
    //}
}