using Diabetes.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Diabetes.Repository.Data
{
    public static class StoreContextSeed
    {
        // Seeding
        public static async Task SeedAsync(StoreContext DbContext)
        {


            if (!DbContext.Admins.Any())
            {
                // Seeding Admins
                var AdminsData = File.ReadAllText("../Diabete.Repository/Data/DataSeed/Admins.json");
                var Admins = JsonSerializer.Deserialize<List<Admin>>(AdminsData);

                if (Admins?.Count > 0)
                {
                    foreach (var Admin in Admins)
                    {
                        await DbContext.Set<Admin>().AddAsync(Admin);
                    }
                    await DbContext.SaveChangesAsync();
                }
            }
            if (!DbContext.Managers.Any())
            {
                // Seeding Managers
                var ManagersData = File.ReadAllText("../Diabete.Repository/Data/DataSeed/Managers.json");
                var Managers = JsonSerializer.Deserialize<List<Manager>>(ManagersData);
                if (Managers?.Count > 0)
                {
                    foreach (var Manager in Managers)
                    {
                        await DbContext.Set<Manager>().AddAsync(Manager);
                    }
                    await DbContext.SaveChangesAsync();
                }
            }
            if (!DbContext.Organizations.Any())
            {
                //Seeding Organizations
                var OrganizationsData = File.ReadAllText("../Diabete.Repository/Data/DataSeed/Organizations.json");
                var Organizations = JsonSerializer.Deserialize<List<Organization>>(OrganizationsData);
                if (Organizations?.Count > 0)
                {
                    foreach (var Organization in Organizations)
                    {
                        await DbContext.Set<Organization>().AddAsync(Organization);
                    }
                    await DbContext.SaveChangesAsync();

                }
            }
            if (!DbContext.Doctors.Any())
            {
                // Seeding Doctors
                var DoctorsData = File.ReadAllText("../Diabete.Repository/Data/DataSeed/Doctors.json");
                var Doctors = JsonSerializer.Deserialize<List<Doctor>>(DoctorsData);

                if (Doctors?.Count > 0)
                {
                    foreach (var Doctor in Doctors)
                    {
                        await DbContext.Set<Doctor>().AddAsync(Doctor);
                    }
                    await DbContext.SaveChangesAsync();
                }
            }
            if (!DbContext.Clerks.Any())
            {
                // Seeding Clerks
                var ClerksData = File.ReadAllText("../Diabete.Repository/Data/DataSeed/Clerks.json");
                var Clerks = JsonSerializer.Deserialize<List<Clerk>>(ClerksData);

                if (Clerks?.Count > 0)
                {
                    foreach (var Clerk in Clerks)
                    {
                        await DbContext.Set<Clerk>().AddAsync(Clerk);
                    }

                    await DbContext.SaveChangesAsync();
                }
            }
            if (!DbContext.Patients.Any())
            {
                //Seeding Patients
                var PatientsData = File.ReadAllText("../Diabete.Repository/Data/DataSeed/Patients.json");
                var Patients = JsonSerializer.Deserialize<List<Patient>>(PatientsData);
                if (Patients?.Count > 0)
                {
                    foreach (var Patient in Patients)
                    {
                        await DbContext.Set<Patient>().AddAsync(Patient);
                    }
                    await DbContext.SaveChangesAsync();

                }
            }
            if (!DbContext.ChatbotQuestionDoctors.Any())
            {
                //Seeding ChatbotQuestionDoctors
                var ChatbotQuestionDoctorsData = File.ReadAllText("../Diabete.Repository/Data/DataSeed/ChatbotQuestionDoctors.json");
                var ChatbotQuestionDoctors = JsonSerializer.Deserialize<List<ChatbotQuestionDoctor>>(ChatbotQuestionDoctorsData);
                if (ChatbotQuestionDoctors?.Count > 0)
                {
                    foreach (var ChatbotQuestionDoctor in ChatbotQuestionDoctors)
                    {
                        await DbContext.Set<ChatbotQuestionDoctor>().AddAsync(ChatbotQuestionDoctor);
                    }
                    await DbContext.SaveChangesAsync();
                }
            }   

        }
    }
}