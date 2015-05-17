using System;
using System.Data.Entity;
using edziennik.Controllers;
using edziennik.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using Models.Interfaces;
using Repositories.Repositories;

namespace edziennik.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<ILogRepository, LogRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IStudentRepository, StudentRepository>(new PerRequestLifetimeManager());
            container.RegisterType<ITeacherRepository, TeacherRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IClasssRepository, ClasssRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IClassroomRepository, ClassroomRepository>(new PerRequestLifetimeManager());
            container.RegisterType<ISubjectRepository, SubjectRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IMarkRepository, MarkRepository>(new PerRequestLifetimeManager());
            //container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
           // container.RegisterType<UserManager<ApplicationUser>>();
            //container.RegisterType<DbContext, ApplicationDbContext>();
            //container.RegisterType<ApplicationUserManager>();
            //container.RegisterType<AccountController>(new InjectionConstructor());

            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<AccountController>(new InjectionConstructor());
        }
    }
}