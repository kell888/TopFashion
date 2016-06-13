using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopFashion
{
    //基础枚举信息
    /// <summary>
    /// 网格地图坐标信息来源枚举
    /// </summary>
    public enum STMapSource : uint
    {
        /// <summary>
        /// 人口
        /// </summary>
        perManage = 1,
        /// <summary>
        /// 房屋
        /// </summary>
        letHouse = 2,
        /// <summary>
        /// 排查整治事件
        /// </summary>
        punEvent = 3,
        /// <summary>
        /// 矛盾联调，中心受理事件
        /// </summary>
        conEvent = 4,
        /// <summary>
        /// 城市部件
        /// </summary>
        cityCommponent = 5,
        /// <summary>
        /// 网格区划
        /// </summary>
        gridRegion = 6
    }

    /// <summary>
    /// 地图标记类型枚举
    /// </summary>
    public enum STMapType : uint
    {
        /// <summary>
        /// 点
        /// </summary>
        point = 1,
        /// <summary>
        /// 线
        /// </summary>
        line = 2,
        /// <summary>
        /// 圆形
        /// </summary>
        circle = 3,
        /// <summary>
        /// 矩形
        /// </summary>
        rectangle = 4,
        /// <summary>
        /// 自定义区域
        /// </summary>
        zone = 5,
        /// <summary>
        /// 路线
        /// </summary>
        roadline = 6
    }

    /// <summary>
    /// 城市部件信息枚举
    /// </summary>
    public enum STCityComponent : uint
    {
        /// <summary>
        /// 桥梁
        /// </summary>
        bridge = 1,
        /// <summary>
        /// 消防设备
        /// </summary>
        fireStation = 2,
        fireControl = 3,
        /// <summary>
        /// 停车场
        /// </summary>
        park = 4,
        /// <summary>
        /// 交通灯
        /// </summary>
        trafficLight = 5,
        /// <summary>
        /// 广告牌
        /// </summary>
        advBoard = 6,
        /// <summary>
        /// 重点建筑
        /// </summary>
        building = 7,
        /// <summary>
        /// 重要场所
        /// </summary>
        place = 8
    }

    /// <summary>
    /// 网格组织成员类别信息
    /// </summary>
    public enum STGridPersonType : uint
    {
        /// <summary>
        /// 格长
        /// </summary>
        gridLeader = 1,
        /// <summary>
        /// 网格监督员
        /// </summary>
        gridSuper = 2,
        /// <summary>
        /// 网格员
        /// </summary>
        gridCommon = 3,
        /// <summary>
        /// 楼长
        /// </summary>
        gridBuilder = 4
    }

    /// <summary>
    /// 部门级别枚举信息
    /// </summary>
    public enum STOrgLevel : uint
    {
        /// <summary>
        /// 系统管理
        /// </summary>
        orgSystem = 99,
        /// <summary>
        /// 省级政法
        /// </summary>
        orgProvinceH = 90,
        /// <summary>
        /// 省级交办
        /// </summary>
        orgProvinceL = 85,
        /// <summary>
        /// 市级政法
        /// </summary>
        orgCityH = 80,
        /// <summary>
        /// 市级交办
        /// </summary>
        orgCityL = 75,
        /// <summary>
        /// 县级政法
        /// </summary>
        orgCountyH = 70,
        /// <summary>
        /// 县级交办
        /// </summary>
        orgCountyL = 65,
        /// <summary>
        /// 镇级政法
        /// </summary>
        orgTownH = 60,
        /// <summary>
        /// 镇级交办
        /// </summary>
        orgTownL = 55,
        /// <summary>
        /// 村/社区
        /// </summary>
        orgCountry = 30,
        /// <summary>
        /// 网格
        /// </summary>
        orgGrid = 10,
        /// <summary>
        /// 个人
        /// </summary>
        orgPerson = 05
    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum STUserType : uint
    {
        /// <summary>
        /// 试用用户
        /// </summary>
        trialer = 0,
        /// <summary>
        /// 正式用户
        /// </summary>
        formaler = 1,
        /// <summary>
        /// 志愿者
        /// </summary>
        volunteer = 2
    }

    /// <summary>
    /// 系统人口类型枚举信息
    /// </summary>
    public enum STPersonType : uint
    {
        /// <summary>
        /// 常住人口
        /// </summary>
        personLive = 1,
        /// <summary>
        /// 流动人口
        /// </summary>
        personFlow = 2,
        /// <summary>
        /// 刑释解教人口
        /// </summary>
        personXS = 3,
        /// <summary>
        /// 社区矫正人口
        /// </summary>
        personJZ = 4,
        /// <summary>
        /// 精神病患者
        /// </summary>
        personJS = 5,
        /// <summary>
        /// 上访重点户
        /// </summary>
        personSF = 6,
        /// <summary>
        /// 吸毒人员
        /// </summary>
        personXD = 7,
        /// <summary>
        /// 留守妇女
        /// </summary>
        personLF = 8,
        /// <summary>
        /// 空巢老人
        /// </summary>
        personKC = 9,
        /// <summary>
        /// 少年儿童
        /// </summary>
        personET = 10,
        /// <summary>
        /// 计生人群
        /// </summary>
        personJQ=11,
        /// <summary>
        /// 特困人口
        /// </summary>
        personTK=12,
        /// <summary>
        /// 重点青少年
        /// </summary>
        personZD=13,
        /// <summary>
        /// 东西回流人员
        /// </summary>
        personDX=14,
        /// <summary>
        /// 314遣返人员
        /// </summary>
        personQF=15,
        /// <summary>
        /// 邪教组织人员
        /// </summary>
        personXJ=16,

        /// <summary>
        /// 案件当事人信息
        /// </summary>
        personParty = 100,

        /// <summary>
        /// 房东信息
        /// </summary>
        personLandlord = 200
    }

    /// <summary>
    /// 用户角色信息
    /// </summary>
    public static class STUserRole
    {
        /// <summary>
        /// 个人用户
        /// </summary>
        public static string roleUser = "00";

        /// <summary>
        /// 监控员
        /// </summary>
        public static string roleMonitor = "01";

        /// <summary>
        /// 办事员
        /// </summary>
        public static string roleHandle = "02";

        /// <summary>
        /// 民意调查员
        /// </summary>
        public static string roleSurvey = "05";

        /// <summary>
        /// 社工
        /// </summary>
        public static string roleSocialworker = "06";

        /// <summary>
        /// 格长
        /// </summary>
        public static string roleGridLeader = "09";

        /// <summary>
        /// 社区领导
        /// </summary>
        public static string roleSocialLeader = "10";

        /// <summary>
        /// 镇企业
        /// </summary>
        public static string roleTownEnp = "20";
        /// <summary>
        /// 乡镇寺庙
        /// </summary>
        public static string roleTownTemp = "21";
        /// <summary>
        /// 镇部门
        /// </summary>
        public static string roleTownOrg = "22"; 
        /// <summary>
        /// 镇工作人员
        /// </summary>
        public static string roleTownWorker = "23";
        /// <summary>
        /// 镇乡领导
        /// </summary>
        public static string roleTownLeader = "24";
        /// <summary>
        /// 镇乡副主任
        /// </summary>
        public static string roleTownAssDir = "25";
        /// <summary>
        /// 镇乡主任
        /// </summary>
        public static string roleTownDirector = "26";
        /// <summary>
        /// 镇乡书记
        /// </summary>
        public static string roleTownSecretary = "27";
        /// <summary>
        /// 镇乡管理员
        /// </summary>
        public static string roleTownAdmin = "29";

        /// <summary>
        /// 县区企业
        /// </summary>
        public static string roleCountyEnp = "30";
        /// <summary>
        /// 区县寺庙
        /// </summary>
        public static string roleCountyTemp = "31";
        /// <summary>
        /// 县区部门
        /// </summary>
        public static string roleCountyOrg = "32";
        /// <summary>
        /// 县区工作人员
        /// </summary>
        public static string roleCountyWorker = "33";
        /// <summary>
        /// 县区领导
        /// </summary>
        public static string roleCountyLeader = "34";
        /// <summary>
        /// 县区副主任
        /// </summary>
        public static string roleCountyAssDir = "35";
        /// <summary>
        /// 县区主任
        /// </summary>
        public static string roleCountyDirector = "36";
        /// <summary>
        /// 县区书记
        /// </summary>
        public static string roleCountySecretary = "37";
        /// <summary>
        /// 县区管理员
        /// </summary>
        public static string roleCountyAdmin = "39";

        /// <summary>
        /// 市企业
        /// </summary>
        public static string roleCityEnp = "40";
        /// <summary>
        /// 市区寺庙
        /// </summary>
        public static string roleCityTemp = "41";
        /// <summary>
        /// 市部门
        /// </summary>
        public static string roleCityOrg = "42";
        /// <summary>
        /// 市工作人员
        /// </summary>
        public static string roleCityWorker = "43";
        /// <summary>
        /// 市区领导
        /// </summary>
        public static string roleCityLeader = "44";
        /// <summary>
        /// 市主任
        /// </summary>
        public static string roleCityDirector = "45";
        /// <summary>
        /// 市书记
        /// </summary>
        public static string roleCitySecretary = "46";
        /// <summary>
        /// 市管理员
        /// </summary>
        public static string roleCityAdmin = "49";

        /// <summary>
        /// 省级工作人员
        /// </summary>
        public static string roleProvinceWorker = "50";

    }

    public static class CommonCitys
    {
        static string[] city3D = new string[] { "广州", "潮州", "惠州", "韶关", "深圳", "合肥", "马鞍山", "芜湖", "宣城", "福州", "莆田", "泉州", "厦门", "贵阳", "石家庄", "承德", "廊坊", "郑州", "安阳", "洛阳", "新乡", "驻马店", "濮阳", "哈尔滨", "武汉", "恩施", "荆门", "荆州", "随州", "宜昌", "长沙", "郴州", "张家界", "长春", "吉林", "南京", "常州", "连云港", "南通", "苏州", "扬州", "南昌", "九江", "沈阳", "大连", "抚顺", "锦州", "营口", "呼和浩特", "包头", "赤峰", "鄂尔多斯", "济南", "东营", "济宁", "临沂", "青岛", "日照", "威海", "淄博", "晋中", "临汾", "朔州", "阳泉", "西安", "安康", "汉中", "成都", "南充", "遂宁", "宜宾", "阳泉", "泸州", "乌鲁木齐", "丽江", "杭州", "湖州", "金华", "宁波", "绍兴", "衢州" };
        static List<string> ltCity3D = new List<string>(city3D);
        public static List<string> LtCity3D
        {
            get { return CommonCitys.ltCity3D; }
            set { CommonCitys.ltCity3D = value; }
        }
    }
}
