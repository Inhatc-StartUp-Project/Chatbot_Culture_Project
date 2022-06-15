using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector;

namespace Culture_ChatBot.Helpers
{
    public static class CardHelper
    {

        static string apiKey = "API Key";
        static string serch_marker = "size:mid%7Ccolor:red%7C";

        // Create Hero card & return
        public static Attachment GetHeroCard(string eventNm, string eventCo,
                                             string eventStartDate, string eventEndDate,
                                             string opar, string latitude, string longitude)
        {
            string lat = latitude; // 목적지
            string lng = longitude; // 목적지

            // 목적지 위치
            string destinationURL = "https://maps.googleapis.com/maps/api/staticmap?center=" +
                                    lat + "," + lng +
                                    "&zoom=16&size=400x400&" +
                                    "&markers=" + serch_marker + lat + "," + lng +
                                    "&key=" + apiKey;


            var images = new List<CardImage>
            {
                new CardImage(url:destinationURL)
            };

            List<CardAction> buttons = new List<CardAction>();
            buttons.Add(new CardAction() {
                Title = "상세보기",
                Value = "content " + eventNm,
                Type = ActionTypes.ImBack
            });

            HeroCard card = new HeroCard()
            {
                Images = images,
                Title = eventNm,
                Subtitle = "행사 정보: " + eventCo,
                Text = "시작 날짜: " + eventStartDate + "\r\r" +
                       "종료 날짜: " + eventEndDate + "\r\r" +
                       "행사 장소: " + opar,
                Buttons = buttons
            };

            return card.ToAttachment();
        }


        public static Attachment GetHeroCard(string eventNm, string opar, string eventCo,
                                                string eventStartDate, string eventEndDate,
                                                string eventStartTime, string eventEndTime,
                                                string chrgeInfo, string phoneNumber,
                                                string seatNumber, string admfee, string entncAge,
                                                string atpn, string advantkInfo,
                                                string latitude, string longitude)
        {
            string lat = latitude; // 목적지
            string lng = longitude; // 목적지

            // 목적지 위치
            string destinationURL = "https://maps.googleapis.com/maps/api/staticmap?center=" +
                                    lat + "," + lng +
                                    "&zoom=16&size=400x400&" +
                                    "&markers=" + serch_marker + lat + "," + lng +
                                    "&key=" + apiKey;


            var images = new List<CardImage>
            {
                new CardImage(url:destinationURL)
            };

            List<CardAction> buttons = new List<CardAction>();
            buttons.Add(new CardAction()
            {
                Title = "즐겨찾기",
                Value = "bookmark " + eventNm,
                Type = ActionTypes.ImBack
            });
            buttons.Add(new CardAction()
            {
                Title = "돌아가기",
                Value = "exit",
                Type = ActionTypes.ImBack
            });

            HeroCard card = new HeroCard()
            {
                Images = images,
                Title = eventNm,
                Subtitle = "행사 정보: " + eventCo,
                Text = "시작 날짜: " + eventStartDate + "\r\r" +
                       "종료 날짜: " + eventEndDate + "\r\r" +
                       "시작 시간: " + eventStartTime + "\r\r" +
                       "종료 시간: " + eventEndTime + "\r\r" +
                       "행사 장소: " + opar + "\r\r" +
                       "요금: " + chrgeInfo + "\r\r" +
                       "전화번호: " + phoneNumber + "\r\r" +
                       "좌석 수: " + seatNumber + "\r\r" +
                       "가격: " + admfee + "\r\r" +
                       "연령: " + entncAge + "\r\r" +
                       "유의사항: " + atpn + "\r\r" +
                       "홈페이지 주소: " + advantkInfo + "\r\r",
                Buttons = buttons
            };

            return card.ToAttachment();
        }

        // Create Thumbnail card & return
        public static Attachment GetThumbnailCard(string eventNm, string eventCo,
                                             string eventStartDate, string eventEndDate,
                                             string opar, string latitude, string longitude)
        {
            string lat = latitude; // 목적지
            string lng = longitude; // 목적지

            // 목적지 위치
            string destinationURL = "https://maps.googleapis.com/maps/api/staticmap?center=" +
                                    lat + "," + lng +
                                    "&zoom=16&size=400x400&" +
                                    "&markers=" + serch_marker + lat + "," + lng +
                                    "&key=" + apiKey;


            var images = new List<CardImage>
            {
                new CardImage(url:destinationURL)
            };

            List<CardAction> buttons = new List<CardAction>();
            buttons.Add(new CardAction()
            {
                Title = "상세보기",
                Value = "content " + eventNm,
                Type = ActionTypes.ImBack
            });

            HeroCard card = new HeroCard()
            {
                Images = images,
                Title = eventNm,
                Subtitle = "행사 정보: " + eventCo,
                Text = "시작 날짜: " + eventStartDate + "\r\r" +
                       "종료 날짜: " + eventEndDate + "\r\r" +
                       "행사 장소: " + opar,
                Buttons = buttons
            };

            return card.ToAttachment();
        }

        // Create Receipt card & return
        public static Attachment GetReceiptCard(string strTitle, List<ReceiptItem> lstItems, string strTotal, string strTax, string strVat)
        {
            ReceiptCard card = new ReceiptCard
            {
                Title = strTitle,
                Items = lstItems,
                Total = strTotal,
                Tax = strTax,
                Vat = strVat,
            };
            return card.ToAttachment();
        }






        //// Create Hero card & return
        //public static Attachment GetHeroCard(string strTitle, string strSubTitle, string strImage,
        //                                     string strButtonText, string strButtonValue)
        //{
        //    // Create image object
        //    List<CardImage> images = new List<CardImage>();
        //    images.Add(new CardImage() { Url = strImage });

        //    // Create Button
        //    List<CardAction> buttons = new List<CardAction>();
        //    buttons.Add(new CardAction()
        //    {
        //        Title = strButtonText,
        //        Value = strButtonValue,
        //        Type = ActionTypes.ImBack
        //    });

        //    HeroCard card = new HeroCard()
        //    {
        //        Title = strTitle,
        //        Subtitle = strSubTitle,
        //        Images = images,
        //        Buttons = buttons
        //    };

        //    return card.ToAttachment();
        //}

        //// Create Thumbnail card & return
        //public static Attachment GetThumbnailCard(string strTitle, string strSubTitle, string strImage,
        //                                          string strButtonText, string strButtonValue)
        //{
        //    // Create image object
        //    List<CardImage> images = new List<CardImage>();
        //    images.Add(new CardImage() { Url = strImage });

        //    // Create Button
        //    List<CardAction> buttons = new List<CardAction>();
        //    buttons.Add(new CardAction()
        //    {
        //        Title = strButtonText,
        //        Value = strButtonValue,
        //        Type = ActionTypes.ImBack
        //    });

        //    HeroCard card = new HeroCard()
        //    {
        //        Title = strTitle,
        //        Subtitle = strSubTitle,
        //        Images = images,
        //        Buttons = buttons
        //    };

        //    return card.ToAttachment();
        //}

        //// Create Receipt card & return
        //public static Attachment GetReceiptCard(string strTitle, List<ReceiptItem> lstItems, string strTotal, string strTax, string strVat)
        //{
        //    ReceiptCard card = new ReceiptCard
        //    {
        //        Title = strTitle,
        //        Items = lstItems,
        //        Total = strTotal,
        //        Tax = strTax,
        //        Vat = strVat,
        //    };
        //    return card.ToAttachment();
        //}
    }
}