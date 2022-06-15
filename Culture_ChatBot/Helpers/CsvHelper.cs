using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Collections.Generic;
using System;
using System.Runtime.Remoting.Messaging;

namespace Culture_ChatBot.Helpers
{
    public class CsvHelper
    {
        public List<List<Dictionary<string, string>>> GetCsvData()
        {
            var reader = new StreamReader(File.OpenRead(@"C:\Users\wnsgu\Desktop\data_conv.csv"));
            List<List<Dictionary<string, string>>> contentList = new List<List<Dictionary<string, string>>>();
            int i = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                if (i == 0)
                {
                    i++;
                    continue;
                }
                List<Dictionary<string, string>> dummyList = new List<Dictionary<string, string>>();
                Dictionary<string, string> dummyDict = new Dictionary<string, string>();

                dummyDict.Add("eventNm", values[0]);        // 행사명
                dummyDict.Add("opar", values[1]);           // 장소
                dummyDict.Add("eventCo", values[2]);        // 행사 내용
                dummyDict.Add("eventStartDate", values[3]); // 행사 시작 일자
                dummyDict.Add("eventEndDate", values[4]);   // 행사 종료 일자
                dummyDict.Add("eventStartTime", values[5]); // 행사 시작 시각
                dummyDict.Add("eventEndTime", values[6]);   // 행사 종료 시각
                dummyDict.Add("chrgeInfo", values[7]);      // 요금 정보
                dummyDict.Add("phoneNumber", values[10]);   // 전화번호
                dummyDict.Add("seatNumber", values[12]);    // 객석 수
                dummyDict.Add("admfee", values[13]);        // 관람 요금
                dummyDict.Add("entncAge", values[14]);      // 입장 연령
                dummyDict.Add("atpn", values[16]);          // 유의사항
                dummyDict.Add("advantkInfo", values[18]);   // 예매 정보
                dummyDict.Add("latitude", values[22]);      // 위도
                dummyDict.Add("longitude", values[23]);     // 경도
                dummyList.Add(dummyDict);
                contentList.Add(dummyList);
            }


            // 예제 (행사 제목 검색)
            //foreach (List<Dictionary<string, string>> content in contentList)
            //{
            //    foreach (Dictionary<string, string> value in content)
            //    {
            //        string eventName = "가을콘서트";
            //        if (value["eventNm"] == eventName)
            //        {
            //            Console.WriteLine("eventNm: " + value["eventNm"]);
            //            Console.WriteLine("opar: " + value["opar"]);
            //            Console.WriteLine("eventCo: " + value["eventCo"]);
            //            Console.WriteLine("eventStartDate: " + value["eventStartDate"]);
            //            Console.WriteLine("eventEndDate: " + value["eventEndDate"]);
            //            Console.WriteLine("eventStartTime: " + value["eventStartTime"]);
            //            Console.WriteLine("eventEndTime: " + value["eventEndTime"]);
            //            Console.WriteLine("chrgeInfo: " + value["chrgeInfo"]);
            //            Console.WriteLine("phoneNumber: " + value["phoneNumber"]);
            //            Console.WriteLine("seatNumber: " + value["seatNumber"]);
            //            Console.WriteLine("admfee: " + value["admfee"]);
            //            Console.WriteLine("entncAge: " + value["entncAge"]);
            //            Console.WriteLine("atpn: " + value["atpn"]);
            //            Console.WriteLine("advantkInfo: " + value["advantkInfo"]);
            //            Console.WriteLine("latitude: " + value["latitude"]);
            //            Console.WriteLine("longitude: " + value["longitude"]);
            //        }
            //        Console.WriteLine(value["latitude"]);
            //    }
            //}
            return contentList;
        }
    }
}