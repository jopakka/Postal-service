using System;

namespace Exceptions
{
    public class InventorySizeTooSmallException : Exception
    {
        public override string Message => "Inventory size is too small";
    }
}