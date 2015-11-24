using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace TwilioSample.Controllers
{
    public class PronamaController : TwilioController
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
            //if (!string.IsNullOrEmpty(From))
            //{
            //    var msg = "From is ";
            //    msg = (From == "+819012345678") ? msg += "TwilioUser" : msg += From;

            //    response.Say(msg);
            //}
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
                .Play("https://pronamademo.blob.core.windows.net/sound/snowhalation.mp3")
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
                    response.Record(new { maxLength = 5, action = "http://hama-nodered.eu-gb.mybluemix.net/pronama" });
                    break;

                case "10":
                    response.Say("Digits is ten.");
                    break;
                default:
                    break;
            }
            return TwiML(response);
        }
    }
}