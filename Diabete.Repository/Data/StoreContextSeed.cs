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
        public static async Task SeedAsnc(StoreContext DbContext)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (!DbContext.Admins.Any())
            {
                // Seeding Admins
                var AdminsData = File.ReadAllText("../Diabete.Repository/Data/DataSeeding/Admins.json");
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
            if (!DbContext.Organizations.Any())
            {
                //Seeding Organizations
                var OrganizationsData = File.ReadAllText("../Diabete.Repository/Data/DataSeeding/Organizations.json");
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
            if (!DbContext.MedicalSyndicates.Any())
            {
                //Seeding MedicalSyndicate
                var MedicalSyndicatesData = File.ReadAllText("../Diabete.Repository/Data/DataSeeding/MedicalSyndicates.json");
                var MedicalSyndicates = JsonSerializer.Deserialize<List<MedicalSyndicate>>(MedicalSyndicatesData);
                if (MedicalSyndicates?.Count > 0)
                {
                    foreach (var MedicalSyndicate in MedicalSyndicates)
                    {
                        await DbContext.Set<MedicalSyndicate>().AddAsync(MedicalSyndicate);
                    }
                    await DbContext.SaveChangesAsync();
                }

            }
            if (!DbContext.NewsFeedPosts.Any())
            {
                //Seeding NewsFeedPosts
                var NewsFeedPostsData = File.ReadAllText("../Diabete.Repository/Data/DataSeeding/NewsFeedPosts.json");
                var NewsFeedPosts = JsonSerializer.Deserialize<List<NewsFeedPost>>(NewsFeedPostsData);
                if (NewsFeedPosts?.Count > 0)
                {
                    foreach (var NewsFeedPost in NewsFeedPosts)
                    {
                        await DbContext.Set<NewsFeedPost>().AddAsync(NewsFeedPost);
                    }
                    await DbContext.SaveChangesAsync();
                }

            }
            if (!DbContext.Doctors.Any())
            {
                // Seeding Doctors
                var DoctorsData = File.ReadAllText("../Diabete.Repository/Data/DataSeeding/Doctors.json");
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
                var ClerksData = File.ReadAllText("../Diabete.Repository/Data/DataSeeding/Clerks.json");
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
                var PatientsData = File.ReadAllText("../Diabete.Repository/Data/DataSeeding/Patient.json");
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
        }
    }
}