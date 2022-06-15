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

namespace Culture_ChatBot.Dialog  // 즐겨찾기로 검색 다이얼로그
{
    [Serializable]
    public class BookmarkSearchDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //await context.PostAsync("[즐겨찾기] 전화번호를 입력해 주세요.");

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            Activity activity = await result as Activity;

            if (activity.Text.Trim().ToUpper() == "EXIT" || activity.Text.Trim().ToUpper() == "Q" || activity.Text.Trim().Contains("뒤로"))
            {
                string strMessage = "[돌아가기] 이전 선택지로 돌아갑니다.";
                await context.PostAsync(strMessage);

                context.Done("");
                return;
            }
            else
            {
                var message = context.MakeMessage();

                message.Attachments.Add(new HeroCard
                {
                    Title = "청소년을 위한 스쿨클래식",
                    Subtitle = "행사 정보: 부천필 연주회",
                    Text = "시작 날짜: 2022-08-18 \r\r" +
                           "종료 날짜: 2022-08-18 \r\r" +
                           "시작 시간: 19:30 \r\r" +
                           "종료 시간: 21:30 \r\r" +
                           "행사 장소: 부천시민회관 대공연장 \r\r" +
                           "홈페이지 주소: http://www.bucheonphil.or.kr/",
                }.ToAttachment());

                message.Attachments.Add(new HeroCard
                {
                    Title = "풍류 피아니스트 임동창 콘서트",
                    Subtitle = "행사 정보: 복합(클래식+국악)",
                    Text = "시작 날짜: 2022-07-14 \r\r" +
                           "종료 날짜: 2022-07-14 \r\r" +
                           "시작 시간: 19:30 \r\r" +
                           "종료 시간: 21:00 \r\r" +
                           "행사 장소: 문화의전당 2층 함월홀 \r\r" +
                           "홈페이지 주소: http://artscenter.junggu.ulsan.kr",
                }.ToAttachment());

                message.Attachments.Add(new HeroCard
                {
                    Title = "가을 콘서트",
                    Subtitle = "행사 정보: 대중가요",
                    Text = "시작 날짜: 2022-09-24 \r\r" +
                           "종료 날짜: 2022-09-24 \r\r" +
                           "시작 시간: 19:30 \r\r" +
                           "종료 시간: 21:00 \r\r" +
                           "행사 장소: 문화예술회관 대공연장\r\r" +
                           "홈페이지 주소: ",
                }.ToAttachment());

                var actions = new List<CardAction>();
                actions.Add(new CardAction() { Title = "돌아가기", Value = "exit", Type = ActionTypes.ImBack });
                message.Attachments.Add(new HeroCard { Buttons = actions }.ToAttachment());

                await context.PostAsync(message);

                Activity activity2 = await result as Activity;
            }
        }
    }
}