﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections;
using Signum.Utilities.Reflection;
using Signum.Entities;
using Signum.Utilities;

namespace Signum.Windows
{
    public class EntityListBase : EntityBase
    {
        public static readonly DependencyProperty EntitiesProperty =
          DependencyProperty.Register("Entities", typeof(IList), typeof(EntityListBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (d, e) => ((EntityListBase)d).EntitiesChanged(e)));
        public IList Entities
        {
            get { return (IList)GetValue(EntitiesProperty); }
            set { SetValue(EntitiesProperty, value); }
        }

        public static readonly DependencyProperty EntitiesTypeProperty =
          DependencyProperty.Register("EntitiesType", typeof(Type), typeof(EntityListBase), new UIPropertyMetadata(null, (d, e) => ((EntityListBase)d).EntitiesTypeChanged((Type)e.NewValue)));
        public Type EntitiesType
        {
            get { return (Type)GetValue(EntitiesTypeProperty); }
            set { SetValue(EntitiesTypeProperty, value); }
        }

        public static readonly DependencyProperty MoveProperty =
            DependencyProperty.Register("Move", typeof(bool), typeof(EntityBase), new FrameworkPropertyMetadata(false, (d, e) => ((EntityListBase)d).UpdateVisibility()));
        public bool Move
        {
            get { return (bool)GetValue(MoveProperty); }
            set { SetValue(MoveProperty, value); }
        }

        private void EntitiesTypeChanged(Type type)
        {
 	        Type = type.ElementType().ThrowIfNull("EntitiesType must be a collection type");
        }

        public new event Func<object> Finding;

        static EntityListBase()
        {
            Common.ValuePropertySelector.SetDefinition(typeof(EntityListBase), EntitiesProperty);
            Common.TypePropertySelector.SetDefinition(typeof(EntityListBase), EntitiesTypeProperty);
        }


        protected override bool CanFind()
        {
            return Find && !Common.GetIsReadOnly(this);
        }

        protected override bool CanCreate()
        {
            return Create && !Common.GetIsReadOnly(this);
        }

        protected virtual bool CanMove()
        {
            return Move && !Common.GetIsReadOnly(this);
        }

        protected new object OnFinding()
        {
            if (!CanFind())
                return null;

            object value;
            if (Finding == null)
            {
                Type type = SelectType(Navigator.IsFindable);
                if (type == null)
                    return null;

                value = Navigator.FindMany(new FindManyOptions { QueryName = type });
            }
            else
                value = Finding();

            if (value == null)
                return null;

            if (value is IEnumerable)
                return ((IEnumerable)value).Cast<object>().Select(o => Server.Convert(o, Type)).ToArray();
            else
                return Server.Convert(value, Type);
        }

        public override PropertyRoute GetEntityPropertyRoute()
        {
            PropertyRoute tc = base.GetEntityPropertyRoute();
            if (tc == null)
                return null;

            return tc.Add("Item");
        }

        public IList EnsureEntities()
        {
            if (Entities == null)
                Entities = (IList)Activator.CreateInstance(EntitiesType);
            return Entities;
        }

        public virtual void EntitiesChanged(DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
