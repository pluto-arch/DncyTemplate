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
                #if Tenant
                Tenant="T20210602000003",
#endif
                Account="sa",
                Roles =
                [
                    RoleEnum.SA
                ]
            },
            new User
            {
                Id = "U0002",
                Name = "管理员",
#if Tenant
                Tenant="T20210602000001",
#endif
                Account="admin",
                Roles =
                [
                    RoleEnum.Admin
                ]
            },
            new User
            {
                Id = "U0003",
                Name = "普通用户",
#if Tenant
                Tenant="T20210602000002",
#endif
                Account="user",
                Roles =
                [
                    RoleEnum.Member
                ]
            }
        };
    }


    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }

        public RoleEnum[] Roles { get; set; }

#if Tenant
        public string Tenant { get; set; }
#endif
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