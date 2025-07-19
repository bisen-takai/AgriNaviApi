using System;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Xunit;
using AgriNaviApi.Shared.Utilities;
using System.Reflection;

namespace AgriNaviApi.Shared.Tests.Utilities
{
    public class DbExceptionHelperTests
    {
        private MySqlException CreateMySqlException(int number)
        {
            // MySqlException の Number プロパティは読み取り専用なので、リフレクションで設定
            var ctor = typeof(MySqlException).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { typeof(string), typeof(int), typeof(string), typeof(string), typeof(Exception) },
                null);

            return (MySqlException)ctor.Invoke(new object[] { "error", number, null, null, null });
        }

        /// <summary>
        /// MySQLのユニーク制約違反（エラーコード1062）の場合にtrueを返すことを検証
        /// </summary>
        [Fact]
        public void IsUniqueConstraintViolation_ReturnsTrue_WhenMySqlUniqueConstraintError()
        {
            var mySqlException = CreateMySqlException(1062);
            var dbUpdateException = new DbUpdateException("Error", mySqlException);

            var result = dbUpdateException.IsUniqueConstraintViolation();

            Assert.True(result);
        }

        /// <summary>
        /// MySQLのその他のエラーコードの場合にfalseを返すことを検証
        /// </summary>
        [Fact]
        public void IsUniqueConstraintViolation_ReturnsFalse_WhenMySqlOtherError()
        {
            var mySqlException = CreateMySqlException(1048);
            var dbUpdateException = new DbUpdateException("Error", mySqlException);

            var result = dbUpdateException.IsUniqueConstraintViolation();

            Assert.False(result);
        }

        /// <summary>
        /// InnerExceptionがnullの場合にfalseを返すことを検証
        /// </summary>
        [Fact]
        public void IsUniqueConstraintViolation_ReturnsFalse_WhenInnerExceptionIsNull()
        {
            var dbUpdateException = new DbUpdateException("Error", (Exception)null);

            var result = dbUpdateException.IsUniqueConstraintViolation();

            Assert.False(result);
        }

        /// <summary>
        /// DbUpdateException自体がnullの場合にfalseを返すことを検証
        /// </summary>
        [Fact]
        public void IsUniqueConstraintViolation_ReturnsFalse_WhenExceptionIsNull()
        {
            DbUpdateException? dbUpdateException = null;

            var result = dbUpdateException?.IsUniqueConstraintViolation() ?? false;

            Assert.False(result);
        }

        /// <summary>
        /// InnerExceptionがMySqlException以外の場合にfalseを返すことを検証
        /// </summary>
        [Fact]
        public void IsUniqueConstraintViolation_ReturnsFalse_WhenInnerExceptionIsNotMySqlException()
        {
            var innerException = new InvalidOperationException("Some error");
            var dbUpdateException = new DbUpdateException("Error", innerException);

            var result = dbUpdateException.IsUniqueConstraintViolation();

            Assert.False(result);
        }
    }
}
