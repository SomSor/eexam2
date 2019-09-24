using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Resources
{
    public abstract class MVVMBase : INotifyPropertyChanged

    {
        #region Implematation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged(Expression<Func<object>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("propertyExpression");

            var propertyName = GetPropertyName(propertyExpression);
            var propertyValue = GetPropertyValue(propertyName);

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(GetPropertyName(propertyExpression)));
        }

        private string GetPropertyName(Expression<Func<object>> propertyExpression)
        {
            var unaryExpression = propertyExpression.Body as UnaryExpression;
            var memberExpression = unaryExpression == null ? (MemberExpression)propertyExpression.Body : (MemberExpression)unaryExpression.Operand;

            return memberExpression.Member.Name;
        }

        private object GetPropertyValue(string propertyName)
        {
            var type = this.GetType();
            var propertyInfo = type.GetProperty(propertyName);

            if (propertyInfo == null)
                throw new ArgumentException(string.Format("Couldn't find any property called {0} on type {1}", propertyName, type));

            return propertyInfo.GetValue(this, null);
        }

        #endregion
    }
}
