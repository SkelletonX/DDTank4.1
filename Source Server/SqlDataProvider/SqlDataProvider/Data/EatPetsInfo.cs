using System;

namespace SqlDataProvider.Data
{
    public class EatPetsInfo : DataObject
    {
        private int int_0;

        private int int_1;

        private int int_2;

        private int int_3;

        private int int_4;

        private int int_5;

        private int int_6;

        private int int_7;

        public int clothesExp
        {
            get
            {
                return this.int_4;
            }
            set
            {
                this.int_4 = value;
                this._isDirty = true;
            }
        }

        public int clothesLevel
        {
            get
            {
                return this.int_5;
            }
            set
            {
                this.int_5 = value;
                this._isDirty = true;
            }
        }

        public int hatExp
        {
            get
            {
                return this.int_6;
            }
            set
            {
                this.int_6 = value;
                this._isDirty = true;
            }
        }

        public int hatLevel
        {
            get
            {
                return this.int_7;
            }
            set
            {
                this.int_7 = value;
                this._isDirty = true;
            }
        }

        public int ID
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
                this._isDirty = true;
            }
        }

        public int UserID
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
                this._isDirty = true;
            }
        }

        public int weaponExp
        {
            get
            {
                return this.int_2;
            }
            set
            {
                this.int_2 = value;
                this._isDirty = true;
            }
        }

        public int weaponLevel
        {
            get
            {
                return this.int_3;
            }
            set
            {
                this.int_3 = value;
                this._isDirty = true;
            }
        }

        public EatPetsInfo()
        {


        }
    }
}