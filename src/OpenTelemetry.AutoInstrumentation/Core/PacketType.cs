namespace WhaTap.Trace
{
    public enum PacketType
    {
        Blank = 0,
        Start = 1,
        DBConn = 2,
        //DBFetch = 3,
        DBSql = 4,
        //DBSqlStart = 5,
        //DBSqlEnd = 6,
        Httpc = 7,
        //Httpc_start = 8,
        //Httpc_end = 9,
        Error = 10,
        Msg = 11,
        Method = 12,
        SecureMsg = 13,
        Param = 30,
        ActiveStack = 40,
        ActiveStats = 41,
        End = 255
    }
}