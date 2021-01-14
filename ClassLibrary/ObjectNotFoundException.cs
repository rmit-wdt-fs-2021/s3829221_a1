using System;

namespace ClassLibrary
{
    public class ObjectNotFoundException : Exception
    {

        public ObjectNotFoundException(string obj)
        {
            Console.WriteLine("This {0} cannot be found, please enter again.", obj);
            Console.WriteLine();
        }
    }
}
