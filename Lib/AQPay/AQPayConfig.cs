using System;

namespace Acquiredapisdkdotnet.Lib.AQPay
{
    public static class AQPayConfig
    {

        public static readonly string COMPANYID = "108";
        public static readonly string COMPANYPASS = "test";
        public static readonly string COMPANYMIDID = "1014";
        public static readonly string HASHCODE = "test";
         
        //require api url
        public static readonly string REQUESTURL = "http://devapi.paymentflow.com/api.php"; 
        //QA: https://qaapi.acquired.com/api.php
        //PROD: https://gateway.acquired.com/api.php    

        // curl timeout
        public static readonly int CURLTIMEOUT = 120;

    }
}
