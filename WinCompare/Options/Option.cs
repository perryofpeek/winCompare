using System;

namespace NDesk.Options
{   
    public abstract class Option {
        string prototype, description;
        string[] names;
        OptionValueType type;

        public Option (string prototype, string description)
        {
            if (prototype == null)
                throw new ArgumentNullException ("prototype");
            if (prototype.Length == 0)
                throw new ArgumentException ("Cannot be the empty string.", "prototype");

            this.prototype   = prototype;
            this.names       = prototype.Split ('|');
            this.description = description;
            this.type        = ValidateNames ();
        }

        public string Prototype { get { return prototype; } }
        public string Description { get { return description; } }
        public OptionValueType OptionValueType { get { return type; } }

        public string[] GetNames ()
        {
            return (string[]) names.Clone ();
        }

        internal string[] Names { get { return names; } }

        static readonly char[] NameTerminator = new char[]{'=', ':'};
        private OptionValueType ValidateNames ()
        {
            char type = '\0';
            for (int i = 0; i < names.Length; ++i) {
                string name = names [i];
                if (name.Length == 0)
                    throw new ArgumentException ("Empty option names are not supported.", "prototype");

                int end = name.IndexOfAny (NameTerminator);
                if (end > 0) {
                    names [i] = name.Substring (0, end);
                    if (type == '\0' || type == name [end])
                        type = name [end];
                    else 
                        throw new ArgumentException (
                            string.Format ("Conflicting option types: '{0}' vs. '{1}'.", type, name [end]),
                            "prototype");
                }
            }
            if (type == '\0')
                return OptionValueType.None;
            return type == '=' ? OptionValueType.Required : OptionValueType.Optional;
        }

        public void Invoke (OptionContext c)
        {
            OnParseComplete (c);
            c.OptionName  = null;
            c.OptionValue = null;
            c.Option      = null;
        }

        protected abstract void OnParseComplete (OptionContext c);

        public override string ToString ()
        {
            return Prototype;
        }
    }
}