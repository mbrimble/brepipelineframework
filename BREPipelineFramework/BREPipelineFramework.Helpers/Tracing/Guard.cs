//=================================================================================
// Inherited from Microsoft BizTalk CAT Team Best Practices Samples
//
// The Framework library is a set of general best practices for BizTalk developers.
//
//=================================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE. YOU BEAR THE RISK OF USING IT.
//=================================================================================

using System;
using System.Globalization;

namespace BREPipelineFramework.Helpers.Tracing
{
    /// <summary>
    /// Implements the common guard methods.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Checks a string argument to ensure it isn't null or empty.
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <returns>The return value should be ignored. It is intended to be used only when validating arguments during instance creation (e.g. when calling base constructor).</returns>
        public static bool ArgumentNotNullOrEmptyString(string argumentValue, string argumentName)
        {
            ArgumentNotNull(argumentValue, argumentName);

            if (argumentValue.Length == 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The specified string argument {0} must not be empty.", argumentName));
            }

            return true;
        }

        /// <summary>
        /// Checks an argument to ensure it isn't null.
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// /// <returns>The return value should be ignored. It is intended to be used only when validating arguments during instance creation (e.g. when calling base constructor).</returns>
        public static bool ArgumentNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }

            return true;
        }

        /// <summary>
        /// Checks an argument to ensure that the value is not the default value for its type.
        /// </summary>
        /// <typeparam name="T">The type of the agrument.</typeparam>
        /// <param name="argumentValue">The value of the argument.</param>
        /// <param name="argumentName">The name of the argument for diagnostic purposes.</param>
        public static void ArgumentNotDefaultValue<T>(T argumentValue, string argumentName)
        {
            if (!IsValueDefined<T>(argumentValue))
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The specified argument {0} cannot be initialised with its default value.", argumentName));
            }
        }

        /// <summary>
        /// Checks an Enum argument to ensure that its value is defined by the specified Enum type.
        /// </summary>
        /// <param name="enumType">The Enum type the value should correspond to.</param>
        /// <param name="value">The value to check for.</param>
        /// <param name="argumentName">The name of the argument holding the value.</param>
        public static void EnumValueIsDefined(Type enumType, object value, string argumentName)
        {
            if (Enum.IsDefined(enumType, value) == false)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The value of the argument {0} provided for the enumeration {1} is invalid.", argumentName, enumType.ToString()));
            }
        }

        /// <summary>
        /// Verifies that an argument type is assignable from the provided type (meaning
        /// interfaces are implemented, or classes exist in the base class hierarchy).
        /// </summary>
        /// <param name="assignee">The argument type.</param>
        /// <param name="providedType">The type it must be assignable from.</param>
        /// <param name="argumentName">The argument name.</param>
        public static void TypeIsAssignableFromType(Type assignee, Type providedType, string argumentName)
        {
            if (!providedType.IsAssignableFrom(assignee))
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The specified type {0} is not compatible with {1}.", assignee, providedType), argumentName);
            }
        }

        #region Private methods
        /// <summary>
        /// Checks the specified value to ensure that its value is defined, i.e. not null and not default value.
        /// </summary>
        /// <typeparam name="T">The type of the value to be checked.</typeparam>
        /// <param name="value">The value to be checked.</param>
        /// <returns>True if the value is defined or false if it's null or represents a default value for its type.</returns>
        private static bool IsValueDefined<T>(T value)
        {
            if (typeof(T).IsValueType)
            {
                return !value.Equals(default(T));
            }
            else
            {
                return value != null;
            }
        }
        #endregion
    }
}
