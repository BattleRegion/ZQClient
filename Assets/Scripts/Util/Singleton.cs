public abstract class Singleton<T> where T: class, new()
{
     private static T _singleton;

     private static object Locker = new object();
     
     public static T GetInstance()
     {
          if (_singleton == null)
          {
               lock (Locker)
               {
                    _singleton = new T();
               }
          }
          return _singleton;
     }
}
