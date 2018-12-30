using Backend.Models;
using SecurityHandle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data
{
    public static class SeedData
    {
        public static void Initialize(this BackendContext context)
        {
            // Seeder for role
            if (!context.Role.Any())
            {
                context.AddRange(
                    new Role
                    {
                        Name = "Admin",
                        Description = "Set role for Admin User"
                    },
                    new Role
                    {
                        Name = "Manage",
                        Description = "Set role for Manage User"
                    },
                    new Role
                    {
                        Name = "Student",
                        Description = "Set role for Student User"
                    });
                context.SaveChanges();
            }

            // Seeder for account: 1 admin, 2 managers, 2 students
            if (!context.Account.Any())
            {
                var salt1 = PasswordHandle.GetInstance().GenerateSalt();
                var salt2 = PasswordHandle.GetInstance().GenerateSalt();
                var salt3 = PasswordHandle.GetInstance().GenerateSalt();
                var salt4 = PasswordHandle.GetInstance().GenerateSalt();
                var salt5 = PasswordHandle.GetInstance().GenerateSalt();
                context.AddRange(
                        new Account
                        {
                            Id = "ADMIN",
                            Username = "ADMIN",
                            Salt = salt1,
                            Password = PasswordHandle.GetInstance().EncryptPassword("Amin@123", salt1),
                            Email = "admin@admin.com",
                        },
                        new Account
                        {
                            Id = "MNG0001",
                            Username = "xuanhung24",
                            Salt = salt2,
                            Password = PasswordHandle.GetInstance().EncryptPassword("A123@a123", salt2),
                            Email = "xuanhung2401@gmail.com",
                        },
                        new Account
                        {
                            Id = "MNG0002",
                            Username = "hongluyen",
                            Salt = salt3,
                            Password = PasswordHandle.GetInstance().EncryptPassword("A123@a123", salt3),
                            Email = "hongluyen@gmail.com",
                        },
                        new Account
                        {
                            Id = "STU0001",
                            Username = "thao541998",
                            Salt = salt4,
                            Password = PasswordHandle.GetInstance().EncryptPassword("A123@a123", salt4),
                            Email = "thao541998@gmail.com",
                        },
                        new Account
                        {
                            Id = "STU0002",
                            Username = "phuonganh",
                            Salt = salt5,
                            Password = PasswordHandle.GetInstance().EncryptPassword("A123@a123", salt5),
                            Email = "phuonganh@gmail.com",
                        }
                    );
                context.SaveChanges();
            }

            // Seeder for general information
            if (!context.GeneralInformation.Any())
            {
                var salt = PasswordHandle.GetInstance().GenerateSalt();
                context.AddRange(
                        new GeneralInformation
                        {
                            AccountId = "ADMIN",
                            FirstName = "ADMIN",
                            LastName = "ADMIN",
                            Phone = "01234567890"
                        },
                        new GeneralInformation
                        {
                            AccountId = "MNG0001",
                            FirstName = "Hung",
                            LastName = "Dao",
                            Phone = "013237416",
                        },
                        new GeneralInformation
                        {
                            AccountId = "MNG0002",
                            FirstName = "Luyen",
                            LastName = "Dao",
                            Phone = "013257416",
                        },
                        new GeneralInformation
                        {
                            AccountId = "STU0001",
                            FirstName = "Thao",
                            LastName = "Nguyen",
                            Phone = "013257983",
                        },
                        new GeneralInformation
                        {
                            AccountId = "STU0002",
                            FirstName = "Anh",
                            LastName = "Nguyen",
                            Phone = "0130387983",
                        }
                    );
                context.SaveChanges();
            }

            // Seeder for account-role
            if (!context.AccountRoles.Any())
            {
                context.AddRange(
                        new AccountRole
                        {
                            AccountId = "Admin",
                            RoleId = 1,
                        },
                        new AccountRole
                        {
                            AccountId = "MNG0001",
                            RoleId = 2,
                        },
                        new AccountRole
                        {
                            AccountId = "MNG0002",
                            RoleId = 2,
                        },
                        new AccountRole()
                        {
                            AccountId = "STU0002",
                            RoleId = 3,
                        },
                        new AccountRole()
                        {
                            AccountId = "STU0001",
                            RoleId = 3,
                        }
                    );
                context.SaveChanges();
            }

            // Seeder for subject: 7 subjects
            if (!context.Subject.Any())
            {
                context.Subject.AddRange(
                        new Subject
                        {
                            Id = "WFP",
                            Name = "Windows Forms Programming",
                            Description = "Working with Windows Forms"
                        },
                        new Subject
                        {
                            Id = "WAD",
                            Name = "Web Application Development",
                            Description = "Develop web application"
                        },
                        new Subject
                        {
                            Id = "EAP",
                            Name = "Enterprise Application Programming",
                            Description = "Develop enterprise application"
                        },
                        new Subject
                        {
                            Id = "WCC",
                            Name = "Working with Cloud Computing",
                            Description = "Cloud Computing"
                        },
                        new Subject
                        {
                            Id = "MCC",
                            Name = "Mobile & Cloud Computing",
                            Description = "Working with Mobile & Cloud Computing"
                        },
                        new Subject
                        {
                            Id = "IEH",
                            Name = "Introduction to Ethical hacking",
                            Description = "Ethical hacking introduction"
                        },
                        new Subject
                        {
                            Id = "ICC",
                            Name = "Introduction to Cloud Computing",
                            Description = "Cloud computing introduction"
                        }
                    );
                context.SaveChanges();
            }

            // Seeder for class: 3 classes
            if (!context.Clazz.Any())
            {
                context.Clazz.AddRange(
                        new Clazz
                        {
                            Id = "T1707A",
                            StartDate = DateTime.Now,
                            Session = ClazzSession.Afternoon,
                            Status = ClazzStatus.Active,
                            CurrentSubjectId = "WAD"
                        },
                        new Clazz
                        {
                            Id = "T1707M",
                            StartDate = DateTime.Now,
                            Session = ClazzSession.Morning,
                            Status = ClazzStatus.Active,
                            CurrentSubjectId = "WFP"
                        },
                        new Clazz
                        {
                            Id = "T1707E",
                            StartDate = DateTime.Now,
                            Session = ClazzSession.Evening,
                            Status = ClazzStatus.Active,
                            CurrentSubjectId = "EAP"
                        }
                    );
                context.SaveChanges();
            }

            // Seeder for class-account: 3
            if (!context.ClazzAccount.Any())
            {
                context.ClazzAccount.AddRange(
                        new ClazzAccount
                        {
                            ClazzId = "T1707A",
                            AccountId = "STU0001"
                        },
                        new ClazzAccount
                        {
                            ClazzId = "T1707M",
                            AccountId = "STU0001"
                        },
                        new ClazzAccount
                        {
                            ClazzId = "T1707E",
                            AccountId = "STU0002"
                        }
                    );
                context.SaveChanges();
            }

            // Seeder for class-subject: 5
            if (!context.ClazzSubject.Any())
            {
                context.ClazzSubject.AddRange(
                        new ClazzSubject
                        {
                            ClazzId = "T1707A",
                            SubjectId = "WFP"
                        },
                        new ClazzSubject
                        {
                            ClazzId = "T1707A",
                            SubjectId = "WAD"
                        },
                        new ClazzSubject
                        {
                            ClazzId = "T1707M",
                            SubjectId = "WFP"
                        },
                        new ClazzSubject
                        {
                            ClazzId = "T1707E",
                            SubjectId = "EAP"
                        },
                        new ClazzSubject
                        {
                            ClazzId = "T1707E",
                            SubjectId = "WAD"
                        }
                    );
                context.SaveChanges();
            }

            // Seeder for mark (2 students)
            if (!context.Mark.Any())
            {
                context.Mark.AddRange(
                        new Mark
                        {
                            AccountId = "STU0001",
                            SubjectId = "WFP",
                            Value = 10,
                            MarkType = MarkType.Theory
                        },
                        new Mark
                        {
                            AccountId = "STU0002",
                            SubjectId = "WFP",
                            Value = 8,
                            MarkType = MarkType.Theory
                        },
                        new Mark
                        {
                            AccountId = "STU0001",
                            SubjectId = "WAD",
                            Value = 5,
                            MarkType = MarkType.Theory
                        },
                        new Mark
                        {
                            AccountId = "STU0001",
                            SubjectId = "WFP",
                            Value = 9,
                            MarkType = MarkType.Assignment
                        },
                        new Mark
                        {
                            AccountId = "STU0002",
                            SubjectId = "WFP",
                            Value = 7,
                            MarkType = MarkType.Assignment
                        },
                        new Mark
                        {
                            AccountId = "STU0001",
                            SubjectId = "WFP",
                            Value = 12,
                            MarkType = MarkType.Practice
                        },
                        new Mark
                        {
                            AccountId = "STU0002",
                            SubjectId = "WFP",
                            Value = 5,
                            MarkType = MarkType.Practice
                        }
                    );
                context.SaveChanges();
            }
        }
    }
}
