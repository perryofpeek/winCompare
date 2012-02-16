using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace NDesk.Options
{
    public class OptionSet : Collection<Option>
    {
        public OptionSet ()
            : this (f => f)
        {
        }

        public OptionSet (Converter<string, string> localizer)
        {
            this.localizer = localizer;
        }

        Dictionary<string, Option> options = new Dictionary<string, Option> ();
        Converter<string, string> localizer;

        protected Option GetOptionForName (string option)
        {
            if (option == null)
                throw new ArgumentNullException ("option");
            Option v;
            if (options.TryGetValue (option, out v))
                return v;
            return null;
        }

        protected override void ClearItems ()
        {
            this.options.Clear ();
        }

        protected override void InsertItem (int index, Option item)
        {
            Add (item);
            base.InsertItem (index, item);
        }

        protected override void RemoveItem (int index)
        {
            Option p = Items [index];
            foreach (string name in p.Names) {
                this.options.Remove (name);
            }
            base.RemoveItem (index);
        }

        protected override void SetItem (int index, Option item)
        {
            RemoveItem (index);
            Add (item);
            base.SetItem (index, item);
        }

        class ActionOption : Option {
            Action<string, OptionContext> action;

            public ActionOption (string prototype, string description, Action<string, OptionContext> action)
                : base (prototype, description)
            {
                if (action == null)
                    throw new ArgumentNullException ("action");
                this.action = action;
            }

            protected override void OnParseComplete (OptionContext c)
            {
                action (c.OptionValue, c);
            }
        }

        public new OptionSet Add (Option option)
        {
            if (option == null)
                throw new ArgumentNullException ("option");
            List<string> added = new List<string> ();
            try {
                foreach (string name in option.Names) {
                    this.options.Add (name, option);
                }
            }
            catch (Exception e) {
                foreach (string name in added)
                    this.options.Remove (name);
                throw;
            }
            return this;
        }

        public OptionSet Add (string options, Action<string> action)
        {
            return Add (options, null, action);
        }

        public OptionSet Add (string options, Action<string, OptionContext> action)
        {
            return Add (options, null, action);
        }

        public OptionSet Add (string options, string description, Action<string> action)
        {
            if (action == null)
                throw new ArgumentNullException ("action");
            return Add (options, description, (v,c) => {action (v);});
        }

        public OptionSet Add (string options, string description, Action<string, OptionContext> action)
        {
            Option p = new ActionOption (options, description, action);
            base.Add (p);
            return this;
        }

        public OptionSet Add<T> (string options, Action<T> action)
        {
            return Add (options, null, action);
        }

        public OptionSet Add<T> (string options, Action<T, OptionContext> action)
        {
            return Add (options, null, action);
        }

        public OptionSet Add<T> (string options, string description, Action<T> action)
        {
            return Add (options, description, (T v, OptionContext c) => {action (v);});
        }

        public OptionSet Add<T> (string options, string description, Action<T, OptionContext> action)
        {
            TypeConverter conv = TypeDescriptor.GetConverter (typeof(T));
            Action<string, OptionContext> a = delegate (string s, OptionContext c) {
                                                                                       T t = default(T);
                                                                                       try {
                                                                                           if (s != null)
                                                                                               t = (T) conv.ConvertFromString (s);
                                                                                       }
                                                                                       catch (Exception e) {
                                                                                           throw new OptionException (
                                                                                               string.Format (
                                                                                                   localizer ("Could not convert string `{0}' to type {1} for option `{2}'."),
                                                                                                   s, typeof(T).Name, c.OptionName),
                                                                                               c.OptionName, e);
                                                                                       }
                                                                                       action (t, c);
            };
            return Add (options, description, a);
        }

        protected virtual OptionContext CreateOptionContext ()
        {
            return new OptionContext ();
        }

#if LINQ
		public List<string> Parse (IEnumerable<string> options)
		{
			bool process = true;
			OptionContext c = CreateOptionContext ();
			c.OptionIndex = -1;
			var unprocessed = 
				from option in options
				where ++c.OptionIndex >= 0 && process 
					? option == "--" 
						? (process = false)
						: !Parse (option, c)
					: true
				select option;
			List<string> r = unprocessed.ToList ();
			if (c.Option != null)
				NoValue (c);
			return r;
		}
#else
        public List<string> Parse (IEnumerable<string> options)
        {
            OptionContext c = CreateOptionContext ();
            c.OptionIndex = -1;
            bool process = true;
            List<string> unprocessed = new List<string> ();
            foreach (string option in options) {
                ++c.OptionIndex;
                if (option == "--") {
                    process = false;
                    continue;
                }
                if (!process) {
                    unprocessed.Add (option);
                    continue;
                }
                if (!Parse (option, c))
                    unprocessed.Add (option);
            }
            if (c.Option != null)
                NoValue (c);
            return unprocessed;
        }
#endif

        private readonly Regex ValueOption = new Regex (
            @"^(?<flag>--|-|/)(?<name>[^:=]+)([:=](?<value>.*))?$");

        protected bool GetOptionParts (string option, out string flag, out string name, out string value)
        {
            Match m = ValueOption.Match (option);
            if (!m.Success) {
                flag = name = value = null;
                return false;
            }
            flag  = m.Groups ["flag"].Value;
            name  = m.Groups ["name"].Value;
            value = !m.Groups ["value"].Success ? null : m.Groups ["value"].Value;
            return true;
        }

        protected virtual bool Parse (string option, OptionContext c)
        {
            if (c.Option != null) {
                c.OptionValue = option;
                c.Option.Invoke (c);
                return true;
            }

            string f, n, v;
            if (!GetOptionParts (option, out f, out n, out v))
                return false;

            Option p;
            if (this.options.TryGetValue (n, out p)) {
                c.OptionName = f + n;
                c.Option     = p;
                switch (p.OptionValueType) {
                    case OptionValueType.None:
                        c.OptionValue = n;
                        c.Option.Invoke (c);
                        break;
                    case OptionValueType.Optional:
                    case OptionValueType.Required: 
                        if (v != null) {
                            c.OptionValue = v;
                            c.Option.Invoke (c);
                        }
                        break;
                }
                return true;
            }
            // no match; is it a bool option?
            if (ParseBool (option, n, c))
                return true;
            // is it a bundled option?
            if (ParseBundled (f, n, c))
                return true;

            return false;
        }

        private bool ParseBool (string option, string n, OptionContext c)
        {
            Option p;
            if (n.Length >= 1 && (n [n.Length-1] == '+' || n [n.Length-1] == '-') &&
                this.options.TryGetValue (n.Substring (0, n.Length-1), out p)) {
                    string v = n [n.Length-1] == '+' ? option : null;
                    c.OptionName  = option;
                    c.OptionValue = v;
                    c.Option      = p;
                    p.Invoke (c);
                    return true;
                }
            return false;
        }

        private bool ParseBundled (string f, string n, OptionContext c)
        {
            Option p;
            if (f == "-" && this.options.TryGetValue (n [0].ToString (), out p)) {
                int i = 0;
                do {
                    string opt = "-" + n [i].ToString ();
                    if (p.OptionValueType != OptionValueType.None) {
                        throw new OptionException (string.Format (
                                                       localizer ("Cannot bundle option '{0}' that requires a value."), opt),
                                                   opt);
                    }
                    c.OptionName  = opt;
                    c.OptionValue = n;
                    c.Option      = p;
                    p.Invoke (c);
                } while (++i < n.Length && this.options.TryGetValue (n [i].ToString (), out p));
                return true;
            }
            return false;
        }

        private void NoValue (OptionContext c)
        {
            c.OptionValue = null;
            Option p = c.Option;
            if (p != null && p.OptionValueType == OptionValueType.Optional) {
                p.Invoke (c);
            }
            else if (p != null && p.OptionValueType == OptionValueType.Required) {
                throw new OptionException (string.Format (
                                               localizer ("Missing required value for option '{0}'."), c.OptionName), 
                                           c.OptionName);
            }
        }

        private const int OptionWidth = 29;

        public void WriteOptionDescriptions (TextWriter o)
        {
            foreach (Option p in this) {
                List<string> names = new List<string> (p.Names);

                int written = 0;
                if (names [0].Length == 1) {
                    Write (o, ref written, "  -");
                    Write (o, ref written, names [0]);
                }
                else {
                    Write (o, ref written, "      --");
                    Write (o, ref written, names [0]);
                }

                for (int i = 1; i < names.Count; ++i) {
                    Write (o, ref written, ", ");
                    Write (o, ref written, names [i].Length == 1 ? "-" : "--");
                    Write (o, ref written, names [i]);
                }

                if (p.OptionValueType == OptionValueType.Optional)
                    Write (o, ref written, localizer ("[=VALUE]"));
                else if (p.OptionValueType == OptionValueType.Required)
                    Write (o, ref written, localizer ("=VALUE"));

                if (written < OptionWidth)
                    o.Write (new string (' ', OptionWidth - written));
                else {
                    o.WriteLine ();
                    o.Write (new string (' ', OptionWidth));
                }

                o.WriteLine (localizer (p.Description));
            }
        }

        static void Write (TextWriter o, ref int n, string s)
        {
            n += s.Length;
            o.Write (s);
        }
    }
}