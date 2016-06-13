using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NPOI.SS.UserModel;

namespace TopFashion
{
    public delegate void ImportDataProcessHandler(int percent);

    public static class Common
    {
        public static RoleCollection GetRoles(string roleids, RoleLogic rl = null)
        {
            RoleCollection roles = new RoleCollection();
            string[] ids = roleids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (rl == null) rl = RoleLogic.GetInstance();
            foreach (string id in ids)
            {
                int I;
                if (int.TryParse(id, out I))
                {
                    Role role = rl.GetRole(I);
                    roles.Add(role);
                }
            }
            return roles;
        }

        public static List<UserGroup> GetUserGroups(string ugroups, UserGroupLogic ul = null)
        {
            List<UserGroup> ugrps = new List<UserGroup>();
            string[] ids = ugroups.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (ul == null) ul = UserGroupLogic.GetInstance();
            foreach (string id in ids)
            {
                int I;
                if (int.TryParse(id, out I))
                {
                    UserGroup ug = ul.GetUserGroup(I);
                    ugrps.Add(ug);
                }
            }
            return ugrps;
        }

        public static List<Department> GetDepartments(string deps, DepartmentLogic dl= null)
        {
            List<Department> depts = new List<Department>();
            string[] ids = deps.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (dl == null) dl = DepartmentLogic.GetInstance();
            foreach (string id in ids)
            {
                int I;
                if (int.TryParse(id, out I))
                {
                    Department dep = dl.GetDepartment(I);
                    depts.Add(dep);
                }
            }
            return depts;
        }
        public static List<int> GetRoleIds(string roleids, RoleLogic rl = null)
        {
            List<int> roles = new List<int>();
            string[] ids = roleids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (rl == null) rl = RoleLogic.GetInstance();
            foreach (string id in ids)
            {
                int I;
                if (int.TryParse(id, out I))
                {
                    roles.Add(I);
                }
            }
            return roles;
        }

        public static List<int> GetUserGroupIds(string ugroups, UserGroupLogic ul = null)
        {
            List<int> ugrps = new List<int>();
            string[] ids = ugroups.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (ul == null) ul = UserGroupLogic.GetInstance();
            foreach (string id in ids)
            {
                int I;
                if (int.TryParse(id, out I))
                {
                    ugrps.Add(I);
                }
            }
            return ugrps;
        }

        public static List<int> GetDepartmentIds(string deps, DepartmentLogic dl = null)
        {
            List<int> depts = new List<int>();
            string[] ids = deps.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (dl == null) dl = DepartmentLogic.GetInstance();
            foreach (string id in ids)
            {
                int I;
                if (int.TryParse(id, out I))
                {
                    depts.Add(I);
                }
            }
            return depts;
        }

        public static PermissionCollection GetPermissions(string pers, PermissionLogic pl = null)
        {
            PermissionCollection perms = new PermissionCollection();
            string[] ids = pers.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (pl == null) pl = PermissionLogic.GetInstance();
            foreach (string id in ids)
            {
                int I;
                if (int.TryParse(id, out I))
                {
                    Permission perm = pl.GetPermission(I);
                    perms.Add(perm);
                }
            }
            return perms;
        }

        public static List<int> GetPermissionIds(string pers, PermissionLogic pl = null)
        {
            List<int> perms = new List<int>();
            string[] ids = pers.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (pl == null) pl = PermissionLogic.GetInstance();
            foreach (string id in ids)
            {
                int I;
                if (int.TryParse(id, out I))
                {
                    perms.Add(I);
                }
            }
            return perms;
        }

