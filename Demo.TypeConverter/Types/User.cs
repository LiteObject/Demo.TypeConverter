namespace Demo.TypeConverter.Types
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Original Source:
    /// https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.design.serialization.instancedescriptor?redirectedfrom=MSDN&view=net-5.0
    /// </summary>
    [TypeConverter(typeof(User.UserConverter))]
    public class User
    {
        public User(int id, string username, string email)
        {
            Id = id;
            Username = username;
            Email = email;
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// A TypeConverter for the User object. Note that you can make it internal,
        /// private, or any scope you want and the designers will still be able to use
        /// it through the TypeDescriptor object. 
        /// 
        /// This type converter provides the capability to convert to an InstanceDescriptor.
        /// This object can be used by the.NET Framework to generate source code that creates 
        /// an instance of a User object.
        /// </summary>
        internal class UserConverter : TypeConverter
        {
            /* This method overrides CanConvertTo from TypeConverter. This is called when someone
             * wants to convert an instance of User to another type.  
             * Here, only conversion to an InstanceDescriptor is supported. */
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(InstanceDescriptor))
                {
                    return true;
                }

                // Always call the base to see if it can perform the conversion.
                return base.CanConvertTo(context, destinationType);
            }

            // This code performs the actual conversion from a Triangle to an InstanceDescriptor.
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(InstanceDescriptor))
                {
                    ConstructorInfo ci = typeof(User).GetConstructor(new Type[] { typeof(int), typeof(string), typeof(string) });
                    User u = (User)value;

                    // Provides the information necessary to create an instance of an object. This class cannot be inherited.                     
                    return new InstanceDescriptor(ci, new object[] { u.Id, u.Username, u.Email });
                }

                // Always call base, even if you can't convert.
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
