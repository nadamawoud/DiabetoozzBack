using Diabetes.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Diabetes.Repository.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Fluent Api

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Admin> Admins { get; set; }//1
        public DbSet<Alarm> Alarms { get; set; }//2
       
        public DbSet<BloodSugar> BloodSugars { get; set; }//4
        public DbSet<ChatbotAnswerCasualUser> ChatbotAnswerCasualUsers { get; set; }//5
        public DbSet<ChatbotQuestionCasualUser> ChatbotQuestionCasualUsers { get; set; }//6
        public DbSet<ChatbotResultCasualUser> ChatbotResultCasualUsers { get; set; }//7
        public DbSet<ChatbotAnswerDoctor> ChatbotAnswerDoctors { get; set; }//8
        public DbSet<ChatbotQuestionDoctor> ChatbotQuestionDoctors { get; set; }//9
        public DbSet<CasualUser> CasualUsers { get; set; } //10
        public DbSet<Clerk> Clerks { get; set; }//11
        public DbSet<DiagnosisType> DiagnosisTypes { get; set; }//12
        public DbSet<Doctor> Doctors { get; set; } //13
        public DbSet<DoctorApproval> DoctorApprovals { get; set; } //14
        
        public DbSet<FoodItem> FoodItems { get; set; }  //16
        public DbSet<Manager> Managers { get; set; }    //17
        public DbSet<MedicalHistory> MedicalHistories { get; set; } //18
        public DbSet<Organization> Organizations { get; set; } //19
        public DbSet<Patient> Patients { get; set; } //20
        public DbSet<Post> Posts { get; set; } //21
        public DbSet<SuggestedFood> SuggestedFoods { get; set; } //22

    }
}





