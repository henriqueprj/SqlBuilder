using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace rahhh.SqlBuilder
{
    /// <summary>
    /// Provides methods to protect against invalid parameters.
    /// </summary>
    [DebuggerStepThrough]
    public static class Guard
    {
        /// <summary>
        /// Verifies, that the method parameter with specified object value is not null
        /// and throws an exception if it is found to be so.
        /// </summary>
        /// <param name="target">The target object, which cannot be null.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <param name="message">The error message, if any to add to the exception.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target" /> is null</exception>
        public static void NotNull(object target, string parameterName, string message = "")
        {
            if (target != null)
                return;
            if (!string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(null, message);
            throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Verifies, that the string method parameter with specified object value and message
        /// is not null, not empty and does not contain only blanks and throws an exception
        /// if the object is null.
        /// </summary>
        /// <param name="target">The target string, which should be checked against being null or empty.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The error message, if any to add to the exception.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="target" /> is empty or contains only blanks.</exception>
        public static void NotNullOrEmpty(string target, string parameterName, string message = "")
        {
            NotNull((object) target, parameterName, message);
            if (!string.IsNullOrWhiteSpace(target))
                return;
            if (!string.IsNullOrWhiteSpace(message))
                throw new ArgumentException(message);
            throw new ArgumentException("O valor não pode ser nulo ou vazio e não pode conter espaços vazios.",
                parameterName);
        }

        /// <summary>
        /// Verifies, that the enumeration is not null and not empty.
        /// </summary>
        /// <typeparam name="T">The type of objects in the <paramref name="target" /></typeparam>
        /// <param name="target">The target enumeration, which should be checked against being null or empty.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The error message, if any to add to the exception.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="target" /> is empty.</exception>
        public static void NotNullOrEmpty<T>(IEnumerable<T> target, string parameterName, string message = "")
        {
            NotNull((object) target, parameterName, message);
            if (target.Any<T>())
                return;
            if (!string.IsNullOrWhiteSpace(message))
                throw new ArgumentException(message);
            throw new ArgumentException("Valor não pode ser nulo ou vazio.", parameterName);
        }
        
        /// <summary>
        /// Verifies, that the enumeration is not null and not empty.
        /// </summary>
        /// <typeparam name="T">The type of objects in the <paramref name="target" /></typeparam>
        /// <param name="target">The target enumeration, which should be checked against being null or empty.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The error message, if any to add to the exception.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="target" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="target" /> is empty.</exception>
        public static void NotNullOrEmpty<T>(T[] target, string parameterName, string message = "")
        {
            Guard.NotNull(target, parameterName, message);
            if (target.Length > 0)
                return;
            
            if (!string.IsNullOrWhiteSpace(message))
                throw new ArgumentException(message);
            
            throw new ArgumentException("Valor não pode ser nulo ou vazio.", parameterName);
        }

        /// <summary>
        /// Verifies that the specified value is less than a maximum value
        /// and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The target value, which should be validated.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <param name="message">The error message, if any to add to the exception.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="value" /> is greater than the maximum value.
        /// </exception>
        public static void MustBeLessThan<TValue>(TValue value, TValue max, string parameterName, string message = "")
            where TValue : IComparable<TValue>
        {
            if (value.CompareTo(max) >= 0)
            {
                if (!string.IsNullOrWhiteSpace(message))
                    throw new ArgumentOutOfRangeException(null, message);

                throw new ArgumentOutOfRangeException(parameterName, value,
                    $"O valor deve ser menor que {(object) max}.");
            }
        }

        /// <summary>
        /// Verifies that the specified value is less than or equal to a maximum value
        /// and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The target value, which should be validated.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <param name="message">The error message, if any to add to the exception.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="value" /> is greater than the maximum value.
        /// </exception>
        public static void MustBeLessThanOrEqualTo<TValue>(TValue value, TValue max, string parameterName,
            string message = "")
            where TValue : IComparable<TValue>
        {
            if (value.CompareTo(max) > 0)
            {
                if (!string.IsNullOrWhiteSpace(message))
                    throw new ArgumentOutOfRangeException(null, message);

                throw new ArgumentOutOfRangeException(parameterName, value,
                    $"O valor deve ser menor ou igual a {(object) max}.");
            }
        }

        /// <summary>
        /// Verifies that the specified value is greater than a minimum value
        /// and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The target value, which should be validated.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <param name="message">The error message, if any to add to the exception.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="value" /> is less than the minimum value.
        /// </exception>
        public static void MustBeGreaterThan<TValue>(TValue value, TValue min, string parameterName,
            string message = "")
            where TValue : IComparable<TValue>
        {
            if (value.CompareTo(min) <= 0)
            {
                if (!string.IsNullOrWhiteSpace(message))
                    throw new ArgumentOutOfRangeException(null, message);

                throw new ArgumentOutOfRangeException($"O valor deve ser maior que {(object) min}.");
            }
        }


        /// <summary>
        /// Verifies that the specified value is greater than or equal to a minimum value
        /// and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The target value, which should be validated.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <param name="message">The error message, if any to add to the exception.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="value" /> is less than the minimum value.
        /// </exception>
        public static void MustBeGreaterThanOrEqualTo<TValue>(TValue value, TValue min, string parameterName,
            string message = "")
            where TValue : IComparable<TValue>
        {
            if (value.CompareTo(min) < 0)
            {
                if (!string.IsNullOrWhiteSpace(message))
                    throw new ArgumentOutOfRangeException(null, message);

                throw new ArgumentOutOfRangeException(parameterName, value,
                    $"O valor deve ser maior ou igual a {(object) min}.");
            }
        }

        /// <summary>
        /// Verifies that the specified value is greater than or equal to a minimum value and less than
        /// or equal to a maximum value and throws an exception if it is not.
        /// </summary>
        /// <param name="value">The target value, which should be validated.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <param name="message">The error message, if any to add to the exception.</param>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="value" /> is less than the minimum value of greater than the maximum value.
        /// </exception>
        public static void MustBeBetweenOrEqualTo<TValue>(TValue value, TValue min, TValue max, string parameterName,
            string message = "")
            where TValue : IComparable<TValue>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            {
                if (!string.IsNullOrWhiteSpace(message))
                    throw new ArgumentOutOfRangeException(null, message);

                throw new ArgumentOutOfRangeException(parameterName, value,
                    $"O valor deve ser estar entre {(object) min} e {(object) max}.");
            }
        }

        /// <summary>
        /// Verifies, that the method parameter with specified target value is true
        /// and throws an exception if it is found to be so.
        /// </summary>
        /// <param name="target">The target value, which cannot be false.</param>
        /// <param name="parameterName">
        /// The name of the parameter that is to be checked.
        /// </param>
        /// <param name="message">
        /// The error message, if any to add to the exception.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="target" /> is false
        /// </exception>
        public static void IsTrue(bool target, string parameterName, string message)
        {
            if (!target)
                throw new ArgumentException(message, parameterName);
        }

        /// <summary>
        /// Verifies, that the method parameter with specified target value is false
        /// and throws an exception if it is found to be so.
        /// </summary>
        /// <param name="target">The target value, which cannot be true.</param>
        /// <param name="parameterName">The name of the parameter that is to be checked.</param>
        /// <param name="message">The error message, if any to add to the exception.</param>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="target" /> is true
        /// </exception>
        public static void IsFalse(bool target, string parameterName, string message)
        {
            if (target)
                throw new ArgumentException(message, parameterName);
        }

        /// <summary>
        /// Verifica se o valor informado é um enum valido referente ao tipo.
        /// </summary>
        /// <param name="enumType">Tipo do enum que sera verificado</param>
        /// <param name="value">Valor do enum</param>
        /// <param name="message">Menssagem de erro</param>
        /// <exception cref="ArgumentException"></exception>
        public static void IsEnumDefined(Type enumType, object value, string message)
        {
            if (!Enum.IsDefined(enumType, value)) throw new ArgumentException(message);    
        }
        
        /// <summary>
        /// Verifica se o valor informado é um enum valido referente ao tipo.
        /// </summary>
        /// <param name="value">Valor do enum</param>
        /// <param name="message">Menssagem de erro</param>
        /// <exception cref="ArgumentException"></exception>
        public static void IsEnumDefined<TEnum>(TEnum value, string message) where TEnum : Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), value)) throw new ArgumentException(message);    
        }

        public static void IsValidDate(DateTimeOffset date, string message)
        {
            if (!(date >= DateTimeOffset.MinValue && date <= DateTimeOffset.MaxValue))
                throw new ArgumentException(message);
        }
    }
}