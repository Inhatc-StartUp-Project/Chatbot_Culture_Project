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

namespace Culture_ChatBot
{
    // [LuisModel(App ID, Subscription Key)]
    [LuisModel("63d8f2a4-dd38-4f82-acca-9dc675e2fb2a", "14e3e793f33b476ba28216bc2a1532d9", domain: "australiaeast.api.cognitive.microsoft.com")]

    [Serializable]
    public class LUISDialog : LuisDialog<string>
    {

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"죄송합니다. 말씀을 이해하지 못했습니다.";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Order")]
        public async Task Order(IDialogContext context, IAwaitable<IMessageActivity> activtity, LuisResult result)
        {
            var message = await activtity;

            EntityRecommendation menuEntityRecommendation;
            EntityRecommendation sizeEntityRecomendatrion;
            EntityRecommendation quantityEntityRecomendatrion;

            string Menu = "";
            string Size = "보통";
            string Quantity = "한그릇";

            if (result.TryFindEntity("Menu", out menuEntityRecommendation))
            {
                Menu = menuEntityRecommendation.Entity.Replace(" ", "");
            }
            else
            {
                await context.PostAsync("없는 메뉴를 선택했습니다.");
                context.Wait(this.MessageReceived);
                return;
            }


            if (result.TryFindEntity("Size", out sizeEntityRecomendatrion))
            {
                Size = sizeEntityRecomendatrion.Entity.Replace(" ", "");
            }


            if (result.TryFindEntity("Quantity", out quantityEntityRecomendatrion))
            {
                Quantity = quantityEntityRecomendatrion.Entity.Replace(" ", "");
            }


            await context.PostAsync($"{Menu}, {Size}, {Quantity}를 주문하셨습니다");

            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Delivery")]
        public async Task Delivery(IDialogContext context, IAwaitable<IMessageActivity> activtity, LuisResult result)
        {
            await context.PostAsync("출발 했습니다. 잠시만 기다려 주세요.");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Finish")]
        public async Task Finish(IDialogContext context, IAwaitable<IMessageActivity> activtity, LuisResult result)
        {
            await context.PostAsync("주문 완료 되었습니다. 감사합니다.");
            context.Done("주문완료");
        }
    }
}