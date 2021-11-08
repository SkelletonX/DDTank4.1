using System;

namespace SqlDataProvider.Data
{
    public class UserGemStone : DataObject
    {
        private int int_0;

        private int int_1;

        private int int_2;

        private string string_0;

        private int int_3;

        public int EquipPlace
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

        public int FigSpiritId
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

        public string FigSpiritIdValue
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
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

        public UserGemStone()
        {


        }
    }
}