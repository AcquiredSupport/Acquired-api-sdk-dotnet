using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Acquiredapisdkdotnet.Lib.AQPay;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Collections;
using System.Web;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Acquiredapisdkdotnet.Controllers
{
    public class ExamplesController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Auth_()
        {
            return View();
        }

        public IActionResult Capture()
        {
            return View();
        }

        public IActionResult Void()
        {
            return View();
        }

        public IActionResult Refund()
        {
            return View();
        }

        public IActionResult Rebill()
        {
            return View();
        }

        public IActionResult Credit()
        {
            return View();
        }

        public IActionResult Update_billing()
        {
            return View();
        }

        [HttpPost]
        public string Auth_submit(){

            /*
             *  "timestamp" has been set on AQPay.cs
             *  "request_hash" has been set on AQPay.cs
             *  "company_id" has been set on AQPayConfig.cs
             *  "company_pass" has been set on AQPayConfig.cs 
             *  
             *  How to use
             *  step 1: Check customer post data.  (need you to do)
             *  step 2: Set parameters by use SetParam().
             *  step 3: Post parameters like AQPay.Capture().
             *  step 4: Check response hash by use AQPay.IsSignatureValid(result).
             *  step 5: Do your business according to the result. (need you to do)
             *
             *  Use 3-D secure
             *  1. setParam['action'] = "ENQUIRE".
             *  2. post to Acquired with AUTH_ONLY or AUTH_CAPTURE request.
             *  3. set pareq,ACS_url,termurl,md after Acquired response.
             *  4. post to ACS by use AQPay.postToACS().
             *  5. The termurl will receive the ACS post.Please see acs_notify.php.
             *  6. post SETTLEMENT to Acquired on acs_notify.php.
             *
             */

            int amount = int.Parse(Request.Form["amount"]);
            string merchant_order_id = DateTime.Now.ToString("yyyyMMddHmmss") + new Random().Next(1000,9999);
            string transaction_type = Request.Form["transaction_type"];
            switch(transaction_type){
                case "2": transaction_type = "AUTH_CAPTURE";break;
                default: transaction_type = "AUTH_ONLY";break;
            }
            string subscription_type = Request.Form["subscription"];
            switch (subscription_type)
            {
                case "1": subscription_type = "INIT"; break;
                default: subscription_type = ""; break;
            }
            string tds_action = Request.Form["tds"];
            switch(tds_action){
                case "1": tds_action = "ENQUIRE"; break;
                default: tds_action = ""; break;
            }

            string currency = Request.Form["currency"];
            string title = Request.Form["title"];
            string fname = Request.Form["fname"];
            string mname = Request.Form["mname"];
            string lname = Request.Form["lname"];
            string gender = Request.Form["gender"];
            string dob = Request.Form["dob"];
            string customer_ipaddress = "127.0.0.1";
            string company = Request.Form["company"];
            string name = Request.Form["name"];
            string number = Request.Form["number"];
            string type = Request.Form["type"];
            string cvv = Request.Form["cvv"];
            string cardexp = Request.Form["cardexp"];
            string address = Request.Form["address"];
            string address2 = Request.Form["address2"];
            string city = Request.Form["city"];
            string state = Request.Form["state"];
            string zipcode = Request.Form["zipcode"];
            string iso2 = Request.Form["iso2"];
            string phone = Request.Form["phone"];
            string email = Request.Form["email"];

            /*====== step 2: Set parameters ======*/
            AQPay aqpay = new AQPay();
            aqpay.SetParam("mid_id", "1014");
            aqpay.SetParam("mid_pass", "test");
            aqpay.SetParam("vt", "");
            aqpay.SetParam("useragent", "");
            //set transaction data
            aqpay.SetParam("merchant_order_id", merchant_order_id);
            aqpay.SetParam("transaction_type", transaction_type);
            aqpay.SetParam("subscription_type", subscription_type);
            aqpay.SetParam("amount", amount.ToString());
            aqpay.SetParam("currency_code_iso3", currency);
            aqpay.SetParam("merchant_customer_id", "C101");
            aqpay.SetParam("merchant_custom_1", "C1");
            aqpay.SetParam("merchant_custom_2", "C2");
            aqpay.SetParam("merchant_custom_3", "C3");
            //set customer data
            aqpay.SetParam("customer_title", title);
            aqpay.SetParam("customer_fname", fname);
            aqpay.SetParam("customer_mname", mname);
            aqpay.SetParam("customer_lname", lname);
            aqpay.SetParam("customer_gender", gender);
            aqpay.SetParam("customer_dob", dob);
            aqpay.SetParam("customer_ipaddress", customer_ipaddress);
            aqpay.SetParam("customer_company", company);
            //set billing data
            aqpay.SetParam("cardholder_name", name);
            aqpay.SetParam("cardnumber", number);
            aqpay.SetParam("card_type", type);
            aqpay.SetParam("cardcvv", cvv);
            aqpay.SetParam("cardexp", cardexp);
            aqpay.SetParam("billing_street", address);
            aqpay.SetParam("billing_street2", address2);
            aqpay.SetParam("billing_city", city);
            aqpay.SetParam("billing_state", state);
            aqpay.SetParam("billing_zipcode", zipcode);
            aqpay.SetParam("billing_country_code_iso2", iso2);
            aqpay.SetParam("billing_phone", phone);
            aqpay.SetParam("billing_email", email);
            //set billing data
            aqpay.SetParam("action", tds_action);

            JObject result = aqpay.Auth_();

            Console.WriteLine("timestamp: " + result["timestamp"]);
            Console.WriteLine("response_code: " + result["response_code"]);
            Console.WriteLine("response_message: " + result["response_message"]);
            Console.WriteLine("transaction_id: " + result["transaction_id"]);

            string resultstr = "";
            /*====== step 4: Check response hash ======*/
            if (aqpay.IsSignatureValid(result))
            {

                Console.WriteLine("SUCCESS: Request sucess");
                resultstr = "SUCCESS: Request sucess";

                //do your job

                /*====== deal 3-D secure ======*/
                if (tds_action.Equals("ENQUIRE") && result["tds"] != null)
                {
                    JObject tdsobj = JsonConvert.DeserializeObject<JObject>(result["tds"].ToString());
                    string[] response_code_array = { "501", "502" };
                    if (Array.IndexOf(response_code_array, result["response_code"].ToString()) != -1)
                    {

                        aqpay.SetParam("pareq", tdsobj["pareq"].ToString());
                        aqpay.SetParam("ACS_url", tdsobj["url"].ToString());
                        string basePath = "http://" + Request.Host + "/examples/";
                        string termurl = basePath + "Acs_notify";
                        aqpay.SetParam("termurl", termurl);

                        /*
                         *  Set MD field 
                         *  This field will required for the subsequent SETTLEMENT request.   
                         *  if you store these param in your sqlserver then you can just set a id
                            And you can read these param from you sqlserver when post SETTLEMENT request.
                         */
                        JObject md = new JObject
                        {
                            { "mid_id", "1014" },
                            { "mid_pass", "test" },
                            { "transaction_id", result["transaction_id"].ToString() },
                            { "merchant_order_id", result["merchant_order_id"].ToString() },
                            { "amount", result["amount"].ToString() },
                            { "currency_code_iso3", result["currency_code_iso3"].ToString() },
                            { "transaction_type", result["transaction_type"].ToString() }
                        };

                        //"md" must be encrypted and Base64 encoded.
                        string mdstr = Convert.ToBase64String(Encoding.UTF8.GetBytes(md.ToString()));
                        aqpay.SetParam("md", mdstr);

                        string postResult = aqpay.PostToACS();
                        Console.WriteLine("Post ACS Result: " + postResult);

                    }
                }
            }else{

                Console.WriteLine("ERROR: Invalid response");
                resultstr = "ERROR: Invalid response. See Console print";

            }

            return resultstr;
                                           
        }

        [HttpPost]
        public string Capture_submit(){

            string original_transaction_id = Request.Form["transaction_id"];
            string amount = Request.Form["amount"];

            AQPay aqpay = new AQPay();
            aqpay.SetParam("mid_id", "1014");
            aqpay.SetParam("mid_pass", "test");
            aqpay.SetParam("original_transaction_id", original_transaction_id);
            aqpay.SetParam("amount", amount);
            JObject result = aqpay.Capture();

            Console.WriteLine("timestamp: " + result["timestamp"]);
            Console.WriteLine("response_code: " + result["response_code"]);
            Console.WriteLine("response_message: " + result["response_message"]);
            Console.WriteLine("transaction_id: " + result["transaction_id"]);

            string resultstr = "";
            if (aqpay.IsSignatureValid(result))
            {

                Console.WriteLine("SUCCESS: Request sucess");
                resultstr = "SUCCESS: Request sucess";
                //do your job

            }
            else
            {
                Console.WriteLine("ERROR: Invalid response");
                resultstr = "ERROR: Invalid response. See Console print";
            }

            return resultstr;

        }

        [HttpPost]
        public string Void_submit()
        {

            string original_transaction_id = Request.Form["transaction_id"];

            AQPay aqpay = new AQPay();
            aqpay.SetParam("mid_id", "1014");
            aqpay.SetParam("mid_pass", "test");
            aqpay.SetParam("original_transaction_id", original_transaction_id);
            JObject result = aqpay.Void_deal();

            Console.WriteLine("timestamp: " + result["timestamp"]);
            Console.WriteLine("response_code: " + result["response_code"]);
            Console.WriteLine("response_message: " + result["response_message"]);
            Console.WriteLine("transaction_id: " + result["transaction_id"]);

            string resultstr = "";
            if (aqpay.IsSignatureValid(result))
            {

                Console.WriteLine("SUCCESS: Request sucess");
                resultstr = "SUCCESS: Request sucess";
                //do your job

            }
            else
            {
                Console.WriteLine("ERROR: Invalid response");
                resultstr = "ERROR: Invalid response. See Console print";
            }

            return resultstr;

        }

        [HttpPost]
        public string Refund_submit()
        {

            string original_transaction_id = Request.Form["transaction_id"];
            string amount = Request.Form["amount"];

            AQPay aqpay = new AQPay();
            aqpay.SetParam("mid_id", "1014");
            aqpay.SetParam("mid_pass", "test");
            aqpay.SetParam("original_transaction_id", original_transaction_id);
            aqpay.SetParam("amount", amount);
            JObject result = aqpay.Refund();

            Console.WriteLine("timestamp: " + result["timestamp"]);
            Console.WriteLine("response_code: " + result["response_code"]);
            Console.WriteLine("response_message: " + result["response_message"]);
            Console.WriteLine("transaction_id: " + result["transaction_id"]);

            string resultstr = "";
            if (aqpay.IsSignatureValid(result))
            {

                Console.WriteLine("SUCCESS: Request sucess");
                resultstr = "SUCCESS: Request sucess";
                //do your job

            }
            else
            {
                Console.WriteLine("ERROR: Invalid response");
                resultstr = "ERROR: Invalid response. See Console print";
            }

            return resultstr;

        }


        [HttpPost]
        public string Rebill_submit()
        {
            
            string merchant_order_id = DateTime.Now.ToString("yyyyMMddHmmss") + new Random().Next(1000, 9999);
            string original_transaction_id = Request.Form["original_transaction_id"];
            string amount = Request.Form["amount"];
            string currency_code_iso3 = Request.Form["currency"];
            string transaction_type = Request.Form["transaction_type"];
            switch (transaction_type)
            {
                case "2": transaction_type = "AUTH_CAPTURE"; break;
                default: transaction_type = "AUTH_ONLY"; break;
            }

            AQPay aqpay = new AQPay();
            aqpay.SetParam("mid_id", "1014");
            aqpay.SetParam("mid_pass", "test");
            aqpay.SetParam("transaction_type", transaction_type);
            aqpay.SetParam("merchant_order_id", merchant_order_id);
            aqpay.SetParam("amount", amount);
            aqpay.SetParam("currency_code_iso3", currency_code_iso3);
            aqpay.SetParam("original_transaction_id", original_transaction_id);

            JObject result = aqpay.Rebill();

            Console.WriteLine("timestamp: " + result["timestamp"]);
            Console.WriteLine("response_code: " + result["response_code"]);
            Console.WriteLine("response_message: " + result["response_message"]);
            Console.WriteLine("transaction_id: " + result["transaction_id"]);

            string resultstr = "";
            if (aqpay.IsSignatureValid(result))
            {

                Console.WriteLine("SUCCESS: Request sucess");
                resultstr = "SUCCESS: Request sucess";
                //do your job

            }
            else
            {
                Console.WriteLine("ERROR: Invalid response");
                resultstr = "ERROR: Invalid response. See Console print";
            }

            return resultstr;

        }

        [HttpPost]
        public string Update_billing_submit()
        {

            string original_transaction_id = Request.Form["transaction_id"];
            string title = Request.Form["title"];
            string fname = Request.Form["fname"];
            string mname = Request.Form["mname"];
            string lname = Request.Form["lname"];
            string gender = Request.Form["gender"];
            string dob = Request.Form["dob"];
            string customer_ipaddress = "127.0.0.1";
            string company = Request.Form["company"];
            string name = Request.Form["name"];
            string number = Request.Form["number"];
            string type = Request.Form["type"];
            string cvv = Request.Form["cvv"];
            string cardexp = Request.Form["cardexp"];
            string address = Request.Form["address"];
            string address2 = Request.Form["address2"];
            string city = Request.Form["city"];
            string state = Request.Form["state"];
            string zipcode = Request.Form["zipcode"];
            string iso2 = Request.Form["iso2"];
            string phone = Request.Form["phone"];
            string email = Request.Form["email"];

            AQPay aqpay = new AQPay();
            aqpay.SetParam("mid_id", "1014");
            aqpay.SetParam("mid_pass", "test");         
            //set transaction data          
            aqpay.SetParam("original_transaction_id", original_transaction_id);
            //set customer data
            aqpay.SetParam("customer_title", title);
            aqpay.SetParam("customer_fname", fname);
            aqpay.SetParam("customer_mname", mname);
            aqpay.SetParam("customer_lname", lname);
            aqpay.SetParam("customer_gender", gender);
            aqpay.SetParam("customer_dob", dob);
            aqpay.SetParam("customer_ipaddress", customer_ipaddress);
            aqpay.SetParam("customer_company", company);
            //set billing data
            aqpay.SetParam("cardholder_name", name);
            aqpay.SetParam("cardnumber", number);
            aqpay.SetParam("card_type", type);
            aqpay.SetParam("cardcvv", cvv);
            aqpay.SetParam("cardexp", cardexp);
            aqpay.SetParam("billing_street", address);
            aqpay.SetParam("billing_street2", address2);
            aqpay.SetParam("billing_city", city);
            aqpay.SetParam("billing_state", state);
            aqpay.SetParam("billing_zipcode", zipcode);
            aqpay.SetParam("billing_country_code_iso2", iso2);
            aqpay.SetParam("billing_phone", phone);
            aqpay.SetParam("billing_email", email);

            JObject result = aqpay.Update_billing();

            Console.WriteLine("timestamp: " + result["timestamp"]);
            Console.WriteLine("response_code: " + result["response_code"]);
            Console.WriteLine("response_message: " + result["response_message"]);
            Console.WriteLine("transaction_id: " + result["transaction_id"]);

            string resultstr = "";
            if (aqpay.IsSignatureValid(result))
            {

                Console.WriteLine("SUCCESS: Request sucess");
                resultstr = "SUCCESS: Request sucess";
                //do your job

            }
            else
            {
                Console.WriteLine("ERROR: Invalid response");
                resultstr = "ERROR: Invalid response. See Console print";
            }

            return resultstr;

        }

        [HttpPost]
        public string Credit_submit()
        {

            string amount = Request.Form["amount"];
            string merchant_order_id = DateTime.Now.ToString("yyyyMMddHmmss") + new Random().Next(1000,9999);
            string currency = Request.Form["currency"];
            string title = Request.Form["title"];
            string fname = Request.Form["fname"];
            string mname = Request.Form["mname"];
            string lname = Request.Form["lname"];
            string gender = Request.Form["gender"];
            string dob = Request.Form["dob"];
            string customer_ipaddress = "127.0.0.1";
            string company = Request.Form["company"];
            string name = Request.Form["name"];
            string number = Request.Form["number"];
            string type = Request.Form["type"];
            string cvv = Request.Form["cvv"];
            string cardexp = Request.Form["cardexp"];
            string address = Request.Form["address"];
            string address2 = Request.Form["address2"];
            string city = Request.Form["city"];
            string state = Request.Form["state"];
            string zipcode = Request.Form["zipcode"];
            string iso2 = Request.Form["iso2"];
            string phone = Request.Form["phone"];
            string email = Request.Form["email"];

            AQPay aqpay = new AQPay();
            aqpay.SetParam("mid_id", "1014");
            aqpay.SetParam("mid_pass", "test");         
            //set transaction data
            aqpay.SetParam("merchant_order_id", merchant_order_id);
            aqpay.SetParam("amount", amount);
            aqpay.SetParam("currency_code_iso3", currency);
            aqpay.SetParam("merchant_customer_id", "C101");
            aqpay.SetParam("merchant_custom_1", "C1");
            aqpay.SetParam("merchant_custom_2", "C2");
            aqpay.SetParam("merchant_custom_3", "C3");
            //set customer data
            aqpay.SetParam("customer_title", title);
            aqpay.SetParam("customer_fname", fname);
            aqpay.SetParam("customer_mname", mname);
            aqpay.SetParam("customer_lname", lname);
            aqpay.SetParam("customer_gender", gender);
            aqpay.SetParam("customer_dob", dob);
            aqpay.SetParam("customer_ipaddress", customer_ipaddress);
            aqpay.SetParam("customer_company", company);
            //set billing data
            aqpay.SetParam("cardholder_name", name);
            aqpay.SetParam("cardnumber", number);
            aqpay.SetParam("card_type", type);
            aqpay.SetParam("cardcvv", cvv);
            aqpay.SetParam("cardexp", cardexp);
            aqpay.SetParam("billing_street", address);
            aqpay.SetParam("billing_street2", address2);
            aqpay.SetParam("billing_city", city);
            aqpay.SetParam("billing_state", state);
            aqpay.SetParam("billing_zipcode", zipcode);
            aqpay.SetParam("billing_country_code_iso2", iso2);
            aqpay.SetParam("billing_phone", phone);
            aqpay.SetParam("billing_email", email);

            JObject result = aqpay.Credit();

            Console.WriteLine("timestamp: " + result["timestamp"]);
            Console.WriteLine("response_code: " + result["response_code"]);
            Console.WriteLine("response_message: " + result["response_message"]);
            Console.WriteLine("transaction_id: " + result["transaction_id"]);

            string resultstr = "";
            if (aqpay.IsSignatureValid(result))
            {

                Console.WriteLine("SUCCESS: Request sucess");
                resultstr = "SUCCESS: Request sucess";
                //do your job

            }
            else
            {
                Console.WriteLine("ERROR: Invalid response");
                resultstr = "ERROR: Invalid response. See Console print";
            }

            return resultstr;

        }


        public void Test_ACS(){

            string basePath = "http://" + Request.Host + "/examples/";
            string resultstr = "{\"mid_id\":\"1014\",\"mid_pass\":\"test\",\"transaction_id\":\"101750\","
                    + "\"merchant_order_id\":\"20171128091112\","
                    + "\"amount\":\"1\","
                    + "\"currency_code_iso3\":\"CBA\","
                    + "\"transaction_type\":\"AUTH_ONLY\","
                    + "\"response_code\":\"501\","
                    + "\"tds\":{\"pareq\":\"testpareq\",\"url\":\"" + basePath + "Acs_servers\"}}";
            JObject result = JsonConvert.DeserializeObject<JObject>(resultstr);
            string tds_action = "ENQUIRE";

            if (tds_action.Equals("ENQUIRE") && result["tds"] != null)
            {

                JObject tdsobj = JsonConvert.DeserializeObject<JObject>(result["tds"].ToString());

                string[] response_code_array = { "501", "502" };
                if (Array.IndexOf(response_code_array, result["response_code"].ToString()) != -1)
                {
                    AQPay aqpay = new AQPay();
                    aqpay.SetParam("pareq", tdsobj["pareq"].ToString());//should be Acquired response
                    aqpay.SetParam("ACS_url", tdsobj["url"].ToString());//should be Acquired response
                    String termurl = HttpUtility.UrlEncode(basePath + "Acs_notify");
                    aqpay.SetParam("termurl", termurl);

                    JObject md = new JObject
                    {
                        { "mid_id", "1014" },
                        { "mid_pass", "test" },
                        { "transaction_id", result["transaction_id"] },
                        { "merchant_order_id", result["merchant_order_id"] },
                        { "amount", result["amount"] },
                        { "currency_code_iso3", result["currency_code_iso3"] },
                        { "transaction_type", result["transaction_type"] }
                    };
                    string mdstr = Convert.ToBase64String(Encoding.UTF8.GetBytes(md.ToString()));
                    aqpay.SetParam("md", mdstr);

                    String postResult = aqpay.PostToACS();
                    Console.WriteLine("Post ACS Result: " + postResult);

                }

            }

        }

        //simulate ACS servers
        [HttpPost]
        public void Acs_servers(){

            string pareq = Request.Form["pareq"];
            string md = Request.Form["md"];
            string url = HttpUtility.UrlDecode(Request.Form["termurl"]);
            string pares = "pares";

            if (!string.IsNullOrEmpty(pareq))
            {

                string post_data = "PaRes=" + pares + "&md=" + md;
                AQPayCommon util = new AQPayCommon();
                String result = util.Http_request(url, post_data, 30);
                Console.WriteLine("Acs_servers result: " + result);

            }

        }

        //receive ACS request
        [HttpPost]
        public void Acs_notify()
        {

            string pares = Request.Form["PaRes"];
            string md = Request.Form["md"];
            Console.WriteLine("Received the ACS servers response");        

            if (!string.IsNullOrEmpty(pares))
            {

                AQPay auth = new AQPay();
                auth.SetParam("pares", pares);
                //"md" need decrypt
                string mdstr = Encoding.UTF8.GetString(Convert.FromBase64String(md));
                JObject mdjson = JsonConvert.DeserializeObject<JObject>(mdstr);

                auth.SetParam("mid_id", mdjson["mid_id"].ToString());
                auth.SetParam("mid_pass", mdjson["mid_pass"].ToString());
                auth.SetParam("original_transaction_id", mdjson["transaction_id"].ToString());
                auth.SetParam("merchant_order_id", mdjson["merchant_order_id"].ToString());
                auth.SetParam("amount", mdjson["amount"].ToString());
                auth.SetParam("currency_code_iso3", mdjson["currency_code_iso3"].ToString());
                auth.SetParam("transaction_type", mdjson["transaction_type"].ToString());
                JObject result = auth.PostSettleACS();

                Console.WriteLine("timestamp: " + result["timestamp"]);
                Console.WriteLine("response_code: " + result["response_code"]);
                Console.WriteLine("response_message: " + result["response_message"]);
                Console.WriteLine("transaction_id: " + result["transaction_id"]);

                if (auth.IsSignatureValid(result))
                {

                    Console.WriteLine("SUCCESS: Request sucess [postSettleACS]");
                    //do your job

                }
                else
                {
                    Console.WriteLine("ERROR: Invalid response [postSettleACS]");
                }


            }

        }
       
    }
}
