using System.ComponentModel;

namespace DncyTemplate.Mvc.Models.Account
{
    public class InMemoryAccount
    {
        public static IEnumerable<User> Users => new User[3]
        {
            new User
            {
                Id = "U0001",
                Name = "超级管理员",
                Tenant="T20210602000003",
                Account="sa",
                Roles = new RoleEnum[]
                {
                    RoleEnum.SA
                }
            },
            new User
            {
                Id = "U0002",
                Name = "管理员",
                Tenant="T20210602000001",
                Account="admin",
                Roles = new RoleEnum[]
                {
                    RoleEnum.Admin
                }
            },
            new User
            {
                Id = "U0003",
                Name = "普通用户",
                Tenant="T20210602000002",
                Account="user",
                Roles = new RoleEnum[]
                {
                    RoleEnum.Member
                }
            }
        };
    }


    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }

        public RoleEnum[] Roles { get; set; }

        public string Tenant { get; set; }
    }

    public enum RoleEnum : byte
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        [Description("超级管理员")]
        SA,
        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin,
        /// <summary>
        /// 普通成员
        /// </summary>
        [Description("普通成员")]
        Member
    }
}