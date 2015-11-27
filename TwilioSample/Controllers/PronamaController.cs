using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace TwilioSample.Controllers
{
    public class TwilioSampleController : TwilioController
    {
        // GET: Pronama
        public ActionResult Index()
        {
            var response = new TwilioResponse();
            response.Say("こんにちは", new { voice = "alice", language = "ja-JP" });
            response.Say("さようなら", new { voice = "alice", language = "ja-JP" });
            return TwiML(response);
        }

        public ActionResult First(string From, string To, string CallSid)
        {
            
            var response = new TwilioResponse();

            var message = @"プログラミング生放送福岡にご参加いただきありがとうございます。５秒間のメッセージを録音します。";
            response.Say(message, new { voice = "alice", language = "ja-JP" });

            message = @"メッセージを録音する場合は1を、終了する場合はそれ以外のキーを入力してください。";
            response.Say(message, new { voice = "alice", language = "ja-JP" });

            var builder = new UriBuilder(Request.Url);
            var urlHelper = new UrlHelper(Request.RequestContext);
            builder.Path = urlHelper.Action("RouteStep");
            var actionUrl = builder.ToString();

            response.BeginGather(new { numDigits = 1, action = actionUrl, method = "POST", finishOnKey = "#" })
                .Say("キーを入力してください。", new { voice = "alice", language = "ja-JP" })
                .Play("https://hogehoge.blob.core.windows.net/sound/snowhalation.mp3")
                .Pause(10)
                .EndGather();

            return TwiML(response);
        }

        public ActionResult RouteStep(string From, string To, string CallSid, string Digits)
        {
            var response = new TwilioResponse();

            switch (Digits)
            {
                case "1":
                    response.Say("5秒間の録音を行います。", new { voice = "alice", language = "ja-JP" });
                    response.Record(new { maxLength = 5, action = "http://hogehoge.eu-gb.mybluemix.net/pronama" });
                    break;

                case "10":
                    response.Say("Digits is ten.");
                    break;
                default:
                    break;
            }
            return TwiML(response);
        }

        public ActionResult CallBound(string From, string To, string CallSid, string AnsweredBy)
        {
            var response = new TwilioResponse();

            var msg = "この電話は、Twilioから";
            if (!string.IsNullOrEmpty(To))
            {
                msg = (To == "+819012345678") ? msg += "あなたの携帯電話" : msg += "電話番号が" + From;
            }
            else
            {
                msg += "発信先は不明な番号";
            }
            msg += "へのお電話です。";
            response.Say(msg, new { voice = "alice", language = "ja-JP" });

            response.Pause(2);

            if (!string.IsNullOrEmpty(AnsweredBy))
            {
                response.Say(String.Format("このAnsweredByパラメータの値は、{0}です。", AnsweredBy), new { voice = "alice", language = "ja-JP" });
                response.Pause(2);
                if (AnsweredBy == "machine")
                {
                    response.Say("この電話は機械によって着信しました。", new { voice = "alice", language = "ja-JP" });
                }
                if (AnsweredBy == "human")
                {
                    response.Say("この電話は人間によって着信しました。", new { voice = "alice", language = "ja-JP" });
                }
                response.Pause(2);
            }
            else
            {
                response.Say("機械の判定パラメータは存在しませんでした。", new { voice = "alice", language = "ja-JP" });
            }

            response.Say("お電話ありがとうございました。", new { voice = "alice", language = "ja-JP" });
            return TwiML(response);
        }
    }
}