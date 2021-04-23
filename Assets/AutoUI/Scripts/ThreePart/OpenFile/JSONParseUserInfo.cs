
/// <summary>
/// 导出表格的时候用于解析用户数据信息
/// </summary>
public class JSONParseUserInfo {
    /// <summary>
    /// json信息
    /// </summary>
    public static string json { get; set; }

    /// <summary>
    /// 解析得到json信息
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static JSONParseUserInfo GetUserInfo(string json) {
        try
        {
            return LitJson.JsonMapper.ToObject<JSONParseUserInfo>(json.Contains("\"lockId\":null") ? json.Replace("\"lockId\":null", "\"lockId\":111111111111111") : json);
        }
        catch (System.Exception ex)
        {
            UnityEngine. Debug.Log(ex.Message) ;
            throw;
        }
    }

    /// <summary>
    /// 错误码
    /// </summary>
    public int resultCode { get; set; }
    /// <summary>
    /// 错误信息
    /// </summary>
    public string errMessage { get; set; }
    /// <summary>
    /// 数据
    /// </summary>
    public Data data { get; set; }

    public class Data{
        public int total { get; set; }
        public int pageSize { get; set; }
        public int page { get; set; }
        public int records { get; set; }

        public Row[] rows { get; set; }
        public class Row {
            public int id { get; set; }
            public string orderCode { get; set; }
            public int paymentType { get; set; }
            public System.Int64 lockId { get {
                    if (LockId == 111111111111111)
                    {
                        lockId = 0;
                    }
                    return lockId;
                } set { LockId = value;
                } }
            public System.Int64 LockId { get; set; }
            public string username { get; set; }
            public string mobile { get; set; }
            public string address { get; set; }
            public string houseNo { get; set; }
            public string productCode { get; set; }
            public string preparedBy { get; set; }
            public string createTime { get; set; }
            public int status { get; set; }
            public string preparedByName { get; set; }
            public string approver { get; set; }
            public string approverName { get; set; }
            public string approvalTime { get; set; }
        }
    }
}
