using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Acquiredapisdkdotnet.Lib.AQPay
{
    public class AQPay
    {

        protected AQPayCommon util = new AQPayCommon();
        protected Hashtable param = new Hashtable();
        protected int connectTimeout;

        public AQPay()
        {

            this.connectTimeout = 120;

        }

        public void SetParam(String key, String paramVal)
        {
            key = this.util.TrimString(key);
            if(key != null) {
                this.param.Add(key, paramVal);
            }
        }

        public void ClearParam()
        {
            this.param.Clear();
        }

        private void SetBasicParam()
        {
            this.param.Add("company_id", this.param["company_id"].ToString());
            this.param.Add("company_pass", this.param["company_pass"].ToString());
            this.param.Add("company_mid_id", this.param["company_mid_id"].ToString());
            this.param.Add("timestamp", this.util.Now());
            string hashcode = this.param["hash_code"].ToString();
            this.param.Add("request_hash", this.util.RequestHash(this.param, hashcode));
        }

        public string GenerateResHash(JObject data, string hashcode){            
            return this.util.ReponseHash(data, hashcode);
        }

        public Boolean IsSignatureValid(JObject response, string hashcode){
            string key = response["response_hash"].ToString();
            string response_hash = this.GenerateResHash(response, hashcode);
            if(key.Equals(response_hash)){
                return true;
            }else{
                return false;
            }
        }

        private JObject PostJson(){

            string[] transactionParam = {
                "transaction_type",
                "merchant_order_id",
                "subscription_type",
                "amount",
                "currency_code_iso3",
                "merchant_customer_id",
                "merchant_custom_1",
                "merchant_custom_2",
                "merchant_custom_3",
                "original_transaction_id",
                "amount"
            };
            string[] customerParam = {
                "customer_title",
                "customer_fname",
                "customer_mname",
                "customer_lname",
                "customer_gender",
                "customer_dob",
                "customer_ipaddress",
                "customer_company"
            };
            string[] billingParam = {
                "cardholder_name",
                "cardnumber",
                "card_type",
                "cardcvv",
                "cardexp",
                "billing_street",
                "billing_street2",
                "billing_city",
                "billing_state",
                "billing_zipcode",
                "billing_country_code_iso2",
                "billing_phone",
                "billing_email"
            };
            string[] tdsParam = {
                "action",
                "pares",
                "ipaddress"
            };


            Hashtable data = new Hashtable();
            Hashtable transactionData = new Hashtable();
            Hashtable customerData = new Hashtable();
            Hashtable billingData = new Hashtable();
            Hashtable tdsData = new Hashtable();

            foreach(DictionaryEntry de in this.param){
                if(Array.IndexOf(transactionParam, de.Key) != -1) {
                    transactionData.Add(de.Key, de.Value);
                    continue;
                }
                if (Array.IndexOf(customerParam, de.Key) != -1)
                {
                    customerData.Add(de.Key, de.Value);
                    continue;
                }
                if (Array.IndexOf(billingParam, de.Key) != -1)
                {
                    billingData.Add(de.Key, de.Value);
                    continue;
                }
                if (Array.IndexOf(tdsParam, de.Key) != -1)
                {
                    tdsData.Add(de.Key, de.Value);
                    continue;
                }
                data.Add(de.Key, de.Value);
            }

            if(transactionData.Count != 0){
                data.Add("transaction", transactionData);
            }
            if(transactionData.Count != 0){
                data.Add("customer", customerData);
            }
            if(billingData.Count != 0){
                data.Add("billing", billingData);
            }
            if(tdsData.Count != 0){
                data.Add("tds", tdsData);
            }

            string json = JsonConvert.SerializeObject(data);
            String url = this.param["request_url"].ToString();
            string response = this.util.Http_request(url, json, this.connectTimeout, "JSON");
            Console.WriteLine(response);

            JObject result = JsonConvert.DeserializeObject<JObject>(response);

            this.ClearParam();
            return result;

        }

        public JObject Auth_(){

            this.SetBasicParam();
            return this.PostJson();

        }

        public JObject Rebill(){

            this.SetParam("subscription_type", "REBILL");
            this.SetBasicParam();
            return this.PostJson();

        }

        public string PostToACS(){

            string acs_url = this.param["ACS_url"].ToString();
            string pareq = this.param["pareq"].ToString();
            string termurl = this.param["termurl"].ToString();
            string mdstr = this.param["md"].ToString();

            string post_data = "pareq=" + pareq + "&termurl=" + termurl + "&md=" + mdstr;
            string response = this.util.Http_request(acs_url, post_data, this.connectTimeout);

            this.ClearParam();

            return response;
        }

        public JObject PostSettleACS(){

            this.SetParam("action", "SETTLEMENT");
            this.SetBasicParam();
            return this.PostJson();

        }

        public JObject Auth_only(){

            this.SetParam("transaction_type", "AUTH_ONLY");
            this.SetBasicParam();
            return this.PostJson();

        }

        public JObject Auth_capture(){

            this.SetParam("transaction_type", "AUTH_CAPTURE");
            this.SetBasicParam();
            return this.PostJson();

        }

        public JObject Capture(){

            this.SetParam("transaction_type", "CAPTURE");
            this.SetBasicParam();
            return this.PostJson();

        }

        public JObject Void_deal(){

            this.SetParam("transaction_type", "VOID");
            this.SetBasicParam();
            return this.PostJson();

        }

        public JObject Refund(){
        
            this.SetParam("transaction_type", "REFUND");
            this.SetBasicParam();
            return this.PostJson();

        }

        public JObject Credit(){
        
            this.SetParam("transaction_type", "CREDIT");
            this.SetBasicParam();
            return this.PostJson();

        }

        public JObject Update_billing(){
            
            this.SetParam("transaction_type", "SUBSCRIPTION_MANAGE");
            this.SetParam("subscription_type", "UPDATE_BILLING");
            this.SetBasicParam();
            return this.PostJson();

        }

    }
}
