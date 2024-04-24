using System.ComponentModel;

namespace DncyTemplate.Mvc.Models.Account
{
    public class InMemoryAccount
    {
        public static List<User> GetUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = "U0001",
                    Name = "超级管理员",
                    Account = "sa_1",
                    Tenant = "T20210602000001",
                    Roles = new RoleEnum[]
                    {
                        RoleEnum.SA
                    }
                },
                new User
                {
                    Id = "U0002",
                    Name = "管理员",
                    Account = "admin_1",
                    Tenant = "T20210602000001",
                    Roles = new RoleEnum[]
                    {
                        RoleEnum.Admin
                    }
                },
                new User
                {
                    Id = "U0003",
                    Name = "普通用户",
                    Account = "user_1",
                    Tenant = "T20210602000001",
                    Roles = new RoleEnum[]
                    {
                        RoleEnum.Member
                    }
                },

                new User
                {
                    Id = "U0001",
                    Name = "超级管理员",
                    Account = "sa_2",
                    Tenant = "T20210602000002",
                    Roles = new RoleEnum[]
                    {
                        RoleEnum.SA
                    }
                },
                new User
                {
                    Id = "U0002",
                    Name = "管理员",
                    Account = "admin_2",
                    Tenant = "T20210602000002",
                    Roles = new RoleEnum[]
                    {
                        RoleEnum.Admin
                    }
                },
                new User
                {
                    Id = "U0003",
                    Name = "普通用户",
                    Account = "user_2",
                    Tenant = "T20210602000002",
                    Roles = new RoleEnum[]
                    {
                        RoleEnum.Member
                    }
                },
                new User
                {
                    Id = "U0001",
                    Name = "超级管理员",
                    Account = "sa_3",
                    Tenant = "T20210602000003",
                    Roles = new RoleEnum[]
                    {
                        RoleEnum.SA
                    }
                },
                new User
                {
                    Id = "U0002",
                    Name = "管理员",
                    Account = "admin_3",
                    Tenant = "T20210602000003",
                    Roles = new RoleEnum[]
                    {
                        RoleEnum.Admin
                    }
                },
                new User
                {
                    Id = "U0003",
                    Name = "普通用户",
                    Account = "user_3",
                    Tenant = "T20210602000003",
                    Roles = new RoleEnum[]
                    {
                        RoleEnum.Member
                    }
                }
            };
        }

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