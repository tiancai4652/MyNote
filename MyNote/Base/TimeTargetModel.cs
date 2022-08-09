using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyNote.Base
{
    public class TimeTargetModel : INotifyPropertyChanged
    {
        #region Static
        public static Dictionary<TimeTargetState, Uri> TimeTargetStateColorse = new Dictionary<TimeTargetState, Uri>();

        static TimeTargetModel()
        {
            TimeTargetStateColorse.Add(TimeTargetState.Finished, new Uri(Environment.CurrentDirectory + "//Resource" + "//TimeTarget" + "//finishTimeTarget.png", UriKind.RelativeOrAbsolute));
            TimeTargetStateColorse.Add(TimeTargetState.One, new Uri(Environment.CurrentDirectory + "//Resource" + "//TimeTarget" + "//blueTimeTarget.png", UriKind.RelativeOrAbsolute));
            TimeTargetStateColorse.Add(TimeTargetState.Two, new Uri(Environment.CurrentDirectory + "//Resource" + "//TimeTarget" + "//yellowTimeTarget.png", UriKind.RelativeOrAbsolute));
            TimeTargetStateColorse.Add(TimeTargetState.Three, new Uri(Environment.CurrentDirectory + "//Resource" + "//TimeTarget" + "//orangeTimeTarget.png", UriKind.RelativeOrAbsolute));
            TimeTargetStateColorse.Add(TimeTargetState.Four, new Uri(Environment.CurrentDirectory + "//Resource" + "//TimeTarget" + "//redTimeTarget.png", UriKind.RelativeOrAbsolute));
            TimeTargetStateColorse.Add(TimeTargetState.Postpone, new Uri(Environment.CurrentDirectory + "//Resource" + "//TimeTarget" + "//redTimeTarget.png", UriKind.RelativeOrAbsolute));
        }

        #endregion

        void Init(double workingTimeMin, DateTime? createTime)
        {
            if (createTime == null)
            {
                createTime = DateTime.Now;
            }
            CreateTime = UpdateTime = (DateTime)createTime;
            ExpectedTime = CreateTime.AddMinutes(workingTimeMin);
        }

        #region INotifyPropertyChanged

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Checks if a property already matches a desired value. Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Checks if a property already matches a desired value. Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <param name="onChanged">Action that is called after the property value has been changed.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="args">The PropertyChangedEventArgs</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }



        #endregion

        #region Properties

        DateTime _CreateTime;
        public DateTime CreateTime
        {
            get
            {
                return _CreateTime;
            }
            set
            {
                SetProperty(ref _CreateTime, value);
            }
        }

        DateTime _UpdateTime;
        public DateTime UpdateTime
        {
            get
            {
                return _UpdateTime;
            }
            set
            {
                SetProperty(ref _UpdateTime, value);
            }
        }

        DateTime _ExpectedTime;
        public DateTime ExpectedTime
        {
            get
            {
                return _ExpectedTime;
            }
            set
            {
                SetProperty(ref _ExpectedTime, value);
            }
        }

        string _TargetDescription = string.Empty;
        public string TargetDescription
        {
            get
            {
                return _TargetDescription;
            }
            set
            {
                SetProperty(ref _TargetDescription, value);
            }
        }

        #endregion
    }

    public enum TimeTargetState
    {
        //完成
        Finished,
        One,
        Two,
        Three,
        Four,
        //推迟，需要重新设定
        Postpone
    }
}
