using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;
using System.Data.Common;

namespace practice2
{
    // Факультет
    public class Department
    {
        [Key]
        public long department_id {get;set;}
        public string? name_department {get;set;}
        public virtual List<Study_program> programs {get;set;} = new();  // Список программ обучения на факультете
    }

    // Предмет
    public class Subject
    {
        [Key]
        public int subject_id {get;set;}
        public string? name_subject{get;set;}
        public virtual List<Enrollee_subject> enrollees {get;set;} = new();  // Список абитуриентов, сдавших этот предмет
    }

    // Абитуриент
    public class Enrollee
    {
        [Key]
        public int enrollee_id {get;set;}
        public string? name_enrollee {get;set;}
        public virtual List<Enrollee_subject> subjects {get;set;} = new();  // Список предметов, которые сдал абитуриент
        public virtual List<Enrollee_achievement> achievements {get;set;} = new();  // Список достижений абитуриента
    }

    // Достижение
    public class Achievement
    {
        [Key]
        public int achievement_id {get;set;}
        public string? name_achievement {get;set;}
        public int bonus {get;set;}
        public virtual List<Enrollee_achievement> enrollees {get;set;} = new();  // Список абитуриентов, получивших это достижение
    }

    // Программа обучения
    public class Study_program
    {
        [Key]
        public int program_id {get;set;}
        public string? name_program {get;set;}
        public virtual Department? department {get;set;}  // подключается таблица факультетов
        public int plan {get;set;}
        public virtual List<Program_enrollee> enrollees {get;set;} = new();  // Список абитуриентов, поступающих на программу обучения
        public virtual List<Program_subject> subjects {get;set;} = new();  // Список предметов, необходимых для поступления на программу
    }

    // Программа, выбранная абитуриентом
    public class Program_enrollee
    {
        [Key]
        public int program_enrollee_id {get;set;}
        public virtual Study_program? program {get;set;}
        public virtual Enrollee? enrollee {get;set;}
    }

    // Предмет, необходимый для поступления на программу
    public class Program_subject
    {
        [Key]
        public int program_subject_id {get;set;}
        public virtual Study_program? program {get;set;}
        public virtual Subject? subject {get;set;}
        public int min_result {get;set;}
    }

    // Предмет, который сдал абитуриент
    public class Enrollee_subject
    {
        [Key]
        public int enrollee_subject_id {get;set;}
        public virtual Enrollee? enrollee {get;set;}
        public virtual Subject? subject {get;set;}
        public int result {get;set;}
    }