        public static string GetRolesStr(RoleCollection roles)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Role role in roles)
            {
                if (sb.Length == 0)
                    sb.Append(role.ID.ToString());
                else
                    sb.Append("," + role.ID.ToString());
            }
            return sb.ToString();
        }

        public static string GetPermissionsStr(PermissionCollection perms)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Permission perm in perms)
            {
                if (sb.Length == 0)
                    sb.Append(perm.ID.ToString());
                else
                    sb.Append("," + perm.ID.ToString());
            }
            return sb.ToString();
        }

        public static string GetUserGroupsStr(List<UserGroup> ugrps)
        {
            StringBuilder sb = new StringBuilder();
            foreach (UserGroup ug in ugrps)
            {
                if (sb.Length == 0)
                    sb.Append(ug.ID.ToString());
                else
                    sb.Append("," + ug.ID.ToString());
            }
            return sb.ToString();
        }

        public static string GetDepartmentsStr(List<Department> deps)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Department dep in deps)
            {
                if (sb.Length == 0)
                    sb.Append(dep.ID.ToString());
                else
                    sb.Append("," + dep.ID.ToString());
            }
            return sb.ToString();
        }

        public static string GetRolesStr(List<int> roles)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int role in roles)
            {
                if (sb.Length == 0)
                    sb.Append(role.ToString());
                else
                    sb.Append("," + role.ToString());
            }
            return sb.ToString();
        }

        public static string GetPermissionsStr(List<int> perms)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int perm in perms)
            {
                if (sb.Length == 0)
                    sb.Append(perm.ToString());
                else
                    sb.Append("," + perm.ToString());
            }
            return sb.ToString();
        }

        public static string GetUserGroupsStr(List<int> ugrps)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int ug in ugrps)
            {
                if (sb.Length == 0)
                    sb.Append(ug.ToString());
                else
                    sb.Append("," + ug.ToString());
            }
            return sb.ToString();
        }

        public static string GetDepartmentsStr(List<int> deps)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int dep in deps)
            {
                if (sb.Length == 0)
                    sb.Append(dep.ToString());
                else
                    sb.Append("," + dep.ToString());
            }
            return sb.ToString();
        }

        private static int adminId;
        public static int AdminId
        {
            get
            {
                if (adminId == 0)
                {
                    int id = 0;
                    User admin = UserLogic.GetInstance().GetUser(Configs.AdminName);
                    if (admin != null)
                        id = admin.ID;
                    adminId = id;
                }
                return adminId;
            }
        }

        public static bool ExportData(DataTable data, string filename)
        {
            int rows = 0;
            using (ExcelHelper excelHelper = new ExcelHelper(filename))
            {
                rows = excelHelper.DataTableToExcel(data, "Sheet1", true);
            }
            return rows > 0;
        }

        public static bool ImportData(string elementType, bool clearOldData, DataTable data, FieldMap<string, string> map, ImportDataProcessHandler process)
        {
            if (data == null && data.Rows.Count == 0)
                return false;
            if (map.Count == 0)
                return false;

            try
            {
                Assembly ass = null;
                Type t = null;
                string typeName = null;
                //清空数据库中的原有数据
                switch (elementType)
                {
                    case "会员":
                        ass = Assembly.Load("Model");
                        t = ass.GetType("TopFashion.Member", false, true);
                        typeName = t.FullName;
                        if (clearOldData)
                            MemberLogic.GetInstance().ClearMembers();
                        break;
                    case "员工":
                        ass = Assembly.Load("Model");
                        t = ass.GetType("TopFashion.Staff", false, true);
                        typeName = t.FullName;
                        if (clearOldData)
                            StaffLogic.GetInstance().ClearStaffs();
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(typeName))
                {
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        DataRow row = data.Rows[i];//FormatData(data.Rows[i]);
                        //保存数据到数据库
                        switch (elementType)
                        {
                            case "会员":
                                Member elementM = ass.CreateInstance(typeName, true) as Member;
                                Member member = GetData<Member>(row, map, elementM, t);
                                MemberLogic.GetInstance().AddMember(member);
                                break;
                            case "员工":
                                Staff elementS = ass.CreateInstance(typeName, true) as Staff;
                                Staff staff = GetData<Staff>(row, map, elementS, t);
                                StaffLogic.GetInstance().AddStaff(staff);
                                break;
                            default:
                                break;
                        }
                        //反馈进度给外部程序
                        if (process != null)
                            process(i + 1);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<string> GetFieldsBySysElement(string elementType)
        {
            List<string> fields = new List<string>();
            Assembly ass;
            switch (elementType)
            {
                case "会员":
                    ass = Assembly.Load("Model");
                    if (ass != null)
                    {
                        Type t = ass.GetType("TopFashion.Member", false, true);
                        if (t != null)
                        {
                            PropertyInfo[] pis = t.GetProperties();
                            foreach (PropertyInfo pi in pis)
                            {
                                fields.Add(pi.Name);
                            }
                        }
                    }
                    break;
                case "员工":
                    ass = Assembly.Load("Model");
                    if (ass != null)
                    {
                        Type t = ass.GetType("TopFashion.Staff", false, true);
                        if (t != null)
                        {
                            PropertyInfo[] pis = t.GetProperties();
                            foreach (PropertyInfo pi in pis)
                            {
                                fields.Add(pi.Name);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return fields;
        }

        public static bool SaveMapToLocal<T, K>(FieldMap<T, K> map, string filePath)
            where T : class
            where K : class
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, map);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static FieldMap<T, K> LoadMapFromLocal<T, K>(string filePath)
            where T : class
            where K : class
        {
            FieldMap<T, K> map = null;
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    map = (FieldMap<T, K>)bf.Deserialize(fs);
                }
            }
            return map;
        }

        public static R GetData<R>(DataRow row, FieldMap<string, string> map, R element, Type t)
            where R : class
        {
            if (row != null && map != null)
            {
                if (element != null && t != null)
                {
                    PropertyInfo[] pis = t.GetProperties();
                    for (int i = 0; i < pis.Length; i++)
                    {
                        PropertyInfo pi = pis[i];
                        if (pi.CanWrite && map.Keys.Contains(pi.Name))
                        {
                            try
                            {
                                t.GetProperty(pi.Name).SetValue(element, row[map[pi.Name]], null);
                            }
                            catch
                            {
                                try
                                {
                                    string colName = map[pi.Name];
                                    object cell = row[colName];
                                    if (cell != null)
                                    {
                                        if (colName.Contains("性别"))
                                        {
                                            t.GetProperty(pi.Name).SetValue(element, (性别)Enum.Parse(typeof(性别), cell.ToString()), null);
                                        }
                                        else if (colName.Contains("类型") || colName.Contains("种类") || colName.Contains("卡种"))
                                        {
                                            CardType ct = CardTypeLogic.GetInstance().GetCardTypeByName(cell.ToString());
                                            t.GetProperty(pi.Name).SetValue(element, ct, null);
                                        }
                                        else if (colName.Contains("状态"))
                                        {
                                            StaffCondition sc = StaffConditionLogic.GetInstance().GetStaffConditionByName(cell.ToString());
                                            t.GetProperty(pi.Name).SetValue(element, sc, null);
                                        }
                                        else if (colName.Contains("开始日") ||colName.Contains("到期日") || colName.Contains("生日") || colName.Contains("日期") || colName.Contains("时间"))
                                        {

                                            DateTime dt = DateTime.MinValue;
                                            DateTime T;
                                            if (DateTime.TryParse(cell.ToString(), out T))
                                                dt = T;
                                            t.GetProperty(pi.Name).SetValue(element, dt, null);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    throw e;
                                }
                            }
                        }
                    }
                }
            }
            return element;
        }

        /// <summary>
        /// 读取Excel数据(2007及以上的文档暂不支持，因为找不到合适的ICSharpCode.SharpZipLib版本)
        /// </summary>
        /// <param name="excelFile">读取指定Excel文档的第一个Sheet</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <param name="fields">返回列名列表</param>
        /// <returns></returns>
        public static DataTable ReadDataFromExcel(string excelFile, bool isFirstRowColumn, out List<string> fields)
        {
            DataTable data = new DataTable();
            fields = null;
            if (File.Exists(excelFile))
            {
                IWorkbook wb = null;
                try
                {
                    using (FileStream fs = new FileStream(excelFile, FileMode.Open, FileAccess.Read))
                    {
                        wb = WorkbookFactory.Create(fs);
                        //if (excelFile.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase)) // 2007版本及其以上的版本
                        //    wb = new XSSFWorkbook(fs);
                        //else if (excelFile.EndsWith(".xls", StringComparison.InvariantCultureIgnoreCase)) // 2003版本
                        //    wb = new HSSFWorkbook(fs);
                    }
                    if (wb == null)
                        return null;
                    int startRow = 0;
                    ISheet sheet = wb.GetSheetAt(0);
                    if (sheet != null)
                    {
                        IRow firstRow = sheet.GetRow(0);
                        int cellFirst = firstRow.FirstCellNum;
                        int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                        fields = new List<string>();
                        if (isFirstRowColumn)
                        {
                            for (int i = cellFirst; i < cellCount; ++i)
                            {
                                ICell cell = firstRow.GetCell(i);
                                if (cell != null)
                                {
                                    string cellValue = cell.StringCellValue;
                                    if (cellValue != null)
                                    {
                                        fields.Add(cellValue);
                                        DataColumn column = new DataColumn(cellValue);
                                        data.Columns.Add(column);
                                    }
                                }
                            }
                            startRow = sheet.FirstRowNum + 1;
                        }
                        else
                        {
                            for (int i = cellFirst; i < cellCount; ++i)
                            {
                                string cellValue = "Field" + i;
                                fields.Add(cellValue);
                                DataColumn column = new DataColumn(cellValue);
                                data.Columns.Add(column);
                            }
                            startRow = sheet.FirstRowNum;
                        }

                        //最后一行的标号
                        int rowCount = sheet.LastRowNum;
                        for (int i = startRow; i <= rowCount; ++i)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue; //没有数据的行默认是null　　　　　　　

                            DataRow dataRow = data.NewRow();
                            for (int j = row.FirstCellNum; j < cellCount; ++j)
                            {
                                if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                    dataRow[j] = row.GetCell(j).ToString();
                            }
                            data.Rows.Add(dataRow);
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    wb = null;
                }
            }
            return data;
        }
    }
}
