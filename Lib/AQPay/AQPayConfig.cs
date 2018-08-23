using System;

namespace Acquiredapisdkdotnet.Lib.AQPay
{
    public static class AQPayConfig
    {

        public static readonly string COMPANYID = "";
        public static readonly string COMPANYPASS = "";
        public static readonly string COMPANYMIDID = "";
        public static readonly string HASHCODE = "";
         
        //require api url
        public static readonly string REQUESTURL = "";
        //QA: https://qaapi.acquired.com/api.php
        //PROD: https://gateway.acquired.com/api.php    

        // curl timeout
        public static readonly int CURLTIMEOUT = 120;

    }
}
