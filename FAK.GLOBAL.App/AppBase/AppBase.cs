using FAK.Domain;
using FAK.Domain.Entities;
using FAK.Persistance;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace FAK.GLOBAL.App
{
    public abstract class AppBase
    {
        private readonly IConfiguration _configuration;
        private readonly JWTConfig _jWTConfig;

        public AppBase()
        {

        }
        public AppBase(AppDbContext appDbContext)
        {

        }

        public AppBase(AppDbContext appDbContext, IConfiguration configuration, IOptions<JWTConfig> options)
        {
            _configuration = configuration;
            _jWTConfig = options.Value;
        }

        public AppBase(IConfiguration configuration, IOptions<JWTConfig> options)
        {
            _configuration = configuration;
            _jWTConfig = options.Value;
        }


    }
    //public abstract class AppBase
    //{
    //    private readonly IConfiguration _configuration;

    //    public AppBase()
    //    {

    //    }
    //    public AppBase(AppDbContext appDbContext)
    //    {

    //    }

    //    public AppBase(AppDbContext appDbContext, IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }

    //    public AppBase(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }


    //    protected AppDbContext GetAppReadDbContext()
    //    {
    //        string Module = Domain.Enumerations.Module.GLOBAL.ToString();
    //        var configName = Module.ToUpper() + "_Read_ConStr";
    //        FAK.Common.Model.ApplicationSettings sett = new Common.Model.ApplicationSettings(_configuration);
    //        string conString = sett.GetConnectionString(configName);
    //        sett = null;

    //        var optionsBuilder = new DbContextOptionsBuilder<GLOBALDbContext>();
    //        optionsBuilder.UseSqlServer(conString);

    //        return new GLOBALDbContext(optionsBuilder.Options);
    //    }

    //    protected string GetValueByType(FilterValueType type, IList<ReportingDate> reportingdt, DateTime CurrDate)
    //    {
    //        string result = "";
    //        switch (type)
    //        {
    //            case FilterValueType.CURR_DATE:
    //                result = CurrDate.ToString("yyyy-MM-dd");
    //                break;
    //            case FilterValueType.PREV_DATE:
    //                result = CurrDate.AddDays(-1).ToString("yyyy-MM-dd");
    //                break;
    //            case FilterValueType.CURR_RPT_DAILY:
    //                result = reportingdt.Where(f => f.PeriodType == PeriodType.DAILY.ToString()).FirstOrDefault().ReportDate.ToString("yyyy-MM-dd");
    //                break;
    //            case FilterValueType.CURR_RPT_WEEK:
    //                result = reportingdt.Where(f => f.PeriodType == PeriodType.WEEKLY.ToString()).FirstOrDefault().ReportDate.ToString("yyyy-MM-dd");
    //                break;
    //            case FilterValueType.CURR_RPT_MONTH:
    //                result = reportingdt.Where(f => f.PeriodType == PeriodType.MONTHLY.ToString()).FirstOrDefault().ReportDate.ToString("yyyy-MM-dd");
    //                break;
    //            case FilterValueType.CURR_RPT_TRIWULAN:
    //                result = reportingdt.Where(f => f.PeriodType == PeriodType.QUARTERLY.ToString()).FirstOrDefault().ReportDate.ToString("yyyy-MM-dd");
    //                break;
    //            case FilterValueType.CURR_RPT_SEMESTER:
    //                result = reportingdt.Where(f => f.PeriodType == PeriodType.SEMESTER.ToString()).FirstOrDefault().ReportDate.ToString("yyyy-MM-dd");
    //                break;
    //            case FilterValueType.CURR_RPT_YEAR:
    //                result = reportingdt.Where(f => f.PeriodType == PeriodType.YEARLY.ToString()).FirstOrDefault().ReportDate.ToString("yyyy-MM-dd");
    //                break;

    //        }

    //        return result;
    //    }

    //    protected string ResolveFilter(IList<FilterModel> list)
    //    {

    //        if (list == null) return "";

    //        string result = "";
    //        FilterOperator opr;
    //        foreach (FilterModel m in list)
    //        {
    //            if (string.IsNullOrEmpty(m.optr))
    //            {
    //                continue;
    //            }

    //            opr = Utility.ParseEnum<FilterOperator>(m.optr);
    //            if ((opr != FilterOperator.EMPTY && opr != FilterOperator.NOTEMPTY && string.IsNullOrEmpty(m.value)) || string.IsNullOrEmpty(m.field) || string.IsNullOrEmpty(m.optr))
    //                continue;
    //            string field = Utility.SafeSqlString(m.field.Trim());
    //            string value = Utility.SafeSqlString(m.value);

    //            switch (m.optr)
    //            {
    //                case "CONTAINS":
    //                    result += field + " LIKE '%" + value + "%'";
    //                    break;
    //                case "NOTCONTAINS":
    //                    result += field + " NOT LIKE '%" + value + "%'";
    //                    break;
    //                case "INCLUDE":
    //                    string[] str = value.Split(',');
    //                    string conds = "";
    //                    for (int i = 0; i < str.Length; i++)
    //                    {
    //                        conds += "'" + str[i].Trim() + "',";
    //                    }
    //                    if (conds != "") conds = conds.Substring(0, conds.Length - 1);

    //                    result += field + " IN (" + conds + ")";

    //                    break;
    //                case "NOTINCLUDE":
    //                    string[] strn = value.Split(',');
    //                    string condsn = "";
    //                    for (int i = 0; i < strn.Length; i++)
    //                    {
    //                        condsn += "'" + strn[i].Trim() + "',";
    //                    }
    //                    if (condsn != "") condsn = condsn.Substring(0, condsn.Length - 1);

    //                    result += field + " NOT IN (" + condsn + ")";

    //                    break;
    //                case "EQUALS":
    //                    if (m.datatype == ControlType.DATETIME.ToString())
    //                    {
    //                        result += "CONVERT(DATE, " + field + ")='" + value + "'";
    //                    }
    //                    else
    //                    {
    //                        result += field + "='" + value + "'";
    //                    }
    //                    break;
    //                case "NOTEQUALS":
    //                    if (m.datatype == ControlType.DATETIME.ToString())
    //                    {
    //                        result += "CONVERT(DATE, " + field + ")!='" + value + "'";
    //                    }
    //                    else
    //                    {
    //                        result += field + "!='" + value + "'";
    //                    }
    //                    break;
    //                case "EMPTY":
    //                    if (m.datatype == ControlType.TEXT.ToString() || m.datatype == ControlType.LOOKUP.ToString())
    //                    {
    //                        result += " ISNULL(" + field + ",'') = ''";
    //                    }
    //                    else
    //                    {
    //                        result += field + " IS NULL";
    //                    }
    //                    break;
    //                case "NOTEMPTY":
    //                    if (m.datatype == ControlType.TEXT.ToString() || m.datatype == ControlType.LOOKUP.ToString())
    //                    {
    //                        result += " ISNULL(" + field + ",'') != ''";
    //                    }
    //                    else
    //                    {
    //                        result += field + " IS NOT NULL";
    //                    }
    //                    break;
    //                case "STARTWITH":
    //                    result += field + " LIKE '" + value + "%'";
    //                    break;
    //                case "ENDWITH":
    //                    result += field + " LIKE '%" + value + "'";
    //                    break;
    //                case "GREATER":
    //                    if (m.datatype == ControlType.DATETIME.ToString())
    //                    {
    //                        result += "CONVERT(DATE, " + field + ") >'" + value + "'";
    //                    }
    //                    else
    //                    {
    //                        result += field + ">'" + value + "'";
    //                    }

    //                    break;
    //                case "GREATERTHAN":
    //                    if (m.datatype == ControlType.DATETIME.ToString())
    //                    {
    //                        result += "CONVERT(DATE, " + field + ") >='" + value + "'";
    //                    }
    //                    else
    //                    {
    //                        result += field + ">='" + value + "'";
    //                    }

    //                    break;
    //                case "LESS":
    //                    if (m.datatype == ControlType.DATETIME.ToString())
    //                    {
    //                        result += "CONVERT(DATE, " + field + ") <'" + value + "'";
    //                    }
    //                    else
    //                    {
    //                        result += field + "<'" + value + "'";
    //                    }

    //                    break;
    //                case "LESSTHAN":
    //                    if (m.datatype == ControlType.DATETIME.ToString())
    //                    {
    //                        result += "CONVERT(DATE, " + field + ") <='" + value + "'";
    //                    }
    //                    else
    //                    {
    //                        result += field + "<='" + value + "'";
    //                    }
    //                    break;
    //            }
    //            result += " AND ";
    //        }

    //        if (!string.IsNullOrEmpty(result))
    //        {
    //            result = result.Substring(0, result.Length - 4);
    //        }

    //        return result;
    //    }

    //    protected Dictionary<string, List<object>> ResolveFilterDynamicLinq(IList<FilterModel> list)
    //    {
    //        Dictionary<string, List<object>> dic = new Dictionary<string, List<object>>();

    //        if (list == null) return dic;

    //        string result = "";
    //        FilterOperator opr;

    //        List<object> ls = new List<object>();

    //        int ctr = 0;
    //        foreach (FilterModel m in list)
    //        {
    //            if (string.IsNullOrEmpty(m.optr))
    //            {
    //                continue;
    //            }

    //            opr = Utility.ParseEnum<FilterOperator>(m.optr);
    //            if ((opr != FilterOperator.EMPTY && opr != FilterOperator.NOTEMPTY && string.IsNullOrEmpty(m.value)) || string.IsNullOrEmpty(m.field) || string.IsNullOrEmpty(m.optr))
    //                continue;
    //            string field = Utility.SafeSqlString(m.field.Trim());
    //            string value = Utility.SafeSqlString(m.value);

    //            switch (m.optr)
    //            {
    //                case "CONTAINS":
    //                    result += field + ".Contains(@" + ctr.ToString() + ")";
    //                    ls.Add(value);
    //                    break;
    //                case "NOTCONTAINS":
    //                    result += field + ".Contains(@" + ctr.ToString() + ")==false";
    //                    ls.Add(value);
    //                    break;
    //                case "INCLUDE":
    //                    string[] str = value.Split(',');
    //                    string conds = "";
    //                    for (int i = 0; i < str.Length; i++)
    //                    {
    //                        conds += field + "=@" + ctr.ToString() + " OR ";
    //                        ls.Add(str[i].Trim());
    //                        ctr++;
    //                    }

    //                    if (conds != "")
    //                    {
    //                        conds = conds.Substring(0, conds.Length - 3);
    //                        result += " (" + conds + ") ";
    //                        ctr--;
    //                    }

    //                    break;
    //                case "NOTINCLUDE":
    //                    string[] strn = value.Split(',');
    //                    string condsn = "";
    //                    for (int i = 0; i < strn.Length; i++)
    //                    {
    //                        condsn += field + "!=@" + ctr.ToString() + " AND ";
    //                        ls.Add(strn[i].Trim());
    //                        ctr++;
    //                    }

    //                    if (condsn != "")
    //                    {
    //                        condsn = condsn.Substring(0, condsn.Length - 4);
    //                        result += " (" + condsn + ") ";
    //                        ctr--;
    //                    }

    //                    break;
    //                case "EQUALS":
    //                    if (m.datatype == ControlType.DATETIME.ToString())
    //                    {
    //                        result += "Convert.ToDateTime(" + field + ").ToString(\"yyyy-MM-dd\")" + " = @" + ctr.ToString() + "";

    //                    }
    //                    else
    //                    {
    //                        result += field + "=@" + ctr.ToString() + "";
    //                    }
    //                    ls.Add(value);
    //                    break;
    //                case "NOTEQUALS":
    //                    if (m.datatype == ControlType.DATETIME.ToString())
    //                    {
    //                        result += "Convert.ToDateTime(" + field + ").ToString(\"yyyy-MM-dd\")" + " != @" + ctr.ToString() + "";

    //                    }
    //                    else
    //                    {
    //                        result += field + "!=@" + ctr.ToString() + "";
    //                    }
    //                    ls.Add(value);
    //                    break;
    //                case "EMPTY":
    //                    result += field + " = @" + ctr.ToString();
    //                    ls.Add(null);
    //                    break;
    //                case "NOTEMPTY":
    //                    result += field + " != @" + ctr.ToString();
    //                    ls.Add(null);
    //                    break;
    //                case "STARTWITH":
    //                    result += field + ".StartsWith(@" + ctr.ToString() + ") ";
    //                    ls.Add(value);
    //                    break;
    //                case "ENDWITH":
    //                    result += field + ".EndsWith(@" + ctr.ToString() + ")";
    //                    ls.Add(value);
    //                    break;
    //                case "GREATER":
    //                    if (m.datatype == ControlType.DATETIME.ToString())
    //                    {
    //                        result += "Convert.ToDateTime(" + field + ").ToString(\"yyyy-MM-dd\")" + ">@" + ctr.ToString() + "";
    //                    }
    //                    else
    //                    {
    //                        result += field + ">@" + ctr.ToString() + "";
    //                    }
    //                    ls.Add(value);
    //                    break;
    //                case "GREATERTHAN":
    //                    if (m.datatype == ControlType.DATETIME.ToString())
    //                    {
    //                        result += "Convert.ToDateTime(" + field + ").ToString(\"yyyy-MM-dd\")" + ">=@" + ctr.ToString() + "";
    //                    }
    //                    else
    //                    {
    //                        result += field + ">=@" + ctr.ToString() + "";
    //                    }
    //                    ls.Add(value);
    //                    break;
    //                case "LESS":
    //                    if (m.datatype == ControlType.DATETIME.ToString())
    //                    {
    //                        result += "Convert.ToDateTime(" + field + ").ToString(\"yyyy-MM-dd\")" + "<@" + ctr.ToString() + "";
    //                    }
    //                    else
    //                    {
    //                        result += field + "<@" + ctr.ToString() + "";
    //                    }
    //                    ls.Add(value);
    //                    break;
    //                case "LESSTHAN":
    //                    if (m.datatype == ControlType.DATETIME.ToString())
    //                    {
    //                        result += "Convert.ToDateTime(" + field + ").ToString(\"yyyy-MM-dd\")" + "<=@" + ctr.ToString() + "";
    //                    }
    //                    else
    //                    {
    //                        result += field + "<=@" + ctr.ToString() + "";
    //                    }
    //                    ls.Add(value);
    //                    break;
    //            }

    //            result += " AND ";
    //            ctr++;
    //        }

    //        if (!string.IsNullOrEmpty(result))
    //        {
    //            result = result.Substring(0, result.Length - 4);
    //        }

    //        dic.Add(result, ls);
    //        return dic;
    //    }


    //    protected Dictionary<string, List<object>> ResolveFilterNoConvertDateDynamicLinq(IList<FilterModel> list)
    //    {
    //        Dictionary<string, List<object>> dic = new Dictionary<string, List<object>>();

    //        if (list == null) return dic;

    //        string result = "";
    //        FilterOperator opr;

    //        List<object> ls = new List<object>();

    //        int ctr = 0;
    //        foreach (FilterModel m in list)
    //        {
    //            if (string.IsNullOrEmpty(m.optr))
    //            {
    //                continue;
    //            }

    //            opr = Utility.ParseEnum<FilterOperator>(m.optr);
    //            if ((opr != FilterOperator.EMPTY && opr != FilterOperator.NOTEMPTY && string.IsNullOrEmpty(m.value)) || string.IsNullOrEmpty(m.field) || string.IsNullOrEmpty(m.optr))
    //                continue;
    //            string field = Utility.SafeSqlString(m.field.Trim());
    //            string value = Utility.SafeSqlString(m.value);

    //            switch (m.optr)
    //            {
    //                case "CONTAINS":
    //                    result += field + ".Contains(@" + ctr.ToString() + ")";
    //                    ls.Add(value);
    //                    break;
    //                case "NOTCONTAINS":
    //                    result += field + ".Contains(@" + ctr.ToString() + ")==false";
    //                    ls.Add(value);
    //                    break;
    //                case "INCLUDE":
    //                    string[] str = value.Split(',');
    //                    string conds = "";
    //                    for (int i = 0; i < str.Length; i++)
    //                    {
    //                        conds += field + "=@" + ctr.ToString() + " OR ";
    //                        ls.Add(str[i].Trim());
    //                        ctr++;
    //                    }

    //                    if (conds != "")
    //                    {
    //                        conds = conds.Substring(0, conds.Length - 3);
    //                        result += " (" + conds + ") ";
    //                        ctr--;
    //                    }

    //                    break;
    //                case "NOTINCLUDE":
    //                    string[] strn = value.Split(',');
    //                    string condsn = "";
    //                    for (int i = 0; i < strn.Length; i++)
    //                    {
    //                        condsn += field + "!=@" + ctr.ToString() + " AND ";
    //                        ls.Add(strn[i].Trim());
    //                        ctr++;
    //                    }

    //                    if (condsn != "")
    //                    {
    //                        condsn = condsn.Substring(0, condsn.Length - 4);
    //                        result += " (" + condsn + ") ";
    //                        ctr--;
    //                    }

    //                    break;
    //                case "EQUALS":
    //                    result += field + "=@" + ctr.ToString() + "";
    //                    ls.Add(value);
    //                    break;
    //                case "NOTEQUALS":
    //                    result += field + "!=@" + ctr.ToString() + "";
    //                    ls.Add(value);
    //                    break;
    //                case "EMPTY":
    //                    result += field + " = @" + ctr.ToString();
    //                    ls.Add(null);
    //                    break;
    //                case "NOTEMPTY":
    //                    result += field + " != @" + ctr.ToString();
    //                    ls.Add(null);
    //                    break;
    //                case "STARTWITH":
    //                    result += field + ".StartsWith(@" + ctr.ToString() + ") ";
    //                    ls.Add(value);
    //                    break;
    //                case "ENDWITH":
    //                    result += field + ".EndsWith(@" + ctr.ToString() + ")";
    //                    ls.Add(value);
    //                    break;
    //                case "GREATER":
    //                    result += field + ">@" + ctr.ToString() + "";
    //                    ls.Add(value);
    //                    break;
    //                case "GREATERTHAN":
    //                    result += field + ">=@" + ctr.ToString() + "";
    //                    ls.Add(value);
    //                    break;
    //                case "LESS":
    //                    result += field + "<@" + ctr.ToString() + "";
    //                    ls.Add(value);
    //                    break;
    //                case "LESSTHAN":
    //                    result += field + "<=@" + ctr.ToString() + "";
    //                    ls.Add(value);
    //                    break;
    //            }

    //            result += " AND ";
    //            ctr++;
    //        }

    //        if (!string.IsNullOrEmpty(result))
    //        {
    //            result = result.Substring(0, result.Length - 4);
    //        }

    //        dic.Add(result, ls);
    //        return dic;
    //    }

    //    protected string GetActionType(string screenAction)
    //    {
    //        string result = "";
    //        if (screenAction == ScreenAction.ADD.ToString() || screenAction == ScreenAction.ADD_DTL.ToString())
    //        {
    //            result = OperationType.ADD.ToString();
    //        }
    //        else if (screenAction == ScreenAction.EDIT.ToString() || screenAction == ScreenAction.EDIT_DTL.ToString())
    //        {
    //            result = OperationType.EDIT.ToString();
    //        }
    //        else if (screenAction == ScreenAction.REMOVE.ToString() || screenAction == ScreenAction.REMOVE_DTL.ToString())
    //        {
    //            result = OperationType.REMOVE.ToString();
    //        }
    //        return result;
    //    }


    //    protected void AddAuditLog<T>(OperationType operationType, object model)
    //    {
    //        //call function on Audit Trail Class
    //    }

    //    protected void AddAuditLog<T>(OperationType operationType, List<T> listmodel)
    //    {
    //        //call function on Audit Trail Class
    //    }
    //}
}