    // Достижение, полученное абитуриентом
    public class Enrollee_achievement
    {
        [Key]
        public int enrollee_achievement_id {get;set;}
        public virtual Enrollee? enrollee {get;set;}
        public virtual Achievement? achievement {get;set;}
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<Department> department {get;set;} = null!;
        public DbSet<Subject> subject {get;set;} = null!;
        public DbSet<Enrollee> enrollee {get;set;} = null!;
        public DbSet<Achievement> achievement {get;set;} = null!;
        public DbSet<Study_program> program {get;set;} = null!;
        public DbSet<Program_enrollee> program_enrollee {get;set;} = null!;
        public DbSet<Program_subject> program_subject {get;set;} = null!;
        public DbSet<Enrollee_subject> enrollee_subject {get;set;} = null!;
        public DbSet<Enrollee_achievement> enrollee_achievement {get;set;} = null!;

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            .UseLazyLoadingProxies()
            .UseNpgsql("Host=localhost;Port=5432;Database=enrolling;Username=voldezhur;Password=291199");
            base.OnConfiguring(optionsBuilder);
        }
    }

    internal class Program
    {
        // Функция для заполнения базы данных информацией
        public static void fillDatabase()
        {
            using (ApplicationContext db = new ApplicationContext())
            {   
                Department departmentIIT = new Department {department_id = 1, name_department = "ИИТ"};
                Department departmentIII = new Department {department_id = 2, name_department = "ИИИ"};
                Department departmentIKB = new Department {department_id = 3, name_department = "ИКБ"};
                Department departmentIPTIP = new Department {department_id = 4, name_department = "ИПТИП"};
                Department departmentIRI = new Department {department_id = 5, name_department = "ИРИ"};
                Department departmentITU = new Department {department_id = 6, name_department = "ИТУ"};
                Department departmentITHT = new Department {department_id = 7, name_department = "ИТХТ"};

                db.department.AddRange(departmentIIT, departmentIIT, departmentIKB, departmentIPTIP, departmentIRI, departmentITU, departmentITHT);

                Subject subjectRussian = new Subject{subject_id = 1, name_subject = "Русский язык"};
                Subject subjectMaths = new Subject{subject_id = 2, name_subject = "Математика"};
                Subject subjectIT = new Subject{subject_id = 3, name_subject = "Информатика"};
                Subject subjectPhysics = new Subject{subject_id = 4, name_subject = "Физика"};
                Subject subjectChemistry = new Subject{subject_id = 5, name_subject = "Химия"};
                Subject subjectGeography = new Subject{subject_id = 6, name_subject = "География"};
                Subject subjectSocial = new Subject{subject_id = 7, name_subject = "Обществознание"};
                Subject subjectEnglish = new Subject{subject_id = 8, name_subject = "Английский язык"};
                Subject subjectLiterature = new Subject{subject_id = 9, name_subject = "Литература"};

                db.subject.AddRange(subjectRussian, subjectMaths, subjectIT, subjectPhysics, subjectChemistry, subjectGeography, subjectSocial, subjectEnglish, subjectLiterature);

                Enrollee enrollee1 = new Enrollee{enrollee_id = 1, name_enrollee = "Василий Василиевич Пупкин"};
                Enrollee enrollee2 = new Enrollee{enrollee_id = 2, name_enrollee = "Иван Иванович Иванов"};
                Enrollee enrollee3 = new Enrollee{enrollee_id = 3, name_enrollee = "Анатолий Анатолиевич Анатольев"};
                Enrollee enrollee4 = new Enrollee{enrollee_id = 4, name_enrollee = "Игорь Игоревич Игорев"};
                Enrollee enrollee5 = new Enrollee{enrollee_id = 5, name_enrollee = "Владимир Владимирович Владимиров"};
                Enrollee enrollee6 = new Enrollee{enrollee_id = 6, name_enrollee = "Андрей Андреевич Андреев"};
                Enrollee enrollee7 = new Enrollee{enrollee_id = 7, name_enrollee = "Николай Николаевич Николаев"};
                Enrollee enrollee8 = new Enrollee{enrollee_id = 8, name_enrollee = "Максим Максимович Максимов"};
                Enrollee enrollee9 = new Enrollee{enrollee_id = 9, name_enrollee = "Виктор Викторович Викторов"};
                Enrollee enrollee10 = new Enrollee{enrollee_id = 10, name_enrollee = "Доброжир"};
                Enrollee enrollee11 = new Enrollee{enrollee_id = 11, name_enrollee = "Гостомысл"};
                Enrollee enrollee12 = new Enrollee{enrollee_id = 12, name_enrollee = "Миролюб"};
                Enrollee enrollee13 = new Enrollee{enrollee_id = 13, name_enrollee = "Велимудр"};
                Enrollee enrollee14 = new Enrollee{enrollee_id = 14, name_enrollee = "Светозар"};
                Enrollee enrollee15 = new Enrollee{enrollee_id = 15, name_enrollee = "Милонег"};

                db.enrollee.AddRange(enrollee1, enrollee2, enrollee3);

                Achievement achievementGTO = new Achievement{achievement_id = 1, name_achievement = "ГТО", bonus = 5};
                Achievement achievementOlympiad = new Achievement{achievement_id = 2, name_achievement = "Олимпиада", bonus = 5};
                Achievement achievementHackathon = new Achievement{achievement_id = 3, name_achievement = "Хакатон", bonus = 5};

                db.achievement.AddRange(achievementGTO, achievementOlympiad, achievementHackathon);

                Study_program study_program1 = new Study_program{program_id = 1, name_program = "Прикладная математика", department = departmentIIT, plan = 5};
                Study_program study_program2 = new Study_program{program_id = 2, name_program = "Информатика и вычислительная техника", department = departmentIIT, plan = 5};
                Study_program study_program3 = new Study_program{program_id = 3, name_program = "Прикладная информатика", department = departmentIRI, plan = 5};
                Study_program study_program4 = new Study_program{program_id = 4, name_program = "Радиотехника", department = departmentIRI, plan = 7};
                Study_program study_program5 = new Study_program{program_id = 5, name_program = "Фундаментальная информатика и информационные технологии", department = departmentIKB, plan = 5};
                Study_program study_program6 = new Study_program{program_id = 6, name_program = "Информационная безопасность", department = departmentIKB, plan = 5};
                Study_program study_program7 = new Study_program{program_id = 7, name_program = "Статистика", department = departmentITU, plan = 5};
                Study_program study_program8 = new Study_program{program_id = 8, name_program = "Бизнес-информатика", department = departmentITU, plan = 8};
                Study_program study_program9 = new Study_program{program_id = 9, name_program = "Физика", department = departmentIII, plan = 5};
                Study_program study_program10 = new Study_program{program_id = 10, name_program = "Мехатроника и робототехника", department = departmentIII, plan = 5};
                Study_program study_program11 = new Study_program{program_id = 11, name_program = "Химия", department = departmentITHT, plan = 10};
                Study_program study_program12 = new Study_program{program_id = 12, name_program = "Биотехнология", department = departmentITHT, plan = 5};
                Study_program study_program13 = new Study_program{program_id = 13, name_program = "Информационные системы и технологии", department = departmentIPTIP, plan = 5};
                Study_program study_program14 = new Study_program{program_id = 14, name_program = "Инноватика", department = departmentIPTIP, plan = 5};

                db.program.AddRange(study_program1, study_program2, study_program3, study_program4, study_program5, study_program6, study_program7, study_program8, study_program9, study_program10, study_program11, study_program12, study_program13, study_program14);

                Program_enrollee pg1 = new Program_enrollee{program_enrollee_id = 1, program = study_program1, enrollee = enrollee1};
                Program_enrollee pg2 = new Program_enrollee{program_enrollee_id = 2, program = study_program3, enrollee = enrollee2};
                Program_enrollee pg3 = new Program_enrollee{program_enrollee_id = 3, program = study_program3, enrollee = enrollee3};
                Program_enrollee pg4 = new Program_enrollee{program_enrollee_id = 4, program = study_program2, enrollee = enrollee4};
                Program_enrollee pg5 = new Program_enrollee{program_enrollee_id = 5, program = study_program2, enrollee = enrollee5};
                Program_enrollee pg6 = new Program_enrollee{program_enrollee_id = 6, program = study_program14, enrollee = enrollee6};
                Program_enrollee pg7 = new Program_enrollee{program_enrollee_id = 7, program = study_program14, enrollee = enrollee7};
                Program_enrollee pg8 = new Program_enrollee{program_enrollee_id = 8, program = study_program14, enrollee = enrollee8};
                Program_enrollee pg9 = new Program_enrollee{program_enrollee_id = 9, program = study_program12, enrollee = enrollee9};
                Program_enrollee pg10 = new Program_enrollee{program_enrollee_id = 10, program = study_program1, enrollee = enrollee10};
                Program_enrollee pg11 = new Program_enrollee{program_enrollee_id = 11, program = study_program1, enrollee = enrollee11};
                Program_enrollee pg12 = new Program_enrollee{program_enrollee_id = 12, program = study_program7, enrollee = enrollee12};
                Program_enrollee pg13 = new Program_enrollee{program_enrollee_id = 13, program = study_program7, enrollee = enrollee13};
                Program_enrollee pg14 = new Program_enrollee{program_enrollee_id = 14, program = study_program6, enrollee = enrollee14};
                Program_enrollee pg15 = new Program_enrollee{program_enrollee_id = 15, program = study_program5, enrollee = enrollee15};

                db.program_enrollee.AddRange(pg1, pg3, pg4, pg5, pg6, pg7, pg8, pg9, pg10, pg11, pg12, pg13, pg14, pg15);

                Program_subject ps1 = new Program_subject{program_subject_id=1, program=study_program1, subject=subjectEnglish, min_result=70};
                Program_subject ps2 = new Program_subject{program_subject_id=2, program=study_program1, subject=subjectMaths, min_result=75};
                Program_subject ps3 = new Program_subject{program_subject_id=3, program=study_program1, subject=subjectPhysics, min_result=80};
                Program_subject ps4 = new Program_subject{program_subject_id=4, program=study_program2, subject=subjectSocial, min_result=65};
                Program_subject ps5 = new Program_subject{program_subject_id=5, program=study_program2, subject=subjectGeography, min_result=70};
                Program_subject ps6 = new Program_subject{program_subject_id=6, program=study_program2, subject=subjectEnglish, min_result=75};
                Program_subject ps7 = new Program_subject{program_subject_id=7, program=study_program3, subject=subjectPhysics, min_result=75};
                Program_subject ps8 = new Program_subject{program_subject_id=8, program=study_program3, subject=subjectChemistry, min_result=80};
                Program_subject ps9 = new Program_subject{program_subject_id=9, program=study_program3, subject=subjectMaths, min_result=70};
                Program_subject ps10 = new Program_subject{program_subject_id=10, program=study_program4, subject=subjectIT, min_result=85};
                Program_subject ps11 = new Program_subject{program_subject_id=11, program=study_program4, subject=subjectMaths, min_result=80};
                Program_subject ps12 = new Program_subject{program_subject_id=12, program=study_program4, subject=subjectPhysics, min_result=75};
                Program_subject ps13 = new Program_subject{program_subject_id=13, program=study_program5, subject=subjectChemistry, min_result=80};
                Program_subject ps14 = new Program_subject{program_subject_id=14, program=study_program5, subject=subjectGeography, min_result=70};
                Program_subject ps15 = new Program_subject{program_subject_id=15, program=study_program6, subject=subjectMaths, min_result=75};
                Program_subject ps16 = new Program_subject{program_subject_id=16, program=study_program6, subject=subjectGeography, min_result=80};
                Program_subject ps17 = new Program_subject{program_subject_id=17, program=study_program6, subject=subjectPhysics, min_result=85};
                Program_subject ps18 = new Program_subject{program_subject_id=18, program=study_program7, subject=subjectEnglish, min_result=70};
                Program_subject ps19 = new Program_subject{program_subject_id=19, program=study_program7, subject=subjectRussian, min_result=75};
                Program_subject ps20 = new Program_subject{program_subject_id=20, program=study_program7, subject=subjectSocial, min_result=80};
                Program_subject ps21 = new Program_subject{program_subject_id=21, program=study_program8, subject=subjectMaths, min_result=85};
                Program_subject ps22 = new Program_subject{program_subject_id=22, program=study_program8, subject=subjectGeography, min_result=70};
                Program_subject ps23 = new Program_subject{program_subject_id=23, program=study_program8, subject=subjectPhysics, min_result=75};
                Program_subject ps24 = new Program_subject{program_subject_id=24, program=study_program9, subject=subjectChemistry, min_result=80};
                Program_subject ps25 = new Program_subject{program_subject_id=25, program=study_program9, subject=subjectIT, min_result=85};
                Program_subject ps26 = new Program_subject{program_subject_id=26, program=study_program9, subject=subjectLiterature, min_result=90};
                Program_subject ps27 = new Program_subject{program_subject_id=27, program=study_program10, subject=subjectLiterature, min_result=70};
                Program_subject ps28 = new Program_subject{program_subject_id=28, program=study_program10, subject=subjectChemistry, min_result=75};
                Program_subject ps29 = new Program_subject{program_subject_id=29, program=study_program10, subject=subjectMaths, min_result=80};
                Program_subject ps30 = new Program_subject{program_subject_id=30, program=study_program11, subject=subjectPhysics, min_result=85};
                Program_subject ps31 = new Program_subject{program_subject_id=31, program=study_program11, subject=subjectIT, min_result=70};
                Program_subject ps32 = new Program_subject{program_subject_id=32, program=study_program11, subject=subjectMaths, min_result=75};
                Program_subject ps33 = new Program_subject{program_subject_id=33, program=study_program12, subject=subjectSocial, min_result=80};
                Program_subject ps34 = new Program_subject{program_subject_id=34, program=study_program12, subject=subjectEnglish, min_result=85};
                Program_subject ps35 = new Program_subject{program_subject_id=35, program=study_program12, subject=subjectRussian, min_result=90};
                Program_subject ps36 = new Program_subject{program_subject_id=36, program=study_program13, subject=subjectMaths, min_result=70};
                Program_subject ps37 = new Program_subject{program_subject_id=37, program=study_program13, subject=subjectGeography, min_result=75};
                Program_subject ps38 = new Program_subject{program_subject_id=38, program=study_program13, subject=subjectPhysics, min_result=80};
                Program_subject ps39 = new Program_subject{program_subject_id=39, program=study_program14, subject=subjectChemistry, min_result=85};
                Program_subject ps40 = new Program_subject{program_subject_id=40, program=study_program14, subject=subjectPhysics, min_result=70};
                Program_subject ps41 = new Program_subject{program_subject_id=41, program=study_program14, subject=subjectMaths, min_result=75};
                Program_subject ps42 = new Program_subject{program_subject_id=42, program=study_program14, subject=subjectIT, min_result=80};
                
                db.program_subject.AddRange(ps1, ps2, ps3, ps4, ps5, ps6, ps7, ps8, ps9, ps10, ps11, ps12, ps13, ps14, ps15, ps16, ps17, ps18, ps19, ps20, ps21, ps22, ps23, ps24, ps25, ps26, ps27, ps28, ps29, ps30, ps31, ps32, ps33, ps34, ps35, ps36, ps37, ps38, ps39, ps40, ps41, ps42);

                Enrollee_subject es1 = new Enrollee_subject{enrollee_subject_id = 1, enrollee = enrollee1, subject = subjectRussian, result = 70};
                Enrollee_subject es2 = new Enrollee_subject{enrollee_subject_id = 2, enrollee = enrollee1, subject = subjectIT, result = 85};
                Enrollee_subject es3 = new Enrollee_subject{enrollee_subject_id = 3, enrollee = enrollee1, subject = subjectPhysics, result = 79};
                Enrollee_subject es4 = new Enrollee_subject{enrollee_subject_id = 4, enrollee = enrollee2, subject = subjectEnglish, result = 88};
                Enrollee_subject es5 = new Enrollee_subject{enrollee_subject_id = 5, enrollee = enrollee2, subject = subjectLiterature, result = 82};
                Enrollee_subject es6 = new Enrollee_subject{enrollee_subject_id = 6, enrollee = enrollee2, subject = subjectMaths, result = 91};
                Enrollee_subject es7 = new Enrollee_subject{enrollee_subject_id = 7, enrollee = enrollee3, subject = subjectChemistry, result = 76};
                Enrollee_subject es8 = new Enrollee_subject{enrollee_subject_id = 8, enrollee = enrollee3, subject = subjectGeography, result = 83};
                Enrollee_subject es9 = new Enrollee_subject{enrollee_subject_id = 9, enrollee = enrollee3, subject = subjectSocial, result = 78};
                Enrollee_subject es10 = new Enrollee_subject{enrollee_subject_id = 10, enrollee = enrollee4, subject = subjectPhysics, result = 89};
                Enrollee_subject es11 = new Enrollee_subject{enrollee_subject_id = 11, enrollee = enrollee4, subject = subjectRussian, result = 77};
                Enrollee_subject es12 = new Enrollee_subject{enrollee_subject_id = 12, enrollee = enrollee4, subject = subjectMaths, result = 82};
                Enrollee_subject es13 = new Enrollee_subject{enrollee_subject_id = 13, enrollee = enrollee5, subject = subjectChemistry, result = 85};
                Enrollee_subject es14 = new Enrollee_subject{enrollee_subject_id = 14, enrollee = enrollee5, subject = subjectEnglish, result = 80};
                Enrollee_subject es15 = new Enrollee_subject{enrollee_subject_id = 15, enrollee = enrollee5, subject = subjectSocial, result = 75};
                Enrollee_subject es16 = new Enrollee_subject{enrollee_subject_id = 16, enrollee = enrollee6, subject = subjectIT, result = 90};
                Enrollee_subject es17 = new Enrollee_subject{enrollee_subject_id = 17, enrollee = enrollee6, subject = subjectGeography, result = 85};
                Enrollee_subject es18 = new Enrollee_subject{enrollee_subject_id = 18, enrollee = enrollee6, subject = subjectPhysics, result = 87};
                Enrollee_subject es19 = new Enrollee_subject{enrollee_subject_id = 19, enrollee = enrollee7, subject = subjectEnglish, result = 92};
                Enrollee_subject es20 = new Enrollee_subject{enrollee_subject_id = 20, enrollee = enrollee7, subject = subjectMaths, result = 94};
                Enrollee_subject es21 = new Enrollee_subject{enrollee_subject_id = 21, enrollee = enrollee7, subject = subjectIT, result = 89};
                Enrollee_subject es22 = new Enrollee_subject{enrollee_subject_id = 22, enrollee = enrollee8, subject = subjectRussian, result = 79};
                Enrollee_subject es23 = new Enrollee_subject{enrollee_subject_id = 23, enrollee = enrollee8, subject = subjectSocial, result = 71};
                Enrollee_subject es24 = new Enrollee_subject{enrollee_subject_id = 24, enrollee = enrollee8, subject = subjectChemistry, result = 83};
                Enrollee_subject es25 = new Enrollee_subject{enrollee_subject_id = 25, enrollee = enrollee9, subject = subjectPhysics, result = 88};
                Enrollee_subject es26 = new Enrollee_subject{enrollee_subject_id = 26, enrollee = enrollee9, subject = subjectIT, result = 85};
                Enrollee_subject es27 = new Enrollee_subject{enrollee_subject_id = 27, enrollee = enrollee9, subject = subjectEnglish, result = 90};
                Enrollee_subject es28 = new Enrollee_subject{enrollee_subject_id = 28, enrollee = enrollee10, subject = subjectMaths, result = 93};
                Enrollee_subject es29 = new Enrollee_subject{enrollee_subject_id = 29, enrollee = enrollee10, subject = subjectLiterature, result = 86};
                Enrollee_subject es30 = new Enrollee_subject{enrollee_subject_id = 30, enrollee = enrollee10, subject = subjectGeography, result = 81};
                Enrollee_subject es31 = new Enrollee_subject{enrollee_subject_id = 31, enrollee = enrollee11, subject = subjectSocial, result = 75};
                Enrollee_subject es32 = new Enrollee_subject{enrollee_subject_id = 32, enrollee = enrollee11, subject = subjectChemistry, result = 79};
                Enrollee_subject es33 = new Enrollee_subject{enrollee_subject_id = 33, enrollee = enrollee11, subject = subjectRussian, result = 72};
                Enrollee_subject es34 = new Enrollee_subject{enrollee_subject_id = 34, enrollee = enrollee12, subject = subjectIT, result = 87};
                Enrollee_subject es35 = new Enrollee_subject{enrollee_subject_id = 35, enrollee = enrollee12, subject = subjectPhysics, result = 83};
                Enrollee_subject es36 = new Enrollee_subject{enrollee_subject_id = 36, enrollee = enrollee12, subject = subjectEnglish, result = 88};
                Enrollee_subject es37 = new Enrollee_subject{enrollee_subject_id = 37, enrollee = enrollee13, subject = subjectMaths, result = 92};
                Enrollee_subject es38 = new Enrollee_subject{enrollee_subject_id = 38, enrollee = enrollee13, subject = subjectGeography, result = 86};
                Enrollee_subject es39 = new Enrollee_subject{enrollee_subject_id = 39, enrollee = enrollee13, subject = subjectSocial, result = 80};
                Enrollee_subject es40 = new Enrollee_subject{enrollee_subject_id = 40, enrollee = enrollee14, subject = subjectPhysics, result = 85};
                Enrollee_subject es41 = new Enrollee_subject{enrollee_subject_id = 41, enrollee = enrollee14, subject = subjectEnglish, result = 89};
                Enrollee_subject es42 = new Enrollee_subject{enrollee_subject_id = 42, enrollee = enrollee14, subject = subjectChemistry, result = 82};
                Enrollee_subject es43 = new Enrollee_subject{enrollee_subject_id = 43, enrollee = enrollee15, subject = subjectIT, result = 91};
                Enrollee_subject es44 = new Enrollee_subject{enrollee_subject_id = 44, enrollee = enrollee15, subject = subjectGeography, result = 87};
                Enrollee_subject es45 = new Enrollee_subject{enrollee_subject_id = 45, enrollee = enrollee15, subject = subjectPhysics, result = 90};

                db.enrollee_subject.AddRange(es1, es2, es3, es4, es5, es6, es7, es8, es9, es10, es11, es12, es13, es14, es15, es16, es17, es18, es19, es20, es21, es22, es23, es24, es25, es26, es27, es28, es29, es30, es31, es32, es33, es34, es35, es36, es37, es38, es39, es40, es41, es42, es43, es44, es45);

                Enrollee_achievement ea1 = new Enrollee_achievement{enrollee_achievement_id = 1, achievement = achievementGTO, enrollee = enrollee1};
                Enrollee_achievement ea2 = new Enrollee_achievement{enrollee_achievement_id = 2, achievement = achievementGTO, enrollee = enrollee2};
                Enrollee_achievement ea3 = new Enrollee_achievement{enrollee_achievement_id = 3, achievement = achievementOlympiad, enrollee = enrollee3};
                Enrollee_achievement ea4 = new Enrollee_achievement{enrollee_achievement_id = 4, achievement = achievementOlympiad, enrollee = enrollee4};
                Enrollee_achievement ea5 = new Enrollee_achievement{enrollee_achievement_id = 5, achievement = achievementOlympiad, enrollee = enrollee5};
                Enrollee_achievement ea6 = new Enrollee_achievement{enrollee_achievement_id = 6, achievement = achievementHackathon, enrollee = enrollee6};

                db.enrollee_achievement.AddRange(ea1, ea2, ea3, ea4, ea5, ea6);

                try 
                {
                    db.SaveChanges();
                }

                catch (Exception ex)
                {
                    Console.Write("\n\n\n========\nПроизошла ошибка при попытке записи в базу данных\nТекст ошибки:\n");
                    Console.Write(ex);
                    Console.Write("\n========\n\n\n");
                }
            }
        }

        // Запрос 1
        public static void query1(string study_program_name = "Информационные системы и технологии")
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Абитуриенты, желающие поступить на программу обучения {study_program_name}:\n");

                var enrolleesInStudyProgram = from pe in db.program_enrollee.ToList()
                    where pe.program?.name_program == study_program_name
                    select pe.enrollee?.name_enrollee;

                foreach (var item in enrolleesInStudyProgram)
                {
                    Console.WriteLine(item);
                }
            }
        }

        // Запрос 2
        public static void query2(string required_subject = "Русский язык")
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Программы обучения, для которых необходим предмет {required_subject}:\n");

                var selected_study_programs = from sp in db.program.ToList()
                    where sp.subjects.Any(item => item.subject?.name_subject == required_subject)
                    select sp.program_id;

                foreach (var item in selected_study_programs)
                {
                    Console.WriteLine(item);
                }
            }
        }

        // Запрос 3
        public static void query3()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Статистика каждого предмета ЕГЭ:\n");
                
                var subject_statistics = from s in db.subject.ToList()
                    select new {s.name_subject, s.enrollees.Count, max_result = s.enrollees.Max(x => x.result), min_result = s.enrollees.Min(x => x.result)};

                foreach (var item in subject_statistics)
                {
                    Console.WriteLine($"Название предмета: {item.name_subject};\nМинимальный результат: {item.min_result}\nМаксимальный результат: {item.max_result};\nКоличество сдающих: {item.Count};\n");
                }
            }
        }

        // Запрос 4
        public static void query4(int min = 70)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Программы обучения, у которых минимальные баллы всех необходимых предметов больше {min}:\n");

                var program_statistics = from p in db.program.ToList()
                    where p.subjects.All(x => x.min_result > 70)
                    select new {p.name_program, p.subjects};

                foreach (var item in program_statistics)
                {
                    Console.WriteLine(item.name_program);
                }
            }
        }

        // Запрос 5
        public static void query5()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Программа обучения с самым большим планом набора:\n");

                var max_plan_program = db.program.OrderByDescending(x => x.plan).FirstOrDefault();

                Console.WriteLine(max_plan_program!.name_program);
            }
        }

        // Запрос 6
        public static void query6()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Дополнительные баллы для каждого абитуриента:\n");

                var additional_scores = from e in db.enrollee.ToList()
                    select new {e.name_enrollee, additional_score = e.achievements.Sum(x => x.achievement!.bonus)};

                foreach (var item in additional_scores)
                {
                    Console.WriteLine($"Абитуриент: {item.name_enrollee}\t Дополнительные баллы: {item.additional_score}");
                }
            }            
        }

        // Запрос 7
        public static void query7()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Конкурс для каждой образовательной программы:\n");

                var program_comp = from p in db.program.ToList()
                    select new {p.name_program, p.enrollees};

                foreach (var item in program_comp)
                {
                    Console.WriteLine($"Программа обучения: {item.name_program}");
                    
                    foreach (var enrollee in item.enrollees)
                    {
                        Console.WriteLine($"{enrollee.enrollee!.name_enrollee}: {enrollee.enrollee!.achievements.Sum(x => x.achievement!.bonus) + enrollee.enrollee!.subjects.Sum(x => x.result)}");
                    }

                    Console.Write('\n');
                }
            }            
        }

        // Запрос 8
        public static void query8(string subject1 = "Английский язык", string subject2 = "Математика")
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine($"Образовательные программы, для поступления на которые требуются следующие предметы: {subject1}, {subject2}:\n");

                var program_statistics = from p in db.program.ToList()
                    where p.subjects.Any(x => x.subject!.name_subject == subject1) && p.subjects.Any(x => x.subject!.name_subject == subject2)
                    select new {p.name_program, p.subjects};

                foreach (var item in program_statistics)
                {
                    Console.WriteLine($"Название программы: {item.name_program};\nСписок предметов:");

                    foreach (var subject in item.subjects)
                    {
                        Console.WriteLine(subject.subject!.name_subject);
                    }
                }
            }
        }

        // Запрос 9
        public static void query9()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine("Количество баллов каждого студента для каждой учебной программы");

                var student_scores = 
                    from s in db.enrollee.ToList()
                    from p in db.program.ToList()
                    select new {s.name_enrollee, p.name_program, enrollee_score = s.subjects.Where(x => p.subjects.Any(y => x.subject!.subject_id == y.subject!.subject_id)).Sum(x => x.result)};

                foreach (var item in student_scores)
                {
                    Console.WriteLine($"{item.name_enrollee}, {item.name_program}, {item.enrollee_score}");
                }
            }
        }

        // Запрос 10
        public static void query10()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine("Студенты, которые не могут быть зачисленны на выбранную образовательную программу");

                var bad_enrollees = 
                    from e in db.program_enrollee.ToList()
                    where e.enrollee!.subjects.Where(x => e.program!.subjects.Any(y => x.subject!.subject_id == y.subject!.subject_id)).Sum(x => x.result) < e.program!.subjects.Sum(x => x.min_result)
                    select new {e.enrollee!.name_enrollee};

                foreach (var item in bad_enrollees)
                {
                    Console.WriteLine(item.name_enrollee);
                }
            }
        }

        static void Main(string[] args)
        {
            // Инициализация БД
            // fillDatabase();

            // Запросы
            // Аномалии при выводе данных могут возникнуть из-за не совсем корректных данных в БД
            // query1();
            // query2();
            // query3();
            // query4();
            // query5();
            // query6();
            // query7();
            // query8();
            // query9();
            // query10();
        }
    }
}