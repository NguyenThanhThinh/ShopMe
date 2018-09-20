namespace ShopMe.Entities.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using ShopMe.Entities.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ShopMe.Entities.ShopMeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ShopMe.Entities.ShopMeDbContext context)
        {
            CreateRole();
            CreateGroup(context);
            CreateUser(context);
            CreateSystemConfigSample(context);
            CreateShopInfoSample(context);
           
        }
        private void CreateRole()
        {
            var roleManager = new RoleManager<Role>(new RoleStore<Role>(new ShopMeDbContext()));
            if (!roleManager.Roles.Any(x => x.Name == "Access"))
            {
                roleManager.Create(new Role { Name = "Access", Description = "Truy cập" });
            }
            if (!roleManager.Roles.Any(x => x.Name == "Edit"))
            {
                roleManager.Create(new Role { Name = "Edit", Description = "Chỉnh sửa" });
            }
            if (!roleManager.Roles.Any(x => x.Name == "Add"))
            {
                roleManager.Create(new Role { Name = "Add", Description = "Thêm mới" });
            }
            if (!roleManager.Roles.Any(x => x.Name == "Delete"))
            {
                roleManager.Create(new Role { Name = "Delete", Description = "Xóa" });
            }
        }

        private void CreateGroup(ShopMeDbContext context)
        {
            var roleManager = new RoleManager<Role>(new RoleStore<Role>(new ShopMeDbContext()));
            if (!context.Groups.Any(x => x.Name == "Admin"))
            {
                context.Groups.Add(new Group() { Name = "Admin", Description = "Quản trị" });
                context.SaveChanges();
                var groupId = context.Groups.Where(x => x.Name == "Admin").Select(x => x.ID).FirstOrDefault();
                List<RoleGroup> listRoleGroup = new List<RoleGroup>()
                  {
                      new RoleGroup() { RoleId = roleManager.FindByName("Access").Id.ToString(), GroupId=groupId},
                      new RoleGroup() { RoleId = roleManager.FindByName("Edit").Id.ToString(), GroupId=groupId },
                      new RoleGroup() { RoleId = roleManager.FindByName("Add").Id.ToString(), GroupId=groupId},
                      new RoleGroup() { RoleId = roleManager.FindByName("Delete").Id.ToString(), GroupId=groupId}
                  };
                context.RoleGroups.AddRange(listRoleGroup);
                context.SaveChanges();
            }
            if (!context.Groups.Any(x => x.Name == "Customer"))
            {
                context.Groups.Add(new Group() { Name = "Customer", Description = "Khách hàng" });
            }
        }

        private void CreateUser(ShopMeDbContext context)
        {
            if (context.Users.Count() == 0)
            {
                var manager = new UserManager<User>(new UserStore<User>(new ShopMeDbContext()));

                var roleManager = new RoleManager<Role>(new RoleStore<Role>(new ShopMeDbContext()));

                var user = new User()
                {
                    UserName = "thanhthinhgh1",
                    Email = "thanhthinhcntt@gmail.com",
                    EmailConfirmed = true,
                    BirthDay = DateTime.Now,
                    FullName = "Administrator"
                };

                manager.Create(user, "thanhthinh@123");

                var adminUser = manager.FindByEmail("thanhthinhcntt@gmail.com");

                manager.AddToRoles(adminUser.Id, new string[] { "Access", "Add", "Edit", "Delete" });
            }
            //Create user for app android
            if (!context.Users.Any(x => x.UserName == "ShopMe"))
            {
                var manager = new UserManager<User>(new UserStore<User>(new ShopMeDbContext()));
                var userApp = new User()
                {
                    UserName = "ShopMe",
                    Email = "lingok2014@gmail.com",
                    EmailConfirmed = true,
                    BirthDay = DateTime.Now,
                    FullName = "Ứng dụng Web"
                };
                manager.Create(userApp, "app2014@123");
            }
        }
        private void CreateSystemConfigSample(ShopMeDbContext context)
        {
            List<SystemConfig> listSystemConfig = new List<SystemConfig>();
            if (!context.SystemConfigs.Any(x => x.Code == "Pending"))
            {
                listSystemConfig.Add(new SystemConfig() { Code = "Pending", ValueString = "Chưa xử lý", ValueInt = 10 });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "Processing"))
            {
                listSystemConfig.Add(new SystemConfig() { Code = "Processing", ValueString = "Đang xử lý", ValueInt = 20 });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "Unconfirmed"))
            {
                listSystemConfig.Add(new SystemConfig() { Code = "Unconfirmed", ValueString = "Chưa xác nhận", ValueInt = 30 });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "Confirmed"))
            {
                listSystemConfig.Add(new SystemConfig() { Code = "Confirmed", ValueString = "Đã xác nhận", ValueInt = 40 });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "Complete"))
            {
                listSystemConfig.Add(new SystemConfig() { Code = "Complete", ValueString = "Đã hoàn thành", ValueInt = 50 });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "Cancelled"))
            {
                listSystemConfig.Add(new SystemConfig() { Code = "Cancelled", ValueString = "Hủy", ValueInt = 60 });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "HasShipped"))
            {
                listSystemConfig.Add(new SystemConfig() { Code = "HasShipped", ValueString = "Đã gửi đi", ValueInt = 70 });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "Packed"))
            {
                listSystemConfig.Add(new SystemConfig() { Code = "Packed", ValueString = "Đã đóng gói", ValueInt = 80 });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "Unpaid"))
            {
                listSystemConfig.Add(new SystemConfig() { Code = "Unpaid", ValueString = "Chưa thanh toán", ValueInt = 11 });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "Paid"))
            {
                listSystemConfig.Add(new SystemConfig() { Code = "Paid", ValueString = "Đã thanh toán", ValueInt = 21 });
            }

            context.SystemConfigs.AddRange(listSystemConfig);
            context.SaveChanges();
        }

        private void CreateShopInfoSample(ShopMeDbContext context)
        {
            if (!context.SystemConfigs.Any(x => x.Code == "ShopName"))
            {
                context.SystemConfigs.Add(new SystemConfig() { Code = "ShopName", ValueString = "ShopMe" });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "ShopPhone"))
            {
                context.SystemConfigs.Add(new SystemConfig() { Code = "ShopPhone", ValueString = "0965899230" });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "ShopAddress"))
            {
                context.SystemConfigs.Add(new SystemConfig() { Code = "ShopAddress", ValueString = "Cần Thơ" });
            }
            context.SaveChanges();
        }

    }
}
