using ServicesOfSchoolOfForeignLanguagesApp.AppData;

namespace ServicesOfSchoolOfForeignLanguagesApp.Classes
{
    class Manager
    {
        public static DemoTestBaseEntities Context = new DemoTestBaseEntities();
        public static string Title { get; set; }
        public static bool IsAdminMode;
    }
}
