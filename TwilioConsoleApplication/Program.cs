using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;

namespace TwilioConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("通話を開始します。何かキーを入力してください。");
            System.Console.ReadKey();

            var accountSid = "12345";
            var authToken = "abcde";
            var twilioPhonenumber = "+815098765432";

            var client = new TwilioRestClient(accountSid, authToken);

            var options = new CallOptions();
            options.Url = "http://hogehoge.net/twiliosample/callbound";
            options.To = "+819012345678";
            options.From = twilioPhonenumber;
            options.IfMachine = "Continue";

            var call = client.InitiateOutboundCall(options);

            if (call.RestException == null)
            {
                System.Console.WriteLine("通話を開始しました");
                System.Console.WriteLine(string.Format("Started call: {0}", call.Sid));
            }
            else
            {
                System.Console.WriteLine("通話は異常終了しました。");
                System.Console.WriteLine(string.Format("Error: {0}", call.RestException.Message));
            }

            System.Console.ReadKey();
        }
    }
}
